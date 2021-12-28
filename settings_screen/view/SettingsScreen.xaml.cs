using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GasStationModeling.add_forms;
using GasStationModeling.exceptions;
using GasStationModeling.main_window.view;
using GasStationModeling.modelling.view;
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
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            try
            {
                ModellingSettings settings = new ModellingSettings();

                settings.Interval = IntervalSlider.Value;
                settings.ArrivalProbability = ProbabilitySlider.Value;
                settings.Fuels = settingsViewModel.getChosenFuels() ?? throw new NullReferenceException("Не выбраны виды топлива");
                settings.CashLimit = CashLimitSlider.Value;
                settings.Dispenser = settingsViewModel.getChosenFuelDispenser() ?? throw new NullReferenceException("Не выбрана ТРК");
                settings.FuelTank = settingsViewModel.getChosenTank() ?? throw new NullReferenceException("Не выбран ТБ");
                
               
                mainViewModel.CurrentPageUri = new Uri(MainViewModel.MODELLING_SCREEN_URI, UriKind.Relative);
                mainViewModel.ModellingSettings = settings;

                SimpleIoc.Default.Unregister<ModellingScreenViewModel>();
                SimpleIoc.Default.Register<ModellingScreenViewModel>();

                mainViewModel.CurrentPageUri = new Uri(MainViewModel.MODELLING_SCREEN_URI, UriKind.Relative);
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
    }
}
