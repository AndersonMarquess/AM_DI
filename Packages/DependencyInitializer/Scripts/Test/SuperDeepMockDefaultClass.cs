using System;
using UnityEngine;
using AM_DI.Scripts.Attributes;
using UnityEngine.AI;

namespace AM_DI.Scripts.Test
{
    [Serializable]
    public class SuperDeepMockDefaultClass
    {
        [field: SerializeField, FindInScene]
        public NavMeshAgent FindInComponent { get; private set; }
    }
}
