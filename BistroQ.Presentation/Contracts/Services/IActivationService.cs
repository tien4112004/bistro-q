namespace BistroQ.Presentation.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
