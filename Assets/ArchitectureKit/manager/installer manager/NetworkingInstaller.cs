using UnityEngine;
using PurrNet;

namespace Namespace_Networking
{
    public readonly struct RequestCreateRoom : IEvent { }
    public readonly struct RequestJoinRoom : IEvent { }
    public readonly struct RequestLeaveRoom : IEvent { }

    public readonly struct RoomCreated : IEvent { }
    public readonly struct JoinedRoom : IEvent { }
    public readonly struct LeftRoom : IEvent { }


    internal class NetworkingInstaller : INetworkingInstaller
    {
        private readonly GameObject _networkPrefab;

        public NetworkingInstaller(GameObject networkPrefab)
        {
            _networkPrefab = networkPrefab;
        }

        public void Install(IBootstrapContext ctx)
        {
            var bus = ctx.IGetBus;

            INetworkManager networkManager = Object.FindFirstObjectByType<MonoBehaviour>() as INetworkManager;

            if (networkManager == null)
            {
                var go = Object.Instantiate(_networkPrefab);
                Object.DontDestroyOnLoad(go);
                networkManager = go.GetComponent<INetworkManager>();
            }

            ctx.IRegister<INetworkManager>(networkManager);

            bus.ISubscribe<RequestCreateRoom>(_ => networkManager.StartServer());
            bus.ISubscribe<RequestJoinRoom>(_ => networkManager.StartClient());
            bus.ISubscribe<RequestLeaveRoom>(_ =>
            {
                if (networkManager.isServer) networkManager.StopServer();
                if (networkManager.isClient) networkManager.StopClient();
            });
        }
    }
}
