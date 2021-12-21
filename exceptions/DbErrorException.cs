using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.exceptions
{
    public class DbErrorException : Exception
    {
        public DbErrorException(string message)
            : base(message)
        {
        }
    }

    class DbErrorMessage
    {
        public static string CONNECTION_ERROR = " Ошибка подключения к базе данных";
    }
}
