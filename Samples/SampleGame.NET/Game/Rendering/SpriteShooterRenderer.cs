using System;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games;
using Ssit.CrossX.Games.Rendering;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;

namespace SampleGame.Game.Rendering;

public class SpriteShooterRenderer : SpriteRenderer, IShooterRenderer
{
    private static readonly string[] AimingStates =
        ["aim0", "aim22", "aim45", "aim67"];

    private ImageTransform _gunTransform = ImageTransform.None;
    public bool IsAiming { get; set; }
    public bool GunBehind { get; set; }
    private float _recoil = 0;

    private Vector2 _aimingVector;
    private readonly SpriteInstance _gunSprite;
    private readonly Vector2 _gunOffset;
    private readonly ResourceHandle<ITexture> _shadowTexture;
    private readonly Vector2 _shadowOrigin;

    public SpriteShooterRenderer(GameObject heroObj, GameObject gunObj, Vector2 gunOffset, GameObject shadowObject) : base(heroObj)
    {
        _gunSprite = gunObj.CreateSpriteInstance();
        _gunOffset = gunOffset;
        _shadowTexture = shadowObject.RequestTexture();
        _shadowOrigin = shadowObject.Description.Origin;
    }

    public void UpdateAimingAngle(float angle, float recoil)
    {
        if (float.IsNaN(angle))
        {
            return;
        }

        _recoil = recoil;
        
        var intAngle = (int)MathF.Round((180 * angle / MathF.PI + 360) / 22.5f);
        intAngle %= AimingStates.Length * 4;
        
        _gunTransform = (ImageTransform)(intAngle / AimingStates.Length);
        
        intAngle %= AimingStates.Length;
        
        _gunSprite.SetSequence(AimingStates[intAngle], false);

        _aimingVector = Vector2.Transform(new Vector2(0, 1), Matrix3x2.CreateRotation(angle));
    }

    public override void Render(IRenderer renderer, Vector2 position, RenderPass renderPass)
    {
        if (renderPass == RenderPass.Shadow)
        {
            renderer.DrawTexture(_shadowTexture.Resource, (position + new Vector2(1, 1)) * Scale, null, _shadowOrigin, 0, Scale, RgbaColor.White * 0.2f);
            return;
        }
        
        if (!IsAiming || renderPass != RenderPass.Normal)
        {
            base.Render(renderer, position, renderPass);
            return;
        }

        var offset = _gunOffset - _aimingVector * _recoil;
        
        if (GunBehind)
        {
            renderer.DrawSprite(_gunSprite, (position + offset) * Scale, Scale, null, _gunTransform);
            base.Render(renderer, position, renderPass);
        }
        else
        {
            base.Render(renderer, position, renderPass);
            renderer.DrawSprite(_gunSprite, (position + offset) * Scale, Scale, null, _gunTransform);
        }

        var pos = (position + _gunOffset) * Scale + _aimingVector * Scale * 32;
        renderer.FillRectangle(new RectangleF(pos.X - 2, pos.Y - 2, 4, 4), new RgbaColor( 255, 0, 0, 255));

        pos = position * Scale;
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        _gunSprite?.Dispose();
        _shadowTexture?.Dispose();
    }
}