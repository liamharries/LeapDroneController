using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Leap;
using LeapDroneController.Annotations;
using LeapDroneController.Config;
using LeapDroneController.StateManagement;

namespace LeapDroneController.Models
{
    public enum Action
    {
        Left,
        Right,
        Up,
        Down,
        Forwards,
        Backwards,
        Clockwise,
        AntiClockwise,
        Hover
    }

    public class DroneData : INotifyPropertyChanged
    {
        private decimal _leftRightAngle;
        private decimal _thrust;
        private decimal _forwardsBackwardsAngle;
        private decimal _heading;

        private Action _currentAction = Action.Hover;

        public Action CurrentAction
        {
            get { return _currentAction; }
            private set
            {
                _currentAction = value;
                OnPropertyChanged();
            }
        }

        public XAxisState XAxisState { get; private set; }

        public YAxisState YAxisState { get; private set; }

        public ZAxisState ZAxisState { get; private set; }

        public HeadingState HeadingState { get; private set; }

        public decimal LeftRightAngle
        {
            get { return _leftRightAngle; }
            private set
            {
                _leftRightAngle = value;
                OnPropertyChanged();
            }
        }

        public decimal Thrust
        {
            get { return _thrust; }
            private set
            {
                _thrust = value;
                OnPropertyChanged();
            }
        }

        public decimal ForwardBackwardAngle
        {
            get { return _forwardsBackwardsAngle; }
            private set
            {
                _forwardsBackwardsAngle = value;
                OnPropertyChanged();
            }
        }

        public decimal Heading
        {
            get { return _heading; }
            private set
            {
                _heading = value;
                OnPropertyChanged();
            }
        }

        public void SetSafe()
        {
            CurrentAction = Action.Hover;
            UpdateValues();
        }

        public void UpdateGesture(Vector leftHandVector, Vector rightHandVector)
        {
            float xValue = leftHandVector.y - rightHandVector.y;
            float yValue = (leftHandVector.y + rightHandVector.y) / 2 - 150;
            float zValue = (leftHandVector.z + rightHandVector.z) / 2 - 50;
            float headingValue = rightHandVector.z - leftHandVector.z;

            CalculateGesture(xValue, yValue, zValue, headingValue);

            UpdateValues();
        }

        #region ValueUpdate

        private void UpdateValues()
        {
            UpdateLeftRightAngle();
            UpdateForwardBackwardAngle();
            UpdateThrust();
            UpdateHeading();
        }

        private void UpdateLeftRightAngle()
        {
            decimal val;
            switch (CurrentAction)
            {
                case Action.Left:
                    val = LeftRightAngle - ConfigData.LeftRightAngleChange;
                    LeftRightAngle = val < 0 - ConfigData.MaxLeftRightAngle ? 0 - ConfigData.MaxLeftRightAngle : val;
                    break;
                case Action.Right:
                    val = LeftRightAngle + ConfigData.LeftRightAngleChange;
                    LeftRightAngle = val > ConfigData.MaxLeftRightAngle ? ConfigData.MaxLeftRightAngle : val;
                    break;
                default:
                    LeftRightAngle = 0;
                    break;
            }
        }

        private void UpdateForwardBackwardAngle()
        {
            decimal val;
            switch (CurrentAction)
            {
                case Action.Forwards:
                    val = ForwardBackwardAngle + ConfigData.ForwardBackwardAngleChange;
                    ForwardBackwardAngle = val > ConfigData.MaxForwardBackAngle ? ConfigData.MaxForwardBackAngle : val;
                    break;
                case Action.Backwards:
                    val = ForwardBackwardAngle - ConfigData.ForwardBackwardAngleChange;
                    ForwardBackwardAngle = val < 0 - ConfigData.MaxForwardBackAngle ? 0 - ConfigData.MaxForwardBackAngle : val;
                    break;
                default:
                    ForwardBackwardAngle = 0;
                    break;
            }
        }

        private void UpdateThrust()
        {
            decimal val;
            switch (CurrentAction)
            {
                case Action.Up:
                    val = Thrust + ConfigData.ThrustValueChange;
                    Thrust = val > ConfigData.MaxThrust ? ConfigData.MaxThrust : val;
                    break;
                case Action.Down:
                    val = Thrust - ConfigData.ThrustValueChange;
                    Thrust = val < ConfigData.MinThrust ? ConfigData.MinThrust : val;
                    break;
                default:
                    Thrust = ConfigData.HoverThrust;
                    break;
            }
        }

        private void UpdateHeading()
        {
            switch (CurrentAction)
            {
                case Action.Clockwise:
                    {
                        decimal val = Heading + ConfigData.HeadingValueChange;
                        if (val >= 360)
                        {
                            Heading = val - 360;
                        }
                        else
                        {
                            Heading = val;
                        }
                    }
                    break;
                case Action.AntiClockwise:
                    {
                        decimal val = Heading - ConfigData.HeadingValueChange;
                        if (val < 0)
                        {
                            Heading = 360 - Math.Abs(val);
                        }
                        else
                        {
                            Heading = val;
                        }
                    }
                    break;
            }
        }

        #endregion//ValueUpdate

        private void CalculateGesture(float xValue, float yValue, float zValue, float heading)
        {
            //Check for xValue as gesture source.
            if (Math.Abs(xValue) > Math.Abs(yValue) &&
                Math.Abs(xValue) > Math.Abs(zValue) &&
                Math.Abs(xValue) > Math.Abs(heading))
            {
                if (Math.Abs(xValue) > 50)
                {
                    if (Math.Abs(Math.Abs(xValue) - xValue) > 0.5)
                    {
                        CurrentAction = Action.Left;
                        return;
                    }
                    CurrentAction = Action.Right;
                    return;
                }
                CurrentAction = Action.Hover;
                return;
            }
            //Check for yValue as gesture source.
            if (Math.Abs(yValue) > Math.Abs(xValue) &&
                Math.Abs(yValue) > Math.Abs(zValue) &&
                Math.Abs(yValue) > Math.Abs(heading))
            {
                if (Math.Abs(yValue) > 50)
                {
                    if (Math.Abs(Math.Abs(yValue) - yValue) > 0.5)
                    {
                        CurrentAction = Action.Down;
                        return;
                    }
                    CurrentAction = Action.Up;
                    return;
                }
                CurrentAction = Action.Hover;
                return;
            }
            //Check for zValue as gesture source.
            if (Math.Abs(zValue) > Math.Abs(xValue) &&
                Math.Abs(zValue) > Math.Abs(yValue) &&
                Math.Abs(zValue) > Math.Abs(heading))
            {
                if (Math.Abs(zValue) > 50)
                {
                    if (Math.Abs(Math.Abs(zValue) - zValue) > 0.5)
                    {
                        CurrentAction = Action.Forwards;
                        return;
                    }
                    CurrentAction = Action.Backwards;
                    return;
                }
                CurrentAction = Action.Hover;
                return;
            }

            //Check for heading as gesture source.
            if (Math.Abs(heading) > 50)
            {
                if (Math.Abs(Math.Abs(heading) - heading) > 0.5)
                {
                    CurrentAction = Action.AntiClockwise;
                    return;
                }
                CurrentAction = Action.Clockwise;
                return;
            }
            CurrentAction = Action.Hover;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
