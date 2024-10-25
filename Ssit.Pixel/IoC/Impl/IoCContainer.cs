using System;
using System.Collections.Generic;

namespace Ssit.Pixel.Framework.IoC.Impl;

internal class IoCContainer : IIoCContainer
{
    public IIoCContainer Parent { get; set; }

    private readonly Dictionary<Type, object> _instances = new();

    private readonly Dictionary<Type, Type> _implementations = new();

    public void Register(Type type, object instance) => _instances.Add(type, instance);
        
    public void RegisterImplementation(Type type, Type impl) => _implementations.Add(type, impl);
        
    public void Dispose()
    {
        var list = new List<IDisposable>();
        
        foreach (var inst in _instances)
        {
            if (inst.Value is IDisposable disposable && !ReferenceEquals(disposable, this))
            {
                if (!list.Contains(disposable))
                {
                    disposable.Dispose();
                    list.Add(disposable);
                }
            }
        }
    }

    public TType Get<TType>()
    {
        if (!TryGet<TType>(out var instance))
        {
            throw new KeyNotFoundException();
        }

        return instance;
    }

    public object Get(Type type)
    {
        if (!TryGet(type, out var instance))
        {
            throw new KeyNotFoundException();
        }

        return instance;
    }

    public bool TryGet<TType>(out TType instance)
    {
        if (TryGet(typeof(TType), out var @object))
        {
            instance = (TType) @object;
            return true;
        }

        instance = default;
        return false;
    }

    public bool TryGet(Type type, out object instance)
    {
        if (_instances.TryGetValue(type, out instance))
        {
            return true;
        }

        return Parent?.TryGet(type, out instance) ?? false;
    }

    public TType IoCConstruct<TType>(object parameters = null) => (TType) IoCConstruct(typeof(TType), parameters);

    public object IoCConstruct(Type type, object parameters = null)
    {
        if (type.IsAbstract)
        {
            type = ResolveImplementation(type);
        }

        return ObjectCreationHelper.CreateObject(type, parameters, TryGet);
    }

    public Type ResolveImplementation(Type abstractType)
    {
        if (_implementations.TryGetValue(abstractType, out var type))
        {
            return type;
        }
            
        if (abstractType.IsGenericTypeDefinition && _implementations.TryGetValue(abstractType.GetGenericTypeDefinition(), out type))
        {
            return type.MakeGenericType(abstractType.GetGenericArguments());
        }

        if (Parent is null)
        {
            throw new KeyNotFoundException();
        }

        return Parent.ResolveImplementation(abstractType);
    }

    public Type ResolveImplementation<TType>() => ResolveImplementation(typeof(TType));
}