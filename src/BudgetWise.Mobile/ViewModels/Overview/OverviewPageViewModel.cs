using BudgetWise.Core.Domain.Abstractions.Services;
using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Core.Domain.Model.Summaries;
using BudgetWise.Core.Domain.Model.Summaries.ValueObjects;
using BudgetWise.Mobile.Abstractions;
using BudgetWise.Mobile.Resources.Strings;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace BudgetWise.Mobile.ViewModels.Overview;

public class OverviewPageViewModel : BaseNotifyObject
{
    private readonly ICalculationService _calculationService;

    private readonly ObservableCollection<decimal> _categoriesExpensesValues = [];
    private readonly ObservableCollection<string> _categoriesExpensesLabelsValues = [];
    private readonly ObservableCollection<decimal> _mainSpendingTypeExpenses = [];
    private readonly ObservableCollection<decimal> _secondarySpendingTypeExpenses = [];
    private readonly ObservableCollection<decimal> _savedSpendingTypeExpenses = [];
    private readonly ObservableCollection<decimal> _totalIncome = [];
    private readonly ObservableCollection<decimal> _totalExpense = [];
    private readonly ObservableCollection<decimal> _plannedCategoriesExpenses = [];
    private readonly ObservableCollection<string> _plannedCategoriesExpensesLabels = [];
    private readonly ObservableCollection<decimal> _actualCategoriesExpenses = [];
    private readonly ObservableCollection<decimal> _dailyExpensesValues = [];
    private readonly ObservableCollection<string> _dailyExpensesLabelsValues = [];
    private readonly ObservableCollection<decimal> _weeklyExpensesValues = [];
    private readonly ObservableCollection<string> _weeklyExpensesLabelsValues = [];

    private SummaryCalculationDisplayType _selectedDisplayCalculationType = DisplayCalculationTypes[0];
    private bool _isShowDailyExpenses;
    private bool _isShowWeeklyExpenses;

    public OverviewPageViewModel(ICalculationService calculationService)
    {
        _calculationService = calculationService;
        _isShowDailyExpenses = false;
        _isShowWeeklyExpenses = false;

        CategoriesExpenses =
        [
            new ColumnSeries<decimal, RoundedRectangleGeometry, LabelGeometry>
            {
                Values = _categoriesExpensesValues
            }
        ];

        CategoriesExpensesLabels =
        [
            new Axis
            {
               Labels = _categoriesExpensesLabelsValues,
               LabelsRotation = 90,
               UnitWidth = 1,
               TextSize = 10,
               Padding = new LiveChartsCore.Drawing.Padding(1)
            }
        ];

        SpendingTypeDistribution =
        [
            new PieSeries<decimal>
            {
                Values = _mainSpendingTypeExpenses,
                Name = AppResources.OverView_SpendingType_Main,
                DataLabelsFormatter = point => point.Model.ToString(),
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            },
            new PieSeries<decimal>
            {
                Values = _secondarySpendingTypeExpenses,
                Name = AppResources.Overview_SpendingType_Secondary,
                DataLabelsFormatter = point => point.Model.ToString(),
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            },
            new PieSeries<decimal>
            {
                Values = _savedSpendingTypeExpenses,
                Name = AppResources.Overview_SpendingType_Saved,
                DataLabelsFormatter = point => point.Model.ToString(),
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            }
        ];

        IncomeAndExpense =
        [
            new PieSeries<decimal>
            {
                Values = _totalIncome,
                Name = AppResources.Overview_Income,
                DataLabelsFormatter = point => point.Model.ToString(),
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            },
            new PieSeries<decimal>
            {
                Values = _totalExpense,
                Name = AppResources.Overview_Expense,
                DataLabelsFormatter = point => point.Model.ToString(),
                DataLabelsSize = 10,
                DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                DataLabelsPaint = new SolidColorPaint(SKColors.White)
            }
        ];

        PlannedAndActualCategoriesAmounts =
        [
            new ColumnSeries<decimal> { Values = _plannedCategoriesExpenses },
            new ColumnSeries<decimal> { Values = _actualCategoriesExpenses }
        ];

        PlannedAndActualCategoriesAmountsLabels =
        [
            new Axis
            {
               Labels = _plannedCategoriesExpensesLabels,
               LabelsRotation = 90,
               UnitWidth = 1,
               TextSize = 10,
               Padding = new LiveChartsCore.Drawing.Padding(1)
            }
        ];

        DailyExpenses =
        [
            new LineSeries<decimal>
            {
                Values = _dailyExpensesValues,
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null
            }
        ];

        DailyExpensesLabels =
        [
            new Axis
            {
               Labels = _dailyExpensesLabelsValues,
               LabelsRotation = 90,
               UnitWidth = 1,
               TextSize = 10,
               Padding = new LiveChartsCore.Drawing.Padding(1)
            }
        ];

        WeeklyExpenses =
        [
            new LineSeries<decimal>
            {
                Values = _weeklyExpensesValues,
                Fill = null,
                GeometryFill = null,
                GeometrySize = 8
            }
        ];

        WeeklyExpensesLabels =
        [
            new Axis
            {
               Labels = _weeklyExpensesLabelsValues,
               LabelsRotation = 90,
               UnitWidth = 1,
               TextSize = 10,
               Padding = new LiveChartsCore.Drawing.Padding(1)
            }
        ];
    }

    public ISeries[] CategoriesExpenses { get; set; }
    public Axis[] CategoriesExpensesLabels { get; set; }
    public ISeries[] SpendingTypeDistribution { get; set; }
    public ISeries[] IncomeAndExpense { get; set; }
    public ISeries[] PlannedAndActualCategoriesAmounts { get; set; }
    public Axis[] PlannedAndActualCategoriesAmountsLabels { get; set; }
    public ISeries[] DailyExpenses { get; set; }
    public Axis[] DailyExpensesLabels { get; set; }
    public ISeries[] WeeklyExpenses { get; set; }
    public Axis[] WeeklyExpensesLabels { get; set; }

    public static ObservableCollection<SummaryCalculationDisplayType> DisplayCalculationTypes { get; } =
    [
        new(SummaryCalculationType.ForMonth, AppResources.Overview_ForMonth),
        new(SummaryCalculationType.ForThreeMonths, AppResources.Overview_ForThreeMonths),
        new(SummaryCalculationType.ForSixMonths, AppResources.Overview_ForSixMonths)
    ];

    public SummaryCalculationDisplayType SelectedDisplayCalculationType
    {
        get => _selectedDisplayCalculationType;
        set => SetProperty(ref _selectedDisplayCalculationType, value);
    }

    public bool IsShowDailyExpenses
    {
        get => _isShowDailyExpenses;
        set => SetProperty(ref _isShowDailyExpenses, value);
    }

    public bool IsShowWeeklyExpenses
    {
        get => _isShowWeeklyExpenses;
        set => SetProperty(ref _isShowWeeklyExpenses, value);
    }

    public async Task Initialize(bool calcTypeChanged = false)
    {
        if (!calcTypeChanged)
        {
            SelectedDisplayCalculationType = DisplayCalculationTypes[0];
        }

        var summary = await _calculationService.GetSummaryForPeriod(_selectedDisplayCalculationType.CalculationType);

        InvalidateSeries();
        DistributeSummary(summary);
    }

    private void InvalidateSeries()
    {
        _categoriesExpensesValues.Clear();
        _categoriesExpensesLabelsValues.Clear();
        _mainSpendingTypeExpenses.Clear();
        _secondarySpendingTypeExpenses.Clear();
        _savedSpendingTypeExpenses.Clear();
        _totalIncome.Clear();
        _totalExpense.Clear();
        _plannedCategoriesExpenses.Clear();
        _actualCategoriesExpenses.Clear();
        _plannedCategoriesExpensesLabels.Clear();
        _dailyExpensesValues.Clear();
        _dailyExpensesLabelsValues.Clear();
        _weeklyExpensesValues.Clear();
        _weeklyExpensesLabelsValues.Clear();
    }

    private void DistributeSummary(Summary summary)
    {
        foreach (var categoryExpense in summary.CategoryExpenses.Values)
        {
            _categoriesExpensesValues.Add(categoryExpense.Amount);
            _categoriesExpensesLabelsValues.Add(categoryExpense.CategoryName);
        }

        _mainSpendingTypeExpenses.Add(summary.SpendingTypesExpenses[SpendingType.Main]);
        _secondarySpendingTypeExpenses.Add(summary.SpendingTypesExpenses[SpendingType.Secondary]);
        _savedSpendingTypeExpenses.Add(summary.SpendingTypesExpenses[SpendingType.Saved]);

        _totalExpense.Add(summary.TotalExpenses);
        _totalIncome.Add(summary.TotalIncome);

        foreach (var categoryExpectation in summary.CategoryExpenseExpectations.Values)
        {
            _plannedCategoriesExpenses.Add(categoryExpectation.PlannedAmount);
            _actualCategoriesExpenses.Add(categoryExpectation.ActualAmount);
            _plannedCategoriesExpensesLabels.Add(categoryExpectation.CategoryName);
        }

        IsShowDailyExpenses = _selectedDisplayCalculationType.CalculationType
            is SummaryCalculationType.ForMonth
            or SummaryCalculationType.ForThreeMonths;

        IsShowWeeklyExpenses = _selectedDisplayCalculationType.CalculationType
            is SummaryCalculationType.ForThreeMonths
            or SummaryCalculationType.ForSixMonths;

        if (IsShowDailyExpenses)
        {
            DistributeDailyExpenses(summary);
        }

        if (IsShowWeeklyExpenses)
        {
            DistributeWeeklyExpenses(summary);
        }
    }

    private void DistributeDailyExpenses(Summary summary)
    {
        if (summary.DailyExpenses is null)
        {
            return;
        }

        foreach (var dailyExpense in summary.DailyExpenses)
        {
            _dailyExpensesValues.Add(dailyExpense.Amount);
            _dailyExpensesLabelsValues.Add(dailyExpense.Date.ToString("dd.MM"));
        }
    }

    private void DistributeWeeklyExpenses(Summary summary)
    {
        if (summary.WeeklyExpenses is null)
        {
            return;
        }

        foreach (var weeklyExpense in summary.WeeklyExpenses)
        {
            var dateStr = $"{weeklyExpense.DateFrom.Date:dd.MM} - {weeklyExpense.DateTo.Date:dd.MM}";

            _weeklyExpensesValues.Add(weeklyExpense.Amount);
            _weeklyExpensesLabelsValues.Add(dateStr);
        }
    }
}

public class SummaryCalculationDisplayType
{
    public SummaryCalculationDisplayType(SummaryCalculationType type, string displayName)
    {
        CalculationType = type;
        DisplayName = displayName;
    }

    public SummaryCalculationType CalculationType { get; }
    public string DisplayName { get; }
}