using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agent;
using System.Windows.Forms;
using CommonTool.FileStreamTool;
using Co_Defend_Client_v2.UI;
using CommonTool;

namespace Co_Defend_Client_v2.Agent
{
    internal class AgentLoader : BaseAgentLoader
    {
        private AgentMainForm _mainForm;
        private LoginForm _loginForm;

        protected override void Initialize()
        {
            base.Initialize();

            _loginForm = LoginForm.Instance;
            _mainForm = new AgentMainForm();
        }

        protected override void EventRegist()
        {
            base.EventRegist();

            _mainForm.FormClosing += delegate(object sender, FormClosingEventArgs e)
            {
                this.Stop();
            };

            _loginForm.FormClosing += delegate(object sender, FormClosingEventArgs e)
            {
                this.Stop();
            };

            AgentController.Instance.onStatusMsg += delegate(object sender, StatusMsgArgs e)
            {
                switch (e.Code)
                {
                    case CommonTool.StatusCode.Auth_OK:
                        if (_loginForm.InvokeRequired)
                            _loginForm.Invoke((MethodInvoker)delegate
                            {
                                _loginForm.Hide();
                                _mainForm.ShowForm();
                            });
                        else
                        {
                            _loginForm.Hide();
                            _mainForm.ShowForm();
                        }
                        break;
                    case CommonTool.StatusCode.Auth_Error:
                        if (_loginForm.InvokeRequired)
                            _loginForm.Invoke((MethodInvoker)delegate
                            {
                                MessageBox.Show(_loginForm, e.Message, e.Code.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _loginForm.EnableComponent(true);
                            });
                        else
                        {
                            MessageBox.Show(_loginForm, e.Message, e.Code.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _loginForm.EnableComponent(true);
                        }
                        break;
                }
            };
        }

        protected override void SetupController()
        {
            this.Controller = AgentController.Instance;
        }

        protected override void RegistAgents()
        {
            this.RegistAgentList.Add(typeof(SPA));
            this.RegistAgentList.Add(typeof(RAA));
            this.RegistAgentList.Add(typeof(PAA));
        }

        protected override void Start()
        {
            base.Start();

            _loginForm.Show();
        }
    }
}
