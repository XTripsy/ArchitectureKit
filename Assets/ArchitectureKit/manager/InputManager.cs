using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InputManager : Manager
{
    private IEventBus _bus;

    /*private InputActionAsset _action;
    private string _mapMainMenu;
    private string _actionPlay;*/
    private InputGroup _group;

    private InputAction _aPlay;

    public InputManager(IEventBus bus, InputGroup group)
    {
        _bus = bus;
        _group = group;

        _bus.ISubscribe<MainMenuStateEnter>(_ => EnableMainMenu());

        BindAction();
        Callback();
    }

    private void BindAction()
    {
        if (_group.action == null) return;

        var mapMainMenu = _group.action.FindActionMap(_group.mapMainMenu, throwIfNotFound: false);

        if (mapMainMenu != null)
        {
            _aPlay = mapMainMenu.FindAction(_group.actionPlay, false);
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
        _group.action.FindActionMap(_group.mapMainMenu, false)?.Enable();
    }

    private void DisableAll()
    {
        foreach (var map in _group.action.actionMaps) map.Disable();
    }

    public void IStart() {}

    public void IUpdate() {}
}
