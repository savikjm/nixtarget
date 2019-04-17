using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml.Linq;
using Bonitet.Web.Classes;
using System.Text;
using iTextSharp.text.pdf.parser;
using Novacode;
using TuesPechkin;
using System.Drawing.Printing;
using Bonitet.Web;
using Bonitet.DAL;

namespace Bonitet.Web.Authenticated
{
    public partial class CompanyDetails : System.Web.UI.Page
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
                        var id = Request.QueryString["id"];

                        if (!String.IsNullOrEmpty(id))
                        {
                            var curCompany = DALHelper.GetCompanyByID(Convert.ToInt32(id));

                            c_CurCompanyID.Value = curCompany[0].ID.ToString();
                            c_embs.Text = curCompany[0].EMBS;
                            c_naziv.Text = curCompany[0].Naziv;
                            //c_datum.Text = curCompany[0].Datum.ToString();

                            email_for_report.Visible = false;
                            short_report.Visible = false;

                            crm_report.Visible = false;
                            email_for_crm_report.Visible = true;

                            if (DALHelper.CheckIfCompanyHasDataForReport(curCompany[0].EMBS, 2))
                            {
                                short_report.Visible = true;
                            }
                            else
                            {
                                email_for_report.Visible = true;
                            }

                            if (DALHelper.CheckIfCompanyHasDataForReport(curCompany[0].EMBS, 1))
                            {
                                crm_report.Visible = false;
                            }
                            else
                            {
                                email_for_crm_report.Visible = true;
                            }
                        }
                        else
                        {
                            Response.Redirect("/Authenticated/CompanyListing.aspx");
                        }
                    }
                }

            }

        }
    }
}