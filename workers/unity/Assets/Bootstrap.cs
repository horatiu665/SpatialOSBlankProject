namespace Assets.Gamelogic
{
    using Improbable.Unity;
    using Improbable.Unity.Configuration;
    using Improbable.Unity.Core;
    using UnityEngine;

    // Placed on a gameobject in client scene to execute connection logic on client startup
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private int _targetFramerate = 90;

        [SerializeField]
        private int _fixedFramerate = 90;

        public WorkerConfigurationData Configuration = new WorkerConfigurationData();

        private void OnEnable()
        {
            SpatialOS.ApplyConfiguration(Configuration);

            Application.targetFrameRate = _targetFramerate;
            switch (SpatialOS.Configuration.EnginePlatform)
            {
                case EnginePlatform.FSim:
                {
                    Time.fixedDeltaTime = 1.0f / _fixedFramerate;
                    SpatialOS.OnDisconnected += reason => Application.Quit();
                    break;
                }

                case EnginePlatform.Client:
                {
                    SpatialOS.OnConnected += OnConnected;
                    break;
                }
            }

            SpatialOS.Connect(this.gameObject);
        }

        private void OnConnected()
        {
            Debug.Log("Bootstrap connected to SpatialOS...");
            ClientPlayerSpawner.SpawnPlayer();
        }

        private void OnApplicationQuit()
        {
            if (SpatialOS.IsConnected)
            {
                Debug.Log("Bootstrap disconnecting from SpatialOS.");
                SpatialOS.OnConnected -= OnConnected;
                SpatialOS.Disconnect();
            }
        }
    }
}