namespace LeapDroneController.Config
{
    public static class ConfigData
    {
        public static decimal LeftRightAngleChange { get; internal set; } = 2;
        public static decimal ForwardBackwardAngleChange { get; internal set; } = 2;
        public static decimal ThrustValueChange { get; private set; } = 5;
        public static decimal HoverThrust { get; private set; } = 30;
        public static decimal HeadingValueChange { get; private set; } = 5;

        public static decimal MaxLeftRightAngle { get; private set; } = 30;
        public static decimal MaxForwardBackAngle { get; private set; } = 30;
        public static decimal MaxThrust { get; private set; } = 100;
        public static decimal MinThrust { get; private set; } = 5;

        public static int ErrorToleranceBeforeSafeMode { get; private set; } = 20;
    }
}
