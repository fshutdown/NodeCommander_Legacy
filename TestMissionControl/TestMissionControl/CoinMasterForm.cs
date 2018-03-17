using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using CoinmasterClient;
using CoinmasterClient.Config;
using CoinmasterClient.Network;
using CoinMasterAgent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestMissionControl.Agents;
using TestMissionControl.Workers;
using TestMissionControl.Workers.DataStreams;
using Timer = System.Timers.Timer;


namespace TestMissionControl
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

        private List<BaseWorker> _workers = new List<BaseWorker>();
        private NodePerformanceWorker nodePerformanceWorker;
        private Dictionary<string, AgentConnection> agentConnectionList;

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

            //Create workers
            nodePerformanceWorker = new NodePerformanceWorker(5000);
            nodePerformanceWorker.StateChange += DashboardWorkerStateChanged;
            //nodePerformanceWorker.DataUpdate += (source, args) => InvokeWrapper(NodePerformanceUpdated, source, args);
            _workers.Add(nodePerformanceWorker);

            //Start all workers
            foreach (BaseWorker worker in _workers) worker.Start();

            CreateListOfAgents();
            ConnectToAgents();

        }

        private void CreateListOfAgents()
        {
            agentConnectionList = new Dictionary<string, AgentConnection>();

            foreach (SingleNode node in network.NetworkNodes.Values)
            {
                string[] addressParts = node.Agent.Split(':');
                AgentConnection newAgent = AgentConnection.Create(addressParts[0], addressParts[1]);

                if (!agentConnectionList.ContainsKey(node.Agent))
                    agentConnectionList.Add(newAgent.Address, newAgent);
            }
        }

        private void ConnectToAgents()
        {
            foreach (AgentConnection connection in agentConnectionList.Values)
            {
                connection.OnConnect(agentConnection =>
                {
                    agentConnection.State = AgentState.Connected;
                    Invoke(new Action<string>(AgentDataTableUpdated), connection.Address);

                    List<SingleNode> nodeList = (from n in network.NetworkNodes.Values
                        where n.Agent == connection.Address
                        select n).ToList();

                    agentConnection.SendMessage(JsonConvert.SerializeObject(nodeList));
                });
                connection.OnDisconnect(agentConnection =>
                {
                    agentConnection.State = AgentState.Disconnected;
                    Invoke(new Action<string>(AgentDataTableUpdated), connection.Address);
                });
                connection.OnMessage((s, agentConnection) =>
                {
                    AnalysisPackage package = Newtonsoft.Json.JsonConvert.DeserializeObject<AnalysisPackage>(s);
                    DataUpdateEventArgs eventArgs = new DataUpdateEventArgs();
                    eventArgs.Data = package;
                    
                    Invoke(new Action<object, DataUpdateEventArgs>(NodePerformanceUpdated), agentConnection, eventArgs);
                    Invoke(new Action<string>(AgentDataTableUpdated), connection.Address);
                });

                Timer reconnectionTimer = new Timer
                {
                    AutoReset = true,
                    Interval = 3000
                };
                reconnectionTimer.Elapsed += (sender, args) =>
                {
                    if (connection.State == AgentState.Disconnected || connection.State == AgentState.Error)
                    {
                        connection.Connect();
                        connection.State = AgentState.Connecting;
                        Invoke(new Action<string>(AgentDataTableUpdated), connection.Address);
                    }
                };
                reconnectionTimer.Start();
            }
        }



        private void DashboardWorkerStateChanged(object source, StateChangeHandlerEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, StateChangeHandlerEventArgs>(DashboardWorkerStateChanged), source, args);
                return;
            }

            toolStripStatusLabelWorker.Text = args.State.ToString();
            switch (args.State)
            {
                case WorkerState.Idle:
                    toolStripStatusLabelWorker.BackColor = DefaultBackColor;
                    toolStripStatusLabelWorker.ForeColor = Color.Black;
                    break;
                case WorkerState.Stopped:
                    toolStripStatusLabelWorker.BackColor = Color.FromArgb(255, 255, 20, 60);
                    toolStripStatusLabelWorker.ForeColor = Color.White;
                    break;
                case WorkerState.Running:
                    toolStripStatusLabelWorker.BackColor = Color.FromArgb(255, 20, 128, 60);
                    toolStripStatusLabelWorker.ForeColor = Color.White;
                    break;
            }

            Refresh();
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

            foreach (AgentConnection connection in agentConnectionList.Values)
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

            AgentConnection connection = agentConnectionList[address];
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

        private void NodePerformanceUpdated(object source, DataUpdateEventArgs args)
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

            MergeMeasuresIntoNode(network, args.Data);
            MergeMeasuresIntoDataTable(dataGridViewNodes, network);
            //newUploadCount = ((DataView)dataGridViewUploads.DataSource).Table.MergeDataSource(args.Data, "Workflow Id");

            if (notifyAboutPerformanceIssuesToolStripMenuItem.Checked && performanceIsues > 0)
            {
                string message = $"Potential performance issue";
                ShowNotifier("Dashboard", message, NotificationType.PerformanceIssue);
            }
        }

        private void MergeMeasuresIntoNode(NodeNetwork network, AnalysisPackage package)
        {
            if (package == null) return;

            foreach (string nodeName in network.NetworkNodes.Keys)
            {
                if (package.NodeMeasures.ContainsKey(nodeName))
                {
                    foreach (MeasureType measureType in package.NodeMeasures[nodeName].Keys)
                    {
                        network.NetworkNodes[nodeName].NodeMeasures[measureType] = package.NodeMeasures[nodeName][measureType];
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
