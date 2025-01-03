namespace Ssit.CrossX.Games.Rendering;

public interface IShooterRenderer : IGameObjectRenderer
{
    bool IsAiming { get; set; }
    void UpdateAimingAngle(float angle, float recoil);
}