using CommonServiceLocator;
using GasStationModeling.add_forms;
using GasStationModeling.exceptions;
using GasStationModeling.settings_screen.model;
using GasStationModeling.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GasStationModeling.settings_screen.view
{
    /// <summary>
    /// Interaction logic for SettingsScreen.xaml
    /// </summary>
    public partial class SettingsScreen : Page
    {
        public SettingsScreen()
        {
            InitializeComponent();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsViewModel = ServiceLocator.Current.GetInstance<SettingsScreenViewModel>();
            try
            {
                ModellingSettings settings = new ModellingSettings()
                {
                    Interval = IntervalSlider.Value,
                    ArrivalProbability = ProbabilitySlider.Value,
                    Fuels = settingsViewModel.getChosenFuels() ?? throw new NullReferenceException("Не выбраны виды топлива"),
                    CashLimit = CashLimitSlider.Value,
                    Dispenser = settingsViewModel.getChosenFuelDispenser() ?? throw new NullReferenceException("Не выбрана ТРК"),
                    FuelTank = settingsViewModel.getChosenTank() ?? throw new NullReferenceException("Не выбран ТБ")
                };
                var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
                mainViewModel.CurrentPageUri = new Uri(MainViewModel.MODELLING_SCREEN_URI, UriKind.Relative);
                mainViewModel.ModellingSettings = settings;
            }
            catch(Exception ex)
            {
                ErrorMessageBoxShower.show(ex.Message);
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.CurrentPageUri = new Uri(MainViewModel.TOPOLOGY_SCREEN_URI, UriKind.Relative);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddModelTrkWindow();
            window.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new AddFuelTankWindow();
            window.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var window = new AddFuelTypeWindow();
            window.Show();
        }

        private void IntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            IntervalValue.Text = ((int)IntervalSlider.Value).ToString();
        }

        private void CashLimitSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CashLimitValue.Text = ((int)CashLimitSlider.Value).ToString();
        }

        private void ProbabilitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProbabilityValueTB.Text = String.Format("{0:0.##}", ProbabilitySlider.Value);
        }
    }
}
