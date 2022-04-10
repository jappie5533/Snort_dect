using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CommonTool;
using BootstrapTools;

namespace CD2WebApp
{
    public class Node
    {
        public List<string> adjacencies { get; set; }
        public string data { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public partial class PeerConnectionDiagram : System.Web.UI.Page
    {
        protected static List<Node> _nodes;
        //protected string JSONnodes { get { return DataUtility.ToJson(_nodes); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            List<BootstrapTools.CD2BSObject.HubInformation> hubs = sql.GetHubsInformation();/*sql.GetHubsInfo();*/
            _nodes = new List<Node>();
            foreach (var hub in hubs)
            {
                Node node = new Node();
                node.id = hub.IP.ToString();
                node.name = hub.IP.ToString();

                if (hub.ConnectionList.Count == 0)
                    node.adjacencies = null;
                else
                    node.adjacencies = hub.ConnectionList.Keys.ToList();

                //node.adjacencies = hub.ConnectionList.Count == 0 ? null : hub.Connections.Split(',').ToList();
                node.type = "hub";
                _nodes.Add(node);
            }

            List<BootstrapTools.CD2BSObject.AgentInformation> users = sql.GetUsersConnected();
            foreach (var u in users)
            {
                Node node = new Node();

                node.id = u.Account;
                node.name = u.Account;

                if (u.Hub.IP != null && !string.IsNullOrEmpty(u.Hub.IP.ToString()))
                    node.adjacencies = u.Hub.IP.ToString().Split(',').ToList();
                node.type = "agent";
                _nodes.Add(node);
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetNodes()
        {
            return DataUtility.ToJson(_nodes);
        }

        [System.Web.Services.WebMethod]
        public static BootstrapTools.CD2BSObject.HubInformation GetNodeInfo(string ip)
        {
            //return new CD2SQLUtility().GetHubInfo_old(ip);
            return new CD2SQLUtility().GetHubInformation(ip);
        }

        [System.Web.Services.WebMethod]
        public static BootstrapTools.CD2BSObject.AgentInformation GetUNodeInfo(string account)
        {
            return new CD2SQLUtility().GetUserInfo(account);
        }
    }
}