using System.ComponentModel;

namespace BistroQ.Core.Models;
public class Pagination : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}
