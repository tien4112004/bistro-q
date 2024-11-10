using BistroQ.Validation;

namespace BistroQ.Models;

public class AddTableForm : ValidatorBase
{
    public AddTableForm()
    {
        AddZoneIdValidator();
        AddSeatsCountValidator();
    }

    private int _seatsCount;

    private int _zoneId;

    public int SeatsCount
    {
        get => _seatsCount;
        set
        {
            if (_seatsCount != value)
            {
                _seatsCount = value;
                OnPropertyChanged();
            }
        }
    }
    public int ZoneId
    {
        get => _zoneId;
        set
        {
            if (_zoneId != value)
            {
                _zoneId = value;
                OnPropertyChanged();
            }
        }
    }

    public void AddZoneIdValidator()
    {
        AddValidator(nameof(ZoneId), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "ZoneId"),
                v => ValidationRules.StringRules.OnlyDigit(v, "ZoneId")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddSeatsCountValidator()
    {
        AddValidator(nameof(SeatsCount), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "SeatsCount"),
                v => ValidationRules.StringRules.OnlyDigit(v, "SeatsCount"),
                v => ValidationRules.StringRules.IsPositive(v, "SeatsCount")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public override void ValidateAll()
    {
        base.ValidateProperty(nameof(ZoneId), ZoneId);
        base.ValidateProperty(nameof(SeatsCount), SeatsCount);
    }
}
