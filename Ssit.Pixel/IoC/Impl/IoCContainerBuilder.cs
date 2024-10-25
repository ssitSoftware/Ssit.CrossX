using System;
using System.Collections.Generic;

namespace Ssit.Pixel.Framework.IoC.Impl;

internal class IoCContainerBuilder: IIoCContainerBuilder
{
    private class SingletonInfo
    {
        public Type Type;
        public object Parameter;
    }
        
    private readonly IoCContainer _container = new();
        
    private readonly Dictionary<Type, SingletonInfo> _singletonTypes = new();

    public IoCContainerBuilder()
    {
        _container.RegisterImplementation(typeof(IIoCContainerBuilder), typeof(IoCContainerBuilder));
        _container.Register(typeof(IIoCContainer), _container);
    }
        
    public IIoCContainerBuilder WithParent(IIoCContainer container)
    {
        _container.Parent = container;
        return this;
    }

    public IIoCContainerBuilder WithInstance<TAbstract>(TAbstract instance) where TAbstract: class
    {
        _container.Register(typeof(TAbstract), instance);
        return this;
    }

    public IIoCContainerBuilder WithInstance(object instance, params Type[] asTypes)
    {
        foreach (var type in asTypes)
        {
            _container.Register(type, instance);
        }
        return this;
    }

    public IIoCContainerBuilder WithSingleton<TAbstract, TImplementation>(object parameter = null)
        where TAbstract : class where TImplementation : class, TAbstract
    {
        _singletonTypes.Add(typeof(TAbstract), new SingletonInfo 
            {
                Type = typeof(TImplementation), 
                Parameter = parameter
            }
        );
        return this;
    }
        
    public IIoCContainerBuilder WithImplementation(Type @abstract, Type implementation)
    {
        _container.RegisterImplementation(@abstract, implementation);
        return this;
    }

    public IIoCContainerBuilder WithImplementation<TAbstract, TImplementation>()
        where TAbstract : class where TImplementation : class, TAbstract
    {
        _container.RegisterImplementation(typeof(TAbstract), typeof(TImplementation));
        return this;
    }

    public bool IsRegistered(Type type)
    {
        if (_singletonTypes.TryGetValue(type, out _))
        {
            return true;
        }

        if (_container.TryGet(type, out _))
        {
            return true;
        }

        try
        {
            _container.ResolveImplementation(type);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public IIoCContainer Build()
    {
        foreach (var type in _singletonTypes)
        {
            if (type.Value is null)
                continue;
            
            TryGet(type.Key, out var _);
        }

        return _container;
    }
        
    private bool TryGet(Type type, out object instance)
    {
        if (_container.TryGet(type, out instance))
        {
            return true;
        }

        SingletonInfo implType = null;
            
        if (type.IsAbstract)
        {
            if (!_singletonTypes.TryGetValue(type, out implType))
            {
                throw new KeyNotFoundException();
            }

            _singletonTypes[type] = null;
            if ( implType is null) throw new InvalidOperationException($"Circular dependency detected!");
        }

        if (implType is null)
        {
            implType = new SingletonInfo
            {
                Type = type,
                Parameter = null
            };
        }

        try
        {
            instance = ObjectCreationHelper.CreateObject(implType.Type, implType.Parameter, TryGet);
            _container.Register(type, instance);
            return true;
        }
        catch
        {
            return false;
        }
    }
}