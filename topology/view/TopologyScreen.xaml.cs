using CommonServiceLocator;
using GasStationModeling.core.topology;
using GasStationModeling.add_forms;
using GasStationModeling.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GasStationModeling.topology.view
{
    /// <summary>
    /// Логика взаимодействия для TopologyScreen.xaml
    /// </summary>
    public partial class TopologyScreen : Page
    {
        public TopologyScreen()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.CurrentPageUri = new Uri(MainViewModel.SETTINGS_SCREEN_URI, UriKind.Relative);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void CashBoxRectangle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.GetTopology.SelectedTopologyElement = TopologyElement.CashBox;
        }

        private void FuelDispenserRectangle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.GetTopology.SelectedTopologyElement = TopologyElement.FuelDispenser;
        }

        private void TankRectangle_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.GetTopology.SelectedTopologyElement = TopologyElement.Tank;
        }

        private void EntrancePolygon_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.GetTopology.SelectedTopologyElement = TopologyElement.Entrance;
        }

        private void ExitPolygon_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.GetTopology.SelectedTopologyElement = TopologyElement.Exit;
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            AddTopologyWindow addTopologyWindow = new AddTopologyWindow(mainViewModel.GetTopology);
            addTopologyWindow.Show();
        }
    }
}
