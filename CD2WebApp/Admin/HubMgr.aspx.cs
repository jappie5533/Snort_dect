using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootstrapTools;

namespace CD2WebApp.Admin
{
    public partial class HubMgr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        protected string SortExpression
        {
            get { return ViewState["SortExpression"] as string; }
            set { ViewState["SortExpression"] = value; }
        }

        protected SortDirection SortDirection
        {
            get
            {
                object o = ViewState["SortDirection"];
                if (o == null)
                    return SortDirection.Ascending;
                else
                    return (SortDirection)o;
            }
            set { ViewState["SortDirection"] = value; }
        }

        protected void BindData()
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            List<BootstrapTools.CD2BSObject.HubInformation> hubs = sql.GetHubsInformation();

            bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
            switch (this.SortExpression)
            {
                case "ip":
                    hubs = (sortAscending ? hubs.OrderBy(h => h.IP) : hubs.OrderByDescending(h => h.IP)).ToList();
                    break;
                default:
                    hubs = (sortAscending ? hubs.OrderBy(h => h.ID) : hubs.OrderByDescending(h => h.ID)).ToList();
                    break;
            }

            GridView1.DataSource = hubs;
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

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.SortExpression == e.SortExpression)
                this.SortDirection = this.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            else
                this.SortDirection = SortDirection.Ascending;
            this.SortExpression = e.SortExpression;

            GridView1.EditIndex = -1;
            GridView1.SelectedIndex = -1;
        }

        protected void GridView1_Sorted(object sender, EventArgs e)
        {
            BindData();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            GridView1.DataSource = sql.GetHubsInformation();/*sql.GetHubsInfo(TextBox1.Text).ToList();*/
            GridView1.DataBind();
        }
    }
}