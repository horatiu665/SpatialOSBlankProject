namespace Assets.Gamelogic
{
    using Improbable.Player;
    using Improbable.Unity.Visualizer;
    using UnityEngine;

    public sealed class PlayerSyncReceiver : MonoBehaviour
    {
        [SerializeField]
        private Transform _head;

        [SerializeField]
        private Transform _leftHand;

        [SerializeField]
        private Transform _rightHand;

        [Require]
        private PlayerComponent.Reader _playerReader;

        private void OnEnable()
        {
            var data = _playerReader.Data;

            this.transform.position = data.position.ToVector3();
            _head.UpdateFromTransformData(data.head);
            _leftHand.UpdateFromTransformData(data.leftHand);
            _rightHand.UpdateFromTransformData(data.rightHand);

            _playerReader.ComponentUpdated += OnComponentUpdated;
        }

        private void OnDisable()
        {
            _playerReader.ComponentUpdated -= OnComponentUpdated;
        }

        private void OnComponentUpdated(PlayerComponent.Update update)
        {
            if (!_playerReader.HasAuthority)
            {
                var pos = update.position;
                if (pos.HasValue)
                {
                    this.transform.position = pos.Value.ToVector3();
                }

                var head = update.head;
                if (head.HasValue)
                {
                    _head.UpdateFromTransformData(head.Value);
                }

                var lhand = update.leftHand;
                if (lhand.HasValue)
                {
                    _leftHand.UpdateFromTransformData(lhand.Value);
                }

                var rhand = update.rightHand;
                if (rhand.HasValue)
                {
                    _rightHand.UpdateFromTransformData(rhand.Value);
                }
            }
        }
    }
}