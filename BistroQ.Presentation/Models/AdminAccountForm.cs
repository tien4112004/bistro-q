namespace BistroQ.Presentation.Models;

public class AddAccountForm : ValidatorBase
{
    private string _username;
    private string _password;
    private string _role;
    private int? _tableId;

    public AddAccountForm()
    {
        AddUsernameValidator();
        AddPasswordValidator();
        AddRoleValidator();
    }

    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                ValidateProperty(nameof(Username), value);
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
            }
        }
    }

    public string Role
    {
        get => _role;
        set
        {
            if (_role != value)
            {
                _role = value;
                ValidateProperty(nameof(Role), value);
            }
        }
    }

    public int? TableId
    {
        get => _tableId;
        set
        {
            if (_tableId != value)
            {
                _tableId = value;
                ValidateProperty(nameof(TableId), value);
            }
        }
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
                v => ValidationRules.StringRules.MinLength(v, 6, "Password"),
                v => ValidationRules.PasswordRules.RequireUppercase(v),
                v => ValidationRules.PasswordRules.RequireLowercase(v),
                v => ValidationRules.PasswordRules.RequireDigit(v),
                v => ValidationRules.PasswordRules.RequireSpecialChar(v)
            ).Where(r => !r.IsValid).ToList();
        });
    }

    private void AddRoleValidator()
    {
        AddValidator(nameof(Role), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Role"),
                v => ValidationRules.EnumRules.IsValidRole(v)
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Username), Username);
        ValidateProperty(nameof(Password), Password);
        ValidateProperty(nameof(Role), Role);
        // TableId is optional, so no validation needed
    }
}

// Add these rules to your ValidationRules class if not already present
public static class PasswordRules
{
    public static ValidationResult RequireUppercase(string value)
    {
        return new ValidationResult
        {
            IsValid = value?.Any(char.IsUpper) ?? false,
            Message = "Password must contain at least one uppercase letter"
        };
    }

    public static ValidationResult RequireLowercase(string value)
    {
        return new ValidationResult
        {
            IsValid = value?.Any(char.IsLower) ?? false,
            Message = "Password must contain at least one lowercase letter"
        };
    }

    public static ValidationResult RequireDigit(string value)
    {
        return new ValidationResult
        {
            IsValid = value?.Any(char.IsDigit) ?? false,
            Message = "Password must contain at least one digit"
        };
    }

    public static ValidationResult RequireSpecialChar(string value)
    {
        return new ValidationResult
        {
            IsValid = value?.Any(c => !char.IsLetterOrDigit(c)) ?? false,
            Message = "Password must contain at least one special character"
        };
    }
}

public static class EnumRules
{
    public static ValidationResult IsValidRole(string value)
    {
        var validRoles = new[] { "Admin", "Staff", "Customer" };
        return new ValidationResult
        {
            IsValid = validRoles.Contains(value),
            Message = "Invalid role selected"
        };
    }
}