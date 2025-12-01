using UnityEngine;
using Namespace_GameLoop;
using Namespace_Input;
using Namespace_Level;
using Namespace_UI;
using Namespace_StateMainMenu;
using Namespace_StateLobby;
using Namespace_StateGameplay;
using Namespace_GameState;
using Namespace_StatePause;
using Namespace_Object;

public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameLoopGroup _gameLoopManager;
    [SerializeField] private UIGroup _uiManager;
    [SerializeField] private LevelGroup _levelManager;
    [SerializeField] private InputGroup _inputManager;
    [SerializeField] private ObjectGroup _objectManager;

    private IBootstrapContext _context;
    private IGameLoopManager _interface_gameLoopManager;

    private void Awake()
    {
        IEventBus bus = new EventBus();
        IStateRegistry stateRegistry = new StateRegistry();
        GameState gameState = new GameState();

        object[] objects =
        {
            _gameLoopManager,
            _uiManager,
            _levelManager,
            _inputManager,
            _objectManager
        };

        _context = new BootstrapContext(
            bus,
            stateRegistry,
            gameState,
            objects
        );

        IInstaller<IBootstrapContext>[] installers =
        {
            // core
            new GameStateInstaller(),
            new GameLoopInstaller(),
            new InputInstaller(),
            new LevelInstaller(),
            new UIInstaller(),
            new ObjectInstaller(),
            // state
            new MainMenuStateInstaller(),
            new LobbyStateInstaller(),
            new GameplayStateInstaller(),
            new PauseStateInstaller()
        };

        foreach (var inst in installers)
            inst.Install(_context);

        _interface_gameLoopManager = _context.IGetGameLoop;
    }

    private void Start()
    {
        _context.IGetGameState.Change(_context.IGetStateRegistry.ICreate("mainmenu_state"));
        _context.IGetBus.IPublish(new LevelLoad("mainmenu_scene"));
    }

    private void Update()
    {
        _interface_gameLoopManager.IUpdate();
    }
}
