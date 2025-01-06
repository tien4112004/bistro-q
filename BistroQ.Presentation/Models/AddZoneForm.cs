using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a form for adding new zone with built-in validation rules.
/// Inherits from ValidatorBase to provide validation functionality.
/// </summary>
public class AddZoneForm : ValidatorBase
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the name of the zone.
    /// Validated for non-empty and minimum length (2 chars).
    /// </summary>
    public string Name { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AddZoneForm class.
    /// Sets up the validator for the zone name.
    /// </summary>
    public AddZoneForm()
    {
        AddZoneNameValidator();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds validation rules for the Name property.
    /// Validates that the zone name:
    /// - Is not empty
    /// - Has minimum length of 2 characters
    /// </summary>
    private void AddZoneNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Zone Name"),
                v => ValidationRules.StringRules.MinLength(v, 2, "Zone Name")
            ).Where(r => !r.IsValid).ToList();
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Validates all properties of the form.
    /// Executes validation rules for the zone Name.
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