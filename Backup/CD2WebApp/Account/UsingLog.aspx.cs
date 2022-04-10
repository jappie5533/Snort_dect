using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootstrapTools;
using CommonTool;

namespace CD2WebApp.Account
{
    public partial class UsingLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        protected void BindData()
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            GridView1.DataSource = sql.GetDateLog(Page.User.Identity.Name, new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Local), DateTime.Now.AddHours(24));
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
            if (!string.IsNullOrEmpty(TextBox1.Text) && !string.IsNullOrEmpty(TextBox2.Text))
            {
                CD2SQLUtility sql = new CD2SQLUtility();
                if (TextBox1.Text.CompareTo(TextBox2.Text) != 0)
                {
                    DateTime tmp_d1 = DateTime.Parse(TextBox1.Text);
                    tmp_d1 = new DateTime(tmp_d1.Year, tmp_d1.Month, tmp_d1.Day, 0, 0, 0, DateTimeKind.Local);

                    DateTime tmp_d2 = DateTime.Parse(TextBox2.Text);
                    tmp_d2 = new DateTime(tmp_d2.Year, tmp_d2.Month, tmp_d2.Day, 0, 0, 0, DateTimeKind.Local);

                    if (DateTime.Parse(TextBox1.Text).CompareTo(DateTime.Parse(TextBox2.Text)) < 0)
                        GridView1.DataSource = sql.GetDateLog(Page.User.Identity.Name, tmp_d1, tmp_d2);
                    else
                        GridView1.DataSource = sql.GetDateLog(Page.User.Identity.Name, tmp_d2, tmp_d1);
                }
                else
                {
                    DateTime tmp_d1 = DateTime.Parse(TextBox1.Text);
                    tmp_d1 = new DateTime(tmp_d1.Year, tmp_d1.Month, tmp_d1.Day, 0, 0, 0, DateTimeKind.Local);

                    GridView1.DataSource = sql.GetDateLog(Page.User.Identity.Name, tmp_d1, tmp_d1.AddHours(24));
                }
                GridView1.DataBind();
            }
            else
                BindData();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.CompareTo("selectLogByDate") == 0 && e.CommandArgument != null)
            {
                CD2SQLUtility sql = new CD2SQLUtility();
                DateTime tmp_d = DateTime.Parse(e.CommandArgument.ToString());
                tmp_d = new DateTime(tmp_d.Year, tmp_d.Month, tmp_d.Day, 0, 0, 0, DateTimeKind.Local);
                GridView2.DataSource = sql.GetLog(Page.User.Identity.Name, tmp_d, tmp_d.AddHours(24));
                GridView2.DataBind();
                ViewState["date"] = e.CommandArgument.ToString();
                MultiView1.ActiveViewIndex = 1;
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            GridView2.EditIndex = -1;
            GridView2.SelectedIndex = -1;
        }

        protected void GridView2_PageIndexChanged(object sender, EventArgs e)
        {
            CD2SQLUtility sql = new CD2SQLUtility();
            DateTime tmp_d = DateTime.Parse(ViewState["date"].ToString());
            tmp_d = new DateTime(tmp_d.Year, tmp_d.Month, tmp_d.Day, 0, 0, 0, DateTimeKind.Local);
            GridView2.DataSource = sql.GetLog(Page.User.Identity.Name, tmp_d, tmp_d.AddHours(24));
            GridView2.DataBind();
        }

        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                Label lbl = GridView2.Rows[i].FindControl("Label1") as Label;
                if (lbl != null)
                    lbl.Text = DateTime.FromBinary(Convert.ToInt64(lbl.Text)).ToString();// Convert.ToInt64(lbl.Text).ToDateTime().ToString();
            }
        }
    }
}