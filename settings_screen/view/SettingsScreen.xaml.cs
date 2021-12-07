using CommonServiceLocator;
using GasStationModeling.add_forms;
using GasStationModeling.ViewModel;
using System;
using System.Collections.Generic;
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

namespace GasStationModeling.settings_screen.view
{
    /// <summary>
    /// Interaction logic for SettingsScreen.xaml
    /// </summary>
    public partial class SettingsScreen : Page
    {
        public SettingsScreen()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.CurrentPageUri = new Uri(MainViewModel.TOPOLOGY_SCREEN_URI, UriKind.Relative);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddModelTrkWindow();
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new AddFuelTankWindow();
            window.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var window = new AddFuelTypeWindow();
            window.Show();
        }
    }
}
