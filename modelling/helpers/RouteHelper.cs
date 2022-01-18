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

        public DestinationPointHelper DpHelper { get; set; }
        private CanvasParser canvasElems; 

        public RouteHelper(CanvasParser parsedCanvas, ModellingSettings settings,DestinationPointHelper destPointHelper)
        {
            DpHelper = new DestinationPointHelper(parsedCanvas);
            canvasElems = parsedCanvas;
        }

        public MoveableElem RouteVehicle(MoveableElem vehicle)
        {
            if (!vehicle.IsGoingToFill)
            {
                return vehicle;
            }

            CarView carView = null;
            CollectorView collectorView = null;

            if (vehicle is CarElem)
            {
                carView = vehicle.Tag as CarView;
            }

            if (vehicle.Type.Equals("Collector"))
            {
                collectorView = vehicle.Tag as CollectorView;
            }

            var isOnStation = vehicle.IsOnStation;
            var isFilled = vehicle.IsFilled;

            // New car
            if (!isOnStation && !isFilled && !vehicle.HasDestPoints())
            {
                MoveVehicleToEnter(vehicle);
            }

            // Just entered the station
            if (vehicle is CarElem)
            {
                if (isOnStation && !carView.FuelDispenserChosen)
                {
                    vehicle = ChooseFuelDispenser((CarElem)vehicle);
                }
            }

            if (vehicle.Type.Equals("Collector"))
            {
                var collector = vehicle as CollectorElem;
                if (isOnStation && !collectorView.IsMovingToCashBox)
                {
                   MoveCollectorToCashCounter(collector);
                }
            }
            // After filling 
            if (isOnStation && isFilled)
            {
                MoveVehicleToExit(vehicle);
                vehicle.IsOnStation = false;
            }
            return vehicle;
        }

        public CarElem ChooseFuelDispenser(CarElem car)
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
            car.AddDestinationPoint(DpHelper.FuelDispensersDestPoints[fuelDispenserView.Id]);
            return car;
        }

        private CollectorElem MoveCollectorToCashCounter(CollectorElem collector)
        {
            var collectorView = collector.Tag as CollectorView;
            var cashCounter = canvasElems.CashBox;
            collectorView.IsMovingToCashBox = true;
            collector.AddDestinationPoint(DpHelper.CashBoxPoint);
            return collector;
        }

        private void MoveVehicleToEnter(MoveableElem vehicle)
        {
            var newPoint = new Point(
                DpHelper.EntrancePoint.X,
                DpHelper.EntrancePoint.Y + ElementSizeHelper.CELL_HEIGHT
                );
            vehicle.AddDestinationPoint(DpHelper.EntrancePoint);
            vehicle.AddDestinationPoint(newPoint);
        }

        private void MoveVehicleToExit(MoveableElem vehicle)
        {
            vehicle.removeDestinationPoints();
            vehicle.AddDestinationPoint(DpHelper.LeavePointFilled);
            vehicle.AddDestinationPoint(DpHelper.ExitPoint);
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
                GoToServiceEntrancePoint(ref refueller);
            }
            else
            {
                LeaveServiceArea(ref refueller);
            }
        }

        private void SetAnotherRefuellersWaitingCondition(ref RefuellerElem refueller,Boolean condition)
        {
            foreach (var refElem in canvasElems.RefuellersOnServiceArea)
            {
                if (refElem.Equals(refueller)) continue;

                else if (refueller.IsFilled)
                {
                    refElem.IsGoingToWait = condition;
                }
            }
        }

        private void GoToServiceEntrancePoint(ref RefuellerElem refueller)
        {
            refueller.AddDestinationPoint(DpHelper.ServiceMiddlePoint);
            refueller.AddDestinationPoint(DpHelper.ServiceAreaEntrancePoint);
        }

        private void GoToWaitingPoint(ref RefuellerElem refueller)
        {
            refueller.AddDestinationPoint(DpHelper.ServiceAreaWaitingPoint);
        }

        private void GoToFuelTank(ref RefuellerElem refueller)
        {
            var chosenTank = (refueller.Tag as RefuellerView).ChosenTank;
            var tankId = (chosenTank.Tag as TankView).Id;
            refueller.AddDestinationPoint(DpHelper.RefuellerDestPoints[tankId]);
        }

        private void LeaveServiceArea(ref RefuellerElem refueller)
        {
            refueller.AddDestinationPoint(DpHelper.LeavePointNoFilling);
            refueller.AddDestinationPoint(DpHelper.ServiceMiddleExitPoint);
            refueller.AddDestinationPoint(DpHelper.ServiceAreaEntrancePoint);
        }
    }
}
