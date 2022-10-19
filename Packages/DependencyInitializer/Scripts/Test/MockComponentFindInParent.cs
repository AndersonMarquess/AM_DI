using UnityEngine;
using UnityEngine.Assertions;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Test
{
    public class MockComponentFindInParent : MonoBehaviour
    {
        [SerializeField, FindInParent]
        private MockComponent _parentComponent = null;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(_parentComponent, "Find in parent not working");
#endif
        }
    }
}
