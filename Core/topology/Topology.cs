using GasStationModeling.core.models;
using System;
using System.Drawing;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommonServiceLocator;
using GasStationModeling.ViewModel;

namespace GasStationModeling.core.topology
{
    public partial class Topology
    {
        private Grid grid;
        private IGasStationElement[,] topologyElements;

        private int topologyColumnCountMain = 5;
        private int topologyColumnCountWorker = 3;
        private int topologyRowCount = 6;

        public IGasStationElement[,] TopologyElements
        {
            get { return topologyElements; }
        }
        public Grid TopologyGrid
        {
            get
            {
                if (grid == null)
                    return GetEmptyTopologyGrid(topologyRowCount, topologyColumnCountMain  + topologyColumnCountWorker);

                return grid;
            }
            private set
            {
                grid = value;

                var viewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
                if (viewModel != null)
                {
                    viewModel.RaisePropertyChanged("GetTopology");
                }
            }
        }

        public int  TopologyColumnCountMain
        {
            get { return topologyColumnCountMain; } 
            set
            {
                topologyColumnCountMain = value;
                TopologyGrid = GetEmptyTopologyGrid(TopologyRowCount, value + TopologyColumnCountWorker);
            }
        }

        public int TopologyColumnCountWorker
        {
            get { return topologyColumnCountWorker; }
            set
            {
                topologyColumnCountWorker = value;
                TopologyGrid = GetEmptyTopologyGrid(TopologyRowCount, value + TopologyColumnCountMain);
            }
        }

        public int TopologyRowCount
        {
            get { return topologyRowCount; }
            set
            {
                topologyRowCount = value;
                TopologyGrid = GetEmptyTopologyGrid(value, TopologyColumnCountWorker + TopologyColumnCountMain);
            }
        }

        private Grid GetEmptyTopologyGrid(int rowCount, int columnCount)
        {
            var topology = new Grid();
            topology.Height = TOPOLOGY_CELL_SIZE * rowCount;
            topology.Width = TOPOLOGY_CELL_SIZE * columnCount;

            for (int i = 0; i < rowCount; i++)
            {
                topology.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(TOPOLOGY_CELL_SIZE)
                });
            }
            for (int i = 0; i < columnCount; i++)
            {
                topology.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(TOPOLOGY_CELL_SIZE)
                });
            }

            FillEmptyTopology(topology);

            return topology;
        }

        private void FillEmptyTopology(Grid grid)
        {
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                {
                    var item = new Label()
                    {
                        Background = new ImageBrush(Application.Current.TryFindResource("Cell") as BitmapImage)
                    };

                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);
                    grid.Children.Add(item);
                }
            }
        }

        enum ChangedGridSize
        {
            ColumnCount,
            RowCountMain,
            RowCountWorker
        }
    }
}
