using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bonitet.Web.Classes;
using Bonitet.Document;
using System.Web.SessionState;
using Bonitet.DAL;
using System.Net.Mail;

namespace Bonitet.Web
{
    /// <summary>
    /// Summary description for DocumentHelper
    /// </summary>
    public class DocumentHelperC : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                var create_pdf = context.Request.Form["create_pdf"];

                var send_mail = context.Request.Form["send_mail"];

                context.Response.ContentType = "text/plain";

                if (create_pdf != null)
                {
                    var ReportType = Convert.ToInt32(context.Request.Form["type"]);
                    var CompanyID = Convert.ToInt32(context.Request.Form["companyid"]);
                    var EMBS = context.Request.Form["embs"];
                    var CurYear = 2013;


                    var check = DocumentClass.CheckDocument(CurYear, userobj.UserID, EMBS, ReportType);

                    if (check == 0)
                    {
                        var res = DocumentClass.GenerateReport(userobj.UserID, 0, EMBS, CurYear, ReportType);

                        if (res != null)
                        {
                            context.Response.Write(res);
                        }
                        else
                            context.Response.Write("error");
                    }
                    else if (check == 1)
                    {

                        var res = DocumentClass.GetReportByUserCompanyYearAndReport(userobj.UserID, EMBS, CurYear, ReportType);

                        if (res != null)
                        {
                            context.Response.Write(res.UID);
                        }
                        else
                            context.Response.Write("error");
                    }

                }
                else if (send_mail != null)
                {
                    var UserID = userobj.UserID;
                    var CompanyID = Convert.ToInt32(context.Request.Form["companyid"]);
                    var EMBS = context.Request.Form["embs"];

                    var user = DALHelper.GetUserByID(UserID);

                    if (user.Count() > 0)
                    {
                        var company = DALHelper.GetCompanyByEMBS(EMBS);

                        if (company != null)
                        {
                            MailMessage mailMessage = new MailMessage();

                            mailMessage.From = new MailAddress("stojanov1990@gmail.com", "Mite");
                            mailMessage.To.Add(new MailAddress("stojanov1990@gmail.com", "Bonitet"));

                            mailMessage.Body = "Клиентот: " + user[0].Username + " <" + user[0].Email + "> бара информации за \nКомпанија: " + company.Name + "\nЕдинствен матичен број на компанија: " + EMBS;

                            mailMessage.Subject = "Барање за краток извештај";


                            SmtpClient smtpClient = new SmtpClient();
                            smtpClient.EnableSsl = true;

                            smtpClient.Send(mailMessage);

                            context.Response.Write("Mail Sent");
                        }
                        else
                            context.Response.Write("error");
                    }
                    else
                        context.Response.Write("error");
                }
                else
                {
                    context.Response.Write("error");
                }
            }
            else
            {
                context.Response.Write("error");
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