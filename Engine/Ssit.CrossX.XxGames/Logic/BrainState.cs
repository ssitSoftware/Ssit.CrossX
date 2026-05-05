namespace Ssit.CrossX.XxGames.Logic;

public abstract class BrainState<TObject>() where TObject : class
{
    public abstract void Analyze(Brain<TObject> brain);
}