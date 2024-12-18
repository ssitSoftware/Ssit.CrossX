namespace Ssit.CrossX.Input.Internal;

public interface ITouchEntity: ITouchEvent
{
    double InitialTime { get; }
    double Time { get; }
}