using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.NodeCommander.Controls.NodeExceptions;

namespace Stratis.NodeCommander.Controls.Agents
{
    public class AgentsDataGridView : DataGridView
    {
        private DataView dataView;
        public event Action<DataView> Filter;
        public AgentsDataGridViewMapper Mapper;

        public AgentsDataGridView()
        {
            Mapper = new AgentsDataGridViewMapper();
            this.CellFormatting += dataGridView_CellFormatting;
        }

        public void UpdateNodes(AgentConnection agentConnection, AgentHealthState state, string message)
        {
            Mapper.MergeDataRows(agentConnection, state, message);
            Mapper.UpdateDataTable(agentConnection, state, message);
            if (this.DataSource == null) ConfigureDataGridView();

            if (SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = SelectedRows[0];
                DataGridViewRowStateChangedEventArgs e = new DataGridViewRowStateChangedEventArgs(selectedRow, DataGridViewElementStates.None);
                this.OnRowStateChanged(selectedRow.Index, e);
            }
        }

        private void ConfigureDataGridView()
        {
            if (Mapper.DataTable.Columns.Count == 0) return;
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
