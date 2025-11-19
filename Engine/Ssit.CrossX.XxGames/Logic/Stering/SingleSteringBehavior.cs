namespace Ssit.CrossX.XxGames.Logic.Stering;

public abstract class SingleSteringBehavior<TObject, TBehavior> : SteringBehavior<TObject> where TBehavior : SingleSteringBehavior<TObject, TBehavior>, new()
{
    public static readonly TBehavior Instance = new();
    
    protected SingleSteringBehavior()
    {
        if (Instance != null)
        {
            throw new System.Exception("SingleSteringBehavior is a singleton");
        }
    }
}