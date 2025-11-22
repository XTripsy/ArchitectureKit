using UnityEngine;
using Namespace_GameLoop;
using Namespace_Input;
using Namespace_Level;
using Namespace_UI;
using Namespace_StateMainMenu;
using Namespace_StateGameplay;
using Namespace_GameState;
using Namespace_StatePause;

public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameLoopGroup gameLoopManager;
    [SerializeField] private UIGroup uiManager;
    [SerializeField] private LevelGroup levelManager;
    [SerializeField] private InputGroup inputManager;

    private IBootstrapContext context;

    private void Awake()
    {
        IEventBus bus = new EventBus();
        IStateRegistry stateRegistry = new StateRegistry();
        GameState gameState = new GameState();

        object[] objects =
        {
            gameLoopManager,
            uiManager,
            levelManager,
            inputManager
        };

        context = new BootstrapContext(
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
            // state
            new MainMenuStateInstaller(),
            new GameplayStateInstaller(),
            new PauseStateInstaller()
        };

        foreach (var inst in installers)
            inst.Install(context);

    }

    private void Start()
    {
        context.IGetGameState.Change(context.IGetStateRegistry.ICreate("mainmenu_state"));
        context.IGetBus.IPublish(new LevelLoad("mainmenu_scene"));
    }

    private void Update()
    {
        context.IGetGameLoop.IUpdate();
    }
}
