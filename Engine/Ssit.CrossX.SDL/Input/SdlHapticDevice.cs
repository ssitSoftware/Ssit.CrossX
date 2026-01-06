#if !IOS
using System.Numerics;
using SDL;
using Ssit.CrossX.Input;
using Ssit.CrossX.SDL.Common;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Input;

public class SdlHapticDevice: IHapticDevice
{
    public FeedbackLevel ButtonFeedbackLevel { get; set; } = FeedbackLevel.Level1;
    public FeedbackLevel ForceFeedbackLevel { get; set; } = FeedbackLevel.Level2;
    
    readonly SdlHandle<SDL_Haptic> _haptic;

    public unsafe SdlHapticDevice()
    {
        using var array = SDL_GetHaptics();
        foreach(var id in array!)
        {
            var hapticPtr = SDL_OpenHaptic(id);
            
            if (hapticPtr == null)
                continue;
            
            if (SDL_InitHapticRumble(hapticPtr) == true)
            {
                _haptic = new SdlHandle<SDL_Haptic>(hapticPtr);
                break;
            }
            SDL_CloseHaptic(hapticPtr);
        }
    }

    
    public void Feedback(FeedbackStyle style, Vector2? _) => Rumble(style);

    private unsafe void Rumble(FeedbackStyle style)
    {
        if (_haptic == null || _haptic.Pointer == null)
            return;

        uint timeInMs = 100;
        float strength = 1;
        
        switch (style)
        {
            case FeedbackStyle.Push:
                timeInMs = 50;
                strength = 0.1f * (int)ButtonFeedbackLevel;
                break;
            
            case FeedbackStyle.Release:
                timeInMs = 50;
                strength = 0.05f * (int)ButtonFeedbackLevel;
                break;
            
            case FeedbackStyle.Light:
                timeInMs = 100;
                strength = 0.1f * (int)ForceFeedbackLevel;
                break;
            
            case FeedbackStyle.Medium:
                timeInMs = 150;
                strength = 0.15f * (int)ForceFeedbackLevel;
                break;
            
            case FeedbackStyle.Heavy:
                timeInMs = 200;
                strength = 0.25f * (int)ForceFeedbackLevel;
                break;
        }

        if (strength < 0.001f) return;
        
        SDL_PlayHapticRumble(_haptic.Pointer, strength, timeInMs);
    }

    public unsafe void Dispose()
    {
        if (_haptic != null && _haptic.Pointer != null)
        {
            SDL_CloseHaptic(_haptic.Pointer);
        }
    }
}
#endif