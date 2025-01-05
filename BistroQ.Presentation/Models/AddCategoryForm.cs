using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a form for adding new category with built-in validation rules.
/// Inherits from ValidatorBase to provide validation functionality.
/// </summary>
public class AddCategoryForm : ValidatorBase
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the name of the category.
    /// Validated for non-empty and minimum length (2 chars).
    /// </summary>
    public string Name { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AddCategoryForm class.
    /// Sets up the validator for the category name.
    /// </summary>
    public AddCategoryForm()
    {
        AddCategoryNameValidator();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds validation rules for the Name property.
    /// Validates that the category name:
    /// - Is not empty
    /// - Has minimum length of 2 characters
    /// </summary>
    private void AddCategoryNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Category Name"),
                v => ValidationRules.StringRules.MinLength(v, 2, "Category Name")
            ).Where(r => !r.IsValid).ToList();
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Validates all properties of the form.
    /// Executes validation rules for the category Name.
    /// </summary>
    /// <remarks>
    /// This method overrides the base ValidateAll method to provide
    /// form-specific validation logic.
    /// </remarks>
    public override void ValidateAll()
    {
        ValidateProperty(nameof(Name), Name);
    }
    #endregion
}