using System.Windows;

namespace GasStationModeling.settings_screen
{
    class DeleteConfirmationMessageBoxShower
    {
        public static MessageBoxResult show(string name)
        {
            return MessageBox.Show(
                " Удалить " + name + "?", 
                " Подтвердите удаление",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );
        }
    }
}
