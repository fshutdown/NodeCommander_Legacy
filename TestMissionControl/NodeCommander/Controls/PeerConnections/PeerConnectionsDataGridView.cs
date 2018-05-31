using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.NodeObjects;
using Stratis.NodeCommander.Controls.NodeOverview;

namespace Stratis.NodeCommander.Controls.PeerConnections
{
    public class PeerConnectionsDataGridView : DataGridView
    {
        private DataView dataView;
        public event Action<DataView> Filter;
        public PeerConnectionsDataGridViewMapper Mapper;

        public PeerConnectionsDataGridView()
        {
            Mapper = new PeerConnectionsDataGridViewMapper();
            this.CellFormatting += dataGridView_CellFormatting;
        }

        public void UpdateNodes(BlockchainNode node, DatabaseConnection database)
        {
            Mapper.MergeDataRows(node.NodeState.NodeOperationState.Peers);
            Mapper.UpdateDataTable(database, node);
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

        }
    }
}
