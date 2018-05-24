using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Graphic;

namespace Stratis.NodeCommander.Controls.NodeOverview
{
    public class NodeOverviewDataGridViewMapper
    {
        public DataTable NodesDataTable;

        public NodeOverviewDataGridViewMapper()
        {
            NodesDataTable = new DataTable();
        }


        public void MergeDataRows(AgentConnection agentConnection, NodeNetwork managedNodes)
        {
            //Remove nodes not managed by agent
            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow row in NodesDataTable.Rows)
            {
                BlockchainNode node = (BlockchainNode)row["Node"];

                if (node.NodeConfig.Agent == agentConnection.Address && !managedNodes.Nodes.ContainsKey(node.NodeEndpoint.FullNodeName))
                    rowsToDelete.Add(row);
            }
            foreach (DataRow row in rowsToDelete) NodesDataTable.Rows.Remove(row);

            //Add and update nodes
            foreach (string nodeName in managedNodes.Nodes.Keys)
            {
                BlockchainNode node = managedNodes.Nodes[nodeName];

                var matchingNodes = from DataRow r in NodesDataTable.Rows
                    let nodeInDataTable = (BlockchainNode)r["Node"]
                    where nodeInDataTable.NodeConfig.Agent == agentConnection.Address &&
                          nodeInDataTable.NodeEndpoint.FullNodeName == node.NodeEndpoint.FullNodeName
                    select r;

                if (!matchingNodes.Any())
                {
                    object[] rowData = new object[2];

                    CreateColumnIfNotExist("Status", "", typeof(Bitmap), 16);
                    rowData[NodesDataTable.Columns.IndexOf("Status")] = StatusIconProvider.GrayCircle;

                    CreateColumnIfNotExist("Node", "Node", typeof(BlockchainNode), 130);
                    rowData[NodesDataTable.Columns.IndexOf("Node")] = node;
                    NodesDataTable.Rows.Add(rowData);
                }
            }
        }

        public void UpdateDataTable(DatabaseConnection database, NodeNetwork managedNodes)
        {
            if (managedNodes == null) return;

            foreach (string fullNodeName in managedNodes.Nodes.Keys)
            {
                foreach (DataRow dataRow in NodesDataTable.Rows)
                {
                    BlockchainNode node = managedNodes.Nodes[fullNodeName];
                    if (node.NodeState.Initialized && ((BlockchainNode)dataRow["Node"]).NodeEndpoint.FullNodeName.Equals(fullNodeName))
                    {
                        switch (node.NodeState.NodeOperationState.State)
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

                        dataRow["Node"] = node;

                        CreateColumnIfNotExist("HeaderHeight", "Header", typeof(String), 40);
                        dataRow["HeaderHeight"] = node.NodeState.NodeLogState.HeadersHeight;

                        CreateColumnIfNotExist("ConsensusHeight", "Consen", typeof(String), 35);
                        dataRow["ConsensusHeight"] = node.NodeState.NodeLogState.ConsensusHeight;

                        CreateColumnIfNotExist("BlockHeight", "Block", typeof(String), 35);
                        dataRow["BlockHeight"] = node.NodeState.NodeLogState.BlockStoreHeight;

                        CreateColumnIfNotExist("WalletHeight", "Wallet", typeof(String), 35);
                        dataRow["WalletHeight"] = node.NodeState.NodeLogState.WalletHeight;

                        CreateColumnIfNotExist("NetworkHeight", "Network", typeof(String), 35);
                        dataRow["NetworkHeight"] = node.NodeState.NodeOperationState.NetworkHeight;

                        CreateColumnIfNotExist("Mempool", "Mpool", typeof(String), 30);
                        dataRow["Mempool"] = node.NodeState.NodeOperationState.MempoolTransactionCount;

                        CreateColumnIfNotExist("Events", "Events", typeof(String), 60);
                        dataRow["Events"] = $"M: {database.GetMinedBlockCount(fullNodeName)} / R: {database.GetReorgCount(fullNodeName)}";

                        CreateColumnIfNotExist("Peers", "Peers", typeof(String), 70);
                        dataRow["Peers"] = $"In:{node.NodeState.NodeOperationState.InboundPeersCount} / Out:{node.NodeState.NodeOperationState.OutboundPeersCount}";

                        CreateColumnIfNotExist("Uptime", "Uptime", typeof(String), 50);
                        dataRow["Uptime"] = node.NodeState.NodeOperationState.Uptime.ToString("d'.'hh':'mm");

                        CreateColumnIfNotExist("Branch", "Branch", typeof(String), 100);
                        int lastComitDays = (DateTime.Now - node.GitRepositoryInfo.LatestLocalCommitDateTime).Days;
                        dataRow["Branch"] = $"{node.GitRepositoryInfo.CurrentBranchName} [{node.GitRepositoryInfo.CommitDifference}] {lastComitDays} days ago";
                    }
                }
            }
        }

        private void CreateColumnIfNotExist(string name, string headerText, Type type, int width)
        {
            if (NodesDataTable.Columns.Contains(name)) return;

            DataColumn column = new DataColumn(name, type);
            NodesDataTable.Columns.Add(column);
            column.ExtendedProperties.Add("Width", width);
            column.ExtendedProperties.Add("HeaderText", headerText);
        }
    }
}

