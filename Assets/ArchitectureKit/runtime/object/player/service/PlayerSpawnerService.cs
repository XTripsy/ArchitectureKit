using Namespace_PlayerController;
using Namespace_PlayerModulMovement;
using Namespace_PlayerState;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Rendering.FilterWindow;

public sealed class PlayerSpawnerService : IPlayerSpawnService
{
    private readonly IEventBus _bus;
    private readonly IPlayerIdService _playerIdService;
    private readonly IObjectManager _objectManager;
    private readonly IGameLoopManager _gameLoopManager;
    private IUpdateManager _stateManager;

    public PlayerSpawnerService(IEventBus bus, IPlayerIdService playerIdService,
        IObjectManager objectManager, IGameLoopManager gameLoopManager)
    {
        _bus = bus;
        _playerIdService = playerIdService;
        _objectManager = objectManager;
        _gameLoopManager = gameLoopManager;

        _stateManager = new PlayerStateManager();
        _gameLoopManager.IRegister("player_state_manager", _stateManager);
    }

    public void ISpawnPlayer()
    {
        int id = _playerIdService.INewID();
        var obj = _objectManager.IDuplicateSpawn("player", id).transform;

        IPlayerStateMachine player_state = _InstallPlayerState(obj);

        IPlayerStateManager manager = _stateManager as IPlayerStateManager;
        manager.IAddStateMachine("player_state-"+id, player_state);

        _InstallPlayerController(id, _bus, player_state, obj);
    }

    public void IDespawnPlayer(int id)
    {
        _playerIdService.IRemoveID(id);
        IPlayerStateManager manager = _stateManager as IPlayerStateManager;
        manager.IRemoveStateMachine("player_state-" + id);
    }

    private IPlayerStateMachine _InstallPlayerState(Transform obj)
    {
        PlayerModulMovement modulMovement = new PlayerModulMovement(obj.GetComponent<CharacterController>());

        IPlayerState idle_state = new PlayerStateIdle(modulMovement);
        IPlayerState move_state = new PlayerStateMovement(_bus, modulMovement);

        IPlayerStateMachine player_state = new PlayerStateMachine();
        player_state.IRegister("idle", idle_state);
        player_state.IRegister("movement", move_state);

        player_state.IChangeState("idle");
        return player_state;
    }

    private void _InstallPlayerController(int id, IEventBus bus, IPlayerStateMachine player_state, Transform obj)
    {
        var playerInput = obj.GetComponent<PlayerInput>();
        var player_controller = new PlayerController(id, playerInput, bus, player_state);
    }
}