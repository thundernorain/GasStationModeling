using GasStationModeling.core.models;
using GasStationModeling.core.topology;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.managers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.models;
using GasStationModeling.settings_screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.mapper
{
    class TopologyMapper
    {
        public ModellingSettings Settings { get; set; }

        public Topology Topology { get; set; }

        public List<Fuel> Fuels { get; set; }

        private int chosenFuelId;

        public int ChosenFuelId
        {
            get
            {
                return chosenFuelId;
            }
            set
            {
                if (chosenFuelId >= Fuels.Count) chosenFuelId = 0;
                else chosenFuelId = value;
            }
        }

        private double tick;

        public TopologyMapper(ModellingSettings settings,Topology topology)
        {
            Topology = topology;
            Settings = settings;
            tick = ModellingTimeHelper.TIMER_TICK_MILLISECONDS;
            Fuels = new List<Fuel>(Settings.Fuels);
            ChosenFuelId = 0;
        }

        public CanvasParser mapTopology(Canvas stationCanvas)
        {
            CanvasParser parsedCanvas = new CanvasParser();
            int width = Topology.TopologyColumnCountMain
                + Topology.TopologyColumnCountWorker;

            int height = Topology.TopologyRowCount;

            int heightWithRoad = height + 3;
            stationCanvas.Width = width  * ElementSizeHelper.CELL_WIDTH;
            stationCanvas.Height = heightWithRoad * ElementSizeHelper.CELL_HEIGHT;
    
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ImageBrush brush = new ImageBrush();
                    var elemImage = Topology.GetCellImageModelling(i, j);
                    brush.ImageSource = elemImage;
                    Rectangle topologyElem = createTopologyElem(Topology.TopologyElements[i, j], brush,ref parsedCanvas);
                    Canvas.SetLeft(topologyElem, ElementSizeHelper.CELL_WIDTH * j);
                    Canvas.SetTop(topologyElem, ElementSizeHelper.CELL_HEIGHT * i);
                    stationCanvas.Children.Add(topologyElem);
                }
            }
            //Road
            for (int i = height; i < heightWithRoad; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ImageBrush brush = new ImageBrush();
                    var roadImage = Application.Current.TryFindResource("Road") as BitmapImage;
                    brush.ImageSource = roadImage;
                    Rectangle topologyElem = createTopologyElem(TopologyElement.Road, brush);
                    Canvas.SetLeft(topologyElem, ElementSizeHelper.CELL_WIDTH * j);
                    Canvas.SetTop(topologyElem, ElementSizeHelper.CELL_HEIGHT * i);
                    stationCanvas.Children.Add(topologyElem);
                }
            }
            parsedCanvas.StationCanvas = stationCanvas;
            return parsedCanvas;
        }

        public Rectangle createTopologyElem(TopologyElement elemType, ImageBrush brush,ref  CanvasParser parsedCanvas)
        {
            var topologyElem = createTopologyElem(elemType, brush);
            switch (elemType)
            {
                case TopologyElement.Entrance: parsedCanvas.Entrance = topologyElem; break;
                case TopologyElement.Exit: parsedCanvas.Exit = topologyElem; break;
                case TopologyElement.CashBox: parsedCanvas.CashBox = topologyElem; break;
                case TopologyElement.FuelDispenser: parsedCanvas.Dispensers.Add(topologyElem); break;
                case TopologyElement.Tank: parsedCanvas.Tanks.Add(topologyElem); break;
                default: break;
            }
            return topologyElem;
        }

        public Rectangle createTopologyElem(TopologyElement elemType, ImageBrush brush)
        {
            object tag = null;
            if(elemType == TopologyElement.Entrance ||
                elemType == TopologyElement.Exit ||
                elemType == TopologyElement.Nothing ||
                elemType == TopologyElement.Road)
            {
                tag = elemType;
            }
            else if (elemType == TopologyElement.CashBox)
            {
                tag = new CashBoxView(Settings.CashLimit);
            }
            else if (elemType == TopologyElement.FuelDispenser)
            {
                tag = new DispenserView(Settings.Dispenser,tick);
            }
            else if (elemType == TopologyElement.Tank)
            {
                Tank tank = Settings.FuelTank;
                tank.TypeFuel = Fuels[ChosenFuelId];
                ChosenFuelId++;
                tag = new TankView(Settings.FuelTank);
            }

            Rectangle topologyElem = new Rectangle()
            {
                Tag = tag,
                Height = ElementSizeHelper.CELL_HEIGHT,
                Width = ElementSizeHelper.CELL_WIDTH,
                Fill = brush
            };
            return topologyElem;
        }
    }
}
