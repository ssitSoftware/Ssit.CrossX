using Ssit.IoC;

namespace Ssit.CrossX.UI.Services;

public delegate void InitializeUiDelegate(IIoCContainerBuilder builder, INavigationMap navigationMap, IHandlerMapper mapper);