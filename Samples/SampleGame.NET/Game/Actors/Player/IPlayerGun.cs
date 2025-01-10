using System;

namespace SampleGame.Game.Actors.Player;

public interface IPlayerGun: IDisposable
{
    int ShotsLeft { get; }
    int MaxShots { get; }
    float Recoil { get; }
    bool IsReloading { get; }
    void Reload();
    bool Shot();
    void CancelReload();
    void Update( float dt );
    void FastReload();
}