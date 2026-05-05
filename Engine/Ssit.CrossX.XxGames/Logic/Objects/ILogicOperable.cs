using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Logic.Objects;

public interface ILogicOperable
{
    void Operate(IBodyOwner @operator);
}