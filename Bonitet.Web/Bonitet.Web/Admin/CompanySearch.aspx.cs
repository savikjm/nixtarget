using Bonitet.DAL;
using Bonitet.DAL.BiznisMreza;
using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bonitet.Web.Admin
{
    public partial class CompanySearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    if (IsPostBack == true)
                    {
                    }
                }
                else
                {
                    Response.Redirect("/Admin/");
                }
            }
            else
            {
                Response.Redirect("/Admin/");
            }
        }

        public void PopulateData(string EMBS)
        {
        }

        protected void search_submit_Click(object sender, EventArgs e)
        {
            var companies = new List<Subjekt>();
            var errMsg = "";
            var EMBS = search_keyword.Value.Trim();
            if (EMBS.Length > 0)
            {
                companies = DALHelper.GetSubjektByEMBS(EMBS);

                var force = Request.QueryString["force"];

                if (!String.IsNullOrEmpty(force))
                {
                    if (Boolean.Parse(force) == true)
                    {

                        if (companies.Count() > 0)
                            Response.Redirect("/Admin/GenerateReportForce.aspx?id=" + companies[0].PK_Subjekt);
                        else
                            errMsg = "Не постои компанија со внесениот ЕМБС.";
                    }
                    else {

                        if (companies.Count() > 0)
                            Response.Redirect("/Admin/GenerateReport.aspx?id=" + companies[0].PK_Subjekt);
                        else
                            errMsg = "Не постои компанија со внесениот ЕМБС.";
                    }
                }


            }
            errMsg = "Внесете ЕМБС.";

            error_msg.Text = errMsg;
            if (errMsg.Count() > 0)
                error_msg.Visible = true;
        }
    }
}