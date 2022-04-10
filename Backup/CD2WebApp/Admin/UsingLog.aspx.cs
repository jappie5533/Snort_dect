using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootstrapTools;
using CommonTool;

namespace CD2WebApp.Admin
{
    public partial class UsingLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void BindData()
        {
            ViewState["Name"] = string.Empty;
            CD2SQLUtility sql = new CD2SQLUtility();
            GridView1.DataSource = sql.GetDateLog();
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

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                CD2SQLUtility sql = new CD2SQLUtility();
                switch (e.CommandName)
                {
                    case "selectLogByDate":
                        DateTime tmp_d = DateTime.Parse(e.CommandArgument.ToString());
                        tmp_d = new DateTime(tmp_d.Year, tmp_d.Month, tmp_d.Day, 0, 0, 0, DateTimeKind.Local);
                        GridView2.DataSource = sql.GetLog(ViewState["Name"].ToString(), tmp_d, tmp_d.AddHours(24));
                        ViewState["date"] = e.CommandArgument.ToString();
                        MultiView1.ActiveViewIndex = 1;
                        GridView2.DataBind();
                        break;
                    case "selectLogByName":
                        ViewState["Name"] = e.CommandArgument.ToString();
                        tmp_d = DateTime.Parse("01/01/1970 00:00:00");
                        tmp_d = new DateTime(tmp_d.Year, tmp_d.Month, tmp_d.Day, 0, 0, 0, DateTimeKind.Local);
                        GridView1.DataSource = sql.GetDateLog(e.CommandArgument.ToString(), tmp_d, DateTime.Now.AddHours(24));
                        GridView1.DataBind();
                        break;
                }
            }
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
            GridView2.DataSource = sql.GetLog(ViewState["Name"].ToString(), tmp_d, tmp_d.AddHours(24));
            GridView2.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox2.Text) && !string.IsNullOrEmpty(TextBox3.Text))
            {
                DateTime tmp_d1 = DateTime.Parse(TextBox2.Text);
                tmp_d1 = new DateTime(tmp_d1.Year, tmp_d1.Month, tmp_d1.Day, 0, 0, 0, DateTimeKind.Local);

                DateTime tmp_d2 = DateTime.Parse(TextBox3.Text);
                tmp_d2 = new DateTime(tmp_d2.Year, tmp_d2.Month, tmp_d2.Day, 0, 0, 0, DateTimeKind.Local);

                CD2SQLUtility sql = new CD2SQLUtility();
                if (TextBox2.Text.CompareTo(TextBox3.Text) != 0)
                {
                    if (tmp_d1.CompareTo(tmp_d2) < 0)
                        GridView1.DataSource = sql.GetDateLog(string.Empty, tmp_d1, tmp_d2);
                    else
                        GridView1.DataSource = sql.GetDateLog(string.Empty, tmp_d2, tmp_d1);
                }
                else
                    GridView1.DataSource = sql.GetDateLog(string.Empty, tmp_d1, tmp_d1.AddHours(24));
                GridView1.DataBind();
            }
            else
                BindData();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                CD2SQLUtility sql = new CD2SQLUtility();
                DateTime tmp_d = DateTime.Parse("01/01/1970 00:00:00");
                tmp_d = new DateTime(tmp_d.Year, tmp_d.Month, tmp_d.Day, 0, 0, 0, DateTimeKind.Local);
                GridView1.DataSource = sql.GetDateLog(TextBox1.Text, tmp_d, DateTime.Now.AddHours(24));
                GridView1.DataBind();
            }
            else
                BindData();
        }

        protected void GridView2_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < GridView2.Rows.Count; i++)
            {
                Label lbl = GridView2.Rows[i].FindControl("Label1") as Label;
                if (lbl != null)
                    lbl.Text = DateTime.FromBinary(Convert.ToInt64(lbl.Text)).ToString();
            }
        }
    }
}