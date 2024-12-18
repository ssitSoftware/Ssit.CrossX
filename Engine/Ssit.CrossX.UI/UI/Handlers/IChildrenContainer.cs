using System.Collections.Generic;

namespace Ssit.CrossX.UI.Handlers;

public interface IChildrenContainer
{
    IReadOnlyList<ViewHandler> Children { get; }
}