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
    public class FuelComboBoxItem
    {
        private ICommand _deleteCommand;

        [XmlAttribute("Name")]
        public string Name { get; }

        public ObjectId Id { get; }

        public Boolean IsChecked { get; set; }

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

        public FuelComboBoxItem(ObjectId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        private void DeleteItem()
        {
            if (DeleteConfirmationMessageBoxShower.show(Name).Equals(MessageBoxResult.Yes))
            {
                var db = DbInitializer.getInstance();
                var dbWorker = new DbWorker<Fuel>(db, DBWorkerKeys.FUEL_TYPES_KEY);
                var newCollection = dbWorker.deleteEntry(Id);

                var viewModel = ServiceLocator.Current.GetInstance<SettingsScreenViewModel>();
                viewModel.Fuels = new FuelToFuelComboBoxMapper().MapList(newCollection);
            }
        }
    }
}
