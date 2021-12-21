using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
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

namespace GasStationModeling.ViewModel
{
    public class ModellingScreenViewModel : ViewModelBase
    {
        public ModellingSettings Settings { get; set; }

        public ObservableCollection<CarTableItem> CarTableItems { get; set; }

        public List<FuelTableItem> Fuels { get; set; }

        public double CurrentFuelVolume { get; set; }

        public double CurrentCashCount { get; set; }

        public ModellingScreenViewModel()
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            Settings = mainViewModel.ModellingSettings;
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

    }
}
