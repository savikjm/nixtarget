using Bonitet.CRM.CRMService;
using Bonitet.CRM.CRMServiceProxy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Bonitet.CRM
{
    public class CRM_ServiceHelper
    {


        public static string GetCRM_AccountStatus(string EMBS){

            //return null;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            XmlWebService ws = new XmlWebService();

            X509Store store = new X509Store(StoreName.My);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates[0];

            ws.ClientCertificates.Add(cert);

            var strParameters = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CrmRequest ProductName=\"LEInfoBlockedbyBankAccountsTARGET\"><Parameters TemplateName=\"LEInfoBlockedBA\"><Parameter Name=\"@LEID\">{0}</Parameter></Parameters></CrmRequest>";

            string strResult = string.Empty;

            List<string[]> results = new List<string[]>();


            var tmp = string.Format(strParameters, EMBS);
            strResult = ws.ProcessRequest(tmp);

            ws.Dispose();
            ws = null;

            return strResult;
        }

        public static string GetCRM_Account(string EMBS, int Year)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            XmlWebService ws = new XmlWebService();

            X509Store store = new X509Store(StoreName.My);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates[0];

            ws.ClientCertificates.Add(cert);

            var strParameters = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CrmRequest ProductName=\"AAListingTARGET\"><Parameters TemplateName=\"CVLEInfo\"><Parameter Name=\"@LEID\">{0}</Parameter><Parameter Name=\"@Year\">{1}</Parameter></Parameters><Parameters TemplateName=\"AAListingInfo\"><Parameter Name=\"@LEID\">{0}</Parameter><Parameter Name=\"@Year\">{1}</Parameter></Parameters></CrmRequest>";

            string strResult = string.Empty;

            List<string[]> results = new List<string[]>();


            var tmp = string.Format(strParameters, EMBS,Year);
            strResult = ws.ProcessRequest(tmp);

            ws.Dispose();
            ws = null;

            return strResult;
        }

        public static string GetLiveCRM_Account(string EMBS, int Year, bool force_crm)
        {
            var ws = new CRMServiceProxy.CRMServiceProxy();

            try
            {
                var res = ws.GetResultsForReport(EMBS, Year, force_crm);

                return res;
            }
            catch (Exception ex)
            {
                return "Timed out!";
            }
        }

        public static string GetLiveCRM_AccountStatus(string EMBS, int UserID, string Ticket)
        {
            var ws = new CRMServiceProxy.CRMServiceProxy();

            try
            {
                var res = ws.GetResults(EMBS, UserID, Ticket);

                return res;
            }
            catch (Exception ex)
            {
                return "Timed out!";
            }
        }


    }
}
