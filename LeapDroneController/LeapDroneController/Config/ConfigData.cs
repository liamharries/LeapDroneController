namespace LeapDroneController.Config
{
    public static class ConfigData
    {
        public static int MaxAltitudeFt { get; private set; } = 500;

        public static double MaxLeftRightAngle { get; private set; } = 30;

        public static double MaxForwardBackAngle { get; private set; } = 30;

        public static int ErrorToleranceBeforeSafeMode { get; private set; } = 20;
    }
}
