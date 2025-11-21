using UnityEngine;
using MyGameLoop;
using MyInput;
using MyLevel;
using MyUI;
using State_MainMenu;
using State_Gameplay;

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
            new GameplayStateInstaller()
        };

        foreach (var inst in installers)
            inst.Install(context);

    }

    private void Start()
    {
        context.IGetGameState.Change(context.IGetStateRegistry.ICreate("mainmenu_state"));
        context.IGetBus.IPublish(new SLevelLoad("mainmenu_scene"));
    }

    private void Update()
    {
        context.IGetGameLoop.IUpdate();
    }
}
