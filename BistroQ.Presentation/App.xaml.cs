using BistroQ.Domain.Contracts.Http;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Contracts.Services.Realtime;
using BistroQ.Domain.Services.Http;
using BistroQ.Presentation.Activation;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Mappings;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.Services;
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
using BistroQ.Presentation.ViewModels.KitchenOrder.Strategies;
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
using BistroQ.Service.Auth;
using BistroQ.Service.Common;
using BistroQ.Service.Data;
using BistroQ.Service.Http;
using BistroQ.Service.Realtime;
using CommunityToolkit.Mvvm.Messaging;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System.Diagnostics;

namespace BistroQ.Presentation;

public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; set; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        InitializeComponent();

        LiveCharts.Configure(config =>
            config
                .UseDefaults()
                .AddSkiaSharp()
                .AddDefaultMappers()
                .AddDarkTheme()
                .AddLightTheme()
        );

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Message Bus
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Http
            var BASE_URI = new Uri(Environment.GetEnvironmentVariable("API_BASE_URI") ?? "http://localhost:5256");
            services.AddTransient<AuthenticationDelegatingHandler>();
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = BASE_URI;
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();
            services.AddHttpClient<IPublicApiClient, PublicApiClient>(client =>
            {
                client.BaseAddress = BASE_URI;
            });
            services.AddHttpClient<IMultipartApiClient, MultipartApiClient>(client =>
            {
                client.BaseAddress = BASE_URI;
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();
            services.AddHttpClient();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ITokenStorageService, TokenSecureStorageService>();
            services.AddSingleton<KitchenStrategyFactory>();

            // Views and ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<LoginViewModel>();
            // AdminZone V&VM
            services.AddTransient<AdminZoneViewModel>();
            services.AddTransient<AdminZonePage>();
            services.AddTransient<AdminZoneAddPageViewModel>();
            services.AddTransient<AdminZoneAddPage>();
            services.AddTransient<AdminZoneEditPageViewModel>();
            services.AddTransient<AdminZoneEditPage>();
            services.AddTransient<AdminTableViewModel>();
            services.AddTransient<AdminTablePage>();
            services.AddTransient<AdminTableAddPageViewModel>();
            services.AddTransient<AdminTableAddPage>();
            services.AddTransient<AdminTableEditPageViewModel>();
            services.AddTransient<AdminTableEditPage>();
            services.AddTransient<AdminCategoryViewModel>();
            services.AddTransient<AdminCategoryPage>();
            services.AddTransient<AdminCategoryAddPageViewModel>();
            services.AddTransient<AdminCategoryAddPage>();
            services.AddTransient<AdminCategoryEditPageViewModel>();
            services.AddTransient<AdminCategoryEditPage>();
            services.AddTransient<AdminProductViewModel>();
            services.AddTransient<AdminProductPage>();
            services.AddTransient<AdminProductAddPageViewModel>();
            services.AddTransient<AdminProductAddPage>();
            services.AddTransient<AdminProductEditPageViewModel>();
            services.AddTransient<AdminProductEditPage>();
            services.AddTransient<AdminAccountPage>();
            services.AddTransient<AdminAccountViewModel>();
            services.AddTransient<AdminAccountAddPage>();
            services.AddTransient<AdminAccountAddPageViewModel>();
            services.AddTransient<AdminAccountEditPage>();
            services.AddTransient<AdminAccountEditPageViewModel>();


            services.AddSingleton<IZoneDataService, ZoneDataService>();
            services.AddSingleton<ITableDataService, TableDataService>();
            services.AddSingleton<IOrderDataService, OrderDataService>();
            services.AddSingleton<IOrderItemDataService, OrderItemDataService>();
            services.AddSingleton<ICategoryDataService, CategoryDataService>();
            services.AddSingleton<IProductDataService, ProductDataService>();
            services.AddSingleton<IAccountDataService, AccountDataService>();

            // Realtime service
            services.AddSingleton<ICheckoutRealTimeService, CheckoutRealTimeService>();

            services.AddTransient<IDialogService, DialogService>();

            // Client V&VM
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<ProductListViewModel>();
            services.AddTransient<OrderCartViewModel>();

            // Cashier V&VM
            services.AddTransient<CashierTableViewModel>();
            services.AddTransient<CashierTablePage>();
            services.AddTransient<ZoneOverviewViewModel>();
            services.AddTransient<ZoneTableGridViewModel>();
            services.AddTransient<TableOrderDetailViewModel>();

            // Kitchen V&VM
            services.AddTransient<KitchenOrderViewModel>();
            services.AddTransient<KitchenOrderPage>();
            services.AddTransient<OrderKanbanColumnViewModel>();
            services.AddTransient<KitchenOrderButtonsViewModel>();
            services.AddTransient<KitchenHistoryPage>();
            services.AddTransient<KitchenHistoryViewModel>();
            services.AddTransient<OrderItemGridViewModel>();

            // Auto Mapper
            services.AddAutoMapper((serviceProvider, cfg) =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.ConstructServicesUsing(type =>
                    ActivatorUtilities.CreateInstance(serviceProvider, type));
            }, typeof(MappingProfile).Assembly);


            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        if (e.Exception != null)
        {
            Debug.WriteLine(e.Exception.Message);
            Debug.WriteLine(e.Exception.StackTrace);
        }
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        new LoginWindow().Activate();

    }
}