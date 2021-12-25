using System;

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
