using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.exceptions
{
    public class ParameterNotSelectedException : Exception
    {
        public ParameterNotSelectedException(string message)
            :base(message)
        {    
        }
    }

    class ParameterErrorMessage
    {
        public const string FUELS_NOT_SELECTED = "* Не выбран параметр \"Вид топлива\"";
        public const string DISPENSER_NOT_SELECTED = "* Не выбран параметр \"ТРК\"";
        public const string TANK_NOT_SELECTED = "* Не выбран параметр \"ТБ\"";
    }
}
