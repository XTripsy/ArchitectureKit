using System.Collections.Generic;
using UnityEngine;
using MyGameLoop;
using MyInput;
using MyLevel;
using MyUI;
using System;

public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameLoopGroup gameLoopManager;
    [SerializeField] private UIGroup uiManager;
    [SerializeField] private LevelGroup levelManager;
    [SerializeField] private InputGroup inputManager;

    private IBootstrapContext context;
    /*private List<Manager> managers = new List<Manager>();
    private IGameLoopManager _iGameLoopManager;

    private IEventBus _bus;
    private GameState _gameState;*/

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

        /*IFactory<FactoryComponent.Args, GameObject> factory_component = new FactoryComponent();
        IUpdateHandler updateHandler = new UpdateHandler();

        _bus = new EventBus();
        _iGameLoopManager = new GameLoopManager(updateHandler as UpdateHandler);
        managers.Add(_iGameLoopManager);

        Manager temp = new UIManager(uiManager, factory_component);
        managers.Add(temp);

        temp = new LevelManager(_bus);
        managers.Add(temp);

        temp = new InputManager(_bus, inputManager);
        managers.Add(temp);*/
    }

    private void Start()
    {
        context.IGetGameState.Change(new MainMenuState(context.IGetBus));
        context.IGetBus.IPublish(new SLevelLoad("mainmenu_scene"));

        /*IKeyFactory<FactoryState.EState, IState> factory_state = new FactoryState(_bus);

        IUIManager ui_manager = GetManager<UIManager>();

        _gameState = new GameState();
        UIController _uiController = new UIController(_bus, ui_manager);
        StateController _stateController = new StateController(_bus, _gameState, factory_state);
        InputController _inputController = new InputController(_bus);

        _gameState.Change(new MainMenuState(_bus));

        _bus.IPublish(new SLevelLoad("mainmenu_scene"));*/
    }

    private void Update()
    {
        context.IGetGameLoop.IUpdate();
        /*_iGameLoopManager.IUpdate();
        _gameState.Update(Time.deltaTime);*/
    }

    /*private T GetManager<T>() where T : Manager
    {
        foreach (var manager in managers)
        {
            if (manager is T)
                return (T)manager;
        }

        return default;
    }*/
}
