using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a login form with validation capabilities.
/// Inherits from ValidatorBase to provide validation functionality.
/// </summary>
public class LoginForm : ValidatorBase
{
    #region Private Fields
    /// <summary>
    /// Backing field for Username property.
    /// </summary>
    private string _username = string.Empty;

    /// <summary>
    /// Backing field for Password property.
    /// </summary>
    private string _password = string.Empty;
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets the username for login.
    /// Validated for non-empty and minimum length (3 chars).
    /// Triggers validation and property change notification on set.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the password for login.
    /// Validated for non-empty value.
    /// Triggers validation and property change notification on set.
    /// </summary>
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
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the LoginForm class.
    /// Sets up validators for username and password.
    /// </summary>
    public LoginForm()
    {
        AddUsernameValidator();
        AddPasswordValidator();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds validation rules for the Username property.
    /// Validates that the username:
    /// - Is not empty
    /// - Has minimum length of 3 characters
    /// </summary>
    private void AddUsernameValidator()
    {
        AddValidator(nameof(Username), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Username"),
                v => ValidationRules.StringRules.MinLength(v, 3, "Username")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Password property.
    /// Validates that the password:
    /// - Is not empty
    /// </summary>
    private void AddPasswordValidator()
    {
        AddValidator(nameof(Password), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Password")
            ).Where(r => !r.IsValid).ToList();
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Validates all properties of the form.
    /// Executes validation rules for Username and Password.
    /// </summary>
    /// <remarks>
    /// This method overrides the base ValidateAll method to provide
    /// form-specific validation logic.
    /// </remarks>
    public override void ValidateAll()
    {
        ValidateProperty(nameof(Username), Username);
        ValidateProperty(nameof(Password), Password);
    }
    #endregion
}