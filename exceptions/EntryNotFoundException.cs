using System;

namespace GasStationModeling.exceptions
{
    public class EntryNotFoundException: Exception
    {
        public EntryNotFoundException(string message)
            : base(message)
        {
        }  
    }

    class EntryNotFoundErrorMessage
    {
        public const string NOT_FOUND = "* Объект не найден";
    }
}
