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
    public class DocumentHelper : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 1)
                {
                    var create_pdf = context.Request.Form["create_pdf"];

                    var send_mail = context.Request.Form["send_mail"];

                    var mail_type = context.Request.Form["mail_type"];

                    context.Response.ContentType = "text/plain";

                    if (create_pdf != null)
                    {
                        var ReportType = Convert.ToInt32(context.Request.Form["type"]);
                        var CompanyID = Convert.ToInt32(context.Request.Form["companyid"]);
                        var EMBS = context.Request.Form["embs"];
                        var CurYear = 2013;

                        var tmpCompany = DALHelper.GetCompanyByEMBS(EMBS);
                        var compYears = DALHelper.GetLastYearByCompanyID(tmpCompany.ID);

                        CurYear = compYears.Year;


                        //if (EMBS == "6753108" || EMBS == "4783018" || EMBS == "4056043" || EMBS == "6328849" || EMBS == "5323983" || EMBS == "6264921")
                        //    CurYear = 2014;

                        DALHelper.CreateReportRequest(userobj.UserID, 2, EMBS, CurYear);

                        // check if user report exists
                        var check = DocumentClass.CheckIfReportExists(CurYear, EMBS, ReportType, userobj.UserID);

                        if (check == 0)
                        {
                            // check user licence
                            var licence_check = DALHelper.CheckPrepayLicence(userobj.UserID);

                            if (licence_check != null)
                            {
                                // get report for user
                                var rep = DocumentClass.GetReportByUserCompanyYearAndReport(userobj.UserID, EMBS, CurYear, ReportType);

                                string res = null;
                                // check if report exists
                                if (rep == null)
                                {
                                    // generete report
                                    res = DocumentClass.GenerateReport(userobj.UserID, 0, EMBS, CurYear, ReportType, licence_check.ID);
                                }
                                else
                                {
                                    res = rep.UID.ToString();
                                }


                                if (res != null)
                                {
                                    //update prepay for user
                                    DALHelper.UpdateUserPrepay(licence_check.ID, userobj.UserID);

                                    //create user reports
                                    DALHelper.CreateUserReport(CurYear, userobj.UserID, EMBS, ReportType, licence_check.ID, res);

                                    context.Response.Write(res);
                                }
                                else
                                    context.Response.Write("Report Generator error" + res);
                            }
                            else
                                context.Response.Write("Licence error");
                        }
                        else if (check == 1)
                        {
                            // check user licence
                            var licence_check = DALHelper.CheckPrepayLicence(userobj.UserID);

                            var user_doc_check = DALHelper.CheckReportByUserCompanyYearAndReport(CurYear, EMBS, ReportType, userobj.UserID);
                            if (user_doc_check == 1)
                            {
                                // get report for user
                                var res = DocumentClass.GetReportByUserCompanyYearAndReport(userobj.UserID, EMBS, CurYear, ReportType);

                                if (res != null)
                                {
                                    context.Response.Write(res.UID);
                                }
                                else
                                    context.Response.Write("No report error");
                            }
                            else
                            {
                                if (licence_check != null)
                                {

                                    // get report for user
                                    var res = DocumentClass.GetReportByUserCompanyYearAndReport(userobj.UserID, EMBS, CurYear, ReportType);

                                    if (res != null)
                                    {
                                        context.Response.Write(res.UID);
                                    }
                                    else
                                        context.Response.Write("No report error");
                                }
                                else
                                    context.Response.Write("Licence error");
                            }
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
                                var res1 = SendRequestMail(Convert.ToInt32(mail_type), user[0], company);

                                var res2 = SendMailToClient(Convert.ToInt32(mail_type), user[0], company);

                                var curYear = 2014;


                                //if (EMBS == "6753108" || EMBS == "4783018" || EMBS == "4056043" || EMBS == "6328849" || EMBS == "5323983" || EMBS == "6264921")
                                //    curYear = 2014;
                                DALHelper.CreateReportRequest(UserID, Convert.ToInt32(mail_type), EMBS, curYear);

                                if(res1 == false || res2 == false)
                                    context.Response.Write("error");

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
                else if (userobj.Type == 2)
                {

                    var send_request_mail = context.Request.Form["send_request_mail"];
                    context.Response.ContentType = "text/plain";
                
                    if (send_request_mail != null)
                    {
                        var UserID = Convert.ToInt32(context.Request.Form["userid"]);
                        var EMBS = context.Request.Form["embs"];

                        var user = DALHelper.GetUserByID(UserID);

                        if (user.Count() > 0)
                        {
                            var comp1 = DALHelper.GetCompanyByEMBS_BM(EMBS);

                            var res = SendRequestReportMailToClient(user[0].Email, comp1.PK_Subjekt, comp1.CelosenNazivNaSubjektot);
                            if(res != false)
                                context.Response.Write("Mail Sent");
                            else
                                context.Response.Write("Error");
                        }
                    }
                }
            }
            else
            {
                context.Response.Write("error");
            }
        }

        public bool SendRequestMail(int mail_type, UserProfile user, Company company)
        {

            var Message = "Клиентот: " + user.Username + " <" + user.Email + "> бара информации за \nКомпанија: " + company.Name + "\nЕдинствен матичен број на компанија: " + company.EMBS;

            var Subject = "";

            if (mail_type == 1)
            {
                Subject = "Барање за бонитет";
            }
            else if (mail_type == 2)
            {
                Subject = "Барање за краток извештај";
            }
            try
            {
                MailHelper.SendMailToAdmin(Subject, Message, false);

                return true;
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }

        public bool SendRequestReportMailToClient(string email, int companyID, string companyName)
        {
            var link = "http://eboniteti.mk/Authenticated/CompanyDetails.aspx?id=" + companyID;

            var Message = "Почитувани, <br/><br/>Извештајот за компанијата " + companyName + " е достапен. <br/> <br/>Можете да го превземете на следниов линк: <br/><br/><a href=\"" + link + "\">" + link + "</a> <br/> <br/><br/>За останати информации контактирајте не на 02/3117-100 или boniteti@targetgroup.mk<br/><br/>Тимот на Ебонитети.мк";

            var Subject = "Нотификација за побаран извештај";

            try
            {
                MailHelper.SendMail(email, Subject, Message, true);

                return true;
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }

        public bool SendMailToClient(int mail_type, UserProfile user, Company company)
        {
            var Message = "";
            var Subject = "";

            if (mail_type == 1)
            {
                Message = "Почитувани, <br/><br/>Ви благодариме за нарачката на бонитетниот извештај. <br/><u>Податоци за нарачката:</u><br/>Компанија: " + company.Name + "<br/>ЕМБС: " + company.EMBS + "<br/><br/>Извештајот ќе Ви биде доставен најдоцна во рок од 2 (два) работни дена по извршената нарачка. <br/>За останати информации контактирајте не на 02/3117-100 или boniteti@targetgroup.mk<br/><br/>Тимот на Ебонитети.мк";

                Subject = "Барање за бонитетен извештај";

                try
                {
                    MailHelper.SendMail(user.Email, Subject, Message, true);

                    return true;
                }
                catch (Exception ex)
                {
                    var a = ex;
                    return false;
                }
            }
            else if (mail_type == 2)
                return true;
            return false;
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