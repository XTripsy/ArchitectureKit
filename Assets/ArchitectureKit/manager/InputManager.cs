using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputManager : Manager
{
    private IEventBus _bus;

    [SerializeField] private InputActionAsset _action;

    [Header("Map Mapping")]
    [SerializeField] private string _mapMainMenu;

    [Header("Action Mapping")]
    [SerializeField] private string _actionPlay;

    private InputAction _aPlay;

    public void Init(IEventBus bus)
    {
        _bus = bus;

        _bus.ISubscribe<MainMenuStateEnter>(_ => EnableMainMenu());

        BindAction();
        Callback();
    }

    private void BindAction()
    {
        if (_action == null) return;

        var mapMainMenu = _action.FindActionMap(_mapMainMenu, throwIfNotFound: false);

        if (mapMainMenu != null)
        {
            _aPlay = mapMainMenu.FindAction(_actionPlay, false);
        }
    }

    private void Callback()
    {
        if (_aPlay != null)
        {
            _aPlay.started += _ => _bus.IPublish(new ActionPlayMainMenuState());
        }
    }

    private void EnableMainMenu()
    {
        DisableAll();
        _action.FindActionMap(_mapMainMenu, false)?.Enable();
    }

    private void DisableAll()
    {
        foreach (var map in _action.actionMaps) map.Disable();
    }
}
