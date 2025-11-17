using UnityEngine;

public sealed class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameLoopManager _gameLoopManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private InputManager _inputManager;

    private IEventBus _bus;
    private UpdateHandler _updateHandler;
    private GameState _gameState;
    private UIController _uiController;
    private StateController _stateController;
    InputController _inputController;

    private void Start()
    {
        _bus = new EventBus();
        _updateHandler = new UpdateHandler();
        _gameState = new GameState();
        _uiController = new UIController(_bus, _uiManager);
        _stateController = new StateController(_bus, _gameState);
        _inputController = new InputController(_bus);

        _levelManager.Init(_bus);
        _inputManager.Init(_bus);
        _gameState.Change(new MainMenuState(_bus));

        _bus.IPublish(new SLevelLoad("mainmenu_scene"));
    }

    private void Update()
    {
        _updateHandler.Update(Time.deltaTime);
        _gameState.Update(Time.deltaTime);
    }
}
