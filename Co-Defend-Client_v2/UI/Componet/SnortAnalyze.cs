using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonTool;
using CommonTool.FileStreamTool;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Co_Defend_Client_v2.SnortService;
using System.Threading;
using Co_Defend_Client_v2.Agent;


namespace Co_Defend_Client_v2.UI
{
    public partial class SnortAnalyze : UserControl
    {
        public SnortAnalyze()
        {
            InitializeComponent();

            this.Load += new EventHandler(SnortAnalyze_Load);
        }

        void SnortAnalyze_Load(object sender, EventArgs e)
        {
            
            btn_send.Enabled = false;
            //btn_show.Enabled = false;
        }

        private void tb_upload_MouseClick(object sender, MouseEventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string filePath;

                filePath = Path.GetFullPath(openFileDialog1.FileName);

                tb_upload.Text = filePath;
                btn_send.Enabled = true;
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            string line = "";

            this.label1.Text = new Snort().Analyze(File.ReadAllBytes(this.tb_upload.Text), Path.GetFileName(this.tb_upload.Text));

            Thread.Sleep(3000);

            string sFile = AppDomain.CurrentDomain.BaseDirectory + @"SnortLog\alert.csv";

            MemoryStream streaminput = new MemoryStream();
            FileStream filestream = new FileStream(sFile, FileMode.Create, FileAccess.ReadWrite);

            Snort service = new Snort();
            int len = (int)service.DownloadFileLen(sFile.Remove(0, sFile.LastIndexOf("\\") + 1));
            Byte[] mybytearray = new Byte[len];
            mybytearray = service.AnalysisResult(sFile.Remove(0, sFile.LastIndexOf("\\") + 1));
            filestream.Write(mybytearray, 0, len);
            filestream.Close();

            tb_upload.Text = "";

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(sFile))
                {
                    // Read the stream to a string, and write the string to the console.
                    line = sr.ReadToEnd();
                }

                if (line == "")
                    label2.Text = "This PCAP file is safe";
                else
                {
                    label2.Text = "This PCAP file is unsafe";
                   // btn_show.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                label2.Text = "The file could not be read:" + ex.Message;
            }
         
        }

        private void btn_show_Click(object sender, EventArgs e)
        {
           

            SnortAnalsischart SnortAnalsischart1 = new SnortAnalsischart();
            SnortAnalsischart1.Show(this);
            
        }

    }
}
