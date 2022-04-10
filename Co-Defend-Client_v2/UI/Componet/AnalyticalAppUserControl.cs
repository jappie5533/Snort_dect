using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonTool;
using Co_Defend_Client_v2.Agent;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Co_Defend_Client_v2.UI
{
    public partial class AnalyticalAppUserControl : UserControl
    {
        private int _backLogListIndex;
        private bool _isFirst;
        private string _tableName;
        private List<string> _susIPChekcList;

        public AnalyticalAppUserControl()
        {
            InitializeComponent();

            RegistEvents();

            _backLogListIndex = 0;
            _isFirst = true;
            _susIPChekcList = new List<string>();
            btn_analysis.Enabled = false;
            btn_block.Enabled = false;
        }

        private void RegistEvents()
        {
            // Virtual Gateway Event Regist.
            tb_vg_excuteTime.TextChanged += new EventHandler(tb_TextChanged);
            tb_vg_logName.TextChanged +=new EventHandler(tb_TextChanged);
            tb_vg_logName.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            tb_vg_excuteTime.KeyPress += new KeyPressEventHandler(tb_KeyPress);
            btn_startService.Click += delegate(object sender, EventArgs e)
            {
                string log = tb_vg_logName.Text.Replace(' ', '_');
                string target = DataUtility.IsPrivateIP(AgentController.Instance.Agent_IP) ?
                                    AgentController.Instance.Agent_ID.ToString() :
                                    AgentController.Instance.Agent_IP.ToString();
                int excutedTime = int.Parse(tb_vg_excuteTime.Text);

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("run VirtualGateway {0} {1} {2}", log, target, excutedTime);

                DataProtocol dp = new DataProtocol();
                DPType type;

                dp.Action = DPAction.Request;
                dp.Content = CommandUtility.ToJson(sb.ToString(), out type);
                dp.Type = type;

                AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);

                tb_vg_logName.Clear();
                tb_vg_excuteTime.Clear();
            };
            AgentController.Instance.onBackLogRecv += new EventHandler<BackLogEventArgs>(Instance_onBackLogRecv);
            btn_startParsing.Click += new EventHandler(parsingNext);

            lv_tables.ItemCheck += (sender, e) =>
            {
                if (lv_tables.CheckedIndices.Count >= 2)
                    e.NewValue = CheckState.Unchecked;

                btn_analysis.Enabled = lv_tables.CheckedIndices.Count == 0 && e.NewValue == CheckState.Checked ||
                                       lv_tables.CheckedIndices.Count == 1 && e.NewValue != CheckState.Unchecked ||
                                       lv_tables.CheckedIndices.Count > 1;
            };

            btn_analysis.Click += (sender, e) =>
            {
                if (lv_tables.CheckedIndices.Count == 2)
                {
                    ComparativeAnalysisForm form = new ComparativeAnalysisForm(lv_tables.CheckedItems[0].SubItems[0].Text, lv_tables.CheckedItems[0].SubItems[1].Text, lv_tables.CheckedItems[1].SubItems[0].Text, lv_tables.CheckedItems[1].SubItems[1].Text);
                    form.Show();
                }
                else
                {
                    AnalysisForm form = new AnalysisForm(lv_tables.CheckedItems[0].SubItems[0].Text, lv_tables.CheckedItems[0].SubItems[1].Text);
                    form.Show();
                }
            };

            tabControl1.SelectedIndexChanged += (sender, e) =>
            {
                if (tabControl1.SelectedTab.Equals(tp_filter))
                    dgv_sus_ip.DataSource = Utils.AgentAnalysisUtility.Instance.ObtainSuspiciousTargetTable();
            };

            dgv_sus_ip.CellValueChanged += (sender, e) =>
            {
                _susIPChekcList.Clear();

                foreach (DataGridViewRow row in dgv_sus_ip.Rows)
                {
                    DataGridViewCheckBoxCell chkCell = row.Cells["Blocked"] as DataGridViewCheckBoxCell;

                    if (chkCell.Value != null && chkCell.Value.GetType() != typeof(DBNull) && Convert.ToBoolean(chkCell.Value))
                        _susIPChekcList.Add(row.Cells["Source_IP"].Value.ToString());
                }

                refreshFilterCmd(sender, e);
            };
            tb_ft_blockIP.TextChanged += new EventHandler(refreshFilterCmd);
            tb_ft_excutionTime.TextChanged += new EventHandler(refreshFilterCmd);
            tb_ft_logName.TextChanged += new EventHandler(refreshFilterCmd);

            btn_block.Click += (sender, e) =>
            {
                DataProtocol dp = new DataProtocol();
                DPType type;

                dp.Action = DPAction.Request;
                dp.Content = CommandUtility.ToJson(tb_ft_cmd.Text, out type);
                dp.Type = type;

                AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);

                tb_ft_cmd.Clear();
                btn_block.Enabled = false;
            };
        }

        private void refreshFilterCmd(object sender, EventArgs e)
        {
            int excutionTime;
            Regex regex = new Regex(@"(1?[1-9]?[0-9]|2?(?:[0-4]?[0-9]|5[0-5]))\.(1?[1-9]?[0-9]|2?(?:[0-4]?[0-9]|5[0-5]))\.(1?[1-9]?[0-9]|2?(?:[0-4]?[0-9]|5[0-5]))\.(1?[1-9]?[0-9]|2?(?:[0-4]?[0-9]|5[0-5]))\b");

            tb_ft_cmd.Clear();

            if (!string.IsNullOrEmpty(tb_ft_logName.Text) && int.TryParse(tb_ft_excutionTime.Text, out excutionTime) && (_susIPChekcList.Count > 0 || regex.IsMatch(tb_ft_blockIP.Text)))
            {   
                StringBuilder sb = new StringBuilder();
                string target = DataUtility.IsPrivateIP(AgentController.Instance.Agent_IP) ? AgentController.Instance.Agent_ID.ToString() : AgentController.Instance.Agent_IP.ToString();
                string blockIPs = string.Empty;

                foreach (string str in _susIPChekcList)
                    blockIPs += str + ",";

                foreach (var matchItem in regex.Matches(tb_ft_blockIP.Text))
                    blockIPs += matchItem.ToString() + ",";
                
                sb.AppendFormat("run Filtering {0} {1} {2} {3} 1", tb_ft_logName.Text, target, excutionTime, blockIPs.TrimEnd(','));

                tb_ft_cmd.Text = sb.ToString();

                btn_block.Enabled = true;
            }
            else
                btn_block.Enabled = false;
        }

        private void Instance_onBackLogRecv(object sender, BackLogEventArgs e)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { Instance_onBackLogRecv(sender, e); });
            else
            {
                string filePath = Path.Combine(ServiceLogger.Instance.BackLogPath, e.LogName);
                ListViewItem lvi = new ListViewItem(new string[] { Path.GetFileName(filePath), filePath, "Queued." });

                lv_backLogList.BeginUpdate();
                lv_backLogList.Items.Add(lvi);
                lv_backLogList.EndUpdate();

                btn_startParsing.Enabled = true;
            }
        }

        private void parsingNext(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { parsingNext(sender, e); });
            else
            {
                if (sender is Button)
                    _tableName = "_" + DateTime.Now.ToTimeStamp().ToString();

                if (!_isFirst)
                    lv_backLogList.Items[_backLogListIndex - 1].SubItems[2].Text = "Completed.";
                else
                {
                    Utils.AgentAnalysisUtility.Instance.onPcapToSQLiteCompleted += new EventHandler<EventArgs>(parsingNext);
                    _isFirst = false;
                }

                pgb_parsing.Value = _backLogListIndex / lv_backLogList.Items.Count * 100;

                if (_backLogListIndex < lv_backLogList.Items.Count)
                {
                    string parsingFilePath = lv_backLogList.Items[_backLogListIndex].SubItems[1].Text;
                    lv_backLogList.Items[_backLogListIndex++].SubItems[2].Text = "Parsing...";

                    Utils.AgentAnalysisUtility.Instance.PcapToSQLite(_tableName, parsingFilePath);
                }
                else
                {
                    RefreshTable();
                    btn_startParsing.Enabled = false;
                }
            }
        }

        internal void RefreshTable()
        {
            lv_tables.BeginUpdate();

            lv_tables.Items.Clear();

            foreach (string str in Utils.AgentAnalysisUtility.Instance.ObtainTableNames())
            {
                long ticks;
                ticks = long.Parse(str.TrimStart('_' ));

                lv_tables.Items.Add(new ListViewItem(new string[] { ticks.ToDateTime().ToString("yyyy/MM/dd HH:mm:ss"), str }));
            }

            lv_tables.EndUpdate();
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Equals(tb_vg_logName) && e.KeyChar == (char)Keys.Space)
            {
                tb.AppendText("_");
                tb.ScrollToCaret();
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                if (btn_startService.Enabled)
                    btn_startService.PerformClick();
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            int excuteTime;

            // Try LogName and ExcutionTime whether or not have a valid value.
            btn_startService.Enabled = !string.IsNullOrEmpty(tb_vg_logName.Text) && int.TryParse(tb_vg_excuteTime.Text, out excuteTime);
        }
    }
}
