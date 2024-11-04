using BistroQ.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Models;

public class LoginForm : ValidatorBase
{
    public LoginForm()
    {
        AddUsernameValidator();
        AddPasswordValidator();
    }

    private string _username = string.Empty;
    private string _password = string.Empty;

    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                ValidateProperty(nameof(Username), value);
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
                ValidateProperty(nameof(Password), value);
                OnPropertyChanged();
            }
        }
    }

    public void AddUsernameValidator()
    {
        AddValidator(nameof(Username), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Username"),
                v => ValidationRules.StringRules.MinLength(v, 3, "Username")
            ).FirstOrDefault(r => !r.IsValid);
        });
    }

    public void AddPasswordValidator() {
        AddValidator(nameof(Password), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Password")
            ).FirstOrDefault(r => !r.IsValid);
        });
    }

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Username), Username);
        ValidateProperty(nameof(Password), Password);
    }
}