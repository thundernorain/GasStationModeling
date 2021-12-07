using GalaSoft.MvvmLight;
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
            CurrentPageUri = new Uri(SETTINGS_SCREEN_URI, UriKind.Relative);

            pageIndicatorBrushes = InitBrushes();
            SetIndicatorBrushesActive(CurrentPageUri);
        }

        public void SetIndicatorBrushesActive(Uri pageUri)
        {
            pageIndicatorBrushes[0] = Application.Current.TryFindResource("AquaBrush") as Brush;
            RaisePropertyChanged(() => PageIndicatorBrushes);
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