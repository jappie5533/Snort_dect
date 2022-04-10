using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonTool;

namespace Co_Defend_Client_v2.UI
{
    public partial class ServicePlugin : UserControl
    {
        public ServicePlugin()
        {
            InitializeComponent();

            this.Load += new EventHandler(ServicePlugin_Load);
        }

        private void ServicePlugin_Load(object sender, EventArgs e)
        {
            AppendList();
            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox_ItemCheck);
            checkedListBox2.ItemCheck += new ItemCheckEventHandler(checkedListBox_ItemCheck);
            checkedListBox3.ItemCheck += new ItemCheckEventHandler(checkedListBox_ItemCheck);
            checkedListBox4.ItemCheck += new ItemCheckEventHandler(checkedListBox_ItemCheck);
            checkedListBox5.ItemCheck += new ItemCheckEventHandler(checkedListBox_ItemCheck);
            checkedListBox6.ItemCheck += new ItemCheckEventHandler(checkedListBox_ItemCheck);

            btn_reset.Click += new EventHandler(btn_reset_Click);
            btn_save.Click += new EventHandler(btn_save_Click);
        }

        void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!btn_save.Enabled)
            {
                btn_save.Enabled = true;
                btn_reset.Enabled = true;
            }
        }

        private void AppendList()
        {
            // Native
            ServiceList nlist = ServiceManager.Instance.NativeSvc;

            foreach (string s in nlist.SPA)
                checkedListBox1.Items.Add(s, true);

            foreach (string s in nlist.RAA)
                checkedListBox2.Items.Add(s, true);

            foreach (string s in nlist.PAA)
                checkedListBox3.Items.Add(s, true);

            // User-Define
            ServiceList ulist = ServiceManager.Instance.UserDefineSvc;

            foreach (string s in ulist.SPA)
                checkedListBox4.Items.Add(s, !ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Contains(s));

            foreach (string s in ulist.RAA)
                checkedListBox5.Items.Add(s, !ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Contains(s));

            foreach (string s in ulist.PAA)
                checkedListBox6.Items.Add(s, !ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Contains(s));
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            // User-Define
            for (int i = 0; i < checkedListBox4.Items.Count; i++)
                checkedListBox4.SetItemChecked(i, true);
            for (int i = 0; i < checkedListBox5.Items.Count; i++)
                checkedListBox5.SetItemChecked(i, true);
            for (int i = 0; i < checkedListBox6.Items.Count; i++)
                checkedListBox6.SetItemChecked(i, true);
            foreach (string s in ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service)
            {
                if (checkedListBox4.Items.Contains(s))
                    checkedListBox4.SetItemChecked(checkedListBox4.Items.IndexOf(s), false);
                else if (checkedListBox5.Items.Contains(s))
                    checkedListBox5.SetItemChecked(checkedListBox5.Items.IndexOf(s), false);
                else if (checkedListBox6.Items.Contains(s))
                    checkedListBox6.SetItemChecked(checkedListBox6.Items.IndexOf(s), false);
            }
            btn_save.Enabled = false;
            btn_reset.Enabled = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            btn_save.Enabled = false;
            btn_reset.Enabled = false;

            ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Clear();

            for (int i = 0; i < checkedListBox4.Items.Count; i++)
                if (!checkedListBox4.GetItemChecked(i))
                    ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Add(checkedListBox4.Items[i].ToString());

            for (int i = 0; i < checkedListBox5.Items.Count; i++)
                if (!checkedListBox5.GetItemChecked(i))
                    ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Add(checkedListBox5.Items[i].ToString());

            for (int i = 0; i < checkedListBox6.Items.Count; i++)
                if (!checkedListBox6.GetItemChecked(i))
                    ServiceManager.Instance.ServiceConf.Not_Allow_User_Define_Service.Add(checkedListBox6.Items[i].ToString());

            ServiceManager.Instance.SaveConfig();
        }
    }
}
