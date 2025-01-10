using System;
using Ssit.CrossX.Audio;
using Ssit.CrossX.Content;

namespace SampleGame.Game.Actors.Player;

public class Revolver : PlayerGun
{
    public Revolver(IContentManager contentManager)
    {
        MaxShots = 6;
        Recoil = 4;
        
        ShotTime = 0.7f;
        BulletReloadTime = 0.75f;
        EmptyTime = 0.6f;
        
        StartReloadSound = contentManager.Get<ISoundEffect>("assets:/Sounds/StartReload.wav");
        ShotSound = contentManager.Get<ISoundEffect>("assets:/Sounds/GunShot.wav");
        InsertBulletSound = contentManager.Get<ISoundEffect>("assets:/Sounds/Reload.wav");
        FinishedReloadSound = contentManager.Get<ISoundEffect>("assets:/Sounds/ReloadDone.wav");
        EmptySound = contentManager.Get<ISoundEffect>("assets:/Sounds/EmptyMagazine.wav");
    }
    
    protected override int InsertBullet()
    {
        var newShots = ShotsLeft / 2;
        newShots++;
        return Math.Min(MaxShots, newShots * 2);
    }
}

public class FastGun : PlayerGun
{
    public FastGun(IContentManager contentManager)
    {
        MaxShots = 20;
        Recoil = 2;
        
        ShotTime = 0.1f;
        BulletReloadTime = 0.75f;
        EmptyTime = 0.6f;
        
        StartReloadSound = contentManager.Get<ISoundEffect>("assets:/Sounds/StartReload.wav");
        ShotSound = contentManager.Get<ISoundEffect>("assets:/Sounds/GunShot.wav");
        InsertBulletSound = contentManager.Get<ISoundEffect>("assets:/Sounds/Reload.wav");
        FinishedReloadSound = contentManager.Get<ISoundEffect>("assets:/Sounds/ReloadDone.wav");
        EmptySound = contentManager.Get<ISoundEffect>("assets:/Sounds/EmptyMagazine.wav");
    }
    
    protected override int InsertBullet()
    {
        var newShots = ShotsLeft / 5;
        newShots++;
        return Math.Min(MaxShots, newShots * 5);
    }
}