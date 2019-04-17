using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    if (IsPostBack == false)
                        Response.Redirect("/Admin/UserListing.aspx");
                }

            }
        }
        protected void btn_submit_Click(object sender, EventArgs e)
        {
            var username = tb_username.Text.Trim();
            var password = tb_password.Text.Trim();
            var userobj = UserHelper.instance;

            var res = 0;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                res = userobj.Login(username, password, 2);

                if (userobj.isAuthenticated)
                    Response.Redirect("/Admin/UserListing.aspx");
                else
                    p_message.Visible = true;
            }
            else
                p_message.Visible = true;
        }
    }
}