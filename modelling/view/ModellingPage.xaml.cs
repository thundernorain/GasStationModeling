using CommonServiceLocator;
using GasStationModeling.core.models;
using GasStationModeling.core.topology;
using GasStationModeling.modelling.model;
using GasStationModeling.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace GasStationModeling.modelling.view
{
    /// <summary>
    /// Interaction logic for ModellingPage.xaml
    /// </summary>
    public partial class ModellingPage : Page    
    {
        DispatcherTimer timer = new DispatcherTimer();
        ModellingScreenViewModel mscViewModel;

        public ModellingPage()
        {
            InitializeComponent();
            mscViewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();        
        }

        private void TransportGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.CurrentPageUri = new Uri(MainViewModel.SETTINGS_SCREEN_URI, UriKind.Relative);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            StationCanvas = mscViewModel.initializeStationCanvas(StationCanvas);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}
