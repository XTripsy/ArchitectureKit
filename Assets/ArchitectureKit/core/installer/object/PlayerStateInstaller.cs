using Namespace_InputLobby_Event;
using Namespace_PlayerService;

namespace Namespace_PlayerState
{
    internal class PlayerStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            IGameLoopManager gameLoopManager = installer.IResolve<IGameLoopManager>();
            IObjectManager objectManager = installer.IResolve<IObjectManager>();
            IPlayerManager playerManager = installer.IResolve<IPlayerManager>();

            IPlayerIdService playerIdService = new PlayerIdService();
            IPlayerSpawnService playerSpawnService = new PlayerSpawnerService(bus, playerIdService, objectManager, gameLoopManager, playerManager);

            bus.ISubscribe<ActionJoinLobbyState>(playerSpawnService.ISpawnPlayer);
        }
    }
}
