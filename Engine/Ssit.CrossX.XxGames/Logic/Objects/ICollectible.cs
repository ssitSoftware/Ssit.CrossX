namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface ICollectible
{
    bool Collect();
}

public interface ICollector
{
    void Collect(ICollectible collectible);
}