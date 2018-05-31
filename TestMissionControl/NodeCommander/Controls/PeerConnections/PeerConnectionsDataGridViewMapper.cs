using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.NodeObjects;
using Stratis.NodeCommander.Graphic;

namespace Stratis.NodeCommander.Controls.PeerConnections
{
    public class PeerConnectionsDataGridViewMapper
    {
        public DataTable DataTable;

        public PeerConnectionsDataGridViewMapper()
        {
            DataTable = new DataTable();
        }

        public void MergeDataRows(List<ConnectionPeer> peers)
        {
            //Remove old peers
            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow row in DataTable.Rows)
            {
                ConnectionPeer node = (ConnectionPeer)row["Address"];

                if (peers.All(p => p.RemoteSocketEndpoint != node.RemoteSocketEndpoint))
                    rowsToDelete.Add(row);
            }
            foreach (DataRow row in rowsToDelete) DataTable.Rows.Remove(row);

            //Add and update nodes
            foreach (ConnectionPeer connectionPeer in peers)
            {
                var matchingNodes = from DataRow r in DataTable.Rows
                    let nodeInDataTable = (ConnectionPeer)r["Address"]
                    where nodeInDataTable.RemoteSocketEndpoint == connectionPeer.RemoteSocketEndpoint
                    select r;

                if (!matchingNodes.Any())
                {
                    object[] rowData = new object[2];

                    CreateColumnIfNotExist("Type", "Type", typeof(String), 16);
                    rowData[DataTable.Columns.IndexOf("Type")] = connectionPeer.PeerType;

                    CreateColumnIfNotExist("Address", "Address", typeof(ConnectionPeer), 130);
                    rowData[DataTable.Columns.IndexOf("Address")] = connectionPeer;
                    DataTable.Rows.Add(rowData);
                }
            }
        }

        public void UpdateDataTable(DatabaseConnection database, BlockchainNode node)
        {
            List<ConnectionPeer> peers = node.NodeState.NodeOperationState.Peers;

            if (peers == null) return;

            if (!int.TryParse(node.NodeState.NodeLogState.HeadersHeight, out int nodeHight)) nodeHight = 0;
            int maxTip = Math.Max(peers.Max(p => p.TipHeight), nodeHight);

            foreach (ConnectionPeer connectionPeer in peers)
            {
                foreach (DataRow dataRow in DataTable.Rows)
                {
                    if (((ConnectionPeer)dataRow["Address"]).RemoteSocketEndpoint == connectionPeer.RemoteSocketEndpoint)
                    {
                        dataRow[DataTable.Columns.IndexOf("Type")] = connectionPeer.PeerType;
                        dataRow["Address"] = connectionPeer;

                        CreateColumnIfNotExist("TipHeight", "TipHeight", typeof(string), 50);
                        dataRow["TipHeight"] = $"{connectionPeer.TipHeight} ({connectionPeer.TipHeight - maxTip})";

                        CreateColumnIfNotExist("Version", "Agent", typeof(string), 80);
                        dataRow["Version"] = connectionPeer.Version;
                    }
                }
            }
        }

        private void CreateColumnIfNotExist(string name, string headerText, Type type, int width)
        {
            if (DataTable.Columns.Contains(name)) return;

            DataColumn column = new DataColumn(name, type);
            DataTable.Columns.Add(column);
            column.ExtendedProperties.Add("Width", width);
            column.ExtendedProperties.Add("HeaderText", headerText);
        }
    }
}

