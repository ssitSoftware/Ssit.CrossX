using System;
using Ssit.CrossX;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;

namespace Gunslinger.Core.Game;

public class Simulation: ISimulation
{
    int ISimulation.RenderPasses => (int)RenderPass.Count;
    void ISimulation.Render(IRenderer renderer, RectangleF target, int renderPass, float scale) => Render(renderer, target, (RenderPass)renderPass, scale);
    void ISimulation.Update(float deltaTime) => Update(deltaTime);

    public event Action Updated;
    
    private void Render(IRenderer renderer, RectangleF target, RenderPass renderPass, float scale)
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