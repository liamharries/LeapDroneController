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
        static DroneStateManager()
        {
            DroneData = new DroneData();
        }

        public static DroneData DroneData { get; set; }

        public static void SetDroneToSafe()
        {
            DroneData.SafeMode = true;
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
