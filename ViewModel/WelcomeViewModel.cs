using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.core.DB;
using GasStationModeling.core.DB.dto;
using GasStationModeling.core.models;
using GasStationModeling.DB;
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
        private string _selectedTopology;
        private List<TopologyDTO> _topologies;
        #endregion

        #region Properties
        public string SelectedTopology
        {
            get => _selectedTopology;
            set
            {
                _selectedTopology = value;
                LoadTopology();
                RaisePropertyChanged(() => SelectedTopology);
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
            var db = DbInitializer.getInstance();
            Topologies = getTopologiesFromDB(db);
            Topologies = new List<TopologyDTO>();
            TopologyDTO tdto = new TopologyDTO();
            tdto.Name = "Загрузить";
            Topologies.Add(tdto);
        }

        public TopologyDTO getChosenTopology()
        {
            return Topologies
               .Where(topology => topology.Name.Equals(_selectedTopology))
               .First();
        }

        #region GetFromDBMethods
        private List<TopologyDTO> getTopologiesFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<TopologyDTO>(db, DBWorkerKeys.TOPOLOGIES_KEY);
            return dbWorker.getCollection();
        }
        #endregion

        public void LoadTopology()
        {
            var mainWindow = new MainWindow();

            var viewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            var topology = getChosenTopology();
            viewModel.GetTopology.LoadTopology(topology.Topology, topology.ServiceAreaWidth);

            mainWindow.Show();
        }
    }
}
