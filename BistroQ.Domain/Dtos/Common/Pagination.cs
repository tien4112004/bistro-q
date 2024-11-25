using System.ComponentModel;

namespace BistroQ.Domain.Dtos;
public class Pagination : INotifyPropertyChanged
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public event PropertyChangedEventHandler PropertyChanged;
}
