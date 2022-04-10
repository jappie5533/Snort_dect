using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using CommonTool;
using System.IO;
using Lidgren.Network;
using CommonTool.FileStreamTool;
using System.Diagnostics;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class PublishService : UserControl
    {
        private System.Net.IPAddress _ip;
        private int _pubCount;
        private BackgroundWorker _worker;
        private string selectedSvc;

        public PublishService()
        {
            InitializeComponent();

            this.Load += new EventHandler(PublishService_Load);
        }

        void PublishService_Load(object sender, EventArgs e)
        {
            System.Net.IPAddress mask;
            _ip = NetUtility.GetMyAddress(out mask);
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;

            EventRegist();

            AppendItem();

            btn_publish.Enabled = false;
        }

        private void EventRegist()
        {
            FileHandler.Instance.onDownloadRequest += new EventHandler<FileDownloadEventArgs>(Instance_onDownloadRequest);
            AgentController.Instance.onDownloadFileRequest += new EventHandler<FileDownloadEventArgs>(Instance_onDownloadRequest);

            FileHandler.Instance.onStatusUpdate += delegate(object sender, FileStatusEventArgs e)
            {
                switch (e.Code)
                {
                    case StatusCode.File_Transfer_Done:
                        //if (e.File_Name == _lastPubSvc)
                        //{
                            if (lv_service.Items.ContainsKey(e.Agent_ID.ToString()))
                            {
                                int tmp = (int)lv_service.Items[e.Agent_ID.ToString()].Tag + 1;
                                lv_service.Items[e.Agent_ID.ToString()].Tag = tmp;
                                lv_service.Items[e.Agent_ID.ToString()].SubItems[2].Text = "Downloaded.(" + tmp + "/" + _pubCount + ")";
                            }
                        //}
                        break;
                }
            };

            lv_service.ColumnClick += delegate(object sender, ColumnClickEventArgs e)
            {
                ListViewSorter Sorter = new ListViewSorter();
                lv_service.ListViewItemSorter = Sorter;
                if (!(lv_service.ListViewItemSorter is ListViewSorter))
                    return;
                Sorter = (ListViewSorter)lv_service.ListViewItemSorter;

                if (Sorter.LastSort == e.Column)
                {
                    if (lv_service.Sorting == SortOrder.Ascending)
                        lv_service.Sorting = SortOrder.Descending;
                    else
                        lv_service.Sorting = SortOrder.Ascending;
                }
                else
                {
                    lv_service.Sorting = SortOrder.Descending;
                }
                Sorter.ByColumn = e.Column;

                lv_service.Sort();
            };

            lv_task.ItemSelectionChanged += delegate(object sender, ListViewItemSelectionChangedEventArgs e)
            {
                btn_remove.Enabled = lv_task.SelectedItems.Count > 0;
            };

            lv_task.DoubleClick += delegate(object sender, EventArgs e)
            {
                if (lv_task.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem lvi in lv_task.SelectedItems)
                    {
                        string file_path = lvi.Tag as string;

                        if (file_path != null)
                        {
                            Process.Start("notepad.exe", file_path);
                        }
                    }
                }
            };

            cb_services.SelectedValueChanged += new EventHandler(ctrl_ValueChanged);
            cb_services.Click += new EventHandler(ctrl_GotFocus);
            cb_services.KeyPress += new KeyPressEventHandler(ctrl_KeyPress);
            tb_logname.KeyPress += new KeyPressEventHandler(ctrl_KeyPress);
            tb_logname.TextChanged += new EventHandler(ctrl_ValueChanged);
            tb_logname.GotFocus += new EventHandler(ctrl_GotFocus);
            tb_params.TextChanged += new EventHandler(ctrl_ValueChanged);
            tb_params.GotFocus += new EventHandler(ctrl_GotFocus);
            tb_params.KeyPress += new KeyPressEventHandler(ctrl_KeyPress);
            ServiceManager.Instance.onAllowServiceChanged += delegate(object sender, EventArgs e)
            {
                AppendItem();
            };

            AgentController.Instance.onBackLogRecv += new EventHandler<BackLogEventArgs>(Instance_onBackLogRecv);

            _worker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                ListViewItem task_item;

                while (TryTakeTaskQueue(out task_item) && !e.Cancel)
                {
                    ListViewItem lvi;

                    if (FindIdleAgent(out lvi))
                    {
                        string input = Path.GetFileName(task_item.SubItems[0].Text);
                        string svc = selectedSvc;

                        StringBuilder sb = new StringBuilder();
                        string target = DataUtility.IsPrivateIP(_ip) ? AgentController.Instance.Agent_ID.ToString() : _ip.ToString();
                        sb.AppendFormat("run Cmd {0} {1} {2} {3}", input + ".log", target, svc, input);

                        DataProtocol dp = new DataProtocol();
                        DPType tmp;

                        dp.Action = DPAction.Request;
                        dp.Des = Convert.ToInt64(lvi.SubItems[0].Text);
                        dp.Content = CommandUtility.ToJson(sb.ToString(), out tmp);
                        dp.Type = tmp;

                        AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);

                        UpdateStatus(lvi.Name, task_item.SubItems[0].Text, "Running.");
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            };

            _worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Cancelled)
                {

                }
                else
                {

                }
            };
        }

        void Instance_onDownloadRequest(object sender, FileDownloadEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Instance_onDownloadRequest(sender, e);
                });
            }
            else
            {
                if (!lv_service.Items.ContainsKey(e.PeerID.ToString()))
                {
                    ListViewItem lvi = new ListViewItem(new string[] { e.PeerID.ToString(), e.RequestPath, "Downloading... (0/" + _pubCount + ")" });
                    lvi.Name = e.PeerID.ToString();
                    lvi.Tag = 0;

                    lv_service.BeginUpdate();
                    lv_service.Items.Add(lvi);
                    lv_service.EndUpdate();
                }
                else
                {
                    int tmp = (int)lv_service.Items[e.PeerID.ToString()].Tag;
                    lv_service.Items[e.PeerID.ToString()].SubItems[1].Text = e.RequestPath;
                    lv_service.Items[e.PeerID.ToString()].SubItems[2].Text = "Downloading...(" + tmp + "/" + _pubCount + ")";
                }
                //}

                if (e.FileInfo != null)
                    e.FileInfo.onCompleted += new EventHandler<FileStatusEventArgs>(FileInfo_onCompleted);
            }
        }

        void FileInfo_onCompleted(object sender, FileStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    FileInfo_onCompleted(sender, e);
                });
            }
            else
            {
                if (lv_service.Items.ContainsKey(e.Agent_ID.ToString()))
                {
                    int tmp = (int)lv_service.Items[e.Agent_ID.ToString()].Tag + 1;
                    lv_service.Items[e.Agent_ID.ToString()].Tag = tmp;
                    lv_service.Items[e.Agent_ID.ToString()].SubItems[2].Text = "Downloaded.(" + tmp + "/" + _pubCount + ")";
                }

                // Unregist Event.
                if (sender is FileInfomation)
                {
                    FileInfomation info = sender as FileInfomation;
                    info.onCompleted -= FileInfo_onCompleted;
                }
            }
        }

        void Instance_onBackLogRecv(object sender, BackLogEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Instance_onBackLogRecv(sender, e);
                });
            }
            else
            {
                if (lv_service.Items.ContainsKey(e.Agent_ID.ToString()))
                {
                    //if (e.LogName.Contains(lv_service.Items[e.Agent_ID.ToString()].SubItems[1].Text))
                    //{
                    lv_service.Items[e.Agent_ID.ToString()].SubItems[2].Text = "Complete.";

                    string file_name = lv_service.Items[e.Agent_ID.ToString()].SubItems[1].Text;

                    if (lv_task.Items.ContainsKey(file_name))
                    {
                        lv_task.Items[file_name].SubItems[1].Text = "Done. Clicks to open " + e.LogName + ".";
                        lv_task.Items[file_name].Tag = Path.Combine(ServiceLogger.Instance.BackLogPath, e.LogName);
                    }
                    //}
                }
            }
        }


        #region Run Service Event Handler.

        private void ctrl_GotFocus(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            TextBox tb = sender as TextBox;

            if (cb != null)
            {
                if (cb.SelectedIndex < 0)
                    cb.SelectedIndex = 0;
                cb.DroppedDown = true;
            }
            else if (tb != null)
                tb.SelectAll();
        }

        private void ctrl_ValueChanged(object sender, EventArgs e)
        {
            UpdateCmd();
        }

        private void ctrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            ComboBox cb = sender as ComboBox;

            if (tb != null)
            {
                if (tb.Equals(tb_logname) && e.KeyChar == (char)Keys.Space)
                {
                    tb.AppendText("_");
                    tb.ScrollToCaret();
                    e.Handled = true;
                }
                else if (e.KeyChar == (char)Keys.Enter)
                {
                    if (btn_send.Enabled)
                        Send();
                }
            }
            else if (cb != null)
            {
                if (e.KeyChar != (char)Keys.Enter)
                    e.Handled = true;
            }
        }

        private void AppendItem()
        {
            cb_services.Items.Clear();
            cb_services.Items.AddRange(ServiceManager.Instance.GetAllowService().ToArray());
            //cb_services.SelectedIndex = 0;
        }

        private void UpdateCmd()
        {
            string log = tb_logname.Text.Replace(' ', '_');
            StringBuilder sb = new StringBuilder();
            string target = DataUtility.IsPrivateIP(_ip) ? AgentController.Instance.Agent_ID.ToString() : _ip.ToString();
            sb.AppendFormat("run {0} {1} {2} {3}", cb_services.SelectedItem.ToString(), log, target, tb_params.Text);
            tb_cmd.Text = sb.ToString();

            bool isCorrect;

            isCorrect = !string.IsNullOrEmpty(cb_services.SelectedItem.ToString());
            isCorrect &= !string.IsNullOrEmpty(tb_logname.Text);
            isCorrect &= !string.IsNullOrEmpty(tb_params.Text);

            btn_send.Enabled = isCorrect;
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void Send()
        {
            string cmd = tb_cmd.Text;
            tb_cmd.Clear();

            DataProtocol dp = new DataProtocol();
            DPType tmp;

            dp.Action = DPAction.Request;
            dp.Content = CommandUtility.ToJson(cmd, out tmp);
            dp.Type = tmp;

            AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);
            tb_logname.Text = string.Empty;
            tb_params.Text = string.Empty;
            cb_services.Focus();
        }

        #endregion

        #region Publish Service Event Handler.

        private void btn_publish_Click(object sender, EventArgs e)
        {
            lv_service.Items.Clear();

            string cmd = tb_publish.Text;

            DataProtocol dp = new DataProtocol();
            DPType tmp;

            dp.Action = DPAction.Request;
            dp.Content = CommandUtility.ToJson(cmd, out tmp);
            dp.Type = tmp;

            AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);

            PublishContent pc = DataUtility.ToObject(dp.Content, typeof(PublishContent)) as PublishContent;
            if (pc != null)
            {
                _pubCount = pc.FileNames.Count;
                foreach (string file in pc.FileNames)
                    selectedSvc = Path.GetFileName(file);
            }
        }

        private void tb_publish_MouseClick(object sender, MouseEventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("pub {0}", DataUtility.IsPrivateIP(_ip) ? AgentController.Instance.Agent_ID.ToString() : _ip.ToString());

                foreach (string filename in openFileDialog1.FileNames)
                {
                    if (File.Exists(filename))
                    {
                        string parent = "\\tmp";
                        string finalName;

                        finalName =  Path.GetExtension(filename).Contains("exe") ? Path.GetFileNameWithoutExtension(filename) : Path.GetFileName(filename);

                        ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
                        {
                            FilePath = filename,
                            ZipPath = Path.Combine(Application.StartupPath, "tmp", finalName),
                            IsZip = true
                        };
                        ZipUtility.Instance.Zip(options);

                        sb.Append(" " + Path.Combine(parent, finalName));
                    }
                }
                tb_publish.Text = sb.ToString();
                btn_publish.Enabled = true;
            }
        }

        private class ListViewSorter : System.Collections.IComparer
        {
            public int Compare(object o1, object o2)
            {
                if (!(o1 is ListViewItem))
                    return (0);
                if (!(o2 is ListViewItem))
                    return (0);

                ListViewItem lvi1 = (ListViewItem)o2;
                string str1 = lvi1.SubItems[ByColumn].Text;
                ListViewItem lvi2 = (ListViewItem)o1;
                string str2 = lvi2.SubItems[ByColumn].Text;

                int result;
                if (lvi1.ListView.Sorting == SortOrder.Ascending)
                    result = String.Compare(str1, str2);
                else
                    result = String.Compare(str2, str1);

                LastSort = ByColumn;

                return (result);
            }


            public int ByColumn
            {
                get { return Column; }
                set { Column = value; }
            }
            int Column = 0;

            public int LastSort
            {
                get { return LastColumn; }
                set { LastColumn = value; }
            }
            int LastColumn = 0;
        }

        #endregion

        private void btn_select_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog(this);

            foreach (string file in openFileDialog1.FileNames)
            {
                if (File.Exists(file))
                {
                    FileInfo info = new FileInfo(file);
                    string name = info.Name;
                    string parent;

                    if (info.Directory.FullName.Contains(Application.StartupPath))
                    {
                        parent = info.Directory.FullName.Replace(Application.StartupPath, "");
                        parent = string.IsNullOrEmpty(parent) ? "\\" : parent;
                    }
                    else
                    {
                        parent = "\\tmp";
                        if (!Directory.Exists(Path.Combine(Application.StartupPath, "tmp")))
                            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
                        info.CopyTo(Path.Combine(Application.StartupPath, "tmp", info.Name), true);
                    }

                    string req_path = Path.Combine(parent, name);

                    ListViewItem lvi = new ListViewItem(new string[] { req_path, "Queued." });
                    lvi.Name = req_path;

                    lv_task.BeginUpdate();
                    lv_task.Items.Add(lvi);
                    lv_task.EndUpdate();
                }
            }

            btn_run.Enabled = true;
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lv_task.SelectedItems)
                lv_task.Items.Remove(lvi);

            btn_remove.Enabled = false;
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            _worker.RunWorkerAsync();
            btn_run.Enabled = false;
        }

        private void UpdateStatus(string aid, string file_name, string msg)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    UpdateStatus(aid, file_name, msg);
                });
            }
            else
            {
                lv_service.Items[aid].SubItems[1].Text = file_name;
                lv_service.Items[aid].SubItems[2].Text = msg;

                if (lv_task.Items.ContainsKey(file_name))
                {
                    lv_task.Items[file_name].SubItems[1].Text = msg;
                }
            }
        }

        private bool FindIdleAgent(out ListViewItem item)
        {
            if (InvokeRequired)
            {
                ListViewItem lvi = null;
                bool result = false;

                Invoke((MethodInvoker)delegate
                {
                    result = FindIdleAgent(out lvi);
                });

                item = lvi;
                return result;
            }
            else
            {
                foreach (ListViewItem lvi in lv_service.Items)
                {
                    if (!lvi.SubItems[2].Text.Contains("Running"))
                    {
                        item = lvi;
                        return true;
                    }
                }

                item = null;
                return false;
            }
        }

        private bool TryTakeTaskQueue(out ListViewItem item)
        {
            if (InvokeRequired)
            {
                ListViewItem outItem = null;
                bool result = false;
                
                Invoke((MethodInvoker)delegate
                {
                    result = TryTakeTaskQueue(out outItem);
                });

                item = outItem;
                return result;
            }
            else
            {
                foreach (ListViewItem lvi in lv_task.Items)
                {
                    if (lvi.SubItems[1].Text.Contains("Queued"))
                    {
                        item = lvi;
                        return true;
                    }
                }

                item = null;
                return false;
            }
        }
    }
}
