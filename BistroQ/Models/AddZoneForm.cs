using BistroQ.Validation;

namespace BistroQ.Models;

public class AddZoneForm : ValidatorBase
{
    public AddZoneForm()
    {
        AddZoneNameValidator();
    }

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public void AddZoneNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "ZoneName"),
                v => ValidationRules.StringRules.MinLength(v, 2, "ZoneName")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Name), Name);
    }
}