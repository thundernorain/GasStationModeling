using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using GasStationModeling.exceptions;
using GasStationModeling.settings_screen.mapper;
using GasStationModeling.settings_screen.model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private double carInterval = 5;
        private double carProbability = 0;
        private double cashBoxCapacity = 10_000;
        #endregion

        #region DbData
        public List<Fuel> FuelsDB { get; set; }

        public List<Tank> TanksDB { get; set; }

        public List<FuelDispenser> FuelDispensersDB { get; set; }
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

        public double CarInterval
        {
            get => carInterval;
            set
            {
                carInterval = value;

                RaisePropertyChanged(() => CarInterval);
                RaisePropertyChanged(() => CarIntervalFormated);
            }
        }

        public string CarIntervalFormated
        {
            get => ((int)carInterval).ToString();
        }

        public double CarProbability
        {
            get => carProbability;
            set
            {
                carProbability = value;

                RaisePropertyChanged(() => CarProbability);
                RaisePropertyChanged(() => CarProbabilityFormated);
            }
        }

        public string CarProbabilityFormated
        {
            get => String.Format("{0:0.##}", carProbability);
        }

        public double CashBoxCapacity
        {
            get => cashBoxCapacity;
            set
            {
                cashBoxCapacity = value;

                RaisePropertyChanged(() => CashBoxCapacity);
                RaisePropertyChanged(() => CashBoxCapacityFormated);
            }
        }

        public string CashBoxCapacityFormated
        {
            get => ((int)cashBoxCapacity).ToString();
        }
        #endregion

        public SettingsScreenViewModel()
        {
            var db = DbInitializer.getInstance();

            getFuelsFromDB(db);
            getFuelTanksFromDB(db);
            getFuelDispensersFromDB(db);
        }

        public List<Fuel> getChosenFuels()
        {
            var ids = _fuels
                .Where(fuelItem => fuelItem.IsChecked)
                .Select(fuelItem => fuelItem.Id)
                .ToList();

            if (ids.Count == 0) throw new ParameterNotSelectedException(ParameterErrorMessage.FUELS_NOT_SELECTED);

            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();

            var fuelTanksOnTopologyCount = mainViewModel.GetTopology.FuelTankCountChosen;

            if(ids.Count != fuelTanksOnTopologyCount) throw new ParameterNotSelectedException(ParameterErrorMessage.FUELS_COUNT_NOT_EQUALS_TANKS_COUNT);

            var elems = FuelsDB
                .Where(fuel => ids.Any(id => id == fuel.Id))
                .ToList();

            return elems;
        }

        public FuelDispenser getChosenFuelDispenser()
        {
            FuelDispenser dispenser = FuelDispensersDB.Where(disp => disp.Name.Equals(SelectedFuelDispenser)).First();
            if (dispenser == null) throw new ParameterNotSelectedException(ParameterErrorMessage.DISPENSER_NOT_SELECTED);
            return dispenser;
        }

        public Tank getChosenTank()
        {
            Tank tank = TanksDB.Where(fuel => fuel.Name.Equals(SelectedFuelTank)).First();
            if(tank == null) throw new ParameterNotSelectedException(ParameterErrorMessage.TANK_NOT_SELECTED);
            return tank;
        }

        #region GetFromDBMethods
        private void getFuelsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Fuel>(db, DBWorkerKeys.FUEL_TYPES_KEY);
            FuelsDB = dbWorker.getCollection();

            Fuels = new FuelToFuelComboBoxMapper().MapList(FuelsDB);
            SelectedFuel = Fuels[0].Name;
        }

        private void getFuelTanksFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Tank>(db, DBWorkerKeys.FUEL_TANKS_KEY);
            TanksDB = dbWorker.getCollection();

            FuelTanks = new TankToFuelTankComboBoxItemMapper().MapList(TanksDB);
            SelectedFuelTank = FuelTanks[0].Name;
        }

        private void getFuelDispensersFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<FuelDispenser>(db, DBWorkerKeys.FUEL_DISPENSERS_KEY);
            FuelDispensersDB = dbWorker.getCollection();

            FuelDispensers = new FuelDispenserToFuelDispenserComboBoxItemMapper().MapList(FuelDispensersDB);
            SelectedFuelDispenser = FuelDispensers[0].Name;
        }
        #endregion

    }
}
