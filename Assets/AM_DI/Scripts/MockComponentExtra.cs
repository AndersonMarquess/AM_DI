using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts
{
    public class MockComponentExtra : MonoBehaviour
    {
        [SerializeField, FindInScene]
        private Camera _mainCamera = null;
        [SerializeField, FindInChild]
        private Image _findInChild = null;

        private void Start()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(_mainCamera, "Find in scene on extra component not working");
            Assert.IsNotNull(_findInChild, "Find in child on extra component not working");
#endif
        }
    }
}
