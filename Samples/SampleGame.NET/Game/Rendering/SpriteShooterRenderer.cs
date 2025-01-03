using System;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace SampleGame.Game.Rendering;

public class SpriteShooterRenderer(SpriteInstance spriteInstance, SpriteInstance gunSprite, Vector2 gunOffset)
    : SpriteRenderer(spriteInstance), IShooterRenderer
{
    private static readonly string[] AimingStates =
        ["aim0", "aim22", "aim45", "aim67"];

    private ImageTransform _gunTransform = ImageTransform.None;
    public bool IsAiming { get; set; }
    private bool _gunBehind;

    private Vector2 _aimingVector;

    public void UpdateAimingAngle(float angle)
    {
        if (float.IsNaN(angle))
        {
            return;
        }
        
        var intAngle = (int)MathF.Round((180 * angle / MathF.PI + 360) / 22.5f);
        intAngle %= AimingStates.Length * 4;
        
        _gunTransform = (ImageTransform)(intAngle / AimingStates.Length);
        
        _gunBehind = (intAngle / AimingStates.Length) is 1 or 2;
        intAngle %= AimingStates.Length;
        
        gunSprite.SetSequence(AimingStates[intAngle], false);

        _aimingVector = Vector2.Transform(new Vector2(0, 1), Matrix3x2.CreateRotation(angle));
    }

    public override void Render(IRenderer renderer, Vector2 position)
    {
        if (!IsAiming)
        {
            base.Render(renderer, position);
            return;
        }
        
        if (_gunBehind)
        {
            renderer.DrawSprite(gunSprite, (position + gunOffset) * Scale, Scale, null, _gunTransform);
            base.Render(renderer, position);
        }
        else
        {
            base.Render(renderer, position);
            renderer.DrawSprite(gunSprite, (position + gunOffset) * Scale, Scale, null, _gunTransform);
        }

        var pos = (position + gunOffset) * Scale + _aimingVector * Scale * 32;
        renderer.FillRectangle(new RectangleF(pos.X - 2, pos.Y - 2, 4, 4), new RgbaColor( 255, 0, 0, 255));
    }
}