using Bonitet.CRM;
using Bonitet.DAL;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TuesPechkin;

namespace Bonitet.Document
{
    public class CRM_DocumentClass
    {
        public static string PopulateTemplate(string crm_data)
        {
            HtmlDocument doc = new HtmlDocument();

            var html = AppDomain.CurrentDomain.BaseDirectory + "Templates\\CRM_Blokada_Template.html";
            try
            {
                doc.Load(html);
            }
            catch (Exception)
            {
                throw;
            }


            doc = MapInfoToTemplate(crm_data, doc);


            return doc.DocumentNode.InnerHtml;
        }

        public static Dictionary<string, string> GetCRM_BlokadaStatus(string crm_data)
        {
            var res = new Dictionary<string, string>();

            if (crm_data.Contains("Не е пронајден запис за барањето!"))
                res["AccInfo"] = "Не е пронајден запис за барањето!";

            res["EMBS"] = ExtractString(crm_data, "LEID");
            res["AccInfo"] = ExtractString(crm_data, "AccInfo");
            res["TimeStamp"] = ExtractString(crm_data, "TimeStamp");
            res["InfoMessage"] = ExtractString(crm_data, "InfoMessage");

            if (res["InfoMessage"] == null)
                return null;

            if (res["InfoMessage"].ToLower().Contains("невалиден матичен број"))
                res["AccInfo"] = "Невалиден матичен број";

            return res;
        }

        public static HtmlDocument MapInfoToTemplate(string crm_data, HtmlDocument doc)
        {

            var EMBS = ExtractString(crm_data, "LEID");
            var AccInfo = ExtractString(crm_data, "AccInfo");
            var TimeStamp = ExtractString(crm_data, "TimeStamp");
            var InfoMessage = ExtractString(crm_data, "InfoMessage");

            var errorMsg = "";
            if (crm_data == null)
                errorMsg = "Проблем со сервисот на еБонитети!";
            if (crm_data.Contains("Certificate Error"))
                errorMsg = "Проблем со сертификатот!";
            else if (crm_data.Contains("Timed out"))
                errorMsg = "Проблем со сертификатот!";
            else if (crm_data.Contains("Не е пронајден запис за барањето!") || InfoMessage == null)
                errorMsg = "Не е пронајден запис за барањето!";
            else if (InfoMessage.ToLower().Contains("невалиден матичен број"))
                errorMsg = "Невалиден матичен број!";





            if (errorMsg.Length > 0)
            {

                doc.DocumentNode.RemoveAll();
                doc.DocumentNode.InnerHtml = errorMsg;

                return doc;
            }

            var webRootPath = System.Web.HttpContext.Current.Server.MapPath("~");

            var AbsoluteUrlPath = Path.GetFullPath(Path.Combine(webRootPath, "..\\Bonitet.Web\\img"));

            var curCompany = DAL.DALHelper.GetCompanyByEMBS_BM(EMBS);

            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{AbsoluteUrl}", AbsoluteUrlPath);

            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{EMBS}", EMBS);
            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{naziv}", curCompany.CelosenNazivNaSubjektot);
            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{adresa}", curCompany.Sediste);
            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{datum}", TimeStamp);
            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{status}", AccInfo);
            doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{komentar}", InfoMessage);

            return doc;
        }
        public static string ExtractString(string s, string tag)
        {
            // You should check for errors in real-world code, omitted for brevity
            try
            {
                var startTag = "<" + tag + ">";
                int startIndex = s.IndexOf(startTag) + startTag.Length;
                int endIndex = s.IndexOf("</" + tag + ">", startIndex);
                return s.Substring(startIndex, endIndex - startIndex);
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public static bool SaveImportReportValues(Dictionary<string, string>[] data)
        {
            // all rows
            foreach (var item in data)
            {
                var EMBS = "";
                var Year = "";
                var ReportValues = new Dictionary<int, string>();

                if (!item.TryGetValue("EMBS", out EMBS))
                    break;
                if (!item.TryGetValue("Year", out Year))
                    break;

                // current row values
                foreach (var val in item)
                {
                    if (val.Key.Equals("EMBS") == true || val.Key.Equals("Year") == true)
                    {
                    }
                    else
                    {
                        var AccountNo = val.Key;
                        var CurrentYear = val.Value;
                        if (string.IsNullOrEmpty(CurrentYear))
                            continue;

                        int val1 = 0;
                        double value = 0.0;

                        double.TryParse(CurrentYear, out value);
                        int.TryParse(AccountNo, out val1);

                        if (val1 == 0)
                            continue;

                        var tmpValue = Convert.ToInt64(Math.Round(value)).ToString();

                        if (String.IsNullOrEmpty(tmpValue))
                        {
                            tmpValue = "0";
                        }
                        ReportValues.Add(val1, tmpValue);
                    }
                }
                if (string.IsNullOrEmpty(EMBS) == false && string.IsNullOrEmpty(Year) == false)
                {
                    Bonitet.DAL.DALHelper.InsertReportValuesImport(ReportValues, EMBS, Int32.Parse(Year));
                }
            }
            return true;
        }



        public static bool SaveImportCompanyValues(Dictionary<string, string>[] data)
        {
            // all rows
            foreach (var item in data)
            {
                var EMBS = "";
                var Year = "";
                var CompanyValues = new Dictionary<int, double>();

                if (!item.TryGetValue("EMBS", out EMBS))
                    break;
                if (!item.TryGetValue("Year", out Year))
                    break;

                // current row values
                foreach (var val in item)
                {
                    if (val.Key.Equals("EMBS") == true || val.Key.Equals("Year") == true)
                    {
                    }
                    else
                    {
                        var AccountNo = val.Key;
                        var CurrentYear = val.Value;
                        if (string.IsNullOrEmpty(CurrentYear))
                            continue;

                        int val1 = 0;
                        double value = 0.0;

                        double.TryParse(CurrentYear, out value);
                        int.TryParse(AccountNo, out val1);

                        if (val1 == 0)
                            continue;

                        CompanyValues.Add(val1, value);
                    }
                }
                if (string.IsNullOrEmpty(EMBS) == false && string.IsNullOrEmpty(Year) == false)
                {
                    var isImported = Bonitet.DAL.DALHelper.InsertCompanyValuesImport(CompanyValues, EMBS, Int32.Parse(Year));
                }
            }
            return true;
        }



        public static string SaveCRM_Account(string EMBS, int Year, bool force_crm)
        {
            if (force_crm == false)
            {
                var check1 = DALHelper.GetCompanyValuesForCurrentYear(EMBS, Year);
                if (check1 != null)
                {
                    if (check1.Count() > 0)
                        return "Exists";
                    else
                    {
                        var check2 = DALHelper.CheckIfCRMHasData(EMBS);

                        if (check2 == true)
                        {
                            return "Exists";
                        }
                    }
                }
            }
            else
            {
                // delete existing data for company
                DALHelper.DeleteCompanyValuesForCurrentYear(EMBS, Year);
                Console.WriteLine("Forcing CRM Request");
            }

            //var EMBS = "4001192";
            //var Year = "2014";
            //var crmResponse = DALHelper.GetResponse(8227, Year, 1);
            //var res = DALHelper.GetXmlResuls(35);
            var res = CRM_ServiceHelper.GetLiveCRM_Account(EMBS, Year, force_crm);

            if (res == null)
                return "Request already sent.";
            if (res == "Timed out!")
                return "Timed out!";
            if (res.Contains("Системска Грешка!"))
                return "Системска Грешка!";
            if (res.StartsWith("Certificate Error"))
                return res;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(res);

            var CompanyValues = new Dictionary<int, double>();
            var CompanyDetails = new Company();

            if (doc.InnerText.Contains("Невалиден матичен број"))
                return "Невалиден матичен број";
            if (doc.InnerText.Contains("CRM Error"))
                return "CRM Error";
            if (doc.InnerText.Contains("Не е пронајден запис за барањето!"))
                return "Не е пронајден запис за барањето!";

            foreach (XmlElement data in doc.SelectNodes("CrmResponse"))
            {
                var i = 0;
                foreach (XmlElement child in data.SelectNodes("CrmResultItems"))
                {
                    if (i == 0)
                    {
                        CompanyDetails.Name = ExtractString(child.InnerXml, "LEFullName");
                        CompanyDetails.Mesto = ExtractString(child.InnerXml, "Place");
                        CompanyDetails.EMBS = ExtractString(child.InnerXml, "LEID");
                        i++;
                        continue;
                    }

                    foreach (XmlElement child1 in child.SelectNodes("CrmResultItem"))
                    {
                        var AccountNo = ExtractString(child1.InnerXml, "AccountNo");
                        var CurrentYear = ExtractString(child1.InnerXml, "CurrentYear");
                        if (string.IsNullOrEmpty(CurrentYear))
                            continue;

                        int val1 = 0;
                        double value = 0.0;

                        double.TryParse(CurrentYear, out value);
                        int.TryParse(AccountNo, out val1);

                        if (val1 == 0)
                            continue;

                        CompanyValues.Add(val1, value);

                    }

                }

            }

            Bonitet.DAL.DALHelper.InsertCompanyValues(CompanyDetails, CompanyValues, EMBS, Year, res);

            CompanyValues = new Dictionary<int, double>();
            CompanyDetails = new Company();

            foreach (XmlElement data in doc.SelectNodes("CrmResponse"))
            {
                var i = 0;
                foreach (XmlElement child in data.SelectNodes("CrmResultItems"))
                {
                    if (i == 0)
                    {
                        CompanyDetails.Name = ExtractString(child.InnerXml, "LEFullName");
                        CompanyDetails.Mesto = ExtractString(child.InnerXml, "Place");
                        CompanyDetails.EMBS = ExtractString(child.InnerXml, "LEID");
                        i++;
                        continue;
                    }

                    foreach (XmlElement child1 in child.SelectNodes("CrmResultItem"))
                    {
                        var AccountNo = ExtractString(child1.InnerXml, "AccountNo");
                        var Previous = ExtractString(child1.InnerXml, "Previous");
                        if (string.IsNullOrEmpty(Previous))
                            continue;

                        int val1 = 0;
                        double value = 0.0;

                        double.TryParse(Previous, out value);
                        int.TryParse(AccountNo, out val1);

                        if (val1 == 0)
                            continue;

                        CompanyValues.Add(val1, value);

                    }

                }

            }

            Bonitet.DAL.DALHelper.InsertCompanyValues(CompanyDetails, CompanyValues, EMBS, (Year - 1), res);


            return "Exists";
        }
    }
}
