using System;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;

namespace SampleGame.Game.Logic;

public class Simulation: IDisposable
{
    private float _time = 0;
    public World World { get; }
    public float UnitScale { get; }
    public float TimeDelta { get; set; } = 1 / 60f;
    
    public ICamera Camera { get; set; }

    public event Action Updated; 
    
    public Simulation(Vector2 gravity, float unitScale = 1)
    {
        World = new World(gravity);
        UnitScale = unitScale;
        World.BodyRemoved = BodyRemoved;
    }

    private void BodyRemoved(Body body)
    {
        if (body.UserData is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public void Render(IRenderer renderer, RectangleF target, RenderPass renderPass, float scale)
    {
        var center = target.Center;
        var pixelLookAt = Camera.LookAt * scale * UnitScale;
        var offset = center - pixelLookAt;

        renderer.SetClipRect(new Rectangle((int)target.X, (int)target.Y, (int)target.Width, (int)target.Height));
        renderer.SetTransform(Matrix3x2.CreateScale(scale) * Matrix3x2.CreateTranslation(offset));
        renderer.SetBlendMode(BlendMode.AlphaBlend);
        
        switch (renderPass)
        {
            case RenderPass.Shadow:
            case RenderPass.Normal:
            case RenderPass.Overlay:
            case RenderPass.Glow:
                for (var idx = 0; idx < World.BodyList.Count; ++idx)
                {
                    var body = World.BodyList[idx];
                    if (body.UserData is IDrawable drawable)
                    {
                        drawable.Draw(renderer, renderPass);
                    }
                }
                break;
            
            case RenderPass.Background:
                break;
        }
        
        renderer.SetTransform(null);
        renderer.SetClipRect(null);
    }
    
    public void Update(float dt)
    {
        _time += dt;

        for (var idx = 0; idx < World.BodyList.Count; ++idx)
        {
            var body = World.BodyList[idx];
            if (body.UserData is IUpdatable updatable)
            {
                updatable.Update(TimeDelta);
            }
        }
        
        while (_time >= TimeDelta)
        {
            _time -= TimeDelta;

            for (var idx = 0; idx < World.BodyList.Count; ++idx)
            {
                var body = World.BodyList[idx];
                if (body.UserData is IUpdatable updatable)
                {
                    updatable.FixedUpdate(TimeDelta);
                }
            }

            World.Step(TimeDelta);
            
            for (var idx = 0; idx < World.BodyList.Count; ++idx)
            {
                var body = World.BodyList[idx];
                if (body.UserData is IUpdatable updatable)
                {
                    updatable.PostFixedUpdate();
                }
            }
        }
        
        for (var idx = 0; idx < World.BodyList.Count; ++idx)
        {
            var body = World.BodyList[idx];
            if (body.UserData is IUpdatable updatable)
            {
                updatable.PostUpdate();
            }
        }
        
        Updated?.Invoke();
    }

    public void Dispose()
    {
        for (var idx = 0; idx < World.BodyList.Count; ++idx)
        {
            var body = World.BodyList[idx];
            if (body.UserData is IDisposable disposable)
            {
                disposable?.Dispose();
            }
        }
        
        World.Clear();
    }
}