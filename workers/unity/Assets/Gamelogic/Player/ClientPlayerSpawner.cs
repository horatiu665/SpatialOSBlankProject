namespace Assets.Gamelogic
{
    using System;
    using Improbable;
    using Improbable.Player;
    using Improbable.Unity.Core;
    using Improbable.Unity.Core.EntityQueries;
    using UnityEngine;

    public static class ClientPlayerSpawner
    {
        private static EntityId _simulationManagerEntityId = EntityId.InvalidEntityId;

        public static void SpawnPlayer()
        {
            FindSimulationManagerEntityId(RequestPlayerSpawn);
        }

        public static void DeletePlayer()
        {
            if (EntityId.IsValidEntityId(_simulationManagerEntityId))
            {
                SpatialOS.Connection.SendCommandRequest(_simulationManagerEntityId, new PlayerComponent.Commands.DeletePlayer.Request(new DeletePlayerRequest()), null);
            }
        }

        private static void FindSimulationManagerEntityId(Action<EntityId> callback)
        {
            if (EntityId.IsValidEntityId(_simulationManagerEntityId))
            {
                callback(_simulationManagerEntityId);
                return;
            }

            var query = Query.HasComponent<PlayerComponent>().ReturnOnlyEntityIds();
            SpatialOS.WorkerCommands.SendQuery(query, response => OnSearchResult(callback, response));
        }

        private static void OnSearchResult(Action<EntityId> callback, ICommandCallbackResponse<EntityQueryResult> response)
        {
            if (!response.Response.HasValue || response.StatusCode != Improbable.Worker.StatusCode.Success)
            {
                Debug.LogError("Find player spawner query failed with error: " + response.ErrorMessage);
                return;
            }

            var result = response.Response.Value;
            if (result.EntityCount <= 0)
            {
                Debug.LogError("Failed to find player spawner: no entities found with the PlayerSpawner component.");
                return;
            }

            _simulationManagerEntityId = result.Entities.First.Value.Key;
            callback(_simulationManagerEntityId);
        }

        private static void RequestPlayerSpawn(EntityId simulationManagerEntityId)
        {
            SpatialOS.WorkerCommands.SendCommand(PlayerComponent.Commands.SpawnPlayer.Descriptor, new SpawnPlayerRequest(), simulationManagerEntityId, response => OnSpawnPlayerResponse(simulationManagerEntityId, response));
        }

        private static void OnSpawnPlayerResponse(EntityId simulationManagerEntityId, ICommandCallbackResponse<SpawnPlayerResponse> response)
        {
            if (!response.Response.HasValue || response.StatusCode != Improbable.Worker.StatusCode.Success)
            {
                Debug.LogError("SpawnPlayer command failed: " + response.ErrorMessage);
                RequestPlayerSpawn(simulationManagerEntityId);
                return;
            }
        }
    }
}