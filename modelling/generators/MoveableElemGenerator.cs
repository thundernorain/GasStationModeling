using GasStationModeling.core.models;
using GasStationModeling.modelling.helpers;
using GasStationModeling.modelling.model;
using GasStationModeling.modelling.moveableElems;
using GasStationModeling.modelling.pictureView;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.generators
{
    public class MoveableElemGenerator
    {
        private static int carId = 0;

        public static CarElem createCarElem(Car car, Point location)
        {
            var carView = new CarView(carId, car);
            carId++;

            var brush = BrushHelper.getBrushFor("Car");

            brush.TileMode = TileMode.Tile;
            // set the bg view port unit
            brush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;

            return new CarElem(brush,location,carView);
        }

        public static CollectorElem createCollectorElem(
            double tick,
            double collectingSpeed,
            Point location)
        {
            var collectorView = new CollectorView(tick, collectingSpeed);

            var brush = BrushHelper.getBrushFor("Collector");

            return new CollectorElem(brush, location, collectorView);
        }

        public static RefuellerElem createRefuellerElem(
            Rectangle tank,
            double tick,
            double refuellingSpeed,
            Point location)
        {
            var refuellerView = new RefuellerView(tick, refuellingSpeed);

            refuellerView.ChosenTank = tank;

            var brush = BrushHelper.getBrushFor("Refueller");

            return new RefuellerElem(brush, location, refuellerView);
        }
    }
}
