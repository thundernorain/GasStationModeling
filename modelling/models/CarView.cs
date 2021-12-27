using GasStationModeling.core.models;
using GasStationModeling.modelling.models;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.model
{
    public class CarView : TopologyView
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string TypeFuel { get; set; }

        public double CurrentFuelSupply { get; set; }

        public double CurrentFuelVolume { get; set; }

        public double MaxVolumeTank { get; set; }

        public bool FuelDispenserChosen { get; set; }

        public bool IsGoingToFill { get; set; }

        public Rectangle ChosenDispenser { get; set; }

        public CarView(int id,Car car)
        {
            Id = id;
            Model = car.Model;
            TypeFuel = car.TypeFuel;
            CurrentFuelSupply = car.CurrentFuelSupply;
            MaxVolumeTank = car.MaxVolumeTank;
            FuelDispenserChosen = false;
            IsGoingToFill = false;
        }

        public void PayForFuel(ref CashBoxView cashBoxView, double price)
        {
            cashBoxView.CurrentCashCount += CurrentFuelSupply * price;
        }
    }
}
