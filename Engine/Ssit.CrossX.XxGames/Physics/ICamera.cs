namespace Ssit.CrossX.XxGames.Physics;

public enum FocusMode
{
    Permanent,
    Momentary
}

public interface ICamera
{
    Aabb VisibleArea { get; }
    void FocusOn(IBodyOwner bodyOwner, FocusMode mode);
}