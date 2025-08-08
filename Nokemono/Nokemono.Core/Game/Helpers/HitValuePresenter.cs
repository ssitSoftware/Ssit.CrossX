using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Games.Template;
using Ssit.CrossX.Games.Utils;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Font;
using Ssit.CrossX.Graphics.Renderer;

namespace Nokemono.Core.Game.Helpers;

public class HitValuePresenter
{
    private class HitValue
    {
        public string Text { get; init; }
        public Vector2 Position { get; init; }

        public float Fade = 0;
        public readonly TextRenderingContext TextRenderingContext = new ();
    }
    
    private readonly IPaletteSource _paletteSource;
    private readonly IGameTemplate _gameTemplate;
    private readonly IFont _font;

    private readonly List<HitValue> _hitValues = new();

    public HitValuePresenter(IPaletteSource paletteSource, IFontsManager fontsManager, IGameTemplate gameTemplate)
    {
        _paletteSource = paletteSource;
        _gameTemplate = gameTemplate;
        _font = fontsManager.GetFont("Default", 10);
    }

    public void Render(IRenderer2 renderer)
    {
        var accentColor = _paletteSource.Palette[4];
        var outlineColor =  _paletteSource.Palette[1];

        for (var idx = 0; idx < _hitValues.Count; idx++)
        {
            var hv  =  _hitValues[idx];
            
            var pos = hv.Position * _gameTemplate.TileSize;
            pos = pos.TrimVectorToPixels(_gameTemplate.TrimToPixels);
            
            renderer.TextRenderer.DrawText(_font, hv.Text, pos - new Vector2(0, _gameTemplate.TileSize * 2 + _gameTemplate.TileSize *  hv.Fade), 
                ContentAlign.Center | ContentAlign.VCenter, color: accentColor, outlineColor:  outlineColor, context: hv.TextRenderingContext);
        }
    }

    public void Update(float dt)
    {
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

    public void AddValue(int value, Vector2 position)
    {
        _hitValues.Add(new HitValue
        {
            Text = $"{value}",
            Position = position
        });
    }
}