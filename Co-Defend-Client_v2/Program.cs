using System;
using System.Windows.Forms;
using Co_Defend_Client_v2.Agent;

namespace Co_Defend_Client_v2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new AgentLoader());

            Environment.Exit(0);
        }
    }
}
