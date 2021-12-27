using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using GasStationModeling.settings_screen.model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.helpers
{
    class RouteHelper
    {

        private Canvas stationCanvas;
        public DestinationPointHelper DpHelper { get; set; }
        private TrafficGenerator trafficGenerator;
        private CanvasParser canvasElems; 

        public RouteHelper(CanvasParser parsedCanvas, ModellingSettings settings,DestinationPointHelper destPointHelper)
        {
            stationCanvas = parsedCanvas.StationCanvas;
            DpHelper = new DestinationPointHelper(parsedCanvas);
            canvasElems = parsedCanvas;
            trafficGenerator = new TrafficGenerator(settings,destPointHelper);
        }

        public void RouteVehicle(ref MoveableElem vehicle)
        {
            if (!vehicle.IsGoingToFill)
            {
                return;
            }

            CarView carView = null;
            CollectorView collectorView = null;

            if (vehicle is CarElem)
            {
                carView = vehicle.Tag as CarView;
            }

            if (vehicle is CollectorElem)
            {
                collectorView = vehicle.Tag as CollectorView;
            }

            var isOnStation = vehicle.IsOnStation;
            var isFilled = vehicle.IsFilled;

            // New car
            if (!isOnStation && !isFilled && !vehicle.HasDestPoints())
            {
                MoveVehicleToEnter(ref vehicle);
            }

            // Just entered the station
            if (vehicle is CarElem carElem)
            {
                if (isOnStation && !carView.FuelDispenserChosen)
                {
                    ChooseFuelDispenser(ref carElem);
                }
            }

            if (vehicle is CollectorElem collector)
            {
                if (isOnStation && !collectorView.IsMovingToCashBox)
                {
                   MoveCollectorToCashCounter(ref collector);
                }
            }

            // After filling 
            if (isOnStation && isFilled)
            {
                MoveVehicleToExit(ref vehicle);
                vehicle.IsOnStation = false;
            }
        }

        public void ChooseFuelDispenser(ref CarElem car)
        {
            var carView = car.Tag as CarView;

            Rectangle optimalFuelDispenser = canvasElems.Dispensers[0];
            DispenserView fuelDispenserView = optimalFuelDispenser.Tag as DispenserView;
            var minQueue = fuelDispenserView.CarsInQueue;

            foreach (var fuelDispenser in canvasElems.Dispensers)
            {
                fuelDispenserView = fuelDispenser.Tag as DispenserView;
                if (fuelDispenserView.CarsInQueue < minQueue)
                {
                    minQueue = fuelDispenserView.CarsInQueue;
                    optimalFuelDispenser = fuelDispenser;
                }
            }

            carView.ChosenDispenser = optimalFuelDispenser;
            fuelDispenserView = optimalFuelDispenser.Tag as DispenserView;
            fuelDispenserView.CarsInQueue++;
            carView.FuelDispenserChosen = true;

            var destPointX = (int)Canvas.GetLeft(optimalFuelDispenser) - DpHelper.FuelingPointDeltaX;
            var destPointY = (int)Canvas.GetBottom(optimalFuelDispenser) + ElementSizeHelper.CELL_WIDTH - 10;
            car.AddDestinationPoint(new Point(destPointX, destPointY));

            destPointX = (int)Canvas.GetLeft(optimalFuelDispenser) + DpHelper.FuelingPointDeltaX;
            destPointY = (int)Canvas.GetBottom(optimalFuelDispenser) + DpHelper.FuelingPointDeltaY;
            car.AddDestinationPoint(new Point(destPointX, destPointY));

            destPointX = (int)Canvas.GetRight(optimalFuelDispenser);
            destPointY += ElementSizeHelper.CELL_HEIGHT + 5;
            car.AddDestinationPoint(new Point(destPointX, destPointY));
        }

        public int getBottom(UIElement element)
        {
            return (int)Canvas.GetTop(element) + ElementSizeHelper.CELL_HEIGHT;
        }

        public int getRight(UIElement element)
        {
            return (int)Canvas.GetLeft(element) + ElementSizeHelper.CELL_WIDTH;
        }

        private CollectorElem MoveCollectorToCashCounter(ref CollectorElem collector)
        {
            var collectorView = collector.Tag as CollectorView;
            var cashCounter = canvasElems.CashBox;

            collectorView.IsMovingToCashBox = true;

            var destPointX = (int)Canvas.GetLeft(cashCounter) - ElementSizeHelper.CELL_WIDTH + 5;
            var destPointY = (int)Canvas.GetBottom(cashCounter) + ElementSizeHelper.CELL_WIDTH - 10;
            collector.AddDestinationPoint(new Point(destPointX, destPointY));

            destPointX = (int)Canvas.GetLeft(cashCounter) + DpHelper.FuelingPointDeltaX;
            destPointY = (int)Canvas.GetBottom(cashCounter) + DpHelper.FuelingPointDeltaY;
            collector.AddDestinationPoint(new Point(destPointX, destPointY));

            destPointX = (int)Canvas.GetRight(cashCounter) - ElementSizeHelper.CELL_WIDTH/2;
            destPointY += ElementSizeHelper.CELL_HEIGHT + 5;
            collector.AddDestinationPoint(new Point(destPointX, destPointY));

            return collector;
        }

        private void MoveVehicleToEnter(ref MoveableElem vehicle)
        {
            vehicle.AddDestinationPoint(DpHelper.EntrancePoint);
        }

        private void MoveVehicleToExit(ref MoveableElem vehicle)
        {
            var fillingFinishedPoint = vehicle.GetDestinationPoint();
            vehicle.removeDestinationPoints();

            vehicle.AddDestinationPoint(DpHelper.ExitPoint);
            vehicle.AddDestinationPoint(DpHelper.LeavePointFilled);
  
            vehicle.AddDestinationPoint(fillingFinishedPoint);
        }

        public void RouteRefueller(ref RefuellerElem refueller)
        {
            var isFilled = refueller.IsFilled;

            if (refueller.IsFilling || refueller.HasDestPoints())
            {
                return;
            }

            if (!isFilled)
            {
                GoToFuelTank(ref refueller);
            }
            else
            {
                LeaveServiceArea(ref refueller);
            }
        }

        private void GoToFuelTank(ref RefuellerElem refueller)
        {
            refueller.AddDestinationPoint(DpHelper.ServiceAreaEntrancePoint);
        }

        public Point getPointOfRefilling(String fuelTypeLacked)
        {
            Point pointOfRefilling = new Point (-1,-1);
            foreach(KeyValuePair<Rectangle,Point> keyValue in DpHelper.RefuellerDestPoints)
            {
                var tankView = keyValue.Key.Tag as TankView;
                if(tankView.TypeFuel.Equals(fuelTypeLacked))
                {
                    pointOfRefilling = keyValue.Value;
                    break;
                }
            }
            return pointOfRefilling;
        }

        private void LeaveServiceArea(ref RefuellerElem refueller)
        {
            refueller.AddDestinationPoint(DpHelper.ServiceAreaEntrancePoint);
            refueller.AddDestinationPoint(DpHelper.LeavePointNoFilling);
        }
    }
}
