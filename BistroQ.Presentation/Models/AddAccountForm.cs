using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

public class AddAccountForm : ValidatorBase
{
    private string _username;
    private string _password;
    private string _role;
    private int? _tableId;
    private int? _zoneId;

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

    public int? ZoneId
    {
        get => _zoneId;
        set
        {
            if (_zoneId != value)
            {
                _zoneId = value;
                ValidateProperty(nameof(ZoneId), value);
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
