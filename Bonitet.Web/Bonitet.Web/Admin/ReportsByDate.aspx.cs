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
    public partial class ReportsByDate : System.Web.UI.Page
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
                        var embs = Request.QueryString["embs"];
                        var start = Request.QueryString["start"];
                        var end = Request.QueryString["end"];
                        var type_selector = Request.QueryString["type_selector"];

                        if (string.IsNullOrEmpty(type_selector))
                            type_selector = "-1";

                        if (curPage == null || curPage.Length == 0)
                            curPage = "1";

                        PopulateData(Convert.ToInt32(curPage), embs, start, end, type_selector);
                    }
                    else
                    {
                        var start = start_date1.Text;
                        var end = end_date1.Text;

                        DateTime StartDate = DateTime.Now;
                        DateTime EndDate = DateTime.Now;

                        if (DateTime.TryParse(start, out StartDate) == false || DateTime.TryParse(end, out EndDate) == false)
                           error_text.Visible = true;

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

        public void PopulateData(int page, string EMBS, string sDate, string eDate, string type_selector)
        {
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            if (string.IsNullOrEmpty(sDate) == false)
                DateTime.TryParse(sDate, out StartDate);

            if (string.IsNullOrEmpty(eDate) == false)
                DateTime.TryParse(eDate, out EndDate);

            if (string.IsNullOrEmpty(EMBS) == false)
                c_embs.Text = EMBS;

            type_selector1.SelectedValue = type_selector;

            c_sDate.Value = StartDate.ToShortDateString();
            c_eDate.Value = EndDate.ToShortDateString();


            var total = DALHelper.GetTotalClientReports(EMBS, StartDate, EndDate, type_selector);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            if (canPage == false)
                return;

            var res = DALHelper.GetClientsReports(1, skip, pageSize, EMBS, StartDate, EndDate, type_selector);


            r_ReportByDate.DataSource = res;
            r_ReportByDate.DataBind();


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
            var embs = Request.QueryString["embs"];
            var start = Request.QueryString["start"];
            var end = Request.QueryString["end"];
            var type_selector = Request.QueryString["type_selector"];

            if (string.IsNullOrEmpty(type_selector))
                type_selector = "-1";

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            if (string.IsNullOrEmpty(start) == false)
                DateTime.TryParse(start, out StartDate);

            if (string.IsNullOrEmpty(end) == false)
                DateTime.TryParse(end, out EndDate);


            if (string.IsNullOrEmpty(tmpPage) == false)
            {
                var curPage = Convert.ToInt32(tmpPage);

                curPage--;

                if (curPage <= 1)
                    curPage = 1;

                ChangeUrlParamter(curPage, embs, StartDate.ToString(), EndDate.ToString(), type_selector);
            }
            else
            {
                ChangeUrlParamter(1, embs, StartDate.ToString(), EndDate.ToString(), type_selector);
            }
        }

        protected void NextPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];
            var embs = Request.QueryString["embs"];
            var start = Request.QueryString["start"];
            var end = Request.QueryString["end"];
            var type_selector = Request.QueryString["type_selector"];

            if (string.IsNullOrEmpty(type_selector))
                type_selector = "-1";

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            if (string.IsNullOrEmpty(start) == false)
                DateTime.TryParse(start, out StartDate);

            if (string.IsNullOrEmpty(end) == false)
                DateTime.TryParse(end, out EndDate);


            if (string.IsNullOrEmpty(tmpPage) == false)
            {
                var curPage = Convert.ToInt32(tmpPage);

                var total = DALHelper.GetTotalClientReports(embs, StartDate, EndDate, type_selector);

                if (Math.Ceiling((double)total / 10) > curPage)
                    curPage++;

                ChangeUrlParamter(curPage, embs, StartDate.ToString(), EndDate.ToString(), type_selector);
            }
            else
            {
                ChangeUrlParamter(2, embs, StartDate.ToString(), EndDate.ToString(), type_selector);
            }

        }

        protected void ChangeUrlParamter(int page, string embs, string start, string end, string type_selector)
        {
            if (string.IsNullOrEmpty(embs) == false)
                Response.Redirect("ReportsByDate.aspx?page=" + page.ToString() + "&embs=" + embs + "&start=" + start + "&end=" + end + "&type_selector=" + type_selector, true);
            else
                Response.Redirect("ReportsByDate.aspx?page=" + page.ToString() + "&start=" + start + "&end=" + end + "&type_selector=" + type_selector, true);
        }

        protected void search_submit_Click(object sender, EventArgs e)
        {
            var embs = c_embs.Text;
            var sDate = start_date1.Text;
            var eDate = end_date1.Text;
            var type = type_selector1.SelectedItem.Value;

            if (string.IsNullOrEmpty(embs) == false)
                Response.Redirect("ReportsByDate.aspx?embs=" + embs + "&start=" + sDate + "&end=" + eDate + "&type_selector=" + type, true);
            else
                Response.Redirect("ReportsByDate.aspx?start=" + sDate + "&end=" + eDate + "&type_selector=" + type, true);

        }
    }
}