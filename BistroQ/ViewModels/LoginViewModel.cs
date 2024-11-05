using BistroQ.Contracts.Services;
using BistroQ.Core.Contracts.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using BistroQ.Models;
using System.Text.Json;

namespace BistroQ.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    public event EventHandler NavigationRequested;
    public event EventHandler CloseRequested;

    private readonly IAuthService _authService;

    [ObservableProperty]
    private bool _isProcessing = false;
    [ObservableProperty]
    private LoginForm _form = new();
    [ObservableProperty]
    private string _errorMessage = "";
    public ICommand LoginCommand { get; }
    public ICommand FormChangeCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        LoginCommand = new AsyncRelayCommand(async () =>
        {
            Debug.WriteLine("Login command executed");
            await Login();
        });

        FormChangeCommand = new RelayCommand<(string Field, string Value)>((param) =>
        {
            Form.ValidateProperty(param.Field, param.Value);
        });
    }
    
    public bool CanLogin()
    {
        Form.ValidateAll();
        return !Form.HasErrors;
    }

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

    public void RequestNavigation() =>
        NavigationRequested?.Invoke(this, EventArgs.Empty);

    public void RequestClose() =>
        CloseRequested?.Invoke(this, EventArgs.Empty);

    public async Task<bool> IsAuthenticated()
    {
        return await _authService.IsAuthenticatedAsync();
    }
}
