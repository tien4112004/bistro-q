using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

public class AddAccountForm : ValidatorBase
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public int? TableId { get; set; }
    public int? ZoneId { get; set; }

    public AddAccountForm()
    {
        AddUsernameValidator();
        AddPasswordValidator();
        AddRoleValidator();
    }

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

    private void AddRoleValidator()
    {
        AddValidator(nameof(Role), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Role")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Username), Username);
        ValidateProperty(nameof(Password), Password);
        ValidateProperty(nameof(Role), Role);
    }
}
