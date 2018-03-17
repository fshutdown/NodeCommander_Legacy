using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Stratis.CoinmasterClient;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.TestMissionControl.Agents;
using Timer = System.Timers.Timer;


namespace Stratis.TestMissionControl
{
    public partial class CoinMasterForm : Form
    {
        private enum NotificationType
        {
            PerformanceIssue,
            Exception
        }

        private NodeNetwork network;
        private DataView dataGridViewNodesDataView;
        private DataView dataGridViewAgentsDataView;
        private Tulpep.NotificationWindow.PopupNotifier notifier;
        private AgentConnectionManager agentConnectionManager;

        public CoinMasterForm()
        {
            InitializeComponent();

            ConfigReader reader = new ConfigReader();
            network = reader.Config;

            //Create pop-up
            notifier = new Tulpep.NotificationWindow.PopupNotifier();
            notifier.Delay = 10000;
            notifier.Click += (sender, eventArgs) =>
            {
                Visible = true;
                BringToFront();
            };

            agentConnectionManager = new AgentConnectionManager();
            agentConnectionManager.CreateListOfAgents(network);
            agentConnectionManager.ConnectionStatusChanged += connectionAddress => Invoke(new Action<string>(AgentDataTableUpdated), connectionAddress);
            agentConnectionManager.MessageReceived += (agentConnection, networkSegment) => Invoke(new Action<object, NodeNetwork>(NodePerformanceUpdated), agentConnection, networkSegment);

            agentConnectionManager.ConnectToAgents();

        }

        private DataTable BuildNodeDataTable()
        {
            DataTable nodesData = new DataTable();
            nodesData.Columns.Add("Node", typeof(SingleNode));
            nodesData.Columns.Add(MeasureType.Status.ToString());
            nodesData.Columns.Add(MeasureType.CPU.ToString());
            nodesData.Columns.Add(MeasureType.Memory.ToString());
            nodesData.Columns.Add(MeasureType.BlockHeight.ToString());
            nodesData.Columns.Add(MeasureType.ExceptionCount.ToString());

            foreach (string nodeName in network.NetworkNodes.Keys)
            {
                SingleNode node = network.NetworkNodes[nodeName];

                nodesData.Rows.Add(node, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }

            return nodesData;
        }

        private DataTable BuildAgentDataTable()
        {
            DataTable agentsData = new DataTable();
            agentsData.Columns.Add("Address", typeof(AgentConnection));
            agentsData.Columns.Add("Status");
            agentsData.Columns.Add("Info");
            
            foreach (AgentConnection connection in agentConnectionManager.AgentConnectionList.Values)
            {
                agentsData.Rows.Add(connection, string.Empty, string.Empty);
            }
            
            return agentsData;
        }

        private void AgentDataTableUpdated(string address)
        {
            DataTable dataTable;
            if (dataGridViewAgents.DataSource == null)
            {
                dataTable = BuildAgentDataTable();

                dataGridViewAgentsDataView = new DataView(dataTable, string.Empty, string.Empty, DataViewRowState.CurrentRows);
                dataGridViewAgents_Filter();

                dataGridViewAgents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewAgents.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridViewAgents.AutoGenerateColumns = false;

                dataGridViewAgents.Columns["Address"].Width = 10;
                dataGridViewAgents.Columns["Status"].Width = 10;
                dataGridViewAgents.Columns["Info"].Width = 60;
            }
            else
            {
                dataTable = dataGridViewAgentsDataView.Table;
            }

            AgentConnection connection = agentConnectionManager.AgentConnectionList[address];
            foreach (DataRow row in dataTable.Rows)
            {
                AgentConnection tableRecord = (AgentConnection) row["Address"];

                if (tableRecord.Address == address)
                {
                    row["Status"] = connection.State;
                    row["Info"] = connection.ConnectionInfo;

                    break;
                }
            }
        }

        private void NodePerformanceUpdated(object source, NodeNetwork networkSegment)
        {
            int performanceIsues = 0;
            if (dataGridViewNodes.DataSource == null)
            {
                DataTable dataTable = BuildNodeDataTable();

                dataGridViewNodesDataView = new DataView(dataTable, string.Empty, string.Empty, DataViewRowState.CurrentRows);
                dataGridViewNodes_Filter();

                dataGridViewNodes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewNodes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridViewNodes.AutoGenerateColumns = false;

                foreach (DataGridViewColumn column in dataGridViewNodes.Columns)
                    column.Width = 1;

                dataGridViewNodes.Columns["Node"].Width = 60;
                dataGridViewNodes.Columns[MeasureType.Status.ToString()].Width = 45;
                dataGridViewNodes.Columns[MeasureType.CPU.ToString()].Width = 45;
                dataGridViewNodes.Columns[MeasureType.Memory.ToString()].Width = 45;
                dataGridViewNodes.Columns[MeasureType.BlockHeight.ToString()].Width = 45;
                dataGridViewNodes.Columns[MeasureType.ExceptionCount.ToString()].Width = 45;

                foreach (DataGridViewColumn column in dataGridViewNodes.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Width == 1))
                    column.Visible = false;
            }

            MergeMeasuresIntoNode(network, networkSegment);
            MergeMeasuresIntoDataTable(dataGridViewNodes, network);
            //newUploadCount = ((DataView)dataGridViewUploads.DataSource).Table.MergeDataSource(args.Data, "Workflow Id");

            if (notifyAboutPerformanceIssuesToolStripMenuItem.Checked && performanceIsues > 0)
            {
                string message = $"Potential performance issue";
                ShowNotifier("Dashboard", message, NotificationType.PerformanceIssue);
            }
        }

        private void MergeMeasuresIntoNode(NodeNetwork network, NodeNetwork networkSegment)
        {
            if (networkSegment == null) return;

            foreach (string nodeName in network.NetworkNodes.Keys)
            {
                if (networkSegment.NetworkNodes.ContainsKey(nodeName))
                {
                    foreach (MeasureType measureType in networkSegment.NetworkNodes[nodeName].NodeMeasures.Keys)
                    {
                        network.NetworkNodes[nodeName].NodeMeasures[measureType] = networkSegment.NetworkNodes[nodeName].NodeMeasures[measureType];
                    }
                }
            }
        }

        private void MergeMeasuresIntoDataTable(DataGridView grid, NodeNetwork network)
        {
            if (network == null) return;

            DataView dataView = ((DataView)grid.DataSource);
            DataTable table = dataView.Table;

            foreach (string nodeName in network.NetworkNodes.Keys)
            {
                foreach (DataRow dataRow in table.Rows)
                {
                    if (((SingleNode)dataRow["Node"]).NodeName.Equals(nodeName))
                    {
                        dataRow["Node"] = network.NetworkNodes[nodeName];
                        foreach (MeasureType measureType in network.NetworkNodes[nodeName].NodeMeasures.Keys)
                        {
                            dataRow[measureType.ToString()] = network.NetworkNodes[nodeName].NodeMeasures[measureType];
                        }
                    }
                }
            }
        }

        private void dataGridViewNodes_Filter()
        {
            List<string> filterCriteria = new List<string>();

            if (checkBoxRunningNodesOnly.Checked) filterCriteria.Add($"[{MeasureType.Status}] = 'Running'");

            dataGridViewNodesDataView.RowFilter = string.Join(" AND ", filterCriteria);
            dataGridViewNodes.DataSource = dataGridViewNodesDataView;
        }
        private void dataGridViewAgents_Filter()
        {
            dataGridViewAgents.DataSource = dataGridViewAgentsDataView;
        }


        private async void dataGridViewNodes_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            if (dataGridViewNodes.SelectedRows.Count == 0) return;


            SingleNode node = (SingleNode) dataGridViewNodes.SelectedRows[0].Cells["Node"].Value;

            textBoxNodeName.Text = node.DisplayName;
            textBoxUptime.Text = node.DataDir;
           
            dataGridViewNodeExceptions.DataSource = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void ShowNotifier(string title, string message, NotificationType type)
        {
            notifier.TitleText = title;
            notifier.ContentText = message;

            switch (type)
            {
                case NotificationType.PerformanceIssue:
                    notifier.BodyColor = Color.FromArgb(255, 215, 228, 188);
                    //notifier.Image = Resources.Browser_aol;
                    notifier.ImagePadding = new Padding(5);
                    break;
                case NotificationType.Exception:
                    notifier.BodyColor = Color.FromArgb(255, 230, 185, 184);
                    //notifier.Image = Resources.Bug;
                    notifier.ImagePadding = new Padding(5);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            notifier.Popup();
        }
    }
}
