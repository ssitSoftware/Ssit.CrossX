using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;
using Ssit.IoC;
using Ssit.CrossX.UI.Services;

namespace Ssit.CrossX.UI.Values;

public sealed class ImageSource: IDisposable
{
    public static implicit operator ImageSource(string path) => new(path);
    public static implicit operator ImageSource(Uri uri) => new(uri);
    
    private string _resourcePath;
    private Uri _resourceUri;
    
    private Task<ResourceHandle<ITexture>> _loadTextureTask;
    private CancellationTokenSource _cancellationTokenSource;
    
    private readonly object _lock = new();
    
    private ResourceHandle<ITexture> _texture;

    public event Action ImageChanged;

    private bool _reload;

    public bool IsLoading
    {
        get
        {
            lock (_lock)
            {
                return _loadTextureTask is not null;
            }
        }
    }
    
    public ResourceHandle<ITexture> Texture
    {
        set
        {
            lock (_lock)
            {
                if (_loadTextureTask != null)
                {
                    throw new InvalidOperationException("Cannot change image while loading image in background.");
                }
                
                _texture?.Dispose();
                _texture = value;
                ImageChanged?.Invoke();
            }
        }
    }
    
    public ResourceHandle<ITexture> GetTexture(IIoCContainer container)
    {
        lock (_lock)
        {
            if (_texture == null || _reload)
            {
                if (_loadTextureTask == null)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    _loadTextureTask = LoadTextureFromParameters(container, _cancellationTokenSource.Token);
                }

                _reload = false;
            }

            if (_loadTextureTask is { IsCompleted: true })
            {
                if (_loadTextureTask.IsCompletedSuccessfully)
                {
                    _texture?.Dispose();
                    _texture = _loadTextureTask.Result;
                }
                _loadTextureTask = null;
                _cancellationTokenSource = null;
            }

            return _texture;
        }
    }

    public void SetSource(string path)
    {
        lock (_lock)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _loadTextureTask = null;
            
            _resourcePath = path;
            _reload = true;
        }
    }

    public void SetSource(Uri uri)
    {
        lock (_lock)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _loadTextureTask = null;
            
            _resourceUri = uri;
            _reload = true;
        }
    }
    
    private ImageSource(string resourcePath)
    {
        _resourcePath = resourcePath;
    }

    private ImageSource(Uri uri)
    {
        _resourceUri = uri;
    }

    private async Task<ResourceHandle<ITexture>> LoadTextureFromParameters(IIoCContainer container, CancellationToken cancellationToken)
    {
        ResourceHandle<ITexture> result = null;
        if (_resourcePath is not null)
        {
            result = await LoadContent(container, cancellationToken);
        }

        if (_resourceUri is not null)
        {
            result = await LoadFromUri(container, cancellationToken);
        }

        lock (_lock)
        {
            if (result is not null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    result.Dispose();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                container.Get<IActionDispatcher>().Enqueue(() => ImageChanged?.Invoke());
                return result;
            }
        }

        throw new InvalidOperationException("Cannot load image from resource path.");
    }

    private Task<ResourceHandle<ITexture>> LoadContent(IIoCContainer container, CancellationToken _)
    {
        ResourceHandle<ITexture> result = null;
        lock (_lock)
        {
            if (_resourcePath is not null)
            {
                result = container.Get<IContentManager>().Get<ITexture>(_resourcePath);
                _resourcePath = null;
            }
        }

        if (result is not null)
        {
            return Task.FromResult(result);
        }
        
        return Task.FromException<ResourceHandle<ITexture>>(new InvalidProgramException("Cannot load image from resource path."));
    }
    
    private async Task<ResourceHandle<ITexture>> LoadFromUri(IIoCContainer container,
        CancellationToken cancellationToken)
    {
        Uri uri;
        lock (_lock)
        {
            uri = _resourceUri;
            _resourceUri = null;
        }

        var memoryStream = new MemoryStream();
        using (HttpClient client = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            cancellationToken.ThrowIfCancellationRequested();
            
            var response = await client.SendAsync(request, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                cancellationToken.ThrowIfCancellationRequested();
                await stream.CopyToAsync(memoryStream, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        if (memoryStream.Length == 0)
        {
            throw new InvalidOperationException("Cannot load image from resource path.");
        }
        
        memoryStream.Seek(0, SeekOrigin.Begin);
        var texture = container.IoCConstruct<ITexture>(new LoadTextureParameters
        {
            DiffuseMapStream = memoryStream
        });

        if (cancellationToken.IsCancellationRequested)
        {
            texture.Dispose();
            cancellationToken.ThrowIfCancellationRequested();
        }
        
        return new ResourceHandleUnmanaged<ITexture>(texture, uri.AbsoluteUri);
    }

    public void Dispose()
    {
        var tokenSrc = _cancellationTokenSource;
        var disposable = _texture;
        
        lock (_lock)
        {
            _texture = null;
            _cancellationTokenSource = null;
        }

        tokenSrc?.Cancel();
        disposable?.Dispose();
    }
}