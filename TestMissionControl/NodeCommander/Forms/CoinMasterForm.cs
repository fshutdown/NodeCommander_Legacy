using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.Resources;
using Stratis.NodeCommander.Forms;
using Stratis.NodeCommander.Properties;
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

        private ClientConfig clientConfig;
        private NodeNetwork managedNodes;
        private DataView dataGridViewAgentsDataView;
        private DataView dataGridViewBlockchainDataView;
        private Tulpep.NotificationWindow.PopupNotifier notifier;
        private AgentConnectionManager clientConnectionManager;

        private List<BaseWorker> _workers = new List<BaseWorker>();
        private CryptoIdWorker cryptoIdWorker;


        

        public CoinMasterForm()
        {
            InitializeComponent();


            //Create pop-up
            notifier = new Tulpep.NotificationWindow.PopupNotifier();
            notifier.Delay = 10000;
            notifier.Click += (sender, eventArgs) =>
            {
                Visible = true;
                BringToFront();
            };

            //Create workers
            cryptoIdWorker = new CryptoIdWorker(60000, new NodeEndpointName("Stratis", "StratisTest"));
            cryptoIdWorker.StateChange += DashboardWorkerStateChanged;
            cryptoIdWorker.DataUpdate += (source, args) => Invoke(new Action<object, CryptoIdDataUpdateEventArgs>(CryptoIdUpdated), source, args);
            _workers.Add(cryptoIdWorker);

            //Start all workers
            foreach (BaseWorker worker in _workers) worker.Start();


            ClientConfigReader reader = new ClientConfigReader();
            clientConfig = reader.ReadConfig();
            List<String> agentList = clientConfig.GetAgentList();
            this.managedNodes = new NodeNetwork();
            foreach (string fullNodeName in clientConfig.NodeItems.Keys)
            {
                BlockchainNode blockchainNode = new BlockchainNode(clientConfig.NodeItems[fullNodeName]);
                this.managedNodes.Nodes.Add(fullNodeName, blockchainNode);
            }

            clientConnectionManager = new AgentConnectionManager(managedNodes);
            
            clientConnectionManager.Session.AgentHealthcheckStatsUpdated += (agentConnection, state, message) => Invoke(new Action<AgentConnection, AgentHealthState, String>(AgentDataTableUpdated), agentConnection, state, message);
            clientConnectionManager.Session.NodeStatsUpdated += (agentConnection, nodesStates) => Invoke(new Action<AgentConnection, BlockchainNodeState[]>(NodeDataUpdated), agentConnection, nodesStates);
            clientConnectionManager.Session.AgentRegistrationUpdated += (agentConnection, agentRegistration) => Invoke(new Action<AgentConnection, AgentRegistration>(AgentRegistrationUpdated), agentConnection, agentRegistration);

            clientConnectionManager.ConnectToAgents(agentList);
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



        private DataTable BuildAgentDataTable()
        {
            DataTable agentsData = new DataTable();
            agentsData.Columns.Add("Address", typeof(AgentConnection));
            agentsData.Columns.Add("Status");
            agentsData.Columns.Add("Info");
            agentsData.Columns.Add("LastUpdate");
            
            foreach (AgentConnection connection in clientConnectionManager.Session.Agents.Values)
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

        private void AgentDataTableUpdated(AgentConnection clientConnection, AgentHealthState state, string message)
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
                dataGridViewAgents.Columns["LastUpdate"].Width = 60;

            }
            else
            {
                dataTable = dataGridViewAgentsDataView.Table;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                AgentConnection tableRecord = (AgentConnection) row["Address"];

                if (tableRecord.Address == clientConnection.Address)
                {
                    row["Status"] = clientConnection.State;
                    row["Info"] = message;
                    row["LastUpdate"] = $"L: {state?.LastUpdateTimestamp} / C: {state?.UpdateCount} / {state?.GitRepositoryInfo[0].CurrentBranchName} / {state?.GitRepositoryInfo[0].LatestLocalCommitDateTime} / {state?.GitRepositoryInfo[0].CommitDifference} / {state?.GitRepositoryInfo[0].RepositoryFullName} / {state?.GitRepositoryInfo[0].RepositoryUrl}";

                    break;
                }
            }
        }

        private void AgentRegistrationUpdated(AgentConnection clientConnection, AgentRegistration agentRegistration)
        {

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
                BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
                var agent = clientConnectionManager.GetAgent(node.NodeConfig.Agent);

                ResourceFromClientDispatcher dispatcher = (ResourceFromClientDispatcher)agent.Dispatchers[MessageType.ResourceFromClient];
                dispatcher.DeployFile(clientConfig, ResourceScope.Node, node.NodeEndpoint.FullNodeName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
                var agent = clientConnectionManager.GetAgent(node.NodeConfig.Agent);

                NodeActionDispatcher dispatcher = (NodeActionDispatcher)agent.Dispatchers[MessageType.ActionRequest];
                dispatcher.StartNode(node);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
                var agent = clientConnectionManager.GetAgent(node.NodeConfig.Agent);

                NodeActionDispatcher dispatcher = (NodeActionDispatcher)agent.Dispatchers[MessageType.ActionRequest];
                dispatcher.StopNode(node);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
                var agent = clientConnectionManager.GetAgent(node.NodeConfig.Agent);

                NodeActionDispatcher dispatcher = (NodeActionDispatcher)agent.Dispatchers[MessageType.ActionRequest];
                dispatcher.RemoveFile(node, "$NetworkDirectory\\hello.txt");
            }
        }

        private async void dataGridViewNodes_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            BlockchainNode node = (BlockchainNode)this.SelectedRows[0].Cells["Node"].Value;
            if (!node.NodeState.Initialized) return;

            groupBox8.Text = "General - " + node.NodeEndpoint.FullNodeName;
            textBoxCodeDirectory.Text = node.NodeConfig.CodeDirectory;
            textBoxProjectDirectory.Text = node.NodeConfig.ProjectDirectory;
            textBoxDataDirectory.Text = node.NodeConfig.DataDir;
            textBoxNetworkDirectory.Text = node.NodeConfig.NetworkDirectory;
            textBoxNodeConfig.Text = node.NodeConfig.NodeConfig;
            labelUptime.Text = node.NodeState.NodeOperationState.Uptime.ToString("d' days, 'hh':'mm':'ss");

            if (node.NodeState.NodeDeploymentState.DirectoryExists)
            {
                if (node.NodeState.NodeDeploymentState.MemPoolFileExists)
                    labelMempool.Text = node.NodeState.NodeDeploymentState.MemPoolFileSize.ToString();
                else labelMempool.Text = "No File";
                if (node.NodeState.NodeDeploymentState.PeersFileExists)
                    labelPeers.Text = node.NodeState.NodeDeploymentState.PeersFileExists.ToString();
                else labelPeers.Text = "No File";
            }
            else
            {
                labelMempool.Text = "No Data Dir";
                labelPeers.Text = "No Data Dir";
            }

            //---------------------
            DataTable exceptions = new DataTable();
            exceptions.Columns.Add("Date");
            exceptions.Columns.Add("Thread");
            exceptions.Columns.Add("Level");
            exceptions.Columns.Add("Source");
            exceptions.Columns.Add("Message");
            exceptions.Columns.Add("ExceptionMessage");
            exceptions.Columns.Add("Stacktrace");
            dataGridViewNodeExceptions.DataSource = exceptions;

            if (node.NodeState.NodeLogState == null) return;


            string resourcePath = Path.Combine(@"C:\Code\TestMissionControl\TestMissionControl\NodeCommander\bin\Debug\Data", node.NodeState.Resources["nodeCommander.txt"].ToString());
            FileInfo resourceFile = new FileInfo(resourcePath);
            if (!resourceFile.Exists) return;

            FileStream f = new FileStream(resourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(f);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] fields = line.Split(new[] { "||" }, StringSplitOptions.None);

                exceptions.Rows.Add(fields);
            }

            f.Close();
        }


        private void button10_Click(object sender, EventArgs e)
        {
            RemoveResourceForm removeResourceForm = new RemoveResourceForm();
            removeResourceForm.Show();
        }
    }
}
