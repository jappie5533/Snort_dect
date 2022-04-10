using System;
using System.Windows.Forms;
using CommonTool;
using CommonTool.FileStreamTool;
using System.Collections.Generic;

namespace Agent
{
    public abstract class BaseAgentLoader : ApplicationContext
    {
        protected List<Type> RegistAgentList { get { return BaseAgentController.BaseInstance.I_RegistAgentList; } }
        protected BaseAgentController Controller { set { BaseAgentController.BaseInstance = value; } }

        protected BaseAgentLoader()
        {
            Initialize();
            EventRegist();
            SetupController();
            RegistAgents();
            LoadAgent();
        }

        protected virtual void Initialize()
        {
            Logger.Initialize();
            ServiceManager.Instance.Initailize();
            ServiceLogger.Instance.Initialize();
        }

        protected virtual void EventRegist()
        {
            Application.ApplicationExit += delegate(object sender, EventArgs e)
            {
                Logger.Log("========== AgentLoader  Exited... ==========");
                Logger.Close();
            };
        }

        protected abstract void SetupController();
        protected abstract void RegistAgents();

        private void LoadAgent()
        {
            if (RegistAgentList.Count > 0)
            {
                foreach (Type type in RegistAgentList)
                {
                    BaseAgent ba = Activator.CreateInstance(type) as BaseAgent;

                    if (ba != null)
                        BaseAgentController.BaseInstance.I_Agent.Add(type.Name, ba);
                }

                BaseAgentController.BaseInstance.I_AgentEventRegist();
                Start();
            }
            else
            {
                MessageBox.Show("Application can not starup without any registed agents, please add regist list on \"RegistAgent\" function and use \"RegistAgentList\" property to add agents.",
                                "Loading Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                                );
                Logger.Close();
                Environment.Exit(0);
            }
        }

        protected virtual void Start()
        {
            Logger.Log("========== AgentLoader Startup... ==========");
            FileHandler.Instance.Start();
            ZipUtility.Instance.StartZipWorker();
        }

        protected virtual void Stop()
        {
            FileHandler.Instance.Stop();
            ZipUtility.Instance.StopZipWorker();
            ServiceLogger.Instance.ClearTmpFolder();
            Hub.BaseHub.Instance.Stop();
            Application.Exit();
        }
    }
}
