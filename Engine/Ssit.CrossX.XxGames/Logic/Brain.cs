using System.Collections.Generic;

namespace Ssit.CrossX.XxGames.Logic;

public class Brain<TObject>(TObject owner)
    where TObject : class
{
    public delegate void StateFunc(Brain<TObject> brain);
    
    public TObject Owner { get; } = owner;

    private StateFunc _currentState;
    
    private readonly Dictionary<string, object> _parameters = new();
    private readonly HashSet<string> _flags = new();

    public Brain<TObject> ResetFlags()
    {
        _flags.Clear();
        return this;
    }
    
    public bool this[string flag]
    {
        get => _flags.Contains(flag);
        set
        {
            if (value)
            {
                _flags.Add(flag);
            }
            else
            {
                _flags.Remove(flag);
            }
        }
    }
    
    public void SetState(StateFunc func) => _currentState = func;
    public void Analyze() => _currentState?.Invoke(this);

    public void SetParameter(string key, object value)
    {
        if (value is null)
        {
            _parameters.Remove(key);
            return;
        }
        _parameters[key] = value;
    }

    public T GetParameter<T>(string key, T defaultValue)
    {
        _parameters.TryGetValue(key, out var obj);

        if (obj is T t)
        {
            return t;
        }
        
        return defaultValue;
    }
}