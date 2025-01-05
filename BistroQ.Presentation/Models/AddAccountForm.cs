using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a form for adding new account with built-in validation rules.
/// Inherits from ValidatorBase to provide validation functionality.
/// </summary>
public class AddAccountForm : ValidatorBase
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the username for the new account.
    /// Validated for non-empty, length (3-50 chars), and no whitespace.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password for the new account.
    /// Validated for non-empty and minimum length (6 chars).
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the role for the new account.
    /// Validated for non-empty value.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Gets or sets the associated table ID for the account.
    /// Optional property that can be null.
    /// </summary>
    public int? TableId { get; set; }

    /// <summary>
    /// Gets or sets the associated zone ID for the account.
    /// Optional property that can be null.
    /// </summary>
    public int? ZoneId { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AddAccountForm class.
    /// Sets up all required validators for Username, Password, and Role.
    /// </summary>
    public AddAccountForm()
    {
        AddUsernameValidator();
        AddPasswordValidator();
        AddRoleValidator();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds validation rules for the Username property.
    /// Validates that the username:
    /// - Is not empty
    /// - Has minimum length of 3 characters
    /// - Has maximum length of 50 characters
    /// - Contains no whitespace
    /// </summary>
    private void AddUsernameValidator()
    {
        AddValidator(nameof(Username), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Username"),
                v => ValidationRules.StringRules.MinLength(v, 3, "Username"),
                v => ValidationRules.StringRules.MaxLength(v, 50, "Username"),
                v => ValidationRules.StringRules.NoWhitespace(v, "Username")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Password property.
    /// Validates that the password:
    /// - Is not empty
    /// - Has minimum length of 6 characters
    /// </summary>
    private void AddPasswordValidator()
    {
        AddValidator(nameof(Password), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Password"),
                v => ValidationRules.StringRules.MinLength(v, 6, "Password")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Role property.
    /// Validates that the role:
    /// - Is not empty
    /// </summary>
    private void AddRoleValidator()
    {
        AddValidator(nameof(Role), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Role")
            ).Where(r => !r.IsValid).ToList();
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Validates all properties of the form.
    /// Executes validation rules for Username, Password, and Role.
    /// </summary>
    /// <remarks>
    /// This method overrides the base ValidateAll method to provide
    /// form-specific validation logic.
    /// </remarks>
    public override void ValidateAll()
    {
        ValidateProperty(nameof(Username), Username);
        ValidateProperty(nameof(Password), Password);
        ValidateProperty(nameof(Role), Role);
    }
    #endregion
}