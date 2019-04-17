using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;
using System.IO;
using System.Drawing.Printing;
using Bonitet.DAL;
using Bonitet.Charts;
using System.Reflection;
using System.Globalization;
using HtmlAgilityPack;

namespace Bonitet.Document
{
    public partial class DocumentClass
    {
        public static string webRootPath = (System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.Server.MapPath("~") : "");

        public static string AbsolutePath = Path.GetFullPath(Path.Combine(webRootPath, "..\\Bonitet.Web\\App_Data\\"));

        public static string AbsoluteUrlPath = Path.GetFullPath(Path.Combine(webRootPath, "..\\Bonitet.Web\\"));

        public static int CurrentPage = 1;

        public static int CheckIfReportExists(int Year, string EMBS, int ReportType, int UserID)
        {
            var doc = DALHelper.CheckReportByUserCompanyYearAndReport(Year, EMBS, ReportType, UserID);

            return doc;
        }


        public static Report GetReportByUID(Guid uid)
        {

            var report = DALHelper.GetReportByUIDFromDB(uid);

            return report;

        }
        public static Report GetReportByID(int ReportID)
        {

            var report = DALHelper.GetReportByIDFromDB(ReportID);

            return report;

        }



        public static Report GetReportByUserCompanyYearAndReport(int UserID, string EMBS, int Year, int ReportType)
        {
            var report = DALHelper.GetReportByUserCompanyYearAndReportFromDB(UserID, EMBS, Year, ReportType);

            return report;
        }

        public static string GenerateHTMLFromDataForReport(Attributes data, int ReportType)
        {
            CurrentPage = 1;
            var html = "";
            if (ReportType == 1)
            {
                GetGlobalTotals(data);

                HtmlDocument doc = new HtmlDocument();

                var lastPage = AppDomain.CurrentDomain.BaseDirectory + "Templates\\styles.css";
                try
                {
                    doc.Load(lastPage);
                }
                catch (Exception)
                {
                    throw;
                }

                html = DocumentClass.CoverPage(data);

                html = html.Replace("{css}", doc.DocumentNode.InnerHtml);

                html += DocumentClass.FirstPage(data);
                html += DocumentClass.SecondPage(data);
                html += DocumentClass.ThirdPage(data);
                if (data.GodisnaSmetka)
                {
                    html += DocumentClass.FourthPage(data);
                }
                html += DocumentClass.FifthPage(data);
                html += DocumentClass.SixthPage(data);
                if (data.GodisnaSmetka)
                {

                    html += DocumentClass.ChartsPage(data);

                    html += DocumentClass.SeventhPage(data);
                    html += DocumentClass.EighthPage(data);

                    html += DocumentClass.NinthPage(data);
                }
                //html += DocumentClass.TenthPage(data);


                lastPage = AppDomain.CurrentDomain.BaseDirectory + "Templates\\CRM_Znacenje_Na_Semafor.html";
                try
                {
                    doc.Load(lastPage);
                }
                catch (Exception)
                {
                    throw;
                }
                doc.DocumentNode.InnerHtml = doc.DocumentNode.InnerHtml.Replace("{PageNumber}", CurrentPage.ToString());

                html += doc.DocumentNode.InnerHtml;
            }
            else if (ReportType == 2)
            {
                html = DocumentClass.ShortReportPage(data);
            }
            return html;
        }

        public static string GenerateBlokada(int UserID, string EMBS, int Year, int PackID, string ticket)
        {
            var company = new Company();


            if (EMBS.Length > 0)
            {
                company = DALHelper.GetCompanyByEMBS(EMBS);
            }

            if (company != null && company.EMBS != null)
            {

                var HTML = SetReport3Data_Live(EMBS, UserID, ticket);

                var filename = EMBS + "_" + Year + "_" + Guid.NewGuid();

                filename = filename + ".pdf";

                var document = new HtmlToPdfDocument
                {
                    GlobalSettings =
                    {
                        ProduceOutline = true,
                        DocumentTitle = "Bonitet",
                        PaperSize = PaperKind.A4
                    },
                    Objects = {
                new ObjectSettings { HtmlText = HTML }
                }
                };

                IPechkin converter = Factory.Create();
                byte[] result = converter.Convert(document);


                try
                {
                    var path_filename = @AbsolutePath + filename;

                    File.WriteAllBytes(path_filename, result);

                    var uid = Guid.NewGuid();

                    var tmp_document = new Report
                    {
                        UID = uid,
                        Year = Year,
                        ReportType = 3,
                        Path = path_filename,
                        CompanyID = company.ID
                    };

                    DALHelper.SaveReportToDatabase(tmp_document.Path, tmp_document.CompanyID, tmp_document.Year, (Guid)tmp_document.UID, Convert.ToInt32(tmp_document.ReportType), PackID);

                    return uid.ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
                return null;
        }

        public static string GenerateReport(int UserID, int CompanyID, string EMBS, int Year, int ReportType, int PackID, string clientEMBS, bool force_crm)
        {
            var company = new Company();
            var crm_res = "Exists";

            if (EMBS.Length > 0)
            {
                company = DALHelper.GetCompanyByEMBS(EMBS);
            }
            else if (CompanyID != 0)
            {
                company = DALHelper.GetCompanyByID1(CompanyID);
            }

            //if (ReportType == 1)
            //{
            //    crm_res = CRM_DocumentClass.SaveCRM_Account(EMBS, Year, force_crm);
            //}

            if (crm_res == "Exists")
            {
                company = DALHelper.GetCompanyByEMBS(EMBS);

                var data = new Attributes();
                var HTML = "";
                if (ReportType == 1)
                    data = SetReport1Data(UserID, company.EMBS, Year, clientEMBS, force_crm);
                else if (ReportType == 2)
                    data = SetReport2Data(UserID, company.EMBS, Year);
                else if (ReportType == 3)
                {
                    HTML = SetReport3Data(company.EMBS);
                }

                if (ReportType != 3)
                {
                    data.last_year = (Year - 1).ToString();
                    data.this_year = Year.ToString();

                    HTML = GenerateHTMLFromDataForReport(data, ReportType);
                }

                var filename = EMBS + "_" + Year + "_" + ReportType;

                if (ReportType == 3)
                    filename += Guid.NewGuid();

                filename = filename + ".pdf";

                var document = new HtmlToPdfDocument
                {
                    GlobalSettings =
                    {
                        ProduceOutline = true,
                        DocumentTitle = "Bonitet",
                        PaperSize = PaperKind.A4
                    },
                    Objects = {
                    new ObjectSettings { HtmlText = HTML }
                }
                };

                IPechkin converter = Factory.Create();
                byte[] result = converter.Convert(document);


                try
                {
                    var path_filename = @AbsolutePath + filename;

                    File.WriteAllBytes(path_filename, result);

                    var uid = Guid.NewGuid();

                    var tmp_document = new Report
                    {
                        UID = uid,
                        Year = Year,
                        ReportType = ReportType,
                        Path = path_filename,
                        CompanyID = company.ID
                    };

                    DALHelper.SaveReportToDatabase(tmp_document.Path, tmp_document.CompanyID, tmp_document.Year, (Guid)tmp_document.UID, Convert.ToInt32(tmp_document.ReportType), PackID);

                    return uid.ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
                return crm_res;
        }

        public static string ConvertChars(string val, string type)
        {
            var tmp = val;

            tmp = val.Replace("|", "ѓ").Replace("h", "х").Replace("~", "ч");


            switch (type)
            {
                case "lower":
                    tmp = tmp.ToLower();
                    break;
                case "upper":
                    tmp = tmp.ToUpper();
                    break;
                case "firstUpper":
                    tmp = tmp.ToLower();
                    tmp = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tmp);
                    break;
            }

            var index = tmp.ToLower().IndexOf(" мк");
            if (index == -1)
                index = tmp.ToLower().IndexOf(" mk");
            if (index > -1)
            {
                if ((index + 3) >= tmp.Length)
                {
                    var aStringBuilder = new StringBuilder(tmp);
                    aStringBuilder.Remove(index, 3);
                    aStringBuilder.Insert(index, " МК");

                    tmp = aStringBuilder.ToString();
                }
            }

            return tmp;
        }

        public static string GetBankFromZS(string vodecki_broj)
        {
            switch (vodecki_broj)
            {
                case "100":
                    return "Народна банка на Република Македонија";
                case "200":
                    return "Стопанска Банка АД Скопје";
                case "210":
                    return "НЛБ Тутунска Банка АД Скопје";
                case "240":
                    return "Универзална Инвестициона Банка а.д. Скопје";
                case "250":
                    return "Шпаркасе Банка Македонија  а.д. Скопје";
                case "270":
                    return "Халк Банка  АД Скопје";
                case "280":
                    return "Алфа Банка АД Скопје";
                case "290":
                    return "ТТК банка АД Скопје";
                case "300":
                    return "Комерцијална Банка АД Скопје";
                case "320":
                    return "Централна кооперативна банка АД Скопје";
                case "330":
                    return "Капитал банка АД Скопје";
                case "350":
                    return "Македонска Банка за подршка на развојот АД Скопје";
                case "360":
                    return "Поштенска Банка АД Скопје";
                case "500":
                    return "Стопанска Банка АД Битола";
                case "530":
                    return "Охридска Банка АД Охрид";
                case "370":
                    return "Еуростандард Банка А.Д. Скопје";
                case "380":
                    return "ПроКредит Банка АД Скопје";
                default:
                    return string.Empty;
            }
        }

        public static bool SaveImportAccount(Dictionary<string, string>[] data)
        {
            return Bonitet.Document.CRM_DocumentClass.SaveImportReportValues(data);
        }

        //report 2
        public static Attributes SetReport2Data(int UserID, string EMBS, int Year)
        {

            var res = DALHelper.GetShortReportValues(EMBS, Year);

            var company = DALHelper.GetCompanyByEMBS_BM(EMBS);

            var data = new Attributes();

            data.datum_izdavanje = DateTime.Now.ToShortDateString();

            data.celosen_naziv_firma = "";
            if (company.CelosenNazivNaSubjektot != null)
                data.celosen_naziv_firma = company.CelosenNazivNaSubjektot;

            data.adresa_firma = "";
            if (company.Sediste != null)
                data.adresa_firma = company.Sediste;

            data.embs = EMBS;

            data.edb = "";
            if (company.EdinstvenDanocenBroj != null)
                data.edb = company.EdinstvenDanocenBroj;
            else
            {
                var tmp = DALHelper.GetCompanyAddInfoByEMBS_BM(EMBS);

                if (tmp != null)
                    data.edb = tmp.DanocenBroj;
            }

            data.golemina_na_subjekt = "";
            if (company.GoleminaNaDelovniotSubjekt != null)
                data.golemina_na_subjekt = company.GoleminaNaDelovniotSubjekt;


            var dejnost = DALHelper.GetDejnostForCompany(company.PK_Subjekt);

            Bonitet.DAL.BiznisMreza.DejnostSifra company_dejnost = null;

            if (dejnost != null)
            {
                company_dejnost = dejnost;
            }

            if (company_dejnost != null)
                data.dejnost = company_dejnost.PrioritetnaDejnost.ToString();

            data.organizacionen_oblik = "";
            if (company.VidNaSubjektNaUpis != null)
                data.organizacionen_oblik = company.VidNaSubjektNaUpis;

            data.deponent_banka = "";
            data.ziro_smetka = "";
            if (company.ZiroSmetka != null && company.ZiroSmetka.Length > 2)
            {
                var vodecki_broj = company.ZiroSmetka.Substring(0, 3);

                data.deponent_banka = GetBankFromZS(vodecki_broj);

                data.ziro_smetka = company.ZiroSmetka;
            }

            if (CheckIfKeyExists(res, "Вкупни приходи"))
            {
                data.sr_vkupni_prihodi = FormatCurrency(res["Вкупни приходи"].Value);
            }
            if (CheckIfKeyExists(res, "Вкупни расходи"))
            {
                data.sr_vkupni_rashodi = FormatCurrency(res["Вкупни расходи"].Value);
            }
            if (CheckIfKeyExists(res, "Добивка за финансиска година"))
            {
                data.sr_dobivka_za_finansiska_godina = FormatCurrency(res["Добивка за финансиска година"].Value);
            }
            if (CheckIfKeyExists(res, "Загуба за финансиска година"))
            {
                data.sr_zaguba_za_finansiska_godina = FormatCurrency(res["Загуба за финансиска година"].Value);
            }
            if (CheckIfKeyExists(res, "Просечен број на вработени"))
            {
                data.sr_prosecen_broj_vraboteni = FormatCurrency(res["Просечен број на вработени"].Value);
            }
            if (CheckIfKeyExists(res, "НЕТЕКОВНИ СРЕДСТВА"))
            {
                data.sr_netekovni_sredstva = FormatCurrency(res["НЕТЕКОВНИ СРЕДСТВА"].Value);
            }
            if (CheckIfKeyExists(res, "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА"))
            {
                data.sr_odlozeni_danocni_obvrski_aktiva = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА"].Value);
            }
            if (CheckIfKeyExists(res, "ТЕКОВНИ СРЕДСТВА"))
            {
                data.sr_tekovni_sredstva = FormatCurrency(res["ТЕКОВНИ СРЕДСТВА"].Value);
            }
            if (CheckIfKeyExists(res, "ЗАЛИХИ"))
            {
                data.sr_zalihi = FormatCurrency(res["ЗАЛИХИ"].Value);
            }
            if (CheckIfKeyExists(res, "СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)"))
            {
                data.sr_sredstva_ili_grupi_za_otugjuvanje = FormatCurrency(res["СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)"].Value);
            }
            if (CheckIfKeyExists(res, "ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)"))
            {
                data.sr_plateni_trosoci_za_idni_periodi = FormatCurrency(res["ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)"].Value);
            }
            if (CheckIfKeyExists(res, "Д. ВКУПНА АКТИВА"))
            {
                data.sr_d_vkupna_aktiva = FormatCurrency(res["Д. ВКУПНА АКТИВА"].Value);
            }
            if (CheckIfKeyExists(res, "ГЛАВНИНА И РЕЗЕРВИ"))
            {
                data.sr_glavnina_i_rezervi = FormatCurrency(res["ГЛАВНИНА И РЕЗЕРВИ"].Value);
            }
            if (CheckIfKeyExists(res, "ОБВРСКИ"))
            {
                data.sr_obvrski = FormatCurrency(res["ОБВРСКИ"].Value);
            }
            if (CheckIfKeyExists(res, "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА"))
            {
                data.sr_odlozeni_danocni_obvrski_pasiva = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА"].Value);
            }
            if (CheckIfKeyExists(res, "ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)"))
            {
                data.sr_odlozeno_plakanje_trosoci = FormatCurrency(res["ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)"].Value);
            }
            if (CheckIfKeyExists(res, "ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА"))
            {
                data.sr_obvrski_po_osnov_na_netekovni_sredstva = FormatCurrency(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА"].Value);

            }
            if (CheckIfKeyExists(res, "Д. ВКУПНО ПАСИВА"))
            {
                data.sr_d_vkupna_pasiva = FormatCurrency(res["Д. ВКУПНО ПАСИВА"].Value);
            }



            if (CheckIfKeyExists(res, "Вкупни приходи_LastYear"))
            {
                data.sr_vkupni_prihodi_lastyear = FormatCurrency(res["Вкупни приходи_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "Вкупни расходи_LastYear"))
            {
                data.sr_vkupni_rashodi_lastyear = FormatCurrency(res["Вкупни расходи_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "Добивка за финансиска година_LastYear"))
            {
                data.sr_dobivka_za_finansiska_godina_lastyear = FormatCurrency(res["Добивка за финансиска година_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "Загуба за финансиска година_LastYear"))
            {
                data.sr_zaguba_za_finansiska_godina_lastyear = FormatCurrency(res["Загуба за финансиска година_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "Просечен број на вработени_LastYear"))
            {
                data.sr_prosecen_broj_vraboteni_lastyear = FormatCurrency(res["Просечен број на вработени_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "НЕТЕКОВНИ СРЕДСТВА_LastYear"))
            {
                data.sr_netekovni_sredstva_lastyear = FormatCurrency(res["НЕТЕКОВНИ СРЕДСТВА_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_LastYear"))
            {
                data.sr_odlozeni_danocni_obvrski_aktiva_lastyear = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ТЕКОВНИ СРЕДСТВА_LastYear"))
            {
                data.sr_tekovni_sredstva_lastyear = FormatCurrency(res["ТЕКОВНИ СРЕДСТВА_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_LastYear"))
            {
                data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear = FormatCurrency(res["СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_LastYear"].Value);

            }
            if (CheckIfKeyExists(res, "ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_LastYear"))
            {
                data.sr_plateni_trosoci_za_idni_periodi_lastyear = FormatCurrency(res["ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "Д. ВКУПНА АКТИВА_LastYear"))
            {
                data.sr_d_vkupna_aktiva_lastyear = FormatCurrency(res["Д. ВКУПНА АКТИВА_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ГЛАВНИНА И РЕЗЕРВИ_LastYear"))
            {
                data.sr_glavnina_i_rezervi_lastyear = FormatCurrency(res["ГЛАВНИНА И РЕЗЕРВИ_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ОБВРСКИ_LastYear"))
            {
                data.sr_obvrski_lastyear = FormatCurrency(res["ОБВРСКИ_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_LastYear"))
            {
                data.sr_odlozeni_danocni_obvrski_pasiva_lastyear = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_LastYear"))
            {
                data.sr_odlozeno_plakanje_trosoci_lastyear = FormatCurrency(res["ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_LastYear"].Value);
            }
            if (CheckIfKeyExists(res, "ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_LastYear"))
            {
                data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear = FormatCurrency(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_LastYear"].Value);

            }
            if (CheckIfKeyExists(res, "Д. ВКУПНО ПАСИВА_LastYear"))
            {
                data.sr_d_vkupna_pasiva_lastyear = FormatCurrency(res["Д. ВКУПНО ПАСИВА_LastYear"].Value);
            }


            if (data.sr_vkupni_prihodi != null && data.sr_vkupni_prihodi_lastyear != null)
                data.sr_vkupni_prihodi_ind = CalculateGrowth(Convert.ToDouble(data.sr_vkupni_prihodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_prihodi_lastyear.Replace(".", ""))).ToString();

            if (data.sr_vkupni_rashodi != null && data.sr_vkupni_rashodi_lastyear != null)
                data.sr_vkupni_rashodi_ind = CalculateGrowth(Convert.ToDouble(data.sr_vkupni_rashodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_rashodi_lastyear.Replace(".", ""))).ToString();

            if (data.sr_dobivka_za_finansiska_godina != null && data.sr_dobivka_za_finansiska_godina_lastyear != null)
                data.sr_dobivka_za_finansiska_godina_ind = CalculateGrowth(Convert.ToDouble(data.sr_dobivka_za_finansiska_godina.Replace(".", "")), Convert.ToDouble(data.sr_dobivka_za_finansiska_godina_lastyear.Replace(".", ""))).ToString();

            if (data.sr_zaguba_za_finansiska_godina != null && data.sr_zaguba_za_finansiska_godina_lastyear != null)
                data.sr_zaguba_za_finansiska_godina_ind = CalculateGrowth(Convert.ToDouble(data.sr_zaguba_za_finansiska_godina.Replace(".", "")), Convert.ToDouble(data.sr_zaguba_za_finansiska_godina_lastyear.Replace(".", ""))).ToString();

            if (data.sr_prosecen_broj_vraboteni != null && data.sr_prosecen_broj_vraboteni_lastyear != null)
                data.sr_prosecen_broj_vraboteni_ind = CalculateGrowth(Convert.ToDouble(data.sr_prosecen_broj_vraboteni.Replace(".", "")), Convert.ToDouble(data.sr_prosecen_broj_vraboteni_lastyear.Replace(".", ""))).ToString();

            if (data.sr_netekovni_sredstva != null && data.sr_netekovni_sredstva_lastyear != null)
                data.sr_netekovni_sredstva_ind = CalculateGrowth(Convert.ToDouble(data.sr_netekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_netekovni_sredstva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_aktiva != null && data.sr_odlozeni_danocni_obvrski_aktiva_lastyear != null)
                data.sr_odlozeni_danocni_obvrski_aktiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_tekovni_sredstva != null && data.sr_tekovni_sredstva_lastyear != null)
                data.sr_tekovni_sredstva_ind = CalculateGrowth(Convert.ToDouble(data.sr_tekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_tekovni_sredstva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_zalihi != null && data.sr_zalihi_lastyear != null)
                data.sr_zalihi_ind = CalculateGrowth(Convert.ToDouble(data.sr_zalihi.Replace(".", "")), Convert.ToDouble(data.sr_zalihi_lastyear.Replace(".", ""))).ToString();

            if (data.sr_sredstva_ili_grupi_za_otugjuvanje != null && data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear != null)
                data.sr_sredstva_ili_grupi_za_otugjuvanje_ind = CalculateGrowth(Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje.Replace(".", "")), Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear.Replace(".", ""))).ToString();

            if (data.sr_plateni_trosoci_za_idni_periodi != null && data.sr_plateni_trosoci_za_idni_periodi_lastyear != null)
                data.sr_plateni_trosoci_za_idni_periodi_ind = CalculateGrowth(Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi.Replace(".", "")), Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi_lastyear.Replace(".", ""))).ToString();

            if (data.sr_d_vkupna_aktiva != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_d_vkupna_aktiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_glavnina_i_rezervi != null && data.sr_glavnina_i_rezervi_lastyear != null)
                data.sr_glavnina_i_rezervi_ind = CalculateGrowth(Convert.ToDouble(data.sr_glavnina_i_rezervi.Replace(".", "")), Convert.ToDouble(data.sr_glavnina_i_rezervi_lastyear.Replace(".", ""))).ToString();

            if (data.sr_obvrski != null && data.sr_obvrski_lastyear != null)
                data.sr_obvrski_ind = CalculateGrowth(Convert.ToDouble(data.sr_obvrski.Replace(".", "")), Convert.ToDouble(data.sr_obvrski_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_pasiva != null && data.sr_odlozeni_danocni_obvrski_pasiva_lastyear != null)
                data.sr_odlozeni_danocni_obvrski_pasiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva.Replace(".", "")), Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeno_plakanje_trosoci != null && data.sr_odlozeno_plakanje_trosoci_lastyear != null)
                data.sr_odlozeno_plakanje_trosoci_ind = CalculateGrowth(Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci.Replace(".", "")), Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci_lastyear.Replace(".", ""))).ToString();

            if (data.sr_obvrski_po_osnov_na_netekovni_sredstva != null && data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear != null)
                data.sr_obvrski_po_osnov_na_netekovni_sredstva_ind = CalculateGrowth(Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_d_vkupna_pasiva != null && data.sr_d_vkupna_pasiva_lastyear != null)
                data.sr_d_vkupna_pasiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_d_vkupna_pasiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_pasiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_netekovni_sredstva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_netekovni_sredstva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_netekovni_sredstva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_netekovni_sredstva != null && data.sr_d_vkupna_aktiva != null)
                data.sr_netekovni_sredstva_procent = CalculatePercent(Convert.ToDouble(data.sr_netekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_aktiva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_odlozeni_danocni_obvrski_aktiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_aktiva != null && data.sr_d_vkupna_aktiva != null)
                data.sr_odlozeni_danocni_obvrski_aktiva_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_aktiva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_tekovni_sredstva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_aktiva != null && data.sr_d_vkupna_aktiva != null)
                data.sr_tekovni_sredstva_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_zalihi_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_zalihi_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_zalihi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_zalihi != null && data.sr_d_vkupna_aktiva != null)
                data.sr_zalihi_procent = CalculatePercent(Convert.ToDouble(data.sr_zalihi.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_sredstva_ili_grupi_za_otugjuvanje_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_sredstva_ili_grupi_za_otugjuvanje != null && data.sr_d_vkupna_aktiva != null)
                data.sr_sredstva_ili_grupi_za_otugjuvanje_procent = CalculatePercent(Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_plateni_trosoci_za_idni_periodi_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_plateni_trosoci_za_idni_periodi_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_plateni_trosoci_za_idni_periodi != null && data.sr_d_vkupna_aktiva != null)
                data.sr_plateni_trosoci_za_idni_periodi_procent = CalculatePercent(Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_d_vkupna_aktiva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_d_vkupna_aktiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_d_vkupna_aktiva != null && data.sr_d_vkupna_aktiva != null)
                data.sr_d_vkupna_aktiva_procent = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_glavnina_i_rezervi_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_glavnina_i_rezervi_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_glavnina_i_rezervi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_glavnina_i_rezervi != null && data.sr_d_vkupna_aktiva != null)
                data.sr_glavnina_i_rezervi_procent = CalculatePercent(Convert.ToDouble(data.sr_glavnina_i_rezervi.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_obvrski_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_obvrski_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_obvrski_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_obvrski != null && data.sr_d_vkupna_aktiva != null)
                data.sr_obvrski_procent = CalculatePercent(Convert.ToDouble(data.sr_obvrski.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_pasiva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_odlozeni_danocni_obvrski_pasiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeni_danocni_obvrski_pasiva_lastyear != null && data.sr_d_vkupna_aktiva != null)
                data.sr_odlozeni_danocni_obvrski_pasiva_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_odlozeno_plakanje_trosoci_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_odlozeno_plakanje_trosoci_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_odlozeno_plakanje_trosoci != null && data.sr_d_vkupna_aktiva != null)
                data.sr_odlozeno_plakanje_trosoci_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_obvrski_po_osnov_na_netekovni_sredstva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear != null && data.sr_d_vkupna_aktiva != null)
                data.sr_obvrski_po_osnov_na_netekovni_sredstva_procent = CalculatePercent(Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_d_vkupna_pasiva_lastyear != null && data.sr_d_vkupna_aktiva_lastyear != null)
                data.sr_d_vkupna_pasiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_pasiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();

            if (data.sr_d_vkupna_pasiva != null && data.sr_d_vkupna_aktiva != null)
                data.sr_d_vkupna_pasiva_procent = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_pasiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            if (data.sr_vkupni_rashodi != null && data.sr_vkupni_rashodi_lastyear != null)
                data.bar_chart_filename = DocumentChart.CreateBarChart(Convert.ToDouble(data.sr_vkupni_rashodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_rashodi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_prihodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_prihodi_lastyear.Replace(".", "")));

            var sr_tekovni_sredstva = 0.0;
            var sr_netekovni_sredstva = 0.0;
            var sr_sredstva_ili_grupi_za_otugjuvanje = 0.0;
            var sr_odlozeni_danocni_obvrski_aktiva = 0.0;
            var sr_plateni_trosoci_za_idni_periodi = 0.0;
            if (data.sr_tekovni_sredstva != null)
                sr_tekovni_sredstva = Convert.ToDouble(data.sr_tekovni_sredstva.Replace(".", ""));
            if (data.sr_netekovni_sredstva != null)
                sr_netekovni_sredstva = Convert.ToDouble(data.sr_netekovni_sredstva.Replace(".", ""));
            if (data.sr_sredstva_ili_grupi_za_otugjuvanje != null)
                sr_sredstva_ili_grupi_za_otugjuvanje = Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje.Replace(".", ""));
            if (data.sr_odlozeni_danocni_obvrski_aktiva != null)
                sr_odlozeni_danocni_obvrski_aktiva = Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", ""));
            if (data.sr_plateni_trosoci_za_idni_periodi != null)
                sr_plateni_trosoci_za_idni_periodi = Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi.Replace(".", ""));

            data.pie_chart_filename1 = DocumentChart.CreatePieChart(1,
                sr_tekovni_sredstva,
                sr_netekovni_sredstva,
                sr_sredstva_ili_grupi_za_otugjuvanje,
                sr_odlozeni_danocni_obvrski_aktiva,
                sr_plateni_trosoci_za_idni_periodi, 0.0);

            var sr_obvrski = 0.0;
            var sr_glavnina_i_rezervi = 0.0;
            var sr_obvrski_po_osnov_na_netekovni_sredstva = 0.0;
            var sr_odlozeni_danocni_obvrski_pasiva = 0.0;
            var sr_odlozeno_plakanje_trosoci = 0.0;
            if (data.sr_obvrski != null)
                sr_obvrski = Convert.ToDouble(data.sr_obvrski.Replace(".", ""));
            if (data.sr_glavnina_i_rezervi != null)
                sr_glavnina_i_rezervi = Convert.ToDouble(data.sr_glavnina_i_rezervi.Replace(".", ""));
            if (data.sr_obvrski_po_osnov_na_netekovni_sredstva != null)
                sr_obvrski_po_osnov_na_netekovni_sredstva = Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva.Replace(".", ""));
            if (data.sr_odlozeni_danocni_obvrski_pasiva != null)
                sr_odlozeni_danocni_obvrski_pasiva = Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva.Replace(".", ""));
            if (data.sr_odlozeno_plakanje_trosoci != null)
                sr_odlozeno_plakanje_trosoci = Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci.Replace(".", ""));

            data.pie_chart_filename2 = DocumentChart.CreatePieChart(2,
                sr_obvrski,
                sr_glavnina_i_rezervi,
                sr_obvrski_po_osnov_na_netekovni_sredstva,
                sr_odlozeni_danocni_obvrski_pasiva,
                sr_odlozeno_plakanje_trosoci, 0.0);

            return data;
        }

        public static bool CheckIfKeyExists(Dictionary<string, c_ReportValues> res, string key)
        {
            if (res.ContainsKey(key))
            {
                return true;
            }
            return false;
        }

        public static string SetReport3Data_Live(string EMBS, int UserID, string ticket)
        {
            var res = CRM.CRM_ServiceHelper.GetLiveCRM_AccountStatus(EMBS, UserID, ticket);

            Bonitet.DAL.DALHelper.InsertCrmResponse(EMBS, 3, res);

            var html = Bonitet.Document.CRM_DocumentClass.PopulateTemplate(res);

            return html;
        }

        public static string SetReport3Data(string EMBS)
        {
            var res = CRM.CRM_ServiceHelper.GetCRM_AccountStatus(EMBS);

            var html = Bonitet.Document.CRM_DocumentClass.PopulateTemplate(res);

            return html;
        }
        private static string FormatCurrency(string Number)
        {
            long a = 0;
            if (long.TryParse(Number, out a))
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("mk-MK"), "{0:N0}", a);

            return Number;
        }
        private static string FormatCurrencyComma(string Number)
        {
            long a = 0;

            int index = Number.IndexOf(".");
            if (index > 0)
                Number = Number.Substring(0, index);

            if (long.TryParse(Number, out a))
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:N0}", a);

            return Number;
        }

        private static string CalculateGrowth(double CurYearValue, double LastYearValue)
        {
            var res = Math.Round((CurYearValue / LastYearValue - 1) * 100, 2, MidpointRounding.AwayFromZero);

            if (LastYearValue == 0.00)
                return "0,00";
            if (res == 0)
                return "0,00";
            return res.ToString().Replace(".", ",");
        }

        private static string CalculatePercent(double A, double B)
        {
            var res = Math.Round((A / B * 100), 2, MidpointRounding.AwayFromZero);
            if (B == 0.0)
                return "0,00";
            if (res == 0)
                return "0,00";
            return res.ToString().Replace(".", ",");
        }

        private static string ShortReportFooter(int page, string datum)
        {
            var html = "<div class=\"footer\">" +
                        "       <div class=\"left_text\">" +
                        "           <p>" + datum + "</p>" +
                        "           <p>Е-бонитет.мк</p>" +
                        "       </div>" +
                        "       <div class=\"center_text\">" +
                        "           <p><b>Ебонитети.мк - паметна деловна одлука</b></p>" +
                        "       </div>" +
                        "       <div class=\"right_text\">" +
                        "           <p>" + page + "</p>" +
                        "       </div>" +
                        "</div>";


            return html;
        }

        private static string ShortReportPage(Attributes data)
        {
            var HTML = "<!DOCTYPE html>" +
                        "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                        "    <head>" +
                        "        <title></title>" +
                        "        <style>body{height:auto;width:1040px;padding:0;margin:0 auto;font-family:Calibri;font-size:16px;}.header_img{height:100px;float:left;}h1{text-align:center;float:left;margin-top:200px;font-size:28px;}.fill_top{height:150px;}.fill_left{width:70px;}.page1,.page2{height:1470px;width:1040px;padding:0;margin:0;float:left}#page_wrapper{width:100%;height:770px;float:left;background-color:#fff}.table{width:960px;margin:0;padding:0;border-spacing:0}.left_border{border-left:4px solid #000}.right_border{border-right:4px solid #000}.top_border{border-top:4px solid #000}.bottom_border{border-bottom:4px solid #000}.gray{color:#515151}.black{color:#000}.bold_text{font-weight:700}.align_left{text-align:left}.align_center{text-align:center}.align_right{text-align:right}.text_uppercase{text-transform:uppercase}.big_text{font-size:18px}.first{width:438px}.second,.third{width:98px}.second_third{width:198px}.forth{width:98px}.fifth,.sixth{width:98px}.fifth_sixth{width:198px}.white_space{height:20px}.image_wrapper{margin-left:70px;text-align:center}.image_wrapper img{float:left}.spacer{height:100px;width:100%}.spacer2{height:20px;width:100%}.footer{width:90%;height:auto;height:40px;padding:0 5%;}.footer p{margin:0;padding:0;color:#000;font-weight:700}.left_text,.right_text{text-align:left;width:20%;float:left}.right_text{margin-top:15px;text-align:right}.center_text{text-align:center;width:60%;float:left;margin-top:15px}.row_wrapper{width:100%;height:370px;margin-bottom:5px}.legend_table{width:600px;margin:0;padding:0;border-spacing:0;margin-left:215px;}.legend_table tr td{border-width:1px!important;border-color:#ccc!important}.legend_table span{width:10px;height:10px;margin-top:4px;margin-left:4px;float:left}.legend_table .blue{background-color:rgba(30,144,255,1)}.legend_table .red{background-color:rgba(255,69,0,1)}.legend_table .fixed_width{width:60px}.chart_text{float:left;width:250px;height:200px;position:relative}#trosoci{position:absolute;top:200px; left:170px;}#prihodi{position:absolute;top:270px; left:170px;}#title_chart1{position:absolute;top:0;left:350px;width:265px;font-weight:400;font-size:28px}#title_chart2,#title_chart3{width:100%;text-align:center;font-weight:400;font-size:28px}.image_wrapper.no_float img{clear:both;float:none}.spacer_top{margin-top:50px;}</style>" +
                        "    </head>" +
                        "    <body>" +
                        "        <div class=\"page1\">" +
                        "            <div id=\"page_wrapper\">" +
                        "               <div style=\"margin-top:50px;width:30%;float:left;margin-left:70px;\" class=\"header_img_wrapper\"><img height=\"100\" src=\"" + AbsoluteUrlPath + "\\img\\target_logo.png\" class=\"header_img\"/></div>" +
                        "               <h1>Преглед на субјект</h1>" +
                        "               <div style=\"margin-top:50px;width:30%;float:right;margin-right:70px;\" class=\"header_img_wrapper\"><img height=\"100\" src=\"" + AbsoluteUrlPath + "\\img\\ebonitet_logo.png\" class=\"header_img\"/></div>" +
                        "                <table class=\"table\">" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black top_border left_border\">Назив на правното лице</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray top_border right_border\">" + data.celosen_naziv_firma + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Место</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.adresa_firma + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Матичен број</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.embs + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Даночен број</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.edb + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Големина</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.golemina_na_subjekt + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Дејност</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.dejnost + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Организационен облик</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.organizacionen_oblik + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border\">Депонент банка</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border\">" + data.deponent_banka + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bold_text align_left black left_border bottom_border\">Жиро с-ка</td>" +
                        "                        <td colspan=\"5\" class=\"align_left gray right_border bottom_border\">" + data.ziro_smetka + "</td>" +
                        "                    </tr>" +
                        "                   <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                       <td class=\"white_space \"></td>" +
                        "                   </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td colspan=\"6\" class=\"first big_text bold_text align_center black left_border right_border top_border text_uppercase\">Биланс на успех</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first bottom_border left_border\"></td>" +
                        "                        <td colspan=\"2\" class=\"second_third gray bold_text align_center bottom_border\">Износ во денари</td>" +
                        "                        <td colspan=\"3\"class=\"forth gray bold_text align_center bottom_border right_border\">Раст</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first  bold_text align_left black text_uppercase left_border\">Приходи</td>" +
                        "                        <td class=\"second bold_text gray align_center\">" + data.last_year + "</td>" +
                        "                        <td class=\"third bold_text gray align_center\">" + data.this_year + "</td>" +
                        "                        <td colspan=\"3\" class=\"right_border\"></td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first align_left gray left_border\">Вкупни приходи</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_vkupni_prihodi_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_vkupni_prihodi + "</td>" +
                        "                        <td colspan=\"3\" class=\"gray align_center right_border\">" + data.sr_vkupni_prihodi_ind + "%</td>" +
                        "                    </tr>" +
                        "                   <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                       <td colspan=\"6\" class=\"white_space left_border right_border\"></td>" +
                        "                   </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td colspan=\"6\" class=\"first  bold_text align_left black text_uppercase left_border right_border\">Трошоци</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first align_left gray left_border\">Вкупни расходи</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_vkupni_rashodi_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_vkupni_rashodi + "</td>" +
                        "                        <td colspan=\"3\" class=\"gray align_center right_border\">" + data.sr_vkupni_rashodi_ind + "%</td>" +
                        "                    </tr>" +
                        "                   <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                       <td colspan=\"6\" class=\"white_space bottom_border top_border left_border right_border\"></td>" +
                        "                   </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td colspan=\"6\" class=\"first  bold_text align_left black text_uppercase left_border right_border\">Финансиски резултат</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first align_left gray left_border\">Добивка за финансиска година</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_dobivka_za_finansiska_godina_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_dobivka_za_finansiska_godina + "</td>" +
                        "                        <td colspan=\"3\" class=\"gray align_center right_border\">" + data.sr_dobivka_za_finansiska_godina_ind + "%</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first align_left gray left_border\">Загуба за финансиска година</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_zaguba_za_finansiska_godina_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_zaguba_za_finansiska_godina + "</td>" +
                        "                        <td colspan=\"3\" class=\"gray align_center right_border\">" + data.sr_zaguba_za_finansiska_godina_ind + "%</td>" +
                        "                    </tr>" +
                        "                   <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                       <td colspan=\"6\" class=\"white_space bottom_border top_border left_border right_border\"></td>" +
                        "                   </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first align_left gray left_border bottom_border\">Просечен број на вработени</td>" +
                        "                        <td class=\"second gray align_right bottom_border\">" + data.sr_prosecen_broj_vraboteni_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right bottom_border\">" + data.sr_prosecen_broj_vraboteni + "</td>" +
                        "                        <td colspan=\"3\" class=\"gray align_center right_border bottom_border\">" + data.sr_prosecen_broj_vraboteni_ind + "%</td>" +
                        "                    </tr>" +
                        "                   <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                       <td class=\"white_space \"></td>" +
                        "                   </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td colspan=\"6\" class=\"first big_text bold_text align_center black  top_border left_border right_border text_uppercase\">Биланс на состојба</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first left_border bottom_border\"></td>" +
                        "                        <td colspan=\"2\" class=\"second_third gray bold_text align_center bottom_border\">Износ во денари</td>" +
                        "                        <td class=\"forth gray bold_text align_center bottom_border\">Раст</td>" +
                        "                        <td colspan=\"2\" class=\"fifth_sixth gray bold_text align_center right_border bottom_border\">Учество во %</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first  bold_text align_left black text_uppercase left_border\">Актива:</td>" +
                        "                        <td class=\"second bold_text black align_center\">" + data.last_year + "</td>" +
                        "                        <td class=\"third bold_text black align_center\">" + data.this_year + "</td>" +
                        "                        <td class=\"forth gray align_center\">%</td>" +
                        "                        <td class=\"fifth bold_text black align_center\">" + data.last_year + "</td>" +
                        "                        <td class=\"sixth bold_text black align_center right_border\">" + data.this_year + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">Нетековни средства</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_netekovni_sredstva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_netekovni_sredstva + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_netekovni_sredstva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_netekovni_sredstva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_netekovni_sredstva_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">Одложени даночни обврски</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_odlozeni_danocni_obvrski_aktiva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_odlozeni_danocni_obvrski_aktiva + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_odlozeni_danocni_obvrski_aktiva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_odlozeni_danocni_obvrski_aktiva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_odlozeni_danocni_obvrski_aktiva_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">Тековни средства</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_tekovni_sredstva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_tekovni_sredstva + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_tekovni_sredstva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_tekovni_sredstva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_tekovni_sredstva_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">Залихи</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_zalihi_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_zalihi + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_zalihi_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_zalihi_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_zalihi_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_sredstva_ili_grupi_za_otugjuvanje + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_sredstva_ili_grupi_za_otugjuvanje + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_sredstva_ili_grupi_za_otugjuvanje_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_sredstva_ili_grupi_za_otugjuvanje_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">АВР</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_plateni_trosoci_za_idni_periodi_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_plateni_trosoci_za_idni_periodi + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_plateni_trosoci_za_idni_periodi_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_plateni_trosoci_za_idni_periodi_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_plateni_trosoci_za_idni_periodi_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">Д. ВКУПНА АКТИВА</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_d_vkupna_aktiva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_d_vkupna_aktiva + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_d_vkupna_aktiva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_d_vkupna_aktiva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_d_vkupna_aktiva_procent + "</td>" +
                        "                    </tr>" +
                        "                   <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                       <td colspan=\"6\" class=\"white_space bottom_border top_border left_border right_border\"></td>" +
                        "                   </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td colspan=\"6\" class=\"first  bold_text align_left black text_uppercase left_border right_border\">Пасива: </td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">ГЛАВНИНА И РЕЗЕРВИ</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_glavnina_i_rezervi_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_glavnina_i_rezervi + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_glavnina_i_rezervi_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_glavnina_i_rezervi_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_glavnina_i_rezervi_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">ОБВРСКИ</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_obvrski_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_obvrski + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_obvrski_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_obvrski_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_obvrski_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_odlozeni_danocni_obvrski_pasiva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_odlozeni_danocni_obvrski_pasiva + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_odlozeni_danocni_obvrski_pasiva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_odlozeni_danocni_obvrski_pasiva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_odlozeni_danocni_obvrski_pasiva_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">ПВР</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_odlozeno_plakanje_trosoci_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_odlozeno_plakanje_trosoci + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_odlozeno_plakanje_trosoci_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_odlozeno_plakanje_trosoci_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_odlozeno_plakanje_trosoci_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border\">ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА</td>" +
                        "                        <td class=\"second gray align_right\">" + data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right\">" + data.sr_obvrski_po_osnov_na_netekovni_sredstva + "</td>" +
                        "                        <td class=\"forth gray align_center\">" + data.sr_obvrski_po_osnov_na_netekovni_sredstva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center\">" + data.sr_obvrski_po_osnov_na_netekovni_sredstva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center right_border\">" + data.sr_obvrski_po_osnov_na_netekovni_sredstva_procent + "</td>" +
                        "                    </tr>" +
                        "                    <tr>" +
                        "                        <td class=\"fill_left\"></td>" +
                        "                        <td class=\"first text_uppercase align_left gray left_border bottom_border\">Д. ВКУПНО ПАСИВА</td>" +
                        "                        <td class=\"second gray align_right last_td bottom_border\">" + data.sr_d_vkupna_pasiva_lastyear + "</td>" +
                        "                        <td class=\"third gray align_right last_td bottom_border\">" + data.sr_d_vkupna_pasiva + "</td>" +
                        "                        <td class=\"forth gray align_center last_td bottom_border\">" + data.sr_d_vkupna_pasiva_ind + "</td>" +
                        "                        <td class=\"fifth gray align_center last_td bottom_border\">" + data.sr_d_vkupna_pasiva_procent_lastyear + "</td>" +
                        "                        <td class=\"sixth gray align_center last_td right_border bottom_border\">" + data.sr_d_vkupna_pasiva_procent + "</td>" +
                        "                    </tr>" +
                        "                </table>" +
                        "                <div class=\"spacer\"></div>" +
                                         ShortReportFooter(1, data.datum_izdavanje) +
                        "            </div>" +
                        "        </div>" +
                        "       <div class=\"page2\">" +
                        "           <div class=\"page_wrapper\">" +
                        "               <div class=\"row_wrapper\">" +
                        "                   <div class=\"image_wrapper spacer_top\">" +
                        "                       <div class=\"chart_text\">" +
                        "                           <p id=\"trosoci\">ТРОШОЦИ</p>" +
                        "                           <p id=\"prihodi\">ПРИХОДИ</p>" +
                        "                           <p id=\"title_chart1\">ПРИХОДИ / ТРОШОЦИ</p>" +
                        "                       </div>" +
                        "                       <img style=\"margin-top:150px;\" src=\"" + AbsolutePath + "\\chart_images\\" + data.bar_chart_filename + "\" />" +
                        "                   </div>" +
                        "                   <table class=\"legend_table\">" +
                        "                       <tr>" +
                        "                           <td class=\"white_space\"></td>" +
                        "                           <td></td>" +
                        "                           <td class=\"align_center top_border left_border right_border\">ПРИХОДИ</td>" +
                        "                           <td class=\"align_center right_border top_border\">ТРОШОЦИ</td>" +
                        "                       </tr>" +
                        "                       <tr>" +
                        "                           <td class=\"white_space\"></td>" +
                        "                           <td class=\"fixed_width align_center top_border left_border\"><span class=\"blue\"></span>" + data.last_year + "</td>" +
                        "                           <td class=\"align_center top_border left_border right_border\">" + data.sr_vkupni_prihodi_lastyear + "</td>" +
                        "                           <td class=\"align_center right_border top_border\">" + data.sr_vkupni_rashodi_lastyear + "</td>" +
                        "                       </tr>" +
                        "                       <tr>" +
                        "                           <td class=\"white_space\"></td>" +
                        "                           <td class=\"fixed_width align_center top_border left_border bottom_border\"><span class=\"red\"></span>" + data.this_year + "</td>" +
                        "                           <td class=\"align_center top_border left_border right_border bottom_border\">" + data.sr_vkupni_prihodi + "</td>" +
                        "                           <td class=\"align_center right_border top_border bottom_border\">" + data.sr_vkupni_rashodi + "</td>" +
                        "                       </tr>" +
                        "                   </table>" +
                        "               </div>" +
                        "               <div class=\"row_wrapper spacer_top\" style=\"height:auto;\">" +
                        "                   <div class=\"image_wrapper no_float\">" +
                        "                       <p id=\"title_chart2\" style=\"position:relative;z-index:100;margin-top:140px;margin-bottom:0;\">АКТИВА</p>" +
                        "                       <img style=\"position:relative;margin-top:0px;z-index:1;\"src=\"" + AbsolutePath + "\\chart_images\\" + data.pie_chart_filename1 + "\" />" +
                        "                   </div>" +
                        "               </div>" +
                        "               <div class=\"row_wrapper\" style=\"height:auto;\">" +
                        "                   <div class=\"image_wrapper no_float\">" +
                        "                       <p style=\"margin:0;position:relative;z-index:100;\" id=\"title_chart3\">ПАСИВА</p>" +
                        "                       <img style=\"position:relative;margin-top:0px;z-index:1;\"src=\"" + AbsolutePath + "\\chart_images\\" + data.pie_chart_filename2 + "\" />" +
                        "                   </div>" +
                        "               </div>" +
                        "                <div class=\"spacer2\"></div>" +
                                        ShortReportFooter(2, data.datum_izdavanje) +
                        "           </div>" +
                        "       </div>" +
                        "    </body>" +
                        "</html>";

            return HTML;
        }

        private static string BankarskiSmetkiHTML(Attributes data)
        {
            var bankarski_smetki = data.bankarski_smetki;

            var HTML = "";

            if (bankarski_smetki != null)
            {
                var i = 1;
                foreach (var item in bankarski_smetki)
                {
                    if (i > 12)
                        break;
                    HTML += "<p class=\"border_bottom\"><span>" + item.Smetka + "</span><p>" + item.Ime + "</p></p>";
                    i++;
                }
            }
            if (bankarski_smetki == null || bankarski_smetki.Count() == 0)
                HTML += "<p>Нема податоци за банкарски сметки</p>";
            return HTML;
        }

        public static string BankarskiSmetkiHTMLAdditional(Attributes data, int page)
        {
            var tmp = data.bankarski_smetki.Skip(12).ToList();
            var bankarski_smetki = tmp.Skip((page * 24)).Take(24).ToList();

            var HTML = "";

            foreach (var item in bankarski_smetki)
            {
                HTML += "<p style=\"margin-bottom:10px;\" class=\"border_bottom\"><span>" + item.Smetka + "</span><p>" + item.Ime + "</p></p>";
            }

            return HTML;
        }

        public static int TotalBankarskiSmetki(Attributes data)
        {

            var bankarski_smetki = data.bankarski_smetki;

            var HTML = "";
            var total = 0;
            if (bankarski_smetki != null)
            {
                total = bankarski_smetki.Count();
            }
            return total;
        }

        public static bool IsAkcionerskoDrustvo(Attributes data)
        {
            if (data.pravna_forma.ToLower() == "ад")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsDOOEL(Attributes data)
        {
            if (data.pravna_forma.ToLower() == "дооел")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void GetGlobalTotals(Attributes data)
        {
            data.tekovni_podruznici = "0";
            if (IsAkcionerskoDrustvo(data))
            {
                data.tekovni_sopstvenici = "0 (АД)";
            }
            else
            {
                if (data.ovlasteni_lica != null)
                    data.tekovni_sopstvenici = TotalSopstvenici(data).ToString();
            }

            if (data.organizacioni_edinici != null)
                data.tekovni_podruznici = data.organizacioni_edinici.Count().ToString();

            data.promeni_minata_godina = "0";
            if (data.promeni != null)
                data.promeni_minata_godina = TotalPromeni(data).ToString();

            data.solventnost_minata_godina = "0";
            if (data.solventnost != null)
                data.solventnost_minata_godina = TotalSolventnost(data).ToString();

            data.kazni_minata_godina = "0";
            if (data.kazni_i_sankcii != null)
                data.kazni_minata_godina = TotalKazniSankcii(data).ToString();
        }

        public static int TotalKazniSankcii(Attributes data)
        {
            var kazni_i_sankcii = data.kazni_i_sankcii;
            var total = 0;
            if (kazni_i_sankcii != null)
                total = kazni_i_sankcii.Count();

            return total;
        }

        public static int TotalSolventnost(Attributes data)
        {

            var solventnost = data.solventnost;
            var total = 0;
            if (solventnost != null)
                total = solventnost.Count();

            return total;
        }
        public static int TotalPromeni(Attributes data)
        {

            var promeni = data.promeni;
            var total = 0;
            if (promeni != null)
                total = promeni.Count();

            return total;
        }

        private static string PromeniHTML(Attributes data, int page)
        {
            var promeni = data.promeni;

            var HTML = "";

            if (promeni != null)
            {
                var i = (page * 25) + 1;
                foreach (var item in promeni.Skip((page * 25)).Take(25))
                {
                    HTML += "<tr>" +
                                "<td>" +
                                    "<p>" + i + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + FormatStringDate(item.OdobruvanjeNaPrijava.ToString()) + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + item.VidNaPromena + "</p>" +
                                "</td>" +
                            "</tr>";
                    i++;
                }
            }
            if (promeni == null || promeni.Count() == 0)
                HTML += "<tr><td>1</td><td>" + FormatStringDate(DateTime.Now.ToString()) + "</td><td colspan=\"2\">Не се евидентирани промени во последните 365 дена</td></tr>";

            return HTML;
        }

        private static string SolventnostHTML(Attributes data, int page)
        {
            var solventnost = data.solventnost;

            var HTML = "";

            if (solventnost != null)
            {
                var i = (page * 25) + 1;
                foreach (var item in solventnost.Skip((page * 25)).Take(25))
                {
                    HTML += "<tr>" +
                                "<td>" +
                                    "<p>" + i + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + FormatStringDate(item.OdobruvanjeNaPrijava) + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + item.TipNaPostapka + "</p>" +
                                "</td>" +
                            "</tr>";
                    i++;
                }
            }
            if (solventnost == null || solventnost.Count() == 0)
                HTML += "<tr><td>1</td><td>" + FormatStringDate(DateTime.Now.ToString()) + "</td><td colspan=\"2\">Не се евидентирани промени во солвентноста на компанијата во последните 365 дена</td></tr>";

            return HTML;
        }

        private static string KazniSankciiHTML(Attributes data, int page)
        {
            var kazni_i_sankcii = data.kazni_i_sankcii;

            var HTML = "";

            if (kazni_i_sankcii != null)
            {
                var i = (page * 25) + 1;
                foreach (var item in kazni_i_sankcii.Skip((page * 25)).Take(25))
                {
                    HTML += "<tr>" +
                                "<td>" +
                                    "<p>" + i + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + FormatStringDate(item.OdobruvanjeNaPrijava) + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + item.PovrzanostStatusVoPKD + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + item.TipNaPostapka + "</p>" +
                                "</td>" +
                            "</tr>";
                    i++;
                }
            }
            if (kazni_i_sankcii == null || kazni_i_sankcii.Count() == 0)
                HTML += "<tr><td>1</td><td>" + FormatStringDate(DateTime.Now.ToString()) + "</td><td colspan=\"2\">Не се евидентирани казни и санкции во последните 365 дена</td></tr>";

            return HTML;
        }

        private static string OvlasteniLicaShortHTML(Attributes data)
        {
            data.IndexHelper = 0;
            var ovlasteni_lica = data.ovlasteni_lica;

            var HTML = "";

            if (ovlasteni_lica != null)
            {
                var tmpData = new Dictionary<string, string>();
                var i = 1;

                foreach (var item in ovlasteni_lica)
                {
                    if (IsAkcionerskoDrustvo(data))
                    {
                        if (item.IsOwner == FL_TipPovrzanost.Sopstvenik || item.IsOwner == FL_TipPovrzanost.OvlastenoLice)
                        {
                            data.IndexHelper++;
                            continue;
                        }
                    }
                    if (i > 8)
                        break;

                    if (tmpData.ContainsKey(item.Ime))
                    {
                        tmpData[item.Ime] += ", " + item.Povrzanost;
                    }
                    else
                    {
                        tmpData.Add(item.Ime, item.Povrzanost);
                    }
                    data.IndexHelper++;
                    i++;
                }
                foreach (var item in tmpData)
                {
                    HTML += "<p class=\"border_bottom\"><span>" + item.Key + "</span><p>" + item.Value + "</p></p>";
                }
                if (ovlasteni_lica.Count() > 8)
                {
                    HTML += "<p class=\"border_bottom\"><span>Сопственици</span><p>Во прилог на бонитетниот извештај</p></p>";
                }
            }

            if (ovlasteni_lica == null || ovlasteni_lica.Count() == 0)
                HTML += "<p>Нема податоци за овластени лица</p>";
            return HTML;
        }

        private static string OvlasteniLicaShortHTMLAdditional(Attributes data, int page)
        {
            var HTML = "";
            var tmp = data.ovlasteni_lica.Skip(data.IndexHelper).ToList();

            var ovlasteni_lica = tmp.Skip((page * 20)).Take(20).ToList();

            var HTML_Left = "<div class=\"left_p\">" +
                                "<div class=\"p_wrapper left\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Овластени лица</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";


            var HTML_Right = "<div class=\"right_p\">" +
                                "<div class=\"p_wrapper right\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Овластени лица</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";

            var tmpData = new Dictionary<string, string>();
            foreach (var item in ovlasteni_lica)
            {
                if (IsAkcionerskoDrustvo(data))
                {
                    if (item.IsOwner == FL_TipPovrzanost.Sopstvenik || item.IsOwner == FL_TipPovrzanost.OvlastenoLice)
                        continue;
                }
                if (tmpData.ContainsKey(item.Ime))
                {
                    tmpData[item.Ime] += ", " + item.Povrzanost;
                }
                else
                {
                    tmpData.Add(item.Ime, item.Povrzanost);
                }
            }
            var i = 0;
            foreach (var item in tmpData)
            {

                if (i % 2 == 0)
                {
                    HTML_Left += "<p class=\"border_bottom\"><span>" + item.Key + "</span><p>" + item.Value + "</p></p>";
                }
                else
                {
                    HTML_Right += "<p class=\"border_bottom\"><span>" + item.Key + "</span><p>" + item.Value + "</p></p>";
                }
                i++;
            }

            HTML_Left += "</div>" +
                            "</div>" +
                        "</div>";

            HTML_Right += "</div>" +
                        "</div>" +
                    "</div>";

            HTML += HTML_Left + HTML_Right;

            data.IndexHelper = 0;

            return HTML;
        }

        private static string OvlasteniLicaLongHTML(Attributes data)
        {
            data.IndexHelper = 0;
            var ovlasteni_lica = data.ovlasteni_lica;

            var total = 0;
            var items = new List<OvlastenoLiceObject>();
            if (ovlasteni_lica != null)
            {
                items = ovlasteni_lica.Where(c => c.IsOwner != (FL_TipPovrzanost)1).ToList();
                if (items.Count() == 0)
                    items = ovlasteni_lica.Where(c => c.Povrzanost.ToLower().Contains("сопственик/овластено лице")).ToList();
                foreach (var item in items)
                {
                    if (IsAkcionerskoDrustvo(data))
                    {
                        if (item.IsOwner == FL_TipPovrzanost.Sopstvenik || item.IsOwner == FL_TipPovrzanost.OvlastenoLice)
                        {
                            continue;
                        }
                    }
                    total++;
                }
            }
            var HTML = "<div class=\"p_wrapper_header\">" +
                            "<h2>Овластени лица</h2>" +
                            "<p class=\"black\">Прикажани " + total + " од " + total + "</p>" +
                        "</div>" +
                        "<div class=\"content\">";

            if (items != null)
            {
                var i = 1;
                foreach (var item in items)
                {
                    if (IsAkcionerskoDrustvo(data))
                    {
                        if (item.IsOwner == FL_TipPovrzanost.Sopstvenik || item.IsOwner == FL_TipPovrzanost.OvlastenoLice)
                        {
                            data.IndexHelper++;
                            continue;
                        }
                    }
                    if (i > 4)
                        break;

                    HTML += "<div class=\"podruznici_filijali\">";
                    if (string.IsNullOrEmpty(item.Ime) == false)
                        HTML += "<p><span>" + item.Ime + "</span></p>";

                    if (string.IsNullOrEmpty(item.Ovlastuvanja) == false)
                        HTML += "<p>Овластувања: " + item.Ovlastuvanja + "</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja1) == false || string.IsNullOrEmpty(item.TipOvlastuvanja2) == false)
                        HTML += "<p>Тип овластување:</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja1) == false)
                        HTML += "<p>- " + item.TipOvlastuvanja1 + "</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja2) == false)
                        HTML += "<p>- " + item.TipOvlastuvanja2 + "</p>";

                    if (string.IsNullOrEmpty(item.Ogranicuvanja) == false)
                        HTML += "<p>Ограничувања" + item.Ogranicuvanja + "</p>";
                    HTML += "</div>";

                    data.IndexHelper++;
                    i++;
                }
            }
            if (items == null || items.Count() == 0)
                HTML += "<p>Нема податоци за овластени лица</p>";
            HTML += "</div>";


            return HTML;
        }

        private static string OvlasteniLicaHTMLAdditional(Attributes data, int page)
        {
            var HTML = "";
            var items = data.ovlasteni_lica.Where(c => c.IsOwner != (FL_TipPovrzanost)1).ToList();
            var tmp = items.Skip(data.IndexHelper).ToList();

            var ovlasteni_lica = tmp.Skip((page * 10)).Take(10).ToList();


            var HTML_Left = "<div class=\"left_p\">" +
                                "<div class=\"p_wrapper left\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Овластени лица</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";


            var HTML_Right = "<div class=\"right_p\">" +
                                "<div class=\"p_wrapper right\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Овластени лица</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";


            var i = 0;
            foreach (var item in ovlasteni_lica)
            {
                if (IsAkcionerskoDrustvo(data))
                {
                    if (item.IsOwner == FL_TipPovrzanost.Sopstvenik || item.IsOwner == FL_TipPovrzanost.OvlastenoLice)
                        continue;
                }
                if (i % 2 == 0)
                {
                    HTML_Left += "<div class=\"podruznici_filijali\">";
                    if (string.IsNullOrEmpty(item.Ime) == false)
                        HTML_Left += "<p><span>" + item.Ime + "</span></p>";

                    if (string.IsNullOrEmpty(item.Ovlastuvanja) == false)
                        HTML_Left += "<p>Овластувања: " + item.Ovlastuvanja + "</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja1) == false || string.IsNullOrEmpty(item.TipOvlastuvanja2) == false)
                        HTML_Left += "<p>Тип овластување:</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja1) == false)
                        HTML_Left += "<p>- " + item.TipOvlastuvanja1 + "</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja2) == false)
                        HTML_Left += "<p>- " + item.TipOvlastuvanja2 + "</p>";

                    if (string.IsNullOrEmpty(item.Ogranicuvanja) == false)
                        HTML_Left += "<p>Ограничувања" + item.Ogranicuvanja + "</p>";
                    HTML_Left += "</div>";
                }
                else
                {
                    HTML_Right += "<div class=\"podruznici_filijali\">";
                    if (string.IsNullOrEmpty(item.Ime) == false)
                        HTML_Right += "<p><span>" + item.Ime + "</span></p>";

                    if (string.IsNullOrEmpty(item.Ovlastuvanja) == false)
                        HTML_Right += "<p>Овластувања: " + item.Ovlastuvanja + "</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja1) == false || string.IsNullOrEmpty(item.TipOvlastuvanja2) == false)
                        HTML_Right += "<p>Тип овластување:</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja1) == false)
                        HTML_Right += "<p>- " + item.TipOvlastuvanja1 + "</p>";

                    if (string.IsNullOrEmpty(item.TipOvlastuvanja2) == false)
                        HTML_Right += "<p>- " + item.TipOvlastuvanja2 + "</p>";

                    if (string.IsNullOrEmpty(item.Ogranicuvanja) == false)
                        HTML_Right += "<p>Ограничувања" + item.Ogranicuvanja + "</p>";
                    HTML_Right += "</div>";
                }
                i++;
            }
            HTML_Left += "</div>" +
                            "</div>" +
                        "</div>";

            HTML_Right += "</div>" +
                        "</div>" +
                    "</div>";

            HTML += HTML_Left + HTML_Right;
            data.IndexHelper = 0;
            return HTML;
        }

        private static int TotalOvlasteniLica(Attributes data)
        {
            var isAD = IsAkcionerskoDrustvo(data);

            var ovlasteni_lica = data.ovlasteni_lica;

            var total = 0;
            foreach (var item in ovlasteni_lica.Where(c => c.IsOwner != (FL_TipPovrzanost)1))
            {
                if (isAD)
                {
                    if (item.IsOwner == FL_TipPovrzanost.Sopstvenik || item.IsOwner == FL_TipPovrzanost.OvlastenoLice)
                        continue;
                }
                total++;
            }

            return total;
        }

        private static int TotalSopstvenici(Attributes data)
        {
            if (IsAkcionerskoDrustvo(data))
                return 0;
            var ovlasteni_lica = data.ovlasteni_lica;

            var sopstvenici = new List<OvlastenoLiceObject>();
            if (ovlasteni_lica != null)
            {

                sopstvenici = ovlasteni_lica.Where(c => c.IsOwner == (FL_TipPovrzanost)1).ToList();

                var add = ovlasteni_lica.Where(c => c.IsOwner != (FL_TipPovrzanost)1 && c.Povrzanost.ToLower().Contains("сопственик")).ToList();
                sopstvenici.AddRange(add);
            }

            return sopstvenici.Count();
        }

        private static string SopstveniciHTML(Attributes data)
        {
            var ovlasteni_lica = data.ovlasteni_lica;

            var sopstvenici = new List<OvlastenoLiceObject>();
            if (ovlasteni_lica != null)
            {
                sopstvenici = ovlasteni_lica.Where(c => c.IsOwner == (FL_TipPovrzanost)1).ToList();

                var add = ovlasteni_lica.Where(c => c.IsOwner != (FL_TipPovrzanost)1 && c.Povrzanost.ToLower().Contains("сопственик")).ToList();
                sopstvenici.AddRange(add);

            }
            if (IsAkcionerskoDrustvo(data))
            {
                sopstvenici = new List<OvlastenoLiceObject>();
            }
            var HTML = "<div class=\"p_wrapper_header\">" +
                            "<h2>Сопственици</h2>" +
                            "<p class=\"black\">Прикажани " + sopstvenici.Count() + " од " + sopstvenici.Count() + "</p>" +
                        "</div>" +
                        "<div class=\"content\">";
            //"<p><span>" + data.celosen_naziv_firma + "</span></p>";
            var addInfo = "";
            data.IndexHelper2 = 0;
            if (sopstvenici != null)
            {
                if (IsDOOEL(data))
                {
                    addInfo = "100.00 %";
                }

                var i = 1;
                foreach (var item in sopstvenici)
                {
                    if (i > 4)
                        break;

                    HTML += "<div class=\"podruznici_filijali\">";
                    HTML += "<p><span>" + item.Ime + " " + addInfo + "</span></p>";
                    HTML += "</div>";
                    data.IndexHelper2++;
                    i++;
                }
            }
            if (IsAkcionerskoDrustvo(data))
                HTML += "<p>Акционерско друштво (АД)</p>";
            else if (sopstvenici == null || sopstvenici.Count() == 0)
            {
                HTML += "<p>Нема податоци за сопственици</p>";
            }

            HTML += "</div>";

            return HTML;
        }

        private static string SopstveniciHTMLAdditional(Attributes data, int page)
        {
            var HTML = "";
            var ovlasteni_lica = data.ovlasteni_lica;

            var sopstvenici = new List<OvlastenoLiceObject>();
            if (ovlasteni_lica != null)
            {
                var tmp = ovlasteni_lica.Where(c => c.IsOwner == (FL_TipPovrzanost)1).Skip(data.IndexHelper2).ToList();

                var add = ovlasteni_lica.Where(c => c.IsOwner != (FL_TipPovrzanost)1 && c.Povrzanost.ToLower().Contains("сопственик")).ToList();
                tmp.AddRange(add);

                sopstvenici = tmp.Skip((page * 50)).Take(50).ToList();

            }

            if (IsAkcionerskoDrustvo(data))
            {
                sopstvenici = new List<OvlastenoLiceObject>();
            }

            var HTML_Left = "<div class=\"left_p\">" +
                                "<div class=\"p_wrapper left\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Сопственици</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";


            var HTML_Right = "<div class=\"right_p\">" +
                                "<div class=\"p_wrapper right\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Сопственици</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";

            if (sopstvenici != null)
            {
                var i = 0;
                foreach (var item in sopstvenici)
                {
                    if (i % 2 == 0)
                    {
                        HTML_Left += "<div class=\"podruznici_filijali\">";
                        HTML_Left += "<p><span>" + item.Ime + "</span></p>";
                        HTML_Left += "</div>";
                    }
                    else
                    {
                        HTML_Right += "<div class=\"podruznici_filijali\">";
                        HTML_Right += "<p><span>" + item.Ime + "</span></p>";
                        HTML_Right += "</div>";
                    }
                    i++;
                }
            }
            HTML_Left += "</div>" +
                         "</div>" +
                     "</div>";

            HTML_Right += "</div>" +
                       "</div>" +
                   "</div>";

            HTML += HTML_Left + HTML_Right;

            return HTML;


        }

        private static int TotalPodruzniciFilijali(Attributes data)
        {
            var ovlasteni_lica = data.ovlasteni_lica;

            var podruznici_filijali = data.organizacioni_edinici;

            var total = 0;
            if (podruznici_filijali != null)
                total = podruznici_filijali.Count();

            return total;
        }

        private static string PodruzniciFilijaliHTMLAdditional(Attributes data, int page)
        {
            var HTML = "";
            var tmp = data.organizacioni_edinici.Skip(3).ToList();
            var podruznici_filijali = tmp.Skip((page * 6)).Take(6).ToList();

            var HTML_Left = "<div class=\"left_p\">" +
                                "<div class=\"p_wrapper left\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Подружници / Филијали</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";


            var HTML_Right = "<div class=\"right_p\">" +
                                "<div class=\"p_wrapper right\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Подружници / Филијали</h2>" +
                                    "</div>" +
                                    "<div class=\"content\">";


            var i = 0;
            foreach (var item in podruznici_filijali)
            {
                if (i % 2 == 0)
                {
                    HTML_Left += "<div class=\"podruznici_filijali\">";
                    if (string.IsNullOrEmpty(item.Podbroj) == false)
                        HTML_Left += "<p><span>Подброј:</span>" + item.Podbroj + "</p>";

                    if (string.IsNullOrEmpty(item.Naziv) == false)
                        HTML_Left += "<p><span>Назив:</span>" + item.Naziv + "</p>";

                    if (string.IsNullOrEmpty(item.Tip) == false)
                        HTML_Left += "<p><span>Тип:</span>" + item.Tip + "</p>";

                    if (string.IsNullOrEmpty(item.Podtip) == false)
                        HTML_Left += "<p><span>Подтип:</span>" + item.Podtip + "</p>";

                    if (string.IsNullOrEmpty(item.GlavnaPrihodnaSifra) == false)
                        HTML_Left += "<p><span>Приоритетна дејност/Главна приходна шифра:</span>" + item.GlavnaPrihodnaSifra;
                    else
                        HTML_Left += "<p><span>Приоритетна дејност/Главна приходна шифра:</span>";

                    if (string.IsNullOrEmpty(item.PrioritetnaDejnost) == false)
                        HTML_Left += "-" + ConvertChars(item.PrioritetnaDejnost, "lower") + "</p>";
                    else
                        HTML_Left += "</p>";

                    if (string.IsNullOrEmpty(item.Adresa) == false)
                        HTML_Left += "<p><span>Адреса:</span>" + ConvertChars(item.Adresa, "firstUpper") + "</p>";

                    if (string.IsNullOrEmpty(item.OvlastenoLice) == false)
                        HTML_Left += "<p><span>Овластено лице:</span>" + ConvertChars(item.OvlastenoLice, "firstUpper") + "</p>";
                    HTML_Left += "</div>";

                }
                else
                {
                    HTML_Right += "<div class=\"podruznici_filijali\">";
                    if (string.IsNullOrEmpty(item.Podbroj) == false)
                        HTML_Right += "<p><span>Подброј:</span>" + item.Podbroj + "</p>";

                    if (string.IsNullOrEmpty(item.Naziv) == false)
                        HTML_Right += "<p><span>Назив:</span>" + item.Naziv + "</p>";

                    if (string.IsNullOrEmpty(item.Tip) == false)
                        HTML_Right += "<p><span>Тип:</span>" + item.Tip + "</p>";

                    if (string.IsNullOrEmpty(item.Podtip) == false)
                        HTML_Right += "<p><span>Подтип:</span>" + item.Podtip + "</p>";

                    if (string.IsNullOrEmpty(item.GlavnaPrihodnaSifra) == false)
                        HTML_Right += "<p><span>Приоритетна дејност/Главна приходна шифра:</span>" + item.GlavnaPrihodnaSifra;
                    else
                        HTML_Right += "<p><span>Приоритетна дејност/Главна приходна шифра:</span>";

                    if (string.IsNullOrEmpty(item.PrioritetnaDejnost) == false)
                        HTML_Right += "-" + ConvertChars(item.PrioritetnaDejnost, "lower") + "</p>";
                    else
                        HTML_Right += "</p>";

                    if (string.IsNullOrEmpty(item.Adresa) == false)
                        HTML_Right += "<p><span>Адреса:</span>" + ConvertChars(item.Adresa, "firstUpper") + "</p>";

                    if (string.IsNullOrEmpty(item.OvlastenoLice) == false)
                        HTML_Right += "<p><span>Овластено лице:</span>" + ConvertChars(item.OvlastenoLice, "firstUpper") + "</p>";
                    HTML_Right += "</div>";
                }
                i++;
            }

            HTML_Left += "</div>" +
                            "</div>" +
                        "</div>";

            HTML_Right += "</div>" +
                        "</div>" +
                    "</div>";

            HTML += HTML_Left + HTML_Right;

            return HTML;
        }

        private static string PodruzniciFilijaliHTML(Attributes data)
        {
            var ovlasteni_lica = data.ovlasteni_lica;

            var podruznici_filijali = data.organizacioni_edinici;

            var total = 0;
            if (podruznici_filijali != null)
                total = podruznici_filijali.Count();
            var HTML = "<div class=\"p_wrapper_header\">" +
                            "<h2 style=\"font-size:21px;\">Подружници / филијали</h2>" +
                            "<p class=\"black\">Прикажани " + total + " од " + total + "</p>" +
                        "</div>" +
                        "<div class=\"content\">";
            if (podruznici_filijali != null)
            {
                var i = 1;
                foreach (var item in podruznici_filijali)
                {
                    if (i > 3)
                        break;
                    HTML += "<div class=\"podruznici_filijali\">";
                    if (string.IsNullOrEmpty(item.Podbroj) == false)
                        HTML += "<p><span>Подброј:</span>" + item.Podbroj + "</p>";

                    if (string.IsNullOrEmpty(item.Naziv) == false)
                        HTML += "<p><span>Назив:</span>" + item.Naziv + "</p>";

                    if (string.IsNullOrEmpty(item.Tip) == false)
                        HTML += "<p><span>Тип:</span>" + item.Tip + "</p>";

                    if (string.IsNullOrEmpty(item.Podtip) == false)
                        HTML += "<p><span>Подтип:</span>" + item.Podtip + "</p>";

                    if (string.IsNullOrEmpty(item.GlavnaPrihodnaSifra) == false)
                        HTML += "<p><span>Приоритетна дејност/Главна приходна шифра:</span>" + item.GlavnaPrihodnaSifra;
                    else
                        HTML += "<p><span>Приоритетна дејност/Главна приходна шифра:</span>";

                    if (string.IsNullOrEmpty(item.PrioritetnaDejnost) == false)
                        HTML += "-" + ConvertChars(item.PrioritetnaDejnost, "lower") + "</p>";
                    else
                        HTML += "</p>";

                    if (string.IsNullOrEmpty(item.Adresa) == false)
                        HTML += "<p><span>Адреса:</span>" + ConvertChars(item.Adresa, "firstUpper") + "</p>";

                    if (string.IsNullOrEmpty(item.OvlastenoLice) == false)
                        HTML += "<p><span>Овластено лице:</span>" + ConvertChars(item.OvlastenoLice, "firstUpper") + "</p>";

                    HTML += "</div>";
                    i++;
                }

                data.tekovni_podruznici = podruznici_filijali.Count().ToString();
            }
            if (podruznici_filijali == null || podruznici_filijali.Count() == 0)
                HTML += "<p>Нема податоци за подружници / филијали</p>";
            HTML += "</div>";

            return HTML;
        }

        private static string Footer()
        {
            string html = "<div class=\"footer\">" +
                                "<div>" +
                                    "<p>© CREDIT REPORT, all rights reserved</p>" +
                                    "<p>www.targetgroup.mk, tel/fax: +389 (2) 3117 - 100</p>" +
                                "</div>" +
                                "<div class=\"img_wrapper\">" +
                                    "<img src=\"" + AbsoluteUrlPath + "\\img\\target_group.png\" />" +
                                "</div>" +
                                "<div class=\"pagination\">" +
                                    "<p>" + CurrentPage + "</p>" +
                                "</div>" +
                            "</div>";

            CurrentPage++;

            return html;
        }

        private static string CoverPage(Attributes data)
        {
            string html = "<!DOCTYPE html>" +
                        "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                        "<head>" +
                            "<title></title>{css}" +
                //"<style>body{height:auto;width:1040px;padding:0;margin:0 auto;font-family:Calibri}.chart_container{float:left;width:50%;position:relative;}.chart_container img{width:100%;}.page{position:relative;height:1471px;width:1040px;padding:0;margin:0;float:left}#first_page_wrapper{width:100%;height:770px;float:left;background-color:#B9D431}#eigth_page_wrapger,#eleventh_page_wrapper,#fifth_page_wrapper,#fourth_page_wrapper,.fourth_page_dynamic,#ninth_page_wrapper,#second_page_wrapper,#seventh_page_wrapper,#sixth_page_wrapper,#tenth_page_wrapper,#third_page_wrapper{width:100%;height:1200px;float:left}h1{font-size:60px;margin-left:50px;margin-top:250px;padding:0;margin-bottom:0}h2{font-size:36px;display:inline-block;margin-left:50px;color:#444}#cover_wrapper{text-align:left;width:100%;float:left}p.inline_block{display:inline-block;font-size:52px;margin-left:150px;margin-top:0;margin-bottom:0;color:#444}p{font-size:22px;margin-left:50px}p span{font-weight:700}.spacer_100{width:100%;height:100px;float:left}.spacer_200{width:100%;height:200px;float:left}.footer{width:100%;height:123px;background-color:#B9D431;opacity:.7;float:left;bottom:0;position:absolute;}.footer .img_wrapper img{padding-top:0;position:absolute;top:0;right:0;}.footer .img_wrapper{float:left;background-color:#fff!important;height:123px;margin-top:0;position:absolute;top:0;right:120px;}.footer.cover_footer{height:200px}.footer div{float:left;margin-top:25px; position:absolute;top:0;}.footer p{padding:0;margin:0 0 0 50px}.footer .pagination{margin:0;padding:0;width:120px;height:123px;text-align:center;float:left;position:absolute;top:0;right:0;}.footer .pagination p{line-height:123px;color:#fff;font-size:35px;margin:0;padding:0}#white_cover{width:100%;height:100px;float:left}#white_cover p{font-weight:700}.footer.cover_footer div{float:left;margin-top:50px}.footer.cover_footer .img_wrapper_cover{position:absolute;top:0;left:500px;float:left;background-color:#fff!important;width:256px;height:200px;margin-left:220px;margin-top:0}.footer.cover_footer p{padding:0;margin:20px 0 0 50px}.footer.cover_footer .img_wrapper_cover img{padding-top:67px}.header{background-color:#F6F6F8;width:100%;height:135px;opacity:.7;border-bottom:5px #000 solid;float:left;position:relative}.float_left{float:left}.float_left h2{font-size:50px;padding:0;margin:10px 0 0 50px;color:#000}.float_left h3{padding:0;margin:0 0 0 50px;color:#000;font-size:20px}.float_left p{margin:0 0 0 50px;font-size:16px}.float_right{float:right}.float_right img{margin-top:10px;margin-right:50px}.left_p{float:left;width:520px}.right_p{float:right;width:520px}.p_wrapper{width:490px;float:left;margin-top:20px}.p_wrapper.left{margin-right:0;margin-left:30px;padding-right:0}.p_wrapper.right{margin-right:30px;margin-left:0;padding-right:0}.p_wrapper.full{width:960px;display:block;margin-left:30px}.podruznici_filijali{border-bottom: 1px solid #000;margin-bottom: 10px;}.podruznici_filijali p{font-size:20px !important;}.p_wrapper.full .p_wrapper_header{width:100%}.p_wrapper .p_wrapper_header{width:470px;float:left;padding-left:20px}.p_wrapper_header h2{margin:0;padding:0;float:left;text-transform:uppercase;color:#000;font-weight:700;font-size:30px}.p_wrapper_header p{margin:0 10px 0 0;padding:0;float:right}.p_wrapper_header p.black{background-color:#000;color:#fff;line-height:30px}.p_wrapper .content{float:left;padding:20px;width:450px;background-color:#fff}.p_wrapper.full .content{width:940px}.p_wrapper .content.gray{background-color:#F2F2F2}.p_wrapper .content p{margin:0;padding:0}.left_p.shorter,.shorter .p_wrapper.left,.shorter .p_wrapper.left .p_wrapper_header{width:340px}.shorter .p_wrapper.left .p_wrapper_header{width:330px !important}.left_p.shorter{width:330px !important}.shorter .p_wrapper.left .content{width:290px}.shorter .p_wrapper.left .content p{margin-bottom:40px}.right_p.longer{width:700px;margin-top:40px;}.longer h3{margin:0;padding:0;font-size:24px;}.longer .p_wrapper.right{width:674px}.longer .p_wrapper.right .p_wrapper_header{width:674px}.longer .p_wrapper.right .content{width:646px}.spacer_50_600{width:600px;height:50px;float:left}.semaphore{width:72px;height:65px;float:left;display:inline-block}.semaphore_text{width:370px;float:left;margin-left:8px}.full .semaphore_text{width:600px;float:left;margin-left:200px}.rezultati_table{width:960px;margin:0;padding:0;border-spacing:0}.rezultati_table tr td{border-bottom:1px solid #000;line-height:32px}.rezultati_table .gray td{background-color:#D9D9D9;border-bottom:0}.rezultati_table tr td:nth-child(2),.rezultati_table tr td:nth-child(3){text-align:right;width:30%}.rezultati_table .information td{border-bottom:0}.rezultati_table .information td p{font-size:15px}.p_wrapper .content .gray{background-color:#F2F2F2;margin-left:-20px;padding-left:20px;margin-right:-20px}.p_wrapper .content .border_bottom{border-bottom:1px solid #000;margin-top:10px}.header .absolute_poglavje{position:absolute;bottom:-20px;right:50px;background-color:#7F7F7F}.header .absolute_poglavje p{margin:0;line-height:40px;font-size:20px;color:#fff;padding:0 20px}.finansii_table{width:550px;margin:0;padding:0;border-spacing:0}.longer p{font-size:18px !important;}.longer p span{font-weight:500 !important;}.longer .finansii_table{width:650px;} .longer .p_wrapper_header h2{padding-left:20px}.finansii_table tr td{border-bottom:1px solid #000;line-height:32px}.finansii_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:40px}.finansii_table .gray td:nth-child(1){text-align:left!important;padding-left:20px}.finansii_table .gray td:nth-child(2),.finansii_table .gray td:nth-child(3){text-align:center!important}.finansii_table tr td:nth-child(1){text-align:right}.finansii_table tr td:nth-child(2),.finansii_table tr td:nth-child(3){text-align:center;width:30%}.finansii_table .information td{border-bottom:0}.finansii_table .information td p{font-size:15px}.promeni_table{width:100%;margin:0;padding:0;border-spacing:0}.promeni_table tr td{border-bottom:1px solid #000;line-height:32px}.promeni_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:60px}.promeni_table tr td:nth-child(1){text-align:center}.promeni_table tr td{text-align:left}.promeni_table .information td{border-bottom:0}.promeni_table .information td p{font-size:15px}.promeni_table tr td:nth-child(1){width:5%}.promeni_table tr td:nth-child(2){width:20%;}.promeni_table tr td:nth-child(3){width:40%}.kazni_table{width:100%;margin:0;padding:0;border-spacing:0}.kazni_table tr td{border-bottom:1px solid #000;line-height:32px}.kazni_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:60px}.kazni_table tr td:nth-child(1){text-align:center}.kazni_table tr td{text-align:left}.kazni_table .information td{border-bottom:0}.kazni_table .information td p{font-size:15px}.kazni_table tr td:nth-child(1){width:5%}.kazni_table tr td:nth-child(2),.kazni_table tr td:nth-child(3){width:20%}.bilans_table{width:100%;margin:0;padding:0;border-spacing:0}.bilans_table tr td{line-height:20px;padding-left:20px}.bilans_table tr.has_border td{border-bottom:1px solid #000}.bilans_table tr td p{font-size:16px}.bilans_table .gray td{background-color:#D9D9D9;border-bottom:0}.bilans_table .gray.text_right td{text-align:right;padding-right:20px}.bilans_table .information td{border-bottom:0}.bilans_table .information td p{font-size:15px}.bilans_table .margin_top td{height:40px;vertical-align:bottom}.bilans_table.margin_top{margin-top:40px;width:850px} .uppercase_must{ text-transform: uppercase !important;} .no_transform{ text-transform: none !important;} .charts_labels{position: absolute;width: 75px;height: 50px;font-size: 14px;z-index: 100;text-align: center;} .chart_label_indicator{width: 10px; height: 10px; display: inline-block; border-radius: 5px;}.znacenje_table p{font-size:14px;} .znacenje_table tr td:first-child p{font-weight:bold;}.td_text_right{text-align:right !important;}</style>" +
                        "</head>" +
                        "<body>" +
                            "<div class=\"page\">" +
                                "<div class=\"spacer_200\"></div>" +
                                "<div id=\"first_page_wrapper\">" +
                                    "<h1>Бонитетен извештај</h1>" +
                                    "<div id=\"cover_wrapper\">" +
                                        "<h2>" + data.ime_firma + " " + data.drzava + "</h2>" +
                                        "<p>Датум на издавање: <span>" + data.datum_izdavanje + "</span></p>" +
                                        "<p>Издаден за: <span>" + data.izdaden_za + "</span></p>" +
                                    "</div>" +
                                "</div>" +
                                "<div id=\"white_cover\">" +
                                    "<p>Таргет Груп ДОО Скопје</p>" +
                                "</div>" +
                                "<div class=\"spacer_200\"></div>" +
                                "<div class=\"footer cover_footer\">" +
                                    "<div>" +
                                        "<p>© CREDIT REPORT, all rights reserved</p>" +
                                        "<p>www.targetgroup.mk, tel/fax: +389 (2) 3117 - 100</p>" +
                                    "</div>" +
                                   "<div class=\"img_wrapper_cover\">" +
                                        "<img src=\"" + AbsoluteUrlPath + "\\img\\target_group.png\" />" +
                                    "</div>" +
                                "</div>" +
                            "</div>";
            return html;
        }

        private static string FirstPage(Attributes data)
        {
            var check2 = DALHelper.CheckIfCRMHasData(data.embs);



            string html = "<div class=\"page\">" +
                             "<div class=\"header\">" +
                                 "<div class=\"float_left\">" +
                                     "<h2>Кредитен извештај</h2>" +
                                     "<h3>" + data.ime_firma + "</h3>" +
                                     "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                 "</div>" +
                                 "<div class=\"float_right\">" +
                                     "<img src=\"" + AbsoluteUrlPath + "\\img\\bonitet_mk.png\" />" +
                                 "</div>" +
                             "</div>" +
                             "<div id=\"second_page_wrapper\">" +
                                 "<div class=\"left_p\">" +
                                     "<div class=\"p_wrapper left\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Профил</h2>" +
                                             "<p class=\"black\">Поглавје 1</p>" +
                                         "</div>" +
                                         "<div class=\"content gray\">" +
                                             "<p>Назив:<span>" + data.naziv_firma + "</span></p>" +
                                             "<p>Адреса:<span>" + data.adresa_firma + "</span></p>" +
                                             "<p>Дејност:<span>" + data.dejnost + "</span></p>" +
                                             "<p>ЕМБС:<span>" + data.embs + "</span></p>" +
                                             "<p>ЕДБ:<span>" + data.edb + "</span></p>" +
                                             "<p>Големина на субјектот:<span>" + data.golemina_na_subjekt + "</span></p>" +
                                             data.vkupna_osnovna_glavnina + data.uplaten_del + data.paricen_vlog + data.neparicen_vlog +
                                         "</div>" +
                                     "</div>" +
                                     "<div class=\"p_wrapper left\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Лица & подружници</h2>" +
                                             "<p class=\"black\">Поглавје 2</p>" +
                                         "</div>" +
                                         "<div class=\"content gray\">" +
                                             "<p><span>Тековни</span></p>" +
                                             "<p class=\"float_left\">Сопственици:<span>" + data.tekovni_sopstvenici + "</span></p>" +
                                             "<p class=\"float_right\">Подружници:<span>" + data.tekovni_podruznici + "</span></p>" +
                                         "</div>" +
                                     "</div>" +
                                     "<div class=\"p_wrapper left\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Блокади</h2>" +
                                             "<p class=\"black\"></p>" +
                                         "</div>" +
                                         "<div class=\"content\">" +
                                             "<p><span>Тековни</span></p>" +
                                             "<p class=\"float_left\">Статус:<span>" + data.tekovni_blokadi_status + "</span></p>" +
                                         "</div>" +
                                     "</div>" +
                                 "</div>" +
                                 "<div class=\"right_p\">" +
                                     "<div class=\"p_wrapper right\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Состојба</h2>" +
                                             "<p class=\"black\">Поглавје 3</p>" +
                                         "</div>" +
                                         "<div class=\"content\">" +
                                         (check2 ? "<p style=\"font-size:18px;\">Компанијата нема доставено финансиски податоци за тековната година заради што е особено ризична за соработка.</p>" :
                //"<p style=\"font-size:18px;\">Коефициент на задолженост " + data.likvidnost_opis_koeficient_za_zadolzenost + "<br/>" +
                //"Брз показател " + data.likvidnost_opis_brz_pokazatel + "<br/>" + 
                //"Поврат на средства (ROA) " + data.efikasnost_opis_povrat_na_sredstva + "</p>" +
                                             "<p style=\"font-size:18px;\">" + data.short_komentar + "</p>") +
                                         "</div>" +
                                     "</div>" +
                                     "<div class=\"p_wrapper right\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Семафор</h2>" +
                                             "<p class=\"black\"></p>" +
                                         "</div>" +
                                         "<div class=\"content\">" +
                                             "<div class=\"semaphore\">" +
                                             (data.semafor_solventnost == "green" ? "<img style=\"width:72px;height:65px;\" src=\"" + AbsoluteUrlPath + "\\img\\green_light.jpg\" class=\"float_left\" />" : "") +
                                             (data.semafor_solventnost == "yellow" ? "<img style=\"width:72px;height:65px;\" src=\"" + AbsoluteUrlPath + "\\img\\yellow_light.jpg\" class=\"float_left\" />" : "") +
                                             (data.semafor_solventnost == "red" ? "<img style=\"width:72px;height:65px;\" src=\"" + AbsoluteUrlPath + "\\img\\red_light.jpg\" class=\"float_left\" />" : "") +
                                             "</div>" +
                                             "<div class=\"semaphore_text\">" +
                                                 "<p class=\"float_right\">" + data.solventnost_komentar + "</p>" +
                                             "</div>" +
                                         "</div>" +
                                     "</div>" +
                                     "<div class=\"p_wrapper right\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Промени & солвентност</h2>" +
                                             "<p class=\"black\">Поглавје 4</p>" +
                                         "</div>" +
                                         "<div class=\"content\">" +
                                             "<p><span>Мината година</span></p>" +
                                             "<p class=\"float_left\">Промени:<span>" + data.promeni_minata_godina + "</span></p>" +
                                             "<p class=\"float_right\">Солвентност:<span>" + data.solventnost_minata_godina + "</span></p>" +
                                         "</div>" +
                                     "</div>" +
                                     "<div class=\"p_wrapper right\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Казни & санкции</h2>" +
                                             "<p class=\"black\">Поглавје 5</p>" +
                                         "</div>" +
                                         "<div class=\"content\">" +
                                             "<p><span>Мината година</span></p>" +
                                             "<p class=\"float_left\">Казни & санкции :<span>" + data.kazni_minata_godina + "</span></p>" +
                                         "</div>" +
                                     "</div>" +
                                 "</div>";
            if (data.GodisnaSmetka)
            {
                html += "<div class=\"p_wrapper full\" style=\"margin-top:0;\">" +
                    "<div class=\"p_wrapper_header\">" +
                        "<h2>Резултати од работењето</h2>" +
                        "<p class=\"black\">Поглавје 6</p>" +
                    "</div>" +
                    "<div class=\"content\">" +
                        "<table class=\"rezultati_table\">" +
                            "<tr class=\"gray\">" +
                                "<td>" +
                                    "<p><span>Категории</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p><span>" + data.kategorija1 + "</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p><span>" + data.kategorija2 + "</span></p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td> " +
                                    "<p><span>Средства</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.sredstva1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.sredstva2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Капитал</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.kapital1 + "</p>" +
                               "</td>" +
                                "<td>" +
                                    "<p>" + data.kapital2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Вкупни приходи</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.vkupno_prihodi1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.vkupno_prihodi2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                               "<td>" +
                                    "<p><span>Нето добивка за деловната година</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.neto_dobivka_za_delovna_godina1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.neto_dobivka_za_delovna_godina2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Просечен број на вработени</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.prosecen_broj_vraboteni1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.prosecen_broj_vraboteni2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Коефициент на задолженост</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.koeficient_na_zadolzensot1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.koeficient_na_zadolzensot2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Тековен показател</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.tekoven_pokazatel1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.tekoven_pokazatel2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Поврат на средства (ROA)</span></p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.povrat_na_sredstva1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.povrat_na_sredstva2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    "<p><span>Поврат на капитал (ROE)</span></p>" +
                               "</td>" +
                                "<td>" +
                                    "<p>" + data.povrat_na_kapital1 + "</p>" +
                                "</td>" +
                                "<td>" +
                                    "<p>" + data.povrat_na_kapital2 + "</p>" +
                                "</td>" +
                            "</tr>" +
                            "<tr class=\"information\">" +
                                "<td colspan=\"3\">" +
                                    "<p>Data are shown in 1 MKD (1 EUR = 61.5 MKD +/- 0.2) see <a href=\"www.nbrm.mk\">www.nbrm.mk</a> for exact exchange rates</p>" +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
                    "</div>" +
                "</div>";
            }
            html += "</div>" +
               Footer() +
           "</div>";

            return html;
        }

        private static string SecondPage(Attributes data)
        {
            var ovlasteni_lica = TotalOvlasteniLica(data);
            var bankarski_smetki = TotalBankarskiSmetki(data);

            string html = "<div class=\"page\">" +
                            "<div class=\"header\">" +
                                "<div class=\"float_left\">" +
                                    "<h2>Профил</h2>" +
                                    "<h3>" + data.ime_firma + "</h3>" +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                "</div>" +
                                "<div class=\"absolute_poglavje\">" +
                                    "<p>Поглавје 1</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"third_page_wrapper\">" +
                                "<div class=\"left_p\">" +
                                    "<div class=\"p_wrapper left\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Профил</h2>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content gray\">" +
                                            "<p>Целосен назив:<span>" + data.celosen_naziv_firma + "</span></p>" +
                                            "<p>Правна форма:<span>" + data.pravna_forma + "</span></p>" +
                                            "<p>Дејност:<span>" + data.dejnost + "</span></p>" +
                                           data.datum_osnovanje +
                                            "<p>ЕМБС:<span>" + data.embs + "</span></p>" +
                                            "<p>ЕДБ:<span>" + data.edb + "</span></p>" +
                                            "<p>Големина на субјектот:<span>" + data.golemina_na_subjekt + "</span></p>" +
                                            "<p>Просечен број на вработени:<span>" + data.prosecen_broj_vraboteni + "</span></p>" +
                                            "<p>ДДВ Обврзник:<span>" + data.ddv_obvrznik + "</span></p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"p_wrapper left\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Овластени лица</h2>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<p class=\"gray\"><span>Име</span></p>" +
                                            OvlasteniLicaShortHTML(data) +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"right_p\">" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Банкарски сметки</h2>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            BankarskiSmetkiHTML(data) +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer() +
                        "</div>";

            if (ovlasteni_lica > 8)
            {
                var pages = Math.Ceiling(Convert.ToDouble((ovlasteni_lica - 8)) / 20);
                if (pages > 0)
                {
                    for (var i = 0; i < pages; i++)
                    {
                        html += "<div class=\"page\">" +
                                    "<div class=\"header\">" +
                                        "<div class=\"float_left\">" +
                                            "<h2>Профил</h2>" +
                                            "<h3>" + data.ime_firma + "</h3>" +
                                            "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                        "</div>" +
                                        "<div class=\"absolute_poglavje\">" +
                                            "<p>Поглавје 1</p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"fourth_page_dynamic\">" +
                                        OvlasteniLicaShortHTMLAdditional(data, i) +
                                    "</div>" + Footer() +
                                "</div>";
                    }
                }
            }
            if (bankarski_smetki > 12)
            {
                var pages = Math.Ceiling(Convert.ToDouble((bankarski_smetki - 12)) / 24);
                if (pages > 0)
                {
                    for (var i = 0; i < pages; i++)
                    {
                        html += "<div class=\"page\">" +
                                    "<div class=\"header\">" +
                                        "<div class=\"float_left\">" +
                                            "<h2>Профил</h2>" +
                                            "<h3>" + data.ime_firma + "</h3>" +
                                            "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                        "</div>" +
                                        "<div class=\"absolute_poglavje\">" +
                                            "<p>Поглавје 1</p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"fourth_page_dynamic\">" +
                                        BankarskiSmetkiHTMLAdditional(data, i) +
                                    "</div>" + Footer() +
                                "</div>";
                    }
                }
            }
            return html;
        }

        private static string ThirdPage(Attributes data)
        {
            var sopstvenici = TotalSopstvenici(data);
            var ovlasteni_lica = TotalOvlasteniLica(data);
            var podruznici_filijali = TotalPodruzniciFilijali(data);

            var html = "";

            html += "<div class=\"page\">" +
                            "<div class=\"header\">" +
                                "<div class=\"float_left\">" +
                                    "<h2>Лица и подружници</h2>" +
                                    "<h3>" + data.ime_firma + "</h3>" +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                "</div>" +
                                "<div class=\"absolute_poglavje\">" +
                                    "<p>Поглавје 2</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"fourth_page_wrapper\">" +
                                "<div class=\"left_p\">" +
                                    "<div class=\"p_wrapper left\">" +
                                        SopstveniciHTML(data) +
                                    "</div>" +
                                    "<div class=\"p_wrapper left\">" +
                                        OvlasteniLicaLongHTML(data) +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"right_p\">" +
                                    "<div class=\"p_wrapper right\">" +
                                        PodruzniciFilijaliHTML(data) +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer() +
                        "</div>";

            if (sopstvenici > 4)
            {
                var pages = Math.Ceiling(Convert.ToDouble((sopstvenici - 4)) / 50);
                if (pages > 0)
                {
                    for (var i = 0; i < pages; i++)
                    {
                        html += "<div class=\"page\">" +
                                    "<div class=\"header\">" +
                                        "<div class=\"float_left\">" +
                                            "<h2>Лица и  подружници</h2>" +
                                            "<h3>" + data.ime_firma + "</h3>" +
                                            "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                        "</div>" +
                                        "<div class=\"absolute_poglavje\">" +
                                            "<p>Поглавје 2</p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"fourth_page_dynamic\" id=\"fourth_page_wrapper_sopstvenici" + i + "\">" +
                                        SopstveniciHTMLAdditional(data, i) +
                                    "</div>" + Footer() +
                                "</div>";
                    }
                }

            }
            if (ovlasteni_lica > 4)
            {
                var pages = Math.Ceiling(Convert.ToDouble((ovlasteni_lica - 4)) / 10);
                if (pages > 0)
                {
                    for (var i = 0; i < pages; i++)
                    {
                        html += "<div class=\"page\">" +
                                    "<div class=\"header\">" +
                                        "<div class=\"float_left\">" +
                                            "<h2>Лица и  подружници</h2>" +
                                            "<h3>" + data.ime_firma + "</h3>" +
                                            "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                        "</div>" +
                                        "<div class=\"absolute_poglavje\">" +
                                            "<p>Поглавје 2</p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"fourth_page_dynamic\" id=\"fourth_page_wrapper_ovlasteni" + i + "\">" +
                                        OvlasteniLicaHTMLAdditional(data, i) +

                                    "</div>" + Footer() +
                                "</div>";
                    }
                }
            }
            if (podruznici_filijali > 3)
            {
                var pages = Math.Ceiling(Convert.ToDouble((podruznici_filijali - 3)) / 6);
                if (pages > 0)
                {
                    for (var i = 0; i < pages; i++)
                    {
                        html += "<div class=\"page\">" +
                                    "<div class=\"header\">" +
                                        "<div class=\"float_left\">" +
                                            "<h2>Лица и  подружници</h2>" +
                                            "<h3>" + data.ime_firma + "</h3>" +
                                            "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                        "</div>" +
                                        "<div class=\"absolute_poglavje\">" +
                                            "<p>Поглавје 2</p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"fourth_page_dynamic\" id=\"fourth_page_wrapper_podruznici" + i + "\">" +
                                        PodruzniciFilijaliHTMLAdditional(data, i) +
                                    "</div>" + Footer() +
                                "</div>";
                    }
                }
            }
            return html;
        }

        private static string FourthPage(Attributes data)
        {
            var html = "<div class=\"page\">" +
                            "<div class=\"header\">" +
                                "<div class=\"float_left\">" +
                                    "<h2>Состојба на компанија</h2>" +
                                    "<h3>" + data.ime_firma + "</h3>" +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                "</div>" +
                                "<div class=\"absolute_poglavje\">" +
                                    "<p>Поглавје 3</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"fifth_page_wrapper\">" +
                                "<div class=\"p_wrapper full\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Семафор</h2>" +
                                        "<p class=\"black\"></p>" +
                                    "</div>" +
                                    "<div class=\"content gray\">" +
                                        "<div class=\"semaphore\">" +
                                             (data.semafor_solventnost == "green" ? "<img style=\"width:72px;height:65px;\" src=\"" + AbsoluteUrlPath + "\\img\\green_light.jpg\" class=\"float_left\" />" : "") +
                                             (data.semafor_solventnost == "yellow" ? "<img style=\"width:72px;height:65px;\" src=\"" + AbsoluteUrlPath + "\\img\\yellow_light.jpg\" class=\"float_left\" />" : "") +
                                             (data.semafor_solventnost == "red" ? "<img style=\"width:72px;height:65px;\" src=\"" + AbsoluteUrlPath + "\\img\\red_light.jpg\" class=\"float_left\" />" : "") +
                                        "</div>" +
                                        "<div class=\"semaphore_text\">" +
                                            "<p>" + data.solventnost_komentar + "</p>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"left_p shorter\">" +
                                    "<div class=\"p_wrapper left\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Финансиска проценка</h2>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<p>" + data.finansiska_procenka_komentar + "</p>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"\"></div>" +
                                "<div class=\"right_p longer\">" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h3>Ликвидност</h3>" +
                                            "<p class=\"black\">" + data.likvidnost_opis_main + "</p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<table class=\"finansii_table\">" +
                                                "<tr class=\"gray\">" +
                                                    "<td>" +
                                                        "<p><span>Показател</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Вредност</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Опис</span></p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Коефициент за задолженост</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_koeficient_za_zadolzenost + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_opis_koeficient_za_zadolzenost + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Брз показател</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_brz_pokazatel + " </p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_opis_brz_pokazatel + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Просечни денови на плаќање на обврските</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_prosecni_denovi_na_plakanje_ovrski + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_opis_prosecni_denovi + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Кредитна изложеност од работење</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_kreditna_izlozenost_od_rabotenje + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.likvidnost_opis_kreditna_izlozenost + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h3>Ефикасност</h3>" +
                                            "<p class=\"black\">" + data.efikasnost_opis_main + "</p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<table class=\"finansii_table\">" +
                                                "<tr class=\"gray\">" +
                                                    "<td>" +
                                                        "<p><span>Показател</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Вредност</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Опис</span></p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Поврат на средства (ROA)</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.efikasnost_povrat_na_sredstva + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.efikasnost_opis_povrat_na_sredstva + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Нето профитна маржа</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.efikasnost_neto_profitna_marza + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.efikasnost_opis_profitna_marza + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h3>Профитабилност</h3>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<table class=\"finansii_table\">" +
                                                "<tr class=\"gray\">" +
                                                    "<td>" +
                                                        "<p><span>Ставка</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>" + data.profitabilnost_stavka1 + "</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>" + data.profitabilnost_stavka2 + "</span></p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Бруто оперативна добивка</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_bruto_operativna_dobivka1 + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_bruto_operativna_dobivka2 + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>Нето добивка од финансирање</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_neto_dobivka_od_finansiranje1 + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_neto_dobivka_od_finansiranje2 + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>EBITDA</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_ebitda1 + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_ebitda2 + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p><span>EBIT</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_ebit1 + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.profitabilnost_ebit2 + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer() +
                        "</div>";

            return html;
        }

        private static string FifthPage(Attributes data)
        {
            var promeni = TotalPromeni(data);
            var solventnost = TotalSolventnost(data);

            var html = "";
            var pages = Math.Ceiling(Convert.ToDouble(promeni) / 25);

            if (pages == 0)
                pages = 1;
            //if (pages > 0)
            //{
            for (var i = 0; i < pages; i++)
            {
                html += "<div class=\"page\">" +
                        "<div class=\"header\">" +
                            "<div class=\"float_left\">" +
                                "<h2>Промени & Солвентност</h2>" +
                                "<h3>" + data.ime_firma + "</h3>" +
                                "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                            "</div>" +
                            "<div class=\"absolute_poglavje\">" +
                                "<p>Поглавје 4</p>" +
                            "</div>" +
                        "</div>" +
                        "<div id=\"sixth_page_wrapper\">" +
                            "<div class=\"p_wrapper full\">" +
                                "<div class=\"p_wrapper_header\">" +
                                    "<h2>Историски промени</h2>" +
                                    "<p class=\"black\"></p>" +
                                "</div>" +
                                "<div class=\"content\">" +
                                    "<table class=\"promeni_table\">" +
                                        "<tr class=\"gray\">" +
                                            "<td>" +
                                                "<p><span>#</span></p>" +
                                            "</td>" +
                                            "<td>" +
                                                "<p><span>Датум</span></p>" +
                                            "</td>" +
                                            "<td>" +
                                                "<p><span>Вид</span></p>" +
                                            "</td>" +
                                        "</tr>" +
                                        PromeniHTML(data, i) +
                                    "</table>" +
                                "</div>" +
                            "</div>";
                //if (i == pages - 1)
                //    html += "<--Solventnost-->";
                html += "</div>" + Footer() +
            "</div>";
            }
            //}                
            pages = Math.Ceiling(Convert.ToDouble(solventnost) / 25);
            if (pages == 0)
                pages = 1;
            //if (pages > 0)
            //{
            for (var i = 0; i < pages; i++)
            {
                html += "<div class=\"page\">" +
                            "<div class=\"header\">" +
                                "<div class=\"float_left\">" +
                                    "<h2>Промени & Солвентност</h2>" +
                                    "<h3>" + data.ime_firma + "</h3>" +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                "</div>" +
                                "<div class=\"absolute_poglavje\">" +
                                    "<p>Поглавје 4</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"sixth_page_wrapper\">" +
                                "<div class=\"p_wrapper full\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Солвентност</h2>" +
                                        "<p class=\"black\"></p>" +
                                    "</div>" +
                                    "<div class=\"content\">" +
                                        "<table class=\"promeni_table\">" +
                                            "<tr class=\"gray\">" +
                                                "<td>" +
                                                    "<p><span>#</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>Датум</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>Вид</span></p>" +
                                                "</td>" +
                                            "</tr>" +
                                            SolventnostHTML(data, i) +
                                        "</table>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer() +
                        "</div>";
            }
            //}


            return html;
        }

        private static string SixthPage(Attributes data)
        {
            var kazni_sankcii = TotalKazniSankcii(data);

            var html = "";
            var pages = Math.Ceiling(Convert.ToDouble(kazni_sankcii) / 25) - 1;
            if (pages > 0)
            {
                for (var i = 0; i < pages; i++)
                {
                    html += "<div class=\"page\">" +
                                    "<div class=\"header\">" +
                                        "<div class=\"float_left\">" +
                                            "<h2>Казни & Санкции</h2>" +
                                            "<h3>" + data.ime_firma + "</h3>" +
                                            "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                        "</div>" +
                                        "<div class=\"absolute_poglavje\">" +
                                            "<p>Поглавје 5</p>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div id=\"seventh_page_wrapper\">" +
                                        "<div class=\"p_wrapper full\">" +
                                            "<div class=\"p_wrapper_header\">" +
                                                "<h2>Казни & Санкции</h2>" +
                                                "<p class=\"black\"></p>" +
                                            "</div>" +
                                            "<div class=\"content\">" +
                                                "<table class=\"kazni_table\">" +
                                                    "<tr class=\"gray\">" +
                                                        "<td>" +
                                                            "<p><span>#</span></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p><span>Датум</span></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p><span>Вид</span></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p><span>Опис</span></p>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    KazniSankciiHTML(data, i) +
                                                "</table>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class=\"p_wrapper full\">" +
                                            "<div class=\"p_wrapper_header\">" +
                                                "<h2>Блокади</h2>" +
                                                "<p class=\"black\"></p>" +
                                            "</div>" +
                                            "<div class=\"content\">" +
                                                "<table class=\"kazni_table\">" +
                                                    "<tr class=\"gray\">" +
                                                        "<td>" +
                                                            "<p><span>#</span></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p><span>Датум</span></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p><span>Вид</span></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p><span>Опис</span></p>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                    "<tr>" +
                                                        "<td>" +
                                                            "<p>1</p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p>" + data.blokadi_datum1 + "</p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p></p>" +
                                                        "</td>" +
                                                        "<td>" +
                                                            "<p>" + data.blokadi_opis1 + "</p>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" + Footer() +
                                "</div>";
                }
            }
            else
            {
                html = "<div class=\"page\">" +
                                "<div class=\"header\">" +
                                    "<div class=\"float_left\">" +
                                        "<h2>Казни & Санкции</h2>" +
                                        "<h3>" + data.ime_firma + "</h3>" +
                                        "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                    "</div>" +
                                    "<div class=\"absolute_poglavje\">" +
                                        "<p>Поглавје 5</p>" +
                                    "</div>" +
                                "</div>" +
                                "<div id=\"seventh_page_wrapper\">" +
                                    "<div class=\"p_wrapper full\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Казни & Санкции</h2>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<table class=\"kazni_table\">" +
                                                "<tr class=\"gray\">" +
                                                    "<td>" +
                                                        "<p><span>#</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Датум</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Вид</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Опис</span></p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                KazniSankciiHTML(data, 0) +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"p_wrapper full\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Блокади</h2>" +
                                            "<p class=\"black\"></p>" +
                                        "</div>" +
                                        "<div class=\"content\">" +
                                            "<table class=\"kazni_table\">" +
                                                "<tr class=\"gray\">" +
                                                    "<td>" +
                                                        "<p><span>#</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Датум</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Вид</span></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p><span>Опис</span></p>" +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td>" +
                                                        "<p>1</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.blokadi_datum1 + "</p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p></p>" +
                                                    "</td>" +
                                                    "<td>" +
                                                        "<p>" + data.blokadi_opis1 + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" + Footer() +
                            "</div>";
            }
            if (pages > 0)
            {
                html += "<div class=\"page\">" +
                                        "<div class=\"header\">" +
                                            "<div class=\"float_left\">" +
                                                "<h2>Казни & Санкции</h2>" +
                                                "<h3>" + data.ime_firma + "</h3>" +
                                                "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                            "</div>" +
                                            "<div class=\"absolute_poglavje\">" +
                                                "<p>Поглавје 5</p>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div id=\"seventh_page_wrapper\">" +
                                            "<div class=\"p_wrapper full\">" +
                                                "<div class=\"p_wrapper_header\">" +
                                                    "<h2>Блокади</h2>" +
                                                    "<p class=\"black\"></p>" +
                                                "</div>" +
                                                "<div class=\"content\">" +
                                                    "<table class=\"kazni_table\">" +
                                                        "<tr class=\"gray\">" +
                                                            "<td>" +
                                                                "<p><span>#</span></p>" +
                                                            "</td>" +
                                                            "<td>" +
                                                                "<p><span>Датум</span></p>" +
                                                            "</td>" +
                                                            "<td>" +
                                                                "<p><span>Вид</span></p>" +
                                                            "</td>" +
                                                            "<td>" +
                                                                "<p><span>Опис</span></p>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td>" +
                                                                "<p>1</p>" +
                                                            "</td>" +
                                                            "<td>" +
                                                                "<p>" + data.blokadi_datum1 + "</p>" +
                                                            "</td>" +
                                                            "<td>" +
                                                                "<p></p>" +
                                                            "</td>" +
                                                            "<td>" +
                                                                "<p>" + data.blokadi_opis1 + "</p>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                    "</table>" +
                                                "</div>" +
                                            "</div>" +
                                        "</div>" + Footer() +
                                    "</div>";
            }

            return html;
        }

        public static string PieChartHTML(Dictionary<string, List<double>> pie_chart, int left, int right, List<int[]> pie_chart_colors)
        {
            var HTML = "";
            var counter = 0;
            foreach (var item in pie_chart)
            {
                var rgb = pie_chart_colors[counter][0] + ", " + pie_chart_colors[counter][1] + ", " + pie_chart_colors[counter][2];
                if (item.Value[1] == 0)
                {
                    continue;
                }

                switch (counter)
                {
                    case 0:
                        HTML += "<p class=\"charts_labels\" style=\"top:50px;word-break: normal; word-wrap: normal;left:" + left + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                    case 1:
                        HTML += "<p class=\"charts_labels\" style=\"top:250px;word-break: normal; word-wrap: normal;left:" + left + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                    case 2:
                        HTML += "<p class=\"charts_labels\" style=\"top:150px;word-break: normal; word-wrap: normal;left:" + left + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                    case 3:
                        HTML += "<p class=\"charts_labels\" style=\"top:50px;word-break: normal; word-wrap: normal;right:" + right + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                    case 4:
                        HTML += "<p class=\"charts_labels\" style=\"top:250px;word-break: normal; word-wrap: normal;right:" + right + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                    case 5:
                        HTML += "<p class=\"charts_labels\" style=\"top:180px;word-break: normal; word-wrap: normal;right:" + right + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                    case 6:
                        HTML += "<p class=\"charts_labels\" style=\"top:120px;word-break: normal; word-wrap: normal;right:" + right + "px;\">" +
                                    item.Key + " " + item.Value[1] + "% " +
                                    "<span style=\"background-color:rgb(" + rgb + ");\" class=\"chart_label_indicator\"></span>" +
                                "</p>";
                        break;
                }
                counter++;
            }

            return HTML;
        }

        private static string ChartsPage(Attributes data)
        {
            var html = "<div class=\"page\">" +
                            "<div class=\"header\">" +
                                "<div class=\"float_left\">" +
                                    "<h2>Финансиски податоци</h2>" +
                                    "<h3>" + data.ime_firma + "</h3>" +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                "</div>" +
                                "<div class=\"absolute_poglavje\">" +
                                    "<p>Поглавје 6</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"eigth_page_wrapger\">" +
                                "<div class=\"p_wrapper full\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Биланс на состојба</h2>" +
                                        "<p class=\"black\"></p>" +
                                    "</div>";

            var check1 = false;
            var check2 = false;
            var check3 = false;
            var check4 = false;
            foreach (var item in data.pie_chart_aktiva)
            {
                foreach (var val in item.Value)
                {
                    if (val > 0)
                    {
                        check1 = true;
                        break;
                    }
                }
            }
            foreach (var item in data.pie_chart_pasiva)
            {
                foreach (var val in item.Value)
                {
                    if (val > 0)
                    {
                        check2 = true;
                        break;
                    }
                }
            }
            foreach (var item in data.pie_chart_prihodi)
            {
                foreach (var val in item.Value)
                {
                    if (val > 0)
                    {
                        check3 = true;
                        break;
                    }
                }
            }
            foreach (var item in data.pie_chart_rashodi)
            {
                foreach (var val in item.Value)
                {
                    if (val > 0)
                    {
                        check4 = true;
                        break;
                    }
                }
            }
            html += "<div class=\"content\">" +
                        "<div class=\"chart_container\" id=\"aktiva_chart\">" +
                            "<p style=\"text-align:center;font-weight:bold;\">Актива " + data.CurYear + "</p>";
            if (check1)
            {
                html += PieChartHTML(data.pie_chart_aktiva, 0, 20, data.pie_chart_colors) +
                                "<img style=\"position:relative;margin-top:0px;z-index:1;\"src=\"" + AbsolutePath + "\\chart_images\\" + data.pie_chart_filename1 + "\" />";
            }
            else
            {
                html += "<p>Нема податоци</p>";
            }
            html += "</div>" +
                        "<div class=\"chart_container\" id=\"pasiva_chart\">" +
                            "<p style=\"text-align:center;font-weight:bold;\">Пасива " + data.CurYear + "</p>";
            if (check2)
            {
                html += PieChartHTML(data.pie_chart_pasiva, 20, 0, data.pie_chart_colors) +
                                "<img style=\"position:relative;margin-top:0px;z-index:1;\"src=\"" + AbsolutePath + "\\chart_images\\" + data.pie_chart_filename2 + "\" />";
            }
            else
            {
                html += "<p>Нема податоци</p>";
            }
            html += "</div>" +
                    "</div>";


            html += "<div class=\"p_wrapper_header\">" +
                "<h2>Биланс на успех</h2>" +
                "<p class=\"black\"></p>" +
            "</div>" +
            "<div class=\"content\">" +
                "<div class=\"chart_container\" id=\"prihodi_chart\">" +
                    "<p style=\"text-align:center;font-weight:bold;\">Приходи " + data.CurYear + "</p>";
            if (check3)
            {
                html += PieChartHTML(data.pie_chart_prihodi, 0, 20, data.pie_chart_colors) +
                "<img style=\"position:relative;margin-top:0px;z-index:1;\"src=\"" + AbsolutePath + "\\chart_images\\" + data.pie_chart_filename3 + "\" />";
            }
            else
            {
                html += "<p>Нема податоци</p>";
            }
            html += "</div>" +
                "<div class=\"chart_container\" id=\"rashodi_chart\">" +
                    "<p style=\"text-align:center;font-weight:bold;\">Расходи " + data.CurYear + "</p>";
            if (check4)
            {
                html += PieChartHTML(data.pie_chart_rashodi, 20, 0, data.pie_chart_colors) +
                        "<img style=\"position:relative;margin-top:0px;z-index:1;\"src=\"" + AbsolutePath + "\\chart_images\\" + data.pie_chart_filename4 + "\" />";
            }
            else
            {
                html += "<p>Нема податоци</p>";
            }
            html += "</div>" +
            "</div>";
            html += "</div>" +
                   "</div>" + Footer() +
               "</div>";

            return html;
        }

        private static string SeventhPage(Attributes data)
        {
            var html = "<div class=\"page\">" +
                            "<div class=\"header\">" +
                                "<div class=\"float_left\">" +
                                    "<h2>Финансиски податоци</h2>" +
                                    "<h3>" + data.ime_firma + "</h3>" +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
                                "</div>" +
                                "<div class=\"absolute_poglavje\">" +
                                    "<p>Поглавје 6</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"eigth_page_wrapger\">" +
                                "<div class=\"p_wrapper full\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Биланс на состојба</h2>" +
                                        "<p class=\"black\"></p>" +
                                    "</div>" +
                                    "<div class=\"content\">" +
                                        "<table class=\"bilans_table\">" +
                                            "<tr class=\"gray text_right\">" +
                                                "<td colspan=\"7\">" +
                                                    "<p><span>% во вкупно</span></p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"gray\">" +
                                                "<td>" +
                                                    "<p><span>Категорија</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.CurYear + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.LastYear + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>Инд.</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.CurYear + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.LastYear + "</span></p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Нетековни средства</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_netekovni_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_netekovni_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_netekovni_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_netekovni_ind + "</p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_netekovni_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_netekovni_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Нематеријални средства</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_nematerijalni_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_nematerijalni_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_nematerijalni_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_nematerijalni_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_nematerijalni_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_nematerijalni_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Материјални средства</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_materijalni_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_materijalni_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_materijalni_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_materijalni_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_materijalni_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_materijalni_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Вложувања во недвижности</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни финансиски средства</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни побарувања</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Одложени даночни средства</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_odlozeni_danocni_sredstva_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Тековни средства</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_tekovni_sredstva_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_tekovni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                           "<tr>" +
                                                "<td>" +
                                                    "<p>Залихи</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zalihi_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zalihi_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zalihi_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zalihi_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zalihi_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zalihi_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Краткорочни побарувања</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Краткорочни финансиски средства</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Парични средства и парични еквиваленти</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_paricni_sredstva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_paricni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_paricni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_paricni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_paricni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_paricni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Средства (или групи а отуѓување наменети за ...</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_sredstva_grupa_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sredstva_grupa_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sredstva_grupa_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sredstva_grupa_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sredstva_grupa_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sredstva_grupa_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Платени трошоци за идните периоди и пресметан ...</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_plateni_trosoci_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_plateni_trosoci_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_plateni_trosoci_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_plateni_trosoci_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_plateni_trosoci_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_plateni_trosoci_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Вкупна актива</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_aktiva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Главнина и резерви</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_glavnina_i_rezervi_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Основна главнина </p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_osnovna_glavnina_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_osnovna_glavnina_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Премии на емитирани акции</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_premii_akcii_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_premii_akcii_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_premii_akcii_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_premii_akcii_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_premii_akcii_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_premii_akcii_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Сопствени акции</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sopstveni_akcii_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sopstveni_akcii_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Запишан, неуплатен капитал</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zapisan_kapital_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zapisan_kapital_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zapisan_kapital_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zapisan_kapital_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zapisan_kapital_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zapisan_kapital_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Ревалоризациска резерва и разлики од вреднување на ...</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Резерви</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_rezervi_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_rezervi_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_rezervi_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_rezervi_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_rezervi_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_rezervi_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Акумулирана добивка</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Пренесена загуба</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_prenesena_zaguba_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_prenesena_zaguba_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Добивка за деловната година</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                   "<p>" + data.bilans_dobivka_delovna_godina_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2013 + "</p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Загуба за деловната година</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Обврски</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_obvrski_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни резервирања на ризици и трошоци</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни обврски </p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2013 + "</p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Краткорочни обврски</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_sredstva + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_ind + "</p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Одложени даночни обврски</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_odlozeni_obvrski_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Одложено плаќање и приходи во идните периоди</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_odlozeno_plakanje_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2013 + "</p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Обврски по основ на нетековни средства (или груп...</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p><span>" + data.bilans_obvrski_po_osnov_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Вкупна пасива</span></p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p></p>" +
                                                "</td>" +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2013 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2012 + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_pasiva_ind + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"information\">" +
                                                "<td colspan=\"7\">" +
                                                    "<p>Data are shown in 1 MKD (1 EUR = 61.5 MKD +/- 0.2) see <a href=\"www.nbrm.mk\">www.nbrm.mk</a> for exact exchange rates</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer() +
                        "</div>";

            return html;
        }

        private static string EighthPage(Attributes data)
        {
            var html = "<div class=\"page\"> " +
                            "<div class=\"header\"> " +
                                "<div class=\"float_left\"> " +
                                    "<h2>Финансиски податоци</h2> " +
                                    "<h3>" + data.ime_firma + "</h3> " +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p> " +
                                "</div> " +
                                "<div class=\"absolute_poglavje\"> " +
                                    "<p>Поглавје 6</p> " +
                                "</div> " +
                            "</div> " +
                            "<div id=\"ninth_page_wrapper\"> " +
                                "<div class=\"p_wrapper full\"> " +
                                    "<div class=\"p_wrapper_header\"> " +
                                        "<h2>Биланс на успех</h2> " +
                                        "<p class=\"black\"></p> " +
                                    "</div> " +
                                    "<div class=\"content\"> " +
                                        "<table class=\"bilans_table\"> " +
                                            "<tr class=\"gray text_right\"> " +
                                                "<td colspan=\"7\"> " +
                                                    "<p><span>% во вкупно</span></p> " +
                                                "</td> " +
                                           " </tr> " +
                                            "<tr class=\"gray\"> " +
                                                "<td> " +
                                                    "<p><span>Категорија</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>" + data.CurYear + "</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>" + data.LastYear + "</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>Инд.</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>" + data.CurYear + "</span></p> " +
                                               " </td> " +
                                                "<td> " +
                                                    "<p><span>" + data.LastYear + "</span></p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Приходи од работењето</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_2013_procent + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                   " <p>" + data.uspeh_prihodi_rabotenje_2012_procent + "</p> " +
                                               " </td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Финансиски приходи</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_finansiski_prihodi_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2012 + "</p> " +
                                               " </td> " +
                                               "<td class=\"td_text_right\">" +
                                                   " <p>" + data.uspeh_finansiski_prihodi_ind + "</p> " +
                                               " </td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2012_procent + "</p> " +
                                                "</td> " +
                                           " </tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Удел во добивката на придружените друштва</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_udel_vo_dobivka_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_udel_vo_dobivka_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_udel_vo_dobivka_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_udel_vo_dobivka_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_udel_vo_dobivka_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_udel_vo_dobivka_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Вкупно приходи</span></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_vkupno_prihodi_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border margin_top\"> " +
                                                "<td> " +
                                                    "<p><span>Расходи од работењето</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2013_procent + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                    "<p>Расходи од основна дејност</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                    "<p>Останати трошоци од работењето</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_ostanati_trosoci_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                    "<p>Трошоци за вработени</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_trosoci_za_vraboteni_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                    "<p>" + data.uspeh_trosoci_za_vraboteni_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_trosoci_za_vraboteni_ind + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_trosoci_za_vraboteni_2013_procent + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_trosoci_za_vraboteni_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Амортизација на материјалните и нематеријалните средс...</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2012 + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_ind + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Резервирање за трошоци и ризици</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Залихи на готови производи и недовршено производство на почетокот на годината</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Залихи на готови производи и недовршено производство на крајот на годината</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Останати расходи од работењето</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_rashodi_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Финансиски расходи</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_rashodi_sredstva + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_rashodi_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Финансиски расходи од односи со поврзани друштва</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_ind + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2013_procent + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Расходи по основ на камати и курсни разлики од работе...</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_kamati_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_kamati_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_kamati_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_kamati_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_kamati_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Расходи од финансиски средства и вредносно усогласув...</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2013 + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_ind + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2013_procent + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Останати финансиски расходи</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Удел во загуба на придружените друштва</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_sredstva + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2012 + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_ind + "</p> " +
                                                "</td> " +
                                              "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Вкупно расходи</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_vkupno_rashodi_ind + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                        "</table> " +
                                        "<table class=\"bilans_table margin_top\"> " +
                                            "<tr class=\"gray\"> " +
                                                "<td> " +
                                                "    <p><span>Категорија</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p><span>" + data.CurYear + "</span></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p><span>" + data.LastYear + "</span></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p><span>Инд.</span></p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Добивка пред оданочување</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Загуба пред оданочување</span></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Данок на добивка</span></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_danok_dobivka_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_danok_dobivka_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_danok_dobivka_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_danok_dobivka_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Нето добивка во деловната година</span></p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_dobivka_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_dobivka_2013 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_dobivka_2012 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_dobivka_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Нето загуба во деловната година</span></p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_zaguba_sredstva + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_zaguba_2013 + "</p> " +
                                                "</td> " +
                                                "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_zaguba_2012 + "</p> " +
                                                "</td> " +
                                               "<td class=\"td_text_right\">" +
                                                "    <p>" + data.uspeh_neto_zaguba_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"information\"> " +
                                                "<td colspan=\"3\"> " +
                                                "    <p>Data are shown in 1 MKD (1 EUR = 61.5 MKD +/- 0.2) see <a href=\"www.nbrm.mk\">www.nbrm.mk</a> for exact exchange rates</p> " +
                                                "</td> " +
                                            "</tr> " +
                                        "</table> " +
                                    "</div> " +
                                "</div> " +
                            "</div> " + Footer() +
                        "</div>";

            return html;
        }

        private static string NinthPage(Attributes data)
        {
            var html = "<div class=\"page\"> " +
                            "<div class=\"header\"> " +
                                "<div class=\"float_left\"> " +
                                    "<h2>Финансиски податоци</h2> " +
                                    "<h3>" + data.ime_firma + "</h3> " +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p> " +
                                "</div> " +
                                "<div class=\"absolute_poglavje\"> " +
                                    "<p>Поглавје 6</p> " +
                                "</div> " +
                            "</div> " +
                            "<div id=\"tenth_page_wrapper\"> " +
                                "<div class=\"p_wrapper full\"> " +
                                    "<div class=\"p_wrapper_header\"> " +
                                        "<h2>Индикатори</h2> " +
                                        "<p class=\"black\"></p> " +
                                    "</div> " +
                                    "<div class=\"content\"> " +
                                       " <table class=\"bilans_table\"> " +
                                            "<tr class=\"gray\"> " +
                                            "    <td> " +
                                            "        <p><span>Категорија</span></p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p><span>" + data.CurYear + "</span></p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p><span>" + data.LastYear + "</span></p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p><span>Раст а.в.(%)</span></p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                            "    <td colspan=\"4\"> " +
                                            "        <p><span>Ликвидносни индикатори</span></p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Работен капитал</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_raboten_kapital_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_raboten_kapital_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_raboten_kapital_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Тековен показател</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_tekoven_pokazatel_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_tekoven_pokazatel_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_tekoven_pokazatel_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Брз показател</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_brz_pokazatel_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_brz_pokazatel_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_brz_pokazatel_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                            "    <td colspan=\"4\"> " +
                                            "        <p><span>Показатели за обрт</span></p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Обрт на средства</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_sredstva_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_sredstva_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_sredstva_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Денови на обрт на средства</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_sredstva_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_sredstva_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_sredstva_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Обрт на обврските</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_obvrski_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_obvrski_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_obvrski_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Просечни денови на плаќање на обврските</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_prosecni_denovi_obvrski_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_prosecni_denovi_obvrski_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_prosecni_denovi_obvrski_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Обрт на побарувањата</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_pobaruvanja_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_pobaruvanja_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_pobaruvanja_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Денови на обрт на побарувањата</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_pobaruvanja_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_pobaruvanja_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_pobaruvanja_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Обрт на залихи</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_zalihi_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_zalihi_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_obrt_zalihi_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Денови на обрт на залихи</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_zalihi_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_zalihi_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_denovi_obrt_zalihi_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                            "    <td colspan=\"4\"> " +
                                            "        <p><span>Профитабилност</span></p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Поврат на капитал (ROE)</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_povrat_kapital_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_povrat_kapital_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_povrat_kapital_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Поврат на средства (ROA)</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_povrat_sredstva_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_povrat_sredstva_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_povrat_sredstva_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Нето профитна маргина</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_neto_profitna_margina_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_neto_profitna_margina_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_neto_profitna_margina_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                            "    <td colspan=\"4\"> " +
                                            "        <p><span>Показатели за задолженост</span></p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td " +
                                            "        <p>Финансиски левериџ</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_finansiski_leviridz_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_finansiski_leviridz_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_finansiski_leviridz_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Коефициент на задолженост</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_koeficient_zadolzenost_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_koeficient_zadolzenost_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_koeficient_zadolzenost_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Вкупни обврски / EBITDA</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_vkupni_obvrski_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_vkupni_obvrski_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_vkupni_obvrski_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Покриеност на сервисирање на долгови (DSCR)</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_pokrienost_servisiranje_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_pokrienost_servisiranje_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_pokrienost_servisiranje_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Покриеност на камати</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_pokrienost_kamati_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_pokrienost_kamati_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_pokrienost_kamati_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Краткорочни кредити / Продажба</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_kratkorocni_krediti_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_kratkorocni_krediti_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_kratkorocni_krediti_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                            "<tr> " +
                                            "    <td> " +
                                            "        <p>Тековни обврски / Вкупна продажба</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_tekovni_obvrski_2013 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_tekovni_obvrski_2012 + "</p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p>" + data.indikatori_tekovni_obvrski_rast + "</p> " +
                                            "    </td> " +
                                            "</tr> " +
                                       " </table> " +
                                    "</div> " +
                                "</div> " +
                            "</div> " + Footer() +
                        "</div>";

            return html;
        }

        private static string TenthPage(Attributes data)
        {
            var html = "<div class=\"page\"> " +
                            "<div class=\"header\"> " +
                                "<div class=\"float_left\"> " +
                                    "<h2>Финансиски податоци</h2> " +
                                    "<h3>" + data.ime_firma + "</h3> " +
                                    "<p class=\"date\">Издаден " + data.datum_izdavanje + "</p> " +
                                "</div> " +
                                "<div class=\"absolute_poglavje\"> " +
                                    "<p>Поглавје 6</p> " +
                                "</div> " +
                            "</div> " +
                            "<div id=\"eleventh_page_wrapper\"> " +
                                "<div class=\"p_wrapper full\"> " +
                                    "<div class=\"p_wrapper_header\"> " +
                                        "<h2>Готовински текови</h2> " +
                                        "<p class=\"black\"></p> " +
                                    "</div> " +
                                    "<div class=\"content\"> " +
                                        "<table class=\"bilans_table\"> " +
                                        "    <tr class=\"gray\"> " +
                                        "        <td> " +
                                        "            <p><span>Категорија</span></p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p><span>" + data.CurYear + "</span></p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr class=\"has_border\"> " +
                                        "        <td colspan=\"2\"> " +
                                        "            <p><span>Опис</span></p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето профит (добивка) по оданочување</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_profit_odanocuvanje + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Загуба по оданочување</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_zaguba_odanocuvanje + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Амортизација</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_amortizacija + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr class=\"has_border\"> " +
                                        "        <td colspan=\"2\"> " +
                                        "            <p><span>Готовински текови од оперативни (деловни) активности</span></p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Приливи на готовината од деловни активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_prilivi_gotovina_aktivnosti + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Одливи на готовината од деловни активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_odlivi_gotovina_aktivnosti + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето приливи на готовината од деловни активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_prilivi_gotovina_aktivnosti + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето одливи на готовината од деловни активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_odlivi_gotovina_aktivnosti + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr class=\"has_border\"> " +
                                        "        <td colspan=\"2\"> " +
                                        "            <p><span>Готовински текови од инвестициони активности</span></p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Приливи на готовината од инвестициони активности и пласмани</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_prilivi_gotovina_investicioni + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Одливи на готовината од инвестициони активности и пласмани</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_odlivi_gotovina_investicioni + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето приливи на готовината од инвестициони активности и пласмани активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_odlivi_gotovina_investicioni + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето одливи на готовината од инвестициони активности и пласмани</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_odlivi_gotovina_investicioni + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr class=\"has_border\"> " +
                                        "        <td colspan=\"2\"> " +
                                        "            <p><span>Готовински текови од финансиски активности</span></p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Приливи на готовината од финансиски активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_prilivi_gotovina_finansiski + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Одливи на готовината од финансиски активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_odlivi_gotovina_finansiski + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето приливи на готовината од финансиски активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_prilivi_gotovina_finansiski + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Нето одливи на готовината од финансиски активности</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_neto_odlivi_gotovina_finansiski + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Вкупно приливи на готовината </p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_vkupno_prilivi_gotovina + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Вкупно одливи на готовината</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_vkupno_odlivi_gotovina + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Вкупно нето приливи на готовината</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_vkupno_neto_prilivi + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Вкупно нето одливи на готовината</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_vkupno_neto_odlivi + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr> " +
                                        "        <td> " +
                                        "            <p>Парични средства на почеток на годината</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_paricni_sredstva_pocetok + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr class=\"has_border\"> " +
                                        "        <td> " +
                                        "            <p>Парични средства на крајот на годината</p> " +
                                        "        </td> " +
                                        "        <td> " +
                                        "            <p>" + data.tekovi_paricni_sredstva_kraj + "</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "    <tr class=\"information\"> " +
                                        "        <td colspan=\"3\"> " +
                                        "            <p>Data are shown in 1 MKD (1 EUR = 61.5 MKD +/- 0.2) see <a href=\"www.nbrm.mk\">www.nbrm.mk</a> for exact exchange rates</p> " +
                                        "        </td> " +
                                        "    </tr> " +
                                        "</table> " +
                                    "</div> " +
                                "</div> " +
                            "</div> " + Footer() +
                       " </div>";

            return html;
        }




        private static string GetChart1()
        {
            return string.Empty;
        }

    }


    public class Attributes : System.Collections.IEnumerable
    {
        public bool GodisnaSmetka { get; set; }
        public int IndexHelper { get; set; }
        public int IndexHelper2 { get; set; }

        public Dictionary<string, List<double>> pie_chart_aktiva { get; set; }
        public Dictionary<string, List<double>> pie_chart_pasiva { get; set; }
        public Dictionary<string, List<double>> pie_chart_prihodi { get; set; }
        public Dictionary<string, List<double>> pie_chart_rashodi { get; set; }
        public List<int[]> pie_chart_colors { get; set; }

        public string CurYear { get; set; }
        public string LastYear { get; set; }
        public string ime_firma { get; set; }
        public string drzava { get; set; }
        public string datum_izdavanje { get; set; }
        public string izdaden_za { get; set; }
        public string uplaten_del { get; set; }
        public string paricen_vlog { get; set; }
        public string neparicen_vlog { get; set; }

        public string last_year { get; set; }
        public string this_year { get; set; }

        public string organizacionen_oblik { get; set; }
        public string deponent_banka { get; set; }
        public string ziro_smetka { get; set; }

        public string tekovni_sopstvenici { get; set; }
        public string tekovni_podruznici { get; set; }
        public string tekovni_blokadi_status { get; set; }

        public string sostojba_komentar { get; set; }
        public string semafor_solventnost { get; set; }
        public string solventnost_komentar { get; set; }

        public string promeni_minata_godina { get; set; }
        public string solventnost_minata_godina { get; set; }

        public string kazni_minata_godina { get; set; }

        public string naziv_firma { get; set; }
        public string celosen_naziv_firma { get; set; }
        public string adresa_firma { get; set; }
        public string pravna_forma { get; set; }
        public string dejnost { get; set; }
        public string embs { get; set; }
        public string edb { get; set; }
        public string golemina_na_subjekt { get; set; }
        public string vkupna_osnovna_glavnina { get; set; }
        public string datum_osnovanje { get; set; }
        public string prosecen_broj_vraboteni { get; set; }
        public string ddv_obvrznik { get; set; }

        public string sr_vkupni_prihodi { get; set; }
        public string sr_vkupni_rashodi { get; set; }
        public string sr_dobivka_za_finansiska_godina { get; set; }
        public string sr_zaguba_za_finansiska_godina { get; set; }
        public string sr_prosecen_broj_vraboteni { get; set; }
        public string sr_netekovni_sredstva { get; set; }
        public string sr_odlozeni_danocni_obvrski_aktiva { get; set; }
        public string sr_tekovni_sredstva { get; set; }
        public string sr_zalihi { get; set; }
        public string sr_sredstva_ili_grupi_za_otugjuvanje { get; set; }
        public string sr_plateni_trosoci_za_idni_periodi { get; set; }
        public string sr_d_vkupna_aktiva { get; set; }
        public string sr_glavnina_i_rezervi { get; set; }
        public string sr_obvrski { get; set; }
        public string sr_odlozeni_danocni_obvrski_pasiva { get; set; }
        public string sr_odlozeno_plakanje_trosoci { get; set; }
        public string sr_obvrski_po_osnov_na_netekovni_sredstva { get; set; }
        public string sr_d_vkupna_pasiva { get; set; }

        public string sr_vkupni_prihodi_ind { get; set; }
        public string sr_vkupni_rashodi_ind { get; set; }
        public string sr_dobivka_za_finansiska_godina_ind { get; set; }
        public string sr_zaguba_za_finansiska_godina_ind { get; set; }
        public string sr_prosecen_broj_vraboteni_ind { get; set; }
        public string sr_netekovni_sredstva_ind { get; set; }
        public string sr_odlozeni_danocni_obvrski_aktiva_ind { get; set; }
        public string sr_tekovni_sredstva_ind { get; set; }
        public string sr_zalihi_ind { get; set; }
        public string sr_sredstva_ili_grupi_za_otugjuvanje_ind { get; set; }
        public string sr_plateni_trosoci_za_idni_periodi_ind { get; set; }
        public string sr_d_vkupna_aktiva_ind { get; set; }
        public string sr_glavnina_i_rezervi_ind { get; set; }
        public string sr_obvrski_ind { get; set; }
        public string sr_odlozeni_danocni_obvrski_pasiva_ind { get; set; }
        public string sr_odlozeno_plakanje_trosoci_ind { get; set; }
        public string sr_obvrski_po_osnov_na_netekovni_sredstva_ind { get; set; }
        public string sr_d_vkupna_pasiva_ind { get; set; }

        public string sr_vkupni_prihodi_lastyear { get; set; }
        public string sr_vkupni_rashodi_lastyear { get; set; }
        public string sr_dobivka_za_finansiska_godina_lastyear { get; set; }
        public string sr_zaguba_za_finansiska_godina_lastyear { get; set; }
        public string sr_prosecen_broj_vraboteni_lastyear { get; set; }
        public string sr_netekovni_sredstva_lastyear { get; set; }
        public string sr_odlozeni_danocni_obvrski_aktiva_lastyear { get; set; }
        public string sr_tekovni_sredstva_lastyear { get; set; }
        public string sr_zalihi_lastyear { get; set; }
        public string sr_sredstva_ili_grupi_za_otugjuvanje_lastyear { get; set; }
        public string sr_plateni_trosoci_za_idni_periodi_lastyear { get; set; }
        public string sr_d_vkupna_aktiva_lastyear { get; set; }
        public string sr_glavnina_i_rezervi_lastyear { get; set; }
        public string sr_obvrski_lastyear { get; set; }
        public string sr_odlozeni_danocni_obvrski_pasiva_lastyear { get; set; }
        public string sr_odlozeno_plakanje_trosoci_lastyear { get; set; }
        public string sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear { get; set; }
        public string sr_d_vkupna_pasiva_lastyear { get; set; }

        public string sr_netekovni_sredstva_procent { get; set; }
        public string sr_odlozeni_danocni_obvrski_aktiva_procent { get; set; }
        public string sr_tekovni_sredstva_procent { get; set; }
        public string sr_zalihi_procent { get; set; }
        public string sr_sredstva_ili_grupi_za_otugjuvanje_procent { get; set; }
        public string sr_plateni_trosoci_za_idni_periodi_procent { get; set; }
        public string sr_d_vkupna_aktiva_procent { get; set; }
        public string sr_glavnina_i_rezervi_procent { get; set; }
        public string sr_obvrski_procent { get; set; }
        public string sr_odlozeni_danocni_obvrski_pasiva_procent { get; set; }
        public string sr_odlozeno_plakanje_trosoci_procent { get; set; }
        public string sr_obvrski_po_osnov_na_netekovni_sredstva_procent { get; set; }
        public string sr_d_vkupna_pasiva_procent { get; set; }

        public string sr_netekovni_sredstva_procent_lastyear { get; set; }
        public string sr_odlozeni_danocni_obvrski_aktiva_procent_lastyear { get; set; }
        public string sr_tekovni_sredstva_procent_lastyear { get; set; }
        public string sr_zalihi_procent_lastyear { get; set; }
        public string sr_sredstva_ili_grupi_za_otugjuvanje_procent_lastyear { get; set; }
        public string sr_plateni_trosoci_za_idni_periodi_procent_lastyear { get; set; }
        public string sr_d_vkupna_aktiva_procent_lastyear { get; set; }
        public string sr_glavnina_i_rezervi_procent_lastyear { get; set; }
        public string sr_obvrski_procent_lastyear { get; set; }
        public string sr_odlozeni_danocni_obvrski_pasiva_procent_lastyear { get; set; }
        public string sr_odlozeno_plakanje_trosoci_procent_lastyear { get; set; }
        public string sr_obvrski_po_osnov_na_netekovni_sredstva_procent_lastyear { get; set; }
        public string sr_d_vkupna_pasiva_procent_lastyear { get; set; }

        public List<BankarskaSmetka> bankarski_smetki { get; set; }

        public List<OvlastenoLiceObject> ovlasteni_lica { get; set; }

        public List<OrganizacionaEdinicaObject> organizacioni_edinici { get; set; }

        public List<KazniISankcii> kazni_i_sankcii { get; set; }

        public List<PromeniCompany> promeni { get; set; }

        public List<SolventnostCompany> solventnost { get; set; }

        public string vkupen_broj_sopstvenici { get; set; }
        public string prikazan_broj_sopstvenici { get; set; }

        public string vkupen_broj_ovlasteni { get; set; }
        public string prikazan_broj_ovlasteni { get; set; }

        public string prikazan_broj_podruznici { get; set; }
        public string vkupen_broj_podruznici { get; set; }

        public string kategorija1 { get; set; }
        public string sredstva1 { get; set; }
        public string kapital1 { get; set; }
        public string vkupno_prihodi1 { get; set; }
        public string neto_dobivka_za_delovna_godina1 { get; set; }
        public string prosecen_broj_vraboteni1 { get; set; }
        public string koeficient_na_zadolzensot1 { get; set; }
        public string tekoven_pokazatel1 { get; set; }
        public string povrat_na_sredstva1 { get; set; }
        public string povrat_na_kapital1 { get; set; }

        public string kategorija2 { get; set; }
        public string sredstva2 { get; set; }
        public string kapital2 { get; set; }
        public string vkupno_prihodi2 { get; set; }
        public string neto_dobivka_za_delovna_godina2 { get; set; }
        public string prosecen_broj_vraboteni2 { get; set; }
        public string koeficient_na_zadolzensot2 { get; set; }
        public string tekoven_pokazatel2 { get; set; }
        public string povrat_na_sredstva2 { get; set; }
        public string povrat_na_kapital2 { get; set; }

        public string finansiska_procenka_komentar { get; set; }
        public string short_komentar { get; set; }

        public string likvidnost_koeficient_za_zadolzenost { get; set; }
        public string likvidnost_brz_pokazatel { get; set; }
        public string likvidnost_prosecni_denovi_na_plakanje_ovrski { get; set; }
        public string likvidnost_kreditna_izlozenost_od_rabotenje { get; set; }
        public string likvidnost_opis_main { get; set; }
        public string likvidnost_opis_koeficient_za_zadolzenost { get; set; }
        public string likvidnost_opis_brz_pokazatel { get; set; }
        public string likvidnost_opis_prosecni_denovi { get; set; }
        public string likvidnost_opis_kreditna_izlozenost { get; set; }

        public string efikasnost_povrat_na_sredstva { get; set; }
        public string efikasnost_neto_profitna_marza { get; set; }

        public string efikasnost_opis_main { get; set; }
        public string efikasnost_opis_povrat_na_sredstva { get; set; }
        public string efikasnost_opis_profitna_marza { get; set; }

        public string profitabilnost_stavka1 { get; set; }
        public string profitabilnost_stavka2 { get; set; }
        public string profitabilnost_bruto_operativna_dobivka1 { get; set; }
        public string profitabilnost_neto_dobivka_od_finansiranje1 { get; set; }
        public string profitabilnost_ebitda1 { get; set; }
        public string profitabilnost_ebit1 { get; set; }

        public string profitabilnost_bruto_operativna_dobivka2 { get; set; }
        public string profitabilnost_neto_dobivka_od_finansiranje2 { get; set; }
        public string profitabilnost_ebitda2 { get; set; }
        public string profitabilnost_ebit2 { get; set; }

        public string blokadi_datum1 { get; set; }
        public string blokadi_opis1 { get; set; }

        public string bilans_kategorija1 { get; set; }
        public string bilans_kategorija2 { get; set; }
        public string bilans_kategorija_ind { get; set; }
        public string bilans_kategorija_procent1 { get; set; }
        public string bilans_kategorija_procent2 { get; set; }

        public string bilans_netekovni_sredstva { get; set; }
        public string bilans_netekovni_2013 { get; set; }
        public string bilans_netekovni_2012 { get; set; }
        public string bilans_netekovni_ind { get; set; }
        public string bilans_netekovni_2013_procent { get; set; }
        public string bilans_netekovni_2012_procent { get; set; }

        public string bilans_nematerijalni_sredstva { get; set; }
        public string bilans_nematerijalni_2013 { get; set; }
        public string bilans_nematerijalni_2012 { get; set; }
        public string bilans_nematerijalni_ind { get; set; }
        public string bilans_nematerijalni_2013_procent { get; set; }
        public string bilans_nematerijalni_2012_procent { get; set; }

        public string bilans_materijalni_sredstva { get; set; }
        public string bilans_materijalni_2013 { get; set; }
        public string bilans_materijalni_2012 { get; set; }
        public string bilans_materijalni_ind { get; set; }
        public string bilans_materijalni_2013_procent { get; set; }
        public string bilans_materijalni_2012_procent { get; set; }

        public string bilans_vlozuvanje_nedviznosti_sredstva { get; set; }
        public string bilans_vlozuvanje_nedviznosti_2013 { get; set; }
        public string bilans_vlozuvanje_nedviznosti_2012 { get; set; }
        public string bilans_vlozuvanje_nedviznosti_ind { get; set; }
        public string bilans_vlozuvanje_nedviznosti_2013_procent { get; set; }
        public string bilans_vlozuvanje_nedviznosti_2012_procent { get; set; }

        public string bilans_dolgorocni_sredstva_sredstva { get; set; }
        public string bilans_dolgorocni_sredstva_2013 { get; set; }
        public string bilans_dolgorocni_sredstva_2012 { get; set; }
        public string bilans_dolgorocni_sredstva_ind { get; set; }
        public string bilans_dolgorocni_sredstva_2013_procent { get; set; }
        public string bilans_dolgorocni_sredstva_2012_procent { get; set; }

        public string bilans_dolgorocni_pobaruvanja_sredstva { get; set; }
        public string bilans_dolgorocni_pobaruvanja_2013 { get; set; }
        public string bilans_dolgorocni_pobaruvanja_2012 { get; set; }
        public string bilans_dolgorocni_pobaruvanja_ind { get; set; }
        public string bilans_dolgorocni_pobaruvanja_2013_procent { get; set; }
        public string bilans_dolgorocni_pobaruvanja_2012_procent { get; set; }

        public string bilans_odlozeni_danocni_sredstva_sredstva { get; set; }
        public string bilans_odlozeni_danocni_sredstva_2013 { get; set; }
        public string bilans_odlozeni_danocni_sredstva_2012 { get; set; }
        public string bilans_odlozeni_danocni_sredstva_ind { get; set; }
        public string bilans_odlozeni_danocni_sredstva_2013_procent { get; set; }
        public string bilans_odlozeni_danocni_sredstva_2012_procent { get; set; }

        public string bilans_tekovni_sredstva_sredstva { get; set; }
        public string bilans_tekovni_sredstva_2013 { get; set; }
        public string bilans_tekovni_sredstva_2012 { get; set; }
        public string bilans_tekovni_sredstva_ind { get; set; }
        public string bilans_tekovni_sredstva_2013_procent { get; set; }
        public string bilans_tekovni_sredstva_2012_procent { get; set; }

        public string bilans_zalihi_sredstva { get; set; }
        public string bilans_zalihi_2013 { get; set; }
        public string bilans_zalihi_2012 { get; set; }
        public string bilans_zalihi_ind { get; set; }
        public string bilans_zalihi_2013_procent { get; set; }
        public string bilans_zalihi_2012_procent { get; set; }

        public string bilans_kratkorocni_pobaruvanja_sredstva { get; set; }
        public string bilans_kratkorocni_pobaruvanja_2013 { get; set; }
        public string bilans_kratkorocni_pobaruvanja_2012 { get; set; }
        public string bilans_kratkorocni_pobaruvanja_ind { get; set; }
        public string bilans_kratkorocni_pobaruvanja_2013_procent { get; set; }
        public string bilans_kratkorocni_pobaruvanja_2012_procent { get; set; }

        public string bilans_paricni_sredstva_sredstva { get; set; }
        public string bilans_paricni_sredstva_2013 { get; set; }
        public string bilans_paricni_sredstva_2012 { get; set; }
        public string bilans_paricni_sredstva_ind { get; set; }
        public string bilans_paricni_sredstva_2013_procent { get; set; }
        public string bilans_paricni_sredstva_2012_procent { get; set; }

        public string bilans_sredstva_grupa_sredstva { get; set; }
        public string bilans_sredstva_grupa_2013 { get; set; }
        public string bilans_sredstva_grupa_2012 { get; set; }
        public string bilans_sredstva_grupa_ind { get; set; }
        public string bilans_sredstva_grupa_2013_procent { get; set; }
        public string bilans_sredstva_grupa_2012_procent { get; set; }

        public string bilans_plateni_trosoci_sredstva { get; set; }
        public string bilans_plateni_trosoci_2013 { get; set; }
        public string bilans_plateni_trosoci_2012 { get; set; }
        public string bilans_plateni_trosoci_ind { get; set; }
        public string bilans_plateni_trosoci_2013_procent { get; set; }
        public string bilans_plateni_trosoci_2012_procent { get; set; }

        public string bilans_vkupna_aktiva_sredstva { get; set; }
        public string bilans_vkupna_aktiva_2013 { get; set; }
        public string bilans_vkupna_aktiva_2012 { get; set; }
        public string bilans_vkupna_aktiva_ind { get; set; }
        public string bilans_vkupna_aktiva_2013_procent { get; set; }
        public string bilans_vkupna_aktiva_2012_procent { get; set; }

        public string bilans_glavnina_i_rezervi_sredstva { get; set; }
        public string bilans_glavnina_i_rezervi_2013 { get; set; }
        public string bilans_glavnina_i_rezervi_2012 { get; set; }
        public string bilans_glavnina_i_rezervi_ind { get; set; }
        public string bilans_glavnina_i_rezervi_2013_procent { get; set; }
        public string bilans_glavnina_i_rezervi_2012_procent { get; set; }

        public string bilans_osnovna_glavnina_sredstva { get; set; }
        public string bilans_osnovna_glavnina_2013 { get; set; }
        public string bilans_osnovna_glavnina_2012 { get; set; }
        public string bilans_osnovna_glavnina_ind { get; set; }
        public string bilans_osnovna_glavnina_2013_procent { get; set; }
        public string bilans_osnovna_glavnina_2012_procent { get; set; }

        public string bilans_premii_akcii_sredstva { get; set; }
        public string bilans_premii_akcii_2013 { get; set; }
        public string bilans_premii_akcii_2012 { get; set; }
        public string bilans_premii_akcii_ind { get; set; }
        public string bilans_premii_akcii_2013_procent { get; set; }
        public string bilans_premii_akcii_2012_procent { get; set; }

        public string bilans_sopstveni_akcii_sredstva { get; set; }
        public string bilans_sopstveni_akcii_2013 { get; set; }
        public string bilans_sopstveni_akcii_2012 { get; set; }
        public string bilans_sopstveni_akcii_ind { get; set; }
        public string bilans_sopstveni_akcii_2013_procent { get; set; }
        public string bilans_sopstveni_akcii_2012_procent { get; set; }

        public string bilans_zapisan_kapital_sredstva { get; set; }
        public string bilans_zapisan_kapital_2013 { get; set; }
        public string bilans_zapisan_kapital_2012 { get; set; }
        public string bilans_zapisan_kapital_ind { get; set; }
        public string bilans_zapisan_kapital_2013_procent { get; set; }
        public string bilans_zapisan_kapital_2012_procent { get; set; }

        public string bilans_revalorizaciska_rezerva_sredstva { get; set; }
        public string bilans_revalorizaciska_rezerva_2013 { get; set; }
        public string bilans_revalorizaciska_rezerva_2012 { get; set; }
        public string bilans_revalorizaciska_rezerva_ind { get; set; }
        public string bilans_revalorizaciska_rezerva_2013_procent { get; set; }
        public string bilans_revalorizaciska_rezerva_2012_procent { get; set; }

        public string bilans_rezervi_sredstva { get; set; }
        public string bilans_rezervi_2013 { get; set; }
        public string bilans_rezervi_2012 { get; set; }
        public string bilans_rezervi_ind { get; set; }
        public string bilans_rezervi_2013_procent { get; set; }
        public string bilans_rezervi_2012_procent { get; set; }

        public string bilans_akumulirana_dobivka_sredstva { get; set; }
        public string bilans_akumulirana_dobivka_2013 { get; set; }
        public string bilans_akumulirana_dobivka_2012 { get; set; }
        public string bilans_akumulirana_dobivka_ind { get; set; }
        public string bilans_akumulirana_dobivka_2013_procent { get; set; }
        public string bilans_akumulirana_dobivka_2012_procent { get; set; }

        public string bilans_prenesena_zaguba_sredstva { get; set; }
        public string bilans_prenesena_zaguba_2013 { get; set; }
        public string bilans_prenesena_zaguba_2012 { get; set; }
        public string bilans_prenesena_zaguba_ind { get; set; }
        public string bilans_prenesena_zaguba_2013_procent { get; set; }
        public string bilans_prenesena_zaguba_2012_procent { get; set; }

        public string bilans_dobivka_delovna_godina_sredstva { get; set; }
        public string bilans_dobivka_delovna_godina_2013 { get; set; }
        public string bilans_dobivka_delovna_godina_2012 { get; set; }
        public string bilans_dobivka_delovna_godina_ind { get; set; }
        public string bilans_dobivka_delovna_godina_2013_procent { get; set; }
        public string bilans_dobivka_delovna_godina_2012_procent { get; set; }

        public string bilans_zaguba_delovna_godina_sredstva { get; set; }
        public string bilans_zaguba_delovna_godina_2013 { get; set; }
        public string bilans_zaguba_delovna_godina_2012 { get; set; }
        public string bilans_zaguba_delovna_godina_ind { get; set; }
        public string bilans_zaguba_delovna_godina_2013_procent { get; set; }
        public string bilans_zaguba_delovna_godina_2012_procent { get; set; }

        public string bilans_obvrski_sredstva { get; set; }
        public string bilans_obvrski_2013 { get; set; }
        public string bilans_obvrski_2012 { get; set; }
        public string bilans_obvrski_ind { get; set; }
        public string bilans_obvrski_2013_procent { get; set; }
        public string bilans_obvrski_2012_procent { get; set; }

        public string bilans_dolgorocni_rezerviranja_sredstva { get; set; }
        public string bilans_dolgorocni_rezerviranja_2013 { get; set; }
        public string bilans_dolgorocni_rezerviranja_2012 { get; set; }
        public string bilans_dolgorocni_rezerviranja_ind { get; set; }
        public string bilans_dolgorocni_rezerviranja_2013_procent { get; set; }
        public string bilans_dolgorocni_rezerviranja_2012_procent { get; set; }

        public string bilans_dolgorocni_obvrski_sredstva { get; set; }
        public string bilans_dolgorocni_obvrski_2013 { get; set; }
        public string bilans_dolgorocni_obvrski_2012 { get; set; }
        public string bilans_dolgorocni_obvrski_ind { get; set; }
        public string bilans_dolgorocni_obvrski_2013_procent { get; set; }
        public string bilans_dolgorocni_obvrski_2012_procent { get; set; }

        public string bilans_kratkorocni_obvrski_sredstva { get; set; }
        public string bilans_kratkorocni_obvrski_2013 { get; set; }
        public string bilans_kratkorocni_obvrski_2012 { get; set; }
        public string bilans_kratkorocni_obvrski_ind { get; set; }
        public string bilans_kratkorocni_obvrski_2013_procent { get; set; }
        public string bilans_kratkorocni_obvrski_2012_procent { get; set; }

        public string bilans_odlozeni_obvrski_sredstva { get; set; }
        public string bilans_odlozeni_obvrski_2013 { get; set; }
        public string bilans_odlozeni_obvrski_2012 { get; set; }
        public string bilans_odlozeni_obvrski_ind { get; set; }
        public string bilans_odlozeni_obvrski_2013_procent { get; set; }
        public string bilans_odlozeni_obvrski_2012_procent { get; set; }

        public string bilans_odlozeno_plakanje_sredstva { get; set; }
        public string bilans_odlozeno_plakanje_2013 { get; set; }
        public string bilans_odlozeno_plakanje_2012 { get; set; }
        public string bilans_odlozeno_plakanje_ind { get; set; }
        public string bilans_odlozeno_plakanje_2013_procent { get; set; }
        public string bilans_odlozeno_plakanje_2012_procent { get; set; }

        public string bilans_obvrski_po_osnov_sredstva { get; set; }
        public string bilans_obvrski_po_osnov_2013 { get; set; }
        public string bilans_obvrski_po_osnov_2012 { get; set; }
        public string bilans_obvrski_po_osnov_ind { get; set; }
        public string bilans_obvrski_po_osnov_2013_procent { get; set; }
        public string bilans_obvrski_po_osnov_2012_procent { get; set; }

        public string bilans_vkupna_pasiva_sredstva { get; set; }
        public string bilans_vkupna_pasiva_2013 { get; set; }
        public string bilans_vkupna_pasiva_2012 { get; set; }
        public string bilans_vkupna_pasiva_ind { get; set; }
        public string bilans_vkupna_pasiva_2013_procent { get; set; }
        public string bilans_vkupna_pasiva_2012_procent { get; set; }

        public string bilans_kratkorocni_sredstva_sredstva { get; set; }
        public string bilans_kratkorocni_sredstva_2013 { get; set; }
        public string bilans_kratkorocni_sredstva_2012 { get; set; }
        public string bilans_kratkorocni_sredstva_ind { get; set; }
        public string bilans_kratkorocni_sredstva_2013_procent { get; set; }
        public string bilans_kratkorocni_sredstva_2012_procent { get; set; }


        public string uspeh_prihodi_rabotenje_sredstva { get; set; }
        public string uspeh_prihodi_rabotenje_2013 { get; set; }
        public string uspeh_prihodi_rabotenje_2012 { get; set; }
        public string uspeh_prihodi_rabotenje_ind { get; set; }
        public string uspeh_prihodi_rabotenje_2013_procent { get; set; }
        public string uspeh_prihodi_rabotenje_2012_procent { get; set; }

        public string uspeh_finansiski_prihodi_sredstva { get; set; }
        public string uspeh_finansiski_prihodi_2013 { get; set; }
        public string uspeh_finansiski_prihodi_2012 { get; set; }
        public string uspeh_finansiski_prihodi_ind { get; set; }
        public string uspeh_finansiski_prihodi_2013_procent { get; set; }
        public string uspeh_finansiski_prihodi_2012_procent { get; set; }

        public string uspeh_vkupno_prihodi_sredstva { get; set; }
        public string uspeh_vkupno_prihodi_2013 { get; set; }
        public string uspeh_vkupno_prihodi_2012 { get; set; }
        public string uspeh_vkupno_prihodi_ind { get; set; }
        public string uspeh_vkupno_prihodi_2013_procent { get; set; }
        public string uspeh_vkupno_prihodi_2012_procent { get; set; }

        public string uspeh_rashodi_rabotenje_sredstva { get; set; }
        public string uspeh_rashodi_rabotenje_2013 { get; set; }
        public string uspeh_rashodi_rabotenje_2012 { get; set; }
        public string uspeh_rashodi_rabotenje_ind { get; set; }
        public string uspeh_rashodi_rabotenje_2013_procent { get; set; }
        public string uspeh_rashodi_rabotenje_2012_procent { get; set; }

        public string uspeh_rashod_osnovna_dejnost_sredstva { get; set; }
        public string uspeh_rashod_osnovna_dejnost_2013 { get; set; }
        public string uspeh_rashod_osnovna_dejnost_2012 { get; set; }
        public string uspeh_rashod_osnovna_dejnost_ind { get; set; }
        public string uspeh_rashod_osnovna_dejnost_2013_procent { get; set; }
        public string uspeh_rashod_osnovna_dejnost_2012_procent { get; set; }

        public string uspeh_ostanati_trosoci_sredstva { get; set; }
        public string uspeh_ostanati_trosoci_2013 { get; set; }
        public string uspeh_ostanati_trosoci_2012 { get; set; }
        public string uspeh_ostanati_trosoci_ind { get; set; }
        public string uspeh_ostanati_trosoci_2013_procent { get; set; }
        public string uspeh_ostanati_trosoci_2012_procent { get; set; }

        public string uspeh_trosoci_za_vraboteni_sredstva { get; set; }
        public string uspeh_trosoci_za_vraboteni_2013 { get; set; }
        public string uspeh_trosoci_za_vraboteni_2012 { get; set; }
        public string uspeh_trosoci_za_vraboteni_ind { get; set; }
        public string uspeh_trosoci_za_vraboteni_2013_procent { get; set; }
        public string uspeh_trosoci_za_vraboteni_2012_procent { get; set; }

        public string uspeh_amortizacija_sredstva_sredstva { get; set; }
        public string uspeh_amortizacija_sredstva_2013 { get; set; }
        public string uspeh_amortizacija_sredstva_2012 { get; set; }
        public string uspeh_amortizacija_sredstva_ind { get; set; }
        public string uspeh_amortizacija_sredstva_2013_procent { get; set; }
        public string uspeh_amortizacija_sredstva_2012_procent { get; set; }

        public string uspeh_rezerviranje_trosoci_rizici_sredstva { get; set; }
        public string uspeh_rezerviranje_trosoci_rizici_2013 { get; set; }
        public string uspeh_rezerviranje_trosoci_rizici_2012 { get; set; }
        public string uspeh_rezerviranje_trosoci_rizici_ind { get; set; }
        public string uspeh_rezerviranje_trosoci_rizici_2013_procent { get; set; }
        public string uspeh_rezerviranje_trosoci_rizici_2012_procent { get; set; }

        public string uspeh_zalihi_proizvodi_pocetok_sredstva { get; set; }
        public string uspeh_zalihi_proizvodi_pocetok_2013 { get; set; }
        public string uspeh_zalihi_proizvodi_pocetok_2012 { get; set; }
        public string uspeh_zalihi_proizvodi_pocetok_ind { get; set; }
        public string uspeh_zalihi_proizvodi_pocetok_2013_procent { get; set; }
        public string uspeh_zalihi_proizvodi_pocetok_2012_procent { get; set; }

        public string uspeh_zalihi_proizvodi_kraj_sredstva { get; set; }
        public string uspeh_zalihi_proizvodi_kraj_2013 { get; set; }
        public string uspeh_zalihi_proizvodi_kraj_2012 { get; set; }
        public string uspeh_zalihi_proizvodi_kraj_ind { get; set; }
        public string uspeh_zalihi_proizvodi_kraj_2013_procent { get; set; }
        public string uspeh_zalihi_proizvodi_kraj_2012_procent { get; set; }

        public string uspeh_ostanati_rashodi_sredstva { get; set; }
        public string uspeh_ostanati_rashodi_2013 { get; set; }
        public string uspeh_ostanati_rashodi_2012 { get; set; }
        public string uspeh_ostanati_rashodi_ind { get; set; }
        public string uspeh_ostanati_rashodi_2013_procent { get; set; }
        public string uspeh_ostanati_rashodi_2012_procent { get; set; }

        public string uspeh_finansiski_rashodi_sredstva { get; set; }
        public string uspeh_finansiski_rashodi_2013 { get; set; }
        public string uspeh_finansiski_rashodi_2012 { get; set; }
        public string uspeh_finansiski_rashodi_ind { get; set; }
        public string uspeh_finansiski_rashodi_2013_procent { get; set; }
        public string uspeh_finansiski_rashodi_2012_procent { get; set; }

        public string uspeh_finansiski_povrzani_drustva_sredstva { get; set; }
        public string uspeh_finansiski_povrzani_drustva_2013 { get; set; }
        public string uspeh_finansiski_povrzani_drustva_2012 { get; set; }
        public string uspeh_finansiski_povrzani_drustva_ind { get; set; }
        public string uspeh_finansiski_povrzani_drustva_2013_procent { get; set; }
        public string uspeh_finansiski_povrzani_drustva_2012_procent { get; set; }

        public string uspeh_rashodi_kamati_sredstva { get; set; }
        public string uspeh_rashodi_kamati_2013 { get; set; }
        public string uspeh_rashodi_kamati_2012 { get; set; }
        public string uspeh_rashodi_kamati_ind { get; set; }
        public string uspeh_rashodi_kamati_2013_procent { get; set; }
        public string uspeh_rashodi_kamati_2012_procent { get; set; }

        public string uspeh_rashodi_finansiski_sredstva_sredstva { get; set; }
        public string uspeh_rashodi_finansiski_sredstva_2013 { get; set; }
        public string uspeh_rashodi_finansiski_sredstva_2012 { get; set; }
        public string uspeh_rashodi_finansiski_sredstva_ind { get; set; }
        public string uspeh_rashodi_finansiski_sredstva_2013_procent { get; set; }
        public string uspeh_rashodi_finansiski_sredstva_2012_procent { get; set; }

        public string uspeh_ostanati_finansiski_rashodi_sredstva { get; set; }
        public string uspeh_ostanati_finansiski_rashodi_2013 { get; set; }
        public string uspeh_ostanati_finansiski_rashodi_2012 { get; set; }
        public string uspeh_ostanati_finansiski_rashodi_ind { get; set; }
        public string uspeh_ostanati_finansiski_rashodi_2013_procent { get; set; }
        public string uspeh_ostanati_finansiski_rashodi_2012_procent { get; set; }

        public string uspeh_udel_vo_zaguba_sredstva { get; set; }
        public string uspeh_udel_vo_zaguba_2013 { get; set; }
        public string uspeh_udel_vo_zaguba_2012 { get; set; }
        public string uspeh_udel_vo_zaguba_ind { get; set; }
        public string uspeh_udel_vo_zaguba_2013_procent { get; set; }
        public string uspeh_udel_vo_zaguba_2012_procent { get; set; }

        public string uspeh_udel_vo_dobivka_sredstva { get; set; }
        public string uspeh_udel_vo_dobivka_2013 { get; set; }
        public string uspeh_udel_vo_dobivka_2012 { get; set; }
        public string uspeh_udel_vo_dobivka_ind { get; set; }
        public string uspeh_udel_vo_dobivka_2013_procent { get; set; }
        public string uspeh_udel_vo_dobivka_2012_procent { get; set; }

        public string uspeh_vkupno_rashodi_sredstva { get; set; }
        public string uspeh_vkupno_rashodi_2013 { get; set; }
        public string uspeh_vkupno_rashodi_2012 { get; set; }
        public string uspeh_vkupno_rashodi_ind { get; set; }
        public string uspeh_vkupno_rashodi_2013_procent { get; set; }
        public string uspeh_vkupno_rashodi_2012_procent { get; set; }

        public string uspeh_dobivka_odanocuvanje_sredstva { get; set; }
        public string uspeh_dobivka_odanocuvanje_2013 { get; set; }
        public string uspeh_dobivka_odanocuvanje_2012 { get; set; }
        public string uspeh_dobivka_odanocuvanje_ind { get; set; }

        public string uspeh_zaguba_odanocuvanje_sredstva { get; set; }
        public string uspeh_zaguba_odanocuvanje_2013 { get; set; }
        public string uspeh_zaguba_odanocuvanje_2012 { get; set; }
        public string uspeh_zaguba_odanocuvanje_ind { get; set; }

        public string uspeh_danok_dobivka_sredstva { get; set; }
        public string uspeh_danok_dobivka_2013 { get; set; }
        public string uspeh_danok_dobivka_2012 { get; set; }
        public string uspeh_danok_dobivka_ind { get; set; }

        public string uspeh_neto_dobivka_sredstva { get; set; }
        public string uspeh_neto_dobivka_2013 { get; set; }
        public string uspeh_neto_dobivka_2012 { get; set; }
        public string uspeh_neto_dobivka_ind { get; set; }

        public string uspeh_neto_zaguba_sredstva { get; set; }
        public string uspeh_neto_zaguba_2013 { get; set; }
        public string uspeh_neto_zaguba_2012 { get; set; }
        public string uspeh_neto_zaguba_ind { get; set; }


        public string indikatori_raboten_kapital_2013 { get; set; }
        public string indikatori_raboten_kapital_2012 { get; set; }
        public string indikatori_raboten_kapital_rast { get; set; }

        public string indikatori_tekoven_pokazatel_2013 { get; set; }
        public string indikatori_tekoven_pokazatel_2012 { get; set; }
        public string indikatori_tekoven_pokazatel_rast { get; set; }

        public string indikatori_brz_pokazatel_2013 { get; set; }
        public string indikatori_brz_pokazatel_2012 { get; set; }
        public string indikatori_brz_pokazatel_rast { get; set; }

        public string indikatori_obrt_sredstva_2013 { get; set; }
        public string indikatori_obrt_sredstva_2012 { get; set; }
        public string indikatori_obrt_sredstva_rast { get; set; }

        public string indikatori_denovi_obrt_sredstva_2013 { get; set; }
        public string indikatori_denovi_obrt_sredstva_2012 { get; set; }
        public string indikatori_denovi_obrt_sredstva_rast { get; set; }

        public string indikatori_obrt_obvrski_2013 { get; set; }
        public string indikatori_obrt_obvrski_2012 { get; set; }
        public string indikatori_obrt_obvrski_rast { get; set; }

        public string indikatori_prosecni_denovi_obvrski_2013 { get; set; }
        public string indikatori_prosecni_denovi_obvrski_2012 { get; set; }
        public string indikatori_prosecni_denovi_obvrski_rast { get; set; }

        public string indikatori_obrt_pobaruvanja_2013 { get; set; }
        public string indikatori_obrt_pobaruvanja_2012 { get; set; }
        public string indikatori_obrt_pobaruvanja_rast { get; set; }

        public string indikatori_denovi_obrt_pobaruvanja_2013 { get; set; }
        public string indikatori_denovi_obrt_pobaruvanja_2012 { get; set; }
        public string indikatori_denovi_obrt_pobaruvanja_rast { get; set; }

        public string indikatori_obrt_zalihi_2013 { get; set; }
        public string indikatori_obrt_zalihi_2012 { get; set; }
        public string indikatori_obrt_zalihi_rast { get; set; }

        public string indikatori_denovi_obrt_zalihi_2013 { get; set; }
        public string indikatori_denovi_obrt_zalihi_2012 { get; set; }
        public string indikatori_denovi_obrt_zalihi_rast { get; set; }

        public string indikatori_povrat_kapital_2013 { get; set; }
        public string indikatori_povrat_kapital_2012 { get; set; }
        public string indikatori_povrat_kapital_rast { get; set; }

        public string indikatori_povrat_sredstva_2013 { get; set; }
        public string indikatori_povrat_sredstva_2012 { get; set; }
        public string indikatori_povrat_sredstva_rast { get; set; }

        public string indikatori_neto_profitna_margina_2013 { get; set; }
        public string indikatori_neto_profitna_margina_2012 { get; set; }
        public string indikatori_neto_profitna_margina_rast { get; set; }

        public string indikatori_finansiski_leviridz_2013 { get; set; }
        public string indikatori_finansiski_leviridz_2012 { get; set; }
        public string indikatori_finansiski_leviridz_rast { get; set; }

        public string indikatori_koeficient_zadolzenost_2013 { get; set; }
        public string indikatori_koeficient_zadolzenost_2012 { get; set; }
        public string indikatori_koeficient_zadolzenost_rast { get; set; }

        public string indikatori_vkupni_obvrski_2013 { get; set; }
        public string indikatori_vkupni_obvrski_2012 { get; set; }
        public string indikatori_vkupni_obvrski_rast { get; set; }

        public string indikatori_pokrienost_servisiranje_2013 { get; set; }
        public string indikatori_pokrienost_servisiranje_2012 { get; set; }
        public string indikatori_pokrienost_servisiranje_rast { get; set; }

        public string indikatori_pokrienost_kamati_2013 { get; set; }
        public string indikatori_pokrienost_kamati_2012 { get; set; }
        public string indikatori_pokrienost_kamati_rast { get; set; }

        public string indikatori_kratkorocni_krediti_2013 { get; set; }
        public string indikatori_kratkorocni_krediti_2012 { get; set; }
        public string indikatori_kratkorocni_krediti_rast { get; set; }

        public string indikatori_tekovni_obvrski_2013 { get; set; }
        public string indikatori_tekovni_obvrski_2012 { get; set; }
        public string indikatori_tekovni_obvrski_rast { get; set; }


        public string tekovi_neto_profit_odanocuvanje { get; set; }
        public string tekovi_zaguba_odanocuvanje { get; set; }
        public string tekovi_amortizacija { get; set; }
        public string tekovi_prilivi_gotovina_aktivnosti { get; set; }
        public string tekovi_odlivi_gotovina_aktivnosti { get; set; }
        public string tekovi_neto_prilivi_gotovina_aktivnosti { get; set; }
        public string tekovi_neto_odlivi_gotovina_aktivnosti { get; set; }
        public string tekovi_prilivi_gotovina_investicioni { get; set; }
        public string tekovi_odlivi_gotovina_investicioni { get; set; }
        public string tekovi_neto_prilivi_gotovina_investicioni { get; set; }
        public string tekovi_neto_odlivi_gotovina_investicioni { get; set; }
        public string tekovi_prilivi_gotovina_finansiski { get; set; }
        public string tekovi_odlivi_gotovina_finansiski { get; set; }
        public string tekovi_neto_prilivi_gotovina_finansiski { get; set; }
        public string tekovi_neto_odlivi_gotovina_finansiski { get; set; }
        public string tekovi_vkupno_prilivi_gotovina { get; set; }
        public string tekovi_vkupno_odlivi_gotovina { get; set; }
        public string tekovi_vkupno_neto_prilivi { get; set; }
        public string tekovi_vkupno_neto_odlivi { get; set; }
        public string tekovi_paricni_sredstva_pocetok { get; set; }
        public string tekovi_paricni_sredstva_kraj { get; set; }

        public string bar_chart_filename { get; set; }
        public string pie_chart_filename1 { get; set; }
        public string pie_chart_filename2 { get; set; }
        public string pie_chart_filename3 { get; set; }
        public string pie_chart_filename4 { get; set; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }

    public enum F_PieCharts
    {
        Aktiva = 11,
        Pasiva = 12,
        Prihodi = 13,
        Rashodi = 14
    }
}

