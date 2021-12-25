using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.modelling.model
{
    class CollectorView
    {

        public double TakenCashCount { get; set; }
        public bool IsMovingToCashBox { get; set; }

        public double CollectingCashSpeed { get; set; }

        public CollectorView(double tick, double cashCollectingSpeedSecond)
        {
            TakenCashCount = 0;
            IsMovingToCashBox = true;
            CollectingCashSpeed = cashCollectingSpeedSecond * tick / 1000;
        }

        public CashBoxView GetCashFromCashBox(CashBoxView cashBox)
        {
            cashBox.CurrentCashCount -= CollectingCashSpeed;
            TakenCashCount += CollectingCashSpeed;
            return cashBox;
        }
    }
}
