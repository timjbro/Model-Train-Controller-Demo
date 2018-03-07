using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static ModelTrainControllerDemo.SimpleSpeedDSP;

namespace ModelTrainControllerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SimpleSpeedDSP SpeedDSP { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SpeedDSP = new SimpleSpeedDSP();
            SpeedDSP.NewSpeedAvailable += NewSpeedAvailable;
            SpeedDSP.ReverserLeverPosition = ReverserStates.Neutral;
            breakSlider.Value = 3;
        }

        private void NewSpeedAvailable(object sender, EventArgs e)
        {
            try
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() => NewSpeedAvailable(sender, e));
                    return;
                }
            }
            catch
            {
                return;
            }
            if (sender is SimpleSpeedDSP speedDSP)
            {
                directionIndicator.ActualDirection = speedDSP.ActualDirection;
                speedLabel.Content = speedDSP.ActualSpeed.ToString("N1");
                progressSpeed.Value = speedDSP.ActualSpeed;
            }
        }

        private void PowerSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SpeedDSP.PowerLeverPosition = (sender as Slider).Value;
        }

        private void ReverserSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((sender as Slider).Value)
            {
                case -1:
                    directionIndicator.TargetDirection = ReverserStates.Reverse;
                    SpeedDSP.ReverserLeverPosition = ReverserStates.Reverse;
                    break;
                case 0.0:
                    directionIndicator.TargetDirection = ReverserStates.Neutral;
                    SpeedDSP.ReverserLeverPosition = ReverserStates.Neutral;
                    break;
                case 1:
                    directionIndicator.TargetDirection = ReverserStates.Forward;
                    SpeedDSP.ReverserLeverPosition = ReverserStates.Forward;
                    break;
            }
        }

        private void BreakSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SpeedDSP.BreakLeverPosition = (BreakStates)(sender as Slider).Value;
            switch (SpeedDSP.BreakLeverPosition)
            {
                case BreakStates.Release:
                    breakLabel.Content = "Release";
                    breakLabel.Foreground = new SolidColorBrush(Colors.LightGray);
                    break;
                case BreakStates.On1:
                    breakLabel.Content = "1";
                    breakLabel.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case BreakStates.On2:
                    breakLabel.Content = "2";
                    breakLabel.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case BreakStates.FullService:
                    breakLabel.Content = "Full Service";
                    breakLabel.Foreground = new SolidColorBrush(Colors.Orange);
                    break;
                case BreakStates.Emergency:
                    breakLabel.Content = "Emergency";
                    breakLabel.Foreground = new SolidColorBrush(Colors.Red);
                    break;
            }
        }
    }
}
