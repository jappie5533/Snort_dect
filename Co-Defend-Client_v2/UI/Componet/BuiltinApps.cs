using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Co_Defend_Client_v2.UI
{
    public partial class BuiltinApps : UserControl
    {
        public BuiltinApps()
        {
            InitializeComponent();

            tabControl1.SelectedIndexChanged += (sender, e) =>
            {
                if (tabControl1.SelectedTab.Equals(tp_codefence))
                    analyticalAppUserControl1.RefreshTable();
            };
        }
    }
}
