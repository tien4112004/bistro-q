using BistroQ.Presentation.Contracts.Services;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.ViewModels;

/// <summary>
/// ViewModel for the shell/main window of the application.
/// Manages navigation state and selected items in the navigation view.
/// </summary>
/// <remarks>
/// Inherits from ObservableRecipient to support the MVVM pattern and property change notifications.
/// </remarks>
public partial class ShellViewModel : ObservableRecipient
{
    #region Private Fields
    /// <summary>
    /// Flag indicating whether navigation back is possible.
    /// </summary>
    [ObservableProperty]
    private bool isBackEnabled;

    /// <summary>
    /// Currently selected item in the navigation view.
    /// </summary>
    [ObservableProperty]
    private object? selected;
    #endregion

    #region Public Properties
    /// <summary>
    /// Service responsible for handling navigation between pages.
    /// </summary>
    /// <remarks>
    /// This service manages the navigation stack and provides navigation-related functionality.
    /// </remarks>
    public INavigationService NavigationService { get; }

    /// <summary>
    /// Service responsible for managing the navigation view component.
    /// </summary>
    /// <remarks>
    /// This service handles the selection and display of navigation menu items.
    /// </remarks>
    public INavigationViewService NavigationViewService { get; }
    #endregion

    #region Constructor
    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Handles the Navigated event of the NavigationService.
    /// Updates the back navigation state and selected item in the navigation view.
    /// </summary>
    /// <param name="sender">The source of the navigation event.</param>
    /// <param name="e">Navigation event arguments containing the source page type.</param>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;
        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
    #endregion
}