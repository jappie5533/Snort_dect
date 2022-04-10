using System;
using System.ComponentModel;
using CommonTool;
using CommonTool.Base;
using ServiceSDK;
using System.Collections.Concurrent;
using Agent;
using System.Net;
using System.IO;
using CommonTool.FileStreamTool;

namespace Co_Defend_Client_v2.Agent
{
    public partial class SPA : BaseAgent, IAgentEvent
    {
        public event EventHandler<ChatMsgEventArgs> onChatMsgRecv;
        public event EventHandler<PublishEventArgs> onPublishRecv;
        public event EventHandler<BackLogEventArgs> onBackLogRecv;
        public event EventHandler<DiscoverEventArgs> onDiscoverRspRecv;

        private BackgroundWorker _worker;
        private BlockingCollection<object[]> _queue;
        private DiscoverNetworkPC _discoverPC;

        public SPA() 
        {
            _queue = new BlockingCollection<object[]>();
            _worker = new BackgroundWorker();
            _discoverPC = new DiscoverNetworkPC();
            _worker.WorkerSupportsCancellation = true;

            registWorkerEvent();
        }

        public override void Connect(string ip, int port)
        {
            base.Connect(ip, port);

            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();
        }

        public override void Disconnect()
        {
            base.Disconnect();

            if (_worker.IsBusy)
                _worker.CancelAsync();
        }

        protected override void preSend()
        {
            base.preSend();

            if (!Data.IsHandle)
            {
                if (Data.Action == DPAction.Request)
                {
                    switch (Data.Type)
                    {
                        case DPType.Msg:
                            //Data.Callback = "onMsgRsp";
                            Data.IsHandle = true;
                            break;
                        case DPType.Discover_Peer:
                            Data.Callback = "onDiscoverRsp";
                            Data.IsHandle = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected override void doRecv()
        {
            base.doRecv();

            if (!Data.IsHandle)
            {
                if (Data.Action == DPAction.Request)
                {
                    switch (Data.Type)
                    {
                        case DPType.Msg:
                            onMsgReq();
                            Data.IsHandle = true;
                            //Data.Content = AgentController.Instance.UserName;
                            break;
                        case DPType.Run:
                            onRunReq();
                            Data.IsHandle = true;
                            break;
                        case DPType.Publish_Service:
                            onPublishReq();
                            Data.IsHandle = true;
                            break;
                        case DPType.Discover_Peer:
                            onDiscoverReq();
                            Data.IsHandle = true;
                            break;
                        case DPType.Discover_PC:
                            onDiscoverPCReq(Data.Content);
                            Data.IsHandle = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // Agent discover the lan of pcs.
        protected void onDiscoverPCReq(string tid)
        {
            _discoverPC.Initialize(Agent_IP.ToString(), Agent_Mask.ToString(), tid);
            _discoverPC.onDetectionComplete += new EventHandler<EventArgs>(onDiscoverPCCompleted);
            _discoverPC.StartDetection();
        }

        void onDiscoverPCCompleted(object sender, EventArgs e)
        {
            DataProtocol dp = new DataProtocol()
            {
                Action = DPAction.Response,
                Content = DataUtility.ToJson(_discoverPC.DetectionResult),
                Type = DPType.Discover_PC,
                Callback = "DiscoverPCResponse",
                Des = Hub_ID
            };
            AgentController.Instance.Go(ActionType.Send, dp);

            _discoverPC.onDetectionComplete -= onDiscoverPCCompleted;
        }

        // User define Request function to handle the mseesage request.
        protected void onMsgReq()
        {
            // TODO: Handle Exception.
            MessageContent mc = DataUtility.ToObject(Data.Content, typeof(MessageContent)) as MessageContent;

            if (mc != null)
            {
                ChatMsgEventArgs args = new ChatMsgEventArgs();
                args.Message = mc.Content;
                args.RecvTimeStamp = DateTime.UtcNow.Ticks;
                args.Sender = mc.Sender;

                recvChatMsg(args);
            }
        }


        // User define Response function to handle the message response.
        protected void onMsgRsp()
        {
            recvLogMsg("Recv response from: " + Data.Content);
        }

        protected void onRunReq()
        {
            RunContent rc = DataUtility.ToObject(Data.Content, typeof(RunContent)) as RunContent;
            
            if (rc != null)
            {
                recvLogMsg(rc.ToJson());
                IService isvc;
                
                isvc = ServiceManager.Instance.TryCreateInstance(rc.Service, GetType().Name) as IService;
                if (isvc != null)
                {
                    //_queue.Add(new object[] { isvc, rc });
                    System.Threading.ThreadPool.QueueUserWorkItem(runService, new object[] { isvc, rc });
                }
            }
        }

        protected void onPublishReq()
        {
            PublishContent pc = DataUtility.ToObject(Data.Content, typeof(PublishContent)) as PublishContent;

            if (pc != null)
            {
                recvLogMsg("Recv Publish Req: " + pc.Target_IP_or_ID + ", " + pc.FileNames);
                recvPublish(new PublishEventArgs(pc));
            }
        }

        protected void onDiscoverReq()
        {
            DiscoverContent dc = new DiscoverContent(BaseAgentController.BaseInstance.UserName, this.Agent_IP.ToString(), this.Agent_ID);

            Data.Content = dc.ToJson();

            recvLogMsg("Recv Discover Req:");
        }

        protected void onDiscoverRsp()
        {
            DiscoverContent dc = DataUtility.ToObject(Data.Content, typeof(DiscoverContent)) as DiscoverContent;

            if (dc != null)
            {
                recvLogMsg("Recv Discover Rsp: " + dc.ToJson());
                recvRspDiscover(new DiscoverEventArgs(dc.Account, dc.IP, dc.ID));
            }
        }

        private void runService(object obj)
        {
            object[] args = obj as object[];

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
    }
}
