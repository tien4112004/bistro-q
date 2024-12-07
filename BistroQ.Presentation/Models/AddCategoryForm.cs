using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

public class AddCategoryForm : ValidatorBase
{
    public AddCategoryForm()
    {
        AddCategoryNameValidator();
    }
    public string Name { get; set; }

    public void AddCategoryNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Category Name"),
                v => ValidationRules.StringRules.MinLength(v, 2, "Category Name")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Name), Name);
    }
}