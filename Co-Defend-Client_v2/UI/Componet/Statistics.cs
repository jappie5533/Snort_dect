using System;
using System.Windows.Forms;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class Statistics : UserControl
    {
        public Statistics()
        {
            InitializeComponent();

            this.Load += new EventHandler(Statistics_Load);
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            tabControl1.GotFocus += new EventHandler(tabControl1_GotFocus);
            panel2.MouseHover += new EventHandler(panel1_MouseEnter);
            panel3.MouseHover += new EventHandler(panel1_MouseEnter);
        }

        void label1_MouseHover(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            if (lb != null)
            {
                Panel pl = lb.Parent as Panel;
                if (pl != null)
                    pl.Focus();
            }
        }

        void tabControl1_GotFocus(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                panel2.Focus();
            else
                panel3.Focus();
        }

        void panel1_MouseEnter(object sender, EventArgs e)
        {
            Panel pl = sender as Panel;
            if (pl != null)
                pl.Focus();
        }

        public void StatisticUpdate(Object obj)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate
                {
                    dataGridView2.Rows.Clear();
                    dataGridView4.Rows.Clear();


                    foreach (string[] arrs in AgentController.Instance.ConnectionStatistics)
                    {
                        dataGridView2.Rows.Add(arrs);
                    }

                    if (Hub.BaseHub.Instance.ConnectionStatistics != null)
                    {
                        dataGridView4.Enabled = true;
                        foreach (string[] arrs in Hub.BaseHub.Instance.ConnectionStatistics)
                            dataGridView4.Rows.Add(arrs);
                    }
                    else
                        dataGridView4.Enabled = false;
                });
            else
            {
            }
        }
    }
}
