using System.ComponentModel;

namespace BistroQ.Core.Models;
public class Pagination : INotifyPropertyChanged
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

    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int PageSize { get; set; } = 10;

    public event PropertyChangedEventHandler PropertyChanged;
}
