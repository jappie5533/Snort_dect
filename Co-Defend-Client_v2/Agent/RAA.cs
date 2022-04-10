using CommonTool;
using System.IO;
using System.Windows.Forms;
using System;

namespace Co_Defend_Client_v2.Agent
{
    public class RAA : SPA
    {
        public RAA() { }

        protected override void preSend()
        {
            base.preSend();

            if (!Data.IsHandle)
            {
                if (Data.Action == DPAction.Request)
                {
                    if (Data.Type == DPType.Run)
                    {
                        Data.IsHandle = true;
                    }
                }
            }
        }

        protected override void doRecv()
        {
            base.doRecv();

            if (!Data.IsHandle)
            {
            }
        }

        protected void onLogRsp()
        {
            LogContent lc = DataUtility.ToObject(Data.Content, typeof(LogContent)) as LogContent;
            string src_file_path = Path.Combine(Application.StartupPath, "tmp", lc.LogFileName);
            string zip_dir_path = Path.Combine(Application.StartupPath, "tmp", "zipdir");

            if (lc != null)
            {
                if (Data.Src == Agent_ID)
                {
                    string finalLogName = ServiceLogger.Instance.MoveLogFile(src_file_path, Data.Src.ToString());
                    recvBackLog(new BackLogEventArgs(Data.Src, finalLogName));
                }
                else
                {
                    ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
                    {
                        ZipPath = src_file_path,
                        IsZip = false,
                        FilePath = zip_dir_path
                    };

                    EventHandler<EventArgs> unzipCompletedHandler = null;
                    unzipCompletedHandler = (sender, e) =>
                    {
                        (sender as ZipUtility.ZipOptions).onUnZipCompleted -= unzipCompletedHandler;
                        src_file_path = Path.Combine(zip_dir_path, lc.LogFileName);
                        string finalLogName = ServiceLogger.Instance.MoveLogFile(src_file_path, Data.Src.ToString());
                        Directory.Delete(zip_dir_path);
                        recvBackLog(new BackLogEventArgs(Data.Src, finalLogName));
                    };

                    options.onUnZipCompleted += unzipCompletedHandler;

                    ZipUtility.Instance.UnZip(options);
                }
            }
        }
    }
}
