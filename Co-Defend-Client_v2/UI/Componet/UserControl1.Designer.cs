namespace Co_Defend_Client_v2.UI
{
    partial class BuiltinApps
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
            this.tp_click = new System.Windows.Forms.TabPage();
            this.publishService1 = new Co_Defend_Client_v2.UI.PublishService();
            this.tp_codefence = new System.Windows.Forms.TabPage();
            this.analyticalAppUserControl1 = new Co_Defend_Client_v2.UI.AnalyticalAppUserControl();
            this.tabControl1.SuspendLayout();
            this.tp_click.SuspendLayout();
            this.tp_codefence.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp_click);
            this.tabControl1.Controls.Add(this.tp_codefence);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(882, 400);
            this.tabControl1.TabIndex = 0;
            // 
            // tp_click
            // 
            this.tp_click.Controls.Add(this.publishService1);
            this.tp_click.Location = new System.Drawing.Point(4, 22);
            this.tp_click.Name = "tp_click";
            this.tp_click.Padding = new System.Windows.Forms.Padding(3);
            this.tp_click.Size = new System.Drawing.Size(874, 374);
            this.tp_click.TabIndex = 0;
            this.tp_click.Text = "Click & Run";
            this.tp_click.UseVisualStyleBackColor = true;
            // 
            // publishService1
            // 
            this.publishService1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.publishService1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.publishService1.Location = new System.Drawing.Point(3, 3);
            this.publishService1.Margin = new System.Windows.Forms.Padding(4);
            this.publishService1.Name = "publishService1";
            this.publishService1.Size = new System.Drawing.Size(868, 368);
            this.publishService1.TabIndex = 0;
            // 
            // tp_codefence
            // 
            this.tp_codefence.Controls.Add(this.analyticalAppUserControl1);
            this.tp_codefence.Location = new System.Drawing.Point(4, 22);
            this.tp_codefence.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.tp_codefence.Name = "tp_codefence";
            this.tp_codefence.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tp_codefence.Size = new System.Drawing.Size(777, 340);
            this.tp_codefence.TabIndex = 1;
            this.tp_codefence.Text = "Cooperative Defense";
            this.tp_codefence.UseVisualStyleBackColor = true;
            // 
            // analyticalAppUserControl1
            // 
            this.analyticalAppUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.analyticalAppUserControl1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.analyticalAppUserControl1.Location = new System.Drawing.Point(3, 3);
            this.analyticalAppUserControl1.Margin = new System.Windows.Forms.Padding(0);
            this.analyticalAppUserControl1.Name = "analyticalAppUserControl1";
            this.analyticalAppUserControl1.Size = new System.Drawing.Size(771, 337);
            this.analyticalAppUserControl1.TabIndex = 0;
            // 
            // BuiltinApps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "BuiltinApps";
            this.Size = new System.Drawing.Size(882, 400);
            this.tabControl1.ResumeLayout(false);
            this.tp_click.ResumeLayout(false);
            this.tp_codefence.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_codefence;
        private System.Windows.Forms.TabPage tp_click;
        private PublishService publishService1;
        private AnalyticalAppUserControl analyticalAppUserControl1;
    }
}
