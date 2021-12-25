﻿using GasStationModeling.core.models;
using GasStationModeling.modelling.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GasStationModeling.modelling.mapper
{
    public class ModellingScreenMapper
    {

        public static ObservableCollection<CarTableItem> CreateCarTableItems(List<Car> cars, List<Fuel> fuels)
        {
            ObservableCollection<CarTableItem> carTableItems = new ObservableCollection<CarTableItem>();

            Random r = new Random();
            cars.ForEach(car =>
            {
                if(fuels.Exists(fuel => fuel.Name == car.TypeFuel))
                {
                    double price = fuels.Find(fuel => fuel.Name == car.TypeFuel).Price;
                    carTableItems.Add(new CarTableItem()
                    {
                        Id = cars.IndexOf(car),
                        Name = car.Model,
                        Volume = car.CurrentFuelSupply,
                        Price = price
                    });
                }         
            });

            return carTableItems;
        }

        public static List<FuelTableItem> CreateFuelTableitems(List<Fuel> fuels)
        {
            List<FuelTableItem> fuelTableItems = new List<FuelTableItem>();
            fuels.ForEach(fuel => {
                fuelTableItems.Add(new FuelTableItem()
                {
                    Name = fuel.Name,
                    Price = fuel.Price
                });
            });
            return fuelTableItems;
        }

        public static List<TankView> initializeTankViews(List<Fuel> fuels, double maxVolume)
        {
            List<TankView> tankViews = new List<TankView>();
            foreach (var fuel in fuels)
            {
                tankViews.Add(new TankView()
                {
                    MaxVolume = maxVolume,
                    CurrentFuelVolume = maxVolume,
                    FuelName = fuel.Name
                });
            }
            return tankViews;
        }
    }
}
