using System;
using System.Numerics;
using Gunslinger.Core.UI.Views;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Services;

namespace Gunslinger.Core.UI.Handlers;

public class LabelButtonExHandler: LabelButtonHandler<LabelButtonEx>
{
    private float _time;
    private float _waveAmplitude;
    
    public LabelButtonExHandler(CreateHandlerParameters parameters, IFontsManager fontsManager, IActionDispatcher actionDispatcher, IUiSounds uiSounds) 
        : base(parameters, fontsManager, actionDispatcher, uiSounds)
    {
    }

    public override void Update(float dt)
    {
        base.Update(dt);
        
        _time += dt;
        _time %= 1;
        
        var amplitude = (AttachedView.FocusWaveAmplitude ?? 0).Calculate(CurrentScale, 0);
        var targetAmplitude = Focused ? amplitude : 0;

        if (_waveAmplitude < targetAmplitude)
        {
            _waveAmplitude += dt * amplitude * 4;
            _waveAmplitude = MathF.Min(_waveAmplitude, targetAmplitude);
        }
        else if (_waveAmplitude > targetAmplitude)
        {
            _waveAmplitude -= dt * amplitude * 4;
            _waveAmplitude = MathF.Max(_waveAmplitude, targetAmplitude);
        }
    }

    protected override void OnDraw(IRenderer2 renderer)
    {
        var offset = _waveAmplitude * (float) Math.Sin(_time * 2 * Math.PI);

        if (MathF.Abs(offset) > float.Epsilon)
        {
            renderer.StateManager.SaveState();
            renderer.StateManager.Translate(new Vector2(offset, 0));
            base.OnDraw(renderer);
            renderer.StateManager.RestoreState();
            return;
        }
        
        base.OnDraw(renderer);
    }
}