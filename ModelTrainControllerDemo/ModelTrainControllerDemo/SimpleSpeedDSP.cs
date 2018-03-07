using System;
using System.Timers;

namespace ModelTrainControllerDemo
{
    public class SimpleSpeedDSP
    {
        const double adcMaxValue = 10;
        const double pwmMaxValue = 100;
        const double pwmMinValue = 10;
        const double accRatio = 0.005;
        const double decRate = 0.01;

        Timer speedUpdateTimer = new Timer();
        public event EventHandler NewSpeedAvailable;

        public double PowerLeverPosition { get; set; }
        public double TargetSpeed { get; set; }
        public double ActualSpeed { get; set; }

        public enum ReverserStates { Reverse, Neutral, Forward };
        public ReverserStates ReverserLeverPosition { get; set; }
        public ReverserStates ActualDirection { get; set; }

        public enum BreakStates { Release, On1, On2, FullService, Emergency };
        public BreakStates BreakLeverPosition { get; set; }

        public SimpleSpeedDSP()
        {
            speedUpdateTimer.Interval = 100;
            speedUpdateTimer.AutoReset = true;
            speedUpdateTimer.Elapsed += SpeedUpdateTimer_Elapsed;
            speedUpdateTimer.Start();
        }

        private void SpeedUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ActualSpeed < pwmMinValue)
            {
                if (TargetSpeed == 0 || BreakLeverPosition != BreakStates.Release)
                    ActualSpeed -= pwmMinValue / 10;
                else
                    ActualSpeed += pwmMinValue / 10;
            }
            else
            {
                ActualSpeed = ActualSpeed * (1.0 - ((double)BreakLeverPosition * decRate));
                ActualSpeed = ActualSpeed * (1.0 - accRatio) + TargetSpeed * accRatio;
            }
            SetDirection();
            SetTargetSpeed();
            NewSpeedAvailable?.Invoke(this, null);
        }

        private void TrackBarPower_ValueChanged(object sender, EventArgs e)
        {
            SetTargetSpeed();
        }

        private void TrackBarReverser_ValueChanged(object sender, EventArgs e)
        {
            SetDirection();
            SetTargetSpeed();
        }

        private void SetDirection()
        {
            if (ActualSpeed <= 0)
            {
                ActualSpeed = 0;
                ActualDirection = ReverserLeverPosition;
            }
        }

        private void SetTargetSpeed()
        {

            if ((ReverserLeverPosition == ReverserStates.Reverse && ActualDirection == ReverserStates.Reverse)
                || (ReverserLeverPosition == ReverserStates.Forward && ActualDirection == ReverserStates.Forward))
            {
                double setSpeed = (PowerLeverPosition / adcMaxValue) * pwmMaxValue;
                if (setSpeed < pwmMinValue)
                    TargetSpeed = 0;
                else
                    TargetSpeed = setSpeed;
            }
            else
                TargetSpeed = 0;
        }
    }
}
