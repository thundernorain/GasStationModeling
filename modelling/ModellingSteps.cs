using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling
{
    class ModellingSteps
    {
        public Rectangle CashBox { get; private set; }
        public List<Rectangle> Dispensers { get; private set; }
        public List<Rectangle> Tanks { get; private set; }

        private static bool _isCollectingMoney;
        private static bool _isRefilling;

        private TrafficGenerator trafficGenerator;
        private Canvas stationCanvas;

        public ModellingSteps(
            CanvasParser canvasParsed,
            TrafficGenerator trafficGenerator)
        {
            CashBox = canvasParsed.CashBox;
            Dispensers = canvasParsed.Dispensers;
            Tanks = canvasParsed.Tanks;
            _isCollectingMoney = false;
            _isRefilling = false;
            stationCanvas = canvasParsed.StationCanvas;
            this.trafficGenerator = trafficGenerator;
        }
        
        #region Car

       public void StartFilling(ref CarElem car, ref List<MoveableElem> toAdd)
       {
            var carView = car.Tag as CarView;
            var dispenserView = carView.ChosenDispenser.Tag as DispenserView;

            car.IsFilling = true;
            dispenserView.IsBusy = true;

            var tankWithFuelType = Tanks.First(tank => (tank.Tag as TankView).TypeFuel.Name.Equals(carView.TypeFuel));

            var tankView = tankWithFuelType.Tag as TankView;
            var cashBoxView = CashBox.Tag as CashBoxView;

            carView.PayForFuel(ref cashBoxView, tankView.TypeFuel.Price);
            if(tankView.CurrentFuelVolume > 0)
            {
                dispenserView.refuelCar(ref tankView, ref carView);
            }
            
            if (tankView.IsRunOut && !_isRefilling)
            {
                CallRefueller(tankWithFuelType, ref toAdd);
                _isRefilling = true;
            }

            if (cashBoxView.IsFull && !_isCollectingMoney)
            {
                CallCollector(ref toAdd);
                _isCollectingMoney = true;
            }

            if (carView.CurrentFuelVolume >= carView.CurrentFuelSupply)
            {
                StopFilling(ref car,ref dispenserView);
            }     
       }

        public void StopFilling(ref CarElem car, ref DispenserView fuelDispenser)
        {
            car.IsFilling = false;
            car.IsFilled = true;
            fuelDispenser.CarsInQueue--;
            fuelDispenser.IsBusy = false;

            var carView = car.Tag as CarView;
            carView.ChosenDispenser = null;
            carView.FuelDispenserChosen = false;
        }
        #endregion Car

        #region CashCollector

        public void CallCollector(ref List<MoveableElem> toAdd)
        {
            var collector = trafficGenerator.SpawnCollector();
            toAdd.Add(collector);
        }

        public void CollectCash(ref CollectorElem collector, ref CashBoxView cashBox)
        {
            collector.IsFilling = true;
            var collectorView = collector.Tag as CollectorView;

            collectorView.GetCashFromCashBox(ref cashBox);

            if (cashBox.CurrentCashCount <= 0)
            {
                cashBox.CurrentCashCount = 0;
                StopCollecting(ref collector);
            }
        }

        public void StopCollecting(ref CollectorElem collector)
        {
            collector.IsFilling = false;
            collector.IsFilled = true;

            _isCollectingMoney = false;
        }
        #endregion CashCollector

        #region Refueller

        public void CallRefueller(Rectangle fuelTank,ref List<MoveableElem> toAdd)
        {
            var refueller = trafficGenerator.SpawnRefueller(fuelTank);
            toAdd.Add(refueller);
        }

        public void StartRefilling(RefuellerElem refueller)
        {
            refueller.IsFilling = true;
        }

        public void RefillFuelTank( RefuellerElem refueller)
        {
            var refuellerView = refueller.Tag as RefuellerView;

            var fillingFuelTank = refuellerView.TankData;

            refuellerView.refillFuelTank();

            if (fillingFuelTank.CurrentFuelVolume >= fillingFuelTank.MaxVolume)
            {
                StopRefilling( refueller);
            }
        }

        private static void StopRefilling(RefuellerElem refueller)
        {
            var refuellerView = refueller.Tag as RefuellerView;
            refueller.IsFilling = false;
            refueller.IsFilled = true;

            _isRefilling = false;
        }

        #endregion Refueller
    }
}
