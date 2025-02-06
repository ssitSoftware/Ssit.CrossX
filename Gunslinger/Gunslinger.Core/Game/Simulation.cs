using System;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;

namespace Gunslinger.Core.Game;

public class Simulation : ISimulation
{
    int ISimulation.RenderPasses => 1;

    void ISimulation.Render(IRenderer renderer, RenderMode mode, RectangleF target, int renderPass, float scale)
    {
        if (renderPass != 0)
            return;
        
        Render(renderer, target, mode, scale);
    }

    public void RenderDebug(IRenderer renderer, RectangleF target, float scale)
    {
    }

    void ISimulation.Update(float deltaTime) => Update(deltaTime);

    public event Action Updated;

    private void Render(IRenderer renderer, RectangleF target, RenderMode renderMode, float scale)
    {
        
    }

    private void Update(float deltaTime)
    {
        Updated?.Invoke();
    }
    
    public void Dispose()
    {
        
    }
}