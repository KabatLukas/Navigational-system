using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Navigacny_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int GRAPHNUMOFDOTS = 1000;
        private double[] averages;
        private List<double> averagePercentInLimit;
        private CancelToken cancelToken;

        public MainWindow()
        {
            InitializeComponent();
            SimulateButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            for (int i = 1; i < 4; i++)
            {
                AvgLineChart.SetSeriesVisibility(false, i, false);
            }
            cancelToken = new CancelToken(new CancellationTokenSource());
        }

        private async void SimulateButton_OnClick(object sender, RoutedEventArgs e)
        {
            int replications = 0;
            int skipFirst = 0;
            SimulateButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            if (!ValidateInput(out replications, out skipFirst))
            {
                SimulateButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                return;
            }
            AvgLineChart.FreeAxisY();
            averages = new double[4];

            StopButton.Click += (o, args) =>
            {
                cancelToken.Source.Cancel();
            };
            await Task.Run(() =>
            {
                SimulationCore simulation = new NavigationSimulation();
                AvgLineChart.Clear();
                    //double lastUpdate = 0;
                    simulation.AverageChangedE += (average, replicationN) =>
                {
                    if (replicationN > skipFirst)
                            // This will give the total number of milliseconds from Jan 1, 00001
                            //if ((new TimeSpan(DateTime.Now.Ticks)).TotalMilliseconds - lastUpdate > 1/60d)
                            if (replications / GRAPHNUMOFDOTS > 1)
                        {
                            if (replicationN % ((replications - skipFirst) / GRAPHNUMOFDOTS) == 0)
                            {
                                for (int i = 0; i < simulation.Average.Count; i++)
                                {
                                    AvgLineChart.AddValue(replicationN, average[i], i);
                                }
                                    //lastUpdate = (new TimeSpan(DateTime.Now.Ticks)).TotalMilliseconds;
                                }
                        }
                        else
                        {
                            for (int i = 0; i < simulation.Average.Count; i++)
                            {
                                AvgLineChart.AddValue(replicationN, average[i], i);
                            }
                        }
                };
                ((NavigationSimulation)simulation).InTimeChangedE += newAverages =>
                       {
                        averagePercentInLimit = newAverages;
                    };
                simulation.Simulate(replications, cancelToken);
                if (simulation.Average.Count >= 4)
                {
                    averages[0] = simulation.Average[0];
                    averages[1] = simulation.Average[1];
                    averages[2] = simulation.Average[2];
                    averages[3] = simulation.Average[3];
                }
            });
            AvgLineChart.CorrectAxisY();

            DurationTextBox.Text =
                "A-B-C-D-E=" + averages[0] + ", A-F-G-E=" + averages[1] + ", A-F-H-D-E=" + averages[2] +
                    ", A-F-H-C-D-E=" + averages[3] + ". On time in " + string.Format("{0,8:P4}", GetPercentageForMin()) + ".";
            SimulateButton.IsEnabled = true;
            StopButton.IsEnabled = false;

            cancelToken.Source.Dispose();
            cancelToken.Source = new CancellationTokenSource();
        }

        private bool ValidateInput(out int replications, out int skipFirst)
        {
            if ((ReplTextBox.Text).Length == 0 || !int.TryParse(ReplTextBox.Text, out replications))
            {
                ReplTextBox.Background = Brushes.IndianRed;
                replications = 0;
                skipFirst = 0;
                return false;
            }
            ReplTextBox.Background = Brushes.White;
            if ((SkipTextBox.Text).Length == 0 || !int.TryParse(SkipTextBox.Text, out skipFirst))
            {
                SkipTextBox.Background = Brushes.IndianRed;
                replications = 0;
                skipFirst = 0;
                return false;
            }
            SkipTextBox.Background = Brushes.White;
            return true;
        }

        private double GetPercentageForMin()
        {
            if (averages.Length > 0)
            {
                int minI = 0;
                double minAvg = double.MaxValue;
                for (int i = 0; i < averages.Length; i++)
                {
                    if (averages[i] < minAvg)
                    {
                        minAvg = averages[i];
                        minI = i;
                    }
                }

                return averagePercentInLimit[minI];
            }

            return 0;
        }

        private void ABCDECheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(true, 0, true);
        }

        private void AFGECheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(true, 1, true);
        }

        private void AFHDECheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(true, 2, true);
        }

        private void AFHCDECheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(true, 3, true);
        }

        private void ABCDECheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(false, 0, true);
        }

        private void AFGECheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(false, 1, true);
        }

        private void AFHDECheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(false, 2, true);
        }

        private void AFHCDECheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AvgLineChart?.SetSeriesVisibility(false, 3, true);
        }
    }
}