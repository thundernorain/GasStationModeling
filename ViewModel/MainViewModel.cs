using GalaSoft.MvvmLight;
using GasStationModeling.settings_screen.view;
using System.Windows.Controls;

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
        private Page settingsScreen;

        private Page currentPage;

        public Page CurrentPage {
            get
            {
                return currentPage;
            }
            set
            {
                if (currentPage == value)
                    return;

                currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

        public MainViewModel()
        {
            settingsScreen = new SettingsScreen();

            CurrentPage = settingsScreen;
        }
    }
}