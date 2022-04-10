namespace Co_Defend_Client_v2.UI
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.lb_acct = new System.Windows.Forms.Label();
            this.lb_pwd = new System.Windows.Forms.Label();
            this.tb_acct = new System.Windows.Forms.TextBox();
            this.tb_pwd = new System.Windows.Forms.TextBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbtn_auto_select = new System.Windows.Forms.RadioButton();
            this.radio_hub_manu_select = new System.Windows.Forms.RadioButton();
            this.lb_info = new System.Windows.Forms.Label();
            this.btn_start_hub = new System.Windows.Forms.Button();
            this.btn_stop_hub = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_acct
            // 
            this.lb_acct.AutoSize = true;
            this.lb_acct.Location = new System.Drawing.Point(23, 76);
            this.lb_acct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_acct.Name = "lb_acct";
            this.lb_acct.Size = new System.Drawing.Size(73, 18);
            this.lb_acct.TabIndex = 0;
            this.lb_acct.Text = "Account：";
            // 
            // lb_pwd
            // 
            this.lb_pwd.AutoSize = true;
            this.lb_pwd.Location = new System.Drawing.Point(15, 120);
            this.lb_pwd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_pwd.Name = "lb_pwd";
            this.lb_pwd.Size = new System.Drawing.Size(81, 18);
            this.lb_pwd.TabIndex = 1;
            this.lb_pwd.Text = "Password：";
            // 
            // tb_acct
            // 
            this.tb_acct.Location = new System.Drawing.Point(117, 73);
            this.tb_acct.Margin = new System.Windows.Forms.Padding(4);
            this.tb_acct.Name = "tb_acct";
            this.tb_acct.Size = new System.Drawing.Size(152, 26);
            this.tb_acct.TabIndex = 2;
            this.tb_acct.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            // 
            // tb_pwd
            // 
            this.tb_pwd.Location = new System.Drawing.Point(117, 117);
            this.tb_pwd.Margin = new System.Windows.Forms.Padding(4);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.PasswordChar = '*';
            this.tb_pwd.Size = new System.Drawing.Size(152, 26);
            this.tb_pwd.TabIndex = 3;
            this.tb_pwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_KeyDown);
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(285, 73);
            this.btn_login.Margin = new System.Windows.Forms.Padding(4);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(100, 65);
            this.btn_login.TabIndex = 4;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbtn_auto_select);
            this.groupBox5.Controls.Add(this.radio_hub_manu_select);
            this.groupBox5.Location = new System.Drawing.Point(13, 6);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(179, 62);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Hub Select Mode";
            // 
            // rbtn_auto_select
            // 
            this.rbtn_auto_select.AutoSize = true;
            this.rbtn_auto_select.Location = new System.Drawing.Point(8, 26);
            this.rbtn_auto_select.Margin = new System.Windows.Forms.Padding(4);
            this.rbtn_auto_select.Name = "rbtn_auto_select";
            this.rbtn_auto_select.Size = new System.Drawing.Size(49, 19);
            this.rbtn_auto_select.TabIndex = 10;
            this.rbtn_auto_select.Text = "Auto";
            this.rbtn_auto_select.UseVisualStyleBackColor = true;
            // 
            // radio_hub_manu_select
            // 
            this.radio_hub_manu_select.AutoSize = true;
            this.radio_hub_manu_select.Checked = true;
            this.radio_hub_manu_select.Location = new System.Drawing.Point(81, 26);
            this.radio_hub_manu_select.Margin = new System.Windows.Forms.Padding(4);
            this.radio_hub_manu_select.Name = "radio_hub_manu_select";
            this.radio_hub_manu_select.Size = new System.Drawing.Size(67, 19);
            this.radio_hub_manu_select.TabIndex = 11;
            this.radio_hub_manu_select.TabStop = true;
            this.radio_hub_manu_select.Text = "Manual";
            this.radio_hub_manu_select.UseVisualStyleBackColor = true;
            // 
            // lb_info
            // 
            this.lb_info.AutoSize = true;
            this.lb_info.Location = new System.Drawing.Point(156, 171);
            this.lb_info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_info.Name = "lb_info";
            this.lb_info.Size = new System.Drawing.Size(88, 18);
            this.lb_info.TabIndex = 15;
            this.lb_info.Text = "Please Wait...";
            // 
            // btn_start_hub
            // 
            this.btn_start_hub.Location = new System.Drawing.Point(202, 22);
            this.btn_start_hub.Margin = new System.Windows.Forms.Padding(4);
            this.btn_start_hub.Name = "btn_start_hub";
            this.btn_start_hub.Size = new System.Drawing.Size(91, 32);
            this.btn_start_hub.TabIndex = 16;
            this.btn_start_hub.Text = "Start Hub";
            this.btn_start_hub.UseVisualStyleBackColor = true;
            this.btn_start_hub.Click += new System.EventHandler(this.startHub_Click);
            // 
            // btn_stop_hub
            // 
            this.btn_stop_hub.Enabled = false;
            this.btn_stop_hub.Location = new System.Drawing.Point(301, 22);
            this.btn_stop_hub.Margin = new System.Windows.Forms.Padding(4);
            this.btn_stop_hub.Name = "btn_stop_hub";
            this.btn_stop_hub.Size = new System.Drawing.Size(91, 32);
            this.btn_stop_hub.TabIndex = 17;
            this.btn_stop_hub.Text = "Stop Hub";
            this.btn_stop_hub.UseVisualStyleBackColor = true;
            this.btn_stop_hub.Click += new System.EventHandler(this.stopHub_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 217);
            this.Controls.Add(this.btn_stop_hub);
            this.Controls.Add(this.btn_start_hub);
            this.Controls.Add(this.lb_info);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.tb_pwd);
            this.Controls.Add(this.tb_acct);
            this.Controls.Add(this.lb_pwd);
            this.Controls.Add(this.lb_acct);
            this.Font = new System.Drawing.Font("Comic Sans MS", 9.267326F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_acct;
        private System.Windows.Forms.Label lb_pwd;
        private System.Windows.Forms.TextBox tb_acct;
        private System.Windows.Forms.TextBox tb_pwd;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbtn_auto_select;
        private System.Windows.Forms.RadioButton radio_hub_manu_select;
        private System.Windows.Forms.Label lb_info;
        private System.Windows.Forms.Button btn_start_hub;
        private System.Windows.Forms.Button btn_stop_hub;
    }
}