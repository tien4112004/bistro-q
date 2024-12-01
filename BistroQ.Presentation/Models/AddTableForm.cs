using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

public class AddTableForm : ValidatorBase
{
    public AddTableForm()
    {
    }

    public int SeatsCount { get; set; }
    public int ZoneId { get; set; }


    public override void ValidateAll()
    {
    }
}