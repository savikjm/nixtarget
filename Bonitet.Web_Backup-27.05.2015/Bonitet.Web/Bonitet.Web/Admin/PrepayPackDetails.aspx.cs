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
    public partial class PrepayPackDetails : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    var packid = Request.QueryString["packid"];
                    var userid = Request.QueryString["userid"];

                    if (!String.IsNullOrEmpty(packid) && !String.IsNullOrEmpty(userid))
                    {
                        var UUserID = Convert.ToInt32(userid);
                        var UPackID = Convert.ToInt32(packid);

                        Back_UserDetails.HRef = "/Admin/UserDetails.aspx?id=" + UUserID;

                        if (IsPostBack == false)
                        {
                            var curPage = Request.QueryString["page"];

                            if (curPage != null)
                                PopulateData(Convert.ToInt32(curPage), UUserID, UPackID);
                            else
                                PopulateData(1, UUserID, UPackID);
                        }
                    }
                    else
                    {
                        Response.Redirect("/Admin/UserDetails.aspx");
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

        public void PopulateData(int page, int UserID, int PackID)
        {
            var total = DALHelper.GetTotalUserReportsByPackID(UserID, PackID);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            if (canPage == false)
                return;

            var user_reports = DALHelper.GetUserReportsByPackIDByPage(skip, pageSize, UserID, PackID);


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
            Response.Redirect("UserDetails.aspx?page=" + page.ToString(), true);
        }
    }
}