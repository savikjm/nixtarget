using Bonitet.DAL;
using Bonitet.Web.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Bonitet.Web.Services
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportService
    {
        [OperationContract]
        public ReportResponse GenerateReport(int UserID, string EMBS, int Language)
        {
            //UserID = 1;
            var reportResponse = ReportHelper.Service_GenerateReport(UserID, EMBS);

            return reportResponse;
        }
        [OperationContract]
        public ReportResponse GenerateBlokada(int UserID, string EMBS, int Language)
        {
            //UserID = 1;
            var reportResponse = ReportHelper.Service_GenerateBlokada(UserID, EMBS);

            return reportResponse;
        }


        [OperationContract]
        public ReportResponse DownloadReport(int UserID, int ReportID)
        {
            var reportResponse = ReportHelper.PrepareReportForDownload(UserID, ReportID);

            return reportResponse;
        }

        [OperationContract]
        public List<c_UserReportObj> GetAllUserReports(int UserID)
        {
            var ReportType = 1;
            var reportResponse = DALHelper.GetAllUserReportsByType(UserID, ReportType);

            return reportResponse;
        }

        [OperationContract]
        public string GenerateReportJson(int UserID, string EMBS, int Language)
        {
            //UserID = 1;
            var reportResponse = ReportHelper.Service_GenerateReport(UserID, EMBS);

            var settings = SerializeHelper.GetDefaultSettings();

            var res = JsonConvert.SerializeObject(reportResponse, settings);

            return res;
        }

        [OperationContract]
        public string GenerateBlokadaJson(int UserID, string EMBS, int Language)
        {
            //UserID = 1;
            var reportResponse = ReportHelper.Service_GenerateBlokada(UserID, EMBS);

            var settings = SerializeHelper.GetDefaultSettings();

            var res = JsonConvert.SerializeObject(reportResponse, settings);

            return res;
        }


        [OperationContract]
        public string DownloadReportJson(int UserID, int ReportID)
        {
            var reportResponse = ReportHelper.PrepareReportForDownload(UserID, ReportID);

            var settings = SerializeHelper.GetDefaultSettings();

            var res = JsonConvert.SerializeObject(reportResponse, settings);

            return res;
        }

        [OperationContract]
        public string GetAllUserReportsJson(int UserID)
        {
            var ReportType = 1;
            var reportResponse = DALHelper.GetAllUserReportsByType(UserID, ReportType);

            var settings = SerializeHelper.GetDefaultSettings();

            var res = JsonConvert.SerializeObject(reportResponse, settings);

            return res;
        }


    }
}
