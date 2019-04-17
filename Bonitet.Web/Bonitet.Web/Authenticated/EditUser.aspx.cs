using Bonitet.DAL;
using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.Authenticated
{
    public partial class EditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 1)
                {
                    var id = Request.QueryString["id"];

                    if (!String.IsNullOrEmpty(id))
                    {
                        error_text.Visible = false;

                        var uid = Convert.ToInt32(id);
                        var user = DALHelper.GetUserByID(uid);

                        if (user != null && IsPostBack == false)
                        {
                            c_username.Text = user.Username;
                            c_email.Text = user.Email;
                            c_password.Text = user.Password;
                        }
                    }
                    else
                    {
                        Response.Redirect("/Default.aspx");
                    }
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }
            }
            else
            {
                Response.Redirect("/Default.aspx");
            }
        }
        protected void save_user_Click(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];

            if (!String.IsNullOrEmpty(id))
            {
                var uid = Convert.ToInt32(id);
                var user = DALHelper.GetUserByID(uid);
                if(user != null)
                {
                    if (c_username.Text.Length > 0 && c_password.Text.Length > 0 && c_email.Text.Length > 0)
                    {
                        var username = c_username.Text;
                        var password = c_password.Text;
                        var email = c_email.Text;

                        var userobj = new User 
                        {
                            ID = user.ID,
                            Username = username,
                            Password = password,
                            Email = email,
                            EMBS = user.EMBS
                        };

                        var res = DALHelper.UpdateUser(userobj);

                        if (res == true)
                            Response.Redirect("/Authenticated/UserProfile.aspx?id=" + userobj.ID);
                        else
                            error_text.Visible = true;
                    }
                    else
                        error_text.Visible = true;
                }
                else
                    error_text.Visible = true;
            }
        }

    }
}