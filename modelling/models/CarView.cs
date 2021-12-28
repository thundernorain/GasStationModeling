﻿using GasStationModeling.core.models;
using GasStationModeling.modelling.models;
using System;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.model
{
    public class CarView : TopologyView
    {
        const int MIN_FUEL_SUPPLY = 6;
        public int Id { get; set; }

        public string Model { get; set; }

        public string TypeFuel { get; set; }

        public double CurrentFuelSupply { get; set; }

        public double CurrentFuelVolume { get; set; }

        public double MaxVolumeTank { get; set; }

        public bool FuelDispenserChosen { get; set; }

        public double SpendForFuel { get; set; }

        public bool IsGoingToFill { get; set; }

        public Rectangle ChosenDispenser { get; set; }

        public CarView(int id,Car car)
        {
            Id = id;
            Model = car.Model;
            TypeFuel = car.TypeFuel;
            MaxVolumeTank = car.MaxVolumeTank;
            FuelDispenserChosen = false;
            IsGoingToFill = false;
            SpendForFuel = 0;

            Random random = new Random();
            CurrentFuelSupply = MIN_FUEL_SUPPLY + random.NextDouble() * (MaxVolumeTank - MIN_FUEL_SUPPLY);
        }

        public void PayForFuel(ref CashBoxView cashBoxView, double price)
        {
            SpendForFuel = CurrentFuelSupply * price;
            cashBoxView.CurrentCashCount += SpendForFuel;
        }
    }
}
