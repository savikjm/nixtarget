using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bonitet.Web.Classes;
using Bonitet.DAL;
using Bonitet.Document;
using System.Web.SessionState;

namespace Bonitet.Web
{
    /// <summary>
    /// Summary description for Document
    /// </summary>
    public class Document : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            //check if user authenticated
           
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 1 || userobj.Type == 2)
                {
                    var qs = context.Request.QueryString["uid"];

                    if (!String.IsNullOrEmpty(qs))
                    {
                        var uid = new Guid(qs);
                        //get file
                        var doc = DocumentClass.GetReportByUID(uid);

                        //check file status
                        //if not file available write msgs
                        if (doc == null)
                        {
                            context.Response.ContentType = "text/plain";
                            context.Response.Write("Document does not exist!");
                        }

                        //update user reports
                        DALHelper.UpdateUserReport(userobj.UserID, uid);

                        //insert into log
                        DALHelper.SaveDownloadInLog(uid);

                        var res1 = DALHelper.GetCompanyByID(doc.CompanyID);

                        var name = "";
                        if (res1.Count() == 0)
                        {
                            var res2 = DALHelper.GetCompanyByID1(doc.CompanyID);
                            name = res2.Name;
                        }
                        else
                        {
                            if(string.IsNullOrEmpty(res1[0].KratokNaziv) == false)
                                name = res1[0].KratokNaziv;
                            else 
                                name = res1[0].Naziv;

                        }

                        name = name.Replace(" ", "_").Replace(",", "_");
                        var new_name = System.Net.WebUtility.HtmlDecode(name);
                        //stream file
                        context.Response.AddHeader("content-disposition", "attachment; filename=" + new_name + "_" + doc.Year + ".pdf");
                        context.Response.ContentType = "application/pdf";
                        context.Response.WriteFile(doc.Path);
                    }
                }

            }
            //if not write msg
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("You must be logged in to access this page!");
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}