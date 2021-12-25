using GasStationModeling.core.models;

namespace GasStationModeling.modelling.model
{
    public class TankView
    {
        private double _currentFuelVolume;

        public string CurrentFuelVolumeView { get; set; }

        public double MaxVolume { get; set; }

        public string TypeFuel { get; set; }

        private double LimitVolume { get; set; }

        public bool IsRunOut => CurrentFuelVolume <= LimitVolume;

        public double CurrentFuelVolume
        {
            get => _currentFuelVolume;
            set
            {
                _currentFuelVolume = value;
                CurrentFuelVolumeView = "Объём топлива в ТБ (м3) : " + _currentFuelVolume + " \\ " + MaxVolume + " м3";
            }
        }

        public TankView(Tank tank)
        {
            MaxVolume = tank.MaxVolume;
            LimitVolume = tank.LimitVolume;
            TypeFuel = tank.TypeFuel;
            CurrentFuelVolume = MaxVolume;
        }
    }
}
