using Assets.Gamelogic;
using Improbable.Math;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;
using UnityEngine;

namespace Assets.EntityTemplates
{
    public class PlayerEntityTemplate : MonoBehaviour
    {
        public static SnapshotEntity GeneratePlayerSnapshotEntityTemplate()
        {
            // Set name of Unity prefab associated with this entity
            var playerEntity = new SnapshotEntity { Prefab = "PlayerPrefab" };

            // Define componetns attached to snapshot entity
            playerEntity.Add(
                new PlayerComponent.Data(
                    new PlayerComponentData(
                        Coordinates.ZERO,
                        playerEntity.DefaultTransformData(),
                        playerEntity.DefaultTransformData(),
                        playerEntity.DefaultTransformData()
                    )
                )
            );

            var acl = Acl.Build()
                // Both FSim (server) workers and client workers granted read access over all states
                .SetReadAccess(CommonPredicates.PhysicsOrVisual)
                // Both FSim (server) workers and client workers (local authority) granted write access over PlayerComponent component
                .SetWriteAccess<PlayerComponent>(CommonPredicates.PhysicsOrVisual);

            playerEntity.SetAcl(acl);

            return playerEntity;
        }
    }
}