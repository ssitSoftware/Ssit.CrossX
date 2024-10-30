using System;

namespace Ssit.Pixel.IoC;

public interface IImplementationMapper
{
    /// <summary>
    /// Resolves the implementation type for a given abstract type TType.
    /// </summary>
    /// <typeparam name="TType">The abstract type whose implementation needs to be resolved.</typeparam>
    /// <returns>The implementation type associated with the abstract type TType.</returns>
    Type ResolveImplementation<TType>(string key = null);

    /// <summary>
    /// Resolves the concrete implementation type for a given abstract type or interface.
    /// </summary>
    /// <param name="type">The abstract type or interface for which to resolve the implementation.</param>
    /// <returns>The concrete implementation type if found; otherwise, throws a KeyNotFoundException.</returns>
    Type ResolveImplementation(Type type, string key = null);
}

/// <summary>
/// Interface representing an Inversion of Control (IoC) Container.
/// Provides methods for resolving dependencies and constructing objects using injected dependencies.
/// </summary>
public interface IIoCContainer: IImplementationMapper, IDisposable
{
    /// Retrieves an instance of the requested type from the IoC container.
    /// <typeparam name="TType">The type of the instance to retrieve.</typeparam>
    /// <returns>An instance of the requested type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the requested type is not registered in the container and cannot be resolved.</exception>
    TType Get<TType>();

    /// <summary>
    /// Resolves an instance of the specified type from the IoC container.
    /// Throws a KeyNotFoundException if the type is not registered in the container.
    /// </summary>
    /// <param name="type">The type of the instance to resolve.</param>
    /// <returns>An instance of the specified type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the specified type is not found in the container.</exception>
    object Get(Type type);

    /// <summary>
    /// Tries to retrieve an instance of the specified type from the IoC container.
    /// </summary>
    /// <typeparam name="TType">The type of the instance to retrieve.</typeparam>
    /// <param name="instance">When this method returns, contains the object from the container if the type was found; otherwise, the default value for the type.</param>
    /// <returns>true if the type was found in the container; otherwise, false.</returns>
    bool TryGet<TType>(out TType instance);

    /// <summary>
    /// Tries to resolve an instance of the specified generic type from the IoC container.
    /// </summary>
    /// <typeparam name="TType">The type of the object to resolve.</typeparam>
    /// <param name="instance">The resolved instance of the specified type if the resolution is successful; otherwise, the default value of the type.</param>
    /// <returns>True if the resolution is successful; otherwise, false.</returns>
    bool TryGet(Type type, out object instance);

    /// <summary>
    /// Constructs an object of the specified type using IoC (Inversion of Control) container with optional parameters.
    /// </summary>
    /// <typeparam name="TType">The type of object to construct.</typeparam>
    /// <param name="parameters">Optional parameters to be used for the construction of the object.</param>
    /// <returns>An instance of the specified type.</returns>
    TType IoCConstruct<TType>(object parameters = null);

    /// <summary>
    /// Constructs an object of the specified type using the Inversion of Control (IoC) pattern,
    /// optionally passing provided parameters to the constructor.
    /// </summary>
    /// <param name="type">The type of the object to construct.</param>
    /// <param name="parameters">Optional parameters to pass to the constructor.</param>
    /// <returns>An object of the specified type.</returns>
    object IoCConstruct(Type type, object parameters = null);
}