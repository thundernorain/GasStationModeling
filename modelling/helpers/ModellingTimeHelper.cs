using System.Windows.Threading;

namespace GasStationModeling.modelling.managers
{
    public class ModellingTimeHelper
    {
        const int TIMER_TICK_MILLISECONDS = 20;

        static int TimerTicksCount { get; set; }
        static bool IsPaused { get; set; }

        static int TicksAfterLastCarSpawning { get; set; }
        static double TimeAfterLastCarSpawningInSeconds => TicksAfterLastCarSpawning * (double)TIMER_TICK_MILLISECONDS / 1000;
        static double TimeBetweenCars { get; set; }

        static void SetUpTimer(DispatcherTimer timer)
        {
            TimerTicksCount = 0;
            IsPaused = false;

            TicksAfterLastCarSpawning = 0;
            TimeBetweenCars = 0;
        }

    }
}
