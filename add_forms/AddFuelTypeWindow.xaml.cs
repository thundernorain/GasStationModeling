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
    /// Interaction logic for AddFuelTypeWindow.xaml
    /// </summary>
    public partial class AddFuelTypeWindow : Window
    {
        private static IMongoDatabase DB = DbInitializer.getInstance();
        private static DbWorker<Fuel> dbWorker = new DbWorker<Fuel>(DB, DBWorkerKeys.FUEL_TYPES_KEY);

        public AddFuelTypeWindow()
        {
            InitializeComponent();
        }

        private void AddFuelTypeButton_Click(object sender, RoutedEventArgs e)
        {
            string name = FuelTypeNameTB.Text;
            if (!Validator.isNameCorrect(name))
            {
                ErrorMessageBoxShower.show("Некорректное название топлива");
                return;
            }

            string priceInput = FuelTypePriceTB.Text;
            if (!Validator.isValueCanBeParsedAndPositiveCorrect(priceInput))
            {
                ErrorMessageBoxShower.show("Некорректное значение стоимости топлива");
                return;
            }
            var price = Double.Parse(priceInput);

            Fuel fuel = new Fuel
            {
                Name = name,
                Price = price
            };
            dbWorker.insertEntry(fuel);
            Close();
        }
    }
}
