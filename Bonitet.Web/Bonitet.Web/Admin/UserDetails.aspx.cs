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
    public partial class UserDetails : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    var id = Request.QueryString["id"];

                    if (!String.IsNullOrEmpty(id))
                    {
                        var uid = Convert.ToInt32(id);
                        var user = DALHelper.GetUserByID(uid);

                        if (IsPostBack == false)
                        {
                            var curPage = Request.QueryString["page"];

                            if (curPage != null)
                                PopulateData(Convert.ToInt32(curPage), uid);
                            else
                                PopulateData(1, uid);
                        }

                        if (user != null)
                        {
                            edit_user.HRef = "EditUser.aspx?id=" + user.ID;
                            c_username.Text = user.Username;
                            c_email.Text = user.Email;
                            c_password.Text = user.Password;
                            c_embs.Text = user.EMBS;
                            c_boniteten_izvestaj.HRef = "/Admin/CreatePrepayPack.aspx?id=" + user.ID + "&packtype=1";
                            c_kratko_izvestaj.HRef = "/Admin/CreatePrepayPack.aspx?id=" + user.ID + "&packtype=2";
                            c_blokada.HRef = "/Admin/CreatePrepayPack.aspx?id=" + user.ID + "&packtype=3";
                        }
                        else
                        {
                            Response.Redirect("/Admin/UserListing.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("/Admin/UserListing.aspx");
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

        public void PopulateData(int page, int UserID)
        {
            var total = DALHelper.GetTotalUserPrepayPacks(UserID);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            if (canPage == false)
                return;

            var user_reports = DALHelper.GetUserPrepayPacksByPage(skip, pageSize, UserID);


            r_PrepayPacks.DataSource = user_reports;
            r_PrepayPacks.DataBind();


            if (total > pageSize || page > 1)
            {
                pagination_wrapper.Visible = true;
                cur_page.Attributes["cur_page"] = page.ToString();
                cur_page.InnerText = "Page " + page + " of " + totalPages;
            }
        }

        protected void PrevPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                curPage--;

                if (curPage <= 1)
                    curPage = 1;

                ChangeUrlParamter(curPage);
            }
            else
            {
                ChangeUrlParamter(1);
            }

        }

        protected void NextPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                var total = DALHelper.GetTotalSubjektCount(null, null);

                if (Math.Ceiling((double)total / 10) > curPage)
                    curPage++;

                ChangeUrlParamter(curPage);
            }
            else
            {
                ChangeUrlParamter(2);
            }

        }

        protected void ChangeUrlParamter(int page)
        {

            var id = Request.QueryString["id"];

            if (!String.IsNullOrEmpty(id))
            {
                Response.Redirect("UserDetails.aspx?id=" + id + "&page=" + page.ToString(), true);
            }
            else
            {
                Response.Redirect("UserDetails.aspx?id=1&page=" + page.ToString(), true);
            
            }
        }
    }
}