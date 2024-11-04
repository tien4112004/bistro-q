using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Models;

public class LoginForm : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();
    private string _username = string.Empty;
    private string _password = string.Empty;
    public Dictionary<string, List<string>> Errors => _errors;

    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                ValidateUsername();
                OnPropertyChanged();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                ValidatePassword();
                OnPropertyChanged();
            }
        }
    }

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName != null && _errors.ContainsKey(propertyName)
            ? _errors[propertyName]
            : Enumerable.Empty<string>();
    }

    public void ValidateUsername()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(_username))
        {
            errors.Add("Username is required");
        }
        else if (_username.Length < 3)
        {
            errors.Add("Username must be at least 3 characters long");
        }

        UpdateErrors(nameof(Username), errors);
    }

    public void ValidatePassword()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(_password))
        {
            errors.Add("Password is required");
        }

        UpdateErrors(nameof(Password), errors);
    }

    public void ResetError(string propertyName)
    {
        UpdateErrors(propertyName, new List<string>());
    }

    private void UpdateErrors(string propertyName, List<string> errors)
    {
        if (errors.Any())
            _errors[propertyName] = errors;
        else
            _errors.Remove(propertyName);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(Errors));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public void ValidateAll()
    {
        ValidateUsername();
        ValidatePassword();
    }
}