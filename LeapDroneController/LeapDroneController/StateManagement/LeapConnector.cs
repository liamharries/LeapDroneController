using System;
using System.Threading;
using System.Windows;
using Leap;
using LeapDroneController.Config;
using Timer = System.Timers.Timer;

namespace LeapDroneController.StateManagement
{
    public static class LeapConnector
    {
        private static int errorCount = 0;

        static LeapConnector()
        {
            FrameLoadTimer = new Timer();
            FrameLoadTimer.Interval = 50;
            FrameLoadTimer.Elapsed += delegate { LoadData(); };
        }

        private static Timer FrameLoadTimer { get; set; }

        private static Controller Controller { get; set; }

        public static void StartLeapInteraction()
        {
            if (Controller == null)
            {
                try
                {
                Controller = new Leap.Controller();
                Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    ControllerNotConnected();
                    return;
                }
            }
            if (Controller.IsConnected)
            {
                FrameLoadTimer.Start();
            }
            else
            {
                ControllerNotConnected();
            }
        }

        public static void StopLeapInteraction()
        {
            FrameLoadTimer.Stop();
        }

        private static void LoadData()
        {
            if (!Controller.IsConnected)
            {
                ControllerNotConnected();
                return;
            }
            HandList hands = Controller.Frame().Hands;
            if (hands.Count == 2)
            {
                DroneStateManager.Update(hands.Leftmost,hands.Rightmost);
                errorCount = 0;
            }
            else
            {
                if (errorCount < ConfigData.ErrorToleranceBeforeSafeMode)
                {
                    errorCount++;
                }
                else
                {
                    DroneStateManager.SetDroneToSafe();
                    errorCount = 0;
                }
            }
        }

        private static void ControllerNotConnected()
        {
            DroneStateManager.SetDroneToSafe();
            FrameLoadTimer.Stop();
            MessageBox.Show(
                "Controller not connected! \n" +
                "Please ensure your Leap Motion device is connected" +
                "and the Leap service is running.",
                "Controller Error!");
        }
    }
}
