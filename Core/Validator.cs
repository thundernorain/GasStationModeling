using System;

namespace GasStationModeling.core
{
    class Validator
    {
        public static bool isNameCorrect(String input)
        {
            return input != null && input.Length > 0;
        }

        public static bool isValueCanBeParsedAndPositiveCorrect(String input)
        {
            if (Double.TryParse(input, out double size))
            {
                return size > 0;
            }
            return false;
        }
    }
}
