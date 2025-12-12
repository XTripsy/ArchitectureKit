using Namespace_InputLobby_Event;
using Namespace_PlayerController;
using Namespace_PlayerModulMovement;
using Namespace_PlayerState;
using Namespace_Player;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerSpawnerService : IPlayerSpawnService
{
    private readonly IEventBus _bus;
    private readonly IPlayerIdService _playerIdService;
    private readonly IObjectManager _objectManager;
    private readonly IGameLoopManager _gameLoopManager;
    private readonly IPlayerManager _playerManager;
    private IUpdateManager _stateManager;

    public PlayerSpawnerService(IEventBus bus, IPlayerIdService playerIdService,
        IObjectManager objectManager, IGameLoopManager gameLoopManager, IPlayerManager playerManager)
    {
        _bus = bus;
        _playerIdService = playerIdService;
        _objectManager = objectManager;
        _gameLoopManager = gameLoopManager;
        _playerManager = playerManager;

        _stateManager = new PlayerStateManager();
        _gameLoopManager.IRegister("player_state_manager", _stateManager);
    }

    public void ISpawnPlayer(ActionJoinLobbyState _event)
    {
        if (!_playerManager.IIsCanSpawn(_event.device)) return;

        int id = _playerIdService.INewID();
        var obj = _objectManager.IDuplicateSpawn("player", id).transform;

        IPlayerStateMachine player_state = _InstallPlayerState(obj);

        IPlayerStateManager manager = _stateManager as IPlayerStateManager;
        manager.IAddStateMachine("player_state-"+id, player_state);

        _InstallPlayerController(id, _bus, player_state, obj);

        PlayerComponents playerComponents = new PlayerComponents();
        playerComponents.id = id;
        playerComponents.device = _event.device;
        playerComponents.obj = obj.gameObject;
        _playerManager.IAddPlayer(playerComponents);
    }

    public void IDespawnPlayer(int id)
    {
        _playerIdService.IRemoveID(id);
        _playerManager.IRemovePlayer(id);
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