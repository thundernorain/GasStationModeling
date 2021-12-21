using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.core.topology;
using GasStationModeling.DB;
using GasStationModeling.modelling.mapper;
using GasStationModeling.modelling.model;
using GasStationModeling.settings_screen.model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GasStationModeling.ViewModel
{
    public class ModellingScreenViewModel : ViewModelBase
    {
        #region Fields
        public ModellingSettings Settings { get; set; }

        public Topology CurrentTopology { get; set; }

        public ObservableCollection<CarTableItem> CarTableItems { get; set; }

        public List<FuelTableItem> Fuels { get; set; }

        private double _currentCash;

        private double _currentFuelVolume;

        public string CurrentCashView { get; set; }

        public string CurrentFuelVolumeView { get; set; }

        public double CurrentCashCount
        {
            get => _currentCash;
            set
            {
                _currentCash = value;
                CurrentCashView = "Объём денег в кассе (руб) : " + _currentCash + " \\ " + Settings.CashLimit + " руб";
            }
        }

        public double CurrentFuelVolume
        {
            get => _currentFuelVolume;
            set
            {
                _currentFuelVolume = value;
                CurrentFuelVolumeView = "Объём топлива в ТБ (м3) : " + _currentFuelVolume + " \\ " + Settings.FuelTank.MaxVolume + " м3";
            }
        }
        #endregion 

        public ModellingScreenViewModel()
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            Settings = mainViewModel.ModellingSettings;
            CurrentTopology = mainViewModel.GetTopology;
            var db = DbInitializer.getInstance();

            List<Car> cars = getCarsFromDB(db);
            CarTableItems = ModellingScreenMapper.CreateCarTableItems(cars,Settings.Fuels);
            Fuels = ModellingScreenMapper.CreateFuelTableitems(Settings.Fuels);

            CurrentCashCount = 0;
            CurrentFuelVolume = Settings.FuelTank.MaxVolume;
        }

        public List<Car> getCarsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Car>(db, DBWorkerKeys.CARS_KEY);
            return dbWorker.getCollection();
        }


        public Canvas initializeStationCanvas(Canvas modelCanvas)
        {
            int width = CurrentTopology.TopologyColumnCountMain + CurrentTopology.TopologyColumnCountWorker + 1;
            int height = CurrentTopology.TopologyRowCount;
            modelCanvas.Width =  width * 48;
            modelCanvas.Height = height * 48;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                   // modelCanvas.
                }
            }
            return modelCanvas;
        }

    }
}
