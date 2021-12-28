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

        public static void ShowTopology(string errorMessage)
        {
            MessageBox.Show(
                errorMessage,
                "Ошибка при добавлении элемента",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        public static void ShowError(string errorMessage)
        {
            MessageBox.Show(
                errorMessage,
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
