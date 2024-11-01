using System;

namespace Ssit.Utils.Mvvm.Abstractions;

/// <summary>
/// Interface for locating views based on the corresponding view model type.
/// </summary>
public interface IViewLocatorService
{
    /// <summary>
    /// Finds and returns the view type associated with the specified view model type.
    /// </summary>
    /// <param name="viewModelType">The type of the view model for which to locate the view.</param>
    /// <returns>The type of the view associated with the specified view model type.</returns>
    Type FindViewFromViewModel(Type viewModelType);
}