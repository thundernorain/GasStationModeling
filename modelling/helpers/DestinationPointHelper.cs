using GasStationModeling.core.topology;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.mapper;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling
{
    public class DestinationPointHelper
    {
        #region IntPos
        public int FuelingPointDeltaX { get; } = 5;
        public int FuelingPointDeltaY { get; } = 5;
        public int CarRoadPositionY { get; private set; }
        public int ReSpawnPointX { get; } = -50;
        #endregion

        #region CommonPoints
        public Point SpawnPoint { get; private set; }      
        public Point LeavePointNoFilling { get; private set; }
        public Point LeavePointFilled { get; private set; }
        #endregion

        #region TopologyElemsPoints
        public Point EntrancePoint { get; private set; }
        public Point ExitPoint { get; private set; }

        public Point ServiceEntrancePoint { get; set; }

        public Point CashBoxPoint { get; private set; }
        public Dictionary<Rectangle, Point> FuelDispensersDestPoints { get; private set; }
        public Dictionary<Rectangle, Point> RefuellerDestPoints { get; private set; }

        public Point ServiceAreaEntrancePoint { get; private set; }
        public Point ServiceAreaExitPoint { get; private set; }
        #endregion

        public DestinationPointHelper(CanvasParser parsedCanvas)
        {
            defineCommonPoints(parsedCanvas);
            defineElementsPoints(parsedCanvas);
        }

        public void defineCommonPoints(CanvasParser parsedCanvas)
        {
            CarRoadPositionY = (int)parsedCanvas.StationCanvas.Height - 2 * ElementSizeHelper.CELL_HEIGHT; 
            SpawnPoint = new Point((int)parsedCanvas.StationCanvas.Width - 50, CarRoadPositionY);
            LeavePointNoFilling = new Point(ReSpawnPointX, CarRoadPositionY);
            LeavePointFilled = new Point(ReSpawnPointX, CarRoadPositionY - (ElementSizeHelper.CELL_HEIGHT + 1));
        }

        public void defineElementsPoints(CanvasParser parsedCanvas)
        {
            FuelDispensersDestPoints = defineDestPointsToDispensers(parsedCanvas);
            CashBoxPoint = defineDestPointFor(TopologyElement.CashBox, parsedCanvas);
            RefuellerDestPoints = defineDestPointsToFuelTanks(parsedCanvas);

            EntrancePoint = defineDestPointFor(TopologyElement.Entrance, parsedCanvas);
            ExitPoint = defineDestPointFor(TopologyElement.Exit, parsedCanvas);
        }

        public Point defineDestPointFor(TopologyElement elementType, CanvasParser parsedCanvas)
        {
            Rectangle rect = null;
            switch (elementType)
            {
                case TopologyElement.CashBox: rect = parsedCanvas.CashBox; break;
                case TopologyElement.Entrance: rect = parsedCanvas.Entrance; break;
                case TopologyElement.Exit: rect = parsedCanvas.Exit; break;
                default: break;
            }
            Point destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) + ElementSizeHelper.CELL_HEIGHT);
            return destPoint;
        }

        public void defineServiceAreaEntrancePoint(Rectangle elem)
        {
            ServiceAreaEntrancePoint = new Point((int)Canvas.GetLeft(elem) + ElementSizeHelper.CELL_WIDTH, SpawnPoint.Y);
        }

        public Dictionary<Rectangle, Point> defineDestPointsToDispensers(CanvasParser parsedCanvas)
        {
            Dictionary<Rectangle, Point> destPointsDict = new Dictionary<Rectangle, Point>();
            var elems = parsedCanvas.Dispensers;
            foreach (var rect in elems)
            {
                Point destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) + ElementSizeHelper.CELL_HEIGHT);
                destPointsDict.Add(rect, destPoint);
            }       
            return destPointsDict;
        }

        public Dictionary<Rectangle, Point> defineDestPointsToFuelTanks(CanvasParser parsedCanvas)
        {
            Dictionary<Rectangle, Point> destPointsDict = new Dictionary<Rectangle, Point>();
            var elems = parsedCanvas.Tanks;
            defineServiceAreaEntrancePoint(elems[0]);
            foreach (var rect in elems)
            {
                Point destPoint = new Point((int)Canvas.GetLeft(rect) + ElementSizeHelper.CELL_WIDTH, (int)Canvas.GetTop(rect));
                destPointsDict.Add(rect, destPoint);
            }
            return destPointsDict;
        }

        public Rect createDestinationSpot(Point destpoint)
        {
            return new Rect(
                destpoint.X,
                destpoint.Y,
                5,
                5);
        }
    }
}