using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bonitet.DAL;
using Bonitet.DAL.BiznisMreza;
using Bonitet.Web.Classes;

namespace Bonitet.Web.Authenticated
{
    public partial class CompanyListing : System.Web.UI.Page
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

                        pagination_wrapper.Visible = false;
                        var curPage = Request.QueryString["page"];
                        var EMBS = Request.QueryString["embs"];
                        var name = Request.QueryString["name"];
                        var sediste = Request.QueryString["sediste"];


                        if (EMBS != null)
                            PopulateData(1, EMBS.ToString(), null, null);
                        else if(name != null) {
                            if (name != null && curPage != null)
                                PopulateData(Convert.ToInt32(curPage), null, name.ToString(), null);
                            else if (name != null)
                                PopulateData(1, null, name.ToString(), null);
                        }
                        else if (sediste != null)
                        {
                            if (sediste != null && curPage != null)
                                PopulateData(Convert.ToInt32(curPage), null, null, sediste.ToString());
                            else if (sediste != null)
                                PopulateData(1, null, null, sediste.ToString());
                        }
                    }
                }
            }
        }

        public void PopulateData(int page, string EMBS, string name, string sediste)
        {
            var total = DALHelper.GetTotalSubjektCount(name, sediste);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            if (canPage == false)
                return;

            var companies = new List<Subjekt>();
            if (EMBS != null)
            {
                companies = DALHelper.GetSubjektByEMBS(EMBS);
                total = companies.Count();

                totalPages = 1;

                canPage = false;
            }
            else if (name != null)
            {
                companies = DALHelper.GetSubjektByName(skip, pageSize, name);

            }
            else if (sediste != null)
            {
                companies = DALHelper.GetSubjektBySediste(skip, pageSize, sediste);

            }
            else
                companies = DALHelper.GetSubjektsByPage(skip, pageSize);

            r_Companies.DataSource = companies;
            r_Companies.DataBind();


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

            var sediste = Request.QueryString["sediste"];

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                curPage--;

                if (curPage <= 1)
                    curPage = 1;

                ChangeUrlParamter(curPage, name, sediste);
            }
            else
            {
                ChangeUrlParamter(1, name, sediste);
            }

        }

        protected void NextPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];

            var name = Request.QueryString["name"];

            var sediste = Request.QueryString["sediste"];


            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                var total = DALHelper.GetTotalSubjektCount(name, sediste);

                if (Math.Ceiling((double)total / 10) > curPage)
                    curPage++;

                ChangeUrlParamter(curPage, name, sediste);
            }
            else
            {
                ChangeUrlParamter(2, name, sediste);
            }

        }

        protected void ChangeUrlParamter(int page, string name, string sediste)
        {
            if (name != null)
                Response.Redirect("CompanyListing.aspx?page=" + page.ToString() + "&name=" + name, true);
            else if (sediste != null)
                Response.Redirect("CompanyListing.aspx?page=" + page.ToString() + "&sediste=" + sediste, true);
            else
                Response.Redirect("CompanyListing.aspx?page=" + page.ToString(), true);
        }

        protected void search_submit_Click(object sender, EventArgs e)
        {
            var type = search_selector.Value.ToString();

            if (search_keyword.Value.Length > 0)
            {
                Response.Redirect("CompanyListing.aspx?" + type + "=" + search_keyword.Value, true);
            }
            else
                Response.Redirect("CompanyListing.aspx", true);
        }
    }
}