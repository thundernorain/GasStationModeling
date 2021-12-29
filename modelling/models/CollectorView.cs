

using GasStationModeling.modelling.models;
using System.Windows.Shapes;

namespace GasStationModeling.modelling.model
{
    public class CollectorView : TopologyView
    {
        public double TakenCashCount { get; set; }
        public bool IsMovingToCashBox { get; set; }

        public Rectangle CashBox { get; set; }

        public double CollectingCashSpeed { get; set; }

        public CollectorView(double tick, double cashCollectingSpeedSecond)
        {
            TakenCashCount = 0;
            CollectingCashSpeed = cashCollectingSpeedSecond * tick / 50;
        }

        public CashBoxView GetCashFromCashBox(ref CashBoxView cashBox)
        {
            cashBox.CurrentCashCount -= CollectingCashSpeed;
            TakenCashCount += CollectingCashSpeed;
            return cashBox;
        }
    }
}
