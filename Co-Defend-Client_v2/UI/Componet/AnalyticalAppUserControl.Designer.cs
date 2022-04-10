namespace Co_Defend_Client_v2.UI
{
    partial class AnalyticalAppUserControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pgb_parsing = new System.Windows.Forms.ProgressBar();
            this.btn_startParsing = new System.Windows.Forms.Button();
            this.lv_backLogList = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_vg_excuteTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_startService = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_vg_logName = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_analysis = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lv_tables = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tp_filter = new System.Windows.Forms.TabPage();
            this.btn_block = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_ft_blockIP = new System.Windows.Forms.TextBox();
            this.tb_ft_excutionTime = new System.Windows.Forms.TextBox();
            this.tb_ft_logName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_ft_cmd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgv_sus_ip = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tp_filter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_sus_ip)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tp_filter);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(771, 367);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(763, 336);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Run Virtual Gateway";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pgb_parsing);
            this.groupBox3.Controls.Add(this.btn_startParsing);
            this.groupBox3.Controls.Add(this.lv_backLogList);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(4, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(755, 232);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Back Log List";
            // 
            // pgb_parsing
            // 
            this.pgb_parsing.Location = new System.Drawing.Point(3, 197);
            this.pgb_parsing.Name = "pgb_parsing";
            this.pgb_parsing.Size = new System.Drawing.Size(651, 32);
            this.pgb_parsing.TabIndex = 3;
            // 
            // btn_startParsing
            // 
            this.btn_startParsing.Enabled = false;
            this.btn_startParsing.Location = new System.Drawing.Point(657, 197);
            this.btn_startParsing.Name = "btn_startParsing";
            this.btn_startParsing.Size = new System.Drawing.Size(98, 32);
            this.btn_startParsing.TabIndex = 2;
            this.btn_startParsing.Text = "Start Parsing";
            this.btn_startParsing.UseVisualStyleBackColor = true;
            // 
            // lv_backLogList
            // 
            this.lv_backLogList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2});
            this.lv_backLogList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lv_backLogList.FullRowSelect = true;
            this.lv_backLogList.GridLines = true;
            this.lv_backLogList.Location = new System.Drawing.Point(3, 22);
            this.lv_backLogList.Name = "lv_backLogList";
            this.lv_backLogList.ShowItemToolTips = true;
            this.lv_backLogList.Size = new System.Drawing.Size(749, 170);
            this.lv_backLogList.TabIndex = 0;
            this.lv_backLogList.UseCompatibleStateImageBehavior = false;
            this.lv_backLogList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "File Name";
            this.columnHeader0.Width = 200;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File Path";
            this.columnHeader1.Width = 430;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Status";
            this.columnHeader2.Width = 113;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_vg_excuteTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_startService);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_vg_logName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(755, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Virtual Gateway Service Configuration";
            // 
            // tb_vg_excuteTime
            // 
            this.tb_vg_excuteTime.Location = new System.Drawing.Point(138, 57);
            this.tb_vg_excuteTime.Name = "tb_vg_excuteTime";
            this.tb_vg_excuteTime.Size = new System.Drawing.Size(513, 26);
            this.tb_vg_excuteTime.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Log Name: ";
            // 
            // btn_startService
            // 
            this.btn_startService.Enabled = false;
            this.btn_startService.Location = new System.Drawing.Point(657, 57);
            this.btn_startService.Name = "btn_startService";
            this.btn_startService.Size = new System.Drawing.Size(98, 26);
            this.btn_startService.TabIndex = 1;
            this.btn_startService.Text = "Start Service";
            this.btn_startService.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Excution Time (s): ";
            // 
            // tb_vg_logName
            // 
            this.tb_vg_logName.Location = new System.Drawing.Point(138, 25);
            this.tb_vg_logName.Name = "tb_vg_logName";
            this.tb_vg_logName.Size = new System.Drawing.Size(513, 26);
            this.tb_vg_logName.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_analysis);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(763, 336);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Comparative Analysis";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_analysis
            // 
            this.btn_analysis.Location = new System.Drawing.Point(665, 298);
            this.btn_analysis.Name = "btn_analysis";
            this.btn_analysis.Size = new System.Drawing.Size(98, 32);
            this.btn_analysis.TabIndex = 2;
            this.btn_analysis.Text = "Analysis";
            this.btn_analysis.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lv_tables);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(757, 292);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Analysis of the Data Table Lists";
            // 
            // lv_tables
            // 
            this.lv_tables.CheckBoxes = true;
            this.lv_tables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lv_tables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_tables.FullRowSelect = true;
            this.lv_tables.GridLines = true;
            this.lv_tables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_tables.Location = new System.Drawing.Point(3, 22);
            this.lv_tables.Name = "lv_tables";
            this.lv_tables.Size = new System.Drawing.Size(751, 267);
            this.lv_tables.TabIndex = 0;
            this.lv_tables.UseCompatibleStateImageBehavior = false;
            this.lv_tables.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Analysis Date";
            this.columnHeader3.Width = 230;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Table Name";
            this.columnHeader4.Width = 468;
            // 
            // tp_filter
            // 
            this.tp_filter.Controls.Add(this.btn_block);
            this.tp_filter.Controls.Add(this.label6);
            this.tp_filter.Controls.Add(this.tb_ft_blockIP);
            this.tp_filter.Controls.Add(this.tb_ft_excutionTime);
            this.tp_filter.Controls.Add(this.tb_ft_logName);
            this.tp_filter.Controls.Add(this.label5);
            this.tp_filter.Controls.Add(this.label4);
            this.tp_filter.Controls.Add(this.tb_ft_cmd);
            this.tp_filter.Controls.Add(this.label3);
            this.tp_filter.Controls.Add(this.groupBox2);
            this.tp_filter.Location = new System.Drawing.Point(4, 27);
            this.tp_filter.Name = "tp_filter";
            this.tp_filter.Padding = new System.Windows.Forms.Padding(3);
            this.tp_filter.Size = new System.Drawing.Size(763, 336);
            this.tp_filter.TabIndex = 3;
            this.tp_filter.Text = "Start Filtering";
            this.tp_filter.UseVisualStyleBackColor = true;
            // 
            // btn_block
            // 
            this.btn_block.Location = new System.Drawing.Point(682, 302);
            this.btn_block.Name = "btn_block";
            this.btn_block.Size = new System.Drawing.Size(75, 26);
            this.btn_block.TabIndex = 10;
            this.btn_block.Text = "Block";
            this.btn_block.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(348, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 18);
            this.label6.TabIndex = 9;
            this.label6.Text = "Block IPs:";
            // 
            // tb_ft_blockIP
            // 
            this.tb_ft_blockIP.Location = new System.Drawing.Point(420, 251);
            this.tb_ft_blockIP.Name = "tb_ft_blockIP";
            this.tb_ft_blockIP.Size = new System.Drawing.Size(337, 26);
            this.tb_ft_blockIP.TabIndex = 8;
            // 
            // tb_ft_excutionTime
            // 
            this.tb_ft_excutionTime.Location = new System.Drawing.Point(135, 266);
            this.tb_ft_excutionTime.Name = "tb_ft_excutionTime";
            this.tb_ft_excutionTime.Size = new System.Drawing.Size(196, 26);
            this.tb_ft_excutionTime.TabIndex = 7;
            // 
            // tb_ft_logName
            // 
            this.tb_ft_logName.Location = new System.Drawing.Point(135, 234);
            this.tb_ft_logName.Name = "tb_ft_logName";
            this.tb_ft_logName.Size = new System.Drawing.Size(196, 26);
            this.tb_ft_logName.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "Excution Time(s):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Log Name:";
            // 
            // tb_ft_cmd
            // 
            this.tb_ft_cmd.Enabled = false;
            this.tb_ft_cmd.Location = new System.Drawing.Point(135, 302);
            this.tb_ft_cmd.Name = "tb_ft_cmd";
            this.tb_ft_cmd.Size = new System.Drawing.Size(541, 26);
            this.tb_ft_cmd.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 305);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Filtering Command:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgv_sus_ip);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(757, 231);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Suspicious List";
            // 
            // dgv_sus_ip
            // 
            this.dgv_sus_ip.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_sus_ip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_sus_ip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_sus_ip.Location = new System.Drawing.Point(3, 22);
            this.dgv_sus_ip.Name = "dgv_sus_ip";
            this.dgv_sus_ip.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_sus_ip.Size = new System.Drawing.Size(751, 206);
            this.dgv_sus_ip.TabIndex = 0;
            // 
            // AnalyticalAppUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AnalyticalAppUserControl";
            this.Size = new System.Drawing.Size(771, 367);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tp_filter.ResumeLayout(false);
            this.tp_filter.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_sus_ip)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_startService;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_vg_logName;
        private System.Windows.Forms.TextBox tb_vg_excuteTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lv_backLogList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btn_startParsing;
        private System.Windows.Forms.ProgressBar pgb_parsing;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView lv_tables;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btn_analysis;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabPage tp_filter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_block;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_ft_blockIP;
        private System.Windows.Forms.TextBox tb_ft_excutionTime;
        private System.Windows.Forms.TextBox tb_ft_logName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_ft_cmd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgv_sus_ip;

    }
}
