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
    public partial class AnalysisChart : UserControl
    {
        internal string TableName { get; set; }
        internal string Date
        {
            set
            {
                groupBox2.Text = string.Format("Analysis Mode - (Analysis Date: {0})", value);
            }
        }
        private System.Timers.Timer _timer;
        private string _prevWhere;
        private string _preFrom;
        private string _preGroupBy;

        public AnalysisChart()
        {
            InitializeComponent();

            // Analysis Event Regist.
            rdoBtn_rawData.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_icmpAttack.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_synAttack.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_udpAttack.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_sip.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_smac.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_port_scan.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);
            rdobtn_flooding.CheckedChanged += new EventHandler(rdobtn_CheckedChanged);

            // Web browser event regist.
            webBrowser1.DocumentCompleted += (sender, e) =>
            {
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                {
                    _timer = new System.Timers.Timer();
                    _timer.Interval = 1000;
                    _timer.SynchronizingObject = this;
                    _timer.Elapsed += (s, e1) =>
                    {
                        rdoBtn_rawData.Checked = true;
                        _timer.Stop();
                        _timer.Dispose();
                    };
                    _timer.Start();
                }
            };

            webBrowser1.Navigate(CD2Constant.Agent_Statistic_Web);
        }

        private void rdobtn_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != null && (sender as RadioButton).Checked)
            {
                string sqlCmd;
                string fields = string.Format("Time,{0},Source_Port,Destination_IP,Destination_Port,Protocol,Length,TCP_Flag,ICMP_Code,ICMP_Type,count(*) AS Total_Count", rdobtn_sip.Checked ? rdobtn_sip.Tag : rdobtn_smac.Tag);
                string from = TableName;
                string where;
                string groupBy = string.Format("{0}", rdobtn_sip.Checked ? rdobtn_sip.Tag : rdobtn_smac.Tag);
                string jsonData;

                // SYN Attack SQL Command.
                if (sender == rdobtn_synAttack)
                {
                    where = "Protocol LIKE 'TCP' and TCP_Flag LIKE 'Syn%'";
                    groupBy += ",Protocol";
                }
                // UDP Attack SQL Command.
                else if (sender == rdobtn_udpAttack)
                {
                    where = "Protocol LIKE 'UDP'";
                    groupBy += ",Protocol";
                }
                // ICMP Attack SQL Command.
                else if (sender == rdobtn_icmpAttack)
                {
                    where = "Protocol LIKE 'InternetControlMessageProtocol'";
                    groupBy += ",Protocol";
                }
                // Raw Data SQL Command.
                else if (sender == rdoBtn_rawData)
                {
                    where = "1 = 1";
                }
                else if (sender == rdobtn_port_scan)
                {
                    where = "1 = 1";
                    from = string.Format("(SELECT * FROM {0} GROUP BY Source_IP,Source_MAC,Destination_IP,Destination_Port)", TableName);
                    groupBy = "Source_IP,Source_MAC,Destination_IP";
                }
                else if (sender == rdobtn_flooding)
                {
                    where = "1 = 1";
                    from = string.Format("(SELECT * FROM {0} GROUP BY Time,Source_IP,Source_MAC,Destination_IP HAVING count(*) >= 5)", TableName);
                    groupBy = "Source_IP,Source_MAC,Destination_IP";
                }
                else
                {
                    from = _preFrom;
                    where = _prevWhere;
                    groupBy = _preGroupBy;
                }

                _preFrom = from;
                _prevWhere = where;
                _preGroupBy = groupBy;

                // Composing SQL Command.
                sqlCmd = string.Format("SELECT {0} FROM {1} WHERE {2} GROUP BY {3} ORDER BY {4} DESC;", fields, from, where, groupBy, "Total_Count");
                jsonData = Utils.AgentAnalysisUtility.Instance.ObtainSQLiteDataWithSQLCommand(sqlCmd);

                if (string.IsNullOrEmpty(jsonData))
                {
                    MessageBox.Show(this, "No analytical results.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rdoBtn_rawData.Checked = true;
                }
                else
                {
                    // Invoke JavaScript Function - drawChart(JSON_DATA, TYPE);
                    //     JSON_DATA: The data to draw.
                    //     TYPE: The chart type = [PieChart, ColumnChart, ComboChart, BarChart].
                    //           "" - reserve previous chart type.
                    webBrowser1.Document.InvokeScript("drawChart", new object[] { jsonData, "" });
                }
            }
        }
    }
}
