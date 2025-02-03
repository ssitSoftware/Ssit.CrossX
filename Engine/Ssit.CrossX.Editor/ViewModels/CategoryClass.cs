using System;
using Ssit.CrossX.Games;
using Ssit.CrossX.Utils;

namespace Ssit.CrossX.Editor.ViewModels;

public class CategoryClass: BindableModel
{
    private readonly Action _onChanged;
    private bool _isSelected;
    public string Name { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (SetField(ref _isSelected, value))
            {
                _onChanged?.Invoke();
            }
        }
    }

    public CategoryClass(string name, Action onChanged)
    {
        _onChanged = onChanged;
        Name = name;
    }
}