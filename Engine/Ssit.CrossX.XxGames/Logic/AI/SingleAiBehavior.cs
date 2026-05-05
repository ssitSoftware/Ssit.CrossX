namespace Ssit.CrossX.XxGames.Logic.AI;

public abstract class SingleAiBehavior<TObject, TBehavior> : AiBehavior<TObject> where TBehavior : AiBehavior<TObject>, new()
{
    public static readonly TBehavior Instance = new();
}