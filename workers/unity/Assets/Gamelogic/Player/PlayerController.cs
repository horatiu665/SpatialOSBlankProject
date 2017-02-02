namespace Assets.Gamelogic
{
    using Improbable.Player;
    using Improbable.Unity.Core;
    using Improbable.Unity.Visualizer;
    using UnityEngine;

    public sealed class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform _head;

        [SerializeField]
        private Transform _leftHand;

        [SerializeField]
        private Transform _rightHand;

        [Header("Locomotion")]
        [SerializeField]
        private float _moveSpeed = 5f;

        [Require]
        private PlayerComponent.Writer _playerWriter;

        public Transform head
        {
            get { return _head; }
        }

        public Transform leftHand
        {
            get { return _leftHand; }
        }

        public Transform rightHand
        {
            get { return _rightHand; }
        }

        public float moveSpeed
        {
            get { return _moveSpeed; }
        }

        public bool hasAuthority
        {
            get { return _playerWriter.HasAuthority; }
        }

        public PlayerComponent.Writer writer
        {
            get { return _playerWriter; }
        }

        private void OnApplicationQuit()
        {
            if (SpatialOS.IsConnected)
            {
                ClientPlayerSpawner.DeletePlayer();
            }
        }
    }
}