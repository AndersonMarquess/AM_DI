using UnityEditor;
using UnityEngine;
using AM_DI.Scripts.Util;

namespace AM_DI.Scripts
{
    public class BaseTransformDataEditor
    {
        private const string LABEL_TITLE = "Dependency Initializer";
        private const string FIND_BTN = "Find";
        private const string CLEAR_BTN = "Clear";

        public static void DrawSharedTransformEditor(SerializedObject serializedObject, Transform targetClass)
        {
            serializedObject.Update();
            EditorGUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(LABEL_TITLE);

            if (GUILayout.Button(FIND_BTN))
            {
                Component[] transformComponents = targetClass.GetComponents(typeof(Component));
                for (int i = 0; i < transformComponents.Length; i++)
                {
                    DependencyInitializer.InitializeComponents(transformComponents[i]);
                }
            }

            if (GUILayout.Button(CLEAR_BTN))
            {
                Component[] transformComponents = targetClass.GetComponents(typeof(Component));
                for (int i = 0; i < transformComponents.Length; i++)
                {
                    DependencyInitializer.ClearComponents(transformComponents[i]);
                }
            }

            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
