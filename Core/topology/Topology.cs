using GasStationModeling.core.models;
using System;
using System.Drawing;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.core.topology
{
    public partial class Topology
    {
        private readonly IGasStationElement[,] field;
        private readonly int serviceAreaBorderColIndex;

        public Topology(IGasStationElement[,] field, int serviceAreaBorderColIndex)
        {
            this.field = field ?? throw new NullReferenceException();
            this.serviceAreaBorderColIndex = serviceAreaBorderColIndex;
        }

        public IGasStationElement this[int x, int y]
        {
            get
            {
                if (x < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                if (x > LastX)
                {
                    throw new IndexOutOfRangeException();
                }

                if (y < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                if (y > LastY)
                {
                    throw new IndexOutOfRangeException();
                }

                return field[y, x];
            }
        }
        public int ServiceAreaBorderColIndex { get; }

        public int ColsCount
        {
            get
            {
                return field.GetLength(1);
            }
        }

        public int RowsCount
        {
            get
            {
                return field.GetLength(0);
            }
        }

        public int LastX
        {
            get
            {
                return ColsCount - 1;
            }
        }

        public int LastY
        {
            get
            {
                return RowsCount - 1;
            }
        }

        public IGasStationElement GetElement(int x, int y)
        {
            return field[y, x];
        }

        public bool IsCashbox(int x, int y)
        {
            return this[x, y] is Cashbox;
        }


        public bool IsFuelDispenser(int x, int y)
        {
            return this[x, y] is FuelDispenser;
        }


        public bool IsTank(int x, int y)
        {
            return this[x, y] is Tank;
        }

    }
}
