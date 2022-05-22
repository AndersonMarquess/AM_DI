using UnityEngine;
using UnityEngine.Assertions;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Test
{
    public class InheritedMockComponent : MockComponent
    {
        [Header("Inherited specific")]
        [SerializeField, FindInChild(Path = "Graphics")]
        private Transform _childGraphics = null;

        protected override void Start()
        {
            base.Start();
#if UNITY_EDITOR
            Assert.IsNotNull(_childGraphics, "Inherited specific find in child not working");
#endif
        }
    }
}
