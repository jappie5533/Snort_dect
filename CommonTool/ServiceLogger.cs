using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CommonTool
{
    public sealed class ServiceLogger
    {
        public static ServiceLogger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServiceLogger();

                return _instance;
            }
        }
        public string BackLogPath { get { return _svcLogPath; } }
        public string TmpPath { get { return _tmpPath; } }

        private static ServiceLogger _instance;
        private string _svcLogPath;
        private string _tmpPath;

        private ServiceLogger()
        {
            _svcLogPath = Path.Combine(Application.StartupPath, "svc_backlog");
            _tmpPath = Path.Combine(Application.StartupPath, "tmp");
        }

        public void Initialize()
        {
            if (!Directory.Exists(_svcLogPath))
                Directory.CreateDirectory(_svcLogPath);

            if (!Directory.Exists(_tmpPath))
                Directory.CreateDirectory(_tmpPath);
        }

        public void ClearTmpFolder()
        {
            if (Directory.Exists(_tmpPath))
                Directory.Delete(_tmpPath, true);
        }

        /// <summary>
        /// To move the log file with specific log name, and will check the destination path whether or not have the duplicate name, and return the final log name.
        /// </summary>
        /// <param name="logname">log file name.</param>
        /// <param name="content"></param>
        /// <returns>the final log file name.</returns>
        public string MoveLogFile(string src_file_path, string content)
        {
            string des_file_path = Path.Combine(_svcLogPath, Path.GetFileName(src_file_path));
            int iteratorIndex = 1;

            // Check source file path whether or not is exist.
            if (File.Exists(src_file_path))
            {
                // Check destination file path whether or not have duplicate name.
                while (true)
                {
                    if (File.Exists(des_file_path))
                        des_file_path = Path.Combine(_svcLogPath, Path.GetFileNameWithoutExtension(src_file_path) + "(" + iteratorIndex++ + ")" + Path.GetExtension(src_file_path));
                    else
                        break;
                }
                
                // Move file from source path to destination path.
                File.Move(src_file_path, des_file_path);

                // return the final log name.
                return Path.GetFileName(des_file_path);
            }

            // Source file is not exist, and return the empty string.
            return string.Empty;
        }
    }
}
