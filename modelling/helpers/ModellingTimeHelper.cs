using System.Windows.Threading;

namespace GasStationModeling.modelling.managers
{
    public class ModellingTimeHelper
    {
        public const int TIMER_TICK_MILLISECONDS = 20;

        public int TimerTicksCount { get; set; }
        public bool IsPaused { get; set; }
        public int TicksAfterLastCarSpawning { get; set; }
        public double TimeAfterLastCarSpawningInSeconds => TicksAfterLastCarSpawning * (double)TIMER_TICK_MILLISECONDS / 1000;
        public double TimeBetweenCars { get; set; }

        public ModellingTimeHelper(DispatcherTimer timer) {
            TimerTicksCount = 0;
            IsPaused = false;

            TicksAfterLastCarSpawning = 0;
            TimeBetweenCars = 0;
        }
    }
}
 