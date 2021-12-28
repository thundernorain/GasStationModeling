using CommonServiceLocator;
using GasStationModeling.core.models;
using GasStationModeling.core.topology;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.managers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using GasStationModeling.settings_screen.model;
using GasStationModeling.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GasStationModeling.modelling
{
    class ModellingEngine
    {

        private ModellingTimeHelper modellingTimeHelper;
        private ModellingSettings settings;
        private Canvas stationCanvas;
        private DestinationPointHelper dpHelper;

        private List<Car> cars;

        private RouteHelper router;
        private MoveHelper mover;
        private TrafficGenerator trafficGenerator;

        private ModellingScreenViewModel mscvModel;

        private CanvasParser canvasParser;

        public ModellingEngine(
            ModellingTimeHelper timeHelper,
            ModellingSettings _settings,
            CanvasParser parsedCanvas,
            List<Car> cars)
        {
            modellingTimeHelper = timeHelper;
            settings = _settings;
            mscvModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();
            this.stationCanvas = parsedCanvas.StationCanvas;
            dpHelper = new DestinationPointHelper(parsedCanvas);

            router = new RouteHelper(parsedCanvas,settings,dpHelper);
            trafficGenerator = new TrafficGenerator(settings,dpHelper);
            mover = new MoveHelper(parsedCanvas, settings, dpHelper);
            this.canvasParser = parsedCanvas;
            this.cars = filterCarList(cars);
        }


        public List<Car> filterCarList(List<Car> cars)
        {

            return cars.Where(car => settings.Fuels.Exists(fuel => fuel.Name == car.TypeFuel)).ToList();
           
        }

        public Canvas Tick(bool IsPaused)
        {
            if (IsPaused)
            {
                return stationCanvas;
            }

            modellingTimeHelper.TimerTicksCount++;
            modellingTimeHelper.TicksAfterLastCarSpawning++;

            if (modellingTimeHelper.TimeAfterLastCarSpawningInSeconds >= modellingTimeHelper.TimeBetweenCars)
            {
                Random r = new Random();
                var randomCarId = r.Next(0, cars.Count - 1);
                var car = cars[randomCarId];

                var carElem = trafficGenerator.SpawnCar(car,stationCanvas);
                modellingTimeHelper.TimeBetweenCars = settings.Interval/5;
                modellingTimeHelper.TicksAfterLastCarSpawning = 0;
            }

            #region LoopingControls

            List<MoveableElem> toDelete = new List<MoveableElem>();

            List<MoveableElem> toAdd = new List<MoveableElem>();

            foreach (var elem in stationCanvas.Children.OfType<MoveableElem>())
            {
                var moveableElem = elem as MoveableElem;

                // Car
                if (moveableElem is CarElem car)
                {
                    moveableElem = router.RouteVehicle(moveableElem);

                    stationCanvas = mover.MoveCarToDestination(moveableElem,ref toDelete,ref toAdd);

                    continue;
                }

                // Collector
                if (moveableElem is CollectorElem collector)
                {
                    moveableElem =  router.RouteVehicle(moveableElem);

                    stationCanvas = mover.MoveCarToDestination(moveableElem,ref toDelete, ref toAdd);

                    continue;
                }

                // Refueller
                if (moveableElem is RefuellerElem refueller)
                {
                    router.RouteRefueller(ref refueller);

                    mover.MoveRefuellerToDestination(refueller, ref toDelete);
                }
            }

            foreach (var elem in toAdd)
            {
                stationCanvas.Children.Add(elem);
                Canvas.SetLeft(elem, dpHelper.SpawnPoint.X);
                Canvas.SetTop(elem,dpHelper.SpawnPoint.Y);            
            }

            foreach (var elem in toDelete)
            {
                stationCanvas.Children.Remove(elem);
            }
            GC.Collect();

            #endregion /LoopingControls

            #region UI

            UpdateTables();

            return stationCanvas;

            #endregion UI
        }

        private void UpdateTables()
        {
            UpdateCashCounterInfo();
            UpdateFuelDispenserInfo();
            UpdateFuelTankInfo();

            //TODO: Label для заправки и кассы
        }

        private void UpdateCashCounterInfo()
        {
            var cashBox = canvasParser.CashBox.Tag as CashBoxView;
            var viewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();

            if (viewModel != null) viewModel.CurrentCashView = cashBox.CurrentCashView;
        }

        private void UpdateFuelDispenserInfo()
        {
            var viewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();
            if (viewModel == null) return;

            var carsInTable = new LinkedList<CarTableItem>();

            foreach (var elem in stationCanvas.Children.OfType<MoveableElem>())
            {
                if (elem.Tag is CarView car) {
                    if (car.FuelDispenserChosen)
                    {
                        var carToAdd = new CarTableItem();
                        carToAdd.TRK = (car.ChosenDispenser.Tag as DispenserView).Id.ToString();
                        carToAdd.Id = car.Id;
                        carToAdd.Name = car.Model;
                        carToAdd.Volume = (int)car.CurrentFuelSupply;
                        carToAdd.Price = (int)car.SpendForFuel;

                        carsInTable.AddFirst(carToAdd);
                    }
                }
            }

            viewModel.CarTableItems = new ObservableCollection<CarTableItem>(carsInTable);
        }

        private void UpdateFuelTankInfo()
        {
            var tanks = canvasParser.Tanks;
            var viewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();
            if (viewModel == null) return;

            StringBuilder fuelCount = new StringBuilder("");

            foreach (var tank in tanks)
            {
                    var tankView = tank.Tag as TankView;
                    fuelCount.Append(tankView.CurrentFuelVolumeView + "\n");
            }

            viewModel.CurrentFuelVolumeView = fuelCount.ToString();
        }
    }
}
