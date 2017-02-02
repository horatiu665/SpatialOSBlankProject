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

        [SerializeField]
        private float _moveSpeed = 5f;

        [Header("Debugging")]
        [SerializeField]
        private bool _allowKeyInput;

        [SerializeField]
        private KeyCode _moveForward = KeyCode.W;

        [SerializeField]
        private KeyCode _moveLeft = KeyCode.A;

        [SerializeField]
        private KeyCode _moveRight = KeyCode.D;

        [SerializeField]
        private KeyCode _moveBack = KeyCode.S;

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

        private void Update()
        {
            if (!_allowKeyInput || !_playerWriter.HasAuthority)
            {
                return;
            }

            Vector3 velocity = Vector3.zero;
            if (Input.GetKey(_moveForward))
            {
                velocity = this.transform.forward;
            }
            else if (Input.GetKey(_moveBack))
            {
                velocity = -this.transform.forward;
            }

            if (Input.GetKey(_moveRight))
            {
                velocity += this.transform.right;
            }
            else if (Input.GetKey(_moveLeft))
            {
                velocity += -this.transform.right;
            }

            this.transform.position += velocity.normalized * _moveSpeed * Time.deltaTime;
        }
    }
}