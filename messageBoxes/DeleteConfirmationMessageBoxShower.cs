using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
