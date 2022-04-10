namespace Co_Defend_Client_v2.UI
{
    partial class PublishService
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
            this.btn_publish = new System.Windows.Forms.Button();
            this.gb_select_task = new System.Windows.Forms.GroupBox();
            this.lv_task = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_run = new System.Windows.Forms.Button();
            this.btn_select = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.gb_status_list = new System.Windows.Forms.GroupBox();
            this.lv_service = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tb_publish = new System.Windows.Forms.TextBox();
            this.gb_publish = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_params = new System.Windows.Forms.TextBox();
            this.lb_params = new System.Windows.Forms.Label();
            this.tb_logname = new System.Windows.Forms.TextBox();
            this.lb_logname = new System.Windows.Forms.Label();
            this.lb_service = new System.Windows.Forms.Label();
            this.cb_services = new System.Windows.Forms.ComboBox();
            this.gb_setting_service = new System.Windows.Forms.GroupBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_cmd = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gb_select_task.SuspendLayout();
            this.gb_status_list.SuspendLayout();
            this.gb_publish.SuspendLayout();
            this.gb_setting_service.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_publish
            // 
            this.btn_publish.Location = new System.Drawing.Point(268, 39);
            this.btn_publish.Margin = new System.Windows.Forms.Padding(4);
            this.btn_publish.Name = "btn_publish";
            this.btn_publish.Size = new System.Drawing.Size(74, 32);
            this.btn_publish.TabIndex = 1;
            this.btn_publish.Text = "Push";
            this.btn_publish.UseVisualStyleBackColor = true;
            this.btn_publish.Click += new System.EventHandler(this.btn_publish_Click);
            // 
            // gb_select_task
            // 
            this.gb_select_task.Controls.Add(this.lv_task);
            this.gb_select_task.Controls.Add(this.btn_remove);
            this.gb_select_task.Controls.Add(this.btn_run);
            this.gb_select_task.Controls.Add(this.btn_select);
            this.gb_select_task.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gb_select_task.Location = new System.Drawing.Point(3, 107);
            this.gb_select_task.Margin = new System.Windows.Forms.Padding(4);
            this.gb_select_task.Name = "gb_select_task";
            this.gb_select_task.Padding = new System.Windows.Forms.Padding(4);
            this.gb_select_task.Size = new System.Drawing.Size(450, 228);
            this.gb_select_task.TabIndex = 5;
            this.gb_select_task.TabStop = false;
            this.gb_select_task.Text = "Select Tasks (Input Files) for Execution";
            // 
            // lv_task
            // 
            this.lv_task.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.lv_task.Dock = System.Windows.Forms.DockStyle.Top;
            this.lv_task.FullRowSelect = true;
            this.lv_task.GridLines = true;
            this.lv_task.Location = new System.Drawing.Point(4, 22);
            this.lv_task.Margin = new System.Windows.Forms.Padding(4);
            this.lv_task.Name = "lv_task";
            this.lv_task.Size = new System.Drawing.Size(442, 158);
            this.lv_task.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv_task.TabIndex = 1;
            this.lv_task.UseCompatibleStateImageBehavior = false;
            this.lv_task.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "File Name";
            this.columnHeader4.Width = 122;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Status";
            this.columnHeader5.Width = 213;
            // 
            // btn_remove
            // 
            this.btn_remove.Enabled = false;
            this.btn_remove.Location = new System.Drawing.Point(4, 188);
            this.btn_remove.Margin = new System.Windows.Forms.Padding(4);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(74, 32);
            this.btn_remove.TabIndex = 4;
            this.btn_remove.Text = "Remove";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // btn_run
            // 
            this.btn_run.Enabled = false;
            this.btn_run.Location = new System.Drawing.Point(368, 188);
            this.btn_run.Margin = new System.Windows.Forms.Padding(4);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(74, 32);
            this.btn_run.TabIndex = 3;
            this.btn_run.Text = "Start Run";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(86, 188);
            this.btn_select.Margin = new System.Windows.Forms.Padding(4);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(74, 32);
            this.btn_select.TabIndex = 2;
            this.btn_select.Text = "Select...";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // gb_status_list
            // 
            this.gb_status_list.Controls.Add(this.lv_service);
            this.gb_status_list.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_status_list.Location = new System.Drawing.Point(453, 3);
            this.gb_status_list.Margin = new System.Windows.Forms.Padding(4);
            this.gb_status_list.Name = "gb_status_list";
            this.gb_status_list.Padding = new System.Windows.Forms.Padding(4);
            this.gb_status_list.Size = new System.Drawing.Size(404, 332);
            this.gb_status_list.TabIndex = 4;
            this.gb_status_list.TabStop = false;
            this.gb_status_list.Text = "Download / Running Status of Peers";
            // 
            // lv_service
            // 
            this.lv_service.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lv_service.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_service.FullRowSelect = true;
            this.lv_service.GridLines = true;
            this.lv_service.Location = new System.Drawing.Point(4, 22);
            this.lv_service.Margin = new System.Windows.Forms.Padding(4);
            this.lv_service.Name = "lv_service";
            this.lv_service.Size = new System.Drawing.Size(396, 306);
            this.lv_service.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lv_service.TabIndex = 0;
            this.lv_service.UseCompatibleStateImageBehavior = false;
            this.lv_service.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Agent ID";
            this.columnHeader1.Width = 132;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Processing File";
            this.columnHeader2.Width = 114;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Status";
            this.columnHeader3.Width = 138;
            // 
            // tb_publish
            // 
            this.tb_publish.Location = new System.Drawing.Point(9, 43);
            this.tb_publish.Margin = new System.Windows.Forms.Padding(4);
            this.tb_publish.Name = "tb_publish";
            this.tb_publish.ReadOnly = true;
            this.tb_publish.Size = new System.Drawing.Size(251, 25);
            this.tb_publish.TabIndex = 6;
            this.tb_publish.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_publish_MouseClick);
            // 
            // gb_publish
            // 
            this.gb_publish.Controls.Add(this.label2);
            this.gb_publish.Controls.Add(this.tb_publish);
            this.gb_publish.Controls.Add(this.btn_publish);
            this.gb_publish.Dock = System.Windows.Forms.DockStyle.Top;
            this.gb_publish.Location = new System.Drawing.Point(3, 3);
            this.gb_publish.Margin = new System.Windows.Forms.Padding(4);
            this.gb_publish.Name = "gb_publish";
            this.gb_publish.Padding = new System.Windows.Forms.Padding(4);
            this.gb_publish.Size = new System.Drawing.Size(450, 81);
            this.gb_publish.TabIndex = 7;
            this.gb_publish.TabStop = false;
            this.gb_publish.Text = "Select Service";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Format：pub IP_ADDR {FILE1 FILE2 ...}";
            // 
            // tb_params
            // 
            this.tb_params.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_params.Location = new System.Drawing.Point(120, 211);
            this.tb_params.Margin = new System.Windows.Forms.Padding(4);
            this.tb_params.Name = "tb_params";
            this.tb_params.Size = new System.Drawing.Size(220, 25);
            this.tb_params.TabIndex = 13;
            // 
            // lb_params
            // 
            this.lb_params.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_params.AutoSize = true;
            this.lb_params.Location = new System.Drawing.Point(46, 214);
            this.lb_params.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_params.Name = "lb_params";
            this.lb_params.Size = new System.Drawing.Size(66, 18);
            this.lb_params.TabIndex = 12;
            this.lb_params.Text = "Params：";
            // 
            // tb_logname
            // 
            this.tb_logname.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_logname.Location = new System.Drawing.Point(120, 150);
            this.tb_logname.Margin = new System.Windows.Forms.Padding(4);
            this.tb_logname.Name = "tb_logname";
            this.tb_logname.Size = new System.Drawing.Size(220, 25);
            this.tb_logname.TabIndex = 11;
            // 
            // lb_logname
            // 
            this.lb_logname.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_logname.AutoSize = true;
            this.lb_logname.Location = new System.Drawing.Point(30, 153);
            this.lb_logname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_logname.Name = "lb_logname";
            this.lb_logname.Size = new System.Drawing.Size(82, 18);
            this.lb_logname.TabIndex = 10;
            this.lb_logname.Text = "Log Name：";
            // 
            // lb_service
            // 
            this.lb_service.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_service.AutoSize = true;
            this.lb_service.Location = new System.Drawing.Point(42, 92);
            this.lb_service.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_service.Name = "lb_service";
            this.lb_service.Size = new System.Drawing.Size(70, 18);
            this.lb_service.TabIndex = 9;
            this.lb_service.Text = "Service：";
            // 
            // cb_services
            // 
            this.cb_services.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_services.FormattingEnabled = true;
            this.cb_services.Location = new System.Drawing.Point(120, 89);
            this.cb_services.Margin = new System.Windows.Forms.Padding(4);
            this.cb_services.Name = "cb_services";
            this.cb_services.Size = new System.Drawing.Size(220, 25);
            this.cb_services.TabIndex = 8;
            // 
            // gb_setting_service
            // 
            this.gb_setting_service.Controls.Add(this.lb_service);
            this.gb_setting_service.Controls.Add(this.tb_logname);
            this.gb_setting_service.Controls.Add(this.tb_params);
            this.gb_setting_service.Controls.Add(this.cb_services);
            this.gb_setting_service.Controls.Add(this.lb_logname);
            this.gb_setting_service.Controls.Add(this.lb_params);
            this.gb_setting_service.Dock = System.Windows.Forms.DockStyle.Left;
            this.gb_setting_service.Location = new System.Drawing.Point(3, 3);
            this.gb_setting_service.Margin = new System.Windows.Forms.Padding(4);
            this.gb_setting_service.Name = "gb_setting_service";
            this.gb_setting_service.Padding = new System.Windows.Forms.Padding(4);
            this.gb_setting_service.Size = new System.Drawing.Size(370, 332);
            this.gb_setting_service.TabIndex = 15;
            this.gb_setting_service.TabStop = false;
            this.gb_setting_service.Text = "Setting Service";
            // 
            // btn_send
            // 
            this.btn_send.Enabled = false;
            this.btn_send.Location = new System.Drawing.Point(304, 158);
            this.btn_send.Margin = new System.Windows.Forms.Padding(4);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(74, 32);
            this.btn_send.TabIndex = 16;
            this.btn_send.Text = "Run";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 138);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 18);
            this.label1.TabIndex = 15;
            this.label1.Text = "Format：run SVC LOG SENDR_TARGET {PARAM1 PARAM2 ...}";
            // 
            // tb_cmd
            // 
            this.tb_cmd.Cursor = System.Windows.Forms.Cursors.Default;
            this.tb_cmd.Enabled = false;
            this.tb_cmd.Location = new System.Drawing.Point(11, 162);
            this.tb_cmd.Margin = new System.Windows.Forms.Padding(4);
            this.tb_cmd.Name = "tb_cmd";
            this.tb_cmd.ReadOnly = true;
            this.tb_cmd.Size = new System.Drawing.Size(285, 25);
            this.tb_cmd.TabIndex = 14;
            this.tb_cmd.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(868, 368);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gb_publish);
            this.tabPage1.Controls.Add(this.gb_select_task);
            this.tabPage1.Controls.Add(this.gb_status_list);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(860, 338);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Run New Service";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.gb_setting_service);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(860, 338);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Run Existing Service";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_cmd);
            this.groupBox1.Controls.Add(this.btn_send);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(373, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 332);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run Service";
            // 
            // PublishService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PublishService";
            this.Size = new System.Drawing.Size(868, 368);
            this.gb_select_task.ResumeLayout(false);
            this.gb_status_list.ResumeLayout(false);
            this.gb_publish.ResumeLayout(false);
            this.gb_publish.PerformLayout();
            this.gb_setting_service.ResumeLayout(false);
            this.gb_setting_service.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_publish;
        private System.Windows.Forms.GroupBox gb_select_task;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox gb_status_list;
        private System.Windows.Forms.ListView lv_service;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox tb_publish;
        private System.Windows.Forms.GroupBox gb_publish;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.ListView lv_task;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox tb_params;
        private System.Windows.Forms.Label lb_params;
        private System.Windows.Forms.TextBox tb_logname;
        private System.Windows.Forms.Label lb_logname;
        private System.Windows.Forms.Label lb_service;
        private System.Windows.Forms.ComboBox cb_services;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.GroupBox gb_setting_service;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_cmd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
