using GasStationModeling.developers_info_window.view;
using GasStationModeling.main_window.view;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GasStationModeling.welcome_window.view
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-EN");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var systemInfoFile = "..\\..\\system_info.html";

            System.Diagnostics.Process.Start(systemInfoFile);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var developersInfoWindow = new DevelopersInfoWindow();
            developersInfoWindow.Show();
        }
    }
}
