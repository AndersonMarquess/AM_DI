using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using AM_DI.Scripts.Util;

namespace AM_DI.Scripts
{
    /// <summary>
    /// Transform modify source: https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/
    /// </summary>
    [CustomEditor(typeof(Transform), true)]
    public class CustomTransformEditor : Editor
    {
        //Unity's built-in editor
        private Editor _defaultEditor;
        private Transform _targetClass;

        void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector
            _defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
            _targetClass = target as Transform;
        }

        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = _defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
            {
                disableMethod.Invoke(_defaultEditor, null);
            }

            DestroyImmediate(_defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            if (EditorUtility.IsPersistent(_targetClass.gameObject))
            {
                _defaultEditor.OnInspectorGUI();
                return;
            }

            _defaultEditor.OnInspectorGUI();

            EditorGUILayout.Space(5f);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Dependency Initializer");

            if (GUILayout.Button("Find"))
            {
                Component[] transformComponents = _targetClass.GetComponents(typeof(Component));
                for (int i = 0; i < transformComponents.Length; i++)
                {
                    DependencyInitializer.InitializeComponents(transformComponents[i]);
                }
            }

            if (GUILayout.Button("Clear"))
            {
                Component[] transformComponents = _targetClass.GetComponents(typeof(Component));
                for (int i = 0; i < transformComponents.Length; i++)
                {
                    DependencyInitializer.ClearComponents(transformComponents[i]);
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}