using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct GameLoopGroup
{

}

[System.Serializable]
public struct UIGroup
{
    public Canvas canvas;
    public UICatalog catalog;
}

[System.Serializable]
public struct LevelGroup
{

}

[System.Serializable]
public struct InputGroup
{
    public InputActionAsset action;
    public string mapMainMenu;
    public string actionPlay;
}

public sealed class Bootstrap : MonoBehaviour
{
    [Space]
    public GameLoopGroup gameLoopManager;

    [Space]
    public UIGroup uiManager;

    [Space]
    public LevelGroup levelManager;

    [Space]
    public InputGroup inputManager;

    private List<Manager> managers = new List<Manager>();

    private IEventBus _bus;
    private UpdateHandler _updateHandler;
    private GameState _gameState;
    /*private UIController _uiController;
    private StateController _stateController;
    private InputController _inputController;*/

    private void Awake()
    {
        IFactory<FactoryComponent.Args, GameObject> factory_component = new FactoryComponent();

        _bus = new EventBus();

        Manager temp = new GameLoopManager();
        managers.Add(temp);

        temp = new UIManager(uiManager, factory_component);
        managers.Add(temp);

        temp = new LevelManager(_bus);
        managers.Add(temp);

        temp = new InputManager(_bus, inputManager);
        managers.Add(temp);
    }

    private void Start()
    {
        IKeyFactory<FactoryState.EState, IState> factory_state = new FactoryState(_bus);

        GameLoopManager gameloop_manager = GetManager<GameLoopManager>();
        UIManager ui_manager = GetManager<UIManager>();
        LevelManager level_manager = GetManager<LevelManager>();
        InputManager input_manager = GetManager<InputManager>();

        _updateHandler = new UpdateHandler();
        _gameState = new GameState();
        UIController _uiController = new UIController(_bus, ui_manager);
        StateController _stateController = new StateController(_bus, _gameState, factory_state);
        InputController _inputController = new InputController(_bus);

        _gameState.Change(new MainMenuState(_bus));

        _bus.IPublish(new SLevelLoad("mainmenu_scene"));

        foreach (var item in managers)
        {
            item.IStart();
        }
    }

    private void Update()
    {
        foreach (var item in managers)
        {
            item.IUpdate();
        }

        _updateHandler.Update(Time.deltaTime);
        _gameState.Update(Time.deltaTime);
    }

    private T GetManager<T>() where T : Manager
    {
        foreach (var manager in managers)
        {
            if (manager is T)
                return (T)manager;
        }

        return default;
    }
}
