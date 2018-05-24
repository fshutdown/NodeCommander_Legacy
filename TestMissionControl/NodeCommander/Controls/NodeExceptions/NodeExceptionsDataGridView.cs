using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Controls.NodeOverview;

namespace Stratis.NodeCommander.Controls.NodeExceptions
{
    public class NodeExceptionsDataGridView : DataGridView
    {
        private DataView dataView;
        public event Action<DataView> Filter;
        public NodeExceptionsDataGridViewMapper Mapper;

        public NodeExceptionsDataGridView()
        {
            Mapper = new NodeExceptionsDataGridViewMapper();
            this.CellFormatting += dataGridView_CellFormatting;
            this.DataError += foo;
        }

        private void foo(object sender, DataGridViewDataErrorEventArgs e)
        {
            int r = 9;
        }


        public void UpdateNodes(List<NodeLogMessage> logMessages, bool merge)
        {
            Mapper.UpdateDataRows(logMessages, merge);
            if (this.DataSource == null) ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dataView = new DataView(Mapper.DataTable, string.Empty, string.Empty, DataViewRowState.CurrentRows);
            Filter?.Invoke(dataView);

            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.AutoGenerateColumns = false;

            foreach (DataColumn column in Mapper.DataTable.Columns)
            {
                this.Columns[column.ColumnName].Width = (int)column.ExtendedProperties["Width"];
                this.Columns[column.ColumnName].HeaderText = (string)column.ExtendedProperties["HeaderText"];
                this.Columns[column.ColumnName].ToolTipText = (string)column.ExtendedProperties["HeaderText"];
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //NodeLogMessage message = (NodeLogMessage)this.Rows[e.RowIndex].Cells["Message"].Value;
            
        }
    }
}
