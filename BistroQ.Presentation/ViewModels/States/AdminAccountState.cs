using BistroQ.Domain.Dtos.Account;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for the admin account interface.
/// Handles account data, selection state, and search functionality.
/// </summary>
/// <remarks>
/// Provides special handling for the default admin account to prevent deletion.
/// </remarks>
public partial class AdminAccountState : ObservableObject
{
    /// <summary>
    /// Gets or sets the query parameters for account collection
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private AccountCollectionQueryParams _query = new();

    /// <summary>
    /// Gets or sets the currently selected account
    /// </summary>
    [ObservableProperty]
    private AccountViewModel? _selectedAccount;

    /// <summary>
    /// Gets or sets the collection of account data
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<AccountViewModel> _source = new();

    /// <summary>
    /// Gets or sets whether data is currently being loaded
    /// </summary>
    [ObservableProperty]
    private bool _isLoading;

    /// <summary>
    /// Gets or sets the search text for filtering accounts
    /// </summary>
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

    /// <summary>
    /// Gets whether an account is selected and can be edited
    /// </summary>
    public bool CanEdit => SelectedAccount != null;

    /// <summary>
    /// Gets whether the selected account can be deleted
    /// </summary>
    /// <remarks>
    /// Prevents deletion of the default admin account
    /// </remarks>
    public bool CanDelete => SelectedAccount != null && !IsDefaultAdmin;

    /// <summary>
    /// Gets whether the selected account is the default admin
    /// </summary>
    private bool IsDefaultAdmin => SelectedAccount?.Role == "Admin" &&
                                 SelectedAccount?.Username?.ToLower() == "admin";

    /// <summary>
    /// Resets the state to default values
    /// </summary>
    public void Reset()
    {
        SelectedAccount = null;
        Source.Clear();
        IsLoading = false;
        Query = new AccountCollectionQueryParams();
    }

    /// <summary>
    /// Returns to the first page of results
    /// </summary>
    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}