using GalaSoft.MvvmLight;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GasStationModeling.ViewModel
{
    public class SettingsScreenViewModel : ViewModelBase
    {
        private string _selectedFuel;
        private List<FuelComboBoxItem> _fuels = new List<FuelComboBoxItem>();

        private string _selectedFuelTank;
        private List<FuelTankComboBoxItem> _fuelTanks = new List<FuelTankComboBoxItem>();

        private string _selectedFuelDispenser;
        private List<FuelDispenserComboBoxItem> _fuelDispensers = new List<FuelDispenserComboBoxItem>();

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

        public SettingsScreenViewModel()
        {
            var db = DbInitializer.getInstance();

            getFuelsFromDB(db);
            getFuelTanksFromDB(db);
            getFuelDispensersFromDB(db);
        }

        private void getFuelsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Fuel>(db, DBWorkerKeys.FUEL_TYPES_KEY);
            var collectionFromDb = dbWorker.getCollection();
            var collectionToComboBox = new List<FuelComboBoxItem>();

            foreach (var fuel in collectionFromDb)
            {
                collectionToComboBox.Add(
                    new FuelComboBoxItem(fuel.Id, fuel.Name)
                    );
            }

            Fuels = collectionToComboBox;
            SelectedFuel = Fuels[0].Name;
        }

        private void getFuelTanksFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Tank>(db, DBWorkerKeys.FUEL_TANKS_KEY);
            var collectionFromDb = dbWorker.getCollection();
            var collectionToComboBox = new List<FuelTankComboBoxItem>();

            foreach (var fuel in collectionFromDb)
            {
                collectionToComboBox.Add(
                    new FuelTankComboBoxItem(fuel.Id, fuel.Name)
                    );
            }

            FuelTanks = collectionToComboBox;
            SelectedFuelTank = FuelTanks[0].Name;
        }

        private void getFuelDispensersFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<FuelDispenser>(db, DBWorkerKeys.FUEL_DISPENSERS_KEY);
            var collectionFromDb = dbWorker.getCollection();
            var collectionToComboBox = new List<FuelDispenserComboBoxItem>();

            foreach (var fuel in collectionFromDb)
            {
                collectionToComboBox.Add(
                    new FuelDispenserComboBoxItem(fuel.Id, fuel.Name)
                    );
            }

            FuelDispensers = collectionToComboBox;
            SelectedFuelDispenser = FuelDispensers[0].Name;
        }

    }


    [Serializable]
    public class FuelComboBoxItem
    {
        [XmlAttribute("Name")]
        public string Name { get; }

        public ObjectId Id { get; }

        public FuelComboBoxItem(ObjectId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    [Serializable]
    public class FuelTankComboBoxItem
    {
        [XmlAttribute("Name")]
        public string Name { get; }

        public ObjectId Id { get; }

        public FuelTankComboBoxItem(ObjectId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

    [Serializable]
    public class FuelDispenserComboBoxItem
    {
        [XmlAttribute("Name")]
        public string Name { get; }

        public ObjectId Id { get; }

        public FuelDispenserComboBoxItem(ObjectId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
