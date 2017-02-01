namespace Assets.Gamelogic
{
    using Improbable.Player;
    using Improbable.Unity.Visualizer;
    using UnityEngine;

    public sealed class PlayerSyncSender : MonoBehaviour
    {
        [SerializeField]
        private int _sendFrameFrequency = 5;

        [SerializeField]
        private double _positionSendThreshold = 0.5f;

        [SerializeField]
        private double _rotationSendThreshold = 2f;

        [Header("References")]
        [SerializeField]
        private Transform _head;

        [SerializeField]
        private Transform _leftHand;

        [SerializeField]
        private Transform _rightHand;

        [Require]
        private PlayerComponent.Writer _playerWriter;

        private int _lastSend;

        private void FixedUpdate()
        {
            var frameCount = Time.frameCount;
            if (frameCount < _lastSend)
            {
                return;
            }

            _lastSend = frameCount + _sendFrameFrequency;

            if (_playerWriter.HasAuthority)
            {
                var update = new PlayerComponent.Update();
                var change = false;

                var newPos = this.transform.position.ToCoordinates();
                if (newPos.HasChangedBy(_playerWriter.Data.position, _positionSendThreshold))
                {
                    update = update.SetPosition(newPos);
                    change = true;
                }

                var newHead = _head.transform.ToTransformData();
                if (newHead.HasChangedBy(_playerWriter.Data.head, _positionSendThreshold, _rotationSendThreshold))
                {
                    update = update.SetHead(newHead);
                    change = true;
                }

                var newLeftHand = _leftHand.transform.ToTransformData();
                if (newLeftHand.HasChangedBy(_playerWriter.Data.leftHand, _positionSendThreshold, _rotationSendThreshold))
                {
                    update = update.SetLeftHand(newLeftHand);
                    change = true;
                }

                var newRightHand = _rightHand.transform.ToTransformData();
                if (newRightHand.HasChangedBy(_playerWriter.Data.rightHand, _positionSendThreshold, _rotationSendThreshold))
                {
                    update = update.SetRightHand(newRightHand);
                    change = true;
                }

                if (change)
                {
                    _playerWriter.Send(update);
                }
            }
        }
    }
}