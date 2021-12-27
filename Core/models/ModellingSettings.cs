using GasStationModeling.core.models;
using System.Collections.Generic;

namespace GasStationModeling.settings_screen.model
{
    public class ModellingSettings
    {
        public double Interval { get; set; }

        public double ArrivalProbability { get; set; }

        public List<Fuel> Fuels { get; set; }

        public double CashLimit { get; set; }

        public FuelDispenser Dispenser { get; set; }

        public Tank FuelTank { get; set; }

        public double CollectingTimeSec { get; } = 4;

        public double RefuellingTimeSec { get; } = 5;
    }
}
