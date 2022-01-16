using GasStationModeling.core.models;
using GasStationModeling.modelling.models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.model
{
    public class DispenserView : TopologyView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double SpeedRefuelingPerTick { get; set; }

        public bool IsBusy { get; set; }

        public int CarsInQueue { get; set; }

        public Rectangle CurrentTank { get; set; }

        public DispenserView(int id,FuelDispenser dispenser, double tick)
        {
            Id = id;
            Name = dispenser.Name;

            //Скорость в литрах в минуту
            //Моделирование работает на миллисекундах
            //умножаем скорость на размер тика
            // и делим на 1000 (кол-во миллисекнуд в секунде)
            SpeedRefuelingPerTick = dispenser.SpeedRefueling * tick / 10;

            IsBusy = false;
            CarsInQueue = 0;
        }

        public void refuelCar(ref TankView tankView,ref CarView car)
        {
            if(tankView.CurrentFuelVolume > SpeedRefuelingPerTick)
            {
                tankView.CurrentFuelVolume -= SpeedRefuelingPerTick;
                car.CurrentFuelVolume += SpeedRefuelingPerTick;
            }  
            else
            {
                car.CurrentFuelVolume += tankView.CurrentFuelVolume;
                tankView.CurrentFuelVolume = 0;
            }
        }
    }
}
