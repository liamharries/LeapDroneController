using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Leap;
using LeapDroneController.Annotations;
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

    public class DroneData:INotifyPropertyChanged
    {
        public DroneData()
        {
            XAxisState = XAxisState.Stable;
            YAxisState = YAxisState.Stable;
            ZAxisState = ZAxisState.Stable;
            XAxisValue = 0;
            YAxisValue = 0;
            ZAxisValue = 0;
        }

        private Action _currentAction = Action.Hover;

        public Action CurrentAction
        {
            get { return _currentAction; }
            private set
            {
                _currentAction = value;
                OnPropertyChanged(nameof(CurrentAction));
            }
        }

        public XAxisState XAxisState { get; private set; }

        public YAxisState YAxisState { get; private set; }

        public ZAxisState ZAxisState { get; private set; }

        public HeadingState HeadingState { get; private set; }

        public float XAxisValue { get; private set; }

        public float YAxisValue { get; private set; }

        public float ZAxisValue { get; private set; }

        public float HeadingValue { get; private set; }

        public void SetSafe()
        {
            XAxisState = XAxisState.Stable;
            YAxisState = YAxisState.Stable;
            ZAxisState = ZAxisState.Stable;
            XAxisValue = 0;
            YAxisValue = 0;
            ZAxisValue = 0;
        }

        public void SetValues(Vector leftHandVector, Vector rightHandVector)
        {
            float xValue = leftHandVector.y - rightHandVector.y;
            float yValue = (leftHandVector.y + rightHandVector.y) / 2 - 150;
            float zValue = (leftHandVector.z + rightHandVector.z)/2 - 50;
            float headingValue = rightHandVector.z - leftHandVector.z;

            XAxisValue = xValue;
            YAxisValue = yValue;
            ZAxisValue = zValue;
            HeadingValue = headingValue;

            OnPropertyChanged(nameof(XAxisValue));
            OnPropertyChanged(nameof(YAxisValue));
            OnPropertyChanged(nameof(ZAxisValue));
            OnPropertyChanged(nameof(HeadingValue));

            CalculateGesture(XAxisValue, YAxisValue, ZAxisValue, HeadingValue);
        }

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
                Hover();
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
                Hover();
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
                Hover();
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
            Hover();
        }

        private void Hover()
        {
            CurrentAction = Action.Hover;
            //X and Z angles need to be reset
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
