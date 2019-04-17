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
    public partial class CreatePrepayPack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    var id = Request.QueryString["id"];
                    var packtype = Request.QueryString["packtype"];

                    if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(packtype))
                    {
                        var uid = Convert.ToInt32(id);
                        var PackType = Convert.ToInt32(packtype);

                        Back_UserDetails.HRef = "/Admin/UserDetails.aspx?id=" + uid;
                        error_text.Visible = false;
                        if (IsPostBack == true)
                            CreatePack(uid, PackType);
                    }
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

        public void CreatePack(int uid, int PackType)
        {
            DateTime DateStart = new DateTime();
            DateTime DateEnd = new DateTime();
            string Pack = "";
            string Comment = "";

            var IsPostPaid = post_paid.Checked;

            var error = false;
            if (start_date.Text.Length > 0 && end_date.Text.Length > 0 && pack.Text.Length > 0)
            {

                Pack = pack.Text;
                Comment = comment.Text;

                if (!DateTime.TryParse(start_date.Text, out DateStart))
                {
                    error_text.InnerText = "Датумите се со неправилен формат!";
                    error = true;
                }

                if (!DateTime.TryParse(end_date.Text, out DateEnd))
                {
                    error_text.InnerText = "Датумите се со неправилен формат!";
                    error = true;
                }
            }
            else
            {
                error_text.InnerText = "Пополнете ги сите задолжителни полиња!";
                error = true;
            }
            if (error == false)
            {
                if (DALHelper.CreatePrepayPack(uid, Convert.ToDateTime(DateStart), Convert.ToDateTime(DateEnd), Convert.ToInt32(Pack), Comment, PackType, IsPostPaid))
                    Response.Redirect("/Admin/UserDetails.aspx?id=" + uid);
                else
                {
                    error_text.InnerText = "Грешка при креирање!";
                    error_text.Visible = true;
                }
            }
            else
            {
                error_text.Visible = true;
            }

        }
    }
}