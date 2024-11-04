using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Validation;


public class ValidatorBase : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();
    public Dictionary<string, List<string>> Errors => _errors;

    private readonly Dictionary<string, Func<object?, List<(bool IsValid, string Message)>>> _validators = new();

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName != null && _errors.ContainsKey(propertyName)
            ? _errors[propertyName]
            : Enumerable.Empty<string>();
    }

    public void ResetError(string propertyName)
    {
        UpdateErrors(propertyName, new List<string>());
    }

    protected void UpdateErrors(string propertyName, List<string> errors)
    {
        if (errors.Any())
            _errors[propertyName] = errors;
        else
            _errors.Remove(propertyName);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(Errors));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void AddValidator(string propertyName, Func<object?, List<(bool IsValue, string Message)>> validator)
    {
        _validators[propertyName] = validator;
    }

    public void ValidateProperty(string propertyName, object? value)
    {
        if (_validators.ContainsKey(propertyName))
        {
            var errors = new List<string>();

            var messages = _validators[propertyName].Invoke(value);

            foreach (var (isValid, message) in messages)
            {
                if (!isValid)
                {
                    errors.Add(message);
                }
            }

            UpdateErrors(propertyName, errors);
        }
    }

    public virtual void ValidateAll() {
        throw new NotImplementedException();
    }
}
