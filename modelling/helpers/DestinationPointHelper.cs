﻿using GasStationModeling.core.topology;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
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
        public int ServiceRoadPositionY { get; private set; }
        public int ReSpawnPointX { get; } = -40;
        #endregion

        #region CommonPoints
        public Point SpawnPoint { get; private set; }
        public Point ServiceSpawnPoint { get; private set; }
        public Point LeavePointNoFilling { get; private set; }
        public Point LeavePointFilled { get; private set; }
        #endregion

        #region TopologyElemsPoints
        public Point EntrancePoint { get; private set; }
        public Point ExitPoint { get; private set; }

        public Point CashBoxPoint { get; private set; }
        public Dictionary<int, Point> FuelDispensersDestPoints { get; private set; }
        public Dictionary<int, Point> RefuellerDestPoints { get; private set; }

        public Point ServiceAreaEntrancePoint { get; private set; }
        public Point ServiceMiddlePoint { get; private set; }
        public Point ServiceAreaExitPoint { get; private set; }
        public Point ServiceMiddleExitPoint { get; private set; }
        public Point ServiceAreaWaitingPoint { get; private set; }
        #endregion

        public DestinationPointHelper(CanvasParser parsedCanvas)
        {
            defineCommonPoints(parsedCanvas);
            defineElementsPoints(parsedCanvas);
        }

        #region PointsDefiner
        public void defineCommonPoints(CanvasParser parsedCanvas)
        {
            CarRoadPositionY = (int)parsedCanvas.StationCanvas.Height - 2 * ElementSizeHelper.CELL_HEIGHT;
            ServiceRoadPositionY = CarRoadPositionY -  ElementSizeHelper.CELL_HEIGHT;
            SpawnPoint = new Point((int)parsedCanvas.StationCanvas.Width, CarRoadPositionY);
            ServiceSpawnPoint = new Point((int)parsedCanvas.StationCanvas.Width, ServiceRoadPositionY);
            LeavePointNoFilling = new Point(ReSpawnPointX, CarRoadPositionY + ElementSizeHelper.CELL_HEIGHT);
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
            if (elementType.Equals(TopologyElement.Entrance)) destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect));
            if (elementType.Equals(TopologyElement.Exit)) destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) + 2 * ElementSizeHelper.CELL_HEIGHT);
            if (elementType.Equals(TopologyElement.CashBox)) destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) +  ElementSizeHelper.CELL_HEIGHT + 5);
            return destPoint;
        }

        public void defineServiceAreaEntrancePoint(Rectangle elem)
        {
            ServiceAreaEntrancePoint = new Point((int)Canvas.GetLeft(elem) + 2 * ElementSizeHelper.CELL_WIDTH, SpawnPoint.Y);
            ServiceMiddlePoint = new Point(ServiceAreaEntrancePoint.X, Canvas.GetTop(elem) + 2 * ElementSizeHelper.CELL_HEIGHT);
            ServiceMiddleExitPoint = new Point(ServiceAreaEntrancePoint.X,ServiceAreaEntrancePoint.Y + ElementSizeHelper.CELL_HEIGHT + 15);
            ServiceAreaWaitingPoint = new Point(ServiceSpawnPoint.X, ServiceRoadPositionY);
        }

        public Dictionary<int, Point> defineDestPointsToDispensers(CanvasParser parsedCanvas)
        {
            Dictionary<int, Point> destPointsDict = new Dictionary<int, Point>();
            var elems = parsedCanvas.Dispensers;
            foreach (var rect in elems)
            {
                var tag = rect.Tag as DispenserView;
                Point destPoint = new Point((int)Canvas.GetLeft(rect), (int)Canvas.GetTop(rect) + ElementSizeHelper.CELL_HEIGHT);
                destPointsDict.Add(tag.Id, destPoint);
            }       
            return destPointsDict;
        }

        public Dictionary<int, Point> defineDestPointsToFuelTanks(CanvasParser parsedCanvas)
        {
            Dictionary<int, Point> destPointsDict = new Dictionary<int, Point>();
            var elems = parsedCanvas.Tanks;
            defineServiceAreaEntrancePoint(elems[0]);
            foreach (var rect in elems)
            {
                var tag = rect.Tag as TankView;
                Point destPoint = new Point((int)Canvas.GetLeft(rect) + ElementSizeHelper.CELL_WIDTH, (int)Canvas.GetTop(rect));
                destPointsDict.Add(tag.Id, destPoint);
            }
            return destPointsDict;
        }
        #endregion

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