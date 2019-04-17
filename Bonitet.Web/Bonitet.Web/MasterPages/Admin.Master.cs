using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    li_clients.Visible = true;
                    li_packs.Visible = true;
                    li_requests.Visible = true;
                    li_add.Visible = true;
                    li_password_change.Visible = true;
                    li_year_change.Visible = true;
                    li_logout.Visible = true;
                    li_generate.Visible = true;
                    li_generate_force.Visible = true;
                    li_user_reports.Visible = true;
                    li_request_logs.Visible = true;
                }
                else if (userobj.Type == 1)
                {
                    Response.Redirect("/Authenticated/UserProfile.aspx");
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }
            }
            SetNavActivePage();

        }

        public void SetNavActivePage()
        {
            var curUrl = Request.Url.AbsolutePath.ToLower();
            var userobj = UserHelper.instance;

            if (curUrl.ToLower().Equals(a_packs.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "active";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";

            }
            else if (curUrl.ToLower().Equals(a_clients.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "active";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_requests.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "active";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_request_logs.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "active";
            }
            else if (curUrl.ToLower().Equals(a_add.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "active";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_password_change.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "active";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_year_change.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "active";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_generate.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "active";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_generate_force.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "active";
                li_user_reports.Attributes["class"] = "";
                li_request_logs.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_user_reports.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
                li_year_change.Attributes["class"] = "";
                li_generate.Attributes["class"] = "";
                li_generate_force.Attributes["class"] = "";
                li_user_reports.Attributes["class"] = "active";
                li_request_logs.Attributes["class"] = "";
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            userobj.Logout();

            Response.Redirect("/Admin/Default.aspx");
        }
    }
}