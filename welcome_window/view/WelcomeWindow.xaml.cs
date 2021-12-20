﻿using GasStationModeling.add_forms;
using GasStationModeling.core.DB;
using GasStationModeling.developers_info_window.view;
using GasStationModeling.main_window.view;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
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

namespace GasStationModeling.welcome_window.view
{
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-EN");
            DbInitializer.getInstance();
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
