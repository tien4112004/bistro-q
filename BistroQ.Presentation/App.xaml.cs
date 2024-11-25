using BistroQ.Presentation.Activation;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Services;
using BistroQ.Domain.Services.Http;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.Services;
using BistroQ.Presentation.ViewModels;
using BistroQ.Presentation.ViewModels.AdminTable;
using BistroQ.Presentation.ViewModels.AdminZone;
using BistroQ.Presentation.ViewModels.CashierTable;
using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.Views;
using BistroQ.Presentation.Views.AdminTable;
using BistroQ.Presentation.Views.AdminZone;
using BistroQ.Presentation.Views.CashierTable;
using BistroQ.Presentation.Views.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using AutoMapper;
using BistroQ.Domain.Mappings;
using BistroQ.Presentation.Mappings;
using BistroQ.Service.Auth;
using BistroQ.Service.Common;
using BistroQ.Service.Data;

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

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

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
            services.AddScoped<IAdminZoneDialogService, AdminZoneDialogDialogService>();
            services.AddScoped<IAdminTableDialogService, AdminTableDialogService>();
            services.AddScoped<IOrderDataService, OrderDataService>();
            
            // Client V&VM
            services.AddTransient<HomePageViewModel>();
            services.AddTransient<HomePage>();

            // Cashier V&VM
            services.AddTransient<CashierTableViewModel>();
            services.AddTransient<CashierTablePage>();
            services.AddTransient<ZoneOverviewViewModel>();
            services.AddTransient<ZoneTableGridViewModel>();
            services.AddTransient<TableOrderDetailViewModel>();

            // Auto Mapper
            services.AddAutoMapper(typeof(DomainMappingProfile).Assembly);
            services.AddAutoMapper(typeof(PresentationMappingProfile).Assembly);
            services.AddSingleton(provider =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(PresentationMappingProfile).Assembly);    
                    cfg.AddMaps(typeof(DomainMappingProfile).Assembly); 
                });
                    
                config.AssertConfigurationIsValid();
                    
                return config.CreateMapper();
            });
            
            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        Debug.WriteLine(e);
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        new LoginWindow().Activate();
    }
}