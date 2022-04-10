using System;
using System.Windows.Forms;
using CommonTool;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class Command : UserControl
    {
        public Command()
        {
            InitializeComponent();

            EventRegist();
        }

        public void UpdateLog(string log_text)
        {
            richTxtBoxLog.AppendTextAutoClear(log_text);
        }

        private void EventRegist()
        {
            this.Load += delegate(object sender, EventArgs e)
            {
                tb_cmd.Focus();
            };

            this.GotFocus += delegate(object sender, EventArgs e)
            {
                tb_cmd.Focus();
            };

            btn_send.Click += new EventHandler(btn_send_Click);
            tb_cmd.KeyDown += new KeyEventHandler(tb_cmd_KeyDown);
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void tb_cmd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Send();
        }

        private void Send()
        {
            if (!string.IsNullOrEmpty(tb_cmd.Text))
            {
                string cmd = tb_cmd.Text;
                tb_cmd.Clear();

                DataProtocol dp = new DataProtocol();
                DPType tmp;

                dp.Action = DPAction.Request;
                dp.Content = CommandUtility.ToJson(cmd, out tmp);
                dp.Type = tmp;

                AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);
            }

            if (!tb_cmd.Focused)
                tb_cmd.Focus();
        }
    }
}
