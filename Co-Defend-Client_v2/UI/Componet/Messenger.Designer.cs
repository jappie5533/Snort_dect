namespace Co_Defend_Client_v2.UI
{
    partial class Messenger
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.richTxtBoxWriter = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.richTxtBoxLog = new System.Windows.Forms.RichTextBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.richTxtBoxWriter);
            this.groupBox4.Controls.Add(this.btnSend);
            this.groupBox4.Controls.Add(this.richTxtBoxLog);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox4.Size = new System.Drawing.Size(791, 330);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Message";
            // 
            // richTxtBoxWriter
            // 
            this.richTxtBoxWriter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTxtBoxWriter.Location = new System.Drawing.Point(5, 265);
            this.richTxtBoxWriter.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.richTxtBoxWriter.Name = "richTxtBoxWriter";
            this.richTxtBoxWriter.Size = new System.Drawing.Size(674, 61);
            this.richTxtBoxWriter.TabIndex = 3;
            this.richTxtBoxWriter.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(679, 264);
            this.btnSend.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(107, 62);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // richTxtBoxLog
            // 
            this.richTxtBoxLog.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTxtBoxLog.Location = new System.Drawing.Point(5, 23);
            this.richTxtBoxLog.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.richTxtBoxLog.Name = "richTxtBoxLog";
            this.richTxtBoxLog.ReadOnly = true;
            this.richTxtBoxLog.Size = new System.Drawing.Size(781, 241);
            this.richTxtBoxLog.TabIndex = 2;
            this.richTxtBoxLog.TabStop = false;
            this.richTxtBoxLog.Text = "";
            // 
            // Messenger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "Messenger";
            this.Size = new System.Drawing.Size(791, 330);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox richTxtBoxWriter;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox richTxtBoxLog;

    }
}
