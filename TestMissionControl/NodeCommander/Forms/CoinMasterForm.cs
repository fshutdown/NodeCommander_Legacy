using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Stratis.CoinmasterClient.Analysis;
using Stratis.CoinmasterClient.Analysis.SupportingTypes;
using Stratis.CoinmasterClient.Client;
using Stratis.CoinmasterClient.Client.Dispatchers;
using Stratis.CoinmasterClient.Config;
using Stratis.CoinmasterClient.Database.Model;
using Stratis.CoinmasterClient.Git;
using Stratis.CoinmasterClient.Messages;
using Stratis.CoinmasterClient.Network;
using Stratis.CoinmasterClient.NodeResources;
using Stratis.CoinmasterClient.Resources;
using Stratis.CoinmasterClient.Utilities;
using Stratis.NodeCommander.Forms;
using Stratis.NodeCommander.Properties;
using Stratis.NodeCommander.Tools;
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
        private DataView dataGridViewBlockchainDataView;
        private Tulpep.NotificationWindow.PopupNotifier notifier;
        private AgentConnectionManager clientConnectionManager;

        private List<BaseWorker> _workers = new List<BaseWorker>();
        private CryptoIdWorker cryptoIdWorker;

        public CoinMasterForm()
        {
            InitializeComponent();
        }

        private void CoinMasterForm_Load(object sender, EventArgs e)
        {
            buttonEditNodeProfile_Click(null, EventArgs.Empty);
            ReadNodeProfiles();

            //Create pop-up
            notifier = new Tulpep.NotificationWindow.PopupNotifier();
            notifier.Delay = 10000;
            notifier.Click += (notifierSender, eventArgs) =>
            {
                Visible = true;
                BringToFront();
            };

            //Create workers
            cryptoIdWorker = new CryptoIdWorker(60000, new NodeEndpointName("Stratis", "StratisTest"));
            cryptoIdWorker.StateChange += DashboardWorkerStateChanged;
            cryptoIdWorker.DataUpdate += (source, args) => Invoke(new Action<object, CryptoIdDataUpdateEventArgs>(CryptoIdUpdated), source, args);
            _workers.Add(cryptoIdWorker);

            //Start all worker
            // foreach (BaseWorker worker in _workers) worker.Start();


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
            clientConnectionManager.Session.NodesUpdated += (agentConnection, updatedNodes) => Invoke(new Action<AgentConnection, NodeNetwork>(NodeDataUpdated), agentConnection, updatedNodes);
            clientConnectionManager.Session.AgentRegistrationUpdated += (agentConnection, agentRegistration) => Invoke(new Action<AgentConnection, AgentRegistration>(AgentRegistrationUpdated), agentConnection, agentRegistration);

            clientConnectionManager.ConnectToAgents(agentList);
        }

        private void ReadNodeProfiles()
        {
        }

        private void NodeDataUpdated(AgentConnection agentConnection, NodeNetwork updatedNodes)
        {
            dataGridViewNodes.UpdateNodes(updatedNodes, clientConnectionManager.Session.Database);

            toolStripStatusLabelDatabase.Text = "DB: " + clientConnectionManager.Session.Database.GetDatabaseFilesystemSize();

            int totalNodes = managedNodes.Nodes.Count;
            int runningNodes = managedNodes.Nodes.Count(n => n.Value.NodeState.Initialized && n.Value.NodeState.NodeOperationState.State == ProcessState.Running);
            toolStripStatusLabelNodeState.Text = $"Nodes: {runningNodes} / {totalNodes}";
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

        private DataTable BuildBlockchainDataTable()
        {
            DataTable blockchainData = new DataTable();
            blockchainData.Columns.Add("Measure");
            blockchainData.Columns.Add("Value");
            blockchainData.Columns.Add("Additional");
            
            return blockchainData;
        }

        private void AgentDataTableUpdated(AgentConnection agentConnection, AgentHealthState state, string message)
        {
            dataGridViewAgents.UpdateNodes(agentConnection, state, message);
        }

        private void AgentRegistrationUpdated(AgentConnection clientConnection, AgentRegistration agentRegistration)
        {

        }

        private void dataGridViewNodes_Filter(DataView dataView)
        {
            List<string> filterCriteria = new List<string>();

            if (checkBoxRunningNodesOnly.Checked) filterCriteria.Add($"[Status] = 'Running'");

            dataView.RowFilter = string.Join(" AND ", filterCriteria);
            dataGridViewNodes.DataSource = dataView;
        }

        private void dataGridViewNodeExceptions_Filter(DataView dataView)
        {
            dataGridViewNodeExceptions.DataSource = dataView;
        }

        private void dataGridViewAgents_Filter(DataView dataView)
        {
            dataGridViewAgents.DataSource = dataView;
        }

        private void dataGridViewBlockchain_Filter()
        {
            dataGridViewBlockchain.DataSource = dataGridViewBlockchainDataView;
        }

        private void dataGridViewPeers_Filter(DataView dataView)
        {
            dataGridViewPeers.DataSource = dataView;
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
            SendAction((dispatcher, node) => dispatcher.StartNode(node));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendAction((dispatcher, node) => dispatcher.StopNode(node));
        }

        private void linkLabelPullCurrentBranch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SendAction((dispatcher, node) => dispatcher.GitPull(node));
        }

        private void SendAction(Action<NodeActionDispatcher, BlockchainNode> action)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
                var agent = clientConnectionManager.GetAgent(node.NodeConfig.Agent);

                NodeActionDispatcher dispatcher = (NodeActionDispatcher)agent.Dispatchers[MessageType.ActionRequest];
                action.Invoke(dispatcher, node);
            }
        }

        private void dataGridViewNodes_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected && e.StateChanged != DataGridViewElementStates.None) return;
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            BlockchainNode node = (BlockchainNode)dataGridViewNodes.SelectedRows[0].Cells["Node"].Value;
            if (node == null) return;

            if (!node.NodeState.Initialized)
            {
                tabPageOverview.Enabled = false;
            }
            else
            {
                tabPageOverview.Enabled = true;
            }

            linkLabelRepositoryUrl.Text = node.GitRepositoryInfo.RepositoryUrl;
            labelCurrentBranch.Text = node.GitRepositoryInfo.CurrentBranchName;
            labelNumberOfCommitsBehind.Text = node.GitRepositoryInfo.CommitDifference;
            labelLastUpdateDate.Text = node.GitRepositoryInfo.LatestLocalCommitDateTime.ToString("dd MMM yyyy");
            labelLastUpdateTime.Text = node.GitRepositoryInfo.LatestLocalCommitDateTime.ToString("HH:mm");
            labelLastUpdateTimeAgo.Text = "(" + (DateTime.Now - node.GitRepositoryInfo.LatestLocalCommitDateTime).ToHumanReadable() + ")";
            labelLastUpdateAuthor.Text = node.GitRepositoryInfo.LatestLocalCommitAuthor;
            labelLastUpdateMessage.Text = node.GitRepositoryInfo.LatestLocalCommitMessage;
            toolTipHelp.SetToolTip(this.labelLastUpdateMessage, node.GitRepositoryInfo.LatestLocalCommitMessage);
            labelDaemonName.Text = node.NodeConfig.DaemonName;
            labelStartupOptions.Text = node.NodeConfig.StartupSwitches;
            labelBlockchainName.Text = node.NodeEndpoint.NodeBlockchainName;
            labelNetworkName.Text = node.NodeEndpoint.NodeNetworkName;
            labelDataDir.Text = node.NodeConfig.DataDir;
            labelDataDirSize.Text = "(" + node.NodeState.NodeDeploymentState.DataDirSize + ")";
            labelHeaderHeight.Text = node.NodeState.NodeLogState.HeadersHeight;
            labelConsensusHeight.Text = node.NodeState.NodeLogState.ConsensusHeight;
            labelBlockHeight.Text = node.NodeState.NodeLogState.BlockStoreHeight;
            labelWalletHeight.Text = node.NodeState.NodeLogState.WalletHeight;
            labelNetworkHeight.Text = node.NodeState.NodeOperationState.NetworkHeight.ToString();
            labelOutboundPeers.Text = node.NodeState.NodeOperationState.OutboundPeersCount.ToString();
            labelInboundPeers.Text = node.NodeState.NodeOperationState.InboundPeersCount.ToString();
            labelBannedPeers.Text = node.NodeState.NodeOperationState.BannedPeersCount.ToString();

            textBoxCodeDirectory.Text = node.NodeConfig.CodeDirectory;
            textBoxNetworkDirectory.Text = node.NodeConfig.NetworkDirectory;
            List<NodeLogMessage> logMessages = clientConnectionManager.Session.Database.GetLogMessages(node.NodeEndpoint.FullNodeName);
            dataGridViewNodeExceptions.UpdateNodes(logMessages, e.StateChanged == DataGridViewElementStates.None);
            dataGridViewPeers.UpdateNodes(node, clientConnectionManager.Session.Database);
        }


        private void button10_Click(object sender, EventArgs e)
        {
            Point buttonScreenLocation = buttonClearData.FindForm().PointToClient(buttonClearData.Parent.PointToScreen(buttonClearData.Location));
            Point contextMenuLocation = new Point(buttonScreenLocation.X, buttonScreenLocation.Y + buttonClearData.Height);
            contextMenuStripClearData.Show(this, contextMenuLocation);
        }

        private void buttonEditNodeProfile_Click(object sender, EventArgs e)
        {
            if (buttonEditNodeProfile.Tag.ToString() == "Edit")
            {
                buttonEditNodeProfile.Tag = "";
                splitContainer3.SplitterDistance = 54;
                groupBoxNodeFilter.Size = new Size(groupBoxNodeFilter.Size.Width, 54);
                panelNodeFilterEdit.Visible = false;
            }
            else
            {
                buttonEditNodeProfile.Tag = "Edit";
                splitContainer3.SplitterDistance = 224;
                groupBoxNodeFilter.Size = new Size(groupBoxNodeFilter.Size.Width, 225);
                //groupBoxNodeFilter.Size = new Size(785, 225);
                panelNodeFilterEdit.Visible = true;
            }
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveResourceForm removeResourceForm = new RemoveResourceForm();
            removeResourceForm.Show();
        }

        private void mempoolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveResourceForSelectedNodes(NodeResourceType.Mempool);
        }

        private void peersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveResourceForSelectedNodes(NodeResourceType.Peers);
        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveResourceForSelectedNodes(NodeResourceType.Logs);
        }

        private void RemoveResourceForSelectedNodes(NodeResourceType resourceType)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in dataGridViewNodes.SelectedRows)
            {
                BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
                var agent = clientConnectionManager.GetAgent(node.NodeConfig.Agent);

                NodeActionDispatcher dispatcher = (NodeActionDispatcher)agent.Dispatchers[MessageType.ActionRequest];
                dispatcher.RemoveResource(node, resourceType);
            }
        }

        private void dataGridViewNodeExceptions_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                dataGridViewNodeExceptions.Size = new Size(dataGridViewNodeExceptions.Size.Width, groupBoxNodeExceptions.Height - 25);
                stackTraceRichTextBox.Visible = false;
            }
        }

        private void dataGridViewNodeExceptions_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        private void linkLabelSwitchBranch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dataGridViewNodes.SelectedRows.Count == 0) return;
            if (dataGridViewNodes.SelectedRows.Count > 1)
            {
                MessageBox.Show("No more than 1 node can be celected for this operation", "Bulk Operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DataGridViewRow row = dataGridViewNodes.SelectedRows[0];

            BlockchainNode node = (BlockchainNode)row.Cells["Node"].Value;
            //ToDo: Create pop-up to select branch and send command to agent
        }
    }
}
