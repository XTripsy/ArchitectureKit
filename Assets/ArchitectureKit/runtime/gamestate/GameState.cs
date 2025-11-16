using UnityEngine;

public sealed class GameState
{
    private IState _current;
    private bool _isChanging;
    private IState _pending;

    public void Change(IState next)
    {
        if (next == null) return;
        if (_current != null && _current.GetType() == next.GetType()) return;
        if (_isChanging) 
        { 
            _pending = next; 
            return; 
        }

        _isChanging = true;
        _current?.IOnExit();
        _current = next;
        _current.IOnEnter();
        _isChanging = false;

        if (_pending != null) 
        { 
            var p = _pending; 
            _pending = null; 
            Change(p); 
        }
    }

    public void Update(float dt) => _current?.IOnUpdate(dt);
}
