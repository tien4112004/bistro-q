﻿using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Contracts.Services;

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

    void NavigateToEntryPoint();

    void UnregisterEvents();

    NavigationViewItem? GetSelectedItem(Type pageType);
}
