#if ANDROID

using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Droid.Input;

public class NativeTextInputServiceDroid: INativeTextInputService
{
    public INativeTextInput AllocateTextInput(INativeTextInputConsumer consumer, InputType inputType)
    {
        return new NativeTextInputDroid(consumer, inputType);
    }
}

#endif