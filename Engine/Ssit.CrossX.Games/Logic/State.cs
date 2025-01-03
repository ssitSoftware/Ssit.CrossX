namespace Ssit.CrossX.Games.Logic;

public class State
{
    private readonly Behavior[] _behaviors;

    public State(params Behavior[] behaviors)
    {
        _behaviors = behaviors;
    }

    public void Update(float dt) => OnUpdate( dt);

    protected virtual void OnUpdate(float dt)
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            if (_behaviors[idx].Update(dt))
            {
                return;
            }
        }
    }

    public void FixedUpdate(float dt) => OnFixedUpdate(dt);

    protected virtual void OnFixedUpdate(float dt)
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            if (_behaviors[idx].FixedUpdate(dt))
            {
                return;
            }
        }
    }

    public void PostFixedUpdate() => OnPostFixedUpdate();

    protected virtual void OnPostFixedUpdate()
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            if (_behaviors[idx].PostFixedUpdate())
            {
                return;
            }
        }
    }

    public void Enter()
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            _behaviors[idx].EnterState();
        }
    }
    
    public void Leave()
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            _behaviors[idx].LeaveState();
        }
    }

    public void SequenceFinished(string name)
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            if (_behaviors[idx].SequenceFinished(name))
            {
                return;
            }
        }
    }

    public void Event(string name, float parameter)
    {
        for (var idx = 0; idx < _behaviors.Length; ++idx)
        {
            if (_behaviors[idx].Event(name, parameter))
            {
                return;
            }
        }
    }
}