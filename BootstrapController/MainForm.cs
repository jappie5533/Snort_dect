using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BootstrapController
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Bootstrap.Bootstrap.Instance.onDebugMessage += new EventHandler<CommonTool.MsgEventArgs>(Instance_onDebugMessage);
        }

        void Instance_onDebugMessage(object sender, CommonTool.MsgEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Instance_onDebugMessage(sender, e); });
            }
            else
            {
                richTextBox1.AppendText(string.Format("[{0}] {1}", DateTime.Now, e.Message));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bootstrap.Bootstrap.Instance.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bootstrap.Bootstrap.Instance.Stop();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Bootstrap.Bootstrap.Instance.Stop();
        }
    }
}
