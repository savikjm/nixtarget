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
    public partial class ReportRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    if (IsPostBack == false)
                    {
                        var curPage = Request.QueryString["page"];
                        var filter = Request.QueryString["filter"];

                        if (filter != null && curPage != null)
                            PopulateData(Convert.ToInt32(curPage), Convert.ToInt32(filter));
                        else if (filter != null)
                        {
                            PopulateData(1, Convert.ToInt32(filter));
                        }
                        else if (curPage != null)
                        {
                            PopulateData(Convert.ToInt32(curPage), 1);
                        }
                        else
                            PopulateData(1, 1);

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

        public void PopulateData(int page, int filter)
        {
            var total = DALHelper.GetTotalReportRequests(filter);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            search_selector.SelectedIndex = filter - 1;

            if (canPage == false)
                return;

            var report_requests = DALHelper.GetRequestsByPage(skip, pageSize, filter);


            r_ReportRequests.DataSource = report_requests;
            r_ReportRequests.DataBind();


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

            var tmpF = Request.QueryString["filter"];
            int filter = 1;
            if (tmpF != null)
            {
                filter = Convert.ToInt32(tmpF);
            }

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                curPage--;

                if (curPage <= 1)
                    curPage = 1;

                ChangeUrlParamter(curPage, filter);
            }
            else
            {
                ChangeUrlParamter(1, filter);
            }

        }

        protected void NextPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];

            var tmpF = Request.QueryString["filter"];
            int filter = 1;
            if (tmpF != null)
            {
                filter = Convert.ToInt32(tmpF);
            }

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                var total = DALHelper.GetTotalReportRequests(filter);

                if (Math.Ceiling((double)total / 10) > curPage)
                    curPage++;

                ChangeUrlParamter(curPage, filter);
            }
            else
            {
                ChangeUrlParamter(2, filter);
            }

        }

        protected void ChangeUrlParamter(int page, int filter)
        {
            if (filter > 1)
                Response.Redirect("ReportRequests.aspx?page=" + page.ToString() + "&filter=" + filter, true);
            else
                Response.Redirect("ReportRequests.aspx?page=" + page.ToString(), true);
        }

        protected void search_selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = search_selector.SelectedValue;

            Response.Redirect("ReportRequests.aspx?filter=" + selected);
        }


    }
}