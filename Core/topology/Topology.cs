﻿using GasStationModeling.core.models;
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
using System.Windows.Input;

namespace GasStationModeling.core.topology
{
    public partial class Topology
    {
        private Grid grid;
        private TopologyElement[,] topologyElements;

        private int topologyColumnCountMain = 5;
        private int topologyColumnCountWorker = 3;
        private int topologyRowCount = 6;

        private TopologyElement selectedTopologyElement;

        public TopologyElement[,] TopologyElements
        {
            get
            {
                if (topologyElements == null)
                    topologyElements = GetEmptyGasStationElementsArray();

                return topologyElements;
            }
        }
        public Grid TopologyGrid
        {
            get
            {
                if (grid == null)
                    grid = GetTopologyGrid(TopologyRowCount, TopologyColumnCountMain + TopologyColumnCountWorker);

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

        public int TopologyColumnCountMain
        {
            get { return topologyColumnCountMain; }
            set
            {
                topologyColumnCountMain = value;
                TopologyGrid = GetTopologyGrid(TopologyRowCount, value + TopologyColumnCountWorker);
            }
        }

        public int TopologyColumnCountWorker
        {
            get { return topologyColumnCountWorker; }
            set
            {
                topologyColumnCountWorker = value;
                TopologyGrid = GetTopologyGrid(TopologyRowCount, value + TopologyColumnCountMain);
            }
        }

        public int TopologyRowCount
        {
            get { return topologyRowCount; }
            set
            {
                topologyRowCount = value;
                TopologyGrid = GetTopologyGrid(value, TopologyColumnCountWorker + TopologyColumnCountMain);
            }
        }
        public TopologyElement SelectedTopologyElement
        {
            get { return selectedTopologyElement; }
            set { selectedTopologyElement = value; }
        }

        private Grid GetTopologyGrid(int rowCount, int columnCount)
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

            topologyElements = CopyAndGetNewGasStationElements(
                TopologyElements.GetLength(0),
                TopologyElements.GetLength(1),
                TopologyRowCount,
                TopologyColumnCountMain + TopologyColumnCountWorker
                );
            FillTopologyGridWithImages(topology);

            return topology;
        }

        private void FillTopologyGridWithImages(Grid grid)
        {
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                {
                    var item = new Label()
                    {
                        Background = new ImageBrush(GetCellImage(i, j))
                    };

                    Grid.SetRow(item, i);
                    Grid.SetColumn(item, j);

                    item.MouseLeftButtonDown += GridElementMouseLeftButtonDown;

                    grid.Children.Add(item);
                }
            }
        }

        private BitmapImage GetCellImage(int i, int j)
        {
            object image = null;
            var element = TopologyElements[i, j];

            switch (element)
            {
                case TopologyElement.Nothing:
                    image = (j < TopologyColumnCountMain)
                        ? Application.Current.TryFindResource("Cell") as BitmapImage
                        : Application.Current.TryFindResource("CellBrown") as BitmapImage;
                    break;

                case TopologyElement.Tank:
                    image = Application.Current.TryFindResource("Tank") as BitmapImage;
                    break;

                case TopologyElement.CashBox:
                    image = Application.Current.TryFindResource("Cashbox") as BitmapImage;
                    break;

                case TopologyElement.FuelDispenser:
                    image = Application.Current.TryFindResource("FuelDispenser") as BitmapImage;
                    break;

                case TopologyElement.Entrance:
                    image = Application.Current.TryFindResource("Entrance") as BitmapImage;
                    break;

                case TopologyElement.Exit:
                    image = Application.Current.TryFindResource("Exit") as BitmapImage;
                    break;
            }

            return image as BitmapImage;
        }

        private TopologyElement[,] GetEmptyGasStationElementsArray()
        {
            return new TopologyElement[topologyRowCount, topologyColumnCountMain + topologyColumnCountWorker];
        }

        private TopologyElement[,] CopyAndGetNewGasStationElements(int oldRowCount, int oldColumnCount, int newRowCount, int newColumnCount)
        {
            var newGasStationElements = new TopologyElement[newRowCount, newColumnCount];

            var copyRowCount = (oldRowCount < newRowCount) ? oldRowCount : newRowCount;
            var copyColumnCount = (oldColumnCount < newColumnCount) ? oldColumnCount : newColumnCount;

            for (int i = 0; i < copyRowCount; i++)
            {
                for (int j = 0; j < copyColumnCount; j++)
                {
                    newGasStationElements[i, j] = TopologyElements[i, j];
                }
            }

            return newGasStationElements;
        }

        private void GridElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedTopologyElement != TopologyElement.Nothing)
            {
                Label label = sender as Label;

                int row = (int)label.GetValue(Grid.RowProperty);
                int column = (int)label.GetValue(Grid.ColumnProperty);

                TopologyElements[row, column] = SelectedTopologyElement;
                TopologyGrid = GetTopologyGrid(TopologyRowCount, TopologyColumnCountMain + TopologyColumnCountWorker);
                SelectedTopologyElement = TopologyElement.Nothing;
            }
        }

        enum ChangedGridSize
        {
            ColumnCount,
            RowCountMain,
            RowCountWorker
        }
    }

    public enum TopologyElement
    {
        Nothing,
        Tank,
        CashBox,
        FuelDispenser,
        Entrance,
        Exit
    }
}