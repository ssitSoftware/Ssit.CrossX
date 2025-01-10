using System;
using System.Threading.Tasks;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace SampleGame.Game.Actors.Player;

public abstract class PlayerGun : IPlayerGun
{
    public int ShotsLeft { get; private set; }
    public int MaxShots { get; protected set; } = 5;
    public float EmptyTime { get; protected set; } = 0.35f;
    public bool IsReloading { get; private set; }
    protected float BulletReloadTime { get; set; } = 0.5f;
    protected float ShotTime { get; set; } = 0.5f;
    public float Recoil { get; set; } = 4;
    
    protected ResourceHandle<ISoundEffect> EmptySound { get; set; }
    protected ResourceHandle<ISoundEffect> ShotSound { get; set;}
    protected ResourceHandle<ISoundEffect> InsertBulletSound { get; set;}
    protected ResourceHandle<ISoundEffect> StartReloadSound { get; set;}
    protected ResourceHandle<ISoundEffect> FinishedReloadSound { get; set;}
    
    private float _timeToNextBullet;
    
    private float _timeToNextShot;
    
    public void Reload()
    {
        if (IsReloading)
            return;
        
        IsReloading = true;
        _timeToNextBullet = BulletReloadTime;
        StartReloadSound?.Resource?.PlayOnce();
    }

    public void CancelReload()
    {
        if (!IsReloading)
            return;
        
        IsReloading = false;
        FinishedReloadSound?.Resource?.PlayOnce();
    }

    public void Update(float dt)
    {
        _timeToNextShot -= dt;
        _timeToNextShot = MathF.Max(0, _timeToNextShot);
        
        if (!IsReloading) return;
        _timeToNextBullet -= dt;
        
        if (_timeToNextBullet > 0) return;

        if (ShotsLeft == MaxShots)
        {
            _timeToNextBullet = 100;
            FinishedReloadSound?.Resource?.PlayOnce();
            Task.Delay(300).ContinueWith(t => IsReloading = false);
            return;
        }

        ShotsLeft = InsertBullet();
        
        _timeToNextBullet = BulletReloadTime;
        InsertBulletSound?.Resource?.PlayOnce();
    }

    protected virtual int InsertBullet()
    {
        return ShotsLeft + 1;
    }

    public void FastReload()
    {
        ShotsLeft = MaxShots;
    }

    public bool Shot()
    {
        if (IsReloading) return false;
        
        if (_timeToNextShot > 0) return false;

        if (ShotsLeft == 0)
        {
            _timeToNextShot = EmptyTime;
            EmptySound?.Resource?.PlayOnce();
            return false;
        }

        _timeToNextShot = ShotTime;
        ShotsLeft = Math.Max(0, ShotsLeft - 1);
        
        ShotSound?.Resource?.PlayOnce();
        return true;
    }

    public void Dispose()
    {
        EmptySound?.Dispose();
        ShotSound?.Dispose();
        InsertBulletSound?.Dispose();
        FinishedReloadSound?.Dispose();
    }
}