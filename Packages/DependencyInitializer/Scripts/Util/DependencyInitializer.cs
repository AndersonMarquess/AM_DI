using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Util
{
    public class DependencyInitializer
    {
        public static void InitializeComponents(Component searchSource, object destination = null)
        {
            destination ??= searchSource;
            Type currentType = destination.GetType();
            List<FieldInfo> fields = GetFieldsWithInheritance(currentType);

            foreach (FieldInfo fieldInfo in fields)
            {
                if (HasFindInChildAttr(fieldInfo, searchSource, destination))
                {
                    continue;
                }

                if (HasFindInComponentAttr(fieldInfo, searchSource, destination))
                {
                    continue;
                }

                if (HasFindInParentAttr(fieldInfo, searchSource, destination))
                {
                    continue;
                }

                if (HasFindInSceneAttr(fieldInfo, searchSource, destination))
                {
                    continue;
                }

                if (HandleInitializableAttr(fieldInfo, searchSource, destination))
                {
                    continue;
                }
            }
            SetComponentDirty(searchSource);
        }

        private static List<FieldInfo> GetFieldsWithInheritance(Type currentType)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            List<FieldInfo> fields = new List<FieldInfo>();

            while (currentType != null)
            {
                if (currentType == typeof(UnityEngine.MonoBehaviour) || currentType == typeof(UnityEngine.Component) || currentType == typeof(System.Object))
                {
                    break;
                }

                fields.AddRange(currentType.GetFields(flags));
                currentType = currentType.BaseType;
            }
            return fields;
        }

        private static bool HasFindInChildAttr(FieldInfo fieldInfo, Component searchSource, object destination)
        {
            FindInChildAttribute childAttr = fieldInfo.GetCustomAttribute<FindInChildAttribute>();
            if (childAttr != null)
            {
                Component result = null;
                if (string.IsNullOrEmpty(childAttr.Path))
                {
                    result = searchSource.transform.GetComponentInChildren(fieldInfo.FieldType, childAttr.OnlyActive == false);
                }
                else
                {
                    string pathToSearch = FilterComponentPath(searchSource.transform.name, childAttr.Path);
                    Transform pathTransform = searchSource.transform.Find(pathToSearch);
                    if (pathTransform != null)
                    {
                        Component[] targets = pathTransform.GetComponents(fieldInfo.FieldType);
                        for (int i = 0; i < targets.Length; i++)
                        {
                            if (childAttr.OnlyActive == false || targets[i].gameObject.activeInHierarchy)
                            {
                                result = targets[i];
                                break;
                            }
                        }
                    }
                }

#if UNITY_EDITOR
                if (result == null)
                {
                    Debug.LogWarningFormat("Can't find component in children. Field: <color=red>{0}</color> Path: {1}", fieldInfo.Name, childAttr.Path);
                }
#endif
                fieldInfo.SetValue(destination, result);

                return true;
            }
            return false;
        }

        private static string FilterComponentPath(string transformName, string pathToSearch)
        {
            int transformNameWithDirectoryLenght = transformName.Length + 1;
            if (pathToSearch.StartsWith(transformName) && pathToSearch.Length > transformNameWithDirectoryLenght)
            {
                pathToSearch = pathToSearch.Substring(transformNameWithDirectoryLenght);
            }
            return pathToSearch;
        }

        private static bool HasFindInComponentAttr(FieldInfo fieldInfo, Component searchSource, object destination)
        {
            FindInComponentAttribute componentAttr = fieldInfo.GetCustomAttribute<FindInComponentAttribute>();
            if (componentAttr != null)
            {
                searchSource.TryGetComponent(fieldInfo.FieldType, out Component target);
                fieldInfo.SetValue(destination, target);
                return true;
            }
            return false;
        }

        private static bool HasFindInParentAttr(FieldInfo fieldInfo, Component searchSource, object destination)
        {
            FindInParentAttribute parentAttr = fieldInfo.GetCustomAttribute<FindInParentAttribute>();
            if (parentAttr != null)
            {
                Transform ownerTransform = parentAttr.IgnoreSelf ? searchSource.transform.parent : searchSource.transform;
                Component target = ownerTransform.GetComponent(fieldInfo.FieldType);
                fieldInfo.SetValue(destination, target);
                return true;
            }
            return false;
        }

        private static bool HasFindInSceneAttr(FieldInfo fieldInfo, Component searchSource, object destination)
        {
            FindInSceneAttribute sceneAttr = fieldInfo.GetCustomAttribute<FindInSceneAttribute>();
            if (sceneAttr != null)
            {
                UnityEngine.Object target = UnityEngine.Object.FindObjectOfType(fieldInfo.FieldType, sceneAttr.ActiveOnly == false);
                fieldInfo.SetValue(destination, target);
                return true;
            }
            return false;
        }

        private static bool HandleInitializableAttr(FieldInfo fieldInfo, Component searchSource, object destioantion)
        {
            InitializableAttribute initializableAttr = fieldInfo.GetCustomAttribute<InitializableAttribute>();
            if (initializableAttr != null)
            {
                InitializeComponents(searchSource, fieldInfo.GetValue(destioantion));
                return true;
            }
            return false;
        }

        public static void ClearComponents(Component owner)
        {
            List<FieldInfo> fields = GetFieldsWithInheritance(owner.GetType());

            foreach (FieldInfo field in fields)
            {
                FindInChildAttribute childAttr = field.GetCustomAttribute<FindInChildAttribute>();
                FindInComponentAttribute componentAttr = field.GetCustomAttribute<FindInComponentAttribute>();
                FindInSceneAttribute sceneAttr = field.GetCustomAttribute<FindInSceneAttribute>();
                FindInParentAttribute parentAttr = field.GetCustomAttribute<FindInParentAttribute>();
                InitializableAttribute initializableAttr = field.GetCustomAttribute<InitializableAttribute>();
                if (childAttr != null || componentAttr != null || sceneAttr != null || parentAttr != null || initializableAttr != null)
                {
                    field.SetValue(owner, null);
                    continue;
                }
            }
            SetComponentDirty(owner);
        }

        /// <summary>
        /// From: https://stackoverflow.com/questions/46157490/notify-unity-about-modified-values-in-the-inspector
        /// https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html
        /// </summary>
        private static void SetComponentDirty(Component owner)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(owner, "Dependency Initializer");
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(owner);
            UnityEditor.EditorUtility.SetDirty(owner);
#endif
        }
    }
}
