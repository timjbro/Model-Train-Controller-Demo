using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using static ModelTrainControllerDemo.SimpleSpeedDSP;

namespace ModelTrainControllerDemo
{
    /// <summary>
    /// Interaction logic for DirectionIndicator.xaml
    /// </summary>
    public partial class DirectionIndicator : UserControl
    {
        Color offColor = Colors.Gray;
        Color onColor = Colors.Orange;

        ColorAnimation flashColour;

        public DirectionIndicator()
        {
            InitializeComponent();

            flashColour = new ColorAnimation();
            flashColour.From = onColor;
            flashColour.To = offColor;
            flashColour.AutoReverse = true;
            flashColour.RepeatBehavior = RepeatBehavior.Forever;
            flashColour.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            SetDirectionIndicators();
        }

        private ReverserStates targetDirection = ReverserStates.Neutral;

        public ReverserStates TargetDirection
        {
            set
            {
                if (targetDirection != value)
                {
                    targetDirection = value;
                    SetDirectionIndicators();
                }
            }
        }

        private ReverserStates actualDirection = ReverserStates.Neutral;

        public ReverserStates ActualDirection
        {
            set
            {
                if (actualDirection != value)
                {
                    actualDirection = value;
                    SetDirectionIndicators();
                }
            }
        }

        private void SetDirectionIndicators()
        {
            rectForward.Fill = new SolidColorBrush(offColor);
            rectNeutral.Fill = new SolidColorBrush(offColor);
            rectReverse.Fill = new SolidColorBrush(offColor);
            if (targetDirection != actualDirection)
            {
                switch (targetDirection)
                {
                    case ReverserStates.Forward:
                        rectForward.Fill.BeginAnimation(SolidColorBrush.ColorProperty, flashColour);
                        break;
                    case ReverserStates.Neutral:
                        rectNeutral.Fill.BeginAnimation(SolidColorBrush.ColorProperty, flashColour);
                        break;
                    case ReverserStates.Reverse:
                        rectReverse.Fill.BeginAnimation(SolidColorBrush.ColorProperty, flashColour);
                        break;
                }
            }
            switch (actualDirection)
            {
                case ReverserStates.Forward:
                    rectForward.Fill = new SolidColorBrush(onColor);
                    break;
                case ReverserStates.Neutral:
                    rectNeutral.Fill = new SolidColorBrush(onColor);
                    break;
                case ReverserStates.Reverse:
                    rectReverse.Fill = new SolidColorBrush(onColor);
                    break;
            }
        }
    }
}
