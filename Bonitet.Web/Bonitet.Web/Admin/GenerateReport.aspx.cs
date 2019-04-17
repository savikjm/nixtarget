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
    public partial class GenerateReport : System.Web.UI.Page
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
                        var id = Request.QueryString["id"];

                        if (!String.IsNullOrEmpty(id))
                        {
                            var curCompany = DALHelper.GetCompanyByID(Convert.ToInt32(id));

                            c_CurCompanyID.Value = curCompany[0].ID.ToString();
                            c_embs.Text = curCompany[0].EMBS;
                            c_naziv.Text = curCompany[0].Naziv;
                            //c_datum.Text = curCompany[0].Datum.ToString();

                            crm_report.Visible = true;
                            if (DALHelper.CheckIfCompanyHasDataForReport(curCompany[0].EMBS, 1))
                            {
                                crm_report.Visible = true;
                            }
                        }
                        else
                        {
                            Response.Redirect("/Admin/CompanySearch.aspx?force=false");
                        }
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
    }
}