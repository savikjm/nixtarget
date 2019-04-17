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
    public partial class PrepayPackEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    var id = Request.QueryString["userid"];
                    var packid = Request.QueryString["packid"];

                    if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(packid))
                    {
                        var uid = Convert.ToInt32(id);
                        var upackid = Convert.ToInt32(packid);

                        Back_UserDetails.HRef = "/Admin/UserDetails.aspx?id=" + uid;
                        error_text.Visible = false;
                        if (IsPostBack == false)
                            PopulateData(upackid, uid);
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

        public void PopulateData(int upackid, int uid)
        {
            var prepay_pack = DALHelper.GetPrepayPackByID(upackid, uid);

            if (prepay_pack != null)
            {
                start_date.Text = HelperFunctions.ConvertDateTimeString(prepay_pack.DateStart);
                end_date.Text = HelperFunctions.ConvertDateTimeString(prepay_pack.DateEnd);
                pack.Text = prepay_pack.Pack.ToString();
                comment.Text = prepay_pack.Comment;
            }
            else
            {
                Response.Redirect("/Admin/Default.aspx");
            }
        }


        protected void submit_buttn_Click(object sender, EventArgs e)
        {
            var id = Request.QueryString["userid"];
            var packid = Request.QueryString["packid"];

            var error = false;

            if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(packid))
            {
                var uid = Convert.ToInt32(id);
                var upackid = Convert.ToInt32(packid);

                DateTime DateStart = new DateTime();
                DateTime DateEnd = new DateTime();
                string Pack = "";
                string Comment = "";

                if (start_date.Text.Length > 0 && end_date.Text.Length > 0 && pack.Text.Length > 0)
                {
                    var prepay_pack = DALHelper.GetPrepayPackByID(upackid, uid);

                    if (prepay_pack != null)
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
                        error_text.InnerText = "Не постои пакетот!";
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
                    if (DALHelper.UpdatePrepayPack(upackid, uid, Convert.ToDateTime(DateStart), Convert.ToDateTime(DateEnd), Convert.ToInt32(Pack), Comment))
                        Response.Redirect("/Admin/UserDetails.aspx?id=" + uid);
                    else
                    {
                        error_text.InnerText = "Грешка при зачувување!";
                        error_text.Visible = true;
                    }
                }
                else
                {
                    error_text.Visible = true;
                }
            }
            else
            {
                error_text.InnerText = "Не постои пакетот!";
                error = true;
            }
        }
    }
}