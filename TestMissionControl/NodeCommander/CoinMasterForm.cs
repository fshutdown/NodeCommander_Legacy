using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Network;
using Stratis.NodeCommander.Client;
using Stratis.NodeCommander.Workers;
using Stratis.NodeCommander.Workers.DataStreams;


namespace Stratis.NodeCommander
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
        private DataView dataGridViewBlockchainDataView;
        private Tulpep.NotificationWindow.PopupNotifier notifier;
        private AgentConnectionManager agentConnectionManager;

        private List<BaseWorker> _workers = new List<BaseWorker>();
        private CryptoIdWorker cryptoIdWorker;

        public CoinMasterForm()
        {
            InitializeComponent();

            ConfigReader reader = new ConfigReader();
            network = reader.Config;

            //ToDo: Switch networks
            //...

            //Create pop-up
            notifier = new Tulpep.NotificationWindow.PopupNotifier();
            notifier.Delay = 10000;
            notifier.Click += (sender, eventArgs) =>
            {
                Visible = true;
                BringToFront();
            };

            //Create workers
            cryptoIdWorker = new CryptoIdWorker(10000, new NodeEndpointName("Stratis", "StratisTest"));
            cryptoIdWorker.StateChange += DashboardWorkerStateChanged;
            cryptoIdWorker.DataUpdate += (source, args) => Invoke(new Action<object, CryptoIdDataUpdateEventArgs>(CryptoIdUpdated), source, args);
            _workers.Add(cryptoIdWorker);

            //Start all workers
            foreach (BaseWorker worker in _workers) worker.Start();

            agentConnectionManager = new AgentConnectionManager();
            agentConnectionManager.CreateListOfAgents(network);
            agentConnectionManager.ConnectionStatusChanged += connectionAddress => Invoke(new Action<string>(AgentDataTableUpdated), connectionAddress);
            agentConnectionManager.MessageReceived += (agentConnection, networkSegment) => Invoke(new Action<object, NodeNetwork>(NodePerformanceUpdated), agentConnection, networkSegment);

            agentConnectionManager.ConnectToAgents();
        }

        private void CryptoIdUpdated(object source, CryptoIdDataUpdateEventArgs arg1)
        {
            DataTable dataTable;
            if (dataGridViewBlockchain.DataSource == null)
            {
                dataTable = BuildBlockchainDataTable();

                dataGridViewBlockchainDataView = new DataView(dataTable, string.Empty, string.Empty, DataViewRowState.CurrentRows);
                dataGridViewBlockchain_Filter();

                dataGridViewBlockchain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewBlockchain.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridViewBlockchain.AutoGenerateColumns = false;

                dataGridViewBlockchain.Columns["Measure"].Width = 10;
                dataGridViewBlockchain.Columns["Value"].Width = 10;
                dataGridViewBlockchain.Columns["Additional"].Width = 60;
            }
            else
            {
                dataTable = dataGridViewBlockchainDataView.Table;
            }

            foreach (PropertyInfo property in arg1.GetType().GetProperties())
            {
                bool found = false;
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["Measure"].Equals(property.Name))
                    {
                        row["Value"] = property.GetValue(arg1).ToString();

                        found = true;
                        break;
                    }
                }

                if (!found) dataTable.Rows.Add(property.Name, property.GetValue(arg1).ToString());
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

            //Refresh();
        }

        private DataTable BuildNodeDataTable()
        {
            DataTable nodesData = new DataTable();
            nodesData.Columns.Add("Node", typeof(SingleNode));
            nodesData.Columns.Add("Status");
            nodesData.Columns.Add("HeaderHeight");
            nodesData.Columns.Add("ConsensusHeight");
            nodesData.Columns.Add("BlockHeight");
            nodesData.Columns.Add("WalletHeight");
            nodesData.Columns.Add("NetworkHeight");
            nodesData.Columns.Add("Mempool");
            nodesData.Columns.Add("Peers");
            nodesData.Columns.Add("Uptime");
            nodesData.Columns.Add("Messages");

            foreach (string nodeName in network.NetworkNodes.Keys)
            {
                SingleNode node = network.NetworkNodes[nodeName];

                object[] rowData = new object[nodesData.Columns.Count];
                rowData[0] = node;
                nodesData.Rows.Add(rowData);
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

        private DataTable BuildBlockchainDataTable()
        {
            DataTable blockchainData = new DataTable();
            blockchainData.Columns.Add("Measure");
            blockchainData.Columns.Add("Value");
            blockchainData.Columns.Add("Additional");
            
            return blockchainData;
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

                dataGridViewNodes.Columns["Node"].Width = 80;
                dataGridViewNodes.Columns["Status"].Width = 80;
                dataGridViewNodes.Columns["HeaderHeight"].Width = 60;
                dataGridViewNodes.Columns["ConsensusHeight"].Width = 60;
                dataGridViewNodes.Columns["BlockHeight"].Width = 60;
                dataGridViewNodes.Columns["WalletHeight"].Width = 60;
                dataGridViewNodes.Columns["NetworkHeight"].Width = 60;
                dataGridViewNodes.Columns["Mempool"].Width = 60;
                dataGridViewNodes.Columns["Peers"].Width = 80;
                dataGridViewNodes.Columns["Uptime"].Width = 80;
                dataGridViewNodes.Columns["Messages"].Width = 80;
            }

            MergeMeasuresIntoNode(network, networkSegment);
            MergeMeasuresIntoDataTable(dataGridViewNodes, network);

            if (notifyAboutPerformanceIssuesToolStripMenuItem.Checked && performanceIsues > 0)
            {
                string message = $"Potential performance issue";
                ShowNotifier("Dashboard", message, NotificationType.PerformanceIssue);
            }
        }

        private void MergeMeasuresIntoNode(NodeNetwork network, NodeNetwork networkSegment)
        {
            if (networkSegment == null) return;
            network.AgentHealthState = networkSegment.AgentHealthState;

            foreach (string nodeName in network.NetworkNodes.Keys)
            {
                if (networkSegment.NetworkNodes.ContainsKey(nodeName))
                {
                    network.NetworkNodes[nodeName].NodeDeploymentState = networkSegment.NetworkNodes[nodeName].NodeDeploymentState;
                    network.NetworkNodes[nodeName].NodeProcessState = networkSegment.NetworkNodes[nodeName].NodeProcessState;
                    network.NetworkNodes[nodeName].NodeLogState = networkSegment.NetworkNodes[nodeName].NodeLogState;
                    network.NetworkNodes[nodeName].NodeOperationState = networkSegment.NetworkNodes[nodeName].NodeOperationState;
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
                    if (((SingleNode)dataRow["Node"]).NodeEndpoint.FullNodeName.Equals(nodeName))
                    {
                        dataRow["Node"] = network.NetworkNodes[nodeName];
                        dataRow["Status"] = network.NetworkNodes[nodeName].NodeOperationState.State;
                        dataRow["HeaderHeight"] = network.NetworkNodes[nodeName].NodeLogState.HeadersHeight;
                        dataRow["ConsensusHeight"] = network.NetworkNodes[nodeName].NodeLogState.ConsensusHeight;
                        dataRow["BlockHeight"] = network.NetworkNodes[nodeName].NodeLogState.BlockStoreHeight;
                        dataRow["WalletHeight"] = network.NetworkNodes[nodeName].NodeLogState.WalletHeight;
                        dataRow["NetworkHeight"] = network.NetworkNodes[nodeName].NodeOperationState.NetworkHeight;
                        dataRow["Mempool"] = network.NetworkNodes[nodeName].NodeOperationState.MempoolTransactionCount;
                        dataRow["Peers"] = $"In:{network.NetworkNodes[nodeName].NodeOperationState.InboundPeersCount} / Out:{network.NetworkNodes[nodeName].NodeOperationState.OutboundPeersCount}";
                        dataRow["Uptime"] = network.NetworkNodes[nodeName].NodeOperationState.Uptime.ToString("d' days, 'hh':'mm':'ss");
                        dataRow["Messages"] = $"I:{network.NetworkNodes[nodeName].NodeLogState.InfoMessages.Count} / " +
                                              $"W:{network.NetworkNodes[nodeName].NodeLogState.WarningMessages.Count} / " +
                                              $"E:{network.NetworkNodes[nodeName].NodeLogState.ErrorMessages.Count} / " +
                                              $"C:{network.NetworkNodes[nodeName].NodeLogState.CriticalMessages.Count} / ";
                    }
                }
            }
        }


        private void dataGridViewNodes_Filter()
        {
            List<string> filterCriteria = new List<string>();

            if (checkBoxRunningNodesOnly.Checked) filterCriteria.Add($"[Status] = 'Running'");

            dataGridViewNodesDataView.RowFilter = string.Join(" AND ", filterCriteria);
            dataGridViewNodes.DataSource = dataGridViewNodesDataView;
        }
        private void dataGridViewAgents_Filter()
        {
            dataGridViewAgents.DataSource = dataGridViewAgentsDataView;
        }
        private void dataGridViewBlockchain_Filter()
        {
            dataGridViewBlockchain.DataSource = dataGridViewBlockchainDataView;
        }

        private async void dataGridViewNodes_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            SingleNode node = (SingleNode) dataGridViewNodes.SelectedRows[0].Cells["Node"].Value;

            textBoxNodeName.Text = node.DisplayName;
            textBoxUptime.Text = node.DataDir;
           
            dataGridViewNodeExceptions.DataSource = null;
            propertyGrid1.SelectedObject = node;


            //---------------------
            DataTable exceptions = new DataTable();
            exceptions.Columns.Add("Level");
            exceptions.Columns.Add("Message");
            exceptions.Columns.Add("Count");
            dataGridViewNodeExceptions.DataSource = exceptions;

            if (node.NodeLogState == null) return;

            foreach (string key in node.NodeLogState.CriticalMessages.Keys) exceptions.Rows.Add("Crit", key, node.NodeLogState.CriticalMessages[key]);
            foreach (string key in node.NodeLogState.ErrorMessages.Keys) exceptions.Rows.Add("Error", key, node.NodeLogState.ErrorMessages[key]);
            foreach (string key in node.NodeLogState.WarningMessages.Keys) exceptions.Rows.Add("Warn", key, node.NodeLogState.WarningMessages[key]);
            foreach (string key in node.NodeLogState.InfoMessages.Keys) exceptions.Rows.Add("Info", key, node.NodeLogState.InfoMessages[key]);

            
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

        private void buttonDeployFiles_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                SingleNode node = (SingleNode)row.Cells["Node"].Value;
                var agent = agentConnectionManager.GetAgent(node.Agent);

                agent.ProcessFilesToDeploy(node, network);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                SingleNode node = (SingleNode)row.Cells["Node"].Value;
                var agent = agentConnectionManager.GetAgent(node.Agent);

                agent.StartNode(node);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                SingleNode node = (SingleNode)row.Cells["Node"].Value;
                var agent = agentConnectionManager.GetAgent(node.Agent);

                agent.StopNode(node);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                SingleNode node = (SingleNode)row.Cells["Node"].Value;
                var agent = agentConnectionManager.GetAgent(node.Agent);

                agent.RemoveFile(node, "$NetworkDirectory\\hello.txt");
            }
        }
    }
}
