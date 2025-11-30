using Namespace_InputLobby_Event;
using Namespace_PlayerController;
using Namespace_PlayerModulMovement;
using Namespace_StatePause_Event;
using UnityEngine;

namespace Namespace_PlayerState
{
    internal class PlayerStateInstaller : IInstaller<IBootstrapContext>
    {
        public void Install(IBootstrapContext installer)
        {
            IEventBus bus = installer.IGetBus;
            IGameLoopManager gameLoopManager = installer.IResolve<IGameLoopManager>();
            IObjectManager objectManager = installer.IResolve<IObjectManager>();

            IUpdateManager player_state = new PlayerStateManager();
            gameLoopManager.IRegister("player_state_manager", player_state);

            _InstallEvent(bus, gameLoopManager);
            bus.ISubscribe<ActionJoinLobbyState>(_ => _InstallPlayerState(bus, player_state as IPlayerStateManager, objectManager));
        }

        private void _InstallEvent(IEventBus bus, IGameLoopManager gameLoopManager)
        {
            bus.ISubscribe<ActionJoinLobbyState>(_ => gameLoopManager.IActivate("player_state_manager"));
            bus.ISubscribe<PauseStateExit>(_ => gameLoopManager.IActivate("player_state_manager"));
            bus.ISubscribe<PauseStateEnter>(_ => gameLoopManager.IDeActivate("player_state_manager"));
        }

        private void _InstallPlayerState(IEventBus bus, IPlayerStateManager stateManager, IObjectManager objectManager)
        {
            var obj = objectManager.IGet("player").transform;
            PlayerModulMovement modulMovement = new PlayerModulMovement(obj.GetComponent<CharacterController>());

            IPlayerState idle_state = new PlayerStateIdle(modulMovement);
            IPlayerState move_state = new PlayerStateMovement(bus, modulMovement);

            IPlayerStateMachine player_state = new PlayerStateMachine();
            player_state.IRegister("idle", idle_state);
            player_state.IRegister("movement", move_state);

            _InstallPlayerStateMachine(stateManager, "player_state", player_state);

            player_state.IChangeState("idle");
            _InstallPlayerController(0, bus, player_state, obj);
        }

        private void _InstallPlayerController(int id, IEventBus bus, IPlayerStateMachine player_state, Transform obj)
        {
            var player_controller = new PlayerController(id, obj, bus, player_state);
        }

        private void _InstallPlayerStateMachine(IPlayerStateManager stateManager, string name, IPlayerStateMachine playerState)
        {
            stateManager.IAddStateMachine(name, playerState);
        }
    }
}
