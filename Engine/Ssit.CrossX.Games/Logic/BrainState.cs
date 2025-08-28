namespace Ssit.CrossX.Games.Logic;

public abstract class BrainState<TObject>() where TObject : class
{
    public abstract void Analyze(Brain<TObject> brain);
}