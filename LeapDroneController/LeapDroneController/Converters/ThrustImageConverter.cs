using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using LeapDroneController.Config;

namespace LeapDroneController.Converters
{
    public class ThrustImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal thrust = 0;
            decimal.TryParse(value.ToString(), out thrust);
            decimal max = ConfigData.MaxThrust;
            decimal min = ConfigData.MinThrust;
            decimal hover = ConfigData.HoverThrust;

            if (thrust == hover)
            {
                return new Image() { Source = new BitmapImage(new Uri("../../Images/DroneLevel.png",UriKind.Relative)) };
            }

            if (thrust > hover)
            {
                decimal hoverMaxDiff = max - hover;
                if (thrust > hover + (hoverMaxDiff / 2))
                {
                    return new Image()
                    { Source = new BitmapImage(new Uri("../../Images/DroneLevel_Up2.png", UriKind.Relative)) };
                }
                return new Image()
                { Source = new BitmapImage(new Uri("../../Images/DroneLevel_Up.png", UriKind.Relative)) };
            }

            decimal hoverMinDiff = hover - min;
            if (thrust < hover - (hoverMinDiff / 2))
            {
                return new Image() { Source = new BitmapImage(new Uri("../../Images/DroneLevel_Down2.png", UriKind.Relative)) };
            }
            return new Image() { Source = new BitmapImage(new Uri("../../Images/DroneLevel_Down.png", UriKind.Relative)) };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
