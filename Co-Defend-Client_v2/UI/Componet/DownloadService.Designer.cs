namespace Co_Defend_Client_v2.UI
{
    partial class DownloadService
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadService));
            this.lb_all_info = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lv_service = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pb_all_task = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_info = new System.Windows.Forms.Label();
            this.pb_download = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_discover = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lv_discover = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.bt_discover = new System.Windows.Forms.Button();
            this.tb_ttl = new System.Windows.Forms.TextBox();
            this.lb_ttl = new System.Windows.Forms.Label();
            this.tp_download = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lv_pub_discover = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gb_publish = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_publish = new System.Windows.Forms.TextBox();
            this.btn_publish = new System.Windows.Forms.Button();
            this.gb_status_list = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tp_discover.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tp_download.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gb_publish.SuspendLayout();
            this.gb_status_list.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_all_info
            // 
            this.lb_all_info.AutoSize = true;
            this.lb_all_info.Location = new System.Drawing.Point(8, 83);
            this.lb_all_info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_all_info.Name = "lb_all_info";
            this.lb_all_info.Size = new System.Drawing.Size(72, 18);
            this.lb_all_info.TabIndex = 4;
            this.lb_all_info.Text = "All Status:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lv_service);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(771, 210);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recent Item";
            // 
            // lv_service
            // 
            this.lv_service.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lv_service.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_service.FullRowSelect = true;
            this.lv_service.GridLines = true;
            this.lv_service.Location = new System.Drawing.Point(4, 23);
            this.lv_service.Margin = new System.Windows.Forms.Padding(4);
            this.lv_service.Name = "lv_service";
            this.lv_service.Size = new System.Drawing.Size(763, 183);
            this.lv_service.TabIndex = 0;
            this.lv_service.UseCompatibleStateImageBehavior = false;
            this.lv_service.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Name";
            this.columnHeader1.Width = 276;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 478;
            // 
            // pb_all_task
            // 
            this.pb_all_task.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_all_task.Location = new System.Drawing.Point(8, 108);
            this.pb_all_task.Margin = new System.Windows.Forms.Padding(4);
            this.pb_all_task.Name = "pb_all_task";
            this.pb_all_task.Size = new System.Drawing.Size(755, 29);
            this.pb_all_task.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb_all_task.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_all_info);
            this.groupBox2.Controls.Add(this.pb_all_task);
            this.groupBox2.Controls.Add(this.lb_info);
            this.groupBox2.Controls.Add(this.pb_download);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(3, 214);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(771, 148);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process";
            // 
            // lb_info
            // 
            this.lb_info.AutoSize = true;
            this.lb_info.Location = new System.Drawing.Point(8, 17);
            this.lb_info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(61, 18);
            this.lb_info.TabIndex = 2;
            this.lb_info.Text = "No Task.";
            // 
            // pb_download
            // 
            this.pb_download.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_download.Location = new System.Drawing.Point(8, 42);
            this.pb_download.Margin = new System.Windows.Forms.Padding(4);
            this.pb_download.Name = "pb_download";
            this.pb_download.Size = new System.Drawing.Size(755, 29);
            this.pb_download.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb_download.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp_discover);
            this.tabControl1.Controls.Add(this.tp_download);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(785, 396);
            this.tabControl1.TabIndex = 5;
            // 
            // tp_discover
            // 
            this.tp_discover.Controls.Add(this.groupBox4);
            this.tp_discover.Controls.Add(this.groupBox3);
            this.tp_discover.ImageIndex = 0;
            this.tp_discover.Location = new System.Drawing.Point(4, 27);
            this.tp_discover.Name = "tp_discover";
            this.tp_discover.Padding = new System.Windows.Forms.Padding(3);
            this.tp_discover.Size = new System.Drawing.Size(777, 365);
            this.tp_discover.TabIndex = 1;
            this.tp_discover.Text = "Discover Peers";
            this.tp_discover.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lv_discover);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(257, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(517, 359);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Discover List";
            // 
            // lv_discover
            // 
            this.lv_discover.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader3,
            this.columnHeader4});
            this.lv_discover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_discover.FullRowSelect = true;
            this.lv_discover.GridLines = true;
            this.lv_discover.Location = new System.Drawing.Point(3, 22);
            this.lv_discover.Margin = new System.Windows.Forms.Padding(4);
            this.lv_discover.Name = "lv_discover";
            this.lv_discover.Size = new System.Drawing.Size(511, 334);
            this.lv_discover.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv_discover.TabIndex = 1;
            this.lv_discover.UseCompatibleStateImageBehavior = false;
            this.lv_discover.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Account";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Agent ID";
            this.columnHeader3.Width = 192;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "IP";
            this.columnHeader4.Width = 153;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.bt_discover);
            this.groupBox3.Controls.Add(this.tb_ttl);
            this.groupBox3.Controls.Add(this.lb_ttl);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(254, 359);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Seting Discover TTL";
            // 
            // bt_discover
            // 
            this.bt_discover.Location = new System.Drawing.Point(169, 163);
            this.bt_discover.Name = "bt_discover";
            this.bt_discover.Size = new System.Drawing.Size(74, 32);
            this.bt_discover.TabIndex = 2;
            this.bt_discover.Text = "Discover";
            this.bt_discover.UseVisualStyleBackColor = true;
            this.bt_discover.Click += new System.EventHandler(this.btn_discover_Click);
            // 
            // tb_ttl
            // 
            this.tb_ttl.Location = new System.Drawing.Point(67, 167);
            this.tb_ttl.Name = "tb_ttl";
            this.tb_ttl.Size = new System.Drawing.Size(96, 26);
            this.tb_ttl.TabIndex = 1;
            // 
            // lb_ttl
            // 
            this.lb_ttl.AutoSize = true;
            this.lb_ttl.Location = new System.Drawing.Point(12, 170);
            this.lb_ttl.Name = "lb_ttl";
            this.lb_ttl.Size = new System.Drawing.Size(49, 18);
            this.lb_ttl.TabIndex = 0;
            this.lb_ttl.Text = "TTL：";
            // 
            // tp_download
            // 
            this.tp_download.Controls.Add(this.groupBox2);
            this.tp_download.Controls.Add(this.groupBox1);
            this.tp_download.ImageIndex = 1;
            this.tp_download.Location = new System.Drawing.Point(4, 27);
            this.tp_download.Name = "tp_download";
            this.tp_download.Padding = new System.Windows.Forms.Padding(3);
            this.tp_download.Size = new System.Drawing.Size(777, 365);
            this.tp_download.TabIndex = 0;
            this.tp_download.Text = "Download Status";
            this.tp_download.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.gb_publish);
            this.tabPage1.Controls.Add(this.gb_status_list);
            this.tabPage1.ImageIndex = 2;
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(777, 365);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Publish New Services";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.lv_pub_discover);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(419, 184);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Peers Discovery / Select Peers";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(339, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "Discover";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_discover_Click);
            // 
            // lv_pub_discover
            // 
            this.lv_pub_discover.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.lv_pub_discover.Dock = System.Windows.Forms.DockStyle.Top;
            this.lv_pub_discover.FullRowSelect = true;
            this.lv_pub_discover.GridLines = true;
            this.lv_pub_discover.Location = new System.Drawing.Point(3, 22);
            this.lv_pub_discover.Margin = new System.Windows.Forms.Padding(4);
            this.lv_pub_discover.Name = "lv_pub_discover";
            this.lv_pub_discover.Size = new System.Drawing.Size(413, 116);
            this.lv_pub_discover.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv_pub_discover.TabIndex = 1;
            this.lv_pub_discover.UseCompatibleStateImageBehavior = false;
            this.lv_pub_discover.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Account";
            this.columnHeader9.Width = 113;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Agent ID";
            this.columnHeader10.Width = 170;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "IP";
            this.columnHeader11.Width = 107;
            // 
            // gb_publish
            // 
            this.gb_publish.Controls.Add(this.label2);
            this.gb_publish.Controls.Add(this.tb_publish);
            this.gb_publish.Controls.Add(this.btn_publish);
            this.gb_publish.Location = new System.Drawing.Point(427, 101);
            this.gb_publish.Margin = new System.Windows.Forms.Padding(4);
            this.gb_publish.Name = "gb_publish";
            this.gb_publish.Padding = new System.Windows.Forms.Padding(4);
            this.gb_publish.Size = new System.Drawing.Size(347, 86);
            this.gb_publish.TabIndex = 8;
            this.gb_publish.TabStop = false;
            this.gb_publish.Text = "Select Services";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(308, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Format：pub IP_ADDR {FILE1 FILE2 ...} DES_ID";
            // 
            // tb_publish
            // 
            this.tb_publish.Location = new System.Drawing.Point(9, 49);
            this.tb_publish.Margin = new System.Windows.Forms.Padding(4);
            this.tb_publish.Name = "tb_publish";
            this.tb_publish.ReadOnly = true;
            this.tb_publish.Size = new System.Drawing.Size(251, 26);
            this.tb_publish.TabIndex = 6;
            this.tb_publish.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_publish_MouseClick);
            // 
            // btn_publish
            // 
            this.btn_publish.Enabled = false;
            this.btn_publish.Location = new System.Drawing.Point(268, 45);
            this.btn_publish.Margin = new System.Windows.Forms.Padding(4);
            this.btn_publish.Name = "btn_publish";
            this.btn_publish.Size = new System.Drawing.Size(74, 32);
            this.btn_publish.TabIndex = 1;
            this.btn_publish.Text = "Publish";
            this.btn_publish.UseVisualStyleBackColor = true;
            this.btn_publish.Click += new System.EventHandler(this.btn_publish_Click);
            // 
            // gb_status_list
            // 
            this.gb_status_list.Controls.Add(this.listView1);
            this.gb_status_list.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_status_list.Location = new System.Drawing.Point(3, 187);
            this.gb_status_list.Margin = new System.Windows.Forms.Padding(4);
            this.gb_status_list.Name = "gb_status_list";
            this.gb_status_list.Padding = new System.Windows.Forms.Padding(4);
            this.gb_status_list.Size = new System.Drawing.Size(771, 175);
            this.gb_status_list.TabIndex = 9;
            this.gb_status_list.TabStop = false;
            this.gb_status_list.Text = "Download Status of Peers";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(4, 23);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(763, 148);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Agent ID";
            this.columnHeader6.Width = 185;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Processing File";
            this.columnHeader7.Width = 210;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Status";
            this.columnHeader8.Width = 342;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Custom-Icon-Design-Pretty-Office-3-Search-Globe.ico");
            this.imageList1.Images.SetKeyName(1, "download.ico");
            this.imageList1.Images.SetKeyName(2, "publish.ico");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DownloadService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DownloadService";
            this.Size = new System.Drawing.Size(785, 396);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tp_discover.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tp_download.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.gb_publish.ResumeLayout(false);
            this.gb_publish.PerformLayout();
            this.gb_status_list.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lb_all_info;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lv_service;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ProgressBar pb_all_task;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lb_info;
        private System.Windows.Forms.ProgressBar pb_download;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_discover;
        private System.Windows.Forms.TabPage tp_download;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button bt_discover;
        private System.Windows.Forms.TextBox tb_ttl;
        private System.Windows.Forms.Label lb_ttl;
        public System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView lv_discover;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gb_publish;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_publish;
        private System.Windows.Forms.Button btn_publish;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView lv_pub_discover;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.GroupBox gb_status_list;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}
