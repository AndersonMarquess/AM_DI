using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Util
{
    public class DependencyInitializer
    {
        public static void InitializeComponents(Component owner)
        {
            List<FieldInfo> fields = GetFieldsWithInheritance(owner);

            foreach (FieldInfo fieldInfo in fields)
            {
                if (HasFindInChildAttr(fieldInfo, owner))
                {
                    continue;
                }

                if (HasFindInComponentAttr(fieldInfo, owner))
                {
                    continue;
                }

                if (HasFindInParentAttr(fieldInfo, owner))
                {
                    continue;
                }

                if (HasFindInSceneAttr(fieldInfo, owner))
                {
                    continue;
                }
            }
            SetComponentDirty(owner);
        }

        private static List<FieldInfo> GetFieldsWithInheritance(Component owner)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            Type currentType = owner.GetType();
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

        private static bool HasFindInChildAttr(FieldInfo fieldInfo, Component owner)
        {
            FindInChildAttribute childAttr = fieldInfo.GetCustomAttribute<FindInChildAttribute>();
            if (childAttr != null)
            {
                Component target = null;
                if (string.IsNullOrEmpty(childAttr.Path))
                {
                    target = owner.transform.GetComponentInChildren(fieldInfo.FieldType, childAttr.OnlyActive == false);
                }
                else
                {
                    string pathToSearch = FilterComponentPath(owner.transform.name, childAttr.Path);
                    Transform pathTransform = owner.transform.Find(pathToSearch);
                    if (pathTransform != null)
                    {
                        Component[] targets = pathTransform.GetComponents(fieldInfo.FieldType);
                        for (int i = 0; i < targets.Length; i++)
                        {
                            if (childAttr.OnlyActive == false || targets[i].gameObject.activeInHierarchy)
                            {
                                target = targets[i];
                                break;
                            }
                        }
                    }
                }

#if UNITY_EDITOR
                if (target == null)
                {
                    Debug.LogWarningFormat("Can't find component in children. Field: <color=red>{0}</color> Path: {1}", fieldInfo.Name, childAttr.Path);
                }
#endif
                fieldInfo.SetValue(owner, target);

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

        private static bool HasFindInComponentAttr(FieldInfo fieldInfo, Component owner)
        {
            FindInComponentAttribute componentAttr = fieldInfo.GetCustomAttribute<FindInComponentAttribute>();
            if (componentAttr != null)
            {
                owner.TryGetComponent(fieldInfo.FieldType, out Component target);
                fieldInfo.SetValue(owner, target);
                return true;
            }
            return false;
        }

        private static bool HasFindInParentAttr(FieldInfo fieldInfo, Component owner)
        {
            FindInParentAttribute parentAttr = fieldInfo.GetCustomAttribute<FindInParentAttribute>();
            if (parentAttr != null)
            {
                Transform ownerTransform = parentAttr.IgnoreSelf ? owner.transform.parent : owner.transform;
                Component target = ownerTransform.GetComponent(fieldInfo.FieldType);
                fieldInfo.SetValue(owner, target);
                return true;
            }
            return false;
        }

        private static bool HasFindInSceneAttr(FieldInfo fieldInfo, Component owner)
        {
            FindInSceneAttribute sceneAttr = fieldInfo.GetCustomAttribute<FindInSceneAttribute>();
            if (sceneAttr != null)
            {
                UnityEngine.Object target = UnityEngine.Object.FindObjectOfType(fieldInfo.FieldType, sceneAttr.ActiveOnly == false);
                fieldInfo.SetValue(owner, target);
                return true;
            }
            return false;
        }

        public static void ClearComponents(Component owner)
        {
            List<FieldInfo> fields = GetFieldsWithInheritance(owner);

            foreach (FieldInfo field in fields)
            {
                FindInChildAttribute childAttr = field.GetCustomAttribute<FindInChildAttribute>();
                FindInComponentAttribute componentAttr = field.GetCustomAttribute<FindInComponentAttribute>();
                FindInSceneAttribute sceneAttr = field.GetCustomAttribute<FindInSceneAttribute>();
                if (childAttr != null || componentAttr != null || sceneAttr != null)
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
