﻿using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.ViewModels;
using BistroQ.Presentation.ViewModels.AdminAccount;
using BistroQ.Presentation.ViewModels.AdminCategory;
using BistroQ.Presentation.ViewModels.AdminProduct;
using BistroQ.Presentation.ViewModels.AdminTable;
using BistroQ.Presentation.ViewModels.AdminZone;
using BistroQ.Presentation.ViewModels.CashierTable;
using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.ViewModels.KitchenHistory;
using BistroQ.Presentation.ViewModels.KitchenOrder;
using BistroQ.Presentation.Views;
using BistroQ.Presentation.Views.AdminAccount;
using BistroQ.Presentation.Views.AdminCategory;
using BistroQ.Presentation.Views.AdminProduct;
using BistroQ.Presentation.Views.AdminTable;
using BistroQ.Presentation.Views.AdminZone;
using BistroQ.Presentation.Views.CashierTable;
using BistroQ.Presentation.Views.Client;
using BistroQ.Presentation.Views.KitchenHistory;
using BistroQ.Presentation.Views.KitchenOrder;
using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Services;

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

        Configure<AdminCategoryViewModel, AdminCategoryPage>();
        Configure<AdminCategoryAddPageViewModel, AdminCategoryAddPage>();
        Configure<AdminCategoryEditPageViewModel, AdminCategoryEditPage>();

        Configure<AdminProductViewModel, AdminProductPage>();
        Configure<AdminProductAddPageViewModel, AdminProductAddPage>();
        Configure<AdminProductEditPageViewModel, AdminProductEditPage>();

        Configure<AdminAccountViewModel, AdminAccountPage>();
        Configure<AdminAccountAddPageViewModel, AdminAccountAddPage>();
        Configure<AdminAccountEditPageViewModel, AdminAccountEditPage>();

        Configure<KitchenOrderViewModel, KitchenOrderPage>();
        Configure<KitchenHistoryViewModel, KitchenHistoryPage>();
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
