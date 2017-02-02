using Assets.Gamelogic;
using Improbable.Math;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;

namespace Assets.EntityTemplates
{
    public class EntityTemplateFactory
    {
        public static SnapshotEntity GeneratePlayerSnapshotEntityTemplate(string workerId)
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

            var specificClientPredicate = CommonPredicates.SpecificClientOnly(workerId);

            var acl = Acl.Build()
                // Both FSim (server) workers and client workers granted read access over all states
                .SetReadAccess(CommonPredicates.PhysicsOrVisual)
                // Only the local client worker (local authority) granted write access over PlayerComponent component
                .SetWriteAccess<PlayerComponent>(specificClientPredicate);

            playerEntity.SetAcl(acl);

            return playerEntity;
        }
    }
}