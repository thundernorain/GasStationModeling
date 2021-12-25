using GasStationModeling.core.topology;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling
{
    public class DestinationPointHelper
    {
        #region IntPos
        public static int FuelingPointDeltaX { get; } = 5;
        public static int FuelingPointDeltaY { get; } = 5;
        public static int CarRoadPositionY { get; private set; }
        public static int ReSpawnPointX { get; } = -50;
        #endregion

        #region CommonPoints
        public static Point SpawnPoint { get; private set; }      
        public static Point LeavePointNoFilling { get; private set; }
        public static Point LeavePointFilled { get; private set; }
        #endregion

        #region TopologyElemsPoints
        public static Point EntrancePoint { get; private set; }
        public static Point ExitPoint { get; private set; }

        public static Point CashCounterPoint { get; private set; }
        public static Dictionary<Rectangle, Point> FuelDispensersDestPoints { get; private set; }
        public static Dictionary<Rectangle, Point> RefuellerDestPoints { get; private set; }

        public static Point ServiceAreaEntrancePoint { get; private set; }
        public static Point ServiceAreaExitPoint { get; private set; }
        #endregion

        public DestinationPointHelper(Canvas stationCanvas)
        {
            defineCommonPoints(stationCanvas);
            defineElementsPoints(stationCanvas);
        }

        public static void defineCommonPoints(Canvas stationCanvas)
        {
            CarRoadPositionY = (int)stationCanvas.Height - ElementSizeHelper.CELL_HEIGHT; 
            SpawnPoint = new Point((int)stationCanvas.Width + 50, CarRoadPositionY);
            LeavePointNoFilling = new Point(ReSpawnPointX, CarRoadPositionY);
            LeavePointFilled = new Point(ReSpawnPointX, CarRoadPositionY - (ElementSizeHelper.CELL_HEIGHT + 1));
        }

        public static void defineElementsPoints(Canvas stationCanvas)
        {
            FuelDispensersDestPoints = defineDestPointsToDispensers(stationCanvas);
            CashCounterPoint = defineDestPointFor(TopologyElement.CashBox,stationCanvas);
            RefuellerDestPoints = defineDestPointsToFuelTanks(stationCanvas);

            EntrancePoint = defineDestPointFor(TopologyElement.Entrance, stationCanvas);
            ExitPoint = defineDestPointFor(TopologyElement.Exit, stationCanvas);
        }

        public static Point defineDestPointFor(TopologyElement elementType, Canvas stationCanvas)
        {
            var rect = TopologyMapper.getTopologyElemsWithTypeOf(elementType, stationCanvas)[0];
            Point destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) + ElementSizeHelper.CELL_HEIGHT);
            return destPoint;
        }

        public static Dictionary<Rectangle, Point> defineDestPointsToDispensers(Canvas stationCanvas)
        {
            Dictionary<Rectangle, Point> destPointsDict = new Dictionary<Rectangle, Point>();
            var elems = TopologyMapper.getTopologyElemsWithTypeOf(TopologyElement.FuelDispenser, stationCanvas);
            foreach (var rect in elems)
            {
                Point destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) + ElementSizeHelper.CELL_HEIGHT);
                destPointsDict.Add(rect, destPoint);
            }       
            return destPointsDict;
        }

        public static Dictionary<Rectangle, Point> defineDestPointsToFuelTanks(Canvas stationCanvas)
        {
            Dictionary<Rectangle, Point> destPointsDict = new Dictionary<Rectangle, Point>();
            var elems = TopologyMapper.getTopologyElemsWithTypeOf(TopologyElement.Tank, stationCanvas);
            foreach (var rect in elems)
            {
                Point destPoint = new Point((int)Canvas.GetLeft(rect) + ElementSizeHelper.CELL_WIDTH, (int)Canvas.GetTop(rect));
                destPointsDict.Add(rect, destPoint);
            }
            return destPointsDict;
        }
    }
}