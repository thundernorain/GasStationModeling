using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.core;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using GasStationModeling.settings_screen.mapper;
using GasStationModeling.settings_screen.model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace GasStationModeling.ViewModel
{
    public class SettingsScreenViewModel : ViewModelBase
    {
        #region Fields
        private string _selectedFuel;
        private List<FuelComboBoxItem> _fuels = new List<FuelComboBoxItem>();

        private string _selectedFuelTank;
        private List<FuelTankComboBoxItem> _fuelTanks = new List<FuelTankComboBoxItem>();

        private string _selectedFuelDispenser;
        private List<FuelDispenserComboBoxItem> _fuelDispensers = new List<FuelDispenserComboBoxItem>();
        #endregion

        #region Properties
        public string SelectedFuel
        {
            get => _selectedFuel;
            set
            {
                _selectedFuel = value;
                RaisePropertyChanged(() => SelectedFuel);
            }
        }

        public List<FuelComboBoxItem> Fuels
        {
            get => _fuels;
            set
            {
                _fuels = value;
                RaisePropertyChanged(() => Fuels);
            }
        }

        public string SelectedFuelTank
        {
            get => _selectedFuelTank;
            set
            {
                _selectedFuelTank = value;
                RaisePropertyChanged(() => SelectedFuelTank);
            }
        }

        public List<FuelTankComboBoxItem> FuelTanks
        {
            get => _fuelTanks;
            set
            {
                _fuelTanks = value;
                RaisePropertyChanged(() => FuelTanks);
            }
        }

        public string SelectedFuelDispenser
        {
            get => _selectedFuelDispenser;
            set
            {
                _selectedFuelDispenser = value;
                RaisePropertyChanged(() => SelectedFuelDispenser);
            }
        }

        public List<FuelDispenserComboBoxItem> FuelDispensers
        {
            get => _fuelDispensers;
            set
            {
                _fuelDispensers = value;
                RaisePropertyChanged(() => FuelDispensers);
            }
        }
        #endregion

        public SettingsScreenViewModel()
        {
            var db = DbInitializer.getInstance();

            getFuelsFromDB(db);
            getFuelTanksFromDB(db);
            getFuelDispensersFromDB(db);
        }

        #region GetFromDBMethods
        private void getFuelsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Fuel>(db, DBWorkerKeys.FUEL_TYPES_KEY);
            var collectionFromDb = dbWorker.getCollection();

            Fuels = new FuelToFuelComboBoxMapper().MapList(collectionFromDb);
            SelectedFuel = Fuels[0].Name;
        }

        private void getFuelTanksFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Tank>(db, DBWorkerKeys.FUEL_TANKS_KEY);
            var collectionFromDb = dbWorker.getCollection();

            FuelTanks = new TankToFuelTankComboBoxItemMapper().MapList(collectionFromDb);
            SelectedFuelTank = FuelTanks[0].Name;
        }

        private void getFuelDispensersFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<FuelDispenser>(db, DBWorkerKeys.FUEL_DISPENSERS_KEY);
            var collectionFromDb = dbWorker.getCollection();

            FuelDispensers = new FuelDispenserToFuelDispenserComboBoxItemMapper().MapList(collectionFromDb);
            SelectedFuelDispenser = FuelDispensers[0].Name;
        }
        #endregion

    }
}
