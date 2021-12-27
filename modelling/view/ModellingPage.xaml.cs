using CommonServiceLocator;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.managers;
using GasStationModeling.modelling.mapper;
using GasStationModeling.settings_screen.model;
using GasStationModeling.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GasStationModeling.modelling.view
{
    /// <summary>
    /// Interaction logic for ModellingPage.xaml
    /// </summary>
    public partial class ModellingPage : Page    
    {
        ModellingTimeHelper timeHelper;
        DispatcherTimer timer;
        ModellingScreenViewModel mscViewModel;
        ModellingEngine engine;

        CanvasParser parsedCanvas;

        bool IsPaused;

        public ModellingPage()
        {
            InitializeComponent();
            mscViewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();
            TopologyMapper mapper = new TopologyMapper(mscViewModel.Settings, mscViewModel.CurrentTopology);
            parsedCanvas = mapper.mapTopology(StationCanvas);
            StationCanvas = parsedCanvas.StationCanvas;

            timer = new DispatcherTimer();
            timeHelper = new ModellingTimeHelper(timer);
            IsPaused = true;

            engine = new ModellingEngine(
                timeHelper,              
                mscViewModel.Settings,
                parsedCanvas,
                mscViewModel.Cars);
            
        }

        public void ModellingProcess()
        {
            while (true) { 
                engine.Tick(IsPaused);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.CurrentPageUri = new Uri(MainViewModel.SETTINGS_SCREEN_URI, UriKind.Relative);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            IsPaused = false;
            ModellingProcess();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            IsPaused = true;
        }
    }
}
