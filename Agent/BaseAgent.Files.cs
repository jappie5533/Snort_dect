using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTool;
using Lidgren.Network;
using System.IO;
using CommonTool.FileStreamTool;

namespace Agent
{
    public abstract partial class BaseAgent
    {
        // Sending files.
        protected override void handleFileSend(FileProtocol file_data)
        {
            NetOutgoingMessage msg = _client.CreateMessage(ref file_data);

            msg.Encrypt(new NetRSAEncryption(BaseAgentController.BaseInstance.I_Cert.Cert.PublicKey));
            _client.SendMessage(msg, _client.Connections, NetDeliveryMethod.ReliableOrdered, 1);
        }

        // Recieving files.
        protected override void handleFileRecv(FileProtocol file_data)
        {
            FileInfomation fi;
            switch (file_data.Type)
            {
                case FPType.Describe:
                    string content = System.Text.Encoding.UTF8.GetString(file_data.Data);

                    fi = DataUtility.ToObject(content, typeof(FileInfomation)) as FileInfomation;

                    if (fi != null)
                    {
                        // Do streaming.
                        if (fi.IsDownload)
                        {
                            string full_path = FileHandler.Instance.GetFullPath(fi.File_Name);

                            if (!string.IsNullOrEmpty(full_path) && File.Exists(full_path))
                            {
                                // Setting file stream.
                                fi.fileStream = new FileStream(full_path, FileMode.Open, FileAccess.Read, FileShare.Read);
                                FileHandler.Instance.dicFileInfo.Add(file_data.File_ID, fi);

                                // Added to streaming list to start streaming.
                                FileProtocol fp = new FileProtocol { Des = file_data.Src, File_ID = file_data.File_ID, Type = FPType.Data };
                                FileHandler.Instance.stremingList.Add(new AgentFileStreaming(_client.Connections[0], fp));

                                // Invoke onDownloadFileRequest event.
                                downloadFileRequest(new FileDownloadEventArgs(fi.File_Name, file_data.Src, fi));
                            }
                            else
                            {
                                FileProtocol fp = new FileProtocol
                                {
                                    Des = file_data.Src,
                                    Type = FPType.Final,
                                    File_ID = file_data.File_ID,
                                    Data = Encoding.UTF8.GetBytes("Failure: Illegal Path..")
                                };
                                Send(fp);
                            }
                        }
                        // Creating FileStream for upload file data.
                        else
                        {
                            fi.fileStream = new FileStream(fi.File_Path, FileMode.Create, FileAccess.Write, FileShare.None);
                            FileHandler.Instance.dicFileInfo.Add(file_data.File_ID, fi);
                        }
                    }
                    break;

                case FPType.Data:
                    if (FileHandler.Instance.dicFileInfo.TryGetValue(file_data.File_ID, out fi))
                    {
                        if (fi.startTime == -1)
                            fi.startTime = Environment.TickCount;

                        if (fi.Length == 0)
                            fi.Length = file_data.Length;

                        recvLogMsg(file_data.Data.Length.ToString());
                        fi.received += (ulong)file_data.Data.Length;
                        fi.fileStream.Write(file_data.Data, 0, file_data.Data.Length);

                        int v = (int)(((float)fi.received / (float)fi.Length) * 100.0f);
                        v = v > 100 ? 100 : v;
                        if (fi.lastProgressValue != v)
                        {
                            fi.lastProgressValue = v;
                            int passed = Environment.TickCount - fi.startTime;
                            double psec = (double)passed / 1000.0;
                            double bps = (double)fi.received / psec;
                            bps = (bps < 0) ? 0 : bps;

                            fi.UpdateProgress(new FileProgressEventArgs(v, fi.File_Name + " on " + NetUtility.ToHumanReadable((long)bps) + " per second"));
                        }

                        if (fi.received >= fi.Length)
                        {
                            int passed = Environment.TickCount - fi.startTime;
                            double psec = (double)passed / 1000.0;
                            double bps = (double)fi.received / psec;

                            fi.UpdateProgress(new FileProgressEventArgs(100, "Done at " + NetUtility.ToHumanReadable((long)bps) + " per second"));

                            fi.fileStream.Flush();
                            fi.fileStream.Close();
                            fi.fileStream.Dispose();

                            fi.CompleteTrans(new FileStatusEventArgs("Downloaded.", fi.File_Name, Agent_ID, StatusCode.File_Transfer_Done));

                            FileHandler.Instance.dicFileInfo.Remove(file_data.File_ID);

                            FileProtocol fp = new FileProtocol 
                                                { 
                                                    Des = file_data.Src, 
                                                    Type = FPType.Final, 
                                                    File_ID = file_data.File_ID,
                                                    Data = Encoding.UTF8.GetBytes("Downloaded.")
                                                };
                            Send(fp);
                        }
                    }
                    break;
                case FPType.Final:
                    if (FileHandler.Instance.dicFileInfo.TryGetValue(file_data.File_ID, out fi))
                    {
                        string reason = Encoding.UTF8.GetString(file_data.Data);

                        FileHandler.Instance.dicFileInfo.Remove(file_data.File_ID);
                        FileHandler.Instance.stremingList.RemoveAll(x => x.isCompleted());

                        fi.CompleteTrans(new FileStatusEventArgs(reason, fi.File_Name, file_data.Src, StatusCode.File_Transfer_Done));
                    }
                    break;
                default:
                    Logger.Debug("Agent Unknown file type: " + Data.ToJson());
                    break;
            }
        }

        public void GetFile(long id, string local_download_path, FileInfomation file_info)
        {
            file_info.IsDownload = true;
            file_info.localTmpPath = local_download_path;

            FileProtocol fp = new FileProtocol
            {
                Des = id,
                File_ID = DateTime.Now.ToBinary(),
                Type = FPType.Describe,
                Data = Encoding.UTF8.GetBytes(file_info.ToJson())
            };

            file_info.fileStream = new FileStream(file_info.File_Path, FileMode.Create, FileAccess.Write, FileShare.None);
            FileHandler.Instance.dicFileInfo.Add(fp.File_ID, file_info);
            Send(fp);
        }

        public void PostFile(long id, string local_file, FileInfomation file_info)
        {
            if (File.Exists(local_file))
            {
                file_info.IsDownload = false;
                file_info.Length = Convert.ToUInt64(new FileInfo(local_file).Length);

                FileProtocol fp = new FileProtocol
                {
                    Des = id,
                    File_ID = DateTime.Now.ToBinary(),
                    Type = FPType.Describe,
                    Data = Encoding.UTF8.GetBytes(file_info.ToJson())
                };

                file_info.fileStream = new FileStream(local_file, FileMode.Open, FileAccess.Read, FileShare.Read);
                FileHandler.Instance.dicFileInfo.Add(fp.File_ID, file_info);

                FileHandler.Instance.stremingList.Add(new AgentFileStreaming(_client.Connections[0], fp));
                Send(fp);
            }
        }
    }

    public class AgentFileStreaming : StreamingProvider
    {
        private FileStream m_inputStream;
        private FileInfomation m_info;
        private int m_sentOffset;
        private int m_chunkLen;
        private byte[] m_tmpBuffer;
        private FileProtocol m_protocol;
        private NetConnection m_conn;
        private bool completed;

        public AgentFileStreaming(NetConnection conn, FileProtocol fp)
        {
            m_conn = conn;
            m_protocol = fp;
            m_inputStream = FileHandler.Instance.dicFileInfo[fp.File_ID].fileStream;
            m_info = FileHandler.Instance.dicFileInfo[fp.File_ID];
            m_chunkLen = conn.Peer.Configuration.MaximumTransmissionUnit - 37 - 6154;
            m_tmpBuffer = new byte[m_chunkLen];
            m_sentOffset = 0;
            m_protocol.Length = (ulong) m_inputStream.Length;
            completed = false;
        }

        public void Heartbeat()
        {
            if (m_inputStream == null)
                return;

            int windowSize, freeWindowSlots;
            m_conn.GetSendQueueInfo(NetDeliveryMethod.ReliableOrdered, 1, out windowSize, out freeWindowSlots);

            if (freeWindowSlots > 0)
            {
                // send another part of the file!
                long remaining = (long)(m_inputStream.Length - m_sentOffset);
                int sendBytes = (remaining > m_chunkLen ? m_chunkLen : (int)remaining);

                // just assume we can read the whole thing in one Read()
                m_inputStream.Read(m_tmpBuffer, 0, sendBytes);

                m_protocol.Type = FPType.Data;
                m_protocol.Data = new byte[sendBytes];
                Array.Copy(m_tmpBuffer, m_protocol.Data, sendBytes);

                BaseAgentController.BaseInstance.Go(CommonTool.Base.ActionType.Send, m_protocol);

                m_sentOffset += sendBytes;

                // TODO: When file size equal to zero, will crash.
                m_info.UpdateProgress(new FileProgressEventArgs((int)(((double)m_sentOffset / (double)m_inputStream.Length) * 100.0f), "Sent " + m_sentOffset + "/" + m_inputStream.Length + " bytes"));
                //FileDaemon.Instance.DownloadProgressUpdate((int)(m_sentOffset / m_inputStream.Length), "Sent " + m_sentOffset + "/" + m_inputStream.Length + " bytes to " + m_connection);
                //Program.Output("Sent " + m_sentOffset + "/" + m_inputStream.Length + " bytes to " + m_connection);

                if (remaining - sendBytes <= 0)
                {
                    m_inputStream.Close();
                    m_inputStream.Dispose();
                    m_inputStream = null;
                    completed = true;
                }
            }
        }

        public bool isCompleted()
        {
            return completed;
        }
    }
}
