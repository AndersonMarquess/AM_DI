using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using AM_DI.Scripts.Attributes;

namespace AM_DI.Scripts.Test
{
    public class MockComponent : MonoBehaviour
    {
        [SerializeField]
        private int _normalField = 42;

        [Header("Component")]
        [SerializeField, FindInComponent]
        private NavMeshAgent _findInComponent = null;

        [Header("Child")]
        [SerializeField, FindInChild]
        private Renderer _findInChild = null;

        [SerializeField, FindInChild(OnlyActive = true)]
        private Collider _findInActiveChild;

        [SerializeField, FindInChild(Path = "Plane/InnerChild")]
        private Renderer _findInChildPath = null;

        [SerializeField, FindInChild(Path = "InvalidPath")]
        private Renderer _findInChildInvalidPath = null;

        [SerializeField, FindInChild]
        private Canvas _canvas = null;

        [Header("Scene")]
        [SerializeField, FindInScene]
        private Light _findInScene = null;
        [SerializeField, FindInScene(ActiveOnly = true)]
        private SphereCollider _findInSceneActive = null;

        [Header("Composition")]
        [SerializeField, Initializable]
        private MockDefaultClass _mockDefaultClass = null;

        public Collider FindInActiveChild { get => _findInActiveChild; set => _findInActiveChild = value; }

        protected virtual void Start()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(_findInChild, "Find in child not working");
            Assert.IsTrue(_normalField == 42, "Default field was modified");
            Assert.IsNotNull(FindInActiveChild, "Find in active child not working");
            Assert.IsNotNull(_findInComponent, "Find in component not working");
            Assert.IsNotNull(_findInChildPath, "Valid path not working");
            Assert.IsNull(_findInChildInvalidPath, "Invalid path not working");
            Assert.IsNotNull(_canvas, "Find in child not working");
            Assert.IsNotNull(_findInScene, "Find in scene not working");
            Assert.IsNotNull(_findInSceneActive, "Find in scene active not working");
#endif
        }
    }
}