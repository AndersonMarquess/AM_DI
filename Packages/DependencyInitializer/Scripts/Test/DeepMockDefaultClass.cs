using System;
using UnityEngine;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Test
{
    [Serializable]
    public class DeepMockDefaultClass
    {
        [field: SerializeField, FindInScene]
        public Light FindInScene { get; private set; } = null;
        [field: SerializeField, Initializable]
        public SuperDeepMockDefaultClass SuperDeepInitializable { get; private set; } = null;
    }
}
