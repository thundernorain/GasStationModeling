
using GasStationModeling.modelling.models;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.model
{
    public class RefuellerView : TopologyView
    {
        public double SpeedRefueling { get; set; }

        public Rectangle ChosenTank { get; set; }

        public double FuelAmount { get; set; }

        public TankView TankData
        {
            get
            {
                return ChosenTank.Tag as TankView;
            }
        }

        public RefuellerView(double tick, double speedOfRefuellingPerSecond)
        {
            SpeedRefueling = speedOfRefuellingPerSecond * tick / 1000;
        }

        public void refillFuelTank()
        {
            TankData.CurrentFuelVolume += SpeedRefueling;
            FuelAmount -= SpeedRefueling;
            if (TankData.CurrentFuelVolume > TankData.MaxVolume && FuelAmount > 0)
            {
                TankData.CurrentFuelVolume = TankData.MaxVolume;
            }
        }
    }
}
