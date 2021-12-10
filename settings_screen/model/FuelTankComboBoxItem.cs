using CommonServiceLocator;
using GasStationModeling.core;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using GasStationModeling.settings_screen.mapper;
using GasStationModeling.ViewModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace GasStationModeling.settings_screen.model
{
    [Serializable]
    public class FuelTankComboBoxItem
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

        public FuelTankComboBoxItem(ObjectId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        private void DeleteItem()
        {
            var db = DbInitializer.getInstance();
            var dbWorker = new DbWorker<Tank>(db, DBWorkerKeys.FUEL_TANKS_KEY);
            var newCollection = dbWorker.deleteEntry(Id);

            var viewModel = ServiceLocator.Current.GetInstance<SettingsScreenViewModel>();
            viewModel.FuelTanks = new TankToFuelTankComboBoxItemMapper().MapList(newCollection);
        }
    }
}
