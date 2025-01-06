using BistroQ.Presentation.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.KitchenHistory;

/// <summary>
/// ViewModel responsible for managing the kitchen history view.
/// Handles navigation lifecycle and coordinates with the order item grid.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableObject for MVVM pattern
/// - INavigationAware for navigation lifecycle management
/// </remarks>
public partial class KitchenHistoryViewModel : ObservableObject, INavigationAware
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the view model for the order item grid
    /// </summary>
    public OrderItemGridViewModel OrderItemGridViewModel { get; set; }
    #endregion

    #region Constructor
    public KitchenHistoryViewModel(OrderItemGridViewModel orderItemGridViewModel)
    {
        OrderItemGridViewModel = orderItemGridViewModel;
    }
    #endregion

    #region Navigation Methods
    /// <summary>
    /// Handles actions when navigating to this page
    /// </summary>
    /// <param name="parameter">Navigation parameter</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task OnNavigatedTo(object parameter)
    {
        await OrderItemGridViewModel.LoadItemsAsync();
    }

    /// <summary>
    /// Handles cleanup when navigating away from this page
    /// </summary>
    /// <returns>A completed task</returns>
    public Task OnNavigatedFrom()
    {
        OrderItemGridViewModel.Dispose();
        return Task.CompletedTask;
    }
    #endregion
}