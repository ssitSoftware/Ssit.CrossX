namespace Ssit.CrossX.Core.Internal;

public interface IInternalActionScheduler: IActionScheduler
{
    void Process();
}