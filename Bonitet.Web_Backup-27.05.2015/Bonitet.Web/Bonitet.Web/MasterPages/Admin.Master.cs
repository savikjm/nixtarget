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
                    li_logout.Visible = true;
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

            }
            else if (curUrl.ToLower().Equals(a_clients.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "active";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_requests.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "active";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_add.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "active";
                li_password_change.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_password_change.HRef.ToLower()))
            {
                li_packs.Attributes["class"] = "";
                li_clients.Attributes["class"] = "";
                li_requests.Attributes["class"] = "";
                li_add.Attributes["class"] = "";
                li_password_change.Attributes["class"] = "active";
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