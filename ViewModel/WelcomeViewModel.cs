using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.add_forms;
using GasStationModeling.core.DB;
using GasStationModeling.core.DB.dto;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using GasStationModeling.exceptions;
using GasStationModeling.main_window.view;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace GasStationModeling.ViewModel
{
    public class WelcomeViewModel : ViewModelBase
    {
        #region Fields
        private string _selectedTopology = "Загрузить";
        private List<TopologyDTO> _topologies;
        #endregion

        #region Properties
        public string SelectedTopology
        {
            get => _selectedTopology;
            set
            {
                if(value != null)
                {
                    LoadTopology(value);
                    _selectedTopology = "Загрузить";
                    RaisePropertyChanged(() => SelectedTopology);
                }
            }
        }

        public List<TopologyDTO> Topologies
        {
            get => _topologies;
            set
            {
                _topologies = value;
                RaisePropertyChanged(() => Topologies);
            }
        }
        #endregion Properties

        public WelcomeViewModel()
        {
            try
            {
                var db = DbInitializer.getInstance();
                Topologies = getTopologiesFromDB(db);
            }
            catch (DbErrorException e)
            {
                ErrorMessageBoxShower.ShowError(DbErrorMessage.CONNECTION_ERROR);
            }
        }

        public TopologyDTO getChosenTopology(string value)
        {
            return Topologies
               .Where(topology => topology.Name.Equals(value))
               .First();
        }

        #region GetFromDBMethods
        private List<TopologyDTO> getTopologiesFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<TopologyDTO>(db, DBWorkerKeys.TOPOLOGIES_KEY);
            return dbWorker.getCollection();
        }
        #endregion

        public void LoadTopology(string value)
        {
            ViewModelUtils.ReloadMainViewModels();

            var mainWindow = new MainWindow();
            var viewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            var topology = getChosenTopology(value);
            viewModel.GetTopology.LoadTopology(topology.Topology, topology.ServiceAreaWidth);

            mainWindow.Show();
        }
    }
}
