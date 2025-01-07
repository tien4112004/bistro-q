using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels;

public partial class AdminDashboardViewModel : ObservableObject
{
    private decimal todayRevenue;
    private int customerCount;
    private double averageOrderTime;
    private double tableTurnoverRate;
    private string popularCategory;
    private double averageCaloriesPerOrder;

    public decimal TodayRevenue
    {
        get => todayRevenue;
        set => SetProperty(ref todayRevenue, value);
    }

    public int CustomerCount
    {
        get => customerCount;
        set => SetProperty(ref customerCount, value);
    }

    public double AverageOrderTime
    {
        get => averageOrderTime;
        set => SetProperty(ref averageOrderTime, value);
    }

    public double TableTurnoverRate
    {
        get => tableTurnoverRate;
        set => SetProperty(ref tableTurnoverRate, value);
    }

    public string PopularCategory
    {
        get => popularCategory;
        set => SetProperty(ref popularCategory, value);
    }

    public double AverageCaloriesPerOrder
    {
        get => averageCaloriesPerOrder;
        set => SetProperty(ref averageCaloriesPerOrder, value);
    }

    public ObservableCollection<RevenueData> RevenueData { get; } = new();
    public ObservableCollection<PopularDishData> PopularDishes { get; } = new();

    public ISeries[] RevenueSeries { get; private set; }
    public ISeries[] PopularDishesSeries { get; private set; }
    public Axis[] RevenueXAxes { get; private set; }
    public Axis[] RevenueYAxes { get; private set; }
    public Axis[] PopularDishesXAxes { get; private set; }
    public Axis[] PopularDishesYAxes { get; private set; }

    public AdminDashboardViewModel()
    {
        LoadMockData();
        InitializeCharts();
    }

    private void LoadMockData()
    {
        // Mock current stats
        TodayRevenue = 1220000m;
        CustomerCount = 127;
        AverageOrderTime = 24;
        TableTurnoverRate = 4.5;
        PopularCategory = "Main Course (45%)";
        AverageCaloriesPerOrder = 850;

        // Mock revenue trend data
        RevenueData.Clear();
        var mockRevenueData = new[]
        {
            new RevenueData { Date = "01-01-2025", Revenue = 4320059m },
            new RevenueData { Date = "02-01-2025", Revenue = 3481919m },
            new RevenueData { Date = "03-01-2025", Revenue = 3586855m },
            new RevenueData { Date = "04-01-2025", Revenue = 4868979m },
            new RevenueData { Date = "05-01-2025", Revenue = 4715720m },
            new RevenueData { Date = "06-01-2025", Revenue = 3347883m },
            new RevenueData { Date = "07-01-2025", Revenue = 1220000m }
        };
        foreach (var data in mockRevenueData)
        {
            RevenueData.Add(data);
        }

        // Mock popular dishes data
        PopularDishes.Clear();
        var mockPopularDishes = new[]
        {
            new PopularDishData { Name = "Pasta Carbonara", Orders = 150 },
            new PopularDishData { Name = "Margherita Pizza", Orders = 120 },
            new PopularDishData { Name = "Caesar Salad", Orders = 90 },
            new PopularDishData { Name = "Tiramisu", Orders = 80 }
        };
        foreach (var dish in mockPopularDishes)
        {
            PopularDishes.Add(dish);
        }
    }

    private void InitializeCharts()
    {
        var primaryColor = new SKColor(0, 114, 178);      // Professional blue
        var secondaryColor = new SKColor(255, 165, 0);    // Orange
        var textColor = new SKColor(255, 255, 255);       // White for text
        var gridLineColor = new SKColor(128, 128, 128);   // Medium gray for grid lines

        RevenueSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Name = "Revenue",
                Values = RevenueData.Select(x => (double)x.Revenue).ToArray(),
                Stroke = new SolidColorPaint(primaryColor) { StrokeThickness = 3 },
                GeometryStroke = new SolidColorPaint(primaryColor),
                GeometryFill = new SolidColorPaint(SKColors.White),
                GeometrySize = 10
            }
        };

        var commonAxisStyle = new SolidColorPaint(textColor)
        {
            StrokeThickness = 1,
            FontFamily = "Arial"
        };

        var commonSeparatorsPaint = new SolidColorPaint(gridLineColor)
        {
            StrokeThickness = 0.5f,
            PathEffect = new DashEffect(new float[] { 3, 3 })
        };

        RevenueXAxes = new Axis[]
        {
            new Axis
            {
                Labels = RevenueData.Select(x => x.Date).ToArray(),
                LabelsRotation = 15,
                TextSize = 14,
                LabelsPaint = commonAxisStyle,
                SeparatorsPaint = commonSeparatorsPaint,
                ShowSeparatorLines = true
            }
        };

        RevenueYAxes = new Axis[]
        {
            new Axis
            {
                Name = "Revenue ($)",
                NamePaint = commonAxisStyle,
                TextSize = 12,
                LabelsPaint = commonAxisStyle,
                SeparatorsPaint = commonSeparatorsPaint,
                ForceStepToMin = true,
                MinStep = 1000000
            }
        };

        PopularDishesSeries = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "Orders",
                Values = PopularDishes.Select(x => (double)x.Orders).ToArray(),
                Fill = new SolidColorPaint(secondaryColor)
            }
        };

        PopularDishesXAxes = new Axis[]
        {
            new Axis
            {
                Labels = PopularDishes.Select(x => x.Name).ToArray(),
                LabelsRotation = 15,
                TextSize = 12,
                LabelsPaint = commonAxisStyle,
                SeparatorsPaint = commonSeparatorsPaint
            }
        };

        PopularDishesYAxes = new Axis[]
        {
            new Axis
            {
                Name = "Orders",
                NamePaint = commonAxisStyle,
                TextSize = 12,
                LabelsPaint = commonAxisStyle,
                SeparatorsPaint = commonSeparatorsPaint,
                ForceStepToMin = true,
                MinStep = 20
            }
        };
    }

    public async Task RefreshDataAsync()
    {
        try
        {
            // TODO: Replace with actual API calls
            await Task.Delay(1000); // Simulate network delay
            LoadMockData();
            InitializeCharts();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error refreshing dashboard data: {ex.Message}");
        }
    }
}

public class RevenueData
{
    public string Date { get; set; }
    public decimal Revenue { get; set; }
}

public class PopularDishData
{
    public string Name { get; set; }
    public int Orders { get; set; }
}