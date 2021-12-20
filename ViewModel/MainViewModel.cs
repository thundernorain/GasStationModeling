using GalaSoft.MvvmLight;
using GasStationModeling.Properties;
using GasStationModeling.settings_screen.model;
using GasStationModeling.settings_screen.view;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GasStationModeling.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public const string SETTINGS_SCREEN_URI = "/../settings_screen/view/SettingsScreen.xaml";
        public const string TOPOLOGY_SCREEN_URI = "/../topology/view/TopologyScreen.xaml";
        public const string MODELLING_SCREEN_URI = "/../modelling/view/ModellingPage.xaml";

        public ModellingSettings ModellingSettings { get; set; }

        private Uri currentPageUri;
       
        public Uri CurrentPageUri
        {
            get
            {
                return currentPageUri;
            }

            set
            {
                if (currentPageUri == value)
                    return;

                currentPageUri = value;

                SetIndicatorBrushesActive(value);

                RaisePropertyChanged(() => CurrentPageUri);
            }
        }

        private Brush[] pageIndicatorBrushes;

        public Brush[] PageIndicatorBrushes
        {
            get { return pageIndicatorBrushes; }
        }

        public MainViewModel()
        {
            pageIndicatorBrushes = InitBrushes();
            CurrentPageUri = new Uri(TOPOLOGY_SCREEN_URI, UriKind.Relative);
        }

        public void SetIndicatorBrushesActive(Uri pageUri)
        {
            SetGrayIndicator();

            if (pageUri.ToString().Contains("SettingsScreen.xaml"))
            {
                pageIndicatorBrushes[0] = Application.Current.TryFindResource("AquaBrush") as Brush;
            }

            if (pageUri.ToString().Contains("TopologyScreen.xaml"))
            {
                pageIndicatorBrushes[1] = Application.Current.TryFindResource("AquaBrush") as Brush;
            }

            if (pageUri.ToString().Contains("ModellingPage.xaml"))
            {
                pageIndicatorBrushes[2] = Application.Current.TryFindResource("AquaBrush") as Brush;
            }


            RaisePropertyChanged(() => PageIndicatorBrushes);
        }

        private void SetGrayIndicator()
        {
            pageIndicatorBrushes[0] = Application.Current.TryFindResource("LightGrayBrush") as Brush;
            pageIndicatorBrushes[1] = Application.Current.TryFindResource("LightGrayBrush") as Brush;
            pageIndicatorBrushes[2] = Application.Current.TryFindResource("LightGrayBrush") as Brush;
        }

        private Brush[] InitBrushes()
        {
            return new Brush[]
            {
                Application.Current.TryFindResource("LightGrayBrush") as Brush,
                Application.Current.TryFindResource("LightGrayBrush") as Brush,
                Application.Current.TryFindResource("LightGrayBrush") as Brush
            };
        }
    }
}