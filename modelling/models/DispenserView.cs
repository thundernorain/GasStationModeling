using GasStationModeling.core.models;
using GasStationModeling.modelling.models;
using System.Collections.Generic;
using System.Linq;

namespace GasStationModeling.modelling.model
{
    public class DispenserView : TopologyView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double SpeedRefuelingPerTick { get; set; }

        public bool IsBusy { get; set; }

        public int CarsInQueue { get; set; }

        public DispenserView(int id,FuelDispenser dispenser, double tick)
        {
            Id = id;
            Name = dispenser.Name;

            //Скорость в литрах в минуту
            //Моделрование работает на миллисекундах
            //умножаем скорость на размер тика
            // делим на 60 (кол-во секунд в минуте)
            // и 100 (кол-во миллисекнуд в секунде)
            SpeedRefuelingPerTick = dispenser.SpeedRefueling * tick / 1000;

            IsBusy = false;
            CarsInQueue = 0;
        }

        public void refuelCar(ref TankView tankView,ref CarView car)
        {
            tankView.CurrentFuelVolume -= car.CurrentFuelSupply / 2;
            car.CurrentFuelVolume += car.CurrentFuelSupply/2;
        }
    }
}
