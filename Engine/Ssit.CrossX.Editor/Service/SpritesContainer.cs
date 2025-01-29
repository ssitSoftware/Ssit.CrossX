using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Ssit.CrtossX.Editor.Helpers;
using Breeze.Engine;
using Breeze.Engine.Common;
using Breeze.Engine.Graphics.Sprites;
using Breeze.Engine.Graphics.Sprites.Importers;
using SkiaSharp;
using Ssit.CrossX.Games.Template;

namespace Ssit.CrtossX.Editor.Service;

public class SpritesContainer : ISpritesContainer
{
    private readonly IGameTemplate _gameTemplate;
    private readonly Dictionary<string, EditorSprite> _sprites = new();

    public SpritesContainer(IGameTemplate gameTemplate)
    {
        _gameTemplate = gameTemplate;
    }

    public void Load()
    {
        foreach (var obj in _gameTemplate.Objects)
        {
            LoadSprite(obj.GameObject);
        }
        
        foreach (var img in _gameTemplate.Images)
        {
            LoadSprite(img.GameObject);
        }
    }

    private void LoadSprite(string path)
    {
        if (_sprites.ContainsKey(path)) return;
        
        var imgPath = PathHelper.GetForExtension(path, ".png");

        SKImage image;
        using (var stream = _gameTemplate.AssetsProvider.Open(imgPath))
        {
            image = SKImage.FromEncodedData(stream);
        }

        SpriteSequence[] sequences;
        if (Path.GetExtension(path) == ".png")
        {
            sequences = new[]
            {
                new SpriteSequence("", new[]
                {
                    new SpriteFrame(new Rectangle(0, 0, image.Width, image.Height), new Vector2(image.Width/2, image.Height/2), 1)
                }, Array.Empty<SpriteEvent>())
            };
        }
        else if (Path.GetExtension(path) == ".spr")
        {
            using (var stream = _gameTemplate.AssetsProvider.Open(path))
            {
                sequences = SpriteFormat.LoadSpriteSequences(new BinaryReader(stream));
            }
        }
        else if (Path.GetExtension(path) == ".json")
        {
            using var stream = _gameTemplate.AssetsProvider.Open(path);
            using var defStream = _gameTemplate.AssetsProvider.Open(PathHelper.GetForExtension(path, ".def.json"));

            sequences = AsepriteImporter.LoadSpriteSequences(stream, defStream);
        }
        else throw new InvalidDataException();
        
        _sprites.Add(path, new EditorSprite(image, sequences));
    }

    public EditorSprite Get(string name)
    {
        _sprites.TryGetValue(name, out var sprite);
        return sprite;
    }
}