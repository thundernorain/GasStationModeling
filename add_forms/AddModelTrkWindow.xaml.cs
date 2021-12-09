using GasStationModeling.core;
using GasStationModeling.core.DB;
using GasStationModeling.core.models;
using GasStationModeling.DB;
using MongoDB.Driver;
using System;
using System.Windows;

namespace GasStationModeling.add_forms
{
    /// <summary>
    /// Interaction logic for AddModelTrkWindow.xaml
    /// </summary>
    public partial class AddModelTrkWindow : Window
    {
        private static IMongoDatabase DB = DbInitializer.getInstance();
        private static DbWorker<FuelDispenser> dbWorker = new DbWorker<FuelDispenser>(DB, DBWorkerKeys.FUEL_DISPENSERS_KEY);
        public AddModelTrkWindow()
        {
            InitializeComponent();
        }

        private void AddDispenserButton_Click(object sender, RoutedEventArgs e)
        {
            string name = DispenserNameTB.Text;
            if (!Validator.isNameCorrect(name))
            {
                ErrorMessageBoxShower.show("Некорректное название ТРК");
                return;
            }

            string speedInput = DispenserSpeedTB.Text;
            if (!Validator.isValueCanBeParsedAndPositiveCorrect(speedInput))
            {
                ErrorMessageBoxShower.show("Некорректное значение скорости заправки");
                return;
            }
            var speed = Double.Parse(speedInput);

            FuelDispenser fuelDispenser = new FuelDispenser
            {
                Name = name,
                SpeedRefueling = speed
            };
            dbWorker.insertEntry(fuelDispenser);
            Close();
        }
    }
}
