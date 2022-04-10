using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Policy;

namespace CommonTool
{
    public sealed class ServiceManager
    {
        public static ServiceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServiceManager();

                return _instance;
            }
        }
        public ServiceList UserDefineSvc { get { return _uSvc; } }
        public ServiceList NativeSvc { get { return _nSvc; } }
        public ServiceConfig ServiceConf { get { return _svcConf; } }
        public List<string> WorkFineDLL { get { return _work_fine_DLL; } }
        public string ServiceFolder { get { return _svcFolder; } }
        public string UserDefineFolder { get { return _uFolder; } }
        public event EventHandler<EventArgs> onAllowServiceChanged;

        private static ServiceManager _instance;
        private string _svcFolder;
        private string _uFolder;
        private string _configFile;
        private string _nativeFilePath;
        private ServiceList _uSvc;
        private ServiceList _nSvc;
        private ServiceConfig _svcConf;
        private List<string> _work_fine_DLL;
        private AppDomain _ad_plugin;

        private ServiceManager()
        {
            _uSvc = new ServiceList();
            _nSvc = new ServiceList();
            _work_fine_DLL = new List<string>();
        }

        public void Initailize()
        {
            _svcFolder = Path.Combine(Application.StartupPath, "svc");
            _uFolder = Path.Combine(_svcFolder, "usvc");
            _configFile = Path.Combine(_svcFolder, "svc.conf");
            _nativeFilePath = Path.Combine(_svcFolder, CD2Constant.Core_Service_DLL);

            if (!Directory.Exists(_svcFolder))
                Directory.CreateDirectory(_svcFolder);

            if (!File.Exists(_nativeFilePath) && File.Exists(Path.Combine(Application.StartupPath, CD2Constant.Core_Service_DLL)))
            {
                File.Copy(Path.Combine(Application.StartupPath, CD2Constant.Core_Service_DLL), _nativeFilePath);
            }

            if (!Directory.Exists(_uFolder))
                Directory.CreateDirectory(_uFolder);

            Load();
            
            InitConfig();
            
        }

        private void InitConfig()
        {
            if (!File.Exists(_configFile))
            {
                _svcConf = new ServiceConfig();

                using (FileStream fs = File.Create(_configFile))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(DataUtility.ToJson(_svcConf));
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (FileStream fs = File.OpenRead(_configFile))
                {
                    StreamReader sr = new StreamReader(fs);
                    _svcConf = DataUtility.ToObject(sr.ReadToEnd(), typeof(ServiceConfig)) as ServiceConfig;
                    sr.Close();
                }
            }
        }

        public void SaveConfig()
        {
            using (FileStream fs = File.Open(_configFile, FileMode.Create, FileAccess.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(DataUtility.ToJson(_svcConf));
                sw.Flush();
                sw.Close();
            }

            if (onAllowServiceChanged != null)
                onAllowServiceChanged.Invoke(this, null);
        }

        public List<string> GetAllowService()
        {
            List<string> all = new List<string>();

            // Native.
            foreach (string s in _nSvc.SPA)
                all.Add(s.Split(new char[] { ' ' })[0]);
            foreach (string s in _nSvc.RAA)
                all.Add(s.Split(new char[] { ' ' })[0]);
            foreach (string s in _nSvc.PAA)
                all.Add(s.Split(new char[] { ' ' })[0]);

            // User-Define.
            foreach (string s in _uSvc.SPA)
                if (!_svcConf.Not_Allow_User_Define_Service.Contains(s))
                    all.Add(s.Split(new char[] { ' ' })[0]);
            foreach (string s in _uSvc.RAA)
                if (!_svcConf.Not_Allow_User_Define_Service.Contains(s))
                    all.Add(s.Split(new char[] { ' ' })[0]);
            foreach (string s in _uSvc.PAA)
                if (!_svcConf.Not_Allow_User_Define_Service.Contains(s))
                    all.Add(s.Split(new char[] { ' ' })[0]);

            return all;
        }

        private byte[] loadFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            return buffer;
        }

        private bool LoadAddList(string fileName, ServiceList svl)
        {
            bool isLoadSuccess = false;

            if (File.Exists(fileName))
            {
                try
                {
                    Assembly assmb = _ad_plugin.Load(AssemblyName.GetAssemblyName(fileName));
                    foreach (Type tp in assmb.GetExportedTypes())
                    {
                        try
                        {
                            List<string> list = svl.GetType().GetProperty(tp.Namespace).GetValue(svl, null) as List<string>;

                            if (list != null)
                            {
                                list.Add(tp.Name + " From " + Path.GetFileName(fileName));
                                isLoadSuccess = true;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Debug(ex.Message, ex.StackTrace);
                }
            }

            return isLoadSuccess;
        }


        private void Load()
        {
            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setup.PrivateBinPath = Path.Combine(System.Environment.CurrentDirectory, "svc");
                Evidence evi = AppDomain.CurrentDomain.Evidence;

                _ad_plugin = AppDomain.CreateDomain("cd2_plugin", evi, setup);

                // Load Native.
                LoadAddList(_nativeFilePath, _nSvc);

                // Load User-Define.
                DirectoryInfo di = new DirectoryInfo(_uFolder);
                foreach (FileInfo fi in di.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                    if (LoadAddList(fi.FullName, _uSvc))
                        _work_fine_DLL.Add(fi.Name);
            }
            finally
            {
                AppDomain.Unload(_ad_plugin);
            }
        }

        public void Reload()
        {
            _nSvc.ClearAll();
            _uSvc.ClearAll();
            _work_fine_DLL.Clear();
            Load();
        }

        public object TryCreateInstance(string svc, string agentName)
        {
            _ad_plugin = AppDomain.CreateDomain("cd2_plugin");

            try
            {
                // Native Find First.
                Assembly assmb = _ad_plugin.Load(loadFile(_nativeFilePath));
                foreach (CD2Constant.AgentType type in Enum.GetValues(typeof(CD2Constant.AgentType)))
                {
                    if ((int)type > (int)Enum.Parse(typeof(CD2Constant.AgentType), agentName))
                        break;

                    object obj = assmb.CreateInstance(type.ToString() + "." + svc);

                    if (obj != null)
                        return obj;
                }

                // User-Define.
                foreach (string dll in ServiceManager.Instance.WorkFineDLL)
                    if (!ServiceConf.Not_Allow_User_Define_Service.Contains(svc + " From " + dll))
                    {
                        assmb = _ad_plugin.Load(loadFile(Path.Combine(_uFolder, dll)));

                        foreach (CD2Constant.AgentType type in Enum.GetValues(typeof(CD2Constant.AgentType)))
                        {
                            if ((int)type > (int)Enum.Parse(typeof(CD2Constant.AgentType), agentName))
                                break;

                            object obj = assmb.CreateInstance(type.ToString() + "." + svc);

                            if (obj != null)
                                return obj;
                        }
                    }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.Message, ex.StackTrace);
            }
            finally
            {
                AppDomain.Unload(_ad_plugin);
            }

            return null;
        }
    }
}
