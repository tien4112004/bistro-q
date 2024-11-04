using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Validation;

public class ValidatorBase : INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();
    private readonly Dictionary<string, Func<object, List<string>>> _validationRules = new();
    
    public Dictionary<string, List<string>> Errors => _errors;

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public IEnumerable GetErrors(string propertyName)
    {
        return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
    }

    // Method to register validation rules for a property
    public void AddValidationRule(string propertyName, Func<object, List<string>> validationRule)
    {
        _validationRules[propertyName] = validationRule;
        Debug.WriteLine($"Validation rule added for {propertyName}");
    }

    // Method to validate a single property
    public void ValidateProperty(string propertyName, object value)
    {
        Debug.WriteLine($"Validating {propertyName}");
        if (_validationRules.ContainsKey(propertyName))
        {
            var errors = _validationRules[propertyName](value);
            if (errors?.Any() == true)
            {
                _errors[propertyName] = errors;
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    // Method to validate all properties
    public void ValidateAll()
    {
        foreach (var property in _validationRules.Keys)
        {
            ValidateProperty(property, null);
        }
    }
}
