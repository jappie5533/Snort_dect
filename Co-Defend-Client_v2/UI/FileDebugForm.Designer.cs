namespace Co_Defend_Client_v2.UI
{
    partial class FileDebugForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_download_local = new System.Windows.Forms.TextBox();
            this.tb_upload_server = new System.Windows.Forms.TextBox();
            this.tb_download_server = new System.Windows.Forms.TextBox();
            this.btn_download = new System.Windows.Forms.Button();
            this.tb_upload_local = new System.Windows.Forms.TextBox();
            this.btn_upload = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lb_status = new System.Windows.Forms.Label();
            this.btn_pcap_start = new System.Windows.Forms.Button();
            this.tb_pcap = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_fileName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_folder = new System.Windows.Forms.TextBox();
            this.btn_zip = new System.Windows.Forms.Button();
            this.btn_unzip = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_download_local);
            this.groupBox1.Controls.Add(this.tb_upload_server);
            this.groupBox1.Controls.Add(this.tb_download_server);
            this.groupBox1.Controls.Add(this.btn_download);
            this.groupBox1.Controls.Add(this.tb_upload_local);
            this.groupBox1.Controls.Add(this.btn_upload);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 111);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Operation Debug";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "To";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Download From：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Upload From：";
            // 
            // tb_download_local
            // 
            this.tb_download_local.Location = new System.Drawing.Point(243, 65);
            this.tb_download_local.Name = "tb_download_local";
            this.tb_download_local.Size = new System.Drawing.Size(91, 20);
            this.tb_download_local.TabIndex = 11;
            // 
            // tb_upload_server
            // 
            this.tb_upload_server.Location = new System.Drawing.Point(243, 33);
            this.tb_upload_server.Name = "tb_upload_server";
            this.tb_upload_server.Size = new System.Drawing.Size(91, 20);
            this.tb_upload_server.TabIndex = 10;
            // 
            // tb_download_server
            // 
            this.tb_download_server.Location = new System.Drawing.Point(119, 65);
            this.tb_download_server.Name = "tb_download_server";
            this.tb_download_server.Size = new System.Drawing.Size(91, 20);
            this.tb_download_server.TabIndex = 9;
            // 
            // btn_download
            // 
            this.btn_download.Location = new System.Drawing.Point(352, 63);
            this.btn_download.Name = "btn_download";
            this.btn_download.Size = new System.Drawing.Size(75, 23);
            this.btn_download.TabIndex = 7;
            this.btn_download.Text = "Download";
            this.btn_download.UseVisualStyleBackColor = true;
            this.btn_download.Click += new System.EventHandler(this.btn_download_Click);
            // 
            // tb_upload_local
            // 
            this.tb_upload_local.Location = new System.Drawing.Point(119, 33);
            this.tb_upload_local.Name = "tb_upload_local";
            this.tb_upload_local.Size = new System.Drawing.Size(91, 20);
            this.tb_upload_local.TabIndex = 8;
            this.tb_upload_local.Click += new System.EventHandler(this.tb_upload_Click);
            // 
            // btn_upload
            // 
            this.btn_upload.Location = new System.Drawing.Point(352, 31);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(75, 23);
            this.btn_upload.TabIndex = 6;
            this.btn_upload.Text = "Upload";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 164);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(445, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.Location = new System.Drawing.Point(9, 135);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(47, 15);
            this.lb_status.TabIndex = 12;
            this.lb_status.Text = "Status: ";
            // 
            // btn_pcap_start
            // 
            this.btn_pcap_start.Location = new System.Drawing.Point(364, 210);
            this.btn_pcap_start.Name = "btn_pcap_start";
            this.btn_pcap_start.Size = new System.Drawing.Size(75, 25);
            this.btn_pcap_start.TabIndex = 13;
            this.btn_pcap_start.Text = "Start";
            this.btn_pcap_start.UseVisualStyleBackColor = true;
            this.btn_pcap_start.Click += new System.EventHandler(this.btn_pcap_start_Click);
            // 
            // tb_pcap
            // 
            this.tb_pcap.Location = new System.Drawing.Point(121, 213);
            this.tb_pcap.Name = "tb_pcap";
            this.tb_pcap.Size = new System.Drawing.Size(237, 20);
            this.tb_pcap.TabIndex = 14;
            this.tb_pcap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_pcap_MouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Pcap file location: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 286);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 15);
            this.label6.TabIndex = 16;
            this.label6.Text = "Zip/UnZip File Name: ";
            // 
            // tb_fileName
            // 
            this.tb_fileName.Location = new System.Drawing.Point(150, 286);
            this.tb_fileName.Name = "tb_fileName";
            this.tb_fileName.Size = new System.Drawing.Size(289, 20);
            this.tb_fileName.TabIndex = 17;
            this.tb_fileName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_fileName_MouseClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 320);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Zip/UnZip Folder: ";
            // 
            // tb_folder
            // 
            this.tb_folder.Location = new System.Drawing.Point(150, 317);
            this.tb_folder.Name = "tb_folder";
            this.tb_folder.Size = new System.Drawing.Size(289, 20);
            this.tb_folder.TabIndex = 19;
            this.tb_folder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tb_folder_MouseClick);
            // 
            // btn_zip
            // 
            this.btn_zip.Location = new System.Drawing.Point(283, 343);
            this.btn_zip.Name = "btn_zip";
            this.btn_zip.Size = new System.Drawing.Size(75, 23);
            this.btn_zip.TabIndex = 20;
            this.btn_zip.Text = "Zip";
            this.btn_zip.UseVisualStyleBackColor = true;
            this.btn_zip.Click += new System.EventHandler(this.btn_zip_Click);
            // 
            // btn_unzip
            // 
            this.btn_unzip.Location = new System.Drawing.Point(364, 343);
            this.btn_unzip.Name = "btn_unzip";
            this.btn_unzip.Size = new System.Drawing.Size(75, 23);
            this.btn_unzip.TabIndex = 21;
            this.btn_unzip.Text = "UnZip";
            this.btn_unzip.UseVisualStyleBackColor = true;
            this.btn_unzip.Click += new System.EventHandler(this.btn_unzip_Click);
            // 
            // FileDebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 377);
            this.Controls.Add(this.btn_unzip);
            this.Controls.Add(this.btn_zip);
            this.Controls.Add(this.tb_folder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tb_fileName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_pcap);
            this.Controls.Add(this.btn_pcap_start);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Name = "FileDebugForm";
            this.Text = "FileDebugForm";
            this.Load += new System.EventHandler(this.FileDebugForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_download_server;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.TextBox tb_upload_local;
        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_download_local;
        private System.Windows.Forms.TextBox tb_upload_server;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lb_status;
        private System.Windows.Forms.Button btn_pcap_start;
        private System.Windows.Forms.TextBox tb_pcap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_fileName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_folder;
        private System.Windows.Forms.Button btn_zip;
        private System.Windows.Forms.Button btn_unzip;
    }
}