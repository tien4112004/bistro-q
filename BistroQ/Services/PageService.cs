﻿using BistroQ.Contracts.Services;
using BistroQ.ViewModels;
using BistroQ.ViewModels.AdminTable;
using BistroQ.ViewModels.AdminZone;
using BistroQ.ViewModels.CashierTable;
using BistroQ.ViewModels.Client;
using BistroQ.Views;
using BistroQ.Views.AdminTable;
using BistroQ.Views.AdminZone;
using BistroQ.Views.CashierTable;
using BistroQ.Views.Client;
using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<CashierTableViewModel, CashierTablePage>();
        Configure<MainViewModel, MainPage>();
        Configure<AdminZoneViewModel, AdminZonePage>();
        Configure<AdminZoneAddPageViewModel, AdminZoneAddPage>();
        Configure<AdminZoneEditPageViewModel, AdminZoneEditPage>();

        Configure<HomePageViewModel, HomePage>();

        Configure<AdminTableViewModel, AdminTablePage>();
        Configure<AdminTableAddPageViewModel, AdminTableAddPage>();
        Configure<AdminTableEditPageViewModel, AdminTableEditPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
