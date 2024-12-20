using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class NutritionFactViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _calories;

    [ObservableProperty]
    private string? _fat;

    [ObservableProperty]
    private string? _fiber;

    [ObservableProperty]
    private string? _protein;
}