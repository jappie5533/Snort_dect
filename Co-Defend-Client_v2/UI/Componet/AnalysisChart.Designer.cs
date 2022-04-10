namespace Co_Defend_Client_v2.UI
{
    partial class AnalysisChart
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoBtn_rawData = new System.Windows.Forms.RadioButton();
            this.rdobtn_icmpAttack = new System.Windows.Forms.RadioButton();
            this.rdobtn_udpAttack = new System.Windows.Forms.RadioButton();
            this.rdobtn_synAttack = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdobtn_smac = new System.Windows.Forms.RadioButton();
            this.rdobtn_sip = new System.Windows.Forms.RadioButton();
            this.rdobtn_port_scan = new System.Windows.Forms.RadioButton();
            this.rdobtn_flooding = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(0, 66);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(17, 19);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(620, 589);
            this.webBrowser1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdobtn_flooding);
            this.groupBox2.Controls.Add(this.rdobtn_port_scan);
            this.groupBox2.Controls.Add(this.rdoBtn_rawData);
            this.groupBox2.Controls.Add(this.rdobtn_icmpAttack);
            this.groupBox2.Controls.Add(this.rdobtn_udpAttack);
            this.groupBox2.Controls.Add(this.rdobtn_synAttack);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 6, 5, 0);
            this.groupBox2.Size = new System.Drawing.Size(446, 66);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Analysis Mode";
            // 
            // rdoBtn_rawData
            // 
            this.rdoBtn_rawData.AutoSize = true;
            this.rdoBtn_rawData.Location = new System.Drawing.Point(9, 18);
            this.rdoBtn_rawData.Margin = new System.Windows.Forms.Padding(4);
            this.rdoBtn_rawData.Name = "rdoBtn_rawData";
            this.rdoBtn_rawData.Size = new System.Drawing.Size(83, 22);
            this.rdoBtn_rawData.TabIndex = 3;
            this.rdoBtn_rawData.TabStop = true;
            this.rdoBtn_rawData.Text = "Raw Data";
            this.rdoBtn_rawData.UseVisualStyleBackColor = true;
            // 
            // rdobtn_icmpAttack
            // 
            this.rdobtn_icmpAttack.AutoSize = true;
            this.rdobtn_icmpAttack.Location = new System.Drawing.Point(330, 18);
            this.rdobtn_icmpAttack.Margin = new System.Windows.Forms.Padding(4);
            this.rdobtn_icmpAttack.Name = "rdobtn_icmpAttack";
            this.rdobtn_icmpAttack.Size = new System.Drawing.Size(113, 22);
            this.rdobtn_icmpAttack.TabIndex = 2;
            this.rdobtn_icmpAttack.TabStop = true;
            this.rdobtn_icmpAttack.Text = "ICMP Analysis";
            this.rdobtn_icmpAttack.UseVisualStyleBackColor = true;
            // 
            // rdobtn_udpAttack
            // 
            this.rdobtn_udpAttack.AutoSize = true;
            this.rdobtn_udpAttack.Location = new System.Drawing.Point(216, 18);
            this.rdobtn_udpAttack.Margin = new System.Windows.Forms.Padding(4);
            this.rdobtn_udpAttack.Name = "rdobtn_udpAttack";
            this.rdobtn_udpAttack.Size = new System.Drawing.Size(106, 22);
            this.rdobtn_udpAttack.TabIndex = 1;
            this.rdobtn_udpAttack.TabStop = true;
            this.rdobtn_udpAttack.Text = "UDP Analysis";
            this.rdobtn_udpAttack.UseVisualStyleBackColor = true;
            // 
            // rdobtn_synAttack
            // 
            this.rdobtn_synAttack.AutoSize = true;
            this.rdobtn_synAttack.Location = new System.Drawing.Point(100, 18);
            this.rdobtn_synAttack.Margin = new System.Windows.Forms.Padding(4);
            this.rdobtn_synAttack.Name = "rdobtn_synAttack";
            this.rdobtn_synAttack.Size = new System.Drawing.Size(108, 22);
            this.rdobtn_synAttack.TabIndex = 0;
            this.rdobtn_synAttack.TabStop = true;
            this.rdobtn_synAttack.Text = "SYN Analysis";
            this.rdobtn_synAttack.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdobtn_smac);
            this.groupBox1.Controls.Add(this.rdobtn_sip);
            this.groupBox1.Location = new System.Drawing.Point(450, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(167, 66);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Show Mode";
            // 
            // rdobtn_smac
            // 
            this.rdobtn_smac.AutoSize = true;
            this.rdobtn_smac.Location = new System.Drawing.Point(16, 39);
            this.rdobtn_smac.Name = "rdobtn_smac";
            this.rdobtn_smac.Size = new System.Drawing.Size(101, 22);
            this.rdobtn_smac.TabIndex = 1;
            this.rdobtn_smac.Tag = "Source_MAC";
            this.rdobtn_smac.Text = "Source MAC";
            this.rdobtn_smac.UseVisualStyleBackColor = true;
            // 
            // rdobtn_sip
            // 
            this.rdobtn_sip.AutoSize = true;
            this.rdobtn_sip.Checked = true;
            this.rdobtn_sip.Location = new System.Drawing.Point(16, 15);
            this.rdobtn_sip.Name = "rdobtn_sip";
            this.rdobtn_sip.Size = new System.Drawing.Size(87, 22);
            this.rdobtn_sip.TabIndex = 0;
            this.rdobtn_sip.TabStop = true;
            this.rdobtn_sip.Tag = "Source_IP";
            this.rdobtn_sip.Text = "Source IP";
            this.rdobtn_sip.UseVisualStyleBackColor = true;
            // 
            // rdobtn_port_scan
            // 
            this.rdobtn_port_scan.AutoSize = true;
            this.rdobtn_port_scan.Location = new System.Drawing.Point(71, 40);
            this.rdobtn_port_scan.Margin = new System.Windows.Forms.Padding(4);
            this.rdobtn_port_scan.Name = "rdobtn_port_scan";
            this.rdobtn_port_scan.Size = new System.Drawing.Size(139, 22);
            this.rdobtn_port_scan.TabIndex = 4;
            this.rdobtn_port_scan.TabStop = true;
            this.rdobtn_port_scan.Text = "Port Scan Analysis";
            this.rdobtn_port_scan.UseVisualStyleBackColor = true;
            // 
            // rdobtn_flooding
            // 
            this.rdobtn_flooding.AutoSize = true;
            this.rdobtn_flooding.Location = new System.Drawing.Point(244, 40);
            this.rdobtn_flooding.Margin = new System.Windows.Forms.Padding(4);
            this.rdobtn_flooding.Name = "rdobtn_flooding";
            this.rdobtn_flooding.Size = new System.Drawing.Size(132, 22);
            this.rdobtn_flooding.TabIndex = 5;
            this.rdobtn_flooding.TabStop = true;
            this.rdobtn_flooding.Text = "Flooding Analysis";
            this.rdobtn_flooding.UseVisualStyleBackColor = true;
            // 
            // AnalysisChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AnalysisChart";
            this.Size = new System.Drawing.Size(620, 655);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoBtn_rawData;
        private System.Windows.Forms.RadioButton rdobtn_icmpAttack;
        private System.Windows.Forms.RadioButton rdobtn_udpAttack;
        private System.Windows.Forms.RadioButton rdobtn_synAttack;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdobtn_smac;
        private System.Windows.Forms.RadioButton rdobtn_sip;
        private System.Windows.Forms.RadioButton rdobtn_flooding;
        private System.Windows.Forms.RadioButton rdobtn_port_scan;
    }
}
