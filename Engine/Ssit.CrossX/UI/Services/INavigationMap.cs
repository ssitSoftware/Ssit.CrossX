using System.Diagnostics.CodeAnalysis;

namespace Ssit.CrossX.UI.Services;

public interface INavigationMap
{
    INavigationMap Map<TViewModel, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TPage>()
        where TViewModel : class where TPage : Page<TViewModel>;
}