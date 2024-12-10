using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.UI.Services;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public abstract class ChildrenContainerHandler<TContainer> 
    : BackgroundHandler<TContainer>, IViewParent where TContainer : ChildrenContainer
{
    private readonly IHandlerMapper _handlerMapper;
    private readonly Dictionary<Guid, ViewHandler> _childrenHandlers = new();
    protected readonly HashSet<View> RecalculateChildren = new();
    
    private bool _hasUnprocessedRemovedChildren;
    private bool _hasUnprocessedAddedChildren;

    protected RectangleF CalculateTargetBounds(RectangleF bounds)
    {
        var padding = AttachedView.Padding;

        var left = padding.Left?.Calculate(bounds.Width) ?? 0;
        var right = padding.Right?.Calculate(bounds.Width) ?? 0;
        
        var top = padding.Top?.Calculate(bounds.Height) ?? 0;
        var bottom = padding.Bottom?.Calculate(bounds.Height) ?? 0;

        return new RectangleF(left + bounds.X, top + bounds.Y, bounds.Width - right - left, bounds.Height - bottom - top);
    }
    
    protected ChildrenContainerHandler(CreateHandlerParameters parameters, IHandlerMapper handlerMapper) : base(parameters)
    {
        _handlerMapper = handlerMapper;
        
        if (AttachedView.Children is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += OnCollectionChanged;
        }

        if (AttachedView.LayoutSignal is not null)
        {
            AttachedView.LayoutSignal.Signal += RecalculateLayout;
        }

        UpdateCollection(true, false);
    }

    public override void Draw(IRenderer renderer)
    {
        base.Draw(renderer);
        RecalculateChildrenLayouts();

        for (var idx = 0; idx < AttachedView.Children.Count; idx++)
        {
            var child = AttachedView.Children[idx];
            var handlerView = (IHandlerView)child;
            handlerView.Handler.Draw(renderer);
        }
    }

    public override void Update(float dt)
    {
        UpdateCollection(_hasUnprocessedAddedChildren, _hasUnprocessedRemovedChildren);
        RecalculateChildrenLayouts();

        for (var idx = 0; idx < AttachedView.Children.Count; idx++)
        {
            var child = AttachedView.Children[idx];
            var handlerView = (IHandlerView)child;
            handlerView.Handler.Update(dt);
        }
    }

    public override void SetBounds(RectangleF rectangleF)
    {
        base.SetBounds(rectangleF);
        RecalculateChildren.UnionWith(AttachedView.Children);
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        
        if (AttachedView.Children is INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= OnCollectionChanged;
        }

        if (AttachedView.LayoutSignal is not null)
        {
            AttachedView.LayoutSignal.Signal -= RecalculateLayout;
        }
    }

    private void UpdateCollection(bool added, bool removed)
    {
        if (added)
        {
            foreach (var child in AttachedView.Children)
            {
                if (!_childrenHandlers.ContainsKey(child.Guid))
                {
                    CreateChild(child);
                }
            }
        }

        if (removed)
        {
            var toRemove = new List<(Guid, ViewHandler)>();
            foreach (var handler in _childrenHandlers.Values)
            {
                if (!AttachedView.Children.Contains(handler.View))
                {
                    toRemove.Add((handler.View.Guid, handler));
                }
            }
            
            foreach (var pair in toRemove)
            {
                _childrenHandlers.Remove(pair.Item1);
                pair.Item2.Dispose();
            }
        }
    }

    private void OnCollectionChanged(object _, NotifyCollectionChangedEventArgs args)
    {
        _hasUnprocessedRemovedChildren = args.Action is NotifyCollectionChangedAction.Remove
            or NotifyCollectionChangedAction.Replace or NotifyCollectionChangedAction.Reset;

        _hasUnprocessedAddedChildren = args.Action is NotifyCollectionChangedAction.Remove 
            or NotifyCollectionChangedAction.Replace or NotifyCollectionChangedAction.Reset;
    }

    private void CreateChild(View child)
    {
        var handler = _handlerMapper.Create(child, this);
        _childrenHandlers.Add(child.Guid, handler);
        RecalculateChildren.Add(child);
    }
    
    protected abstract void RecalculateChildrenLayouts();
    
    public void RecalculateLayout(View view = null)
    {
        if (view is null)
        {
            RecalculateChildren.UnionWith(AttachedView.Children);
        }
        else
        {
            RecalculateChildren.Add(view);
        }
    }
}