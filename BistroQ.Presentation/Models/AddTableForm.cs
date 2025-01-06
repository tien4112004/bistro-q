using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a form for adding new table with validation capabilities.
/// Inherits from ValidatorBase to provide validation functionality.
/// </summary>
public class AddTableForm : ValidatorBase
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the number of seats for the table.
    /// </summary>
    public int SeatsCount { get; set; }

    /// <summary>
    /// Gets or sets the zone identifier where the table is located.
    /// </summary>
    public int ZoneId { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AddTableForm class.
    /// </summary>
    public AddTableForm()
    {
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Validates all properties of the form.
    /// </summary>
    /// <remarks>
    /// This method overrides the base ValidateAll method.
    /// Currently implements no validation rules.
    /// </remarks>
    public override void ValidateAll()
    {
    }
    #endregion
}