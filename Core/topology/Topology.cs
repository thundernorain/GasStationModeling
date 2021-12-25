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
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Media;
using GasStationModeling.add_forms;

namespace GasStationModeling.core.topology
{
    public partial class Topology
    {
        #region Variables
        private Grid grid;
        private TopologyElement[,] topologyElements;

        private int topologyColumnCountMain = 5;
        private int topologyColumnCountWorker = 3;
        private int topologyRowCount = 6;

        private int availableCashBoxCount = CASHBOX_MAX_COUNT;
        private int availableTankCount = TANK_MAX_COUNT;
        private int availableFuelDispenserCount = FUEL_DISPENSER_MAX_COUNT;
        private int availableEntranceCount = ENTRANCE_MAX_COUNT;
        private int availableExitCount = EXIT_MAX_COUNT;

        private TopologyElement selectedTopologyElement;


        public int TopologyHeight
        {
            get
            {
                return topologyElements.Length;
            }
        }

        public int TopologyWidth
        {
            get
            {
                return topologyElements.GetLength(1);
            }
        }


        public int FuelTankCountChosen
        {
            get
            {
                return TANK_MAX_COUNT - AvailableTankCount;
            }
        }
        #endregion

        #region Fields
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
            set {
                selectedTopologyElement = (value >= 0)? value : 0;

                var viewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
                if (viewModel != null)
                {
                    viewModel.RaisePropertyChanged("GetTopology");
                }
            }
        }
        public Effect[] SelectedElementEffects
        {
            get
            {
                var effects = new Effect[5];
                int changeEffectIndex = -1;

                switch (SelectedTopologyElement)
                {
                    case TopologyElement.CashBox:
                        changeEffectIndex = 0;
                        break;

                    case TopologyElement.FuelDispenser:
                        changeEffectIndex = 1;
                        break;

                    case TopologyElement.Tank:
                        changeEffectIndex = 2;
                        break;

                    case TopologyElement.Entrance:
                        changeEffectIndex = 3;
                        break;

                    case TopologyElement.Exit:
                        changeEffectIndex = 4;
                        break;
                }

                if(changeEffectIndex > -1)
                    effects[changeEffectIndex] = new DropShadowEffect();

                return effects;
            }
        }
        public int AvailableCashBoxCount
        {
            get { return availableCashBoxCount; }
            set { availableCashBoxCount = (value >= 0) ? value : 0; }
        }
        public int AvailableTankCount
        {
            get { return availableTankCount; }
            set { availableTankCount = (value >= 0) ? value : 0; }
        }
        public int AvailableFuelDispenserCount
        {
            get { return availableFuelDispenserCount; }
            set { availableFuelDispenserCount = (value >= 0) ? value : 0; }
        }
        public int AvailableEntranceCount
        {
            get { return availableEntranceCount; }
            set { availableEntranceCount = (value >= 0) ? value : 0; }
        }
        public int AvailableExitCount
        {
            get { return availableExitCount; }
            set { availableExitCount = (value >= 0) ? value : 0; }
        }
        #endregion

        #region TopologyGridMethods
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
            UpdateTopologyElementsCountProperties();
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
                    item.MouseRightButtonDown += GridElementMouseRightButtonDown;

                    grid.Children.Add(item);
                }
            }
        }

        public BitmapImage GetCellImageModelling(int i, int j)
        {
            var element = TopologyElements[i, j];
            if (element == TopologyElement.Nothing)
            {
                return (j < TopologyColumnCountMain)
                        ? Application.Current.TryFindResource("Cell_Model") as BitmapImage
                        : Application.Current.TryFindResource("Cell_Service_Model") as BitmapImage;
            }
            else return GetCellImage(i, j);
        }

        public BitmapImage GetCellImage(int i, int j)
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

            return (image as BitmapImage);
        }

        private void GridElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            int row = (int)label.GetValue(Grid.RowProperty);
            int column = (int)label.GetValue(Grid.ColumnProperty);

            if (SelectedTopologyElement != TopologyElement.Nothing)
            {
                bool canAdd = isCanAddElementToGrid(row, column);

                if (canAdd)
                {
                    TopologyElements[row, column] = SelectedTopologyElement;
                    TopologyGrid = GetTopologyGrid(TopologyRowCount, TopologyColumnCountMain + TopologyColumnCountWorker);
                    SelectedTopologyElement = TopologyElement.Nothing;
                }
            }
            else
            {
                SelectedTopologyElement = TopologyElements[row, column];
                TopologyElements[row, column] = TopologyElement.Nothing;
                UpdateTopologyElementsCountProperties();
            }
        }

        #region CheckAddingToTopologyMethods
        private bool isCanAddElementToGrid(int row, int column)
        {
            switch (SelectedTopologyElement)
            {
                case TopologyElement.Tank:
                    if (AvailableTankCount == 0)
                    {
                        ErrorMessageBoxShower.ShowTopology("Количество элементов выбранного шаблона на топологии максимально");
                        return false;
                    }
                    if (column < TopologyColumnCountMain)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться на общей части территории");
                        return false;
                    }
                    if (row == TopologyRowCount - 1)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться в нижней части сетки");
                        return false;
                    }
                    break;

                case TopologyElement.CashBox:
                    if (AvailableCashBoxCount == 0)
                    {
                        ErrorMessageBoxShower.ShowTopology("Количество элементов выбранного шаблона на топологии максимально");
                        return false;
                    }
                    if (column >= TopologyColumnCountMain)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться на служебной части территории");
                        return false;
                    }
                    if (row == TopologyRowCount - 1)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться в нижней части сетки");
                        return false;
                    }
                    break;

                case TopologyElement.FuelDispenser:
                    if (AvailableFuelDispenserCount == 0)
                    {
                        ErrorMessageBoxShower.ShowTopology("Количество элементов выбранного шаблона на топологии максимально");
                        return false;
                    }
                    if (column >= TopologyColumnCountMain)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться на служебной части территории");
                        return false;
                    }
                    if (row == TopologyRowCount - 1)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться в нижней части сетки");
                        return false;
                    }
                    break;

                case TopologyElement.Entrance:
                    if (AvailableEntranceCount == 0)
                    {
                        ErrorMessageBoxShower.ShowTopology("Количество элементов выбранного шаблона на топологии максимально");
                        return false;
                    }
                    if (column >= TopologyColumnCountMain)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться на служебной части территории");
                        return false;
                    }
                    if(row != TopologyRowCount - 1)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон может распологаться только в нижней части сетки");
                        return false;
                    }
                    break;

                case TopologyElement.Exit:
                    if (AvailableExitCount == 0)
                    {
                        ErrorMessageBoxShower.ShowTopology("Количество элементов выбранного шаблона на топологии максимально");
                        return false;
                    }
                    if (column >= TopologyColumnCountMain)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон не может распологаться на служебной части территории");
                        return false;
                    }
                    if (row != TopologyRowCount - 1)
                    {
                        ErrorMessageBoxShower.ShowTopology("Выбранный шаблон может распологаться только в нижней части сетки");
                        return false;
                    }
                    break;
            }

            return CheckTopologyCellRangeIsFree(row, column);
        }

        private bool CheckTopologyCellRangeIsFree(int row, int column)
        {
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    var checkingRow = row + i;
                    var checkingColumn = column + j;

                    if(checkingRow < 0 
                        || checkingRow >= TopologyRowCount 
                        || checkingColumn < 0 
                        || checkingColumn >= (TopologyColumnCountMain + TopologyColumnCountWorker))
                    {
                        continue;
                    }

                    if (TopologyElements[checkingRow, checkingColumn] != TopologyElement.Nothing)
                    {
                        ErrorMessageBoxShower.ShowTopology("Вокруг выбранной клетки в диапазоне 1 клетки уже существует объект топологии");
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        private void GridElementMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;

            int row = (int)label.GetValue(Grid.RowProperty);
            int column = (int)label.GetValue(Grid.ColumnProperty);

            TopologyElements[row, column] = TopologyElement.Nothing;
            TopologyGrid = GetTopologyGrid(TopologyRowCount, TopologyColumnCountMain + TopologyColumnCountWorker);
        }
        #endregion

        #region TopologyElementMethods
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

        private void UpdateTopologyElementsCountProperties()
        {
            int cashBoxCount = 0;
            int tankCount = 0;
            int fuelDispenserCount = 0;
            int entranceCount = 0;
            int exitCount = 0;

            for(int i = 0; i < TopologyElements.GetLength(0); i++)
            {
                for(int j = 0; j < TopologyElements.GetLength(1); j++)
                {
                    switch (TopologyElements[i, j])
                    {
                        case TopologyElement.CashBox:
                            cashBoxCount++;
                            break;

                        case TopologyElement.Tank:
                            tankCount++;
                            break;

                        case TopologyElement.FuelDispenser:
                            fuelDispenserCount++;
                            break;

                        case TopologyElement.Entrance:
                            entranceCount++;
                            break;

                        case TopologyElement.Exit:
                            exitCount++;
                            break;
                    }
                }
            }

            AvailableCashBoxCount = CASHBOX_MAX_COUNT - cashBoxCount;
            AvailableTankCount = TANK_MAX_COUNT - tankCount;
            AvailableFuelDispenserCount = FUEL_DISPENSER_MAX_COUNT - fuelDispenserCount;
            AvailableEntranceCount = ENTRANCE_MAX_COUNT - entranceCount;
            AvailableExitCount = EXIT_MAX_COUNT - exitCount;
        }
        #endregion

        private void PlaySound()
        {
            SystemSounds.Beep.Play();
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
        NothingWorker,
        Tank,
        CashBox,
        FuelDispenser,
        Entrance,
        Road,
        Exit
    }
}