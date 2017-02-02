namespace Assets.Gamelogic
{
    using Improbable;
    using Improbable.Collections;
    using Improbable.Player;
    using Improbable.Unity.Visualizer;
    using UnityEngine;

    public sealed class PlayerManagementBehaviour : MonoBehaviour
    {
        //[Require]
        //private PlayerComponent.Writer _player;

        //private Map<string, EntityId> playerEntityIds;

        //private void OnEnable()
        //{
        //    _player.CommandReceiver.OnSpawnPlayer += OnSpawnPlayer;
        //    _player.CommandReceiver.OnDeletePlayer += OnDeletePlayer;

        //    playerEntityIds = new Map<string, EntityId>(playerLifeCycle.Data.playerEntityIds);
        //}

        //private void OnDisable()
        //{
        //    _player.CommandReceiver.OnSpawnPlayer -= OnSpawnPlayer;
        //    _player.CommandReceiver.OnDeletePlayer -= OnDeletePlayer;

        //    playerEntityIds = null;
        //}

        //private void SendMapUpdate()
        //{
        //    var update = new PlayerComponent.Update();
        //    update.SetPlayerEntityIds(playerEntityIds);
        //    _player.Send(update);
        //}
    }
}