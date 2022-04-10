using System;
using System.Windows.Forms;
using CommonTool;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class Messenger : UserControl
    {
        public Messenger()
        {
            InitializeComponent();

            EventRegist();
        }

        public void RecvChatMsg(ChatMsgEventArgs args)
        {
            string info = "[" + DateTime.FromBinary(args.RecvTimeStamp).ToShortDateString() + " from: " + args.Sender + "]: ";
            richTxtBoxLog.AppendTextAutoClear(info + args.Message + Environment.NewLine);
        }

        public void RecvChatMsg(string msg)
        {
            string info = "[" + DateTime.Now.ToShortDateString() + " from: me]: ";
            richTxtBoxLog.AppendTextAutoClear(info + msg + Environment.NewLine);
        }

        private void EventRegist()
        {
            richTxtBoxWriter.TextChanged += delegate(object sender, EventArgs e)
            {
                if (richTxtBoxWriter.Text.Equals("\n"))
                    richTxtBoxWriter.Clear();

                if (richTxtBoxWriter.TextLength == 0)
                    btnSend.Enabled = false;
                else
                    btnSend.Enabled = true;
            };

            richTxtBoxWriter.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter && e.Shift)
                    e.SuppressKeyPress = false;
                else if (e.KeyCode == Keys.Enter && btnSend.Enabled)
                {
                    richTxtBoxWriter.Text = richTxtBoxWriter.Text.TrimEnd('\n');
                    btnSend_Click(this, e);
                    richTxtBoxWriter.Clear();
                    e.SuppressKeyPress = true;
                }
            };

            btnSend.Click += new EventHandler(btnSend_Click);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            DataProtocol dp = new DataProtocol();
            MessageContent mc = new MessageContent();

            mc.Sender = AgentController.Instance.UserName;
            mc.Content = richTxtBoxWriter.Text;

            dp.Action = DPAction.Request;
            dp.Content = mc.ToJson();
            dp.Type = DPType.Msg;

            RecvChatMsg(richTxtBoxWriter.Text);
            richTxtBoxWriter.Clear();

            AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);
        }
    }
}
