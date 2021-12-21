using CommonServiceLocator;
using GasStationModeling.core;
using GasStationModeling.core.DB;
using GasStationModeling.core.DB.dto;
using GasStationModeling.core.topology;
using GasStationModeling.DB;
using GasStationModeling.ViewModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GasStationModeling.add_forms
{
    /// <summary>
    /// Логика взаимодействия для AddTopologyWindow.xaml
    /// </summary>
    public partial class AddTopologyWindow : Window
    {
        private static IMongoDatabase DB = DbInitializer.getInstance();
        private static DbWorker<TopologyDTO> dbWorker = new DbWorker<TopologyDTO>(DB, DBWorkerKeys.TOPOLOGIES_KEY);

        private Topology topology;

        public AddTopologyWindow(Topology topology)
        {
            InitializeComponent();
            this.topology = topology;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = TopologyNameTB.Text;
            if (!Validator.isNameCorrect(name))
            {
                ErrorMessageBoxShower.show("Некорректное название ТБ");
                return;
            }

            TopologyDTO topologyDTO = new TopologyDTO
            {
                Name = name,
                Topology = topology.TopologyElements,
                ServiceAreaWidth = topology.TopologyRowCountWorker
            };

            var newCollection = dbWorker.insertEntry(topologyDTO);
            var welcomeViewModel = ServiceLocator.Current.GetInstance<WelcomeViewModel>();
            welcomeViewModel.Topologies = newCollection;

            Close();
        }
    }
}
