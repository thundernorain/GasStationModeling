using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.ViewModel
{
    class ViewModelUtils
    {
        public static void ReloadMainViewModels()
        {
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Unregister<SettingsScreenViewModel>();
            SimpleIoc.Default.Register<SettingsScreenViewModel>();

            SimpleIoc.Default.Unregister<ModellingScreenViewModel>();
            SimpleIoc.Default.Register<ModellingScreenViewModel>();
        }
    }
}
