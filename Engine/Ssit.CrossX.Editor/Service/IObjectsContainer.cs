using System.Collections.Generic;
using Ssit.CrossX.Editor.Helpers;

namespace Ssit.CrossX.Editor.Service;

public interface IObjectsContainer
{
    IReadOnlyList<string> Categories { get; }
    IReadOnlyList<EditorObject> Objects { get; }
    EditorObject Get(string objId);
}

public interface IImagesContainer
{
    IReadOnlyList<string> Categories { get; }
    IReadOnlyList<EditorImage> Images { get; }
    EditorImage Get(string objId);
}