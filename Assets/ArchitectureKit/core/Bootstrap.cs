using UnityEngine;
using MyGameLoop;
using MyInput;
using MyLevel;
using MyUI;

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
            gameState,
            objects
        );

        IInstaller<IBootstrapContext>[] installers =
        {
            new GameStateInstaller(),
            new GameLoopInstaller(),
            new InputInstaller(),
            new LevelInstaller(),
            new UIInstaller()
        };

        foreach (var inst in installers)
            inst.Install(context);
    }

    private void Start()
    {
        context.IGetGameState.Change(new MainMenuState(context.IGetBus));
        context.IGetBus.IPublish(new SLevelLoad("mainmenu_scene"));
    }

    private void Update()
    {
        context.IGetGameLoop.IUpdate();
    }
}
