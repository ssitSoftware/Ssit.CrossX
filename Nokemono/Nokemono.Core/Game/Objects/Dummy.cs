using System;
using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Audio;
using Ssit.CrossX.Games.Logic;
using Ssit.CrossX.Games.Logic.Map;
using Ssit.CrossX.Games.Logic.Objects;
using Ssit.CrossX.Games.Physics.Collision.Shapes;
using Ssit.CrossX.Games.Physics.Dynamics;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;

namespace Nokemono.Core.Game.Objects;

public class Dummy: SpriteGameObject, IHittable
{
    private class HitValue
    {
        public string Text { get; init; }
        public float Fade = 0;
        public readonly TextRenderingContext TextRenderingContext = new ();
    }
    
    private readonly IPaletteSource _paletteSource;
    private readonly IFont _font;

    private readonly List<HitValue> _hitValues = new();

    private readonly ContextSoundContainer _soundContainer;
    
    public bool Active => true;
    
    public Dummy(GameObjectsServices services, ObjectCreationParameters parameters, IPaletteSource paletteSource, IFontsManager fontsManager) : base(services, parameters)
    {
        _paletteSource = paletteSource;
        _font = fontsManager.GetFont("Default", 10);
        
        _soundContainer = services.Container.IoCConstruct<ContextSoundContainer>(new ContextSoundContainer.Parameters
        {
            Emitter = null 
        });
        _soundContainer.RegisterSound("Hit", GamePhysics.Materials.Any, "assets:/Game/Sounds/Effects/Thump.wav");
        
        BoundsRect = new RectangleF(-2, -2, 4, 4);
        InitializeSprite("assets:/Game/Objects/Dummy");

        Body.CreateFixture(new CircleShape(0.3f, 1)
        {
            Position = new Vector2(0, -0.6f)
        });
        
        Body.BodyType = BodyType.Static;
        Body.IsSensor = true;
        
        AddState("Idle", new State());
        AddState("Left", new State());
        AddState("Right", new State());
        
        SetState("Idle");
    }

    protected override void OnRender(IRenderer2 renderer, RgbaColor color)
    {
        base.OnRender(renderer, color);
        
        var pos = Body.Position * Services.GameTemplate.TileSize;
        pos = pos.TrimVectorToPixels(Services.GameTemplate.TrimToPixels);
        
        var accentColor = _paletteSource.Palette[4];
        var outlineColor =  _paletteSource.Palette[1];

        for (var idx = 0; idx < _hitValues.Count; idx++)
        {
            var hv  =  _hitValues[idx];
            
            renderer.TextRenderer.DrawText(_font, hv.Text, pos - new Vector2(0, Services.GameTemplate.TileSize * 2 + Services.GameTemplate.TileSize *  hv.Fade), 
                ContentAlign.Center | ContentAlign.VCenter, color: accentColor, outlineColor:  outlineColor, context: hv.TextRenderingContext);
        }
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);

        for (var idx = 0; idx < _hitValues.Count; )
        {
            var hv = _hitValues[idx];
            hv.Fade += dt * 4;

            if (hv.Fade >= 1)
            {
                _hitValues.RemoveAt(idx);
                continue;
            }

            ++idx;
        }
    }

    public void Hit(Vector2 dir, float power)
    {
        SetState(dir.X > 0 ? "Right" : "Left");
        _hitValues.Add(new HitValue
        {
            Text = $"{(int)(power*10)}"
        });
        
        _soundContainer.Play("Hit", pitch: 0);
    }

    protected override void OnAnimationFinished(string sequenceName)
    {
        base.OnAnimationFinished(sequenceName);

        if (sequenceName is "Right" or "Left")
        {
            SetState("Idle");
        }
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        _soundContainer.Dispose();
    }
}