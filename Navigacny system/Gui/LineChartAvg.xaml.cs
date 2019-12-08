using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Navigacny_system.Gui
{
    /// <summary>
    /// Interaction logic for LineChartAvg.xaml
    /// </summary>
    public partial class LineChartAvg : UserControl
    {
        private CartesianChart cartesianChart;
        private double[] minY;
        private double[] maxY;

        public LineChartAvg()
        {
            InitializeComponent();
            cartesianChart = new CartesianChart();
            cartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>(),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Title = "A-B-C-D-E"
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>(),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Title = "A-F-G-E"
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>(),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Title = "A-F-H-D-E"
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>(),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    Title = "A-F-H-C-D-E"
                }
            };
            minY = new double[cartesianChart.Series.Count];
            maxY = new double[cartesianChart.Series.Count];
            for (int i = 0; i < cartesianChart.Series.Count; i++)
            {
                minY[i] = Double.MaxValue;
                maxY[i] = Double.MinValue;
            }
            cartesianChart.LegendLocation = LegendLocation.Right;
            cartesianChart.DisableAnimations = true;
            cartesianChart.Hoverable = false;
            cartesianChart.DataTooltip = null;
            cartesianChart.AxisX.Add(new Axis());
            cartesianChart.AxisY.Add(CreateAxis(double.NaN, double.NaN));
            cartesianChart.AxisX[0].Title = "Replication";
            DataContext = this;
            Grid1.Children.Add(cartesianChart);
        }

        public Axis CreateAxis(double min, double max)
        {
            Axis axis = new Axis
            {
                MaxValue = max,
                MinValue = min,
                Title = "Average time"
            };
            return axis;
        }

        public void FreeAxisY()
        {
            cartesianChart.AxisY.RemoveAt(0);
            cartesianChart.AxisY.Add(CreateAxis(double.NaN, double.NaN));
        }

        public void CorrectAxisY()
        {
            
            double globalMinY = double.MaxValue;
            double globalMaxY = double.MinValue;
            for (int i = 0; i < cartesianChart.Series.Count; i++)
            {
                if (cartesianChart.Series[i].IsSeriesVisible)
                {
                    if (minY[i] < globalMinY)
                        globalMinY = minY[i];
                    if (maxY[i] > globalMaxY)
                        globalMaxY = maxY[i];
                }
            }
            foreach (var series in cartesianChart.Series)
            {
                if (series.Values.Count < 2)
                {
                    return;
                }
            }
            cartesianChart.AxisY.RemoveAt(0);
            cartesianChart.AxisY.Add(CreateAxis(globalMinY, globalMaxY));
        }

        public void AddValue(double x, double y, int seriesIndex)
        {
            cartesianChart.Series[seriesIndex].Values.Add(
                new ObservablePoint(x, y));
            if (y < minY[seriesIndex])
                minY[seriesIndex] = y;
            if (y > maxY[seriesIndex])
                maxY[seriesIndex] = y;
        }

        public void AddValues(List<ObservablePoint> values, int seriesIndex)
        {
            cartesianChart.Series[seriesIndex].Values.AddRange(values);
            double minLocal = values.Min((e) => e.Y);
            double maxLocal = values.Max((e) => e.Y);
            if (minLocal < minY[seriesIndex])
                minY[seriesIndex] = minLocal;
            if (maxLocal > maxY[seriesIndex])
                maxY[seriesIndex] = maxLocal;
        }

        public void SetSeriesVisibility(bool isVisible, int seriesIndex,bool correctAxis)
        {
            ((LineSeries)cartesianChart.Series[seriesIndex]).Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            if(correctAxis)
                CorrectAxisY();
        }

        public void Clear()
        {
            foreach (var series in cartesianChart.Series)
            {
                series.Values.Clear();
            }

            for (int i = 0; i < cartesianChart.Series.Count; i++)
            {
                minY[i] = Double.MaxValue;
                maxY[i] = Double.MinValue;
            }
        }
    }
}
