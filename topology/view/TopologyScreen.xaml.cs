using CommonServiceLocator;
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            AddTopologyWindow addTopologyWindow = new AddTopologyWindow(mainViewModel.GetTopology);
            addTopologyWindow.Show();
        }
    }
}
