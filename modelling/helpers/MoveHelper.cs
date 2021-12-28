using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using GasStationModeling.settings_screen.model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GasStationModeling.modelling.helpers
{
    class MoveHelper
    {

        private static int CarSpeedFilling = 3;
        private static int CarSpeedNoFilling = 4;
        private static int RefuellerSpeed = 2;

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
            inspector = new IntersectInspector(stationCanvas);
        }

        public Canvas MoveCarToDestination(ref MoveableElem vehicle)
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
                   
                    modellingSteps.StartFilling(ref carElem);
                    return stationCanvas;
                }

                if (vehicle is CollectorElem collector)
                {
                    var cashCounter = collectorView.CashBox.Tag as CashBoxView;
                    modellingSteps.CollectCash(ref collector, ref cashCounter);
                    return stationCanvas;
                }
            }

            var destPoint = vehicle.GetDestinationPoint();
            var carSpeed = vehicle.IsGoingToFill ? CarSpeedFilling : CarSpeedNoFilling;

            var destSpot = vehicle.DestinationSpot;

            destPoint = MoveCar(ref vehicle, destPoint, carSpeed);

            if(vehicle.SpotIsNull)
            {
                destSpot = DpHelper.createDestinationSpot(destPoint);
                vehicle.DestinationSpot = destSpot;
            }
           
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
                            modellingSteps.StartFilling(ref carElem);      
                        }
                    }
                }

                if (vehicle is CollectorElem collector)
                {
                    if (destPoint.Equals(DpHelper.CashBoxPoint))
                    {
                        var cashBoxView = collectorView.CashBox.Tag as CashBoxView;
                        modellingSteps.CollectCash(ref collector,ref cashBoxView);
                        return stationCanvas;
                    }
                }

                if (destPoint.Equals(DpHelper.ExitPoint))
                {
                    vehicle.IsOnStation = false;
                }

                if (destPoint.Equals(DpHelper.LeavePointNoFilling) || destPoint.Equals(DpHelper.LeavePointFilled))
                {
                    stationCanvas.Children.Remove(vehicle);
                    GC.Collect();
                }
            }
            return stationCanvas;
        }

       public Point MoveCar(ref MoveableElem car, Point destPoint, int carSpeed)
        {
            var isHorizontalMoving = false;
            var isVerticalMoving = false;

            double carTop;
            double carLeft;

            if (!car.IsBypassingObject)
            {
                if (!car.IsFilled && !car.IsGoesHorizontal)
                {
                    // Go Up
                    if (Canvas.GetTop(car) >= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop - carSpeed);
                        isVerticalMoving = true;
                        destPoint = inspector.PreventIntersection(ref car, Directions.Up);
                    }

                    // Go Down
                    if (Canvas.GetBottom(car) <= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop + carSpeed);
                        isVerticalMoving = true;
                        destPoint = inspector.PreventIntersection(ref car, Directions.Down);
                    }

                    // Go left
                    if (Canvas.GetLeft(car) >= destPoint.X && !isVerticalMoving)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft - carSpeed);
                        destPoint = inspector.PreventIntersection(ref car, Directions.Left);
                    }

                    // Go Right
                    if (Canvas.GetRight(car) <= destPoint.X && !isVerticalMoving)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft + carSpeed);
                    }
                }
                else
                {
                    // Go left
                    if (Canvas.GetLeft(car) >= destPoint.X)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft - carSpeed);
                        isHorizontalMoving = true;
                        destPoint = inspector.PreventIntersection(ref car, Directions.Left);
                    }

                    // Go Right
                    if (Canvas.GetRight(car) <= destPoint.X)
                    {
                        carLeft = left(car);
                        Canvas.SetLeft(car, carLeft + carSpeed);
                        isHorizontalMoving = true;
                    }

                    // Go Up
                    if (Canvas.GetTop(car) >= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop - carSpeed);
                        destPoint = inspector.PreventIntersection(ref car, Directions.Up);
                    }

                    // Go Down
                    if (Canvas.GetBottom(car) <= destPoint.Y && !isHorizontalMoving)
                    {
                        carTop = top(car);
                        Canvas.SetTop(car, carTop + carSpeed);
                        destPoint = inspector.PreventIntersection(ref car, Directions.Down);
                    }
                }
            }
            else
            {
                // Go left
                if (Canvas.GetLeft(car) >= destPoint.X)
                {
                    carLeft = left(car);
                    Canvas.SetLeft(car, carLeft - carSpeed);
                    destPoint = inspector.PreventIntersection(ref car, Directions.Left);
                }

                // Go Right
                if (Canvas.GetRight(car) <= destPoint.X)
                {
                    carLeft = left(car);
                    Canvas.SetLeft(car, carLeft + carSpeed);
                }

                // Go Up
                if (Canvas.GetTop(car) >= destPoint.Y)
                {
                    carTop = top(car);
                    Canvas.SetTop(car, carTop - carSpeed);
                    destPoint = inspector.PreventIntersection(ref car, Directions.Up);
                }

                // Go Down
                if (Canvas.GetBottom(car) <= destPoint.Y)
                {
                    carTop = top(car);
                    Canvas.SetTop(car, carTop + carSpeed);
                    destPoint = inspector.PreventIntersection(ref car, Directions.Down);
                }
            }

            return destPoint;
        }

        public void MoveRefuellerToDestination(RefuellerElem refueller)
        {
            
            if (refueller.IsFilling)
            {
                modellingSteps.RefillFuelTank(ref refueller);
                return;
            }

            var refuellerView = refueller.Tag as RefuellerView;

            var destPoint = refueller.GetDestinationPoint();
            var destSpot = refueller.DestinationSpot;

            destPoint = MoveRefueller(refueller, destPoint, RefuellerSpeed);

            if(refueller.SpotIsNull)
            {
                destSpot = DpHelper.createDestinationSpot(destPoint);
            }
            
            var refuellerRect = refueller.createIntersectRect();

            if (refuellerRect.IntersectsWith(destSpot))
            {
                refueller.removeDestinationPoint();

                var fuelTank = ((RefuellerView)refueller.Tag).ChosenTank;
                var pointOfFilling = DpHelper.FuelDispensersDestPoints[fuelTank];

                if (destPoint.Equals(pointOfFilling))
                {
                    modellingSteps.StartRefilling(ref refueller);
                }

                if (destPoint.Equals(DpHelper.LeavePointNoFilling))
                {
                    stationCanvas.Children.Remove(refueller);
                }
            }
        }

        public Point MoveRefueller(RefuellerElem refueller, Point destPoint, int refuellerSpeed)
        {
            double refuellerTop;
            double refuellerLeft;

            if (Canvas.GetLeft(refueller) >= destPoint.X)
            {
                refuellerLeft = left(refueller);
                Canvas.SetLeft(refueller, refuellerLeft - refuellerSpeed);
            }

            if (Canvas.GetRight(refueller) <= destPoint.X)
            {
                refuellerLeft = left(refueller);
                Canvas.SetLeft(refueller, refuellerLeft + refuellerSpeed);
            }

            if (Canvas.GetTop(refueller) >= destPoint.Y)
            {
                refuellerTop = top(refueller);
                Canvas.SetTop(refueller, refuellerTop - refuellerSpeed);
            }

            if (Canvas.GetBottom(refueller) <= destPoint.Y)
            {
                refuellerTop = top(refueller);
                Canvas.SetTop(refueller, refuellerTop + refuellerSpeed);
            }

            return destPoint;
        }

        private double left(UIElement element)
        {
            return Canvas.GetLeft(element);
        }

        private double top(UIElement element)
        {
            return Canvas.GetTop(element);
        }
    }
}
