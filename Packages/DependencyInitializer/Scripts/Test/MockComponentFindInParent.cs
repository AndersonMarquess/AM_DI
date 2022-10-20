using UnityEngine;
using UnityEngine.Assertions;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Test
{
    public class MockComponentFindInParent : MonoBehaviour
    {
        [SerializeField, FindInParent]
        private Transform _parentComponent = null;
        [SerializeField, FindInParent(IgnoreSelf = false)]
        private Transform _firstAvailableTransform = null;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(_parentComponent, "Find in parent not working");
            Assert.IsTrue(_parentComponent != transform, "Find in parent wrong reference");
            Assert.AreEqual(transform, _firstAvailableTransform, "Find in parent without ignore self not working");
#endif
        }
    }
}
