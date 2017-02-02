namespace Assets.Gamelogic
{
    using Improbable.Entity.Component;
    using Improbable.Player;
    using Improbable.Unity.Visualizer;
    using UnityEngine;

    [RequireComponent(typeof(PlayerController))]
    public sealed class PlayerSyncComponent : MonoBehaviour
    {
        [SerializeField]
        private int _sendFrameFrequency = 5;

        [SerializeField]
        private double _positionSendThreshold = 0.5f;

        [SerializeField]
        private double _rotationSendThreshold = 2f;

        //[Require]
        //private PlayerComponent.Writer _playerWriter;

        private PlayerController _player;
        private int _lastSend;

        private void Awake()
        {
            _player = this.GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if (!_player.hasAuthority)
            {
                var data = _player.writer.Data;

                this.transform.position = data.position.ToVector3();
                _player.head.UpdateFromTransformData(data.head);
                _player.leftHand.UpdateFromTransformData(data.leftHand);
                _player.rightHand.UpdateFromTransformData(data.rightHand);

                _player.writer.ComponentUpdated += OnComponentUpdated;
            }

            _player.writer.CommandReceiver.OnSpawnPlayer += OnSpawnPlayer;
            _player.writer.CommandReceiver.OnDeletePlayer += OnDeletePlayer;
        }

        private void OnDeletePlayer(ResponseHandle<PlayerComponent.Commands.DeletePlayer, DeletePlayerRequest, DeletePlayerResponse> obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnSpawnPlayer(ResponseHandle<PlayerComponent.Commands.SpawnPlayer, SpawnPlayerRequest, SpawnPlayerResponse> obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnDisable()
        {
            if (!_player.hasAuthority)
            {
                _player.writer.ComponentUpdated -= OnComponentUpdated;
            }

            _player.writer.CommandReceiver.OnSpawnPlayer -= OnSpawnPlayer;
            _player.writer.CommandReceiver.OnDeletePlayer -= OnDeletePlayer;
        }

        private void FixedUpdate()
        {
            if (!_player.hasAuthority)
            {
                return;
            }

            var frameCount = Time.frameCount;
            if (frameCount < _lastSend)
            {
                return;
            }

            _lastSend = frameCount + _sendFrameFrequency;

            var update = new PlayerComponent.Update();
            var change = false;

            var newPos = _player.transform.position.ToCoordinates();
            if (newPos.HasChangedBy(_player.writer.Data.position, _positionSendThreshold))
            {
                update = update.SetPosition(newPos);
                change = true;
            }

            var newHead = _player.head.ToTransformData();
            if (newHead.HasChangedBy(_player.writer.Data.head, _positionSendThreshold, _rotationSendThreshold))
            {
                update = update.SetHead(newHead);
                change = true;
            }

            var newLeftHand = _player.leftHand.ToTransformData();
            if (newLeftHand.HasChangedBy(_player.writer.Data.leftHand, _positionSendThreshold, _rotationSendThreshold))
            {
                update = update.SetLeftHand(newLeftHand);
                change = true;
            }

            var newRightHand = _player.rightHand.ToTransformData();
            if (newRightHand.HasChangedBy(_player.writer.Data.rightHand, _positionSendThreshold, _rotationSendThreshold))
            {
                update = update.SetRightHand(newRightHand);
                change = true;
            }

            if (change)
            {
                _player.writer.Send(update);
            }
        }

        private void OnComponentUpdated(PlayerComponent.Update update)
        {
            if (_player.hasAuthority)
            {
                return;
            }

            var pos = update.position;
            if (pos.HasValue)
            {
                _player.transform.position = pos.Value.ToVector3();
            }

            var head = update.head;
            if (head.HasValue)
            {
                _player.head.UpdateFromTransformData(head.Value);
            }

            var lhand = update.leftHand;
            if (lhand.HasValue)
            {
                _player.leftHand.UpdateFromTransformData(lhand.Value);
            }

            var rhand = update.rightHand;
            if (rhand.HasValue)
            {
                _player.rightHand.UpdateFromTransformData(rhand.Value);
            }
        }
    }
}