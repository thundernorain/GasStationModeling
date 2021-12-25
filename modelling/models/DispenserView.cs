using GasStationModeling.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.modelling.model
{
    class DispenserView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double SpeedRefuelingPerTick { get; set; }

        public bool IsBusy { get; set; }

        public int CarsInQueue { get; set; }

        public DispenserView(FuelDispenser dispenser, double tick)
        {
            Name = dispenser.Name;

            //Скорость в литрах в минуту
            //Моделрование работает на миллисекундах
            //умножаем скорость на размер тика
            // делим на 60 (кол-во секунд в минуте)
            // и 100 (кол-во миллисекнуд в секунде)
            SpeedRefuelingPerTick = dispenser.SpeedRefueling * tick / (60 * 1000);

            IsBusy = false;
            CarsInQueue = 0;
        }

        public void getFuelFromTank(List<TankView> tanks, string selectedFuel)
        {
            var tankWithTypeOfFuel = tanks
                .Where(tank => tank.TypeFuel.Equals(selectedFuel))
                .First();

            tankWithTypeOfFuel.CurrentFuelVolume -= SpeedRefuelingPerTick;
        }
    }
}
