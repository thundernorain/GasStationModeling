using GasStationModeling.core.models;
using GasStationModeling.modelling.generators;
using GasStationModeling.modelling.managers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using GasStationModeling.settings_screen.model;
using System;
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

        public CarElem SpawnCar(Car car)
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
            return carElem;
        }

        public CollectorElem SpawnCollector()
        {
            var elem =  MoveableElemGenerator.createCollectorElem(
                ModellingTimeHelper.TIMER_TICK_MILLISECONDS,
                settings.CashLimit / settings.CollectingTimeSec,
                destPointHelper.SpawnPoint);
            var cashBoxPoint = destPointHelper.CashBoxPoint;
            elem.AddDestinationPoint(cashBoxPoint);
            return elem;
        }

        public RefuellerElem SpawnRefueller(Rectangle fuelTank)
        {
            var elem = MoveableElemGenerator.createRefuellerElem(
                fuelTank,
                ModellingTimeHelper.TIMER_TICK_MILLISECONDS,
                settings.FuelTank.LimitVolume / settings.RefuellingTimeSec,
                destPointHelper.SpawnPoint);

            var tankPoint = destPointHelper.RefuellerDestPoints[fuelTank];

            elem.AddDestinationPoint(tankPoint);

            return elem;
        }
    }
}
