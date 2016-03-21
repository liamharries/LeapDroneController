using System.Timers;

namespace LeapDroneController.Models
{
    public class LeapItem
    {
        public LeapItem()
        {
            IsRecognised = false;
            XPosition = 0;
            YPosition = 0;
            ZPosition = 0;
        }

        private Timer InactivityTimer { get; set; }

        public bool IsRecognised { get; private set; }

        public decimal XPosition { get; private set; }

        public decimal YPosition { get; private set; }

        public decimal ZPosition { get; private set; }

        public void Update(decimal xPosition, decimal yPosition, decimal zPosition)
        {
            if (xPosition == 0 || yPosition == 0 || zPosition == 0)
            {
                Inactive();
            }
            else
            {
                StartInactivityTimer();
                XPosition = xPosition;
                YPosition = yPosition;
                ZPosition = zPosition;
            }
        }

        private void Inactive()
        {
            StopInactivityTimer();
            IsRecognised = false;
            XPosition = 0;
            YPosition = 0;
            ZPosition = 0;
        }

        private void StartInactivityTimer()
        {
            IsRecognised = true;
            InactivityTimer = new Timer();
            InactivityTimer.Interval = 500;
            InactivityTimer.Elapsed += delegate { Inactive(); };
            InactivityTimer.Start();
        }

        private void StopInactivityTimer()
        {
            if (InactivityTimer == null) return;
            InactivityTimer.Stop();
            InactivityTimer.Enabled = false;
            InactivityTimer.Dispose();
        }
    }
}
