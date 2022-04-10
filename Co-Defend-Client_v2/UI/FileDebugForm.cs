using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonTool.FileStreamTool;
using CommonTool;

namespace Co_Defend_Client_v2.UI
{
    public partial class FileDebugForm : Form
    {
        private List<string> PcapFileNames;

        public FileDebugForm()
        {
            InitializeComponent();
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            //FileHandler.Instance.PostFile("140.126.130.75", tb_upload_local.Text, tb_upload_server.Text);
            FileInfomation info = new FileInfomation { File_Name = tb_upload_server.Text };
            info.onProgressUpdated += info_onProgressUpdated;
            info.onCompleted += info_onCompleted;

            Agent.AgentController.Instance.PostFile(3425671522617846345, tb_upload_local.Text, info);
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            //FileHandler.Instance.GetFile("140.126.130.75", tb_download_server.Text, tb_download_local.Text);
            FileInfomation info = new FileInfomation { File_Name = tb_download_server.Text };
            info.onProgressUpdated += info_onProgressUpdated;
            info.onCompleted += info_onCompleted;

            Agent.AgentController.Instance.GetFile(Convert.ToInt64(tb_download_local.Text), FileHandler.Instance.TmpFileFolder, info);
        }

        void info_onCompleted(object sender, FileStatusEventArgs e)
        {
            (sender as FileInfomation).onCompleted -= info_onCompleted;
            (sender as FileInfomation).onProgressUpdated -= info_onProgressUpdated;
        }

        void info_onProgressUpdated(object sender, FileProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { info_onProgressUpdated(sender, e); });
            }
            else
            {
                progressBar1.Value = e.ProgressValue;
                lb_status.Text = "Status: " + e.ProgressInfo;
            }
        }

        private void tb_upload_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog1.ShowDialog();
            tb_upload_local.Text = openFileDialog1.FileName;
        }

        private void FileDebugForm_Load(object sender, EventArgs e)
        {
            Rectangle screenBound = Screen.PrimaryScreen.Bounds;
            int x = screenBound.Width / 2 - Width / 2;
            int y = screenBound.Height / 2 - Height / 2;

            SetBounds(x, y, Width, Height);
        }

        private void btn_pcap_start_Click(object sender, EventArgs e)
        {
            Utils.AgentAnalysisUtility.Instance.PcapToSQLite("ABC", PcapFileNames);
        }

        private void tb_pcap_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.Multiselect = true;
            openFile.Filter = "Pcap Files (*.pcap)|*.pcap|All files (*.*)|*.*";
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_pcap.Text = openFile.FileName;
                PcapFileNames = openFile.FileNames.ToList();
            }
        }

        private void tb_fileName_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = Environment.CurrentDirectory;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_fileName.Text = openFile.FileName;
            }
        }

        private void tb_folder_MouseClick(object sender, MouseEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.InitialDirectory = Environment.CurrentDirectory;
            save.OverwritePrompt = true;

            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_folder.Text = save.FileName;
            }
        }

        private void btn_zip_Click(object sender, EventArgs e)
        {
            ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
            {
                FilePath = tb_fileName.Text,
                IsZip = true,
                ZipPath = tb_folder.Text
            };

            EventHandler<EventArgs> zipCompletedHandler = null;
            zipCompletedHandler = (sndr, eargs) =>
            {
                Console.WriteLine("Zip Completed !!!!");
                (sndr as ZipUtility.ZipOptions).onZipCompleted -= zipCompletedHandler;
            };

            options.onZipCompleted += new EventHandler<EventArgs>(zipCompletedHandler);

            ZipUtility.Instance.Zip(options);
        }

        private void btn_unzip_Click(object sender, EventArgs e)
        {
            ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
            {
                FilePath = tb_folder.Text,
                IsZip = false,
                ZipPath = tb_fileName.Text
            };

            EventHandler<EventArgs> unzipCompletedHandler = null;
            unzipCompletedHandler = (sndr, eargs) =>
            {
                Console.WriteLine("UnZip Completed !!!!");
                (sndr as ZipUtility.ZipOptions).onUnZipCompleted -= unzipCompletedHandler;
            };

            options.onUnZipCompleted += new EventHandler<EventArgs>(unzipCompletedHandler);

            ZipUtility.Instance.UnZip(options);
        }
    }
}
