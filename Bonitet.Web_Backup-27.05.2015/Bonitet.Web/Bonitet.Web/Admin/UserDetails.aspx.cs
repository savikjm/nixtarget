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

                        var curPage = Request.QueryString["page"];

                        if (IsPostBack == false)
                        {
                            if (curPage != null)
                                PopulateData(Convert.ToInt32(curPage), uid);
                            else
                                PopulateData(1, uid);
                        }

                        edit_user.HRef = "EditUser.aspx?id=" + user[0].ID;
                        c_username.Text = user[0].Username;
                        c_email.Text = user[0].Email;
                        c_password.Text = user[0].Password;
                        c_embs.Text = user[0].EMBS;
                        c_uid.HRef = "/Admin/CreatePrepayPack.aspx?id=" + user[0].ID;
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
                    else {

                        Response.Redirect("UserListing.aspx", true);
                    }
        }
    }
}