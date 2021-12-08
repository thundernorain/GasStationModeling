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
    /// Interaction logic for AddFuelTankWindow.xaml
    /// </summary>
    public partial class AddFuelTankWindow : Window
    {
        private static IMongoDatabase DB = DbInitializer.getInstance();
        private static DbWorker<Tank> dbWorker = new DbWorker<Tank>(DB, "fuelTanks");

        public AddFuelTankWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = FuelTankNameTB.Text;
            if (!Validator.isNameCorrect(name))
            {
                ErrorMessageBoxShower.show("Некорректное название ТБ");
                return;
            }

            string size = FuelSizeTB.Text;
            if (!Validator.isValueCanBeParsedAndPositiveCorrect(size))
            {
                ErrorMessageBoxShower.show("Некорректное значение объёма ТБ");
                return;
            }
            var volume = Double.Parse(size);

            Tank tank = new Tank
            {
                Name = name,
                MaxVolume = volume
            };
            dbWorker.insertEntry(tank);
            Close();
        }
    }
}
