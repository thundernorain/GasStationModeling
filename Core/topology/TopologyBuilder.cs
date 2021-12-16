using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GasStationModeling.core.topology
{
    public partial class TopologyBuilder
    {
        private DataGrid field;
        private int serviceAreaInCells;
        private int serviceAreaBorderColIndex;

        private int rowCount;
        private int columnCount;

        public TopologyBuilder(DataGrid dg, int serviceAreaBorderColIndex)
        {
            field = dg ?? throw new NullReferenceException();

            //Image a = field.Columns[0].GetCellContent(field.Items[0]) as Image;

            rowCount = field.Items.Count; 
            columnCount = field.Columns.Count;
            this.serviceAreaBorderColIndex = serviceAreaBorderColIndex;
            DataGridColumn dbc = new DataGridTemplateColumn();

            SetupDgv();
            SetupServiceArea();
            field.Columns.Add(dbc);

            // Get the no. of columns in the first row.

            /*AddDgvCols(Topology.MinColsCount);
            field.RowCount = Topology.MinRowsCount;

            SetupDgv();
            serviceAreaInCells = RecalculateServiceArea();
            SetupServiceArea();
            SetupRoad();*/
        }

        private void SetupDgv()
        {
            field.HeadersVisibility = DataGridHeadersVisibility.None;
            field.CanUserResizeColumns = false;
            field.CanUserResizeRows = false;

            field.AutoGenerateColumns = false;
        }

        private void SetupServiceArea()
        {
            int lastRowIndex = rowCount - 1;

            Image cell;

            for (int currCol = columnCount - 1; currCol >= serviceAreaBorderColIndex; currCol--)
            {
                for (int currRow = 0; currRow < lastRowIndex; currRow++)
                {
                    cell = field.Columns[currCol].GetCellContent(field.Items[currRow]) as Image;
                    cell.Tag = "Служебная часть";
                    //cell.Source = "/";
                }
            }
        }

    }
}
