using CommonServiceLocator;
using GasStationModeling.add_forms;
using GasStationModeling.exceptions;
using GasStationModeling.Properties;
using GasStationModeling.settings_screen.model;
using GasStationModeling.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                    Fuels = settingsViewModel.getChosenFuels() ?? throw new NullReferenceException(),
                    CashLimit = CashLimitSlider.Value,
                    Dispenser = settingsViewModel.getChosenFuelDispenser() ?? throw new NullReferenceException(),
                    FuelTank = settingsViewModel.getChosenTank() ?? throw new NullReferenceException()
                };
                var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
                mainViewModel.CurrentPageUri = new Uri(MainViewModel.MODELLING_SCREEN_URI, UriKind.Relative);
                mainViewModel.ModellingSettings = settings;
            }
            catch(ParameterNotSelectedException ex)
            {
                ErrorMessageBoxShower.show(ex.Message);
            }
            catch(NullReferenceException ex)
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
