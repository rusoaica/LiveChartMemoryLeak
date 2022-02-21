/// Written by: Yulia Danilova
/// Creation Date: 24th of November, 2019
/// Purpose: Code behind for the MainWindow view
#region ========================================================================= USING =====================================================================================
using LiveChartsCore;
using LiveChartsCore.Easing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
#endregion

namespace LiveChartMemoryLeak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window, IMainWindowView
    {
        #region ============================================================== FIELD MEMBERS ================================================================================
        readonly List<TestsClass> hugeList = new();
        #endregion

        #region ================================================================ PROPERTIES =================================================================================
        private IMainWindowVM VM => DataContext as IMainWindowVM ?? throw new ArgumentException("Invalid MainWindowV data context type!");
        #endregion

        #region ================================================================== CTOR =====================================================================================
        /// <summary>
        /// Default C-tor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // simulate huge memory consumption, to illustrate memory not being collected when the window is closed
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new();
            for (int i = 0; i < 8000000; i++)
            {
                hugeList.Add(new TestsClass() { Name = new string(Enumerable.Repeat(chars, 30).Select(s => s[random.Next(s.Length)]).ToArray()) });
            }
        }

        ~MainWindow()
        {
            // finalizer never reached when chart is not commented out
            Debug.WriteLine("destroyed!");
        }
        #endregion

        #region ================================================================= METHODS ===================================================================================
        #region Chart
        /// <summary>
        /// Handles the event risen when the sales data is updated
        /// </summary>
        private void VM_SalesDataUpdated()
        {
            if (VM.Sales != null)
            {
                List<ISeries> series = new();
                // get the highest value in all sales years, it will be used as the highest Y coordinate of the chart
                double highestValue = VM.Sales.Max(s => s.Value.Max());
                int i = 0;
                // iterate all sales years
                foreach (string? year in VM.Sales.Keys)
                {
                    // assign a different shade of blue for each of the five years of sales, from darker (the older the year) to lighter
                    SolidColorPaint? fill = null;
                    if (i == 0)
                        fill = new SolidColorPaint(new SKColor(25, 25, 112, 128));
                    else if (i == 1)
                        fill = new SolidColorPaint(new SKColor(20, 52, 164, 128));
                    else if (i == 2)
                        fill = new SolidColorPaint(new SKColor(73, 63, 211, 128));
                    else if (i == 3)
                        fill = new SolidColorPaint(new SKColor(53, 83, 231, 128));
                    else if (i == 4)
                        fill = new SolidColorPaint(new SKColor(96, 130, 182, 128));
                    // create the columns
                    ColumnSeries<double>? columnSeries = new()
                    {
                        Name = year,
                        Fill = fill,
                        Stroke = null,
                        Values = VM.Sales[year],
                        TooltipLabelFormatter = value => $"{year}: {value.Model:N} USD",
                    };
                    columnSeries.PointMeasured += OnPointMeasured;
                    series.Add(columnSeries);
                    i++;
                }
                chart.Series = series;
                // create the rows
                chart.YAxes = new Axis[]
                {
                    new Axis
                    {
                        MinLimit = 0,
                        TextSize = 14,
                        MinStep = 100000,
                        ForceStepToMin = true,
                        MaxLimit = highestValue,
                        SeparatorsPaint = new SolidColorPaint
                        {
                            StrokeThickness = 2,
                            Color = new SKColor(128, 128, 128, 55),
                            PathEffect = new DashEffect(new float[] { 3, 3 })
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Handles char point's PointMeasured event
        /// </summary>
        /// <param name="point">The point for which to handle the Measure event</param>
        private void OnPointMeasured(ChartPoint<double, RoundedRectangleGeometry, LabelGeometry> point)
        {
            // assign the animation played for the point, when its value is assigned
            RoundedRectangleGeometry? visual = point.Visual;
            DelayedFunction? delayedFunction = new(EasingFunctions.BuildCustomElasticOut(1.5f, 0.60f), point, 30f);
            if (visual != null)
                _ = visual.TransitionateProperties(nameof(visual.Y), nameof(visual.Height))
                          .WithAnimation(animation => animation.WithDuration(delayedFunction.Speed)
                                                               .WithEasingFunction(delayedFunction.Function));
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (chart.Series.Any())
                chart.Series.OfType<ColumnSeries<double>>().First().PointMeasured -= OnPointMeasured;
            VM.SalesDataUpdated -= VM_SalesDataUpdated;
            //chart.XAxes = null;
            //chart.YAxes = null;
            //chart.Series = null;
            //chart = null;
            DataContext = null;
        }
        #endregion

        #region ============================================================= EVENT HANDLERS ================================================================================
        /// <summary>
        /// Handles window's DataContextChanged event
        /// </summary>
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                VM.SalesDataUpdated += VM_SalesDataUpdated;
                VM.GetSalesChartDataAsync();
            }
        }
        #endregion
    }

    public class TestsClass
    {
        public string? Name { get; set; }
    }
}
