using System;
using System.Collections.Generic;
using System.Linq;
using Ssit.CrossX.Editor.Helpers;
using Ssit.CrossX.Games;

namespace Ssit.CrossX.Editor.ViewModels;

public class ImageSelectorViewModel : BindableModel
{
    public class Parameters
    {
        public Action<EditorImage> SelectionChanged;
        public IReadOnlyList<EditorImage> Images;
        public IReadOnlyList<string> Categories;
    }
    
    private readonly Parameters _parameters;

    private IList<EditorImage> _objects;
    private EditorImage _selectedObject;
    public CategoryClass[] Categories { get; }

    public IList<EditorImage> Objects
    {
        get => _objects;
        private set => SetField(ref _objects, value);
    }

    public EditorImage SelectedObject
    {
        get => _selectedObject;
        set
        {
            if (SetField(ref _selectedObject, value))
            {
                _parameters.SelectionChanged?.Invoke(value);
            }
        }
    }

    public ImageSelectorViewModel(Parameters parameters)
    {
        _parameters = parameters;
        _selectedObject = parameters.Images.FirstOrDefault();
        
        Categories = parameters.Categories.Select(o => new CategoryClass(o, UpdateObjectsList)).ToArray();
        UpdateObjectsList();
    }
    
    private void UpdateObjectsList()
    {
        var selectedCategories = Categories.Where(o => o.IsSelected).Select(o=>o.Name).ToArray();

        if (selectedCategories.Length == 0)
        {
            Objects = _parameters.Images.ToArray();
            return;
        }
        
        Objects = _parameters.Images.Where(o=>o.HasTags(selectedCategories)).ToList();
    }
}