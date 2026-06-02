namespace Ssit.CrossX.Input;

public interface INativeTextInputConsumer
{
    void OnTextInput(string text);
    void OnTextInputEnd(bool isAccepted);
    void OnTextInputCancel();
}