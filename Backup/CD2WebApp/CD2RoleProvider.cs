using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BootstrapTools;

namespace CD2WebApp
{
    public class CD2RoleProvider : RoleProvider
    {
        private string pApplicationName;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name))
                name = "CD2RoleProvider";

            if (string.IsNullOrEmpty(config["decription"]))
            {
                config.Remove("decription");
                config.Add("decription", "CD2 Role Provider");
            }

            base.Initialize(name, config);

            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
                pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            else
                pApplicationName = config["applicationName"];
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            if (sql.IsAdmin(username))
                return new string[] { "admin" };
            return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}