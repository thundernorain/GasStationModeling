using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.add_forms;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.core.topology;
using GasStationModeling.DB;
using GasStationModeling.exceptions;
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
        private string currentCashView;
        private string currentFuelVolumeView;
        private ObservableCollection<CarTableItem> carTableItems;

        #region Fields
        public ModellingSettings Settings { get; set; }

        public Topology CurrentTopology { get; set; }

        public ObservableCollection<CarTableItem> CarTableItems
        {
            get => carTableItems;
            set
            {
                carTableItems = value;
                RaisePropertyChanged(() => CarTableItems);
            }
        }

        public List<FuelTableItem> Fuels { get; set; }

        public CashBoxView CashBoxView { get; set; }

        public List<TankView> TankViews { get; set; }

        public List<Car> Cars { get; set; }

        public string CurrentCashView
        {
            get => currentCashView;
            set
            {
                currentCashView = value;
                RaisePropertyChanged(() => CurrentCashView);
            }
        }

        public string CurrentFuelVolumeView
        {
            get => currentFuelVolumeView;
            set
            {
                currentFuelVolumeView = value;
                RaisePropertyChanged(() => CurrentFuelVolumeView);
            }
        }

            
        #endregion 

        public ModellingScreenViewModel()
        {
            try
            {
                var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
                Settings = mainViewModel.ModellingSettings;
                CurrentTopology = mainViewModel.GetTopology;
                var db = DbInitializer.getInstance();

                Cars = getCarsFromDB(db);
                CarTableItems = ModellingScreenMapper.CreateCarTableItems(Cars, Settings.Fuels);
                Fuels = ModellingScreenMapper.CreateFuelTableitems(Settings.Fuels);
            }
            catch(DbErrorException e)
            {
                ErrorMessageBoxShower.ShowError(DbErrorMessage.CONNECTION_ERROR);
            }
        }

        public List<Car> getCarsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Car>(db, DBWorkerKeys.CARS_KEY);
            return dbWorker.getCollection();
        }
        
    }
}
