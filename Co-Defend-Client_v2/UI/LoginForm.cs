using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Hub;
using Lidgren.Network;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class LoginForm : Form
    {
        private static LoginForm _instance;

        public static LoginForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginForm();

                return _instance;
            }
        }

        private LoginForm()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            EnableComponent(true);
            this.Show();
            if (!tb_acct.Focused)
                tb_acct.Focus();
        }

        public void EnableComponent(bool enable)
        {
            lb_info.Visible = !enable;
            btn_login.Enabled = enable;

            if (enable)
            {
                tb_acct.Clear();
                tb_pwd.Clear();
                if (!tb_acct.Focused)
                    tb_acct.Focus();
            }
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(tb_acct.Text) || string.IsNullOrEmpty(tb_pwd.Text))
            {
                MessageBox.Show(this, "Account or Password can not be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (!tb_acct.Focused)
                    tb_acct.Focus();
            }
            else
            {
                EnableComponent(false);
#if DEBUG
                AgentController.Instance.Start(tb_acct.Text, tb_pwd.Text, rbtn_auto_select.Checked);
#else
                AgentController.Instance.Start(tb_acct.Text, tb_pwd.Text, true);
#endif
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            #region Set Form To Center
            Rectangle screenBound = Screen.PrimaryScreen.WorkingArea;
            int x = screenBound.Width / 2 - Width / 2;
            int y = screenBound.Height / 2 - Height / 2;

            SetBounds(x, y, Width, Height);
            #endregion

            EnableComponent(true);

            tb_acct.GotFocus += new EventHandler(tb_GotFocus);
            tb_pwd.GotFocus += new EventHandler(tb_GotFocus);

#if !DEBUG
            groupBox5.Visible = false;
            btn_start_hub.Visible = false;
            btn_stop_hub.Visible = false;
#endif
        }

        private void tb_GotFocus(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb != null)
                tb.SelectAll();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        #region Debug Mode
        // Show File Operation Form
        private void fileOperationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDebugForm fileForm = new FileDebugForm();
            fileForm.Show();
        }

        // Start Hub Daemon
        private void startHub_Click(object sender, EventArgs e)
        {
            Hub.BaseHub.Instance.onHubStatusChanged += new EventHandler<NetPeerStatusEventArgs>(CD2Hub_onHubStatusChanged);
            //CD2Hub.onHubStatusChanged += new EventHandler<NetPeerStatusEventArgs>(CD2Hub_onHubStatusChanged);
            List<string> hublist = new List<string>();
            hublist.Add("140.126.5.59");
            //hublist.Add("140.126.130.76");
            hublist.Add("140.126.21.72");
            hublist.Add("140.126.130.75");
            hublist.Add("140.126.130.74");
            //IPAddress mask;
            //hublist.Remove(NetUtility.GetMyAddress(out mask).ToString());
            Hub.BaseHub.Instance.Start("DEBUG", hublist);
            //CD2Hub.Start("DEBUG", hublist); //TODO: Tomato must modify here
        }

        // Stop Hub Daemon
        private void stopHub_Click(object sender, EventArgs e)
        {
            //CD2Hub.Shutdown();
            Hub.BaseHub.Instance.Stop();
        }

        // on Hub Status Changed
        void CD2Hub_onHubStatusChanged(object sender, NetPeerStatusEventArgs e)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { CD2Hub_onHubStatusChanged(sender, e); });
            else
            {
                switch (e.Status)
                {
                    case NetPeerStatus.NotRunning:
                        //CD2Hub.onHubStatusChanged -= CD2Hub_onHubStatusChanged;
                        Hub.BaseHub.Instance.onHubStatusChanged -= CD2Hub_onHubStatusChanged;
                        btn_start_hub.Enabled = true;
                        btn_stop_hub.Enabled = false;
                        break;
                    case NetPeerStatus.Running:
                        btn_start_hub.Enabled = false;
                        btn_stop_hub.Enabled = true;
                        break;
                }
            }
        }
        #endregion
    }
}
