using System;

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
        public const string FUELS_COUNT_NOT_EQUALS_TANKS_COUNT = "* Количество выбранных видов топлива не соответствует" +
            "\n количеству ТБ на топологии";
    }
}
