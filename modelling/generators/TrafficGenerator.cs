using GasStationModeling.core.models;
using GasStationModeling.modelling.generators;
using GasStationModeling.modelling.managers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using GasStationModeling.settings_screen.model;
using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling
{
    public class TrafficGenerator
    { 
        private ModellingSettings settings;

        private DestinationPointHelper destPointHelper;

        public TrafficGenerator(
            ModellingSettings settings,
            DestinationPointHelper destPointHelper)
        {
            this.settings = settings;
            this.destPointHelper = destPointHelper;
        }

        public CarElem SpawnCar(Car car,  Canvas stCanvas)
        {
            Random r = new Random();

            var carElem = MoveableElemGenerator.createCarElem(car, destPointHelper.SpawnPoint);

            if (r.NextDouble() <= settings.ArrivalProbability)
            {
                carElem.IsGoingToFill = true;
            }

            if (!carElem.IsGoingToFill)
            {
                carElem.AddDestinationPoint(destPointHelper.LeavePointNoFilling);
            }
            stCanvas.Children.Add(carElem);

            Canvas.SetLeft(carElem, destPointHelper.SpawnPoint.X);
            Canvas.SetTop(carElem, destPointHelper.SpawnPoint.Y);

            return carElem;
        }

        public CollectorElem SpawnCollector(ref Canvas stCanvas)
        {
            var elem =  MoveableElemGenerator.createCollectorElem(
                ModellingTimeHelper.TIMER_TICK_MILLISECONDS,
                settings.CashLimit / settings.CollectingTimeSec,
                destPointHelper.SpawnPoint);
            stCanvas.Children.Add(elem);

            Canvas.SetLeft(elem, destPointHelper.SpawnPoint.X);
            Canvas.SetLeft(elem, destPointHelper.SpawnPoint.Y);
            return elem;
        }

        public RefuellerElem SpawnRefueller(Rectangle fuelTank, ref Canvas stCanvas)
        {
            var elem = MoveableElemGenerator.createRefuellerElem(
                fuelTank,
                ModellingTimeHelper.TIMER_TICK_MILLISECONDS,
                settings.FuelTank.LimitVolume / settings.RefuellingTimeSec,
                destPointHelper.SpawnPoint);
            stCanvas.Children.Add(elem);
            Canvas.SetLeft(elem, destPointHelper.SpawnPoint.X);
            Canvas.SetLeft(elem, destPointHelper.SpawnPoint.Y);
            return elem;
        }
    }
}
