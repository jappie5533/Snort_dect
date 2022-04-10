using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace CommonTool.FileStreamTool
{
    public class FileHandler
    {
        public event EventHandler<FileProgressEventArgs> onProgressUpdate;
        public event EventHandler<FileStatusEventArgs> onStatusUpdate;
        public event EventHandler<FileDownloadEventArgs> onDownloadRequest;

        public Dictionary<long, FileInfomation> dicFileInfo;
        public List<StreamingProvider> stremingList;
        public string TmpFileFolder { get {return Path.Combine(_root, "tmp");} }

        private static FileHandler _instance;
        private FileDaemon _server;
        private string _root;
        private long _agentID;

        internal BlockingCollection<FileDaemon.FileInfo> _fileTransQueue;
        internal long Agent_ID { get { return _agentID; } }

        public static FileHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FileHandler();

                return _instance;
            }
        }

        private FileHandler()
        {
            _server = FileDaemon.Instance;
            _root = Application.StartupPath;
            _fileTransQueue = new BlockingCollection<FileDaemon.FileInfo>();
            dicFileInfo = new Dictionary<long, FileInfomation>();
            stremingList = new List<StreamingProvider>();

            registEvent();
        }

        public void SetAgentID(long id)
        {
            _agentID = id;
        }

        public void Start()
        {
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
        }

        public void GetFile(string ip, string server_file, string local_file)
        {
            _fileTransQueue.Add(new FileDaemon.FileInfo(ip, server_file, local_file, true));
        }

        public void PostFile(string ip, string local_file, string server_file)
        {
            if (File.Exists(local_file))
                _fileTransQueue.Add(new FileDaemon.FileInfo(ip, server_file, local_file, false));
        }

        public string GetFullPath(string req_path)
        {
            string fullPath = Path.GetFullPath(Path.Combine(_root, req_path.TrimStart(new char[] { '\\', '/' })));

            if (fullPath.StartsWith(_root, StringComparison.Ordinal))
                return fullPath;

            return string.Empty;
        }

        private void registEvent()
        {
            _server.onDownloadProgressUpdate += delegate(object sender, FileProgressEventArgs e)
            {
                if (onProgressUpdate != null)
                    onProgressUpdate.Invoke(sender, e);
            };

            _server.onStatusUpdate += delegate(object sender, FileStatusEventArgs e)
            {
                if (onStatusUpdate != null)
                    onStatusUpdate.Invoke(sender, e);
            };

            _server.onDownloadRequest += delegate(object sender, FileDownloadEventArgs e)
            {
                if (onDownloadRequest != null)
                    onDownloadRequest.Invoke(sender, e);
            };
        }

        private string TirmPath(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            
            return r.Replace(path, "");
        }
    }
}
