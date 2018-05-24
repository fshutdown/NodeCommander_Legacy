using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Database;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Graphic;

namespace Stratis.NodeCommander.Controls.NodeExceptions
{
    public class NodeExceptionsDataGridViewMapper
    {
        public DataTable DataTable;

        public NodeExceptionsDataGridViewMapper()
        {
            DataTable = new DataTable();
        }

        public void UpdateDataRows(List<NodeLogMessage> logMessages, bool merge)
        {
            if (!merge) DataTable.Clear();

            //Add and update nodes
            foreach (NodeLogMessage logMessage in logMessages)
            {
                if (!merge)
                {
                    AddRow(logMessage);
                    continue;
                }

                var matchingNodes = from DataRow r in DataTable.Rows
                    let messageInDataTable = (NodeLogMessage)r["Message"]
                    where messageInDataTable.Id == logMessage.Id
                    select r;

                if (!matchingNodes.Any())
                {
                    AddRow(logMessage);
                }
            }
        }

        public void AddRow(NodeLogMessage logMessage)
        {
            DataRow rowData = DataTable.NewRow();

            CreateColumnIfNotExist("Timestamp", "Timestamp", typeof(DateTime), 30);
            rowData["Timestamp"] = logMessage.Timestamp;

            CreateColumnIfNotExist("Level", "Level", typeof(String), 30);
            rowData["Level"] = logMessage.Level;

            CreateColumnIfNotExist("Message", "Message", typeof(NodeLogMessage), 130);
            rowData["Message"] = logMessage;

            DataTable.Rows.Add(rowData);
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

