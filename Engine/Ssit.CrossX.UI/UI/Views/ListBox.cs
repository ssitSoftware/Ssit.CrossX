using System;
using System.Collections.Generic;

namespace Ssit.CrossX.UI.Views;

public class ListBox<TModel>: View
{
    public IReadOnlyList<TModel> Items { get; set; }
    public Func<TModel, View> ItemTemplate { get; set; }
}