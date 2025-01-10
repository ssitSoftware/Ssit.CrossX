using System;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;

namespace SampleGame.Game.Logic;

public class ObjectData: IUpdatable, IDrawable, IDisposable
{
    private readonly IUpdatable _updatable;
    private readonly IDrawable _drawable;

    public ObjectData(IUpdatable updatable, IDrawable drawable)
    {
        _updatable = updatable;
        _drawable = drawable;
    }
    
    public void Draw(IRenderer renderer, RenderPass renderPass)
    {
        _drawable.Draw(renderer, renderPass);
    }
    
    public void Update(float dt)
    {
        _updatable.Update(dt);
    }

    public void PostUpdate()
    {
        _updatable.PostUpdate();
    }

    public void FixedUpdate(float dt)
    {
        _updatable.FixedUpdate(dt);
    }

    public void PostFixedUpdate()
    {
        _updatable.PostFixedUpdate();
    }

    public void Dispose()
    {
        if (_updatable is IDisposable disposable)
        {
            disposable?.Dispose();
        }

        if (_drawable is IDisposable disposable2)
        {
            disposable2?.Dispose();
        }
    }
}