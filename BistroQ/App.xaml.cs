using BistroQ.Activation;
using BistroQ.Contracts.Services;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Services;
using BistroQ.Core.Services.Auth;
using BistroQ.Core.Services.Http;
using BistroQ.Models;
using BistroQ.Services;
using BistroQ.ViewModels;
using BistroQ.ViewModels.AdminTable;
using BistroQ.ViewModels.AdminZone;
using BistroQ.ViewModels.Cashier;
using BistroQ.ViewModels.Client;
using BistroQ.Views;
using BistroQ.Views.AdminTable;
using BistroQ.Views.AdminZone;
using BistroQ.Views.Cashier;
using BistroQ.Views.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace BistroQ;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
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

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            // Data Services
            services.AddSingleton<IZoneDataService, ZoneDataService>();

            // Http
            services.AddTransient<AuthenticationDelegatingHandler>();
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_BASE_URI") ?? "http://localhost:5256");
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();
            services.AddHttpClient<IPublicApiClient, PublicApiClient>(client =>
            {
                client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_BASE_URI") ?? "http://localhost:5256");
            });

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddHttpClient();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ITokenStorageService, TokenSecureStorageService>();

            // Views and ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<LoginViewModel>();
            // AdminZone V&VM
            services.AddScoped<AdminZoneViewModel>();
            services.AddTransient<AdminZonePage>();
            services.AddTransient<AdminZoneAddPageViewModel>();
            services.AddTransient<AdminZoneAddPage>();
            services.AddTransient<AdminZoneEditPageViewModel>();
            services.AddTransient<AdminZoneEditPage>();
            services.AddScoped<AdminTableViewModel>();
            services.AddTransient<AdminTablePage>();
            services.AddTransient<AdminTableAddPageViewModel>();
            services.AddTransient<AdminTableAddPage>();
            services.AddTransient<AdminTableEditPageViewModel>();
            services.AddTransient<AdminTableEditPage>();

            services.AddScoped<IZoneDataService, ZoneDataService>();
            services.AddScoped<ITableDataService, TableDataService>();
            services.AddScoped<IAdminZoneService, AdminZoneService>();
            services.AddScoped<IOrderDataService, OrderDataService>();
            services.AddScoped<IAdminTableService, AdminTableService>();
            // Client V&VM
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<HomePage>();

            // Cashier V&VM
            services.AddTransient<CashierTableViewModel>();
            services.AddTransient<CashierTablePage>();

            services.AddTransient<TestKitchenViewModel>();
            services.AddTransient<TestKitchenPage>();
            services.AddTransient<TestAdminPage>();
            services.AddTransient<TestAdminViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        new LoginWindow().Activate();
    }
}
