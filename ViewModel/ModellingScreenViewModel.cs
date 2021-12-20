using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using GasStationModeling.modelling.model;
using GasStationModeling.settings_screen.model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.ViewModel
{
    public class ModellingScreenViewModel : ViewModelBase
    {
        public ModellingSettings Settings { get; set; }

        public ObservableCollection<CarTableItem> CarTableItems { get; set; }

        public List<FuelTableItem> Fuels { get; set; }

        public double CurrentFuelVolume { get; set; }

        public double CurrentCashCount { get; set; }

        public ModellingScreenViewModel()
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            Settings = mainViewModel.ModellingSettings;
            var db = DbInitializer.getInstance();

            List<Car> cars = getCarsFromDB(db);
            CarTableItems = CreateCarTableItems(cars);
            Fuels = CreateFuelTableitems(Settings.Fuels);

            CurrentCashCount = 0;
            CurrentFuelVolume = Settings.FuelTank.MaxVolume;
        }

        public List<Car> getCarsFromDB(IMongoDatabase db)
        {
            var dbWorker = new DbWorker<Car>(db, DBWorkerKeys.CARS_KEY);
            return dbWorker.getCollection();
        }

        public ObservableCollection<CarTableItem> CreateCarTableItems(List<Car> cars)
        {
            ObservableCollection<CarTableItem> carTableItems = new ObservableCollection<CarTableItem>();

            Random r = new Random();
            cars.ForEach(car =>
            {
                double price = Fuels.Find(fuel => fuel.Name == car.TypeFuel).Price;
                carTableItems.Add(new CarTableItem()
                {
                    Id = cars.IndexOf(car),
                    Name = car.Model,
                    Volume = r.Next(1, car.MaxVolumeTank),
                    Price = price
                });
            });

            return carTableItems;
        }

        public List<FuelTableItem> CreateFuelTableitems(List<Fuel> fuels)
        {
            List <FuelTableItem> fuelTableItems= new List<FuelTableItem>();
            fuels.ForEach(fuel => {
                fuelTableItems.Add(new FuelTableItem()
                {
                    Name = fuel.Name,
                    Price = fuel.Price
                });
            });
            return fuelTableItems;
        }
    }
}
