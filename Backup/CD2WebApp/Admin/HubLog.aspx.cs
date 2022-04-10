using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootstrapTools;

namespace CD2WebApp.Admin
{
    public partial class HubLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            GridView1.DataSource = sql.GetHubLog(string.Empty);
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.EditIndex = -1;
            GridView1.SelectedIndex = -1;
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            GridView1.DataSource = sql.GetHubLog(TextBox1.Text);
            GridView1.DataBind();
        }
    }
}