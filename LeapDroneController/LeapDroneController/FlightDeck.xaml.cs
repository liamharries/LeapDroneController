using System.Windows;
using LeapDroneController.StateManagement;

namespace LeapDroneController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = DroneStateManager.DroneData;
        }

        private void BtnFly_OnClick(object sender, RoutedEventArgs e)
        {
            LeapConnector.StartLeapInteraction();
        }

        private void BtnFinish_OnClick(object sender, RoutedEventArgs e)
        {
            LeapConnector.StopLeapInteraction();
        }
    }
}
