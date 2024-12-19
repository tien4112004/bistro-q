using BistroQ.Domain.Dtos.Account;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class AdminAccountState : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private AccountCollectionQueryParams _query = new();

    [ObservableProperty]
    private AccountViewModel? _selectedAccount;

    [ObservableProperty]
    private ObservableCollection<AccountViewModel> _source = new();

    [ObservableProperty]
    private bool _isLoading;

    public string SearchText
    {
        get => Query.Username ?? string.Empty;
        set
        {
            if (Query.Username != value)
            {
                Query.Username = value;
                OnPropertyChanged();
            }
        }
    }

    public bool CanEdit => SelectedAccount != null;
    public bool CanDelete => SelectedAccount != null && !IsDefaultAdmin;

    // Prevent deletion of default admin account
    private bool IsDefaultAdmin => SelectedAccount?.Role == "Admin" &&
                                 SelectedAccount?.Username?.ToLower() == "admin";

    public void Reset()
    {
        SelectedAccount = null;
        Source.Clear();
        IsLoading = false;
        Query = new AccountCollectionQueryParams();
    }

    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}