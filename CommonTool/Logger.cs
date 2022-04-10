using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CommonTool
{
    public class Logger
    {
        private static TextWriter _debugWriter;
        private static TextWriter _logWriter;
        private static string _folder;
        private static string _dateFormate;
        private static string _logDate;

        public static void Initialize()
        {
            _folder = Path.Combine(Application.StartupPath, "log");

            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            _dateFormate = "yyyy-MM-dd";
            _logDate = DateTime.Today.ToString(_dateFormate);
            _debugWriter = createWriter("debug", _logDate);
            _logWriter = createWriter("log", _logDate);
        }

        private static StreamWriter createWriter(string logType, string date)
        {
            try
            {
                FileStream stream = new FileInfo(Path.Combine(_folder, string.Format(logType + "-{0}.log", date))).Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);// OpenWrite();
                stream.Position = stream.Length;
                StreamWriter sw = new StreamWriter(stream);
                return sw;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Can't create log file.");
            }

            return null;
        }

        public static void Log(string message)
        {
            try
            {
                DateTime now = DateTime.Now;

                if (now.ToString(_dateFormate) != _logDate)
                {
                    _logDate = now.ToString(_dateFormate);
                    _logWriter.Close();
                    _logWriter = createWriter("log", _logDate);
                }

                _logWriter.WriteLine(string.Format("[{0}]:\n\t{1}", now.ToString("yyyy/MM/dd HH:mm:ss"), message));
            }
            catch { }
        }
        public static void Debug(string message)
        {
            Debug(message, "");
        }

        public static void Debug(string message, string stackTrace)
        {
            try
            {
                DateTime now = DateTime.Now;

                // Check the date mapping to the log file' name is or not is equal.
                if (now.ToString(_dateFormate) != _logDate)
                {
                    _logDate = now.ToString(_dateFormate);
                    _debugWriter.Close();
                    _debugWriter = createWriter("debug", _logDate);
                }

                if (!string.IsNullOrEmpty(stackTrace))
                    stackTrace = "\n\t" + stackTrace;

                _debugWriter.WriteLine(string.Format("[{0}]:\n\t{1}", now.ToString("yyyy/MM/dd HH:mm:ss"), message + stackTrace));
            }
            catch { }
        }

        public static void Close()
        {
            try
            {
                _logWriter.Flush();
                _logWriter.Close();
                _logWriter.Dispose();

                _debugWriter.Flush();
                _debugWriter.Close();
                _debugWriter.Dispose();
            }
            catch { }
        }
    }
}
