using System.Windows.Threading;

namespace GasStationModeling.modelling.managers
{
    public class ModellingTimeHelper
    {
        public const int TIMER_TICK_MILLISECONDS = 20;

        public static int TimerTicksCount { get; set; }
        public static bool IsPaused { get; set; }
        public static int TicksAfterLastCarSpawning { get; set; }
        public static double TimeAfterLastCarSpawningInSeconds => TicksAfterLastCarSpawning * (double)TIMER_TICK_MILLISECONDS / 1000;
        public static double TimeBetweenCars { get; set; }

        public ModellingTimeHelper() { }

        static void SetUpTimer(DispatcherTimer timer)
        {
            TimerTicksCount = 0;
            IsPaused = false;

            TicksAfterLastCarSpawning = 0;
            TimeBetweenCars = 0;
        }

    }
}
