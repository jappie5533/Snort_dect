using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool;
using System.ComponentModel;
using ServiceSDK;
using CommonTool.Base;
using System.IO;
using CommonTool.FileStreamTool;
using System.Net;

namespace Co_Defend_Client_v2.Agent
{
    public partial class SPA
	{
        protected void recvChatMsg(ChatMsgEventArgs chatArgs)
        {
            if (onChatMsgRecv != null)
                onChatMsgRecv.Invoke(this, chatArgs);
        }

        protected void recvPublish(PublishEventArgs args)
        {
            if (onPublishRecv != null)
                onPublishRecv.Invoke(this, args);
        }

        protected void recvBackLog(BackLogEventArgs args)
        {
            if (onBackLogRecv != null)
                onBackLogRecv.Invoke(this, args);
        }

        protected void recvRspDiscover(DiscoverEventArgs args)
        {
            if (onDiscoverRspRecv != null)
                onDiscoverRspRecv.Invoke(this, args);
        }

        private void registWorkerEvent()
        {
            _worker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                while (!_worker.CancellationPending)
                {
                    object[] args = _queue.Take() as object[];

                    if (args != null && args.Length == 2)
                    {
                        IService isvc = args[0] as IService;
                        RunContent rc = args[1] as RunContent;

                        if (isvc != null && rc != null)
                        {
                            IPAddress ip;
                            long id;

                            // Setting back log file path.
                            rc.BackLog = Path.Combine(FileHandler.Instance.TmpFileFolder, Path.GetFileNameWithoutExtension(rc.BackLog) + "_" + Agent_IP.ToString() + Path.GetExtension(rc.BackLog));

                            // Executing service.
                            isvc.Execute(rc);

                            // Uploading log file.
                            if (File.Exists(rc.BackLog))
                            {
                                // Transferring back log with public ip.
                                if (IPAddress.TryParse(rc.SenderTarget, out ip))
                                {
                                    // Sender target is self, and just send log response data.
                                    if (ip.ToString() == Agent_IP.ToString())
                                    {
                                        LogContent lc = new LogContent(Path.GetFileName(rc.BackLog), "");

                                        DataProtocol dp = new DataProtocol()
                                        {
                                            Action = DPAction.Response,
                                            Type = DPType.Log,
                                            Des = Agent_ID,
                                            Callback = "onLogRsp",
                                            Content = lc.ToJson()
                                        };

                                        AgentController.Instance.Go(ActionType.Send, dp);
                                    }
                                    // Sender target is another one, and prepare to transferring back log file.
                                    else
                                    {
                                        // Setting zip options.
                                        ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
                                        {
                                            FilePath = rc.BackLog,
                                            ZipPath = rc.BackLog,
                                            IsZip = true
                                        };

                                        // Setting zip completed handler.
                                        EventHandler<EventArgs> zipCompletedHandler = null;
                                        zipCompletedHandler = (sndr, earg) =>
                                        {
                                            // Starting send zipped file to sender target.
                                            FileHandler.Instance.PostFile(ip.ToString(), rc.BackLog, "tmp");

                                            // Added transffering done event handler.
                                            FileHandler.Instance.onStatusUpdate += new EventHandler<FileStatusEventArgs>(Instance_onPostFileStatusUpdate);

                                            // Unregist onZipCompleted event.
                                            (sndr as ZipUtility.ZipOptions).onZipCompleted -= zipCompletedHandler;
                                        };

                                        // Adding zip completed handler.
                                        options.onZipCompleted += new EventHandler<EventArgs>(zipCompletedHandler);

                                        // Starting zip.
                                        ZipUtility.Instance.Zip(options);
                                    }
                                }
                                // Transferring back log with agent id.
                                else if (long.TryParse(rc.SenderTarget, out id))
                                {
                                    // Sender target is self, and just send log response data.
                                    if (id == Agent_ID)
                                    {
                                        LogContent lc = new LogContent(Path.GetFileName(rc.BackLog), "");

                                        DataProtocol dp = new DataProtocol()
                                        {
                                            Action = DPAction.Response,
                                            Type = DPType.Log,
                                            Des = Agent_ID,
                                            Callback = "onLogRsp",
                                            Content = lc.ToJson()
                                        };

                                        AgentController.Instance.Go(ActionType.Send, dp);
                                    }
                                    // Sender target is another one, and prepare to transferring back log file.
                                    else
                                    {
                                        // Setting zip options.
                                        ZipUtility.ZipOptions options = new ZipUtility.ZipOptions
                                        {
                                            FilePath = rc.BackLog,
                                            ZipPath = rc.BackLog,
                                            IsZip = true
                                        };

                                        // Setting zip completed handler.
                                        EventHandler<EventArgs> zipCompletedHandler = null;
                                        zipCompletedHandler = (sndr, earg) =>
                                        {
                                            // Setting upload infomation.
                                            FileInfomation info = new FileInfomation { File_Name = Path.GetFileName(rc.BackLog) };
                                            info.onCompleted += new EventHandler<FileStatusEventArgs>(info_onCompleted);

                                            // Starting upload.
                                            PostFile(id, rc.BackLog, info);

                                            // Unregist onZipCompleted event.
                                            (sndr as ZipUtility.ZipOptions).onZipCompleted -= zipCompletedHandler;
                                        };

                                        // Adding zip completed handler.
                                        options.onZipCompleted += new EventHandler<EventArgs>(zipCompletedHandler);

                                        // Starting zip.
                                        ZipUtility.Instance.Zip(options);
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private void Instance_onPostFileStatusUpdate(object sender, FileStatusEventArgs e)
        {
            if (e.Code == StatusCode.File_Transfer_Done)
            {
                LogContent lc = new LogContent(e.File_Name, "");

                DataProtocol dp = new DataProtocol()
                {
                    Action = DPAction.Response,
                    Type = DPType.Log,
                    Des = e.Agent_ID,
                    Callback = "onLogRsp",
                    Content = lc.ToJson()
                };

                AgentController.Instance.Go(ActionType.Send, dp);

                File.Delete(Path.Combine(FileHandler.Instance.TmpFileFolder, e.File_Name));

                FileHandler.Instance.onStatusUpdate -= Instance_onPostFileStatusUpdate;
            }
        }

        private void info_onCompleted(object sender, FileStatusEventArgs e)
        {
            LogContent lc = new LogContent(e.File_Name, "");

            DataProtocol dp = new DataProtocol()
            {
                Action = DPAction.Response,
                Type = DPType.Log,
                Des = e.Agent_ID,
                Callback = "onLogRsp",
                Content = lc.ToJson()
            };

            AgentController.Instance.Go(ActionType.Send, dp);

            File.Delete(Path.Combine(FileHandler.Instance.TmpFileFolder, e.File_Name));

            (sender as FileInfomation).onCompleted -= info_onCompleted;
        }
	}
}
