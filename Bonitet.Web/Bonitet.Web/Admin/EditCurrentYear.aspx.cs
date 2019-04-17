using Bonitet.DAL;
using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.Admin
{
    public partial class EditCurrentYear : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    error_text.Visible = false;
                    c_year.Text = DALHelper.GetCurrentYear(false).ToString();
                    c_year.ReadOnly = true;
                    save_user.Visible = false;
                }
                else
                {
                    Response.Redirect("/Admin/Default.aspx");
                }
            }
            else
            {
                Response.Redirect("/Admin/Default.aspx");
            }
        }
        protected void save_user_Click(object sender, EventArgs e)
        {
            if (c_year.Text.Length > 0)
            {
                var year = c_year.Text;

                int cur_year = 0;

                if(Int32.TryParse(year, out cur_year))
                { 
                    var res = DALHelper.UpdateGlobalYear(cur_year);

                    if (res == true)
                        Response.Redirect("/Admin/Default.aspx");
                    else
                        error_text.Visible = true;
                }
                error_text.Visible = true;
            }
            else
                error_text.Visible = true;
        }
    }
}