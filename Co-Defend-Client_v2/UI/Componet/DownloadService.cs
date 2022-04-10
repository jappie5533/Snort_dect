using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using CommonTool;
using CommonTool.FileStreamTool;
using System.Net;
using Lidgren.Network;
using System.IO;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2.UI
{
    internal partial class DownloadService : UserControl
    {
        private Queue<PublishContent> _queue;
        private PublishContent _nowContent;
        private int _runIndex;
        private int _allTaskCount;
        private int _taskPercent;
        private bool _isRunning;
        private BackgroundWorker _worker;
        private int _pubCount;
        private IPAddress _ip;

        public DownloadService()
        {
            InitializeComponent();

            System.Net.IPAddress mask;
            _ip = NetUtility.GetMyAddress(out mask);
            _queue = new Queue<PublishContent>();
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            this.Load += new EventHandler(DownloadService_Load);
        }

        public void Enqueue(PublishContent pc)
        {
            _queue.Enqueue(pc);
            if (!_isRunning)
                DequeueAndRun();
        }

        private void DequeueAndRun()
        {
            if (_queue.Count > 0)
            {
                _nowContent = _queue.Dequeue();
                Initailize();
                StartRun();
                _isRunning = true;
            }
        }

        private void Initailize()
        {
            //lv_service.Items.Clear();
            AppendList();
            //_runIndex = 0;
            pb_download.Value = 0;
            pb_all_task.Value = 0;
            _allTaskCount = _nowContent.FileNames.Count + 1;
            _taskPercent = 100 / _allTaskCount;
            lb_all_info.Text = "All Status: Downloading...";
        }

        private void DownloadService_Load(object sender, EventArgs e)
        {
            EventRegist();
            _isRunning = false;
        }

        private void AppendList()
        {
            foreach (string file in _nowContent.FileNames)
            {
                ListViewItem lvi = new ListViewItem(new string[] { file, "Queued..." });
                lvi.Name = file;
                lv_service.Items.Add(lvi);
            }
        }

        private void EventRegist()
        {
            _worker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                if (_worker.CancellationPending)
                    e.Cancel = true;
                ServiceManager.Instance.Reload();
            };

            _worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Cancelled)
                {
                    lb_all_info.Text = "All Status: Cancelled.";
                }
                else
                {
                    pb_all_task.Value = 100;
                    lb_all_info.Text = "All Status: Complete.";
                    _isRunning = false;
                    DequeueAndRun();
                }
            };

            FileHandler.Instance.onProgressUpdate += delegate(object sender, FileProgressEventArgs e)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        pb_download.Value = e.ProgressValue;
                        lb_info.Text = "Downloading... " + e.ProgressInfo;
                    });
                }
                else
                {
                    pb_download.Value = e.ProgressValue;
                    lb_info.Text = "Downloading... " + e.ProgressInfo;
                }
            };

            FileHandler.Instance.onStatusUpdate += delegate(object sender, FileStatusEventArgs e)
            {
                switch (e.Code)
                {
                    case StatusCode.File_Transfer_Done:
                        if (_runIndex < lv_service.Items.Count && lv_service.Items[_runIndex].SubItems[0].Text.Contains(e.File_Name))
                        {
                            pb_all_task.Value += _taskPercent;
                            lv_service.Items[_runIndex++].SubItems[1].Text = e.Reason;
                            lb_info.Text = e.Reason;

                            ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
                            {
                                ZipPath = Path.Combine(ServiceLogger.Instance.TmpPath, Path.GetFileName(e.File_Name)),
                                IsZip = false
                            };

                            if (e.File_Name.Contains(CD2Constant.Core_Service_DLL.Split('.')[0]))
                                options.FilePath = ServiceManager.Instance.ServiceFolder;
                            else
                                options.FilePath = ServiceManager.Instance.UserDefineFolder;

                            ZipUtility.Instance.UnZip(options);

                            StartRun();
                        }

                        if (listView1.Items.ContainsKey(e.Agent_ID.ToString()))
                        {
                            int tmp = (int)listView1.Items[e.Agent_ID.ToString()].Tag + 1;
                            listView1.Items[e.Agent_ID.ToString()].Tag = tmp;
                            listView1.Items[e.Agent_ID.ToString()].SubItems[2].Text = "Downloaded.(" + tmp + "/" + _pubCount + ")";
                        }
                        break;
                }
            };

            FileHandler.Instance.onDownloadRequest += delegate(object sender, FileDownloadEventArgs e)
            {
                //if (e.RequestPath == _lastPubSvc)
                //{
                if (!listView1.Items.ContainsKey(e.PeerID.ToString()))
                {
                    ListViewItem lvi = new ListViewItem(new string[] { e.PeerID.ToString(), e.RequestPath, "Downloading... (0/" + _pubCount + ")" });
                    lvi.Name = e.PeerID.ToString();
                    lvi.Tag = 0;

                    listView1.BeginUpdate();
                    listView1.Items.Add(lvi);
                    listView1.EndUpdate();
                }
                else
                {
                    int tmp = (int)listView1.Items[e.PeerID.ToString()].Tag;
                    listView1.Items[e.PeerID.ToString()].SubItems[1].Text = e.RequestPath;
                    listView1.Items[e.PeerID.ToString()].SubItems[2].Text = "Downloading...(" + tmp + "/" + _pubCount + ")";
                }
                //}
            };

            AgentController.Instance.onDiscoverRspRecv += new EventHandler<DiscoverEventArgs>(Instance_onDiscoverRspRecv);
        }

        private void Instance_onDiscoverRspRecv(object sender, DiscoverEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Instance_onDiscoverRspRecv(sender, e);
                });
            }
            else
            {
                lv_discover.BeginUpdate();
                lv_discover.Items.Add(new ListViewItem(new string[] { e.Account, e.ID.ToString(), e.IP }));
                lv_discover.EndUpdate();

                lv_pub_discover.BeginUpdate();
                lv_pub_discover.Items.Add(new ListViewItem(new string[] { e.Account, e.ID.ToString(), e.IP }));
                lv_pub_discover.EndUpdate();
            }
        }

        private void File_Progress(object sender, FileProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    File_Progress(sender, e);
                });
            }
            else
            {
                pb_download.Value = e.ProgressValue;
                lb_info.Text = "Downloading... " + e.ProgressInfo;
            }
        }

        private void File_OnCompleted(object sender, FileStatusEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    File_OnCompleted(sender, e);
                });
            }
            else
            {
                if (_runIndex < lv_service.Items.Count && lv_service.Items[_runIndex].SubItems[0].Text.Contains(e.File_Name))
                {
                    pb_all_task.Value += _taskPercent;
                    lv_service.Items[_runIndex++].SubItems[1].Text = e.Reason;
                    lb_info.Text = e.Reason;

                    ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
                    {
                        ZipPath = Path.Combine(ServiceLogger.Instance.TmpPath, Path.GetFileName(e.File_Name)),
                        IsZip = false
                    };

                    if (e.File_Name.Contains(CD2Constant.Core_Service_DLL.Split('.')[0]))
                        options.FilePath = ServiceManager.Instance.ServiceFolder;
                    else
                        options.FilePath = ServiceManager.Instance.UserDefineFolder;

                    ZipUtility.Instance.UnZip(options);

                    StartRun();
                }

                if (sender is FileInfomation)
                {
                    FileInfomation info = sender as FileInfomation;
                    info.onProgressUpdated -= File_Progress;
                    info.onCompleted -= File_OnCompleted;
                }
            }
        }

        private void StartRun()
        {
            if (_runIndex < lv_service.Items.Count)
            {
                string file = lv_service.Items[_runIndex].SubItems[0].Text;
                lv_service.Items[_runIndex].SubItems[1].Text = "Processing...";
                lv_service.SelectedItems.Clear();
                lv_service.Items[_runIndex].Selected = true;
                pb_download.Value = 0;
                IPAddress ip;
                long id;

                // Download file from actually ip address.
                if (IPAddress.TryParse(_nowContent.Target_IP_or_ID, out ip))
                {
                    FileHandler.Instance.GetFile(ip.ToString(), file, ServiceLogger.Instance.TmpPath);
                    //if (file.Contains(CD2Constant.Core_Service_DLL.Split(new char[] { '.' })[0]))
                    //    FileHandler.Instance.GetFile(ip.ToString(), file, ServiceManager.Instance.ServiceFolder);
                    //else
                    //    FileHandler.Instance.GetFile(ip.ToString(), file, ServiceManager.Instance.UserDefineFolder);
                }
                // Download file from co-defend plateform id.
                else if (long.TryParse(_nowContent.Target_IP_or_ID, out id))
                {
                    FileInfomation info = new FileInfomation { File_Name = file };
                    string local_path = ServiceLogger.Instance.TmpPath;

                    //if (file.Contains(CD2Constant.Core_Service_DLL.Split(new char[] { '.' })[0]))
                    //    local_path = ServiceManager.Instance.ServiceFolder;
                    //else
                    //    local_path = ServiceManager.Instance.UserDefineFolder;

                    info.onProgressUpdated += File_Progress;
                    info.onCompleted += File_OnCompleted;

                    AgentController.Instance.GetFile(id, local_path, info);
                }
            }
            else
            {
                lb_all_info.Text = "All Status: Loading service...";
                _worker.RunWorkerAsync();
            }
        }

        private void btn_discover_Click(object sender, EventArgs e)
        {
            DataProtocol dp = new DataProtocol();
            short ttl;

            dp.Action = DPAction.Request;
            dp.Type = DPType.Discover_Peer;
            if (short.TryParse(tb_ttl.Text, out ttl))
                dp.TTL = ttl;

            AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);

            lv_discover.Items.Clear();
            lv_pub_discover.Items.Clear();
        }

        private void btn_publish_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            string cmd = tb_publish.Text;

            DataProtocol dp = new DataProtocol();
            DPType tmp;

            dp.Action = DPAction.Request;
            dp.Content = CommandUtility.ToJson(cmd, out tmp);
            dp.Type = tmp;

            AgentController.Instance.Go(CommonTool.Base.ActionType.Send, dp);

            PublishContent pc = DataUtility.ToObject(dp.Content, typeof(PublishContent)) as PublishContent;
            if (pc != null)
                _pubCount = pc.FileNames.Count;
        }

        private void tb_publish_MouseClick(object sender, MouseEventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog(this);

            StringBuilder sb = new StringBuilder();

            if (DataUtility.IsPrivateIP(_ip))
                sb.AppendFormat("pub {0}", AgentController.Instance.Agent_ID);
            else
                sb.AppendFormat("pub {0}", _ip);

            foreach (string filename in openFileDialog1.FileNames)
            {
                if (File.Exists(filename))
                {
                    FileInfo info = new FileInfo(filename);
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

                    sb.Append(" " + Path.Combine(parent, name));
                }
            }
            tb_publish.Text = sb.ToString();
            btn_publish.Enabled = true;
        }
    }
}
