using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.modelling.model
{
    class RefuellerView
    {
        public double SpeedRefueling { get; set; }

        public RefuellerView(double speedOfRefuellingPerSecond, double tick)
        {
            SpeedRefueling = speedOfRefuellingPerSecond * tick / 1000;
        }

        public TankView refillFuelTank(TankView tank)
        {
            tank.CurrentFuelVolume += SpeedRefueling;
            return tank;
        }
    }
}
