using System;
using System.Numerics;

namespace Ssit.CrossX.Input;

public interface IHapticDevice: IDisposable
{
    FeedbackLevel ButtonFeedbackLevel { get; set; }
    FeedbackLevel ForceFeedbackLevel { get; set; }

    void Feedback(FeedbackStyle style, Vector2? position = null);
}