using Leap;
using LeapDroneController.Models;

namespace LeapDroneController.StateManagement
{
    public enum XAxisState
    {
        TiltedLeft,
        Stable,
        TiltedRight
    }

    public enum YAxisState
    {
        IncreasingAltitude,
        Stable,
        DecreasingAltitude
    }

    public enum ZAxisState
    {
        Advancing,
        Stable,
        Retreating
    }

    public enum HeadingState
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public static class DroneStateManager
    {
        private static bool _userConnectionState;

        static DroneStateManager()
        {
            IsFlying = false;
            DroneData = new DroneData();
        }

        public static bool IsFlying { get; private set; }

        public static DroneData DroneData { get; set; }

        public static bool UserConnectionState
        {
            get { return _userConnectionState; }
            set
            {
                _userConnectionState = value;
                if (!value)
                {
                    SetDroneToSafe();
                }
            }
        }

        public static void SetDroneToSafe()
        {
            if (IsFlying)
            {
                DroneData.SetSafe();
            }
        }

        public static void Update(Hand leftHand, Hand rightHand)
        {
            //Do some validation here

            Vector lhv = leftHand.PalmPosition;
            Vector rhv = rightHand.PalmPosition;

            DroneData.UpdateGesture(lhv,rhv);

            //Set Variables or safe
        }
    }
}
