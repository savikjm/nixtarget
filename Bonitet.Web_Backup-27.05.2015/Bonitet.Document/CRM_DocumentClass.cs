using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static HtmlDocument MapInfoToTemplate(string crm_data, HtmlDocument doc) 
        {

            var EMBS = ExtractString(crm_data, "LEID");
            var AccInfo = ExtractString(crm_data, "AccInfo");
            var TimeStamp = ExtractString(crm_data, "TimeStamp");
            var InfoMessage = ExtractString(crm_data, "InfoMessage");

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
    }
}
