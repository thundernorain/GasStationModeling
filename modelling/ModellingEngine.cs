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

            this.cars = cars;
            this.canvasParser = parsedCanvas;
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
                modellingTimeHelper.TimeBetweenCars = settings.Interval;
                modellingTimeHelper.TicksAfterLastCarSpawning = 0;
            }

            #region LoopingControls

            foreach (var elem in stationCanvas.Children.OfType<MoveableElem>())
            {
                var moveableElem = elem as MoveableElem;

                // Car
                if (moveableElem is CarElem car)
                {
                    router.RouteVehicle(ref moveableElem);

                    stationCanvas = mover.MoveCarToDestination(ref moveableElem);

                    continue;
                }

                // Collector
                if (moveableElem is CollectorElem collector)
                {
                    router.RouteVehicle(ref moveableElem);

                    stationCanvas = mover.MoveCarToDestination(ref moveableElem);

                    continue;
                }

                // Refueller
                if (moveableElem is RefuellerElem refueller)
                {
                    router.RouteRefueller(ref refueller);

                    mover.MoveRefuellerToDestination(refueller);
                }
            }

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
           //TODO
        }

        private void UpdateFuelTankInfo()
        {
            var tanks = canvasParser.Tanks;
            var viewModel = ServiceLocator.Current.GetInstance<ModellingScreenViewModel>();

            if (viewModel != null)
            {
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
}
