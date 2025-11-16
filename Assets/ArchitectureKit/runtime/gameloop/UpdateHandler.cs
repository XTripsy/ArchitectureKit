using System.Collections.Generic;

public sealed class UpdateHandler
{
    private readonly List<IUpdate> _lists = new();

    public void Add(IUpdate t)
    { 
        if (!_lists.Contains(t)) _lists.Add(t); 
    }

    public void Remove(IUpdate t) 
    { 
        _lists.Remove(t); 
    }

    public void Update(float dt) 
    { 
        for (int i = 0; i < _lists.Count; i++) 
            _lists[i].IUpdate(dt); 
    }
}
