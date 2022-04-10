using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace CommonTool
{
    public class ZipUtility
    {
        public static ZipUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ZipUtility();

                return _instance;
            }
        }

        public class ZipOptions
        {
            public bool IsZip;
            public string ZipPath;
            public string FilePath;
            public event EventHandler<EventArgs> onZipCompleted;
            public event EventHandler<EventArgs> onUnZipCompleted;

            internal void InvokeZipCompleted(object sender, EventArgs e)
            {
                if (onZipCompleted != null)
                    onZipCompleted.Invoke(sender, e);
            }

            internal void InvokeUnZipCompleted(object sender, EventArgs e)
            {
                if (onUnZipCompleted != null)
                    onUnZipCompleted.Invoke(sender, e);
            }
        }

        private static ZipUtility _instance;
        private BackgroundWorker _worker;
        private BlockingCollection<ZipOptions> _queue;
        private ZipOptions _nowOptions;

        private ZipUtility()
        {
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += new DoWorkEventHandler(_worker_DoWork);
            _queue = new BlockingCollection<ZipOptions>();
        }

        public void StartZipWorker()
        {
            _worker.RunWorkerAsync();
        }

        public void StopZipWorker()
        {
            _worker.CancelAsync();
        }

        public void Zip(ZipOptions options)
        {
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();

            _queue.Add(options);
        }

        public void UnZip(ZipOptions options)
        {
            if (!_worker.IsBusy)
                _worker.RunWorkerAsync();

            _queue.Add(options);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!e.Cancel)
            {
                _nowOptions = _queue.Take();

                if (_nowOptions.IsZip)
                {
                    using (var zip = new ZipFile(Encoding.UTF8))
                    {
                        zip.AddFile(_nowOptions.FilePath, "");
                        zip.SaveProgress += new EventHandler<SaveProgressEventArgs>(zip_SaveProgress);
                        zip.Save(_nowOptions.ZipPath);
                    }
                }
                else
                {
                    using (var zip = ZipFile.Read(_nowOptions.ZipPath))
                    {
                        zip.AlternateEncoding = Encoding.UTF8;
                        zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                        zip.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip_ExtractProgress);
                        zip.ExtractAll(_nowOptions.FilePath);
                    }
                }
            }
        }

        private void zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Saving_Completed)
            {
                _nowOptions.InvokeZipCompleted(_nowOptions, e);
                (sender as ZipFile).SaveProgress -= zip_SaveProgress;
            }
        }

        private void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_AfterExtractAll)
            {
                _nowOptions.InvokeUnZipCompleted(_nowOptions, e);
                (sender as ZipFile).ExtractProgress -= zip_ExtractProgress;
            }
        }
    }
}
