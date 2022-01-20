using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using GasStationModeling.settings_screen.model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GasStationModeling.modelling.helpers
{
    class MoveHelper
    {

        private static int CarSpeedFilling = 5;
        private static int CarSpeedNoFilling = 5;
        private static int RefuellerSpeed = 4;

        private Canvas stationCanvas;
        public DestinationPointHelper DpHelper { get; set; }
        private TrafficGenerator trafficGenerator;
        private CanvasParser canvasElems;
        private ModellingSteps modellingSteps;
        private IntersectInspector inspector;

        public MoveHelper(CanvasParser parsedCanvas, ModellingSettings settings, DestinationPointHelper destPointHelper)
        {
            stationCanvas = parsedCanvas.StationCanvas;
            DpHelper = new DestinationPointHelper(parsedCanvas);
            canvasElems = parsedCanvas;
            trafficGenerator = new TrafficGenerator(settings, destPointHelper);
            modellingSteps = new ModellingSteps(canvasElems,trafficGenerator);
            inspector = new IntersectInspector(stationCanvas,DpHelper);
        }

        public Canvas MoveCarToDestination(MoveableElem vehicle, ref List<MoveableElem> toDelete, ref List<MoveableElem> toAdd)
        {
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

            if (vehicle.IsFilling)
            {
                if (vehicle is CarElem carElem)
                {
                   
                    modellingSteps.RefillCar(ref carElem,ref toAdd);
                    return stationCanvas;
                }

                if (vehicle.Type.Equals("Collector"))
                {
                    var collector = vehicle as CollectorElem;
                    var cashCounter = canvasElems.CashBox.Tag as CashBoxView;
                    modellingSteps.CollectCash(ref collector, ref cashCounter);

                    collector.removeDestinationPoints();
                    collector.AddDestinationPoint(DpHelper.ExitPoint);

                    return stationCanvas;
                }
            }

            var destPoint = vehicle.GetDestinationPoint();
            var carSpeed = vehicle.IsGoingToFill ? CarSpeedFilling : CarSpeedNoFilling;

            destPoint = MoveCar(vehicle, destPoint, carSpeed);

  
           var  destSpot = DpHelper.createDestinationSpot(destPoint);
           vehicle.DestinationSpot = destSpot;
            
           
            var vehicleRect = vehicle.createIntersectRect();

            if (vehicleRect.IntersectsWith(destSpot))
            {
                vehicle.removeDestinationPoint();

                vehicle.IsBypassingObject = false;

                if (destPoint.Equals(DpHelper.EntrancePoint))
                {
                    vehicle.IsOnStation = true;
                }

                if (vehicle.IsGoesHorizontal && destPoint.Equals(vehicle.FromLeftBypassingPoint))
                {
                    vehicle.IsGoesHorizontal = false;
                }

                if (vehicle is CarElem carElem)
                {
                    foreach (var fuelDispensersDestPoint in DpHelper.FuelDispensersDestPoints.Values)
                    {
                        if (destPoint.Equals(fuelDispensersDestPoint))
                        { 
                            modellingSteps.StartFill(ref carElem);      
                        }
                    }
                }

                if (vehicle.Type.Equals("Collector"))
                {
                    if(Math.Abs(Canvas.GetLeft(vehicle) - DpHelper.CashBoxPoint.X) <= 20) {
                        var collector = vehicle as CollectorElem;
                        collector.IsFilling = true;
                        collectorView.IsMovingToCashBox = true;

                        var cashBoxView = canvasElems.CashBox;
                        collectorView.CashBox = cashBoxView;

                        if (destPoint.Equals(DpHelper.ExitPoint))
                        {
                            collector.AddDestinationPoint(DpHelper.LeavePointNoFilling);
                            toDelete.Add(vehicle);
                        }

                        return stationCanvas;
                    }

                }

                if (destPoint.Equals(DpHelper.ExitPoint))
                {
                    vehicle.IsOnStation = false;

                    if (vehicle.Type.Equals("Collector"))
                    {
                        vehicle.removeDestinationPoints();
                        vehicle.AddDestinationPoint(DpHelper.LeavePointNoFilling);
                    }
                }

                if (destPoint.Equals(DpHelper.LeavePointNoFilling) || destPoint.Equals(DpHelper.LeavePointFilled))
                {
                    toDelete.Add(vehicle);
                }
            }
            return stationCanvas;
        }

       public Point MoveCar(MoveableElem car, Point destPoint, int carSpeed)
        {
            car.oldX = left(car);
            car.oldY = top(car);

            var isHorizontalMoving = false;
            var isVerticalMoving = false;

            double carTop;
            double carLeft;

            Directions currentDirection = Directions.Left;

            if (!car.IsBypassingObject)
            {
                if (!car.IsFilled && !car.IsGoesHorizontal)
                {
                    // Go Up
                    if (top(car) >= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop - carSpeed);
                        isVerticalMoving = true;
                        currentDirection = Directions.Up;
                        destPoint = inspector.PreventIntersection(ref car, currentDirection);
                    }

                    // Go Down
                    else if (bottom(car) <= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop + carSpeed);
                        isVerticalMoving = true;
                        currentDirection = Directions.Down;
                        destPoint = inspector.PreventIntersection(ref car, currentDirection);
                    }

                    // Go left
                    else if (left(car) >= destPoint.X && !isVerticalMoving)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft - carSpeed);
                        currentDirection = Directions.Left;
                        destPoint = inspector.PreventIntersection(ref car, currentDirection);
                    }

                    // Go Right
                    else if (right(car) <= destPoint.X && !isVerticalMoving)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft + carSpeed);
                        currentDirection = Directions.Right;
                    }
                }
                else
                {
                    // Go left
                    if (left(car) >= destPoint.X)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft - carSpeed);
                        isHorizontalMoving = true;
                        currentDirection = Directions.Left;
                        destPoint = inspector.PreventIntersection(ref car, currentDirection);
                    }
                    // Go Right
                    else if (right(car) <= destPoint.X)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft + carSpeed);
                        isHorizontalMoving = true;
                        currentDirection = Directions.Right;
                    }

                    // Go Up
                    else if (top(car) >= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop - carSpeed);
                        currentDirection = Directions.Up;
                        destPoint = inspector.PreventIntersection(ref car, currentDirection);
                    }

                    // Go Down
                    else if (bottom(car) <= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop + carSpeed);
                        currentDirection = Directions.Down;
                        destPoint = inspector.PreventIntersection(ref car, currentDirection);
                    }
                }
            }
            else
            {
                // Go left
                if (left(car) >= destPoint.X)
                {
                    carLeft = left(car);
                    Canvas.SetLeft(car, carLeft - carSpeed);
                    currentDirection = Directions.Left;
                    destPoint = inspector.PreventIntersection(ref car, currentDirection);
                }

                // Go Right
                else if (right(car) <= destPoint.X)
                {
                    carLeft = left(car);
                    Canvas.SetLeft(car, carLeft + carSpeed);
                    currentDirection = Directions.Right;
                }

                // Go Up
                else if (top(car) >= destPoint.Y)
                {
                    carTop = top(car);
                    Canvas.SetTop(car, carTop - carSpeed);
                    currentDirection = Directions.Up;
                    destPoint = inspector.PreventIntersection(ref car, currentDirection);
                }

                // Go Down
                else if (bottom(car) <= destPoint.Y)
                {
                    carTop = top(car);
                    Canvas.SetTop(car, carTop + carSpeed);
                    currentDirection = Directions.Down;
                    destPoint = inspector.PreventIntersection(ref car, currentDirection);
                }
            }

            car.Fill = BrushHelper.getBrushFor(car.Type + "_" + currentDirection.ToString());

            return destPoint;
        }

        public void MoveRefuellerToDestination(RefuellerElem refueller,ref List<MoveableElem>toDelete)
        {
            
            if (refueller.IsFilling)
            {
                modellingSteps.RefillFuelTank(refueller);
                return;
            }

            var refuellerView = refueller.Tag as RefuellerView;

            var destPoint = refueller.GetDestinationPoint();
            var destSpot = refueller.DestinationSpot;

            destPoint = MoveRefueller(refueller, destPoint, RefuellerSpeed);
 
            destSpot = DpHelper.createDestinationSpot(destPoint);     
            
            var refuellerRect = refueller.createIntersectRect();

            if (refuellerRect.IntersectsWith(destSpot))
            {
                refueller.removeDestinationPoint();

                var fuelTank = ((RefuellerView)refueller.Tag).ChosenTank;
                var tankId = (fuelTank.Tag as TankView).Id;
                var pointOfFilling = DpHelper.RefuellerDestPoints[tankId];

                if (destPoint.Equals(pointOfFilling))
                {
                    modellingSteps.StartRefilling(refueller);
                }

                else if (destPoint.Equals(DpHelper.LeavePointNoFilling))
                {
                    toDelete.Add(refueller);
                }

                else if (destPoint.Equals(DpHelper.ServiceAreaWaitingPoint))
                {
                    refueller.IsWaiting = true;
                }

                else if (destPoint.Equals(DpHelper.ServiceAreaEntrancePoint)){
                    if (refueller.IsFilled)
                    {
                        canvasElems.RefuellersOnServiceArea.Remove(refueller);
                        refueller.IsOnStation = false;
                    }
                    else refueller.IsOnStation = true;
                }
            }
        }

        public Point MoveRefueller(RefuellerElem refueller, Point destPoint, int refuellerSpeed)
        {
            double refuellerTop;
            double refuellerLeft;
            if (refueller.IsWaiting) { return destPoint; }

            Directions currentDirection = Directions.Up;

            if (left(refueller) >= destPoint.X)
            {
                refuellerLeft = left(refueller);
                Canvas.SetLeft(refueller, refuellerLeft - refuellerSpeed);
                currentDirection = Directions.Left;
            }
            else if (right(refueller) <= destPoint.X)
            {
                refuellerLeft = left(refueller);
                Canvas.SetLeft(refueller, refuellerLeft + refuellerSpeed);
                currentDirection = Directions.Right;
            }
            else if (top(refueller) >= destPoint.Y)
            {
                refuellerTop = top(refueller);
                Canvas.SetTop(refueller, refuellerTop - refuellerSpeed);
                currentDirection = Directions.Up;
            }
            else if (bottom(refueller) <= destPoint.Y)
            {
                refuellerTop = top(refueller);
                Canvas.SetTop(refueller, refuellerTop + refuellerSpeed);
                currentDirection = Directions.Down;
            }

            refueller.Fill = BrushHelper.getBrushFor("Refueller_" + currentDirection.ToString());

            return destPoint;
        }
  
        #region CoordinateGetters
        private double left(UIElement element)
        {
            return Canvas.GetLeft(element);
        }

        private double top(UIElement element)
        {
            return Canvas.GetTop(element);
        }


        private double right(UIElement element)
        {
            return Canvas.GetLeft(element) + ElementSizeHelper.CELL_WIDTH;
        }

        private double bottom(UIElement element)
        {
            return Canvas.GetTop(element) + ElementSizeHelper.CELL_HEIGHT;
        }
        #endregion
    }
}
