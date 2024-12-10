using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BistroQ.Presentation.Validation;

/// <summary>
/// ValidatorBase is a base class that provides validation infrastructure for WinUI forms. 
/// It implements both <see cref="INotifyPropertyChanged"/> and <see cref="INotifyDataErrorInfo"/> interfaces to support data binding and validation 
/// </summary>
public partial class ValidatorBase : INotifyPropertyChanged, INotifyDataErrorInfo
{
    #region Private Fields

    private readonly Dictionary<string, List<string>> _errors = new(); // Stores validation errors for each property
    private readonly Dictionary<string, Func<object?, List<(bool IsValid, string Message)>>> _validators = new(); // Stores validation functions for each property

    #endregion

    #region Public Properties

    public Dictionary<string, List<string>> Errors => _errors;
    public bool HasErrors => _errors.Any();

    #endregion

    #region Public Methods

    /// <summary>
    /// Retrieves validation errors for a specific property
    /// </summary>
    /// <param name="propertyName">Name of the property to get errors for</param>
    /// <returns>List of error messages or empty enumerable if no errors exist</returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName != null && _errors.ContainsKey(propertyName)
            ? _errors[propertyName]
            : Enumerable.Empty<string>();
    }

    /// <summary>
    /// Clears all validation errors for a specific property
    /// </summary>
    /// <param name="propertyName">Name of the property to reset errors for</param>
    public void ResetError(string propertyName)
    {
        UpdateErrors(propertyName, new List<string>());
    }

    ///<summary>
    /// Clears all validation errors
    /// </summary>
    public void ClearErrors()
    {
        _errors.Clear();
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(string.Empty));
        OnPropertyChanged(nameof(Errors));
    }

    /// <summary>
    /// Validates a specific property value using registered validators
    /// </summary>
    /// <param name="propertyName">Name of the property to validate</param>
    /// <param name="value">Value to validate</param>
    public void ValidateProperty(string propertyName, object? value)
    {
        if (!_validators.ContainsKey(propertyName))
            return;

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

    /// <summary>
    /// Template method for validating all properties
    /// </summary>
    /// <exception cref="NotImplementedException">if not overridden</exception>
    public virtual void ValidateAll()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Updates the error collection for a property
    /// </summary>
    /// <param name="propertyName">Name of the property</param>
    /// <param name="errors">List of error messages</param>
    protected void UpdateErrors(string propertyName, List<string> errors)
    {
        if (errors.Any())
            _errors[propertyName] = errors;
        else
            _errors.Remove(propertyName);

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(Errors));
    }

    /// <summary>
    /// Registers a list of validator functions for a property
    /// </summary>
    /// <param name="propertyName">Name of the property to validate</param>
    /// <param name="validators">Function that performs validation and returns results</param>
    protected void AddValidator(string propertyName, Func<object?, List<(bool IsValue, string Message)>> validators)
    {
        _validators[propertyName] = validators;
    }

    #endregion

    #region Events

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}