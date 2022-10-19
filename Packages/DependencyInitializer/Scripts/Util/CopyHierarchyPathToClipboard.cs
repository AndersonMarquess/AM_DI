using UnityEngine;
using UnityEditor;

namespace AM_DI.Scripts.Util
{
    /// <summary>
    /// <see href="https://docs.unity3d.com/ScriptReference/MenuItem.html">Menu Item Docs</see><br/>
    /// <see href="https://forum.unity.com/threads/simplest-way-to-add-an-option-to-right-click-menu.424987/">Unity Forum</see>
    /// </summary>
    public class CopyHierarchyPathToClipboard
    {
        private const string COPY_PATH = "GameObject/Copy Path";

        [MenuItem(COPY_PATH, true)]
        private static bool ValidateCopyPathTransform()
        {
            return Selection.activeTransform != null;
        }

        [MenuItem(COPY_PATH, false, 1)]
        private static void CopyHierarchyPath(MenuCommand _)
        {
            Transform targetTransform = Selection.activeTransform;
            string path = GetParentPath(targetTransform, targetTransform.name);
            EditorGUIUtility.systemCopyBuffer = path;
        }

        private static string GetParentPath(Transform transform, string path)
        {
            if (transform.parent == null)
            {
                return path;
            }
            return GetParentPath(transform.parent, $"{transform.parent.name}/{path}");
        }
    }
}
