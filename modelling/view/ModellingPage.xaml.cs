using CommonServiceLocator;
using GasStationModeling.add_forms;
using GasStationModeling.core.models;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.managers;
using GasStationModeling.modelling.mapper;
using GasStationModeling.settings_screen.model;
using GasStationModeling.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

            try
            {
                mscViewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();
                TopologyMapper mapper = new TopologyMapper(mscViewModel.Settings, mscViewModel.CurrentTopology);
                parsedCanvas = mapper.mapTopology(StationCanvas);
                StationCanvas = parsedCanvas.StationCanvas;

                timer = new DispatcherTimer();
                timeHelper = new ModellingTimeHelper(timer);

                var cars = filterCarList(mscViewModel.Cars,mscViewModel.Settings);
                engine = new ModellingEngine(
                    timeHelper,
                    mscViewModel.Settings,
                    parsedCanvas,
                    cars);

                setUpTimer();
            }
            catch(Exception ex)
            {
                ErrorMessageBoxShower.show(ex.Message);
            }       
        }

        public List<Car> filterCarList(List<Car> cars,ModellingSettings settings)
        {
            var carsFiltered = cars.Where(car => settings.Fuels.Exists(fuel => fuel.Name == car.TypeFuel)).ToList();
            if (carsFiltered == null || carsFiltered.Count == 0) throw new Exception("Обновите БД, моделей машин с таким топливом нет");
            return carsFiltered;
        }

        public void setUpTimer()
        {
            timer.Interval = TimeSpan.FromMilliseconds(ModellingTimeHelper.TIMER_TICK_MILLISECONDS);
            timer.Tick += ModellingProcess;
        }


        void ModellingProcess(object sender, EventArgs e)
        {
            engine.Tick(IsPaused);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            timer.Stop();
            IsPaused = true;
            mainViewModel.ModellingSettings = null;
            mainViewModel.CurrentPageUri = new Uri(MainViewModel.SETTINGS_SCREEN_URI, UriKind.Relative);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            IsPaused = false;
            timer.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            IsPaused = true;
        }
    }
}
