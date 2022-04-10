namespace Co_Defend_Client_v2.UI
{
    partial class AgentMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentMainForm));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_main = new System.Windows.Forms.TabPage();
            this.messenger1 = new Co_Defend_Client_v2.UI.Messenger();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_stop_hub = new System.Windows.Forms.Button();
            this.lb_agent_type = new System.Windows.Forms.Label();
            this.btn_start_hub = new System.Windows.Forms.Button();
            this.btn_file = new System.Windows.Forms.Button();
            this.lb_uname = new System.Windows.Forms.Label();
            this.tp_tools = new System.Windows.Forms.TabPage();
            this.downloadService1 = new Co_Defend_Client_v2.UI.DownloadService();
            this.tp_plugin = new System.Windows.Forms.TabPage();
            this.servicePlugin1 = new Co_Defend_Client_v2.UI.ServicePlugin();
            this.tp_buildin = new System.Windows.Forms.TabPage();
            this.builtinApps1 = new Co_Defend_Client_v2.UI.BuiltinApps();
            this.tp_uds = new System.Windows.Forms.TabPage();
            this.tp_snort = new System.Windows.Forms.TabPage();
            this.snortAnalyze2 = new Co_Defend_Client_v2.UI.SnortAnalyze();
            this.tp_logout = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tp_hub = new System.Windows.Forms.TabPage();
            this.hubListView1 = new Co_Defend_Client_v2.UI.HubListView();
            this.tp_statistics = new System.Windows.Forms.TabPage();
            this.statistics1 = new Co_Defend_Client_v2.UI.Statistics();
            this.tp_utility = new System.Windows.Forms.TabPage();
            this.command1 = new Co_Defend_Client_v2.UI.Command();
            this.tp_about = new System.Windows.Forms.TabPage();
            this.about1 = new Co_Defend_Client_v2.UI.About();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tp_main.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tp_tools.SuspendLayout();
            this.tp_plugin.SuspendLayout();
            this.tp_buildin.SuspendLayout();
            this.tp_snort.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tp_hub.SuspendLayout();
            this.tp_statistics.SuspendLayout();
            this.tp_utility.SuspendLayout();
            this.tp_about.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "home.ico");
            this.imageList1.Images.SetKeyName(1, "hub.ico");
            this.imageList1.Images.SetKeyName(2, "1366121459_3283.ico");
            this.imageList1.Images.SetKeyName(3, "publish.ico");
            this.imageList1.Images.SetKeyName(4, "command.ico");
            this.imageList1.Images.SetKeyName(5, "plugin.ico");
            this.imageList1.Images.SetKeyName(6, "1366121753_55517.ico");
            this.imageList1.Images.SetKeyName(7, "statistics.ico");
            this.imageList1.Images.SetKeyName(8, "utility.ico");
            this.imageList1.Images.SetKeyName(9, "log.ico");
            this.imageList1.Images.SetKeyName(10, "about.ico");
            this.imageList1.Images.SetKeyName(11, "logout.ico");
            this.imageList1.Images.SetKeyName(12, "exit.ico");
            this.imageList1.Images.SetKeyName(13, "1366121925_10252.ico");
            this.imageList1.Images.SetKeyName(14, "analysis.ico");
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "hub.ico");
            this.imageList2.Images.SetKeyName(1, "log.ico");
            this.imageList2.Images.SetKeyName(2, "statistics.ico");
            this.imageList2.Images.SetKeyName(3, "about.ico");
            this.imageList2.Images.SetKeyName(4, "minus.ico");
            this.imageList2.Images.SetKeyName(5, "plus.ico");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 679);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(889, 23);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStatus
            // 
            this.toolStatus.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStatus.Name = "toolStatus";
            this.toolStatus.Size = new System.Drawing.Size(116, 18);
            this.toolStatus.Text = "Status: Not Login.";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(889, 702);
            this.splitContainer1.SplitterDistance = 466;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Image = global::Co_Defend_Client_v2.Properties.Resources.minus;
            this.button1.Location = new System.Drawing.Point(0, 450);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(16, 16);
            this.button1.TabIndex = 21;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tp_main);
            this.tabControl1.Controls.Add(this.tp_tools);
            this.tabControl1.Controls.Add(this.tp_plugin);
            this.tabControl1.Controls.Add(this.tp_buildin);
            this.tabControl1.Controls.Add(this.tp_uds);
            this.tabControl1.Controls.Add(this.tp_snort);
            this.tabControl1.Controls.Add(this.tp_logout);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.ItemSize = new System.Drawing.Size(104, 38);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(889, 450);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 20;
            // 
            // tp_main
            // 
            this.tp_main.Controls.Add(this.messenger1);
            this.tp_main.Controls.Add(this.groupBox3);
            this.tp_main.ImageIndex = 0;
            this.tp_main.Location = new System.Drawing.Point(4, 42);
            this.tp_main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_main.Name = "tp_main";
            this.tp_main.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tp_main.Size = new System.Drawing.Size(881, 404);
            this.tp_main.TabIndex = 8;
            this.tp_main.Text = "Main Home";
            // 
            // messenger1
            // 
            this.messenger1.Dock = System.Windows.Forms.DockStyle.Top;
            this.messenger1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messenger1.Location = new System.Drawing.Point(0, 74);
            this.messenger1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.messenger1.Name = "messenger1";
            this.messenger1.Size = new System.Drawing.Size(881, 330);
            this.messenger1.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_stop_hub);
            this.groupBox3.Controls.Add(this.lb_agent_type);
            this.groupBox3.Controls.Add(this.btn_start_hub);
            this.groupBox3.Controls.Add(this.btn_file);
            this.groupBox3.Controls.Add(this.lb_uname);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(881, 74);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "User Infomation";
            // 
            // btn_stop_hub
            // 
            this.btn_stop_hub.Location = new System.Drawing.Point(769, 22);
            this.btn_stop_hub.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_stop_hub.Name = "btn_stop_hub";
            this.btn_stop_hub.Size = new System.Drawing.Size(104, 35);
            this.btn_stop_hub.TabIndex = 4;
            this.btn_stop_hub.Text = "Stop Hub";
            this.btn_stop_hub.UseVisualStyleBackColor = true;
            // 
            // lb_agent_type
            // 
            this.lb_agent_type.AutoSize = true;
            this.lb_agent_type.Location = new System.Drawing.Point(282, 32);
            this.lb_agent_type.Name = "lb_agent_type";
            this.lb_agent_type.Size = new System.Drawing.Size(81, 18);
            this.lb_agent_type.TabIndex = 1;
            this.lb_agent_type.Text = "Agent Type:";
            // 
            // btn_start_hub
            // 
            this.btn_start_hub.Location = new System.Drawing.Point(659, 22);
            this.btn_start_hub.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_start_hub.Name = "btn_start_hub";
            this.btn_start_hub.Size = new System.Drawing.Size(104, 35);
            this.btn_start_hub.TabIndex = 3;
            this.btn_start_hub.Text = "Start Hub";
            this.btn_start_hub.UseVisualStyleBackColor = true;
            // 
            // btn_file
            // 
            this.btn_file.Location = new System.Drawing.Point(525, 21);
            this.btn_file.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_file.Name = "btn_file";
            this.btn_file.Size = new System.Drawing.Size(128, 36);
            this.btn_file.TabIndex = 2;
            this.btn_file.Text = "File Operation";
            this.btn_file.UseVisualStyleBackColor = true;
            // 
            // lb_uname
            // 
            this.lb_uname.AutoSize = true;
            this.lb_uname.Location = new System.Drawing.Point(75, 31);
            this.lb_uname.Name = "lb_uname";
            this.lb_uname.Size = new System.Drawing.Size(78, 18);
            this.lb_uname.TabIndex = 0;
            this.lb_uname.Text = "User Name:";
            // 
            // tp_tools
            // 
            this.tp_tools.Controls.Add(this.downloadService1);
            this.tp_tools.ImageIndex = 2;
            this.tp_tools.Location = new System.Drawing.Point(4, 42);
            this.tp_tools.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_tools.Name = "tp_tools";
            this.tp_tools.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_tools.Size = new System.Drawing.Size(881, 404);
            this.tp_tools.TabIndex = 4;
            this.tp_tools.Text = "Tools";
            // 
            // downloadService1
            // 
            this.downloadService1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadService1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadService1.Location = new System.Drawing.Point(3, 4);
            this.downloadService1.Margin = new System.Windows.Forms.Padding(4);
            this.downloadService1.Name = "downloadService1";
            this.downloadService1.Size = new System.Drawing.Size(875, 396);
            this.downloadService1.TabIndex = 0;
            // 
            // tp_plugin
            // 
            this.tp_plugin.Controls.Add(this.servicePlugin1);
            this.tp_plugin.ImageIndex = 5;
            this.tp_plugin.Location = new System.Drawing.Point(4, 42);
            this.tp_plugin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_plugin.Name = "tp_plugin";
            this.tp_plugin.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_plugin.Size = new System.Drawing.Size(881, 404);
            this.tp_plugin.TabIndex = 12;
            this.tp_plugin.Text = "Services Plugin";
            this.tp_plugin.UseVisualStyleBackColor = true;
            // 
            // servicePlugin1
            // 
            this.servicePlugin1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.servicePlugin1.Location = new System.Drawing.Point(0, 0);
            this.servicePlugin1.Margin = new System.Windows.Forms.Padding(4);
            this.servicePlugin1.Name = "servicePlugin1";
            this.servicePlugin1.Size = new System.Drawing.Size(881, 404);
            this.servicePlugin1.TabIndex = 0;
            // 
            // tp_buildin
            // 
            this.tp_buildin.Controls.Add(this.builtinApps1);
            this.tp_buildin.ImageIndex = 6;
            this.tp_buildin.Location = new System.Drawing.Point(4, 42);
            this.tp_buildin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_buildin.Name = "tp_buildin";
            this.tp_buildin.Size = new System.Drawing.Size(881, 404);
            this.tp_buildin.TabIndex = 7;
            this.tp_buildin.Text = "Built-in Apps";
            // 
            // builtinApps1
            // 
            this.builtinApps1.Location = new System.Drawing.Point(0, 0);
            this.builtinApps1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.builtinApps1.Name = "builtinApps1";
            this.builtinApps1.Size = new System.Drawing.Size(882, 400);
            this.builtinApps1.TabIndex = 0;
            // 
            // tp_uds
            // 
            this.tp_uds.ImageIndex = 13;
            this.tp_uds.Location = new System.Drawing.Point(4, 42);
            this.tp_uds.Name = "tp_uds";
            this.tp_uds.Padding = new System.Windows.Forms.Padding(3);
            this.tp_uds.Size = new System.Drawing.Size(881, 404);
            this.tp_uds.TabIndex = 14;
            this.tp_uds.Text = "User-Defined Apps";
            this.tp_uds.UseVisualStyleBackColor = true;
            // 
            // tp_snort
            // 
            this.tp_snort.Controls.Add(this.snortAnalyze2);
            this.tp_snort.ImageIndex = 14;
            this.tp_snort.Location = new System.Drawing.Point(4, 42);
            this.tp_snort.Name = "tp_snort";
            this.tp_snort.Padding = new System.Windows.Forms.Padding(3);
            this.tp_snort.Size = new System.Drawing.Size(881, 404);
            this.tp_snort.TabIndex = 15;
            this.tp_snort.Text = "Snort Analyze";
            this.tp_snort.UseVisualStyleBackColor = true;
            // 
            // snortAnalyze2
            // 
            this.snortAnalyze2.Location = new System.Drawing.Point(-1, 0);
            this.snortAnalyze2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.snortAnalyze2.Name = "snortAnalyze2";
            this.snortAnalyze2.Size = new System.Drawing.Size(882, 424);
            this.snortAnalyze2.TabIndex = 0;
            // 
            // tp_logout
            // 
            this.tp_logout.ImageIndex = 11;
            this.tp_logout.Location = new System.Drawing.Point(4, 42);
            this.tp_logout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_logout.Name = "tp_logout";
            this.tp_logout.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_logout.Size = new System.Drawing.Size(881, 404);
            this.tp_logout.TabIndex = 13;
            this.tp_logout.Text = "Logout";
            this.tp_logout.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tp_hub);
            this.tabControl2.Controls.Add(this.tp_statistics);
            this.tabControl2.Controls.Add(this.tp_utility);
            this.tabControl2.Controls.Add(this.tp_about);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl2.ImageList = this.imageList2;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(889, 212);
            this.tabControl2.TabIndex = 0;
            // 
            // tp_hub
            // 
            this.tp_hub.Controls.Add(this.hubListView1);
            this.tp_hub.ImageIndex = 0;
            this.tp_hub.Location = new System.Drawing.Point(4, 26);
            this.tp_hub.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_hub.Name = "tp_hub";
            this.tp_hub.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_hub.Size = new System.Drawing.Size(881, 182);
            this.tp_hub.TabIndex = 0;
            this.tp_hub.Text = "Hub Nodes";
            this.tp_hub.UseVisualStyleBackColor = true;
            // 
            // hubListView1
            // 
            this.hubListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hubListView1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hubListView1.Location = new System.Drawing.Point(3, 4);
            this.hubListView1.Margin = new System.Windows.Forms.Padding(5);
            this.hubListView1.Name = "hubListView1";
            this.hubListView1.Size = new System.Drawing.Size(875, 174);
            this.hubListView1.TabIndex = 0;
            // 
            // tp_statistics
            // 
            this.tp_statistics.Controls.Add(this.statistics1);
            this.tp_statistics.ImageIndex = 2;
            this.tp_statistics.Location = new System.Drawing.Point(4, 23);
            this.tp_statistics.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_statistics.Name = "tp_statistics";
            this.tp_statistics.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_statistics.Size = new System.Drawing.Size(881, 185);
            this.tp_statistics.TabIndex = 1;
            this.tp_statistics.Text = "Statistics";
            this.tp_statistics.UseVisualStyleBackColor = true;
            // 
            // statistics1
            // 
            this.statistics1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statistics1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statistics1.Location = new System.Drawing.Point(3, 4);
            this.statistics1.Margin = new System.Windows.Forms.Padding(5);
            this.statistics1.Name = "statistics1";
            this.statistics1.Size = new System.Drawing.Size(875, 177);
            this.statistics1.TabIndex = 0;
            // 
            // tp_utility
            // 
            this.tp_utility.Controls.Add(this.command1);
            this.tp_utility.ImageIndex = 1;
            this.tp_utility.Location = new System.Drawing.Point(4, 23);
            this.tp_utility.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_utility.Name = "tp_utility";
            this.tp_utility.Size = new System.Drawing.Size(881, 185);
            this.tp_utility.TabIndex = 2;
            this.tp_utility.Text = "SystemUtility";
            this.tp_utility.UseVisualStyleBackColor = true;
            // 
            // command1
            // 
            this.command1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.command1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.command1.Location = new System.Drawing.Point(0, 0);
            this.command1.Margin = new System.Windows.Forms.Padding(5);
            this.command1.Name = "command1";
            this.command1.Size = new System.Drawing.Size(881, 185);
            this.command1.TabIndex = 0;
            // 
            // tp_about
            // 
            this.tp_about.Controls.Add(this.about1);
            this.tp_about.ImageIndex = 3;
            this.tp_about.Location = new System.Drawing.Point(4, 23);
            this.tp_about.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tp_about.Name = "tp_about";
            this.tp_about.Size = new System.Drawing.Size(881, 185);
            this.tp_about.TabIndex = 3;
            this.tp_about.Text = "About";
            this.tp_about.UseVisualStyleBackColor = true;
            // 
            // about1
            // 
            this.about1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.about1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.about1.Location = new System.Drawing.Point(0, 0);
            this.about1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.about1.Name = "about1";
            this.about1.Size = new System.Drawing.Size(881, 185);
            this.about1.TabIndex = 0;
            // 
            // AgentMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 702);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(905, 900);
            this.Name = "AgentMainForm";
            this.Text = "Collaborative Applications Platform based on Secure P2P Networks";
            this.Load += new System.EventHandler(this.AgentMainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tp_main.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tp_tools.ResumeLayout(false);
            this.tp_plugin.ResumeLayout(false);
            this.tp_buildin.ResumeLayout(false);
            this.tp_snort.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tp_hub.ResumeLayout(false);
            this.tp_statistics.ResumeLayout(false);
            this.tp_utility.ResumeLayout(false);
            this.tp_about.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_main;
        private Messenger messenger1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_stop_hub;
        private System.Windows.Forms.Label lb_agent_type;
        private System.Windows.Forms.Button btn_start_hub;
        private System.Windows.Forms.Button btn_file;
        private System.Windows.Forms.Label lb_uname;
        private System.Windows.Forms.TabPage tp_tools;
        private System.Windows.Forms.TabPage tp_plugin;
        private System.Windows.Forms.TabPage tp_buildin;
        private System.Windows.Forms.TabPage tp_logout;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tp_hub;
        private System.Windows.Forms.TabPage tp_statistics;
        private HubListView hubListView1;
        private Statistics statistics1;
        private System.Windows.Forms.TabPage tp_utility;
        private Command command1;
        private System.Windows.Forms.TabPage tp_about;
        private About about1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStatus;
        private System.Windows.Forms.TabPage tp_uds;
        private DownloadService downloadService1;
        private System.Windows.Forms.TabPage tp_snort;
        private ServicePlugin servicePlugin1;
        private BuiltinApps builtinApps1;
        private SnortAnalyze snortAnalyze2;
    }
}