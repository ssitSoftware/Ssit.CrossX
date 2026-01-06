#if IOS
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Input;

[SuppressMessage("Interoperability", "CA1422")]
public class SdlHapticDevice: IHapticDevice
{
    public FeedbackLevel ButtonFeedbackLevel { get; set; } = FeedbackLevel.Level1;
    public FeedbackLevel ForceFeedbackLevel { get; set; } = FeedbackLevel.Level2;
    
    private readonly UIImpactFeedbackGenerator[] _feedbackGenerators;

    public SdlHapticDevice()
    {
        _feedbackGenerators = new UIImpactFeedbackGenerator[5];
        _feedbackGenerators[0] = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Rigid);
        _feedbackGenerators[1] = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Soft);
        _feedbackGenerators[2] = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
        _feedbackGenerators[3] = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Medium);
        _feedbackGenerators[4] = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy);
    }

    public void Feedback(FeedbackStyle style, Vector2? position)
    {
        var index = style switch
        {
            FeedbackStyle.Release => 0,
            FeedbackStyle.Push => 1,
            FeedbackStyle.Light => 2,
            FeedbackStyle.Medium => 3,
            FeedbackStyle.Heavy => 4,
            _ => 0
        };

        float strength = style switch
        {
            FeedbackStyle.Push => ((float)ButtonFeedbackLevel + 0.5f) / ((float)FeedbackLevel.Level3 + 0.5f),
            FeedbackStyle.Release => (float)ButtonFeedbackLevel / (float)FeedbackLevel.Level3 * 0.75f,
            _ => (float)ForceFeedbackLevel / (float)FeedbackLevel.Level3,
        };

        if (strength < 0.1f) return;
        
        if (position.HasValue)
        {
            position /= (float)UIScreen.MainScreen.NativeScale;
            _feedbackGenerators[index].ImpactOccurred(strength, new CGPoint(position.Value.X, position.Value.Y));
        }
        else
        {
            _feedbackGenerators[index].ImpactOccurred(strength);
        }
    }

    public void Dispose()
    {
        _feedbackGenerators[0].Dispose();
        _feedbackGenerators[1].Dispose();
        _feedbackGenerators[2].Dispose();
        _feedbackGenerators[3].Dispose();
        _feedbackGenerators[4].Dispose();
    }

    
    
}

#endif