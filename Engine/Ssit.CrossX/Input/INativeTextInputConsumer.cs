namespace Ssit.CrossX.Input;

public interface INativeTextInputConsumer
{
    void OnTextInput(string text);
    void OnTextInputClosed();
    RectangleF TextInputBounds { get; }
    bool OnKey(Key key);
}