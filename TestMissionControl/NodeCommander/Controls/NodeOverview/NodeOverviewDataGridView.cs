using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Graphic;

namespace Stratis.NodeCommander.Controls.NodeOverview
{
    public class NodeOverviewDataGridView : DataGridView
    {
        private DataView dataView;
        public event Action<DataView> dataGridViewNodes_Filter;
        public NodeOverviewDataGridViewMapper Mapper;

        public NodeOverviewDataGridView()
        {
            Mapper = new NodeOverviewDataGridViewMapper();
            this.CellFormatting += dataGridView_CellFormatting;
        }

        public void UpdateNodes(AgentConnection agentConnection, NodeNetwork managedNodes, DatabaseConnection database)
        {
            Mapper.MergeDataRows(agentConnection, managedNodes);
            Mapper.UpdateDataTable(database, managedNodes);
            if (this.DataSource == null) ConfigureDataGridView();

        }

        private void ConfigureDataGridView()
        {
            dataView = new DataView(Mapper.NodesDataTable, string.Empty, string.Empty, DataViewRowState.CurrentRows);
            dataGridViewNodes_Filter?.Invoke(dataView);

            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.AutoGenerateColumns = false;

            foreach (DataColumn column in Mapper.NodesDataTable.Columns)
            {
                this.Columns[column.ColumnName].Width = (int)column.ExtendedProperties["Width"];
                this.Columns[column.ColumnName].HeaderText = (string)column.ExtendedProperties["HeaderText"];
                this.Columns[column.ColumnName].ToolTipText = (string)column.ExtendedProperties["HeaderText"];
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            BlockchainNode node = (BlockchainNode)this.Rows[e.RowIndex].Cells["Node"].Value;
            if (!node.NodeState.Initialized) return;

            int maxHeight;
            int headersHeight;
            if (!int.TryParse(node.NodeState.NodeLogState.HeadersHeight, out headersHeight)) headersHeight = 0;

            int consensusHeight;
            if (!int.TryParse(node.NodeState.NodeLogState.ConsensusHeight, out consensusHeight)) consensusHeight = 0;

            int blockstoreHeight;
            if (!int.TryParse(node.NodeState.NodeLogState.BlockStoreHeight, out blockstoreHeight)) blockstoreHeight = 0;

            int walletHeight;
            if (!int.TryParse(node.NodeState.NodeLogState.WalletHeight, out walletHeight)) walletHeight = -1;

            int networkHeight = node.NodeState.NodeOperationState.NetworkHeight;

            maxHeight = Math.Max(headersHeight, consensusHeight);
            maxHeight = Math.Max(maxHeight, blockstoreHeight);
            maxHeight = Math.Max(maxHeight, walletHeight);
            maxHeight = Math.Max(maxHeight, networkHeight);

            if (walletHeight == -1) walletHeight = maxHeight;


            if (e.ColumnIndex == 2 && headersHeight < maxHeight)
            {
                e.CellStyle.ForeColor = Color.Crimson;
            }
            else if (e.ColumnIndex == 3 && consensusHeight < maxHeight)
            {
                e.CellStyle.ForeColor = Color.Crimson;
            }
            else if (e.ColumnIndex == 4 && blockstoreHeight < maxHeight)
            {
                e.CellStyle.ForeColor = Color.Crimson;
            }
            else if (e.ColumnIndex == 5 && walletHeight < maxHeight)
            {
                e.CellStyle.ForeColor = Color.Crimson;
            }
            else if (e.ColumnIndex == 6 && networkHeight < maxHeight)
            {
                e.CellStyle.ForeColor = Color.Crimson;
            }
            else
            {
                e.CellStyle.ForeColor = Color.Black;
            }
        }
    }
}
