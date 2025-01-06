using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels;

/// <summary>
/// ViewModel responsible for handling user login functionality.
/// Manages login form state, validation, and authentication process.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    #region Events
    /// <summary>
    /// Event triggered when navigation is requested after successful login.
    /// </summary>
    public event EventHandler NavigationRequested;

    /// <summary>
    /// Event triggered when the login window needs to be closed.
    /// </summary>
    public event EventHandler CloseRequested;
    #endregion

    #region Private Fields
    /// <summary>
    /// Service responsible for authentication operations.
    /// </summary>
    private readonly IAuthService _authService;

    /// <summary>
    /// Flag indicating whether a login operation is in progress.
    /// </summary>
    [ObservableProperty]
    private bool _isProcessing = false;

    /// <summary>
    /// The login form containing username and password data.
    /// </summary>
    [ObservableProperty]
    private LoginForm _form = new();

    /// <summary>
    /// Message displayed when login errors occur.
    /// </summary>
    [ObservableProperty]
    private string _errorMessage = "";
    #endregion

    #region Public Properties
    /// <summary>
    /// Command that executes the login operation.
    /// </summary>
    public ICommand LoginCommand { get; }

    /// <summary>
    /// Command that handles form field changes and validates them.
    /// </summary>
    public ICommand FormChangeCommand { get; }
    #endregion

    #region Constructor
    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        LoginCommand = new AsyncRelayCommand(async () =>
        {
            await Login();
        });
        FormChangeCommand = new RelayCommand<(string Field, string Value)>((param) =>
        {
            Form.ValidateProperty(param.Field, param.Value);
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Checks if login operation can proceed by validating all form fields.
    /// </summary>
    /// <returns>True if the form has no validation errors; otherwise, false.</returns>
    public bool CanLogin()
    {
        Form.ValidateAll();
        return !Form.HasErrors;
    }

    /// <summary>
    /// Attempts to authenticate the user with the provided credentials.
    /// Sets error message if login fails.
    /// </summary>
    /// <returns>A task representing the asynchronous login operation.</returns>
    public async Task Login()
    {
        if (!CanLogin())
        {
            return;
        }
        try
        {
            IsProcessing = true;
            ErrorMessage = string.Empty;
            var result = await _authService.LoginAsync(Form.Username, Form.Password);
            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return;
            }
            RequestNavigation();
        }
        catch (Exception)
        {
            ErrorMessage = "An error occurred. Please try again.";
        }
        finally
        {
            IsProcessing = false;
        }
    }

    /// <summary>
    /// Raises the NavigationRequested event to trigger navigation after successful login.
    /// </summary>
    public void RequestNavigation() =>
        NavigationRequested?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Raises the CloseRequested event to close the login window.
    /// </summary>
    public void RequestClose() =>
        CloseRequested?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Checks if the user is currently authenticated.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// true if the user is authenticated; otherwise, false.
    /// </returns>
    public async Task<bool> IsAuthenticated()
    {
        return await _authService.IsAuthenticatedAsync();
    }
    #endregion
}