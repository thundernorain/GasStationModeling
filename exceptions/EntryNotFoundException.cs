using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
