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
                        var EMBS = Request.QueryString["embs"];
                        var name = Request.QueryString["name"];
                        var tip_izvestaj = Request.QueryString["tip_izvestaj"];
                        var sort = Request.QueryString["sort"];

                        if (sort == null || sort.Length == 0)
                            sort = "asc-DateCreated";

                        if (curPage == null)
                            curPage = "1";

                        if (EMBS != null)
                            PopulateData(Convert.ToInt32(curPage), userobj.UserID, EMBS.ToString(), null, null, sort);
                        else if (name != null)
                        {
                            if (name != null && curPage != null)
                                PopulateData(Convert.ToInt32(curPage), userobj.UserID, null, name.ToString(), null, sort);
                            else if (name != null)
                                PopulateData(Convert.ToInt32(curPage), userobj.UserID, null, name.ToString(), null, sort);
                        }
                        else if (tip_izvestaj != null)
                        {
                            if (tip_izvestaj != null && curPage != null)
                                PopulateData(Convert.ToInt32(curPage), userobj.UserID, null, null, tip_izvestaj.ToString(), sort);
                            else if (tip_izvestaj != null)
                                PopulateData(Convert.ToInt32(curPage), userobj.UserID, null, null, tip_izvestaj.ToString(), sort);
                        }
                        else
                            PopulateData(Convert.ToInt32(curPage), userobj.UserID, null, null, null, sort);
                    }

                    var curUser = DALHelper.GetUserByID(userobj.UserID);

                    if (curUser != null)
                    {
                        p_email.Text = curUser.Email;
                        p_username.Text = curUser.Username;
                    }

                    edit_user.HRef = "/Authenticated/EditUser.aspx?id=" + userobj.UserID;
                }
            }
        }

        public void PopulateData(int page, int UserID, string EMBS, string name, string tip_izvestaj, string sort)
        {
            var sort_type = sort.Split('-');
            var new_sort = "asc-" + sort_type[1];
            var new_class = "down";
            if (sort_type[0] == "asc")
            {
                new_sort = "desc-" + sort_type[1];
                new_class = "up";
            }

            switch (sort_type[1]) { 
                case "EMBS":
                    sort_embs.CommandArgument = new_sort;
                    sort_embs.CssClass = new_class;
                    break;
                case "CompanyName":
                    sort_naziv.CommandArgument = new_sort;
                    sort_naziv.CssClass = new_class;
                    break;
                case "DateCreated":
                    sort_datum.CommandArgument = new_sort;
                    sort_datum.CssClass = new_class;
                    break;
                case "Downloads":
                    sort_download.CommandArgument = new_sort;
                    sort_download.CssClass = new_class;
                    break;
                case "PackTypeName":
                    sort_tip.CommandArgument = new_sort;
                    sort_tip.CssClass = new_class;
                    break;
            }

            var total = DALHelper.GetTotalUserReports1(UserID, name, tip_izvestaj);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            if (canPage == false)
                return;

            var user_reports = new List<c_UserReportObj>();
            if (EMBS != null)
            {
                pagination_wrapper.Visible = false;

                user_reports = DALHelper.GetUserReportByEMBS(UserID, EMBS);
                total = user_reports.Count();

                totalPages = 1;

                canPage = false;
            }
            else if (name != null)
            {
                user_reports = DALHelper.GetUserReportByName(UserID, skip, pageSize, name, sort);
            }
            else if (tip_izvestaj != null)
            {
                user_reports = DALHelper.GetUserReportByTip(UserID, skip, pageSize, tip_izvestaj, sort);

            }
            else
                user_reports = DALHelper.GetUserReportsByPage(skip, pageSize, UserID, sort);


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

            var name = Request.QueryString["name"];

            var tip_izvestaj = Request.QueryString["tip_izvestaj"];

            var sort = Request.QueryString["sort"];

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                curPage--;

                if (curPage <= 1)
                    curPage = 1;

                ChangeUrlParamter(curPage, name, tip_izvestaj, sort);
            }
            else
            {
                ChangeUrlParamter(1, name, tip_izvestaj, sort);
            }

        }

        protected void NextPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];

            var name = Request.QueryString["name"];

            var tip_izvestaj = Request.QueryString["tip_izvestaj"];

            var sort = Request.QueryString["sort"];

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                var total = DALHelper.GetTotalUserReports1(UserHelper.instance.UserID, name, tip_izvestaj);

                if (Math.Ceiling((double)total / 10) > curPage)
                    curPage++;

                ChangeUrlParamter(curPage, name, tip_izvestaj, sort);
            }
            else
            {
                ChangeUrlParamter(2, name, tip_izvestaj, sort);
            }

        }

        protected void ChangeUrlParamter(int page, string name, string tip_izvestaj, string sort)
        {
            if (name != null)
                Response.Redirect("UserProfile.aspx?page=" + page.ToString() + "&name=" + name + "&sort=" + sort, true);
            else if (tip_izvestaj != null)
                Response.Redirect("UserProfile.aspx?page=" + page.ToString() + "&tip_izvestaj=" + tip_izvestaj + "&sort=" + sort, true);
            else
                Response.Redirect("UserProfile.aspx?page=" + page.ToString() + "&sort=" + sort, true);
        }

        protected void search_submit_Click(object sender, EventArgs e)
        {
            var type = search_selector.Value.ToString();

            if (search_keyword.Value.Length > 0)
            {
                Response.Redirect("UserProfile.aspx?" + type + "=" + search_keyword.Value, true);
            }
            else
                Response.Redirect("UserProfile.aspx", true);
        }

        protected void sort_embs_Click(object sender, EventArgs e)
        {
            var curPage = Request.QueryString["page"];
            var EMBS = Request.QueryString["embs"];
            var name = Request.QueryString["name"];
            var tip_izvestaj = Request.QueryString["tip_izvestaj"];
            var sort = sort_embs.CommandArgument;

            Response.Redirect(CreateUrl(curPage, EMBS, name, tip_izvestaj, sort), true);
        }

        protected void sort_naziv_Click(object sender, EventArgs e)
        {
            var curPage = Request.QueryString["page"];
            var EMBS = Request.QueryString["embs"];
            var name = Request.QueryString["name"];
            var tip_izvestaj = Request.QueryString["tip_izvestaj"];
            var sort = sort_naziv.CommandArgument;

            Response.Redirect(CreateUrl(curPage, EMBS, name, tip_izvestaj, sort), true);
        }

        protected void sort_tip_Click(object sender, EventArgs e)
        {
            var curPage = Request.QueryString["page"];
            var EMBS = Request.QueryString["embs"];
            var name = Request.QueryString["name"];
            var tip_izvestaj = Request.QueryString["tip_izvestaj"];
            var sort = sort_tip.CommandArgument;

            Response.Redirect(CreateUrl(curPage, EMBS, name, tip_izvestaj, sort), true);
        }

        protected void sort_download_Click(object sender, EventArgs e)
        {
            var curPage = Request.QueryString["page"];
            var EMBS = Request.QueryString["embs"];
            var name = Request.QueryString["name"];
            var tip_izvestaj = Request.QueryString["tip_izvestaj"];
            var sort = sort_download.CommandArgument;

            Response.Redirect(CreateUrl(curPage, EMBS, name, tip_izvestaj, sort), true);
        }

        protected void sort_datum_Click(object sender, EventArgs e)
        {
            var curPage = Request.QueryString["page"];
            var EMBS = Request.QueryString["embs"];
            var name = Request.QueryString["name"];
            var tip_izvestaj = Request.QueryString["tip_izvestaj"];
            var sort = sort_datum.CommandArgument;

            Response.Redirect(CreateUrl(curPage, EMBS, name, tip_izvestaj, sort), true);
        }

        public string CreateUrl(string curPage, string EMBS, string name, string tip_izvestaj, string sort)
        {
            if (sort == null || sort.Length == 0)
                sort = "asc-DateCreated";
            if(curPage == null)
                curPage = "1";


            var url = "";
            if (EMBS != null)
                url = "UserProfile.aspx?page=" + curPage + "&embs=" + EMBS + "&sort=" + sort;
            else if (name != null)
            { 
                url = "UserProfile.aspx?page=" + curPage + "&name=" + name + "&sort=" + sort;
            }
            else if (tip_izvestaj != null)
            {
                url = "UserProfile.aspx?page=" + curPage + "&tip_izvestaj=" + tip_izvestaj + "&sort=" + sort;
            }
            else
                url = "UserProfile.aspx?page=" + curPage + "&sort=" + sort;

            return url;
        }
    }
}