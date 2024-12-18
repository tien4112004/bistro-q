using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class NutritionFactViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _calories;
}