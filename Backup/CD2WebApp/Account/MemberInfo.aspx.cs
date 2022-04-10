using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootstrapTools;

namespace CD2WebApp.Account
{
    public partial class MemberCenter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            DetailsView1.DataSource = sql.GetUserInfoWithDataReader(this.User.Identity.Name);
            DetailsView1.DataBind();
        }

        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < DetailsView1.Rows.Count; i++)
            {
                if (DetailsView1.Rows[i].Cells[0].Text.CompareTo("Agent type") == 0)
                    DetailsView1.Rows[i].Cells[1].Text = Enum.GetName(typeof(CommonTool.CD2Constant.AgentType), Convert.ToInt32(DetailsView1.Rows[i].Cells[1].Text));
            }
        }
    }
}