using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonTool;
using Agent;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class HubListView : UserControl
    {
        public HubListView()
        {
            InitializeComponent();

#if DEBUG
            lv_hub_info.MouseDoubleClick += delegate(object sender, MouseEventArgs e)
            {
                ListView lv = sender as ListView;

                if (lv != null && lv.SelectedItems.Count > 0)
                {
                    AgentController.Instance.GetHubCert(lv.SelectedItems[0].SubItems[0].Text);
                }
            };
#endif
        }

        public void UpdateHubList(List<string> list)
        {
            lv_hub_info.BeginUpdate();
            lv_hub_info.Items.Clear();
            foreach (string s in list)
            {
                ListViewItem item = new ListViewItem(new string[] { s, CD2Constant.HubPort.ToString(), "-1", "Unconnected." });
                item.Name = s;

                if (!lv_hub_info.Items.ContainsKey(s))
                    lv_hub_info.Items.Add(item);
            }
            lv_hub_info.EndUpdate();
        }

        public void UpdateHubCert(HubCertEventArgs e)
        {
            if (lv_hub_info.Items.ContainsKey(e.Hub_IP))
                lv_hub_info.Items[e.Hub_IP].SubItems[3].Text = "Connected.";

            foreach (KeyValuePair<string, RoundTripTime> pair in e.Hub_RTT)
            {
                if (lv_hub_info.Items.ContainsKey(pair.Key))
                {
                    if (pair.Value.Return)
                        lv_hub_info.Items[pair.Key].SubItems[2].Text = pair.Value.TotalTime + " s.";
                    else
                        lv_hub_info.Items[pair.Key].SubItems[2].Text = "Timeout.";
                }
            }
        }
    }
}
