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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GasStationModeling.ViewModel
{
    public class ModellingScreenViewModel : ViewModelBase
    {
         #region Fields
        public ModellingSettings Settings { get; set; }

        public Topology CurrentTopology { get; set; }

        public ObservableCollection<CarTableItem> CarTableItems { get; set; }

        public List<FuelTableItem> Fuels { get; set; }

        public CashBoxView CashBoxView { get; set; }

        public List<TankView> TankViews { get; set; }

        public List<Car> Cars { get; set; }

        public string CurrentFuelVolumeView = "22";

        public string CurrentCashView = "11";

            
        #endregion 

        public ModellingScreenViewModel()
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            Settings = mainViewModel.ModellingSettings;
            CurrentTopology = mainViewModel.GetTopology;
            var db = DbInitializer.getInstance();

            Cars = getCarsFromDB(db);
            CarTableItems = ModellingScreenMapper.CreateCarTableItems(Cars,Settings.Fuels);
            Fuels = ModellingScreenMapper.CreateFuelTableitems(Settings.Fuels);
        }

        public List<Car> getCarsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Car>(db, DBWorkerKeys.CARS_KEY);
            return dbWorker.getCollection();
        }
        
    }
}
