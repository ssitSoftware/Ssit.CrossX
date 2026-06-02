#if ANDROID
using Ssit.CrossX.Input;

namespace Ssit.CrossX.SDL.Droid.Input;

public class NativeTextInputDroid : INativeTextInput
{
    private readonly INativeTextInputConsumer _consumer;

    public NativeTextInputDroid(INativeTextInputConsumer consumer, InputType inputType)
    {
        _consumer = consumer;
    }

    public void Dispose()
    {
        _consumer.OnTextInputCancel();
    }
}
#endif