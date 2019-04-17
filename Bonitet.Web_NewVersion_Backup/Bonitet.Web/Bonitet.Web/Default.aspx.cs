using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            p_message.Visible = false;
            p_disabled.Visible = false;

            var username = tb_username.Text.Trim();
            var password = tb_password.Text.Trim();
            var userobj = UserHelper.instance;

            var res = 0;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                res = userobj.Login(username, password, 1);
            }

            if (userobj.isAuthenticated)
            {
                if (string.IsNullOrEmpty(Request.QueryString["path"]) == false)
                    Response.Redirect(HttpUtility.HtmlDecode(Request.QueryString["path"]));
                else
                    Response.Redirect("/Authenticated/UserProfile.aspx");
            }
            else
            {
                if (res == 2)
                    p_disabled.Visible = true;
                else if (res == 0)
                    p_message.Visible = true;
            }
        }
    }
}