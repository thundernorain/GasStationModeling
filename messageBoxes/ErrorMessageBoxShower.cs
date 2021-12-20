using System.Windows;

namespace GasStationModeling.add_forms
{
    class ErrorMessageBoxShower
    {
        public static void show(string errorMessage)
        {
            MessageBox.Show(
                    errorMessage,
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
        }
    }
}
