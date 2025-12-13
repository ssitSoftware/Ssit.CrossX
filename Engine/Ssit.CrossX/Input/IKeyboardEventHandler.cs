namespace Ssit.CrossX.Input;

public interface IKeyboardEventHandler
{
    void OnKeyDown( Key key );
    void OnKeyUp( Key key );
}