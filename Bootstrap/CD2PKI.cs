using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using CommonTool;
using System.Net;
using CommonTool.Base;
using IP = System.String;
using PublicKey = System.String;
using String = System.String;
using BootstrapTools;
using HubInformation = BootstrapTools.CD2BSObject.HubInformation;

namespace Bootstrap
{
    public class CD2PKI
    {
        private static CD2PKI _instance;
        private CD2SQLUtility sql;
        public CD2PKI()
        {
            this.sql = new CD2SQLUtility();
        }

        public static CD2Cert CertificateAuthority(string account, HubInformation hub/*string hubIP*/)
        {
            if (_instance == null)
                _instance = new CD2PKI();

            BootstrapTools.CD2BSObject.KeyPair agentKP = _instance.sql.GetAgentKey(account);

            // Set which hub is registed to sql by agent id
            _instance.sql.SetUserLogin(account, hub.ID);

            if (agentKP != null)
                return new CD2Cert(hub.KP.PublicKey, agentKP.PrivateKey);
            else
                return null;
        }

        public static bool RegistrationAuthority(AgentRegistRequestContent agent, long ID, string IP)
        {
            if (_instance == null)
                _instance = new CD2PKI();

            BootstrapTools.CD2BSObject.KeyPair kp;
            return _instance.GenerateRSAKey(out kp) && _instance.sql.SetAgentInfo(agent, kp, ID, IP);
        }

        public static bool RegistrationAuthority(ref HubInformation hi)
        {
            if (_instance == null)
                _instance = new CD2PKI();

            BootstrapTools.CD2BSObject.KeyPair kp = new BootstrapTools.CD2BSObject.KeyPair();
            if (_instance.GenerateRSAKey(out kp))
            {
                hi.KP = kp;
                hi.TS = DateTime.Now.ToBinary();
                hi.StartTime = hi.TS;

                _instance.sql.SetHubInfo(ref hi);

                hi.ConnectionList = _instance.sql.GetHubsPubKey(hi.ConnectionList);

                return true;
            }
            return false;
        }

        public static bool ValidationAuthority(ref AgentValidationContent av)
        {
            if (_instance == null)
                _instance = new CD2PKI();

            CD2BSObject.AgentInformation agentInfo = new CD2BSObject.AgentInformation();
            agentInfo = _instance.sql.GetUserInfo(av.Account);
            if (agentInfo != null)
            {
                av.PublicKey = agentInfo.Key.PublicKey;
                av.Type = (CD2Constant.AgentType)agentInfo.Type;
                av.AgentID = agentInfo.ID.ToString();
                return true;
            }
            return false;
        }

        public static void Deregistration(HubInformation hi)
        {
            if (_instance == null)
                _instance = new CD2PKI();

            _instance.sql.DelHubInfo(hi);
        }

        private bool GenerateRSAKey(out BootstrapTools.CD2BSObject.KeyPair kp)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                kp = new CD2BSObject.KeyPair { PublicKey = rsa.ToXmlString(false), PrivateKey = rsa.ToXmlString(true) };
            }
            catch
            {
                kp = null;
                return false;
            }
            return true;
        }
    }
}
