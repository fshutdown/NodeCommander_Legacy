using System.Windows.Forms;

namespace Stratis.NodeCommander
{
    partial class CoinMasterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoinMasterForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBoxPullRequestId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageNodes = new System.Windows.Forms.TabPage();
            this.splitContainerOuter = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBoxNodeFilter = new System.Windows.Forms.GroupBox();
            this.panelNodeFilterEdit = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.checkedListBox3 = new System.Windows.Forms.CheckedListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBoxRunningNodesOnly = new System.Windows.Forms.CheckBox();
            this.buttonEditNodeProfile = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dataGridViewNodes = new Stratis.NodeCommander.Controls.NodeOverview.NodeOverviewDataGridView();
            this.contextMenuStripNodeList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.clearDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deployFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.dataGridViewNodeExceptions = new Stratis.NodeCommander.Controls.NodeExceptions.NodeExceptionsDataGridView();
            this.contextMenuStripExceptionList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showStacktraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.dataGridViewPeers = new Stratis.NodeCommander.Controls.PeerConnections.PeerConnectionsDataGridView();
            this.dataGridViewMempool = new System.Windows.Forms.DataGridView();
            this.buttonRemoveNode = new System.Windows.Forms.Button();
            this.buttonAddNode = new System.Windows.Forms.Button();
            this.labelBannedPeers = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.labelInboundPeers = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.labelOutboundPeers = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelStartupOptions = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelBlockchainName = new System.Windows.Forms.Label();
            this.labelNetworkName = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTotalReorgCount = new System.Windows.Forms.Label();
            this.labelTotalReorgStats = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTotalBlockMinedCount = new System.Windows.Forms.Label();
            this.labelTotalBlockMinedStats = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLastReorgBlockHeight = new System.Windows.Forms.Label();
            this.labelLastReorgTimeAgo = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLastMinedBlockHeight = new System.Windows.Forms.Label();
            this.labelLastMinedTimeAgo = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.labelNetworkHeight = new System.Windows.Forms.Label();
            this.labelWalletHeight = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.labelBlockHeight = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.labelConsensusHeight = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.labelHeaderHeight = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDataDir = new System.Windows.Forms.Label();
            this.labelDataDirSize = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.labelDaemonName = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelCurrentBranch = new System.Windows.Forms.Label();
            this.labelNumberOfCommitsBehind = new System.Windows.Forms.Label();
            this.linkLabelPullCurrentBranch = new System.Windows.Forms.LinkLabel();
            this.linkLabelSwitchBranch = new System.Windows.Forms.LinkLabel();
            this.labelLastUpdateMessage = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLastUpdateDate = new System.Windows.Forms.Label();
            this.labelLastUpdateTime = new System.Windows.Forms.Label();
            this.labelLastUpdateTimeAgo = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.labelLastUpdateAuthor = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.linkLabelRepositoryUrl = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonDeployFiles = new System.Windows.Forms.Button();
            this.buttonClearData = new System.Windows.Forms.Button();
            this.buttonStartNode = new System.Windows.Forms.Button();
            this.buttonStopNode = new System.Windows.Forms.Button();
            this.buttonRestartNode = new System.Windows.Forms.Button();
            this.tabPageGit = new System.Windows.Forms.TabPage();
            this.tabPageNodeConfig = new System.Windows.Forms.TabPage();
            this.textBoxCodeDirectory = new System.Windows.Forms.TextBox();
            this.textBoxNetworkDirectory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.dataGridViewAgents = new Stratis.NodeCommander.Controls.Agents.AgentsDataGridView();
            this.contextMenuStripAgents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showAgentsNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.startTorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopTorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageGitExpert = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPageControlPanel = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPageBlockchain = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.dataGridViewBlockchain = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.contextMenuStripPeers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.banNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unbanNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMemoryPool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStripApplicationMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.networkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suspendMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.notifyAboutNewExceptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyAboutPerformanceIssuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripApplicationMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelNodeState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelWorker = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStripClearData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mempoolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.peersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageNodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).BeginInit();
            this.splitContainerOuter.Panel1.SuspendLayout();
            this.splitContainerOuter.Panel2.SuspendLayout();
            this.splitContainerOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBoxNodeFilter.SuspendLayout();
            this.panelNodeFilterEdit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNodes)).BeginInit();
            this.contextMenuStripNodeList.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNodeExceptions)).BeginInit();
            this.contextMenuStripExceptionList.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMempool)).BeginInit();
            this.flowLayoutPanel8.SuspendLayout();
            this.flowLayoutPanel7.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPageNodeConfig.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAgents)).BeginInit();
            this.contextMenuStripAgents.SuspendLayout();
            this.tabPageGitExpert.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPageControlPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageBlockchain.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlockchain)).BeginInit();
            this.contextMenuStripPeers.SuspendLayout();
            this.menuStripApplicationMain.SuspendLayout();
            this.statusStripApplicationMain.SuspendLayout();
            this.contextMenuStripClearData.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textBoxPullRequestId);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(861, 63);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Full Node";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(264, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Get Code";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBoxPullRequestId
            // 
            this.textBoxPullRequestId.Location = new System.Drawing.Point(103, 22);
            this.textBoxPullRequestId.Name = "textBoxPullRequestId";
            this.textBoxPullRequestId.Size = new System.Drawing.Size(155, 20);
            this.textBoxPullRequestId.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pull Request Id:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageNodes);
            this.tabControl1.Controls.Add(this.tabPageGitExpert);
            this.tabControl1.Controls.Add(this.tabPageControlPanel);
            this.tabControl1.Controls.Add(this.tabPageBlockchain);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1572, 857);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageNodes
            // 
            this.tabPageNodes.Controls.Add(this.splitContainerOuter);
            this.tabPageNodes.Location = new System.Drawing.Point(4, 22);
            this.tabPageNodes.Name = "tabPageNodes";
            this.tabPageNodes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNodes.Size = new System.Drawing.Size(1564, 831);
            this.tabPageNodes.TabIndex = 0;
            this.tabPageNodes.Text = "Nodes";
            this.tabPageNodes.UseVisualStyleBackColor = true;
            // 
            // splitContainerOuter
            // 
            this.splitContainerOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOuter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerOuter.Location = new System.Drawing.Point(3, 3);
            this.splitContainerOuter.Name = "splitContainerOuter";
            // 
            // splitContainerOuter.Panel1
            // 
            this.splitContainerOuter.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainerOuter.Panel2
            // 
            this.splitContainerOuter.Panel2.Controls.Add(this.tabControl2);
            this.splitContainerOuter.Panel2.Controls.Add(this.groupBox9);
            this.splitContainerOuter.Size = new System.Drawing.Size(1558, 825);
            this.splitContainerOuter.SplitterDistance = 902;
            this.splitContainerOuter.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer2.Size = new System.Drawing.Size(902, 825);
            this.splitContainer2.SplitterDistance = 490;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBoxNodeFilter);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer3.Size = new System.Drawing.Size(902, 490);
            this.splitContainer3.SplitterDistance = 224;
            this.splitContainer3.TabIndex = 9;
            // 
            // groupBoxNodeFilter
            // 
            this.groupBoxNodeFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxNodeFilter.Controls.Add(this.panelNodeFilterEdit);
            this.groupBoxNodeFilter.Controls.Add(this.panel1);
            this.groupBoxNodeFilter.Location = new System.Drawing.Point(0, 0);
            this.groupBoxNodeFilter.Name = "groupBoxNodeFilter";
            this.groupBoxNodeFilter.Size = new System.Drawing.Size(902, 224);
            this.groupBoxNodeFilter.TabIndex = 0;
            this.groupBoxNodeFilter.TabStop = false;
            this.groupBoxNodeFilter.Text = "Node Filter";
            // 
            // panelNodeFilterEdit
            // 
            this.panelNodeFilterEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelNodeFilterEdit.Controls.Add(this.label21);
            this.panelNodeFilterEdit.Controls.Add(this.checkedListBox3);
            this.panelNodeFilterEdit.Controls.Add(this.label14);
            this.panelNodeFilterEdit.Controls.Add(this.label13);
            this.panelNodeFilterEdit.Controls.Add(this.checkedListBox2);
            this.panelNodeFilterEdit.Controls.Add(this.label12);
            this.panelNodeFilterEdit.Controls.Add(this.checkedListBox1);
            this.panelNodeFilterEdit.Location = new System.Drawing.Point(3, 53);
            this.panelNodeFilterEdit.Name = "panelNodeFilterEdit";
            this.panelNodeFilterEdit.Size = new System.Drawing.Size(896, 166);
            this.panelNodeFilterEdit.TabIndex = 1;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(327, 2);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(75, 13);
            this.label21.TabIndex = 5;
            this.label21.Text = "Blockchain";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListBox3
            // 
            this.checkedListBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox3.FormattingEnabled = true;
            this.checkedListBox3.Items.AddRange(new object[] {
            "Bitcoin",
            "Stratis"});
            this.checkedListBox3.Location = new System.Drawing.Point(327, 18);
            this.checkedListBox3.Name = "checkedListBox3";
            this.checkedListBox3.Size = new System.Drawing.Size(75, 144);
            this.checkedListBox3.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label14.Location = new System.Drawing.Point(15, 0);
            this.label14.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(866, 2);
            this.label14.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Platform";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Items.AddRange(new object[] {
            "Windows",
            "Mac OS",
            "Linux"});
            this.checkedListBox2.Location = new System.Drawing.Point(6, 20);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(112, 144);
            this.checkedListBox2.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(127, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(194, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Codebase";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "FullNode",
            "SmartContracts",
            "SideChains",
            "Breeze"});
            this.checkedListBox1.Location = new System.Drawing.Point(127, 20);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(194, 144);
            this.checkedListBox1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.checkBoxRunningNodesOnly);
            this.panel1.Controls.Add(this.buttonEditNodeProfile);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(896, 38);
            this.panel1.TabIndex = 0;
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(769, -1);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(127, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Nodes with Warnings";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(769, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(109, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Nodes with Errors";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunningNodesOnly
            // 
            this.checkBoxRunningNodesOnly.AutoSize = true;
            this.checkBoxRunningNodesOnly.Location = new System.Drawing.Point(356, 7);
            this.checkBoxRunningNodesOnly.Name = "checkBoxRunningNodesOnly";
            this.checkBoxRunningNodesOnly.Size = new System.Drawing.Size(66, 17);
            this.checkBoxRunningNodesOnly.TabIndex = 4;
            this.checkBoxRunningNodesOnly.Text = "Running";
            this.checkBoxRunningNodesOnly.UseVisualStyleBackColor = true;
            // 
            // buttonEditNodeProfile
            // 
            this.buttonEditNodeProfile.Location = new System.Drawing.Point(324, 5);
            this.buttonEditNodeProfile.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonEditNodeProfile.Name = "buttonEditNodeProfile";
            this.buttonEditNodeProfile.Size = new System.Drawing.Size(26, 21);
            this.buttonEditNodeProfile.TabIndex = 3;
            this.buttonEditNodeProfile.Tag = "Edit";
            this.buttonEditNodeProfile.Text = "...";
            this.buttonEditNodeProfile.UseVisualStyleBackColor = true;
            this.buttonEditNodeProfile.Click += new System.EventHandler(this.buttonEditNodeProfile_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(77, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(244, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Node Profile:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dataGridViewNodes);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(902, 262);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Nodes";
            // 
            // dataGridViewNodes
            // 
            this.dataGridViewNodes.AllowUserToAddRows = false;
            this.dataGridViewNodes.AllowUserToDeleteRows = false;
            this.dataGridViewNodes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewNodes.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNodes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewNodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNodes.ContextMenuStrip = this.contextMenuStripNodeList;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewNodes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewNodes.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewNodes.MultiSelect = false;
            this.dataGridViewNodes.Name = "dataGridViewNodes";
            this.dataGridViewNodes.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNodes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewNodes.RowHeadersVisible = false;
            this.dataGridViewNodes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewNodes.Size = new System.Drawing.Size(896, 243);
            this.dataGridViewNodes.TabIndex = 8;
            this.dataGridViewNodes.Filter += new System.Action<System.Data.DataView>(this.dataGridViewNodes_Filter);
            this.dataGridViewNodes.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewNodes_RowStateChanged);
            // 
            // contextMenuStripNodeList
            // 
            this.contextMenuStripNodeList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startNodeToolStripMenuItem,
            this.stopNodeToolStripMenuItem,
            this.restartNodeToolStripMenuItem,
            this.toolStripMenuItem4,
            this.clearDataToolStripMenuItem,
            this.deployFilesToolStripMenuItem});
            this.contextMenuStripNodeList.Name = "contextMenuStripNodeList";
            this.contextMenuStripNodeList.Size = new System.Drawing.Size(141, 120);
            // 
            // startNodeToolStripMenuItem
            // 
            this.startNodeToolStripMenuItem.Name = "startNodeToolStripMenuItem";
            this.startNodeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.startNodeToolStripMenuItem.Text = "Start Node";
            // 
            // stopNodeToolStripMenuItem
            // 
            this.stopNodeToolStripMenuItem.Name = "stopNodeToolStripMenuItem";
            this.stopNodeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.stopNodeToolStripMenuItem.Text = "Stop Node";
            // 
            // restartNodeToolStripMenuItem
            // 
            this.restartNodeToolStripMenuItem.Name = "restartNodeToolStripMenuItem";
            this.restartNodeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.restartNodeToolStripMenuItem.Text = "Restart node";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(137, 6);
            // 
            // clearDataToolStripMenuItem
            // 
            this.clearDataToolStripMenuItem.Name = "clearDataToolStripMenuItem";
            this.clearDataToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.clearDataToolStripMenuItem.Text = "Clear Data";
            // 
            // deployFilesToolStripMenuItem
            // 
            this.deployFilesToolStripMenuItem.Name = "deployFilesToolStripMenuItem";
            this.deployFilesToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.deployFilesToolStripMenuItem.Text = "Deploy Files";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Controls.Add(this.dataGridViewNodeExceptions);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(902, 331);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Node Exceptions";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 200);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(896, 125);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // dataGridViewNodeExceptions
            // 
            this.dataGridViewNodeExceptions.AllowUserToAddRows = false;
            this.dataGridViewNodeExceptions.AllowUserToDeleteRows = false;
            this.dataGridViewNodeExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewNodeExceptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewNodeExceptions.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNodeExceptions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewNodeExceptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNodeExceptions.ContextMenuStrip = this.contextMenuStripExceptionList;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewNodeExceptions.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewNodeExceptions.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewNodeExceptions.MultiSelect = false;
            this.dataGridViewNodeExceptions.Name = "dataGridViewNodeExceptions";
            this.dataGridViewNodeExceptions.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewNodeExceptions.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewNodeExceptions.RowHeadersVisible = false;
            this.dataGridViewNodeExceptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewNodeExceptions.Size = new System.Drawing.Size(896, 178);
            this.dataGridViewNodeExceptions.TabIndex = 7;
            this.dataGridViewNodeExceptions.Filter += new System.Action<System.Data.DataView>(this.dataGridViewNodeExceptions_Filter);
            // 
            // contextMenuStripExceptionList
            // 
            this.contextMenuStripExceptionList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showStacktraceToolStripMenuItem});
            this.contextMenuStripExceptionList.Name = "contextMenuStripExceptionList";
            this.contextMenuStripExceptionList.Size = new System.Drawing.Size(169, 26);
            // 
            // showStacktraceToolStripMenuItem
            // 
            this.showStacktraceToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.showStacktraceToolStripMenuItem.Name = "showStacktraceToolStripMenuItem";
            this.showStacktraceToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.showStacktraceToolStripMenuItem.Text = "Show Stacktrace";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPageOverview);
            this.tabControl2.Controls.Add(this.tabPageGit);
            this.tabControl2.Controls.Add(this.tabPageNodeConfig);
            this.tabControl2.Location = new System.Drawing.Point(2, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(650, 696);
            this.tabControl2.TabIndex = 24;
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.dataGridViewPeers);
            this.tabPageOverview.Controls.Add(this.dataGridViewMempool);
            this.tabPageOverview.Controls.Add(this.buttonRemoveNode);
            this.tabPageOverview.Controls.Add(this.buttonAddNode);
            this.tabPageOverview.Controls.Add(this.labelBannedPeers);
            this.tabPageOverview.Controls.Add(this.label27);
            this.tabPageOverview.Controls.Add(this.labelInboundPeers);
            this.tabPageOverview.Controls.Add(this.label38);
            this.tabPageOverview.Controls.Add(this.labelOutboundPeers);
            this.tabPageOverview.Controls.Add(this.label41);
            this.tabPageOverview.Controls.Add(this.label46);
            this.tabPageOverview.Controls.Add(this.label48);
            this.tabPageOverview.Controls.Add(this.label49);
            this.tabPageOverview.Controls.Add(this.label10);
            this.tabPageOverview.Controls.Add(this.labelStartupOptions);
            this.tabPageOverview.Controls.Add(this.label24);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel8);
            this.tabPageOverview.Controls.Add(this.label18);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel7);
            this.tabPageOverview.Controls.Add(this.label57);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel6);
            this.tabPageOverview.Controls.Add(this.label52);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel5);
            this.tabPageOverview.Controls.Add(this.label51);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel4);
            this.tabPageOverview.Controls.Add(this.label47);
            this.tabPageOverview.Controls.Add(this.labelNetworkHeight);
            this.tabPageOverview.Controls.Add(this.labelWalletHeight);
            this.tabPageOverview.Controls.Add(this.label44);
            this.tabPageOverview.Controls.Add(this.labelBlockHeight);
            this.tabPageOverview.Controls.Add(this.label42);
            this.tabPageOverview.Controls.Add(this.labelConsensusHeight);
            this.tabPageOverview.Controls.Add(this.label40);
            this.tabPageOverview.Controls.Add(this.labelHeaderHeight);
            this.tabPageOverview.Controls.Add(this.label37);
            this.tabPageOverview.Controls.Add(this.label36);
            this.tabPageOverview.Controls.Add(this.label35);
            this.tabPageOverview.Controls.Add(this.label34);
            this.tabPageOverview.Controls.Add(this.label33);
            this.tabPageOverview.Controls.Add(this.label32);
            this.tabPageOverview.Controls.Add(this.label31);
            this.tabPageOverview.Controls.Add(this.label30);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel3);
            this.tabPageOverview.Controls.Add(this.label28);
            this.tabPageOverview.Controls.Add(this.labelDaemonName);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel2);
            this.tabPageOverview.Controls.Add(this.labelLastUpdateMessage);
            this.tabPageOverview.Controls.Add(this.flowLayoutPanel1);
            this.tabPageOverview.Controls.Add(this.label17);
            this.tabPageOverview.Controls.Add(this.label16);
            this.tabPageOverview.Controls.Add(this.linkLabelRepositoryUrl);
            this.tabPageOverview.Controls.Add(this.label9);
            this.tabPageOverview.Controls.Add(this.label8);
            this.tabPageOverview.Controls.Add(this.label6);
            this.tabPageOverview.Controls.Add(this.groupBox4);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverview.Size = new System.Drawing.Size(642, 670);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPeers
            // 
            this.dataGridViewPeers.AllowUserToAddRows = false;
            this.dataGridViewPeers.AllowUserToDeleteRows = false;
            this.dataGridViewPeers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPeers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPeers.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPeers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewPeers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPeers.ContextMenuStrip = this.contextMenuStripExceptionList;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewPeers.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewPeers.Location = new System.Drawing.Point(18, 460);
            this.dataGridViewPeers.MultiSelect = false;
            this.dataGridViewPeers.Name = "dataGridViewPeers";
            this.dataGridViewPeers.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPeers.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewPeers.RowHeadersVisible = false;
            this.dataGridViewPeers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPeers.Size = new System.Drawing.Size(594, 143);
            this.dataGridViewPeers.TabIndex = 93;
            this.dataGridViewPeers.Filter += new System.Action<System.Data.DataView>(this.dataGridViewPeers_Filter);
            // 
            // dataGridViewMempool
            // 
            this.dataGridViewMempool.AllowUserToAddRows = false;
            this.dataGridViewMempool.AllowUserToDeleteRows = false;
            this.dataGridViewMempool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMempool.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewMempool.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMempool.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewMempool.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMempool.ContextMenuStrip = this.contextMenuStripExceptionList;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewMempool.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewMempool.Location = new System.Drawing.Point(18, 219);
            this.dataGridViewMempool.MultiSelect = false;
            this.dataGridViewMempool.Name = "dataGridViewMempool";
            this.dataGridViewMempool.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMempool.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewMempool.RowHeadersVisible = false;
            this.dataGridViewMempool.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMempool.Size = new System.Drawing.Size(594, 143);
            this.dataGridViewMempool.TabIndex = 92;
            // 
            // buttonRemoveNode
            // 
            this.buttonRemoveNode.Enabled = false;
            this.buttonRemoveNode.Location = new System.Drawing.Point(501, 428);
            this.buttonRemoveNode.Name = "buttonRemoveNode";
            this.buttonRemoveNode.Size = new System.Drawing.Size(103, 23);
            this.buttonRemoveNode.TabIndex = 91;
            this.buttonRemoveNode.Text = "Remove Node";
            this.buttonRemoveNode.UseVisualStyleBackColor = true;
            // 
            // buttonAddNode
            // 
            this.buttonAddNode.Location = new System.Drawing.Point(392, 428);
            this.buttonAddNode.Name = "buttonAddNode";
            this.buttonAddNode.Size = new System.Drawing.Size(103, 23);
            this.buttonAddNode.TabIndex = 90;
            this.buttonAddNode.Text = "Add Node";
            this.buttonAddNode.UseVisualStyleBackColor = true;
            // 
            // labelBannedPeers
            // 
            this.labelBannedPeers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBannedPeers.Location = new System.Drawing.Point(269, 441);
            this.labelBannedPeers.Margin = new System.Windows.Forms.Padding(3);
            this.labelBannedPeers.Name = "labelBannedPeers";
            this.labelBannedPeers.Size = new System.Drawing.Size(110, 13);
            this.labelBannedPeers.TabIndex = 89;
            this.labelBannedPeers.Text = "1";
            this.labelBannedPeers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label27.Location = new System.Drawing.Point(383, 425);
            this.label27.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(2, 29);
            this.label27.TabIndex = 88;
            // 
            // labelInboundPeers
            // 
            this.labelInboundPeers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInboundPeers.Location = new System.Drawing.Point(149, 441);
            this.labelInboundPeers.Margin = new System.Windows.Forms.Padding(3);
            this.labelInboundPeers.Name = "labelInboundPeers";
            this.labelInboundPeers.Size = new System.Drawing.Size(110, 13);
            this.labelInboundPeers.TabIndex = 87;
            this.labelInboundPeers.Text = "0";
            this.labelInboundPeers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label38.Location = new System.Drawing.Point(264, 425);
            this.label38.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(2, 29);
            this.label38.TabIndex = 86;
            // 
            // labelOutboundPeers
            // 
            this.labelOutboundPeers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOutboundPeers.Location = new System.Drawing.Point(29, 441);
            this.labelOutboundPeers.Margin = new System.Windows.Forms.Padding(3);
            this.labelOutboundPeers.Name = "labelOutboundPeers";
            this.labelOutboundPeers.Size = new System.Drawing.Size(110, 13);
            this.labelOutboundPeers.TabIndex = 85;
            this.labelOutboundPeers.Text = "8";
            this.labelOutboundPeers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label41.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label41.Location = new System.Drawing.Point(143, 425);
            this.label41.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(2, 29);
            this.label41.TabIndex = 84;
            // 
            // label46
            // 
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(269, 423);
            this.label46.Margin = new System.Windows.Forms.Padding(3);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(110, 13);
            this.label46.TabIndex = 81;
            this.label46.Text = "Banned Peers";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label48
            // 
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.Location = new System.Drawing.Point(149, 423);
            this.label48.Margin = new System.Windows.Forms.Padding(3);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(110, 13);
            this.label48.TabIndex = 80;
            this.label48.Text = "Inbound Peers";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label49
            // 
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.Location = new System.Drawing.Point(29, 423);
            this.label49.Margin = new System.Windows.Forms.Padding(3);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(110, 13);
            this.label49.TabIndex = 79;
            this.label49.Text = "Outbound Peers";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(11, 418);
            this.label10.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(606, 2);
            this.label10.TabIndex = 77;
            // 
            // labelStartupOptions
            // 
            this.labelStartupOptions.AutoSize = true;
            this.labelStartupOptions.Location = new System.Drawing.Point(125, 130);
            this.labelStartupOptions.Margin = new System.Windows.Forms.Padding(0);
            this.labelStartupOptions.Name = "labelStartupOptions";
            this.labelStartupOptions.Size = new System.Drawing.Size(125, 13);
            this.labelStartupOptions.TabIndex = 75;
            this.labelStartupOptions.Text = "registration tumblebit light";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(20, 130);
            this.label24.Margin = new System.Windows.Forms.Padding(3);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(99, 13);
            this.label24.TabIndex = 74;
            this.label24.Text = "Startup Options:";
            // 
            // flowLayoutPanel8
            // 
            this.flowLayoutPanel8.Controls.Add(this.labelBlockchainName);
            this.flowLayoutPanel8.Controls.Add(this.labelNetworkName);
            this.flowLayoutPanel8.Location = new System.Drawing.Point(125, 149);
            this.flowLayoutPanel8.Name = "flowLayoutPanel8";
            this.flowLayoutPanel8.Size = new System.Drawing.Size(415, 13);
            this.flowLayoutPanel8.TabIndex = 73;
            // 
            // labelBlockchainName
            // 
            this.labelBlockchainName.AutoSize = true;
            this.labelBlockchainName.Location = new System.Drawing.Point(0, 0);
            this.labelBlockchainName.Margin = new System.Windows.Forms.Padding(0);
            this.labelBlockchainName.Name = "labelBlockchainName";
            this.labelBlockchainName.Size = new System.Drawing.Size(39, 13);
            this.labelBlockchainName.TabIndex = 41;
            this.labelBlockchainName.Text = "Bitcoin";
            // 
            // labelNetworkName
            // 
            this.labelNetworkName.AutoSize = true;
            this.labelNetworkName.ForeColor = System.Drawing.Color.Maroon;
            this.labelNetworkName.Location = new System.Drawing.Point(39, 0);
            this.labelNetworkName.Margin = new System.Windows.Forms.Padding(0);
            this.labelNetworkName.Name = "labelNetworkName";
            this.labelNetworkName.Size = new System.Drawing.Size(49, 13);
            this.labelNetworkName.TabIndex = 43;
            this.labelNetworkName.Text = "(Testnet)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(45, 149);
            this.label18.Margin = new System.Windows.Forms.Padding(3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(74, 13);
            this.label18.TabIndex = 72;
            this.label18.Text = "Blockchain:";
            // 
            // flowLayoutPanel7
            // 
            this.flowLayoutPanel7.Controls.Add(this.labelTotalReorgCount);
            this.flowLayoutPanel7.Controls.Add(this.labelTotalReorgStats);
            this.flowLayoutPanel7.Location = new System.Drawing.Point(447, 395);
            this.flowLayoutPanel7.Name = "flowLayoutPanel7";
            this.flowLayoutPanel7.Size = new System.Drawing.Size(165, 13);
            this.flowLayoutPanel7.TabIndex = 71;
            // 
            // labelTotalReorgCount
            // 
            this.labelTotalReorgCount.AutoSize = true;
            this.labelTotalReorgCount.Location = new System.Drawing.Point(0, 0);
            this.labelTotalReorgCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalReorgCount.Name = "labelTotalReorgCount";
            this.labelTotalReorgCount.Size = new System.Drawing.Size(13, 13);
            this.labelTotalReorgCount.TabIndex = 63;
            this.labelTotalReorgCount.Text = "2";
            // 
            // labelTotalReorgStats
            // 
            this.labelTotalReorgStats.AutoSize = true;
            this.labelTotalReorgStats.ForeColor = System.Drawing.Color.Maroon;
            this.labelTotalReorgStats.Location = new System.Drawing.Point(13, 0);
            this.labelTotalReorgStats.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalReorgStats.Name = "labelTotalReorgStats";
            this.labelTotalReorgStats.Size = new System.Drawing.Size(117, 13);
            this.labelTotalReorgStats.TabIndex = 65;
            this.labelTotalReorgStats.Text = "(longest reorg 2 blocks)";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label57.Location = new System.Drawing.Point(362, 395);
            this.label57.Margin = new System.Windows.Forms.Padding(3);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(84, 13);
            this.label57.TabIndex = 70;
            this.label57.Text = "Total Reorgs:";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Controls.Add(this.labelTotalBlockMinedCount);
            this.flowLayoutPanel6.Controls.Add(this.labelTotalBlockMinedStats);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(129, 395);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(165, 13);
            this.flowLayoutPanel6.TabIndex = 69;
            // 
            // labelTotalBlockMinedCount
            // 
            this.labelTotalBlockMinedCount.AutoSize = true;
            this.labelTotalBlockMinedCount.Location = new System.Drawing.Point(0, 0);
            this.labelTotalBlockMinedCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalBlockMinedCount.Name = "labelTotalBlockMinedCount";
            this.labelTotalBlockMinedCount.Size = new System.Drawing.Size(19, 13);
            this.labelTotalBlockMinedCount.TabIndex = 63;
            this.labelTotalBlockMinedCount.Text = "15";
            // 
            // labelTotalBlockMinedStats
            // 
            this.labelTotalBlockMinedStats.AutoSize = true;
            this.labelTotalBlockMinedStats.ForeColor = System.Drawing.Color.Maroon;
            this.labelTotalBlockMinedStats.Location = new System.Drawing.Point(19, 0);
            this.labelTotalBlockMinedStats.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalBlockMinedStats.Name = "labelTotalBlockMinedStats";
            this.labelTotalBlockMinedStats.Size = new System.Drawing.Size(123, 13);
            this.labelTotalBlockMinedStats.TabIndex = 64;
            this.labelTotalBlockMinedStats.Text = "(12% of all blocks mined)";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label52.Location = new System.Drawing.Point(9, 395);
            this.label52.Margin = new System.Windows.Forms.Padding(3);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(114, 13);
            this.label52.TabIndex = 68;
            this.label52.Text = "Total Block Mined:";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Controls.Add(this.labelLastReorgBlockHeight);
            this.flowLayoutPanel5.Controls.Add(this.labelLastReorgTimeAgo);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(447, 376);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(165, 13);
            this.flowLayoutPanel5.TabIndex = 67;
            // 
            // labelLastReorgBlockHeight
            // 
            this.labelLastReorgBlockHeight.AutoSize = true;
            this.labelLastReorgBlockHeight.Location = new System.Drawing.Point(0, 0);
            this.labelLastReorgBlockHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastReorgBlockHeight.Name = "labelLastReorgBlockHeight";
            this.labelLastReorgBlockHeight.Size = new System.Drawing.Size(43, 13);
            this.labelLastReorgBlockHeight.TabIndex = 63;
            this.labelLastReorgBlockHeight.Text = "847001";
            // 
            // labelLastReorgTimeAgo
            // 
            this.labelLastReorgTimeAgo.AutoSize = true;
            this.labelLastReorgTimeAgo.ForeColor = System.Drawing.Color.Maroon;
            this.labelLastReorgTimeAgo.Location = new System.Drawing.Point(43, 0);
            this.labelLastReorgTimeAgo.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastReorgTimeAgo.Name = "labelLastReorgTimeAgo";
            this.labelLastReorgTimeAgo.Size = new System.Drawing.Size(75, 13);
            this.labelLastReorgTimeAgo.TabIndex = 64;
            this.labelLastReorgTimeAgo.Text = "(12 hours ago)";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.Location = new System.Drawing.Point(373, 376);
            this.label51.Margin = new System.Windows.Forms.Padding(3);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(73, 13);
            this.label51.TabIndex = 66;
            this.label51.Text = "Last Reorg:";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.labelLastMinedBlockHeight);
            this.flowLayoutPanel4.Controls.Add(this.labelLastMinedTimeAgo);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(129, 376);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(165, 13);
            this.flowLayoutPanel4.TabIndex = 65;
            // 
            // labelLastMinedBlockHeight
            // 
            this.labelLastMinedBlockHeight.AutoSize = true;
            this.labelLastMinedBlockHeight.Location = new System.Drawing.Point(0, 0);
            this.labelLastMinedBlockHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastMinedBlockHeight.Name = "labelLastMinedBlockHeight";
            this.labelLastMinedBlockHeight.Size = new System.Drawing.Size(43, 13);
            this.labelLastMinedBlockHeight.TabIndex = 63;
            this.labelLastMinedBlockHeight.Text = "847001";
            // 
            // labelLastMinedTimeAgo
            // 
            this.labelLastMinedTimeAgo.AutoSize = true;
            this.labelLastMinedTimeAgo.ForeColor = System.Drawing.Color.Maroon;
            this.labelLastMinedTimeAgo.Location = new System.Drawing.Point(43, 0);
            this.labelLastMinedTimeAgo.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastMinedTimeAgo.Name = "labelLastMinedTimeAgo";
            this.labelLastMinedTimeAgo.Size = new System.Drawing.Size(75, 13);
            this.labelLastMinedTimeAgo.TabIndex = 64;
            this.labelLastMinedTimeAgo.Text = "(12 hours ago)";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.Location = new System.Drawing.Point(14, 376);
            this.label47.Margin = new System.Windows.Forms.Padding(3);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(109, 13);
            this.label47.TabIndex = 62;
            this.label47.Text = "Last Block Mined:";
            // 
            // labelNetworkHeight
            // 
            this.labelNetworkHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNetworkHeight.Location = new System.Drawing.Point(508, 196);
            this.labelNetworkHeight.Margin = new System.Windows.Forms.Padding(3);
            this.labelNetworkHeight.Name = "labelNetworkHeight";
            this.labelNetworkHeight.Size = new System.Drawing.Size(110, 13);
            this.labelNetworkHeight.TabIndex = 61;
            this.labelNetworkHeight.Text = "847011";
            this.labelNetworkHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWalletHeight
            // 
            this.labelWalletHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWalletHeight.Location = new System.Drawing.Point(389, 196);
            this.labelWalletHeight.Margin = new System.Windows.Forms.Padding(3);
            this.labelWalletHeight.Name = "labelWalletHeight";
            this.labelWalletHeight.Size = new System.Drawing.Size(110, 13);
            this.labelWalletHeight.TabIndex = 59;
            this.labelWalletHeight.Text = "847011";
            this.labelWalletHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label44
            // 
            this.label44.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label44.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label44.Location = new System.Drawing.Point(503, 180);
            this.label44.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(2, 29);
            this.label44.TabIndex = 58;
            // 
            // labelBlockHeight
            // 
            this.labelBlockHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBlockHeight.Location = new System.Drawing.Point(269, 196);
            this.labelBlockHeight.Margin = new System.Windows.Forms.Padding(3);
            this.labelBlockHeight.Name = "labelBlockHeight";
            this.labelBlockHeight.Size = new System.Drawing.Size(110, 13);
            this.labelBlockHeight.TabIndex = 57;
            this.labelBlockHeight.Text = "847011";
            this.labelBlockHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label42
            // 
            this.label42.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label42.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label42.Location = new System.Drawing.Point(383, 180);
            this.label42.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(2, 29);
            this.label42.TabIndex = 56;
            // 
            // labelConsensusHeight
            // 
            this.labelConsensusHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConsensusHeight.Location = new System.Drawing.Point(149, 196);
            this.labelConsensusHeight.Margin = new System.Windows.Forms.Padding(3);
            this.labelConsensusHeight.Name = "labelConsensusHeight";
            this.labelConsensusHeight.Size = new System.Drawing.Size(110, 13);
            this.labelConsensusHeight.TabIndex = 55;
            this.labelConsensusHeight.Text = "847011";
            this.labelConsensusHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label40.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label40.Location = new System.Drawing.Point(264, 180);
            this.label40.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(2, 29);
            this.label40.TabIndex = 54;
            // 
            // labelHeaderHeight
            // 
            this.labelHeaderHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeaderHeight.Location = new System.Drawing.Point(29, 196);
            this.labelHeaderHeight.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeaderHeight.Name = "labelHeaderHeight";
            this.labelHeaderHeight.Size = new System.Drawing.Size(110, 13);
            this.labelHeaderHeight.TabIndex = 53;
            this.labelHeaderHeight.Text = "847011";
            this.labelHeaderHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label37.Location = new System.Drawing.Point(143, 180);
            this.label37.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(2, 29);
            this.label37.TabIndex = 52;
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label36.Location = new System.Drawing.Point(11, 365);
            this.label36.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(606, 2);
            this.label36.TabIndex = 51;
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(508, 178);
            this.label35.Margin = new System.Windows.Forms.Padding(3);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(110, 13);
            this.label35.TabIndex = 50;
            this.label35.Text = "Network Height";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(389, 178);
            this.label34.Margin = new System.Windows.Forms.Padding(3);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(110, 13);
            this.label34.TabIndex = 49;
            this.label34.Text = "Wallet Height";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(269, 178);
            this.label33.Margin = new System.Windows.Forms.Padding(3);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(110, 13);
            this.label33.TabIndex = 48;
            this.label33.Text = "Block Height";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(149, 178);
            this.label32.Margin = new System.Windows.Forms.Padding(3);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(110, 13);
            this.label32.TabIndex = 47;
            this.label32.Text = "Consensus Height";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label31
            // 
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(29, 178);
            this.label31.Margin = new System.Windows.Forms.Padding(3);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(110, 13);
            this.label31.TabIndex = 46;
            this.label31.Text = "Header Height";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label30.Location = new System.Drawing.Point(17, 173);
            this.label30.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(606, 2);
            this.label30.TabIndex = 45;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.labelDataDir);
            this.flowLayoutPanel3.Controls.Add(this.labelDataDirSize);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(125, 92);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(499, 13);
            this.flowLayoutPanel3.TabIndex = 44;
            // 
            // labelDataDir
            // 
            this.labelDataDir.AutoSize = true;
            this.labelDataDir.Location = new System.Drawing.Point(0, 0);
            this.labelDataDir.Margin = new System.Windows.Forms.Padding(0);
            this.labelDataDir.Name = "labelDataDir";
            this.labelDataDir.Size = new System.Drawing.Size(305, 13);
            this.labelDataDir.TabIndex = 41;
            this.labelDataDir.Text = "C:\\Code\\NodeData\\NodeCommanderNode\\Stratis\\StratisMain";
            // 
            // labelDataDirSize
            // 
            this.labelDataDirSize.AutoSize = true;
            this.labelDataDirSize.ForeColor = System.Drawing.Color.Maroon;
            this.labelDataDirSize.Location = new System.Drawing.Point(305, 0);
            this.labelDataDirSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelDataDirSize.Name = "labelDataDirSize";
            this.labelDataDirSize.Size = new System.Drawing.Size(46, 13);
            this.labelDataDirSize.TabIndex = 43;
            this.labelDataDirSize.Text = "(129Mb)";
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label28.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label28.Location = new System.Drawing.Point(18, 82);
            this.label28.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(606, 2);
            this.label28.TabIndex = 42;
            // 
            // labelDaemonName
            // 
            this.labelDaemonName.AutoSize = true;
            this.labelDaemonName.Location = new System.Drawing.Point(125, 111);
            this.labelDaemonName.Margin = new System.Windows.Forms.Padding(0);
            this.labelDaemonName.Name = "labelDaemonName";
            this.labelDaemonName.Size = new System.Drawing.Size(76, 13);
            this.labelDaemonName.TabIndex = 40;
            this.labelDaemonName.Text = "Stratis.StratisD";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.labelCurrentBranch);
            this.flowLayoutPanel2.Controls.Add(this.labelNumberOfCommitsBehind);
            this.flowLayoutPanel2.Controls.Add(this.linkLabelPullCurrentBranch);
            this.flowLayoutPanel2.Controls.Add(this.linkLabelSwitchBranch);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(106, 25);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(518, 13);
            this.flowLayoutPanel2.TabIndex = 39;
            // 
            // labelCurrentBranch
            // 
            this.labelCurrentBranch.AutoSize = true;
            this.labelCurrentBranch.Location = new System.Drawing.Point(0, 0);
            this.labelCurrentBranch.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrentBranch.Name = "labelCurrentBranch";
            this.labelCurrentBranch.Size = new System.Drawing.Size(38, 13);
            this.labelCurrentBranch.TabIndex = 30;
            this.labelCurrentBranch.Text = "master";
            // 
            // labelNumberOfCommitsBehind
            // 
            this.labelNumberOfCommitsBehind.AutoSize = true;
            this.labelNumberOfCommitsBehind.ForeColor = System.Drawing.Color.Maroon;
            this.labelNumberOfCommitsBehind.Location = new System.Drawing.Point(38, 0);
            this.labelNumberOfCommitsBehind.Margin = new System.Windows.Forms.Padding(0);
            this.labelNumberOfCommitsBehind.Name = "labelNumberOfCommitsBehind";
            this.labelNumberOfCommitsBehind.Size = new System.Drawing.Size(60, 13);
            this.labelNumberOfCommitsBehind.TabIndex = 38;
            this.labelNumberOfCommitsBehind.Text = "[22 behind]";
            // 
            // linkLabelPullCurrentBranch
            // 
            this.linkLabelPullCurrentBranch.Location = new System.Drawing.Point(101, 0);
            this.linkLabelPullCurrentBranch.Name = "linkLabelPullCurrentBranch";
            this.linkLabelPullCurrentBranch.Size = new System.Drawing.Size(28, 15);
            this.linkLabelPullCurrentBranch.TabIndex = 72;
            this.linkLabelPullCurrentBranch.TabStop = true;
            this.linkLabelPullCurrentBranch.Text = "Pull";
            this.linkLabelPullCurrentBranch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPullCurrentBranch_LinkClicked);
            // 
            // linkLabelSwitchBranch
            // 
            this.linkLabelSwitchBranch.Location = new System.Drawing.Point(135, 0);
            this.linkLabelSwitchBranch.Name = "linkLabelSwitchBranch";
            this.linkLabelSwitchBranch.Size = new System.Drawing.Size(83, 15);
            this.linkLabelSwitchBranch.TabIndex = 73;
            this.linkLabelSwitchBranch.TabStop = true;
            this.linkLabelSwitchBranch.Text = "Switch Branch";
            // 
            // labelLastUpdateMessage
            // 
            this.labelLastUpdateMessage.ForeColor = System.Drawing.Color.Gray;
            this.labelLastUpdateMessage.Location = new System.Drawing.Point(106, 63);
            this.labelLastUpdateMessage.Margin = new System.Windows.Forms.Padding(3);
            this.labelLastUpdateMessage.Name = "labelLastUpdateMessage";
            this.labelLastUpdateMessage.Size = new System.Drawing.Size(512, 13);
            this.labelLastUpdateMessage.TabIndex = 37;
            this.labelLastUpdateMessage.Text = "Cleanup integration test folder creation part 2 (#1372)";
            this.toolTipHelp.SetToolTip(this.labelLastUpdateMessage, "Zonk");
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.labelLastUpdateDate);
            this.flowLayoutPanel1.Controls.Add(this.labelLastUpdateTime);
            this.flowLayoutPanel1.Controls.Add(this.labelLastUpdateTimeAgo);
            this.flowLayoutPanel1.Controls.Add(this.label23);
            this.flowLayoutPanel1.Controls.Add(this.labelLastUpdateAuthor);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(106, 44);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(517, 13);
            this.flowLayoutPanel1.TabIndex = 36;
            // 
            // labelLastUpdateDate
            // 
            this.labelLastUpdateDate.AutoSize = true;
            this.labelLastUpdateDate.Location = new System.Drawing.Point(0, 0);
            this.labelLastUpdateDate.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastUpdateDate.Name = "labelLastUpdateDate";
            this.labelLastUpdateDate.Size = new System.Drawing.Size(69, 13);
            this.labelLastUpdateDate.TabIndex = 31;
            this.labelLastUpdateDate.Text = "28 May 2018";
            // 
            // labelLastUpdateTime
            // 
            this.labelLastUpdateTime.AutoSize = true;
            this.labelLastUpdateTime.Location = new System.Drawing.Point(69, 0);
            this.labelLastUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastUpdateTime.Name = "labelLastUpdateTime";
            this.labelLastUpdateTime.Size = new System.Drawing.Size(34, 13);
            this.labelLastUpdateTime.TabIndex = 32;
            this.labelLastUpdateTime.Text = "12:12";
            // 
            // labelLastUpdateTimeAgo
            // 
            this.labelLastUpdateTimeAgo.AutoSize = true;
            this.labelLastUpdateTimeAgo.ForeColor = System.Drawing.Color.Maroon;
            this.labelLastUpdateTimeAgo.Location = new System.Drawing.Point(103, 0);
            this.labelLastUpdateTimeAgo.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastUpdateTimeAgo.Name = "labelLastUpdateTimeAgo";
            this.labelLastUpdateTimeAgo.Size = new System.Drawing.Size(75, 13);
            this.labelLastUpdateTimeAgo.TabIndex = 33;
            this.labelLastUpdateTimeAgo.Text = "(12 hours ago)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(178, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(18, 13);
            this.label23.TabIndex = 35;
            this.label23.Text = "by";
            // 
            // labelLastUpdateAuthor
            // 
            this.labelLastUpdateAuthor.AutoSize = true;
            this.labelLastUpdateAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastUpdateAuthor.Location = new System.Drawing.Point(196, 0);
            this.labelLastUpdateAuthor.Margin = new System.Windows.Forms.Padding(0);
            this.labelLastUpdateAuthor.Name = "labelLastUpdateAuthor";
            this.labelLastUpdateAuthor.Size = new System.Drawing.Size(119, 13);
            this.labelLastUpdateAuthor.TabIndex = 34;
            this.labelLastUpdateAuthor.Text = "Francois de la Rouviere";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(20, 44);
            this.label17.Margin = new System.Windows.Forms.Padding(3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(80, 13);
            this.label17.TabIndex = 29;
            this.label17.Text = "Last Update:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(49, 25);
            this.label16.Margin = new System.Windows.Forms.Padding(3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(51, 13);
            this.label16.TabIndex = 28;
            this.label16.Text = "Branch:";
            // 
            // linkLabelRepositoryUrl
            // 
            this.linkLabelRepositoryUrl.Location = new System.Drawing.Point(106, 7);
            this.linkLabelRepositoryUrl.Name = "linkLabelRepositoryUrl";
            this.linkLabelRepositoryUrl.Size = new System.Drawing.Size(518, 15);
            this.linkLabelRepositoryUrl.TabIndex = 27;
            this.linkLabelRepositoryUrl.TabStop = true;
            this.linkLabelRepositoryUrl.Text = "https://github.com/stratisproject/StratisBitcoinFullNode.git";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(62, 111);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Daemon:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(61, 92);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Data Dir:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(29, 7);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Repository:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.buttonDeployFiles);
            this.groupBox4.Controls.Add(this.buttonClearData);
            this.groupBox4.Controls.Add(this.buttonStartNode);
            this.groupBox4.Controls.Add(this.buttonStopNode);
            this.groupBox4.Controls.Add(this.buttonRestartNode);
            this.groupBox4.Location = new System.Drawing.Point(3, 609);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(630, 58);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Node Management";
            // 
            // buttonDeployFiles
            // 
            this.buttonDeployFiles.Location = new System.Drawing.Point(506, 21);
            this.buttonDeployFiles.Name = "buttonDeployFiles";
            this.buttonDeployFiles.Size = new System.Drawing.Size(111, 23);
            this.buttonDeployFiles.TabIndex = 9;
            this.buttonDeployFiles.Text = "Deploy Files";
            this.buttonDeployFiles.UseVisualStyleBackColor = true;
            this.buttonDeployFiles.Click += new System.EventHandler(this.buttonDeployFiles_Click);
            // 
            // buttonClearData
            // 
            this.buttonClearData.Location = new System.Drawing.Point(389, 21);
            this.buttonClearData.Name = "buttonClearData";
            this.buttonClearData.Size = new System.Drawing.Size(111, 23);
            this.buttonClearData.TabIndex = 7;
            this.buttonClearData.Text = "Clear Data";
            this.buttonClearData.UseVisualStyleBackColor = true;
            this.buttonClearData.Click += new System.EventHandler(this.button10_Click);
            // 
            // buttonStartNode
            // 
            this.buttonStartNode.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonStartNode.Location = new System.Drawing.Point(6, 21);
            this.buttonStartNode.Name = "buttonStartNode";
            this.buttonStartNode.Size = new System.Drawing.Size(103, 23);
            this.buttonStartNode.TabIndex = 0;
            this.buttonStartNode.Text = "Start node";
            this.buttonStartNode.UseVisualStyleBackColor = false;
            this.buttonStartNode.Click += new System.EventHandler(this.button4_Click);
            // 
            // buttonStopNode
            // 
            this.buttonStopNode.BackColor = System.Drawing.Color.LightCoral;
            this.buttonStopNode.Location = new System.Drawing.Point(115, 21);
            this.buttonStopNode.Name = "buttonStopNode";
            this.buttonStopNode.Size = new System.Drawing.Size(103, 23);
            this.buttonStopNode.TabIndex = 1;
            this.buttonStopNode.Text = "Stop node";
            this.buttonStopNode.UseVisualStyleBackColor = false;
            this.buttonStopNode.Click += new System.EventHandler(this.button5_Click);
            // 
            // buttonRestartNode
            // 
            this.buttonRestartNode.Location = new System.Drawing.Point(224, 21);
            this.buttonRestartNode.Name = "buttonRestartNode";
            this.buttonRestartNode.Size = new System.Drawing.Size(103, 23);
            this.buttonRestartNode.TabIndex = 5;
            this.buttonRestartNode.Text = "Restart";
            this.buttonRestartNode.UseVisualStyleBackColor = true;
            // 
            // tabPageGit
            // 
            this.tabPageGit.Location = new System.Drawing.Point(4, 22);
            this.tabPageGit.Name = "tabPageGit";
            this.tabPageGit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGit.Size = new System.Drawing.Size(642, 670);
            this.tabPageGit.TabIndex = 1;
            this.tabPageGit.Text = "Git";
            this.tabPageGit.UseVisualStyleBackColor = true;
            // 
            // tabPageNodeConfig
            // 
            this.tabPageNodeConfig.Controls.Add(this.textBoxCodeDirectory);
            this.tabPageNodeConfig.Controls.Add(this.textBoxNetworkDirectory);
            this.tabPageNodeConfig.Controls.Add(this.label4);
            this.tabPageNodeConfig.Controls.Add(this.label7);
            this.tabPageNodeConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageNodeConfig.Name = "tabPageNodeConfig";
            this.tabPageNodeConfig.Size = new System.Drawing.Size(642, 670);
            this.tabPageNodeConfig.TabIndex = 2;
            this.tabPageNodeConfig.Text = "Node Config";
            this.tabPageNodeConfig.UseVisualStyleBackColor = true;
            // 
            // textBoxCodeDirectory
            // 
            this.textBoxCodeDirectory.Location = new System.Drawing.Point(127, 15);
            this.textBoxCodeDirectory.Name = "textBoxCodeDirectory";
            this.textBoxCodeDirectory.Size = new System.Drawing.Size(271, 20);
            this.textBoxCodeDirectory.TabIndex = 1;
            // 
            // textBoxNetworkDirectory
            // 
            this.textBoxNetworkDirectory.Location = new System.Drawing.Point(127, 41);
            this.textBoxNetworkDirectory.Name = "textBoxNetworkDirectory";
            this.textBoxNetworkDirectory.Size = new System.Drawing.Size(271, 20);
            this.textBoxNetworkDirectory.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Code Directory:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Network Directory:";
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.dataGridViewAgents);
            this.groupBox9.Location = new System.Drawing.Point(5, 705);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(647, 117);
            this.groupBox9.TabIndex = 2;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Agents";
            // 
            // dataGridViewAgents
            // 
            this.dataGridViewAgents.AllowUserToAddRows = false;
            this.dataGridViewAgents.AllowUserToDeleteRows = false;
            this.dataGridViewAgents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewAgents.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAgents.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewAgents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAgents.ContextMenuStrip = this.contextMenuStripAgents;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewAgents.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAgents.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewAgents.MultiSelect = false;
            this.dataGridViewAgents.Name = "dataGridViewAgents";
            this.dataGridViewAgents.ReadOnly = true;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAgents.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewAgents.RowHeadersVisible = false;
            this.dataGridViewAgents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAgents.Size = new System.Drawing.Size(641, 98);
            this.dataGridViewAgents.TabIndex = 8;
            this.dataGridViewAgents.Filter += new System.Action<System.Data.DataView>(this.dataGridViewAgents_Filter);
            // 
            // contextMenuStripAgents
            // 
            this.contextMenuStripAgents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAgentsNodesToolStripMenuItem,
            this.toolStripMenuItem5,
            this.startTorToolStripMenuItem,
            this.stopTorToolStripMenuItem});
            this.contextMenuStripAgents.Name = "contextMenuStripAgents";
            this.contextMenuStripAgents.Size = new System.Drawing.Size(184, 76);
            // 
            // showAgentsNodesToolStripMenuItem
            // 
            this.showAgentsNodesToolStripMenuItem.Name = "showAgentsNodesToolStripMenuItem";
            this.showAgentsNodesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.showAgentsNodesToolStripMenuItem.Text = "Show Agent\'s Nodes";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(180, 6);
            // 
            // startTorToolStripMenuItem
            // 
            this.startTorToolStripMenuItem.Name = "startTorToolStripMenuItem";
            this.startTorToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.startTorToolStripMenuItem.Text = "Start Tor";
            // 
            // stopTorToolStripMenuItem
            // 
            this.stopTorToolStripMenuItem.Name = "stopTorToolStripMenuItem";
            this.stopTorToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.stopTorToolStripMenuItem.Text = "Stop Tor";
            // 
            // tabPageGitExpert
            // 
            this.tabPageGitExpert.Controls.Add(this.groupBox3);
            this.tabPageGitExpert.Controls.Add(this.groupBox1);
            this.tabPageGitExpert.Location = new System.Drawing.Point(4, 22);
            this.tabPageGitExpert.Name = "tabPageGitExpert";
            this.tabPageGitExpert.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGitExpert.Size = new System.Drawing.Size(1564, 831);
            this.tabPageGitExpert.TabIndex = 1;
            this.tabPageGitExpert.Text = "GIT";
            this.tabPageGitExpert.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(6, 75);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(861, 63);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Full Node UI";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(264, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Get Code";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Pull Request Id:";
            // 
            // tabPageControlPanel
            // 
            this.tabPageControlPanel.Controls.Add(this.groupBox2);
            this.tabPageControlPanel.Location = new System.Drawing.Point(4, 22);
            this.tabPageControlPanel.Name = "tabPageControlPanel";
            this.tabPageControlPanel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageControlPanel.Size = new System.Drawing.Size(1564, 831);
            this.tabPageControlPanel.TabIndex = 2;
            this.tabPageControlPanel.Text = "Control Panel";
            this.tabPageControlPanel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(646, 337);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 61);
            this.groupBox2.TabIndex = 44;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Control Panel";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 2);
            this.label2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start Tor";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tabPageBlockchain
            // 
            this.tabPageBlockchain.Controls.Add(this.groupBox10);
            this.tabPageBlockchain.Location = new System.Drawing.Point(4, 22);
            this.tabPageBlockchain.Name = "tabPageBlockchain";
            this.tabPageBlockchain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBlockchain.Size = new System.Drawing.Size(1564, 831);
            this.tabPageBlockchain.TabIndex = 3;
            this.tabPageBlockchain.Text = "Blockchain";
            this.tabPageBlockchain.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.dataGridViewBlockchain);
            this.groupBox10.Location = new System.Drawing.Point(6, 6);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(580, 322);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Blocks";
            // 
            // dataGridViewBlockchain
            // 
            this.dataGridViewBlockchain.AllowUserToAddRows = false;
            this.dataGridViewBlockchain.AllowUserToDeleteRows = false;
            this.dataGridViewBlockchain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewBlockchain.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBlockchain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dataGridViewBlockchain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewBlockchain.DefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridViewBlockchain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBlockchain.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewBlockchain.MultiSelect = false;
            this.dataGridViewBlockchain.Name = "dataGridViewBlockchain";
            this.dataGridViewBlockchain.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewBlockchain.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dataGridViewBlockchain.RowHeadersVisible = false;
            this.dataGridViewBlockchain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBlockchain.Size = new System.Drawing.Size(574, 303);
            this.dataGridViewBlockchain.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1564, 831);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Wallet Management";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripPeers
            // 
            this.contextMenuStripPeers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripMenuItem,
            this.removeNodeToolStripMenuItem,
            this.toolStripMenuItem6,
            this.banNodeToolStripMenuItem,
            this.unbanNodeToolStripMenuItem});
            this.contextMenuStripPeers.Name = "contextMenuStripPeers";
            this.contextMenuStripPeers.Size = new System.Drawing.Size(150, 98);
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.addNodeToolStripMenuItem.Text = "Add Node";
            // 
            // removeNodeToolStripMenuItem
            // 
            this.removeNodeToolStripMenuItem.Name = "removeNodeToolStripMenuItem";
            this.removeNodeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.removeNodeToolStripMenuItem.Text = "Remove Node";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(146, 6);
            // 
            // banNodeToolStripMenuItem
            // 
            this.banNodeToolStripMenuItem.Name = "banNodeToolStripMenuItem";
            this.banNodeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.banNodeToolStripMenuItem.Text = "Ban Node";
            // 
            // unbanNodeToolStripMenuItem
            // 
            this.unbanNodeToolStripMenuItem.Name = "unbanNodeToolStripMenuItem";
            this.unbanNodeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.unbanNodeToolStripMenuItem.Text = "Unban Node";
            // 
            // contextMenuStripMemoryPool
            // 
            this.contextMenuStripMemoryPool.Name = "contextMenuStripMemoryPool";
            this.contextMenuStripMemoryPool.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStripApplicationMain
            // 
            this.menuStripApplicationMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.editToolStripMenuItem,
            this.monitoringToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripApplicationMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripApplicationMain.Name = "menuStripApplicationMain";
            this.menuStripApplicationMain.Size = new System.Drawing.Size(1572, 24);
            this.menuStripApplicationMain.TabIndex = 4;
            this.menuStripApplicationMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.networkToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // networkToolStripMenuItem
            // 
            this.networkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainnetToolStripMenuItem,
            this.testnetToolStripMenuItem});
            this.networkToolStripMenuItem.Image = global::Stratis.NodeCommander.Properties.Resources.environment;
            this.networkToolStripMenuItem.Name = "networkToolStripMenuItem";
            this.networkToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.networkToolStripMenuItem.Text = "Network";
            // 
            // mainnetToolStripMenuItem
            // 
            this.mainnetToolStripMenuItem.Name = "mainnetToolStripMenuItem";
            this.mainnetToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.mainnetToolStripMenuItem.Text = "Mainnet";
            // 
            // testnetToolStripMenuItem
            // 
            this.testnetToolStripMenuItem.Name = "testnetToolStripMenuItem";
            this.testnetToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.testnetToolStripMenuItem.Text = "Testnet";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(116, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::Stratis.NodeCommander.Properties.Resources.exit;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.fullRefreshToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Image = global::Stratis.NodeCommander.Properties.Resources.arrow_refresh;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // fullRefreshToolStripMenuItem
            // 
            this.fullRefreshToolStripMenuItem.Name = "fullRefreshToolStripMenuItem";
            this.fullRefreshToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.fullRefreshToolStripMenuItem.Text = "Full Refresh";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // monitoringToolStripMenuItem
            // 
            this.monitoringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resumeMonitoringToolStripMenuItem,
            this.suspendMonitoringToolStripMenuItem,
            this.toolStripMenuItem2,
            this.notifyAboutNewExceptionsToolStripMenuItem,
            this.notifyAboutPerformanceIssuesToolStripMenuItem});
            this.monitoringToolStripMenuItem.Name = "monitoringToolStripMenuItem";
            this.monitoringToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.monitoringToolStripMenuItem.Text = "Monitoring";
            // 
            // resumeMonitoringToolStripMenuItem
            // 
            this.resumeMonitoringToolStripMenuItem.Image = global::Stratis.NodeCommander.Properties.Resources.play;
            this.resumeMonitoringToolStripMenuItem.Name = "resumeMonitoringToolStripMenuItem";
            this.resumeMonitoringToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.resumeMonitoringToolStripMenuItem.Text = "Resume Monitoring";
            // 
            // suspendMonitoringToolStripMenuItem
            // 
            this.suspendMonitoringToolStripMenuItem.Image = global::Stratis.NodeCommander.Properties.Resources.pause;
            this.suspendMonitoringToolStripMenuItem.Name = "suspendMonitoringToolStripMenuItem";
            this.suspendMonitoringToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.suspendMonitoringToolStripMenuItem.Text = "Suspend Monitoring";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(243, 6);
            // 
            // notifyAboutNewExceptionsToolStripMenuItem
            // 
            this.notifyAboutNewExceptionsToolStripMenuItem.Checked = true;
            this.notifyAboutNewExceptionsToolStripMenuItem.CheckOnClick = true;
            this.notifyAboutNewExceptionsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.notifyAboutNewExceptionsToolStripMenuItem.Name = "notifyAboutNewExceptionsToolStripMenuItem";
            this.notifyAboutNewExceptionsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.notifyAboutNewExceptionsToolStripMenuItem.Text = "Notify about new exceptions";
            // 
            // notifyAboutPerformanceIssuesToolStripMenuItem
            // 
            this.notifyAboutPerformanceIssuesToolStripMenuItem.Checked = true;
            this.notifyAboutPerformanceIssuesToolStripMenuItem.CheckOnClick = true;
            this.notifyAboutPerformanceIssuesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.notifyAboutPerformanceIssuesToolStripMenuItem.Name = "notifyAboutPerformanceIssuesToolStripMenuItem";
            this.notifyAboutPerformanceIssuesToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
            this.notifyAboutPerformanceIssuesToolStripMenuItem.Text = "Notify about performance issues";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // statusStripApplicationMain
            // 
            this.statusStripApplicationMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelNodeState,
            this.toolStripStatusLabelDatabase,
            this.toolStripStatusLabelWorker});
            this.statusStripApplicationMain.Location = new System.Drawing.Point(0, 888);
            this.statusStripApplicationMain.Name = "statusStripApplicationMain";
            this.statusStripApplicationMain.Size = new System.Drawing.Size(1572, 24);
            this.statusStripApplicationMain.TabIndex = 7;
            this.statusStripApplicationMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1282, 19);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripStatusLabelNodeState
            // 
            this.toolStripStatusLabelNodeState.AutoSize = false;
            this.toolStripStatusLabelNodeState.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelNodeState.Name = "toolStripStatusLabelNodeState";
            this.toolStripStatusLabelNodeState.Size = new System.Drawing.Size(100, 19);
            this.toolStripStatusLabelNodeState.Text = "Nodes: 0 / 0";
            // 
            // toolStripStatusLabelDatabase
            // 
            this.toolStripStatusLabelDatabase.ActiveLinkColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelDatabase.AutoSize = false;
            this.toolStripStatusLabelDatabase.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelDatabase.Name = "toolStripStatusLabelDatabase";
            this.toolStripStatusLabelDatabase.Size = new System.Drawing.Size(100, 19);
            this.toolStripStatusLabelDatabase.Text = "DB: 0Mb";
            // 
            // toolStripStatusLabelWorker
            // 
            this.toolStripStatusLabelWorker.AutoSize = false;
            this.toolStripStatusLabelWorker.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabelWorker.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelWorker.Name = "toolStripStatusLabelWorker";
            this.toolStripStatusLabelWorker.Size = new System.Drawing.Size(75, 19);
            this.toolStripStatusLabelWorker.Text = "Init";
            // 
            // contextMenuStripClearData
            // 
            this.contextMenuStripClearData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.advancedToolStripMenuItem,
            this.toolStripMenuItem3,
            this.mempoolToolStripMenuItem,
            this.peersToolStripMenuItem,
            this.logsToolStripMenuItem});
            this.contextMenuStripClearData.Name = "contextMenuStrip1";
            this.contextMenuStripClearData.Size = new System.Drawing.Size(130, 98);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.advancedToolStripMenuItem.Text = "Advanced";
            this.advancedToolStripMenuItem.Click += new System.EventHandler(this.advancedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(126, 6);
            // 
            // mempoolToolStripMenuItem
            // 
            this.mempoolToolStripMenuItem.Name = "mempoolToolStripMenuItem";
            this.mempoolToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.mempoolToolStripMenuItem.Text = "Mempool";
            this.mempoolToolStripMenuItem.Click += new System.EventHandler(this.mempoolToolStripMenuItem_Click);
            // 
            // peersToolStripMenuItem
            // 
            this.peersToolStripMenuItem.Name = "peersToolStripMenuItem";
            this.peersToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.peersToolStripMenuItem.Text = "Peers";
            this.peersToolStripMenuItem.Click += new System.EventHandler(this.peersToolStripMenuItem_Click);
            // 
            // logsToolStripMenuItem
            // 
            this.logsToolStripMenuItem.Name = "logsToolStripMenuItem";
            this.logsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.logsToolStripMenuItem.Text = "Logs";
            this.logsToolStripMenuItem.Click += new System.EventHandler(this.logsToolStripMenuItem_Click);
            // 
            // CoinMasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1572, 912);
            this.Controls.Add(this.statusStripApplicationMain);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStripApplicationMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripApplicationMain;
            this.Name = "CoinMasterForm";
            this.Text = "Node Commander";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageNodes.ResumeLayout(false);
            this.splitContainerOuter.Panel1.ResumeLayout(false);
            this.splitContainerOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).EndInit();
            this.splitContainerOuter.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBoxNodeFilter.ResumeLayout(false);
            this.panelNodeFilterEdit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNodes)).EndInit();
            this.contextMenuStripNodeList.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNodeExceptions)).EndInit();
            this.contextMenuStripExceptionList.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            this.tabPageOverview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMempool)).EndInit();
            this.flowLayoutPanel8.ResumeLayout(false);
            this.flowLayoutPanel8.PerformLayout();
            this.flowLayoutPanel7.ResumeLayout(false);
            this.flowLayoutPanel7.PerformLayout();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabPageNodeConfig.ResumeLayout(false);
            this.tabPageNodeConfig.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAgents)).EndInit();
            this.contextMenuStripAgents.ResumeLayout(false);
            this.tabPageGitExpert.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPageControlPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPageBlockchain.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlockchain)).EndInit();
            this.contextMenuStripPeers.ResumeLayout(false);
            this.menuStripApplicationMain.ResumeLayout(false);
            this.menuStripApplicationMain.PerformLayout();
            this.statusStripApplicationMain.ResumeLayout(false);
            this.statusStripApplicationMain.PerformLayout();
            this.contextMenuStripClearData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxPullRequestId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageNodes;
        private System.Windows.Forms.TabPage tabPageGitExpert;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStripApplicationMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonStopNode;
        private System.Windows.Forms.Button buttonStartNode;
        private System.Windows.Forms.TabPage tabPageControlPanel;
        private System.Windows.Forms.StatusStrip statusStripApplicationMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDatabase;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelWorker;
        private System.Windows.Forms.SplitContainer splitContainerOuter;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox5;
        private Stratis.NodeCommander.Controls.NodeExceptions.NodeExceptionsDataGridView dataGridViewNodeExceptions;
        private System.Windows.Forms.ToolStripMenuItem monitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumeMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suspendMonitoringToolStripMenuItem;
        private Stratis.NodeCommander.Controls.NodeOverview.NodeOverviewDataGridView dataGridViewNodes;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem notifyAboutNewExceptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem networkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainnetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testnetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notifyAboutPerformanceIssuesToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxCodeDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox9;
        private Stratis.NodeCommander.Controls.Agents.AgentsDataGridView dataGridViewAgents;
        private System.Windows.Forms.TabPage tabPageBlockchain;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.DataGridView dataGridViewBlockchain;
        private System.Windows.Forms.Button buttonClearData;
        private System.Windows.Forms.Button buttonRestartNode;
        private System.Windows.Forms.Button buttonDeployFiles;
        private System.Windows.Forms.TextBox textBoxNetworkDirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripClearData;
        private System.Windows.Forms.ToolStripMenuItem mempoolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem peersToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.GroupBox groupBoxNodeFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonEditNodeProfile;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelNodeState;
        private System.Windows.Forms.CheckBox checkBoxRunningNodesOnly;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panelNodeFilterEdit;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageOverview;
        private System.Windows.Forms.TabPage tabPageGit;
        private System.Windows.Forms.TabPage tabPageNodeConfig;
        private System.Windows.Forms.Label labelLastUpdateMessage;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelLastUpdateDate;
        private System.Windows.Forms.Label labelLastUpdateTime;
        private System.Windows.Forms.Label labelLastUpdateTimeAgo;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label labelLastUpdateAuthor;
        private System.Windows.Forms.Label labelCurrentBranch;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.LinkLabel linkLabelRepositoryUrl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label labelNumberOfCommitsBehind;
        private System.Windows.Forms.Label labelDataDir;
        private System.Windows.Forms.Label labelDaemonName;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAgents;
        private System.Windows.Forms.ToolStripMenuItem startTorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopTorToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNodeList;
        private System.Windows.Forms.ToolStripMenuItem startNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem clearDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deployFilesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExceptionList;
        private System.Windows.Forms.ToolStripMenuItem showAgentsNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label labelDataDirSize;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label labelNetworkHeight;
        private System.Windows.Forms.Label labelWalletHeight;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label labelBlockHeight;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label labelConsensusHeight;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label labelHeaderHeight;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private System.Windows.Forms.Label labelTotalBlockMinedCount;
        private System.Windows.Forms.Label labelTotalBlockMinedStats;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Label labelLastReorgBlockHeight;
        private System.Windows.Forms.Label labelLastReorgTimeAgo;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label labelLastMinedBlockHeight;
        private System.Windows.Forms.Label labelLastMinedTimeAgo;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
        private System.Windows.Forms.Label labelTotalReorgCount;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.LinkLabel linkLabelPullCurrentBranch;
        private System.Windows.Forms.LinkLabel linkLabelSwitchBranch;
        private System.Windows.Forms.Label labelTotalReorgStats;
        private System.Windows.Forms.ToolTip toolTipHelp;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckedListBox checkedListBox3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
        private System.Windows.Forms.Label labelBlockchainName;
        private System.Windows.Forms.Label labelNetworkName;
        private System.Windows.Forms.Label labelStartupOptions;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelBannedPeers;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label labelInboundPeers;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label labelOutboundPeers;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Button buttonAddNode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPeers;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem banNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unbanNodeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMemoryPool;
        private System.Windows.Forms.ToolStripMenuItem showStacktraceToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonRemoveNode;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logsToolStripMenuItem;
        private Controls.PeerConnections.PeerConnectionsDataGridView dataGridViewPeers;
        private DataGridView dataGridViewMempool;
    }
}

