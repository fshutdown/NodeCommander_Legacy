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
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Graphic;

namespace Stratis.NodeCommander.Controls
{
    public class NodeOverviewDataGridView : DataGridView
    {
        private DataView dataGridViewNodesDataView;
        private DataTable nodesData;


        private DataTable BuildNodeDataTable()
        {
            nodesData = new DataTable();

            nodesData.Columns.Add("Status", typeof(Bitmap));
            nodesData.Columns.Add("Node", typeof(BlockchainNode));
            nodesData.Columns.Add("HeaderHeight");
            nodesData.Columns.Add("ConsensusHeight");
            nodesData.Columns.Add("BlockHeight");
            nodesData.Columns.Add("WalletHeight");
            nodesData.Columns.Add("NetworkHeight");
            nodesData.Columns.Add("Mempool");
            nodesData.Columns.Add("Peers");
            nodesData.Columns.Add("Uptime");
            nodesData.Columns.Add("Messages");
            nodesData.Columns.Add("Agent");

            foreach (string nodeName in managedNodes.Nodes.Keys)
            {
                BlockchainNode node = managedNodes.Nodes[nodeName];

                object[] rowData = new object[nodesData.Columns.Count];
                rowData[0] = StatusIconProvider.GrayCircle;
                rowData[1] = node;
                nodesData.Rows.Add(rowData);
            }

            return nodesData;
        }

        private void NodeDataUpdated(AgentConnection clientConnection, BlockchainNodeState[] nodesStates)
        {
            int performanceIsues = 0;
            if (this.DataSource == null)
            {
                DataTable dataTable = BuildNodeDataTable();

                dataGridViewNodesDataView = new DataView(dataTable, string.Empty, string.Empty, DataViewRowState.CurrentRows);
                dataGridViewNodes_Filter();

                //this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                this.AutoGenerateColumns = false;

                this.Columns["Status"].Width = 16;
                this.Columns["Node"].Width = 80;
                this.Columns["Status"].HeaderText = string.Empty;
                this.Columns["HeaderHeight"].Width = 50;
                this.Columns["ConsensusHeight"].Width = 50;
                this.Columns["BlockHeight"].Width = 50;
                this.Columns["WalletHeight"].Width = 50;
                this.Columns["NetworkHeight"].Width = 60;
                this.Columns["Mempool"].Width = 50;
                this.Columns["Peers"].Width = 80;
                this.Columns["Uptime"].Width = 80;
                this.Columns["Messages"].Width = 200;
                this.Columns["Agent"].Width = 80;
            }

            MergeMeasuresIntoNode(managedNodes, nodesStates);
            MergeMeasuresIntoDataTable(this, managedNodes);

            if (notifyAboutPerformanceIssuesToolStripMenuItem.Checked && performanceIsues > 0)
            {
                string message = $"Potential performance issue";
                ShowNotifier("Dashboard", message, CoinMasterForm.NotificationType.PerformanceIssue);
            }
        }

        private void MergeMeasuresIntoNode(NodeNetwork network, BlockchainNodeState[] nodesStates)
        {
            if (nodesStates == null || nodesStates.Length == 0) return;

            foreach (BlockchainNodeState nodeState in nodesStates)
            {
                if (network.Nodes.ContainsKey(nodeState.NodeEndpoint.FullNodeName))
                {
                    network.Nodes[nodeState.NodeEndpoint.FullNodeName].NodeState = nodeState;
                }
            }
        }

        private void MergeMeasuresIntoDataTable(DataGridView grid, NodeNetwork managedNodes)
        {
            if (managedNodes == null) return;

            DataView dataView = ((DataView)grid.DataSource);
            DataTable table = dataView.Table;

            foreach (string fullNodeName in managedNodes.Nodes.Keys)
            {
                foreach (DataRow dataRow in table.Rows)
                {
                    if (managedNodes.Nodes[fullNodeName].NodeState.Initialized && ((BlockchainNode)dataRow["Node"]).NodeEndpoint.FullNodeName.Equals(fullNodeName))
                    {
                        dataRow["Node"] = managedNodes.Nodes[fullNodeName];
                        switch (managedNodes.Nodes[fullNodeName].NodeState.NodeOperationState.State)
                        {
                            case ProcessState.Unknown:
                                dataRow["Status"] = StatusIconProvider.GrayCircle;
                                break;
                            case ProcessState.Stopped:
                                dataRow["Status"] = StatusIconProvider.RedCircle;
                                break;
                            case ProcessState.Running:
                                dataRow["Status"] = StatusIconProvider.GreenCircle;
                                break;
                            case ProcessState.Starting:
                                dataRow["Status"] = StatusIconProvider.GreenCircle;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        dataRow["HeaderHeight"] = managedNodes.Nodes[fullNodeName].NodeState.NodeLogState.HeadersHeight;
                        dataRow["ConsensusHeight"] = managedNodes.Nodes[fullNodeName].NodeState.NodeLogState.ConsensusHeight;
                        dataRow["BlockHeight"] = managedNodes.Nodes[fullNodeName].NodeState.NodeLogState.BlockStoreHeight;
                        dataRow["WalletHeight"] = managedNodes.Nodes[fullNodeName].NodeState.NodeLogState.WalletHeight;
                        dataRow["NetworkHeight"] = managedNodes.Nodes[fullNodeName].NodeState.NodeOperationState.NetworkHeight;
                        dataRow["Mempool"] = managedNodes.Nodes[fullNodeName].NodeState.NodeOperationState.MempoolTransactionCount;
                        dataRow["Peers"] = $"In:{managedNodes.Nodes[fullNodeName].NodeState.NodeOperationState.InboundPeersCount} / Out:{managedNodes.Nodes[fullNodeName].NodeState.NodeOperationState.OutboundPeersCount}";
                        dataRow["Uptime"] = managedNodes.Nodes[fullNodeName].NodeState.NodeOperationState.Uptime.ToString("d' days, 'hh':'mm':'ss");
                        dataRow["Messages"] = $"Mined: {clientConnectionManager.Session.Database.GetMinedBlockCount(fullNodeName)} / Reorg: {clientConnectionManager.Session.Database.GetReorgCount(fullNodeName)}";
                        dataRow["Agent"] = $"-";
                    }
                }
            }
        }



        private void dataGridViewNodes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
