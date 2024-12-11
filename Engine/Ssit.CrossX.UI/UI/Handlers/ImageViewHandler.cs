using System;
using Ssit.CrossX.IoC;
using Ssit.CrossX.UI.Views;

namespace Ssit.CrossX.UI.Handlers;

public class ImageViewHandler : ViewHandler<ImageView>
{
    private readonly IIoCContainer _container;

    public ImageViewHandler(CreateHandlerParameters parameters, IIoCContainer container) : base(parameters)
    {
        if (AttachedView.Source is null)
        {
            throw new InvalidOperationException("The image view is missing a Source.");
        }

        _container = container;

        AttachedView.Source.ImageChanged += OnImageChanged;
    }

    private void OnImageChanged()
    {
        var img = AttachedView.Source.GetTexture(_container);
        if (img is null)
        {
            return;
        }
    }

    protected override void OnDispose(bool disposing)
    {
        base.OnDispose(disposing);
        AttachedView.Source.ImageChanged += OnImageChanged;
    }
}