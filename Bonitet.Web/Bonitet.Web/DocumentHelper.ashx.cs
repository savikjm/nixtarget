using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bonitet.Web.Classes;
using Bonitet.Document;
using System.Web.SessionState;
using Bonitet.DAL;
using System.Net.Mail;
using Newtonsoft.Json;

namespace Bonitet.Web
{
    /// <summary>
    /// Summary description for DocumentHelper
    /// </summary>
    public class DocumentHelper : IHttpHandler, IRequiresSessionState
    {

        public bool isTesting = true;

        public void ProcessRequest(HttpContext context)
        {
            var userobj = UserHelper.instance;

            var import_from_file = context.Request.Form["import_from_file"];
            var import_type = context.Request.Form["import_type"];
            if (import_from_file != null && import_type != null)
            {
                try
                {
                    Dictionary<string, string>[] values = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(import_from_file);

                    if (import_type == "1")
                    {
                        CRM_DocumentClass.SaveImportCompanyValues(values);
                    }
                    else
                    {
                        CRM_DocumentClass.SaveImportReportValues(values);
                    }
                    context.Response.Write("OK");
                }
                catch (Exception ex)
                {
                    context.Response.Write("Error");
                }
                return;
            }

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 1 || userobj.Type == 2)
                {
                    var create_pdf = context.Request.Form["create_pdf"];

                    var send_mail = context.Request.Form["send_mail"];

                    var mail_type = context.Request.Form["mail_type"];

                    var userTicket = context.Request.Form["userTicket"];

                    var clientEmbs = context.Request.Form["clientEmbs"];

                    var send_request_mail = context.Request.Form["send_request_mail"];

                    var no_data = context.Request.Form["no_data"];

                    var request_id = context.Request.Form["request_id"];

                    var delete_from_list = context.Request.Form["delete_from_list"];

                    var send_charge_mail = context.Request.Form["send_charge_mail"];



                    context.Response.ContentType = "text/plain";

                    var reqID = 0;
                    if (request_id != null)
                    {
                        reqID = Convert.ToInt32(request_id);
                    }

                    if (send_request_mail != null)
                    {
                        var UserID = Convert.ToInt32(context.Request.Form["userid"]);
                        var EMBS = context.Request.Form["embs"];

                        var user = DALHelper.GetUserByID(UserID);

                        if (user != null)
                        {
                            var comp1 = DALHelper.GetCompanyByEMBS_BM(EMBS);
                            var res = SendRequestReportMailToClient(user.Email, comp1.PK_Subjekt, comp1.CelosenNazivNaSubjektot, reqID, no_data);
                            if (res != false)
                                context.Response.Write("Mail Sent");
                            else
                                context.Response.Write("Error");
                        }
                    }
                    else if (delete_from_list != null)
                    {
                        DALHelper.UpdateReportRequestFunc(reqID, DALHelper.ReportRequestActions.Delete);

                        context.Response.Write("Deleted");

                    }
                    else if (send_charge_mail != null)
                    {

                        var UserID = Convert.ToInt32(context.Request.Form["userid"]);
                        var EMBS = context.Request.Form["embs"];

                        var user = DALHelper.GetUserByID(UserID);

                        if (user != null)
                        {
                            var comp1 = DALHelper.GetCompanyByEMBS_BM(EMBS);
                            var ReportType = 1;
                            var licence_check = DALHelper.CheckPrepayLicence(user.ID, ReportType, 1);

                            if (licence_check != null)
                            {
                                DALHelper.UpdateReportRequestFunc(reqID, DALHelper.ReportRequestActions.Paid);
                                var res = SendChargeMailToClient(licence_check.ID, user.Email, user.ID, comp1.CelosenNazivNaSubjektot, ReportType);
                                if (res != false)
                                {
                                    context.Response.Write("Mail Sent");
                                }
                                else
                                    context.Response.Write("Error");
                            }
                            else
                                context.Response.Write("Error");
                        }
                    }
                    else if (create_pdf != null)
                    {
                        var ReportType = Convert.ToInt32(context.Request.Form["type"]);
                        var CompanyID = Convert.ToInt32(context.Request.Form["companyid"]);
                        var EMBS = context.Request.Form["embs"];

                        var force_crm = Convert.ToBoolean(context.Request.Form["force_crm"]);
                        var force_year = Convert.ToInt32(context.Request.Form["forced_year"]);


                        //if (EMBS == "6753108" || EMBS == "4783018" || EMBS == "4056043" || EMBS == "4015215")
                        //    CurYear = 2014;
                        var CurYear = 0;
                        if (force_crm)
                        {
                            DALHelper.UpdateForcedYear(force_year);
                            CurYear = DALHelper.GetCurrentYear(true);
                        }
                        else
                        {
                            CurYear = DALHelper.GetCurrentYear(false);
                        }

                        DALHelper.CreateReportRequest(userobj.UserID, ReportType, EMBS, CurYear);

                        // check if user report exists
                        var check = DocumentClass.CheckIfReportExists(CurYear, EMBS, ReportType, userobj.UserID);

                        if (ReportType != 3)
                        {
                            if (check == 0 || (isTesting && ReportType != 2))
                            {
                                // check user licence
                                var licence_check = DALHelper.CheckPrepayLicence(userobj.UserID, ReportType, userobj.Type);

                                if (licence_check != null)
                                {
                                    // get report for user
                                    var rep = DocumentClass.GetReportByUserCompanyYearAndReport(userobj.UserID, EMBS, CurYear, ReportType);

                                    string res = null;
                                    // check if report exists
                                    if (rep == null || (isTesting && ReportType != 2))
                                    {
                                        // generete report
                                        res = DocumentClass.GenerateReport(userobj.UserID, 0, EMBS, CurYear, ReportType, licence_check.ID, clientEmbs, force_crm);

                                        if (force_crm)
                                        {
                                            DALHelper.DeleteForcedYear();
                                        }

                                        if (res == "Невалиден матичен број")
                                        {
                                            context.Response.Write("Порака од ЦРМ: Невалиден матичен број");
                                            context.Response.End();
                                        }
                                        else if (res == "CRM Error")
                                        {
                                            context.Response.Write("CRM Error");
                                            context.Response.End();
                                        }
                                        else if (res == "Не е пронајден запис за барањето!")
                                        {
                                            context.Response.Write("Порака од ЦРМ: Не е пронајден запис за барањето!");
                                            context.Response.End();
                                        }
                                        else if (res == "Request already sent.")
                                        {
                                            context.Response.Write("Порака од eboniteti.mk: Request already sent.");
                                            context.Response.End();
                                        }
                                        else if (res == "Timed out!")
                                        {
                                            context.Response.Write("Порака од eboniteti.mk: Timed out!");
                                            context.Response.End();
                                        }
                                    }
                                    else
                                    {
                                        res = rep.UID.ToString();
                                    }
                                    Guid checkGuid;
                                    if (userobj.Type == 1)
                                    {
                                        if (Guid.TryParse(res, out checkGuid))
                                        {
                                            //update prepay for user
                                            DALHelper.UpdateUserPrepay(licence_check.ID, userobj.UserID, ReportType);

                                            //create user reports
                                            DALHelper.CreateUserReport(CurYear, userobj.UserID, EMBS, ReportType, licence_check.ID, res);

                                            context.Response.Write(res);
                                        }
                                        else
                                            context.Response.Write("Порака од eboniteti.mk: Report Generator error" + res);
                                    }
                                    else
                                    {
                                        context.Response.Write(res);
                                    }
                                }
                                else
                                    context.Response.Write("Порака од eboniteti.mk: Licence error");
                            }
                            else if (check == 1)
                            {
                                // check user licence
                                var licence_check = DALHelper.CheckPrepayLicence(userobj.UserID, ReportType, userobj.Type);

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
                                        context.Response.Write("Порака од eboniteti.mk: No report error");
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
                                            context.Response.Write("Порака од eboniteti.mk: No report error");
                                    }
                                    else
                                        context.Response.Write("Порака од eboniteti.mk: Licence error");
                                }
                            }
                        }
                        else
                        {
                            // check user licence
                            var licence_check = DALHelper.CheckPrepayLicence(userobj.UserID, ReportType, userobj.Type);

                            if (licence_check != null)
                            {
                                string res = DocumentClass.GenerateBlokada(userobj.UserID, EMBS, CurYear, licence_check.ID, userTicket);

                                if (userobj.Type == 1)
                                {
                                    if (res != null)
                                    {
                                        //update prepay for user
                                        DALHelper.UpdateUserPrepay(licence_check.ID, userobj.UserID, ReportType);

                                        //create user reports
                                        DALHelper.CreateUserReport(CurYear, userobj.UserID, EMBS, ReportType, licence_check.ID, res);

                                        context.Response.Write(res);
                                    }
                                    else
                                        context.Response.Write("Порака од eboniteti.mk: Report Generator error" + res);
                                }
                                else
                                {
                                    context.Response.Write(res);
                                }
                            }
                            else
                                context.Response.Write("Порака од eboniteti.mk: Licence error");
                        }

                    }
                    else if (send_mail != null)
                    {
                        var UserID = userobj.UserID;
                        var CompanyID = Convert.ToInt32(context.Request.Form["companyid"]);
                        var EMBS = context.Request.Form["embs"];

                        var user = DALHelper.GetUserByID(UserID);

                        if (user != null)
                        {
                            var mailType = Convert.ToInt32(mail_type);

                            var licence_check = DALHelper.CheckPrepayLicence(userobj.UserID, mailType, userobj.Type);


                            if (licence_check != null)
                            {
                                var company = DALHelper.GetCompanyByEMBS(EMBS);

                                if (company != null)
                                {
                                    var res1 = SendRequestMail(mailType, user, company);

                                    var res2 = SendMailToClient(mailType, user, company);

                                    var curYear = DALHelper.GetCurrentYear(false);

                                    DALHelper.CreateReportRequest(userobj.UserID, mailType, company.EMBS, curYear);

                                    if (res1 == false || res2 == false)
                                        context.Response.Write("error");

                                    context.Response.Write("Mail Sent");
                                }
                                else
                                    context.Response.Write("error");
                            }
                            else
                                context.Response.Write("Порака од eboniteti.mk: Licence error");

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
            else
            {
                context.Response.Write("error");
            }
        }

        public bool SendRequestMail(int mail_type, User user, Company company)
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

        public bool SendChargeMailToClient(int licenseID, string email, int UserID, string companyName, int packType)
        {

            var Message = "Почитувани, <br/><br/>Извештајот за компанијата " + companyName + " ви е доставен. <br/> <br/><br/>За останати информации контактирајте не на 02/3117-100 или boniteti@targetgroup.mk<br/><br/>Тимот на Ебонитети.мк";

            //DALHelper.UpdateReportRequestFunc(reqID, DALHelper.ReportRequestActions.SendMail);



            var Subject = "Нотификација за Бонитетен извештај";

            try
            {
                DALHelper.UpdateUserPrepay(licenseID, UserID, packType);

                MailHelper.SendMail(email, Subject, Message, true);

                return true;
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }

        public bool SendRequestReportMailToClient(string email, int companyID, string companyName, int reqID, string no_data)
        {
            var link = "http://eboniteti.mk/Authenticated/CompanyDetails.aspx?id=" + companyID;

            var Message = "";
            if (no_data == "1")
            {
                Message = "Почитувани, <br/><br/>Извештајот за компанијата " + companyName + " не е достапен. <br/> <br/>Субјектот нема поднесено годишна сметка или е друг вид на субјект за кој податоците се недостапни.<br/> <br/>За останати информации контактирајте не на 02/3117-100 или boniteti@targetgroup.mk<br/><br/>Тимот на Ебонитети.мк";
                DALHelper.UpdateReportRequestFunc(reqID, DALHelper.ReportRequestActions.NoData);
            }
            else
                Message = "Почитувани, <br/><br/>Извештајот за компанијата " + companyName + " е достапен. <br/> <br/>Можете да го превземете на следниов линк: <br/><br/><a href=\"" + link + "\">" + link + "</a> <br/> <br/><br/>За останати информации контактирајте не на 02/3117-100 или boniteti@targetgroup.mk<br/><br/>Тимот на Ебонитети.мк";

            DALHelper.UpdateReportRequestFunc(reqID, DALHelper.ReportRequestActions.SendMail);

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

        public bool SendMailToClient(int mail_type, User user, Company company)
        {
            var Message = "";
            var Subject = "";

            if (mail_type == 1)
            {
                Message = "Почитувани, <br/><br/>Ви благодариме за нарачката на бонитетниот извештај. <br/><u>Податоци за нарачката:</u><br/>Компанија: " + company.Name + "<br/>ЕМБС: " + company.EMBS + "<br/><br/>Извештајот ќе Ви биде доставен најдоцна во рок од 1 (еден) работен ден по извршената нарачка. <br/>За останати информации контактирајте не на 02/3117-100 или boniteti@targetgroup.mk<br/><br/>Тимот на Ебонитети.мк";

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