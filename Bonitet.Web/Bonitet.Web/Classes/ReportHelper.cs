using Bonitet.DAL;
using Bonitet.DAL.BiznisMreza;
using Bonitet.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bonitet.Web.Classes
{
    public class ReportResponse
    {
        public int RequestType { get; set; }
        public string RequestTypeString { get; set; }

        public string Error { get; set; }
        public string Success { get; set; }
    }

    public class ReportHelper
    {
        public static string ValidateUserReportRequest(Bonitet.DAL.User user, Subjekt curCompany)
        {
            if (user != null)
            {
                if (curCompany != null)
                {
                    return "";
                }
                else
                {
                    return "Компанијата не е пронајдена"; // company not found
                }
            }
            else
            {
                return "Корисникот не е пронајден"; // user not found
            }
        }

        public static ReportResponse Service_GenerateReport(int UserID, string EMBS)
        {
            var user = DALHelper.GetUserByID(UserID);
            var curCompany = DALHelper.GetCompanyByEMBS_BM(EMBS);

            var reportResponse = new ReportResponse();

            reportResponse.Error = ValidateUserReportRequest(user, curCompany);

            if (reportResponse.Error != null && reportResponse.Error.Length > 0)
            {
                // return the error
                return reportResponse;
            }
            else
            {

                // 1 = Boniteten Izvestaj; 3 = Blokada
                var ReportType = 1;

                if (DALHelper.CheckIfCompanyHasDataForReport(EMBS, ReportType))
                {
                    // local data available
                    reportResponse.RequestType = 2;
                    reportResponse.RequestTypeString = "Local";
                }
                else
                {
                    // contact CRM
                    reportResponse.RequestType = 1;
                    reportResponse.RequestTypeString = "CRM";
                    //reportResponse.Error = "Податоците не се преземени од ЦРМ";
                    //reportResponse.Success = false.ToString();
                    //return reportResponse;
                }

                var CurYear = DALHelper.GetCurrentYear(false);

                DALHelper.CreateReportRequest(UserID, ReportType, EMBS, CurYear);

                // check if user report exists
                //var check = DocumentClass.CheckIfReportExists(CurYear, EMBS, ReportType, UserID);

                // check user licence
                var licence_check = DALHelper.CheckPrepayLicence(UserID, ReportType, 1);

                if (licence_check != null)
                {
                    string res = null;

                    //// get report for user
                    //var rep = DocumentClass.GetReportByUserCompanyYearAndReport(UserID, EMBS, CurYear, ReportType);

                    //// check if report exists
                    //if (rep == null)
                    //{
                        // generete report
                        res = DocumentClass.GenerateReport(UserID, 0, EMBS, CurYear, ReportType, licence_check.ID, user.EMBS, false);

                        if (res == "Невалиден матичен број")
                        {
                            reportResponse.Error = "Порака од ЦРМ: Невалиден матичен број";
                        }
                        else if (res == "CRM Error")
                        {
                            reportResponse.Error = "CRM Error";
                        }
                        else if (res == "Не е пронајден запис за барањето!")
                        {
                            reportResponse.Error = "Порака од ЦРМ: Не е пронајден запис за барањето!";
                        }
                        else if (res == "Request already sent.")
                        {
                            reportResponse.Error = "Порака од eboniteti.mk: Request already sent.";
                        }
                        else if (res == "Timed out!")
                        {
                            reportResponse.Error = "Порака од eboniteti.mk: Timed out!";
                        }
                    //}
                    //else
                    //{
                    //    res = rep.UID.ToString();
                    //}
                    

                    if (reportResponse.Error != null && reportResponse.Error.Length > 0)
                    {
                        // return error
                        return reportResponse;
                    }
                    else
                    {
                        Guid checkGuid;
                        if (Guid.TryParse(res, out checkGuid))
                        {
                            //update prepay for user
                            DALHelper.UpdateUserPrepay(licence_check.ID, UserID, ReportType);

                            //create user reports
                            DALHelper.CreateUserReport(CurYear, UserID, EMBS, ReportType, licence_check.ID, res);

                            reportResponse.Success = true.ToString();
                            return reportResponse;
                        }
                        else
                        {
                            reportResponse.Error = "Порака од eboniteti.mk: Report Generator error: " + res;
                            return reportResponse;
                        }
                    }
                }
                else
                {
                    reportResponse.Error = "Licence error";
                    return reportResponse;
                }
            }
        }


        public static ReportResponse Service_GenerateBlokada(int UserID, string EMBS)
        {
            var user = DALHelper.GetUserByID(UserID);
            var curCompany = DALHelper.GetCompanyByEMBS_BM(EMBS);

            var reportResponse = new ReportResponse();

            reportResponse.Error = ValidateUserReportRequest(user, curCompany);

            if (reportResponse.Error != null && reportResponse.Error.Length > 0)
            {
                // return the error
                return reportResponse;
            }
            else
            {

                // 1 = Boniteten Izvestaj; 3 = Blokada
                var ReportType = 3;
                reportResponse.RequestType = 1;
                reportResponse.RequestTypeString = "CRM";
                //reportResponse.Error = "Податоците не се преземени од ЦРМ";
                //reportResponse.Success = false.ToString();
                //return reportResponse;


                var CurYear = DALHelper.GetCurrentYear(false);

                DALHelper.CreateReportRequest(UserID, ReportType, EMBS, CurYear);

                // check if user report exists
                //var check = DocumentClass.CheckIfReportExists(CurYear, EMBS, ReportType, UserID);

                // check user licence
                var licence_check = DALHelper.CheckPrepayLicence(UserID, ReportType, 1);

                if (licence_check != null)
                {
                    string res = null;

                    var blokadaTicket = "Service_" + DateTime.Now.Ticks;

                    res = DocumentClass.GenerateBlokada(UserID, EMBS, CurYear, licence_check.ID, blokadaTicket);

                    if (res == "Невалиден матичен број")
                    {
                        reportResponse.Error = "Порака од ЦРМ: Невалиден матичен број";
                    }
                    else if (res == "CRM Error")
                    {
                        reportResponse.Error = "CRM Error";
                    }
                    else if (res == "Не е пронајден запис за барањето!")
                    {
                        reportResponse.Error = "Порака од ЦРМ: Не е пронајден запис за барањето!";
                    }
                    else if (res == "Request already sent.")
                    {
                        reportResponse.Error = "Порака од eboniteti.mk: Request already sent.";
                    }
                    else if (res == "Timed out!")
                    {
                        reportResponse.Error = "Порака од eboniteti.mk: Timed out!";
                    }
                    if (reportResponse.Error != null && reportResponse.Error.Length > 0)
                    {
                        // return error
                        return reportResponse;
                    }
                    else
                    {
                        Guid checkGuid;
                        if (Guid.TryParse(res, out checkGuid))
                        {
                            //update prepay for user
                            DALHelper.UpdateUserPrepay(licence_check.ID, UserID, ReportType);

                            //create user reports
                            DALHelper.CreateUserReport(CurYear, UserID, EMBS, ReportType, licence_check.ID, res);

                            reportResponse.Success = true.ToString();
                            return reportResponse;
                        }
                        else
                        {
                            reportResponse.Error = "Порака од eboniteti.mk: Report Generator error: " + res;
                            return reportResponse;
                        }
                    }
                }
                else
                {
                    reportResponse.Error = "Порака од eboniteti.mk: Licence error";
                    return reportResponse;
                }
            }
        }

        public static ReportResponse PrepareReportForDownload(int UserID, int ReportID)
        {
            var response = new ReportResponse();

            //get file
            var doc = DocumentClass.GetReportByID(ReportID);

            //check file status
            //if not file available write msgs
            if (doc == null)
            {
                response.Error = "Document does not exist!";
                return response;
            }

            //update user reports
            DALHelper.UpdateUserReport(UserID, doc.UID.Value);

            //insert into log
            DALHelper.SaveDownloadInLog(doc.UID.Value);

            var tmpParts = doc.Path.Split(new string[] { "App_Data\\" }, StringSplitOptions.None);

            if (tmpParts.Length > 1)
            {
                response.Success = doc.Path;
            }
            else
            {
                response.Error = "Problem getting document!";
            }

            return response;
        }

    }
}