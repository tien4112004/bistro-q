using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Contracts.Services;

public interface INavigationViewService
{
    IList<object>? MenuItems
    {
        get;
    }

    object? SettingsItem
    {
        get;
    }

    void Initialize(NavigationView navigationView, string role);

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);
}
