using GasStationModeling.add_forms;
using GasStationModeling.core.DB;
using GasStationModeling.developers_info_window.view;
using GasStationModeling.exceptions;
using GasStationModeling.main_window.view;
using System;
using System.Configuration;
using System.Globalization;
using System.Windows;

namespace GasStationModeling.welcome_window.view
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            try
            {
                InitializeComponent();
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-EN");
                DbInitializer.getInstance();
            }
            catch (Exception ex)
            {
                ErrorMessageBoxShower.ShowError(DbErrorMessage.CONNECTION_ERROR);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var systemInfoFilePath = ConfigurationManager.AppSettings.Get("InfoFilePath"); ;
            try
            {
                System.Diagnostics.Process.Start(systemInfoFilePath);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ErrorMessageBoxShower.show("Ошибка системы. Файл справочной информации не найден.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var developersInfoWindow = new DevelopersInfoWindow();
            developersInfoWindow.Show();
        }
    }
}
