using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BootstrapTools;
using System.Text.RegularExpressions;
using System.Configuration.Provider;
using CommonTool;

namespace CD2WebApp
{
    public class CD2Membership : MembershipProvider
    {
        // CD2Membership property
        private string pApplicationName;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;
        private string pPasswordStrengthRegularExpression;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private MembershipPasswordFormat pPasswordFormat;

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if(string.IsNullOrEmpty(configValue))
                return defaultValue;
            return configValue;
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (string.IsNullOrEmpty(name))
                name = "CD2MembershipProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove(config["description"]);
                config.Add("description", "CD2Membership provider");
            }

            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthReqularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            string format = config["passwordFormat"];
            if (format == null)
                format = "Hashed";

            switch (format)
            { 
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }
        }

        #region GET/SET Property
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }
        #endregion

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                return false;
                //if (args.FailureInformation != null)
                //    throw args.FailureInformation;
                //else
                //    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            }
            else
            {
                CD2SQLUtility sql = new CD2SQLUtility();
                if (sql.UserChangePassword(username, newPassword.CreateMD5Hash(), oldPassword.CreateMD5Hash()))
                    return true;
            }
            return false;
        }

        protected override void OnValidatingPassword(ValidatePasswordEventArgs e)
        {
            base.OnValidatingPassword(e);
            
            if (e.Password.Length < MinRequiredPasswordLength)
                e.Cancel = true;

            int count = 0;
            for (int i = 0; i < e.Password.Length; i++)
                if (!char.IsLetterOrDigit(e.Password, i))
                    count++;

            if (count < MinRequiredNonAlphanumericCharacters)
                e.Cancel = true;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (!Regex.IsMatch(username, @"\w{3,50}"))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (!Regex.IsMatch(email, @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }
            
            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser user = GetUser(username, true);
            if (user == null)
            {
                CD2SQLUtility sql = new CD2SQLUtility();
                if (sql.UserSignUp(username, password.CreateMD5Hash(), email))
                {
                    status = MembershipCreateStatus.Success;
                    return GetUser(username, true);
                }
                else
                    status = MembershipCreateStatus.ProviderError;
            }
            else 
                status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            //throw new NotImplementedException();
            CD2SQLUtility sql = new CD2SQLUtility();
            BootstrapTools.CD2BSObject.AgentInformation user = sql.GetUserInfo(username);
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser(
                    "CD2Membership",
                    username,
                    user.UID,
                    user.Email,
                    string.Empty,
                    string.Empty,
                    true,
                    false,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.Now,
                    DateTime.Now
                    );

                return memUser;
            }
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            return sql.GetUserNameByEmail(email);
        }

        public override bool ValidateUser(string username, string password)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            return sql.UserAuthenticate(username, password.CreateMD5Hash());
        }

        #region Not implemented yet
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}