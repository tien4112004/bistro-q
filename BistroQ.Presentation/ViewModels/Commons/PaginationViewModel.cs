using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Commons;

public partial class PaginationViewModel : ObservableObject
{
    private int _currentPage;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (double.IsNaN(value) || value <= 0)
            {
                value = 1;
            }
            else
            {
                _currentPage = value;
            }
        }
    }

    [ObservableProperty]
    private int _totalPages = 0;

    [ObservableProperty]
    private int _totalItems = 0;

    [ObservableProperty]
    private int _pageSize = 10;
}