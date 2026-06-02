namespace Ssit.CrossX.Input;

public interface INativeTextInputService
{
    INativeTextInput AllocateTextInput(INativeTextInputConsumer consumer, InputType inputType);
}