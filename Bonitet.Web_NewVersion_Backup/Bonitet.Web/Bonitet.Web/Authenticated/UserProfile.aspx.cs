using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bonitet.DAL;

namespace Bonitet.Web.Authenticated
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 1)
                {
                    if (IsPostBack == false)
                    {
                        var curPage = Request.QueryString["page"];

                        if (curPage != null)
                            PopulateData(Convert.ToInt32(curPage), userobj.UserID);
                        else
                            PopulateData(1, userobj.UserID);
                    }

                    var curUser = DALHelper.GetUserByID(userobj.UserID);

                    p_email.Text = curUser[0].Email;
                    p_username.Text = curUser[0].Username;

                    edit_user.HRef = "/Authenticated/EditUser.aspx?id=" + userobj.UserID;
                }
            }
        }

        public void PopulateData(int page, int UserID)
        {
            var total = DALHelper.GetTotalUserReports(UserID);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            if (canPage == false)
                return;

            var user_reports = DALHelper.GetUserReportsByPage(skip, pageSize, UserID);


            r_UserReports.DataSource = user_reports;
            r_UserReports.DataBind();


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
            Response.Redirect("UserProfile.aspx?page=" + page.ToString(), true);
        }
    }
}