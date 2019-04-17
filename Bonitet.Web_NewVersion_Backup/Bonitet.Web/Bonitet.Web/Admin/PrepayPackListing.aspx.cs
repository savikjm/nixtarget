﻿using Bonitet.DAL;
using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.Admin
{
    public partial class PrepayPackListing : System.Web.UI.Page
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
                        var EMBS = Request.QueryString["embs"];
                        var name = Request.QueryString["name"];

                        if (EMBS != null)
                            PopulateData(1, EMBS.ToString(), null);
                        else if (name != null)
                        {
                            if (name != null && curPage != null)
                                PopulateData(Convert.ToInt32(curPage), null, name.ToString());
                            else if (name != null)
                                PopulateData(1, null, name.ToString());
                        }
                        else
                        {
                            if (curPage != null)
                                PopulateData(Convert.ToInt32(curPage), null, null);
                            else
                                PopulateData(1, null, null);
                        }
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

        public void PopulateData(int page, string EMBS, string name)
        {
            var total = DALHelper.GetTotalPrepayPacks(name);

            var pageSize = 10;

            var skip = pageSize * (page - 1);

            var totalPages = Math.Ceiling((decimal)total / (decimal)pageSize);

            var canPage = skip < total;

            pagination_wrapper.Visible = false;

            if (canPage == false)
                return;

            var user_reports = new List<c_PrepayPackObj>();
            if (EMBS != null)
            {
                user_reports = DALHelper.GetPrepayPackByUserEMBS(EMBS);
                total = user_reports.Count();

                totalPages = 1;

                canPage = false;
            }
            else if (name != null)
            {
                user_reports = DALHelper.GetPrepayPackByUsername(skip, pageSize, name);

            }
            else
                user_reports = DALHelper.GetPrepayPacksByPage(skip, pageSize);



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

            var name = Request.QueryString["name"];

            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                curPage--;

                if (curPage <= 1)
                    curPage = 1;

                ChangeUrlParamter(curPage, name);
            }
            else
            {
                ChangeUrlParamter(1, name);
            }
        }

        protected void NextPageBtn_Click(object sender, EventArgs e)
        {
            var tmpPage = Request.QueryString["page"];


            var name = Request.QueryString["name"];


            if (tmpPage != null)
            {
                var curPage = Convert.ToInt32(tmpPage);

                var total = DALHelper.GetTotalPrepayPacks(name);

                if (Math.Ceiling((double)total / 10) > curPage)
                    curPage++;

                ChangeUrlParamter(curPage, name);
            }
            else
            {
                ChangeUrlParamter(2, name);
            }

        }

        protected void ChangeUrlParamter(int page, string name)
        {
            if (name != null)
                Response.Redirect("PrepayPackListing.aspx?page=" + page.ToString() + "&name=" + name, true);
            else
                Response.Redirect("PrepayPackListing.aspx?page=" + page.ToString(), true);
        }

        protected void search_submit_Click(object sender, EventArgs e)
        {
            var type = search_selector.Value.ToString();

            if (search_keyword.Value.Length > 0)
            {
                Response.Redirect("PrepayPackListing.aspx?" + type + "=" + search_keyword.Value, true);
            }
            else
                Response.Redirect("PrepayPackListing.aspx", true);
        }
    }
}