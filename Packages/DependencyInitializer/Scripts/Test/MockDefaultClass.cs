using System;
using UnityEngine;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Test
{
    [Serializable]
    public class MockDefaultClass
    {
        [field: SerializeField, FindInChild]
        public BoxCollider TargetBoxCollider { get; private set; } = null;
        [field: SerializeField, FindInScene]
        public Light FindInScene { get; private set; } = null;
        [field: SerializeField, FindInChild(Path = "Plane/InnerChild")]
        public Renderer FindInChildPath { get; private set; } = null;
        [field: SerializeField, Initializable]
        public DeepMockDefaultClass DeepInitializable { get; private set; } = null;
    }
}
