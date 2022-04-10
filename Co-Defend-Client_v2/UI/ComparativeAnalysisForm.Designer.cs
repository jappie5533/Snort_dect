namespace Co_Defend_Client_v2.UI
{
    partial class ComparativeAnalysisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComparativeAnalysisForm));
            this.analysisChart2 = new Co_Defend_Client_v2.UI.AnalysisChart();
            this.analysisChart1 = new Co_Defend_Client_v2.UI.AnalysisChart();
            this.SuspendLayout();
            // 
            // analysisChart2
            // 
            this.analysisChart2.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.analysisChart2.Location = new System.Drawing.Point(630, 2);
            this.analysisChart2.Margin = new System.Windows.Forms.Padding(4);
            this.analysisChart2.Name = "analysisChart2";
            this.analysisChart2.Size = new System.Drawing.Size(620, 655);
            this.analysisChart2.TabIndex = 1;
            // 
            // analysisChart1
            // 
            this.analysisChart1.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F);
            this.analysisChart1.Location = new System.Drawing.Point(2, 2);
            this.analysisChart1.Margin = new System.Windows.Forms.Padding(4);
            this.analysisChart1.Name = "analysisChart1";
            this.analysisChart1.Size = new System.Drawing.Size(620, 655);
            this.analysisChart1.TabIndex = 0;
            // 
            // ComparativeAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 660);
            this.Controls.Add(this.analysisChart2);
            this.Controls.Add(this.analysisChart1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ComparativeAnalysisForm";
            this.Text = "Comparative Analysis Viewer";
            this.Load += new System.EventHandler(this.ComparativeAnalysisForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private AnalysisChart analysisChart1;
        private AnalysisChart analysisChart2;

    }
}