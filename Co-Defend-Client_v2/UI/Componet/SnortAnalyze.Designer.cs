

namespace Co_Defend_Client_v2.UI
{
    partial class SnortAnalyze
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_upload = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_show = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_upload
            // 
            this.tb_upload.BackColor = System.Drawing.SystemColors.Window;
            this.tb_upload.Location = new System.Drawing.Point(16, 80);
            this.tb_upload.Name = "tb_upload";
            this.tb_upload.ReadOnly = true;
            this.tb_upload.Size = new System.Drawing.Size(246, 22);
            this.tb_upload.TabIndex = 0;
            this.tb_upload.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_upload_MouseClick);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(16, 114);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(70, 28);
            this.btn_send.TabIndex = 1;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.label1.Location = new System.Drawing.Point(92, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 18);
            this.label1.TabIndex = 2;
            // 
            // btn_show
            // 
            this.btn_show.Location = new System.Drawing.Point(16, 217);
            this.btn_show.Name = "btn_show";
            this.btn_show.Size = new System.Drawing.Size(70, 27);
            this.btn_show.TabIndex = 3;
            this.btn_show.Text = "Show";
            this.btn_show.UseVisualStyleBackColor = true;
            this.btn_show.Click += new System.EventHandler(this.btn_show_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.label2.Location = new System.Drawing.Point(13, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.label2.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_upload);
            this.groupBox1.Controls.Add(this.btn_show);
            this.groupBox1.Controls.Add(this.btn_send);
            this.groupBox1.Location = new System.Drawing.Point(9, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(681, 277);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send PCAP File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 12F);
            this.label3.Location = new System.Drawing.Point(16, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(238, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Snort will analyze a PCAP file!!";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SnortAnalyze
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "SnortAnalyze";
            this.Size = new System.Drawing.Size(882, 408);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tb_upload;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_show;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label3;
        private SnortAnalsischart SnortanalysisChart1;
    }
}
