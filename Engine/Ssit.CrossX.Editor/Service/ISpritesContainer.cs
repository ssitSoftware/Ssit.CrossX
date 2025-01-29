using Ssit.CrossX.Editor.Helpers;

namespace Ssit.CrossX.Editor.Service;

public interface ISpritesContainer
{
    EditorSprite Get(string name);
}