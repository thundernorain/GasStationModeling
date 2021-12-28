﻿using GasStationModeling.core.models;
using GasStationModeling.modelling.models;

namespace GasStationModeling.modelling.model
{
    public class TankView : TopologyView
    {
        public int Id { get; set; }

        private double _currentFuelVolume;

        public string CurrentFuelVolumeView { get; set; }

        public double MaxVolume { get; set; }

        public Fuel TypeFuel { get; set; }

        private double LimitVolume { get; set; }

        public bool IsRunOut => CurrentFuelVolume <= LimitVolume;

        public double CurrentFuelVolume
        {
            get => _currentFuelVolume;
            set
            {
                _currentFuelVolume = value;
                CurrentFuelVolumeView = "Объём топлива в ТБ " + TypeFuel.Name + " (м3) : " + _currentFuelVolume + " \\ " + MaxVolume + " м3";
            }
        }

        public TankView(int id,Tank tank)
        {
            Id = id;
            MaxVolume = tank.MaxVolume;
            LimitVolume = tank.LimitVolume;
            TypeFuel = tank.TypeFuel;
            CurrentFuelVolume = MaxVolume;
        }
    }
}
