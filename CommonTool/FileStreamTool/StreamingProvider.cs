using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lidgren.Network;

namespace CommonTool.FileStreamTool
{
    public interface StreamingProvider
    {
        void Heartbeat();
        bool isCompleted();
    }

    internal class Streaming : StreamingProvider
    {
		private FileStream m_inputStream;
		private int m_sentOffset;
		private int m_chunkLen;
		private byte[] m_tmpBuffer;
		private NetConnection m_connection;
        private string server_file;
        internal string file_name;
		
		public Streaming(NetConnection conn, string local_file, string server_file)
		{
			m_connection = conn;
			m_inputStream = new FileStream(local_file, FileMode.Open, FileAccess.Read, FileShare.Read);
			m_chunkLen = m_connection.Peer.Configuration.MaximumTransmissionUnit - 21;
			m_tmpBuffer = new byte[m_chunkLen];
			m_sentOffset = 0;
            this.server_file = server_file;
            this.file_name = Path.GetFileName(m_inputStream.Name);
		}

		public void Heartbeat()
		{
			if (m_inputStream == null)
				return;
            
			int windowSize, freeWindowSlots;
			m_connection.GetSendQueueInfo(NetDeliveryMethod.ReliableOrdered, 1, out windowSize, out freeWindowSlots);
			if (freeWindowSlots > 0)
			{
				// send another part of the file!
				long remaining = (long)(m_inputStream.Length - m_sentOffset);
				int sendBytes = (remaining > m_chunkLen ? m_chunkLen : (int)remaining);

				// just assume we can read the whole thing in one Read()
				m_inputStream.Read(m_tmpBuffer, 0, sendBytes);

				NetOutgoingMessage om;
				if (m_sentOffset == 0)
				{
                    //FileDaemon.Instance.StatusUpdate(new FileStatusEventArgs("File Transfer Starting...", server_file, StatusCode.File_Transfer_Starting));
					// first message; send length, chunk length and file name
					om = m_connection.Peer.CreateMessage(sendBytes + 8);
                    om.Write((byte)CommonTool.FileStreamTool.FileDaemon.FileType.Upload);
					om.Write((ulong)m_inputStream.Length);
					om.Write(Path.Combine(server_file, Path.GetFileName(m_inputStream.Name)));
					m_connection.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 1);
				}

				om = m_connection.Peer.CreateMessage(sendBytes + 8);
                om.Write((byte)CommonTool.FileStreamTool.FileDaemon.FileType.Upload);
				om.Write(m_tmpBuffer, 0, sendBytes);

				m_connection.SendMessage(om, NetDeliveryMethod.ReliableOrdered, 1);
				m_sentOffset += sendBytes;

                // TODO: When file size equal to zero, will crash.
                //FileDaemon.Instance.DownloadProgressUpdate((int)(m_sentOffset / m_inputStream.Length), "Sent " + m_sentOffset + "/" + m_inputStream.Length + " bytes to " + m_connection);
				//Program.Output("Sent " + m_sentOffset + "/" + m_inputStream.Length + " bytes to " + m_connection);

				if (remaining - sendBytes <= 0)
				{
					m_inputStream.Close();
					m_inputStream.Dispose();
					m_inputStream = null;
				}
			}
		}

        public bool isCompleted()
        {
            return false;
        }
    }
}
