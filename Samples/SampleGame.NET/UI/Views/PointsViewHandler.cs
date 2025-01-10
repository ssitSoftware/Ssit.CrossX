using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX;
using Ssit.CrossX.Content;
using Ssit.CrossX.Games;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.Graphics.Sprites;
using Ssit.CrossX.UI.Handlers;
using Ssit.CrossX.UI.Parameters;

namespace SampleGame.UI.Views;

public class PointsViewHandler : ViewHandler<PointsView>
{
    private readonly SizeF _frameSize;

    private readonly List<SpriteInstance> _spriteInstances = new();
    private readonly ResourceHandle<GameObject> _gameObject;

    private int _lastValue = 0;
    
    public PointsViewHandler(CreateHandlerParameters parameters, IContentManager contentManager) : base(parameters)
    {
        _gameObject = contentManager.Get<GameObject>(AttachedView.Path);
        _frameSize = _gameObject.Resource.Description.Size;

        _lastValue = AttachedView.Points.Value;
        
        AttachedView.Points.ValueChanged += PointsChanged;
        AttachedView.MaxPoints.ValueChanged += Recalculate;

        Recalculate(0);
    }

    private void PointsChanged(int val)
    {
        PrepareSprites();
        
        if (_lastValue > _spriteInstances.Count || val > _spriteInstances.Count)
        {
            Recalculate(0);
        }

        if (_lastValue > 0 && val < _lastValue)
        {
            _spriteInstances[_lastValue - 1].SetSequence("Turn Off");
            _spriteInstances[_lastValue - 1].SequenceFinished += SequenceFinished;
        }

        if (val > 0 && val > _lastValue)
        {
            _spriteInstances[val-1].SetSequence("Turn On");
            _spriteInstances[val-1].SequenceFinished += SequenceFinished;
        }

        _lastValue = val;
    }

    private void SequenceFinished(SpriteInstance instance, string sequenceName, bool reverse)
    {
        switch (sequenceName)
        {
            case "Turn Off":
                instance.SetSequence("Off");
                break;
            
            case "Turn On":
                instance.SetSequence("On");
                break;
        }
        instance.SequenceFinished -= SequenceFinished;
    }

    private void Recalculate(int _)
    {
        PrepareSprites();
        Parent?.RecalculateLayout(AttachedView);
    }

    private void PrepareSprites()
    {
        while (_spriteInstances.Count > AttachedView.MaxPoints.Value)
        {
            _spriteInstances.RemoveAt(_spriteInstances.Count - 1);
        }

        while (_spriteInstances.Count < AttachedView.MaxPoints.Value)
        {
            _spriteInstances.Add(_gameObject.Resource.CreateSpriteInstance());
            _spriteInstances[^1].SetSequence("Off");
        }
    }

    public override void Update(float dt)
    {
        base.Update(dt);

        for (var idx = 0; idx < _spriteInstances.Count; ++idx)
        {
            _spriteInstances[idx].Advance(dt);
            if (idx < AttachedView.Points.Value)
            {
                if (_spriteInstances[idx].CurrentSequence == "Off")
                {
                    _spriteInstances[idx].SetSequence("On");
                }
            }
            else
            {
                if (_spriteInstances[idx].CurrentSequence == "On")
                {
                    _spriteInstances[idx].SetSequence("Off");
                }
            }
        }
    }

    public override void Draw(IRenderer renderer)
    {
        var spacing = AttachedView.Spacing.Calculate(CurrentScale, 0);
        for (var idx = 0; idx < AttachedView.MaxPoints.Value; ++idx)
        {
            renderer.DrawSprite(_spriteInstances[idx], ScreenBounds.TopLeft + new Vector2(idx * (_frameSize.Width * CurrentScale + spacing), 0), CurrentScale);
        }
    }

    public override void CalculateSize(out Length width, out Length height)
    {
        width = _frameSize.Width * AttachedView.MaxPoints.Value + AttachedView.Spacing * (AttachedView.MaxPoints.Value - 1);
        height = _frameSize.Height;
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        
        for (var idx = 0; idx < _spriteInstances.Count; ++idx)
        {
            _spriteInstances[idx].Dispose();
        }
        _spriteInstances.Clear();
        _gameObject?.Dispose();
    }
}