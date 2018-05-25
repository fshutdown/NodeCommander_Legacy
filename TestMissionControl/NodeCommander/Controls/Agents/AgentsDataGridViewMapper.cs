using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Graphic;

namespace Stratis.NodeCommander.Controls.Agents
{
    public class AgentsDataGridViewMapper
    {
        public DataTable DataTable;

        public AgentsDataGridViewMapper()
        {
            DataTable = new DataTable();
        }


        public void MergeDataRows(AgentConnection agentConnection, AgentHealthState state, string message)
        {
            if (agentConnection == null || state == null) return;

            //Add and update nodes
            var matchingNodes = from DataRow r in DataTable.Rows
                                let nodeInDataTable = (AgentConnection)r["Agent"]
                                where nodeInDataTable.Address == agentConnection.Address
                                select r;

            if (!matchingNodes.Any())
            {
                object[] rowData = new object[2];

                CreateColumnIfNotExist("Status", "", typeof(Bitmap), 16);
                rowData[DataTable.Columns.IndexOf("Status")] = StatusIconProvider.GrayCircle;

                CreateColumnIfNotExist("Agent", "Agent", typeof(AgentConnection), 60);
                rowData[DataTable.Columns.IndexOf("Agent")] = agentConnection;

                DataTable.Rows.Add(rowData);
            }
        }

        public void UpdateDataTable(AgentConnection agentConnection, AgentHealthState state, string message)
        {
            if (agentConnection == null || state == null) return;

            foreach (DataRow dataRow in DataTable.Rows)
            {
                if (((AgentConnection) dataRow["Agent"]).Address.Equals(agentConnection.Address))
                {
                    switch (agentConnection.State)
                    {
                        case WebSocketState.None:
                            dataRow["Status"] = StatusIconProvider.GrayCircle;
                            break;
                        case WebSocketState.Connecting:
                            dataRow["Status"] = StatusIconProvider.OrangeCircle;
                            break;
                        case WebSocketState.Open:
                            dataRow["Status"] = StatusIconProvider.GreenCircle;
                            break;
                        case WebSocketState.CloseSent:
                            dataRow["Status"] = StatusIconProvider.OrangeCircle;
                            break;
                        case WebSocketState.CloseReceived:
                            dataRow["Status"] = StatusIconProvider.OrangeCircle;
                            break;
                        case WebSocketState.Closed:
                            dataRow["Status"] = StatusIconProvider.GrayCircle;
                            break;
                        case WebSocketState.Aborted:
                            dataRow["Status"] = StatusIconProvider.RedCircle;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    dataRow["Agent"] = agentConnection;

                    CreateColumnIfNotExist("LastUpdated", "Updated", typeof(AgentHealthState), 40);
                    dataRow["LastUpdated"] = state;

                    CreateColumnIfNotExist("Threads", "Threads", typeof(int), 40);
                    dataRow["Threads"] = state.ThreadCount;

                    CreateColumnIfNotExist("Memory", "Memory", typeof(string), 40);
                    dataRow["Memory"] = state.MemoryUsageMb;

                    CreateColumnIfNotExist("Clients", "Clients", typeof(int), 40);
                    dataRow["Clients"] = state.ClientCount;
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

