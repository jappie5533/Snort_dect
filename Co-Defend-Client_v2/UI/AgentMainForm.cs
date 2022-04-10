using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CommonTool;
using Lidgren.Network;
using Hub;
using Co_Defend_Client_v2.Agent;
using CommonTool.FileStreamTool;

namespace Co_Defend_Client_v2.UI
{
    internal partial class AgentMainForm : Form
    {
        private System.Threading.Timer _timer;
        private int _form_fix_height;
        private int _form_fix_width;

        public AgentMainForm()
        {
            InitializeComponent();
            _form_fix_height = this.Size.Height;
            _form_fix_width = this.Size.Width;
        }

        public void ShowForm()
        {
            lb_uname.Text = string.Format("User Name： {0}", AgentController.Instance.UserName);
            lb_agent_type.Text = string.Format("Agent Type： {0}", AgentController.Instance.AgentType);
            EnableComponent(false, "Status: Selecting Hub...");
            //EnableComponent(false, "Status: Connected to Hub (140.126.130.43:50001)");
            tabControl1.SelectedTab = tp_main;
            messenger1.Focus();
            this.Show();
        }

        private void AgentMainForm_Load(object sender, EventArgs e)
        {
            SetCollapsed();

            EventRegist();
            _timer = new System.Threading.Timer(new System.Threading.TimerCallback(statistics1.StatisticUpdate), null, 0, 3000);
            
#if !DEBUG
            btn_file.Visible = false;
            btn_start_hub.Visible = false;
            btn_stop_hub.Visible = false;
#endif
        }

        // Registrate Event
        private void EventRegist()
        {
            // Agent Log Message Receive Event.
            AgentController.Instance.onLogMsgRecv += delegate(object sender, MsgEventArgs e)
            {
                if (!command1.IsDisposed)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)delegate { command1.UpdateLog(e.Message); });
                    else
                        command1.UpdateLog(e.Message);
                }
            };

            // Agent Chat Message Receive Event.
            AgentController.Instance.onChatMsgRecv += delegate(object sender, ChatMsgEventArgs e)
            {
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate
                    {
                        messenger1.RecvChatMsg(e);
                    });
                else
                {
                    messenger1.RecvChatMsg(e);
                }
            };

            // Agent Status Event.
            AgentController.Instance.onStatusMsg += delegate(object sender, StatusMsgArgs e)
            {
                switch (e.Code)
                {
                    case StatusCode.Agent_Connected:
                        lb_agent_type.Text = string.Format("Agent Type： {0}", AgentController.Instance.AgentType);
                        EnableComponent(true, "Status: Connected to Hub (" + e.Message + ")");
                        break;
                    case StatusCode.Agent_Disconnected:
                        EnableComponent(false, "Status: Disconnected.");
                        break;
                }
            };

            // Agent Receive Hub List Event.
            AgentController.Instance.onHublist += delegate(object sender, HubListEventArgs e)
            {
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate
                    {
                        hubListView1.UpdateHubList(e.Hub_List);
                    });
                else
                {
                    hubListView1.UpdateHubList(e.Hub_List);
                }
            };

            AgentController.Instance.onHubInfo += delegate(object sender, HubCertEventArgs e)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        hubListView1.UpdateHubCert(e);
                    });
                }
                else
                    hubListView1.UpdateHubCert(e);
            };

            AgentController.Instance.onPublishRecv += delegate(object sender, PublishEventArgs e)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        downloadService1.Enqueue(e.Content);
                    });
                }
                else
                {
                    downloadService1.Enqueue(e.Content);
                }
            };

            // Main Form Close Event.
            this.FormClosing += delegate(object sender, FormClosingEventArgs e)
            {
                _timer.Dispose();
                AgentController.Instance.Stop();
            };
            
            tabControl1.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                TabControl tc = sender as TabControl;

                if (tc != null)
                {
                    if (tc.SelectedTab.Equals(tp_main))
                    {
                        messenger1.Focus();
                    }
                    else if (tc.SelectedTab.Equals(tp_logout))
                    {
                        DialogResult dr = MessageBox.Show(this, "Do you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {
                            AgentController.Instance.Stop();
                            this.Hide();
                            LoginForm.Instance.ShowForm();
                        }
                        else
                            tc.SelectedTab = tp_main;
                    }
                }
            };

#if DEBUG
            // File Progress Update Event.
            FileHandler.Instance.onProgressUpdate += delegate(object sndr, FileProgressEventArgs fe)
            {
                if (!command1.IsDisposed)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)delegate { command1.UpdateLog(fe.ProgressInfo + Environment.NewLine); });
                    else
                        command1.UpdateLog(fe.ProgressInfo + Environment.NewLine);
                }
            };

            // File Status Update Event.
            FileHandler.Instance.onStatusUpdate += delegate(object sndr, FileStatusEventArgs sme)
            {
                if (!command1.IsDisposed)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)delegate { command1.UpdateLog(sme.Reason + Environment.NewLine); });
                    else
                        command1.UpdateLog(sme.Reason + Environment.NewLine);
                }
            };

            // Show File Operation Form
            btn_file.Click += delegate(object sender, EventArgs e)
            {
                FileDebugForm fileForm = new FileDebugForm();
                fileForm.Show(this);
            };

            btn_start_hub.Click += new EventHandler(btn_start_hub_Click);
            btn_stop_hub.Click += new EventHandler(btn_stop_hub_Click);
#endif
        }

        private void EnableComponent(bool isEnable, string reason)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (InvokeRequired)
                        Invoke((MethodInvoker)delegate { EnableComponent(isEnable, reason); });
                    else
                    {
                        //if (!isEnable)
                        //    list_hub.Items.Clear();
                        //list_hub.Enabled = !isEnable;
                        toolStatus.Text = reason;
                    }
                }
            }
            catch
            {
            }
        }

        #region Debug Mode

        // Start Hub Daemon
        private void btn_start_hub_Click(object sender, EventArgs e)
        {
            Hub.BaseHub.Instance.onHubStatusChanged += new EventHandler<NetPeerStatusEventArgs>(CD2Hub_onHubStatusChanged);
            //CD2Hub.onHubStatusChanged += new EventHandler<NetPeerStatusEventArgs>(CD2Hub_onHubStatusChanged);
            List<string> hublist = new List<string>();
            hublist.Add("140.126.130.75");
            hublist.Add("140.126.130.76");
            hublist.Add("140.126.130.77");
            //IPAddress mask;
            //hublist.Remove(NetUtility.GetMyAddress(out mask).ToString());
            Hub.BaseHub.Instance.Start(AgentController.Instance.UserName, hublist);
            //CD2Hub.Start(AgentController.Instance.UserName, hublist); //TODO: Tomato must modify here
        }

        // Stop Hub Daemon
        private void btn_stop_hub_Click(object sender, EventArgs e)
        {
            Hub.BaseHub.Instance.Stop();
            //CD2Hub.Shutdown();
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
                        Hub.BaseHub.Instance.onHubStatusChanged -= CD2Hub_onHubStatusChanged;
                        //CD2Hub.onHubStatusChanged -= CD2Hub_onHubStatusChanged;
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

        private void button1_Click(object sender, EventArgs e)
        {
            SetCollapsed();
        }

        private void SetCollapsed()
        {
            if (splitContainer1.Panel2Collapsed)
            {
                // Will be to discollapsed.
                this.button1.Image = global::Co_Defend_Client_v2.Properties.Resources.minus;
                this.Height = _form_fix_height;
            }
            else
            {
                this.button1.Image = global::Co_Defend_Client_v2.Properties.Resources.plus;
                this.Height = _form_fix_height - tabControl2.Height;
            }
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;

            #region Set Form To Center
            Rectangle screenBound = Screen.PrimaryScreen.WorkingArea;
            int x = screenBound.Width / 2 - Width / 2;
            int y = screenBound.Height / 2 - Height / 2;

            SetBounds(x, y, Width, Height);
            #endregion
        }
    }
}
