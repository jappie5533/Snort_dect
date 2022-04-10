using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace CommonTool.FileStreamTool
{
    internal class FileDaemon
    {
        internal Dictionary<string, FileInfo> _IP_FileInfo_Pair;
        internal event EventHandler<FileProgressEventArgs> onDownloadProgressUpdate;
        internal event EventHandler<FileStatusEventArgs> onStatusUpdate;
        internal event EventHandler<FileDownloadEventArgs> onDownloadRequest;

        private static FileDaemon _instance;
        private NetPeer _peer;
        private Thread _heartbeatThread;
        private bool _isCanceled;
        private bool _isRunning;

        public static FileDaemon Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FileDaemon();

                return _instance;
            }
        }

        private FileDaemon()
        {
            NetPeerConfiguration config = new NetPeerConfiguration(CD2Constant.FileIdentifier);
            config.Port = CD2Constant.FileServerPort;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.MaximumTransmissionUnit = CD2Constant.MaxDataLength;

            _peer = new NetPeer(config);
            _peer.RegisterReceivedCallback(Recv);

            _IP_FileInfo_Pair = new Dictionary<string, FileInfo>();

            _heartbeatThread = new Thread(Heartbeat);
            _heartbeatThread.Name = "FileHeartbeat";
            _isCanceled = false;
            _isRunning = false;
        }

        internal void Start()
        {
            if (!_isRunning)
            {
                _peer.Start();
                _heartbeatThread.Start();
                //StatusUpdate(new FileStatusEventArgs("File Daemon Started.", "", StatusCode.File_Daemon_Start));
                _isRunning = true;
            }
        }

        internal void Stop()
        {
            if (_isRunning)
            {
                _peer.Shutdown("bye");
                _isCanceled = true;
                _heartbeatThread.Abort();
                //StatusUpdate(new FileStatusEventArgs("File Daemon Stopped.", "", StatusCode.File_Daemon_Stop));
                _isRunning = false;
            }
        }

        private void ExecuteFileTrans(FileInfo info)
        {
            NetConnection nc = _peer.GetConnection((info._ip + ":" + CD2Constant.FileServerPort).ToIPEndPoint());

            if (nc != null)
            {
                // Download file request.
                if (info.isDownload)
                {
                    NetOutgoingMessage outmsg = _peer.CreateMessage();
                    outmsg.Write((byte)FileType.Download);
                    outmsg.Write(info._server_file);
                    outmsg.Write(FileHandler.Instance.Agent_ID);
                    nc.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered, 0);
                }
                // Upload file > setting streaming.
                else
                {
                    nc.Tag = new Streaming(nc, info._local_file, info._server_file);
                }
            }
            else
                _peer.Connect(info._ip, CD2Constant.FileServerPort);
        }

        private void Recv(object obj)
        {
            NetIncomingMessage msg;

            while ((msg = _peer.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Console.Write(msg.ReadString());
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        // TODO: Connection Approval.
                        msg.SenderConnection.Approve();
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                FileInfo info;
                                if (_IP_FileInfo_Pair.TryGetValue(msg.SenderEndPoint.Address.ToString(), out info))
                                {
                                    // Download file request.
                                    if (info.isDownload)
                                    {
                                        NetOutgoingMessage outmsg = _peer.CreateMessage();
                                        outmsg.Write((byte)FileType.Download);
                                        outmsg.Write(info._server_file);
                                        outmsg.Write(FileHandler.Instance.Agent_ID);
                                        msg.SenderConnection.SendMessage(outmsg, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                    // Upload file > setting streaming.
                                    else
                                    {
                                        msg.SenderConnection.Tag = new Streaming(msg.SenderConnection, info._local_file, info._server_file);
                                    }
                                }
                                break;
                            case NetConnectionStatus.Disconnected:
                                string content = msg.ReadString();
                                DisconnectMsg dmsg = DataUtility.ToObject(content, typeof(DisconnectMsg)) as DisconnectMsg;

                                if (dmsg != null)
                                {
                                    string file_name = "";

                                    if (_IP_FileInfo_Pair.ContainsKey(msg.SenderEndPoint.Address.ToString()))
                                    {
                                        FileInfo t_info = _IP_FileInfo_Pair[msg.SenderEndPoint.Address.ToString()];
                                        
                                        if (t_info.isDownload)
                                            file_name = t_info._server_file;
                                        else
                                            file_name = Path.GetFileName(t_info._local_file);

                                        _IP_FileInfo_Pair.Remove(msg.SenderEndPoint.Address.ToString());
                                    }
                                    else
                                    {
                                        file_name = (msg.SenderConnection.Tag as Streaming).file_name;
                                    }

                                    StatusUpdate(new FileStatusEventArgs(dmsg.Reason, file_name, dmsg.Agent_ID, StatusCode.File_Transfer_Done));
                                }
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        FileType type = (FileType)msg.ReadByte();

                        switch (type)
                        {
                            case FileType.Upload:
                                FileInfo info;

                                if (!_IP_FileInfo_Pair.TryGetValue(msg.SenderEndPoint.Address.ToString(), out info))
                                {
                                    info = new FileInfo(msg.SenderEndPoint.Address.ToString(), "", "", false);
                                    _IP_FileInfo_Pair.Add(msg.SenderEndPoint.Address.ToString(), info);
                                }

                                WriteStream(msg, ref info);

                                break;

                            case FileType.Download:
                                string req_path = msg.ReadString();
                                long agent_id = msg.ReadInt64();

                                string full_path = FileHandler.Instance.GetFullPath(req_path);

                                if (!string.IsNullOrEmpty(full_path) && File.Exists(full_path))
                                {
                                    msg.SenderConnection.Tag = new Streaming(msg.SenderConnection, full_path, "");

                                    if (onDownloadRequest != null)
                                        onDownloadRequest.Invoke(this, new FileDownloadEventArgs(req_path, agent_id));
                                }
                                else
                                    msg.SenderConnection.Disconnect(new DisconnectMsg(FileHandler.Instance.Agent_ID, "Failure: Illegal Path.").ToJson());

                                break;
                        }
                        break;
                }
                _peer.Recycle(msg);
            }
        }

        /// <summary>
        /// To check file streaming to sending.
        /// </summary>
        /// <param name="obj"></param>
        private void Heartbeat(object obj)
        {
            while (!_isCanceled)
            {
                if (_IP_FileInfo_Pair.Count == 0)
                {
                    FileInfo fi;
                    if (FileHandler.Instance._fileTransQueue.TryTake(out fi))
                    {
                        _IP_FileInfo_Pair.Add(fi._ip, fi);
                        ExecuteFileTrans(fi);
                    }
                }
                foreach (NetConnection conn in _peer.Connections)
                {
                    Streaming provider = conn.Tag as Streaming;
                    if (provider != null)
                        provider.Heartbeat();
                }
                foreach (StreamingProvider s in FileHandler.Instance.stremingList)
                    s.Heartbeat();

                Thread.Sleep(300);
            }
        }

        private void WriteStream(NetIncomingMessage msg, ref FileInfo info)
        {
            if (info.s_length == 0)
            {
                info.s_length = msg.ReadUInt64();
                info._server_file= msg.ReadString();
                //StatusUpdate(new FileStatusEventArgs("File Transfer Starting...", info._local_file, StatusCode.File_Transfer_Starting));
                try
                {
                    if (info.isDownload)
                    {
                        string fullPath = FileHandler.Instance.GetFullPath(Path.Combine(info._local_file, info._server_file));
                        // TODO: File Accessed by another proecss will crash.
                        if (!string.IsNullOrEmpty(fullPath))
                        {
                            if (!Directory.Exists(Directory.GetParent(fullPath).FullName))
                                Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);
                            info.s_writeStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                        }
                        else
                            msg.SenderConnection.Disconnect(new DisconnectMsg(FileHandler.Instance.Agent_ID, "Failure: Illegal Path.").ToJson());
                        //info.s_writeStream = new FileStream(FileHandler.Instance.GetFullPath(""), FileMode.Create, FileAccess.Write, FileShare.None);
                    }
                    else
                    {
                        string fullPath = FileHandler.Instance.GetFullPath(info._server_file);

                        if (!string.IsNullOrEmpty(fullPath))
                        {
                            if (!Directory.Exists(Directory.GetParent(fullPath).FullName))
                                Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);
                            info.s_writeStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                        }
                        else
                            msg.SenderConnection.Disconnect(new DisconnectMsg(FileHandler.Instance.Agent_ID, "Failure: Illegal Path.").ToJson());
                        //info.s_writeStream = new FileStream(FileHandler.Instance.GetFullPath(""), FileMode.Create, FileAccess.Write, FileShare.None);
                    }
                }
                catch (Exception ex)
                {
                    msg.SenderConnection.Disconnect(new DisconnectMsg(FileHandler.Instance.Agent_ID, ex.Message).ToJson());
                }
                info.s_timeStarted = Environment.TickCount;
                return;
            }

            if (info.s_writeStream == null)
                return;

            byte[] all = msg.ReadBytes(msg.LengthBytes - 1);
            info.s_received += (ulong)all.Length;
            info.s_writeStream.Write(all, 0, all.Length);

            int v = (int)(((float)info.s_received / (float)info.s_length) * 100.0f);
            if (info._lastProgressValue != v)
            {
                info._lastProgressValue = v;
                int passed = Environment.TickCount - info.s_timeStarted;
                double psec = (double)passed / 1000.0;
                double bps = (double)info.s_received / psec;

                DownloadProgressUpdate(v, Path.GetFileName(info.s_writeStream.Name) + " on " + NetUtility.ToHumanReadable((long)bps) + " per second");
            }

            if (info.s_received >= info.s_length)
            {
                int passed = Environment.TickCount - info.s_timeStarted;
                double psec = (double)passed / 1000.0;
                double bps = (double)info.s_received / psec;

                DownloadProgressUpdate(100, "Done at " + NetUtility.ToHumanReadable((long)bps) + " per second");

                info.s_writeStream.Flush();
                info.s_writeStream.Close();
                info.s_writeStream.Dispose();

                msg.SenderConnection.Disconnect(new DisconnectMsg(FileHandler.Instance.Agent_ID, "Downloaded.").ToJson());
            }
        }

        internal void StatusUpdate(FileStatusEventArgs e)
        {
            if (onStatusUpdate != null)
                onStatusUpdate.Invoke(this, e);
        }

        internal void DownloadProgressUpdate(int value, string info)
        {
            if (onDownloadProgressUpdate != null)
                onDownloadProgressUpdate.Invoke(this, new FileProgressEventArgs(value, info));
        }

        internal enum FileType
        {
            Upload,
            Download,
        }

        internal class FileInfo
        {
            public string _ip;
            public string _server_file;
            public string _local_file;
            public ulong s_length;
            public ulong s_received;
            public FileStream s_writeStream;
            public int s_timeStarted;
            public bool isDownload;
            public int _lastProgressValue;

            public FileInfo(string ip, string server_file, string local_name, bool isDownload)
            {
                this._ip = ip;
                this._server_file = server_file;
                this._local_file = local_name;
                this.s_length = 0;
                this.s_received = 0;
                this.s_timeStarted = -1;
                this.isDownload = isDownload;
                this._lastProgressValue = -1;
            }
        }

        internal class DisconnectMsg
        {
            [JsonProperty(PropertyName = "id")]
            public long Agent_ID { get; set; }

            [JsonProperty(PropertyName = "rsn")]
            public string Reason { get; set; }

            public DisconnectMsg(long agent_id, string reason)
            {
                this.Agent_ID = agent_id;
                this.Reason = reason;
            }

            public string ToJson()
            {
                return DataUtility.ToJson(this);
            }
        }
    }
}
