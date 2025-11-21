using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface IPushable
{
    IBody Body { get; }
    bool CanPull => false;
}