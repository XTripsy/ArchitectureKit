using Namespace_InputLobby_Event;
using Namespace_PlayerController;
using Namespace_PlayerModulMovement;
using Namespace_PlayerService;
using Namespace_StatePause_Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Namespace_PlayerState
{
    internal class PlayerStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            IGameLoopManager gameLoopManager = installer.IResolve<IGameLoopManager>();
            IObjectManager objectManager = installer.IResolve<IObjectManager>();

            IPlayerIdService playerIdService = new PlayerIdService();
            IPlayerSpawnService playerSpawnService = new PlayerSpawnerService(bus, playerIdService, objectManager, gameLoopManager);

            bus.ISubscribe<ActionJoinLobbyState>(_ => playerSpawnService.ISpawnPlayer());
        }
    }
}
