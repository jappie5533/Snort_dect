namespace Co_Defend_Client_v2.UI
{
    partial class HubListView
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
            this.lv_hub_info = new System.Windows.Forms.ListView();
            this.ch_hub_ip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_hub_port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_hub_RTT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_hub_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv_hub_info
            // 
            this.lv_hub_info.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_hub_ip,
            this.ch_hub_port,
            this.ch_hub_RTT,
            this.ch_hub_status});
            this.lv_hub_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_hub_info.FullRowSelect = true;
            this.lv_hub_info.GridLines = true;
            this.lv_hub_info.Location = new System.Drawing.Point(4, 23);
            this.lv_hub_info.Margin = new System.Windows.Forms.Padding(4);
            this.lv_hub_info.Name = "lv_hub_info";
            this.lv_hub_info.Size = new System.Drawing.Size(777, 146);
            this.lv_hub_info.TabIndex = 0;
            this.lv_hub_info.UseCompatibleStateImageBehavior = false;
            this.lv_hub_info.View = System.Windows.Forms.View.Details;
            // 
            // ch_hub_ip
            // 
            this.ch_hub_ip.Text = "Hub IP";
            this.ch_hub_ip.Width = 192;
            // 
            // ch_hub_port
            // 
            this.ch_hub_port.Text = "Hub Port";
            this.ch_hub_port.Width = 107;
            // 
            // ch_hub_RTT
            // 
            this.ch_hub_RTT.Text = "Hub RTT";
            this.ch_hub_RTT.Width = 137;
            // 
            // ch_hub_status
            // 
            this.ch_hub_status.Text = "Hub Status";
            this.ch_hub_status.Width = 335;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lv_hub_info);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(785, 173);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hub Infomation";
            // 
            // HubListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HubListView";
            this.Size = new System.Drawing.Size(785, 173);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lv_hub_info;
        private System.Windows.Forms.ColumnHeader ch_hub_ip;
        private System.Windows.Forms.ColumnHeader ch_hub_port;
        private System.Windows.Forms.ColumnHeader ch_hub_status;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader ch_hub_RTT;

    }
}
