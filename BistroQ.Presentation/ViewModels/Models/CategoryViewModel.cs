using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class CategoryViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _name;
    
    private int? _categoryId = null;
    public int? CategoryId
    {
        get => _categoryId;
    }
}