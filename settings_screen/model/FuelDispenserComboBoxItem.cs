using CommonServiceLocator;
using GasStationModeling.core;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using GasStationModeling.settings_screen.mapper;
using GasStationModeling.ViewModel;
using MongoDB.Bson;
using System;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace GasStationModeling.settings_screen.model
{
    [Serializable]
    public class FuelDispenserComboBoxItem
    {
        private ICommand _deleteCommand;

        [XmlAttribute("Name")]
        public string Name { get; }

        public ObjectId Id { get; }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(param => this.DeleteItem());
                }
                return _deleteCommand;
            }
        }

        public FuelDispenserComboBoxItem(ObjectId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        private void DeleteItem()
        {
            if (DeleteConfirmationMessageBoxShower.show(Name).Equals(MessageBoxResult.Yes))
            {
                var db = DbInitializer.getInstance();
                var dbWorker = new DbWorker<FuelDispenser>(db, DBWorkerKeys.FUEL_DISPENSERS_KEY);
                var newCollection = dbWorker.deleteEntry(Id);

                var viewModel = ServiceLocator.Current.GetInstance<SettingsScreenViewModel>();
                viewModel.FuelDispensers = new FuelDispenserToFuelDispenserComboBoxItemMapper().MapList(newCollection);
            }
        }
    }
}
