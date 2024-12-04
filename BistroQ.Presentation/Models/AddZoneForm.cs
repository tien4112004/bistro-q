using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

public class AddZoneForm : ValidatorBase
{
    public AddZoneForm()
    {
        AddZoneNameValidator();
    }
    public string Name { get; set; }

    public void AddZoneNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Zone Name"),
                v => ValidationRules.StringRules.MinLength(v, 2, "Zone Name")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Name), Name);
    }
}