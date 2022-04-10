namespace Co_Defend_Client_v2.UI
{
    partial class Command
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_send = new System.Windows.Forms.Button();
            this.tb_cmd = new System.Windows.Forms.TextBox();
            this.richTxtBoxLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btn_send);
            this.groupBox2.Controls.Add(this.tb_cmd);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(329, 181);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Command";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 72);
            this.label1.TabIndex = 5;
            this.label1.Text = "Useage Format：\r\n    msg SENDER MESSAGE\r\n    run SERVC LOG SENDR {ARG1 ARG2 ...}\r\n" +
    "    pub IP_ADDR {FILE1 FILE2 ...}";
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(117, 140);
            this.btn_send.Margin = new System.Windows.Forms.Padding(4);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(72, 32);
            this.btn_send.TabIndex = 4;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            // 
            // tb_cmd
            // 
            this.tb_cmd.Location = new System.Drawing.Point(8, 103);
            this.tb_cmd.Margin = new System.Windows.Forms.Padding(4);
            this.tb_cmd.Name = "tb_cmd";
            this.tb_cmd.Size = new System.Drawing.Size(309, 25);
            this.tb_cmd.TabIndex = 3;
            // 
            // richTxtBoxLog
            // 
            this.richTxtBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTxtBoxLog.Location = new System.Drawing.Point(4, 23);
            this.richTxtBoxLog.Margin = new System.Windows.Forms.Padding(4);
            this.richTxtBoxLog.Name = "richTxtBoxLog";
            this.richTxtBoxLog.ReadOnly = true;
            this.richTxtBoxLog.Size = new System.Drawing.Size(454, 154);
            this.richTxtBoxLog.TabIndex = 8;
            this.richTxtBoxLog.TabStop = false;
            this.richTxtBoxLog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTxtBoxLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(329, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(462, 181);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // Command
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Command";
            this.Size = new System.Drawing.Size(791, 181);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox tb_cmd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTxtBoxLog;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
