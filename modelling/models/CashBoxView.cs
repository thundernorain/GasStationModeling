
using GasStationModeling.modelling.models;

namespace GasStationModeling.modelling.model
{
    public class CashBoxView : TopologyView
    {
        public string CurrentCashView { get; set; }

        public double CashLimit;

        public bool IsFull => CurrentCashCount >= CashLimit;

        private double _currentCash;

        public double CurrentCashCount
        {
            get => _currentCash;
            set
            {
                _currentCash = value;
                CurrentCashView = "Объём денег в кассе (руб) : " + (int)_currentCash + " \\ " + CashLimit + " руб";
            }
        }

        public CashBoxView(double cashLimit)
        {
            CashLimit = cashLimit;
            CurrentCashCount = 0;
        }
    }
}
