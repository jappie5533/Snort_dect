using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Co_Defend_Client_v2.UI
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();

            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            //this.textBoxDescription.Text = AssemblyDescription;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("The feature of collaborative applications is to cooperation among joining nodes by utilizing various of resources distributed nodes over internet (or intranet). It is good choice to develop collaborative applications based on P2P networks system because Peer-to-Peer (P2P) networks system can be applied to integrate resources over peers. But most of popular P2P systems are focused on files or content sharing and security problems are not considered seriously. It's not enough to develop collaborative applications by current P2P systems which have been implemented. We setup secure P2P networks by authentication of joining peers, encrypted data communication and peers with three levels of priorities. Based on secure P2P networks, a scalable and flexible collaborative application platform composed of core services and user defined services is built. Various of resources provided by peers can be utilized easily by execution of services. Users can develop their creative collaborative applications by our proposed collaborative applications platform. ");
            sb.AppendLine();
            sb.AppendLine("Contact us: ");
            sb.AppendLine("\tIf you have any inquiries, please feel free to get in touch with us!");
            sb.AppendLine();
            sb.AppendLine("\tDevelopment & tech. support");
            sb.AppendLine("\tE-mail: cclien0725@gmail.com");
            sb.AppendLine("\tE-mail: rockers7414@gmail.com");
            this.textBoxDescription.Text = sb.ToString();
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
