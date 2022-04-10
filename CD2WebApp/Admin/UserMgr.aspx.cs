using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootstrapTools;

namespace CD2WebApp.Admin
{
    public partial class UserMgr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindData();
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                Label lbl = GridView1.Rows[i].FindControl("AgentType") as Label;
                if (lbl != null)
                    lbl.Text = Enum.GetName(typeof(CommonTool.CD2Constant.AgentType), Convert.ToInt32(lbl.Text));
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.EditIndex = -1;
            GridView1.SelectedIndex = -1;
        }

        protected void BindData()
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            List<BootstrapTools.CD2BSObject.AgentInformation> users = sql.GetUsersInfo(string.Empty);

            bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
            switch (this.SortExpression)
            {
                case "email":
                    users = (sortAscending ? users.OrderBy(u => u.Email) : users.OrderByDescending(u => u.Email)).ToList();
                    break;
                case "account":
                    users = (sortAscending ? users.OrderBy(u => u.Account) : users.OrderByDescending(u => u.Account)).ToList();
                    break;
                default:
                    users = (sortAscending ? users.OrderBy(u => u.UID) : users.OrderByDescending(u => u.UID)).ToList();
                    break;
            }

            GridView1.DataSource = users;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
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

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Int32.Parse(GridView1.DataKeys[e.RowIndex].Value.ToString());
            string account = ((TextBox)(GridView1.Rows[e.RowIndex].FindControl("Account"))).Text;
            string email = ((TextBox)(GridView1.Rows[e.RowIndex].FindControl("Email"))).Text;
            int type = Convert.ToInt32(((DropDownList)(GridView1.Rows[e.RowIndex].FindControl("DDL_AgentType"))).SelectedValue);
            CD2SQLUtility sql = new CD2SQLUtility();
            sql.UpdateUserInfo(id, account, email, type);
            GridView1.EditIndex = -1;
            BindData();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            GridView1.DataSource = sql.GetUsersInfo(TextBox1.Text).ToList();
            GridView1.DataBind();
        }
    }
}