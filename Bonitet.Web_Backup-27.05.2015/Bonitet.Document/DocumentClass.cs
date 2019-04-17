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

namespace Bonitet.Document
{
    public class DocumentClass
    {
        public static string webRootPath = System.Web.HttpContext.Current.Server.MapPath("~");

        public static string AbsolutePath = Path.GetFullPath(Path.Combine(webRootPath, "..\\Bonitet.Web\\App_Data\\"));

        public static string AbsoluteUrlPath = Path.GetFullPath(Path.Combine(webRootPath, "..\\Bonitet.Web\\"));

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

        public static Report GetReportByUserCompanyYearAndReport(int UserID, string EMBS, int Year, int ReportType)
        {
            var report = DALHelper.GetReportByUserCompanyYearAndReportFromDB(UserID, EMBS, Year, ReportType);

            return report;
        }

        public static string GenerateHTMLFromDataForReport(Attributes data, int ReportType)
        {
            var html = "";
            if (ReportType == 1)
            {
                html = DocumentClass.CoverPage(data);

                html += DocumentClass.FirstPage(data);
                html += DocumentClass.SecondPage(data);
                html += DocumentClass.ThirdPage(data);
                html += DocumentClass.FourthPage(data);
                html += DocumentClass.FifthPage(data);
                html += DocumentClass.SixthPage(data);
                html += DocumentClass.SeventhPage(data);
                html += DocumentClass.EighthPage(data);
                html += DocumentClass.NinthPage(data);
                html += DocumentClass.TenthPage(data);
            }
            else if (ReportType == 2)
            {
                html = DocumentClass.ShortReportPage(data);
            }
            return html;
        }

        public static string GenerateReport(int UserID, int CompanyID, string EMBS, int Year, int ReportType, int PackID)
        {
            var company = new Company();
            if (EMBS.Length > 0)
            {
                company = DALHelper.GetCompanyByEMBS(EMBS);
            }
            else if (CompanyID != 0)
            {
                company = DALHelper.GetCompanyByID1(CompanyID);
            }
            if (company != null)
            {
                var data = new Attributes();
                var HTML = "";
                if (ReportType == 1)
                    data = SetReport1Data(UserID, company.EMBS, Year);
                else if (ReportType == 2)
                    data = SetReport2Data(UserID, company.EMBS, Year);
                //else if (ReportType == 3)
                //{
                //    HTML = SetReport3Data(UserID, company.EMBS);
               // }

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
                return null;
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
                var DejnostSifraID = dejnost.PK_Dejnost;

                company_dejnost = DALHelper.GetDejnostSifraForCompany(DejnostSifraID);

            }

            if(company_dejnost != null)
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

            data.sr_vkupni_prihodi = FormatCurrency(res["Вкупни приходи"].Value);
            data.sr_vkupni_rashodi = FormatCurrency(res["Вкупни расходи"].Value);
            data.sr_dobivka_za_finansiska_godina = FormatCurrency(res["Добивка за финансиска година"].Value);
            data.sr_zaguba_za_finansiska_godina = FormatCurrency(res["Загуба за финансиска година"].Value);
            data.sr_prosecen_broj_vraboteni = FormatCurrency(res["Просечен број на вработени"].Value);
            data.sr_netekovni_sredstva = FormatCurrency(res["НЕТЕКОВНИ СРЕДСТВА"].Value);
            data.sr_odlozeni_danocni_obvrski_aktiva = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА"].Value);
            data.sr_tekovni_sredstva = FormatCurrency(res["ТЕКОВНИ СРЕДСТВА"].Value);
            data.sr_zalihi = FormatCurrency(res["ЗАЛИХИ"].Value);
            data.sr_sredstva_ili_grupi_za_otugjuvanje = FormatCurrency(res["СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)"].Value);
            data.sr_plateni_trosoci_za_idni_periodi = FormatCurrency(res["ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)"].Value);
            data.sr_d_vkupna_aktiva = FormatCurrency(res["Д. ВКУПНА АКТИВА"].Value);
            data.sr_glavnina_i_rezervi = FormatCurrency(res["ГЛАВНИНА И РЕЗЕРВИ"].Value);
            data.sr_obvrski = FormatCurrency(res["ОБВРСКИ"].Value);
            data.sr_odlozeni_danocni_obvrski_pasiva = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА"].Value);
            data.sr_odlozeno_plakanje_trosoci = FormatCurrency(res["ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)"].Value);
            data.sr_obvrski_po_osnov_na_netekovni_sredstva = FormatCurrency(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА"].Value);
            data.sr_d_vkupna_pasiva = FormatCurrency(res["Д. ВКУПНО ПАСИВА"].Value);

            data.sr_vkupni_prihodi_lastyear = FormatCurrency(res["Вкупни приходи_LastYear"].Value);
            data.sr_vkupni_rashodi_lastyear = FormatCurrency(res["Вкупни расходи_LastYear"].Value);
            data.sr_dobivka_za_finansiska_godina_lastyear = FormatCurrency(res["Добивка за финансиска година_LastYear"].Value);
            data.sr_zaguba_za_finansiska_godina_lastyear = FormatCurrency(res["Загуба за финансиска година_LastYear"].Value);
            data.sr_prosecen_broj_vraboteni_lastyear = FormatCurrency(res["Просечен број на вработени_LastYear"].Value);
            data.sr_netekovni_sredstva_lastyear = FormatCurrency(res["НЕТЕКОВНИ СРЕДСТВА_LastYear"].Value);
            data.sr_odlozeni_danocni_obvrski_aktiva_lastyear = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_LastYear"].Value);
            data.sr_tekovni_sredstva_lastyear = FormatCurrency(res["ТЕКОВНИ СРЕДСТВА_LastYear"].Value);
            data.sr_zalihi_lastyear = FormatCurrency(res["ЗАЛИХИ_LastYear"].Value);
            data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear = FormatCurrency(res["СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_LastYear"].Value);
            data.sr_plateni_trosoci_za_idni_periodi_lastyear = FormatCurrency(res["ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_LastYear"].Value);
            data.sr_d_vkupna_aktiva_lastyear = FormatCurrency(res["Д. ВКУПНА АКТИВА_LastYear"].Value);
            data.sr_glavnina_i_rezervi_lastyear = FormatCurrency(res["ГЛАВНИНА И РЕЗЕРВИ_LastYear"].Value);
            data.sr_obvrski_lastyear = FormatCurrency(res["ОБВРСКИ_LastYear"].Value);
            data.sr_odlozeni_danocni_obvrski_pasiva_lastyear = FormatCurrency(res["ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_LastYear"].Value);
            data.sr_odlozeno_plakanje_trosoci_lastyear = FormatCurrency(res["ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_LastYear"].Value);
            data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear = FormatCurrency(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_LastYear"].Value);
            data.sr_d_vkupna_pasiva_lastyear = FormatCurrency(res["Д. ВКУПНО ПАСИВА_LastYear"].Value);

            data.sr_vkupni_prihodi_ind = CalculateGrowth(Convert.ToDouble(data.sr_vkupni_prihodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_prihodi_lastyear.Replace(".", ""))).ToString();
            data.sr_vkupni_rashodi_ind = CalculateGrowth(Convert.ToDouble(data.sr_vkupni_rashodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_rashodi_lastyear.Replace(".", ""))).ToString();
            data.sr_dobivka_za_finansiska_godina_ind = CalculateGrowth(Convert.ToDouble(data.sr_dobivka_za_finansiska_godina.Replace(".", "")), Convert.ToDouble(data.sr_dobivka_za_finansiska_godina_lastyear.Replace(".", ""))).ToString();
            data.sr_zaguba_za_finansiska_godina_ind = CalculateGrowth(Convert.ToDouble(data.sr_zaguba_za_finansiska_godina.Replace(".", "")), Convert.ToDouble(data.sr_zaguba_za_finansiska_godina_lastyear.Replace(".", ""))).ToString();
            data.sr_prosecen_broj_vraboteni_ind = CalculateGrowth(Convert.ToDouble(data.sr_prosecen_broj_vraboteni.Replace(".", "")), Convert.ToDouble(data.sr_prosecen_broj_vraboteni_lastyear.Replace(".", ""))).ToString();
            data.sr_netekovni_sredstva_ind = CalculateGrowth(Convert.ToDouble(data.sr_netekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_netekovni_sredstva_lastyear.Replace(".", ""))).ToString();
            data.sr_odlozeni_danocni_obvrski_aktiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_tekovni_sredstva_ind = CalculateGrowth(Convert.ToDouble(data.sr_tekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_tekovni_sredstva_lastyear.Replace(".", ""))).ToString();
            data.sr_zalihi_ind = CalculateGrowth(Convert.ToDouble(data.sr_zalihi.Replace(".", "")), Convert.ToDouble(data.sr_zalihi_lastyear.Replace(".", ""))).ToString();
            data.sr_sredstva_ili_grupi_za_otugjuvanje_ind = CalculateGrowth(Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje.Replace(".", "")), Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear.Replace(".", ""))).ToString();
            data.sr_plateni_trosoci_za_idni_periodi_ind = CalculateGrowth(Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi.Replace(".", "")), Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi_lastyear.Replace(".", ""))).ToString();
            data.sr_d_vkupna_aktiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_glavnina_i_rezervi_ind = CalculateGrowth(Convert.ToDouble(data.sr_glavnina_i_rezervi.Replace(".", "")), Convert.ToDouble(data.sr_glavnina_i_rezervi_lastyear.Replace(".", ""))).ToString();
            data.sr_obvrski_ind = CalculateGrowth(Convert.ToDouble(data.sr_obvrski.Replace(".", "")), Convert.ToDouble(data.sr_obvrski_lastyear.Replace(".", ""))).ToString();
            data.sr_odlozeni_danocni_obvrski_pasiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva.Replace(".", "")), Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva_lastyear.Replace(".", ""))).ToString();
            data.sr_odlozeno_plakanje_trosoci_ind = CalculateGrowth(Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci.Replace(".", "")), Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci_lastyear.Replace(".", ""))).ToString();
            data.sr_obvrski_po_osnov_na_netekovni_sredstva_ind = CalculateGrowth(Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear.Replace(".", ""))).ToString();
            data.sr_d_vkupna_pasiva_ind = CalculateGrowth(Convert.ToDouble(data.sr_d_vkupna_pasiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_pasiva_lastyear.Replace(".", ""))).ToString();

            data.sr_netekovni_sredstva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_netekovni_sredstva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_netekovni_sredstva_procent = CalculatePercent(Convert.ToDouble(data.sr_netekovni_sredstva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_odlozeni_danocni_obvrski_aktiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_odlozeni_danocni_obvrski_aktiva_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_tekovni_sredstva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_tekovni_sredstva_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_zalihi_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_zalihi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_zalihi_procent = CalculatePercent(Convert.ToDouble(data.sr_zalihi.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_sredstva_ili_grupi_za_otugjuvanje_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_sredstva_ili_grupi_za_otugjuvanje_procent = CalculatePercent(Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_plateni_trosoci_za_idni_periodi_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_plateni_trosoci_za_idni_periodi_procent = CalculatePercent(Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_d_vkupna_aktiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_d_vkupna_aktiva_procent = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_glavnina_i_rezervi_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_glavnina_i_rezervi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_glavnina_i_rezervi_procent = CalculatePercent(Convert.ToDouble(data.sr_glavnina_i_rezervi.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_obvrski_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_obvrski_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_obvrski_procent = CalculatePercent(Convert.ToDouble(data.sr_obvrski.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_odlozeni_danocni_obvrski_pasiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_odlozeni_danocni_obvrski_pasiva_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_odlozeno_plakanje_trosoci_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_odlozeno_plakanje_trosoci_procent = CalculatePercent(Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_obvrski_po_osnov_na_netekovni_sredstva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_obvrski_po_osnov_na_netekovni_sredstva_procent = CalculatePercent(Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.sr_d_vkupna_pasiva_procent_lastyear = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_pasiva_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva_lastyear.Replace(".", ""))).ToString();
            data.sr_d_vkupna_pasiva_procent = CalculatePercent(Convert.ToDouble(data.sr_d_vkupna_pasiva.Replace(".", "")), Convert.ToDouble(data.sr_d_vkupna_aktiva.Replace(".", ""))).ToString();

            data.bar_chart_filename = DocumentChart.CreateBarChart(Convert.ToDouble(data.sr_vkupni_rashodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_rashodi_lastyear.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_prihodi.Replace(".", "")), Convert.ToDouble(data.sr_vkupni_prihodi_lastyear.Replace(".", "")));
            
            data.pie_chart_filename1 = DocumentChart.CreatePieChart(1,
                Convert.ToDouble(data.sr_tekovni_sredstva.Replace(".", "")),
                Convert.ToDouble(data.sr_netekovni_sredstva.Replace(".", "")),
                Convert.ToDouble(data.sr_sredstva_ili_grupi_za_otugjuvanje.Replace(".", "")),
                Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_aktiva.Replace(".", "")),
                Convert.ToDouble(data.sr_plateni_trosoci_za_idni_periodi.Replace(".", "")));

            data.pie_chart_filename2 = DocumentChart.CreatePieChart(2,
                Convert.ToDouble(data.sr_obvrski.Replace(".", "")),
                Convert.ToDouble(data.sr_glavnina_i_rezervi.Replace(".", "")),
                Convert.ToDouble(data.sr_obvrski_po_osnov_na_netekovni_sredstva.Replace(".", "")),
                Convert.ToDouble(data.sr_odlozeni_danocni_obvrski_pasiva.Replace(".", "")),
                Convert.ToDouble(data.sr_odlozeno_plakanje_trosoci.Replace(".", "")));

            return data;
        }

        //public static string SetReport3Data(int UserID, string EMBS)
        //{
            //5607051
            //var res = CRM.CRM_ServiceHelper.GetCRM_AccountStatus(EMBS);

            //var html = Bonitet.Document.CRM_DocumentClass.PopulateTemplate(res);

            //return html;
        //}
        private static string FormatCurrency(string Number)
        {
            long a = 0;
            if (long.TryParse(Number, out a))
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("mk-MK"), "{0:N0}", a);

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
            if(res == 0)
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
                        "                        <td class=\"fill_left\"></td>"+
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
                        "                           <td class=\"align_center top_border left_border right_border\">" +  data.sr_vkupni_prihodi_lastyear + "</td>" +
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

        // report 1
        public static Attributes SetReport1Data(int UserID, string EMBS, int Year)
        {
            var COG_Error = false;
            var D_Error = false;
            var CD_Error = false;
            var CRV_Error = false;
            var CP_Error = false;
            var CV1_Error = true;
            var CV2_Error = true;
            var COE_Error = false;

            var company = DALHelper.GetCompanyByEMBS_BM(EMBS);

            var CompanyID = company.PK_Subjekt;

            // organizacioni edinici
            var company_organizacioni_edinici = new List<OrganizacionaEdinicaObject>();
            var coe = DALHelper.GetOrganizacionaEdinicaByID(CompanyID);
            if (coe != null)
            {
                company_organizacioni_edinici = coe;
            }
            else
                COE_Error = true;

            // osnovna glavnina
            var company_osnovna_glavina = DALHelper.GetOsnovnaGalvinaForCompany(CompanyID);
            if (company_osnovna_glavina == null)
                COG_Error = true;

            // dejnost
            var dejnost = DALHelper.GetDejnostForCompany(CompanyID);
            var company_dejnost = new Bonitet.DAL.BiznisMreza.DejnostSifra();
            if (dejnost != null)
            {
                var DejnostSifraID = dejnost.PK_Dejnost;

                company_dejnost = DALHelper.GetDejnostSifraForCompany(DejnostSifraID);
                if (company_dejnost == null)
                    CD_Error = true;
            }
            else
            {
                CD_Error = true;
                D_Error = true;
            }

            // rabotno vreme
            var company_rabotno_vreme = DALHelper.GetRabotnoVremeForCompany(CompanyID);
            if (company_rabotno_vreme == null)
                CRV_Error = true;

            // ovlasteni lica
            var company_povrzanost = DALHelper.GetPovrzanostForCompany(CompanyID);
            var company_ovlasteni_lica = new List<OvlastenoLiceObject>();
            if (company_povrzanost != null)
            {
                company_ovlasteni_lica = company_povrzanost;
            }
            else
                CP_Error = true;

            var res1 = DALHelper.GetCompanyValuesForCurrentYear(EMBS, Year);

            var company_values = new Dictionary<int, double>();

            if (res1 != null && res1.Count > 0)
                company_values = res1.ToDictionary(c => c.ValueID, c => c.Value);
            else
                CV1_Error = true;

            var LastYear = Year - 1;

            var res2 = DALHelper.GetCompanyValuesForLastYear(EMBS, LastYear);

            var company_values_lastyear = new Dictionary<int, double>();

            if (res2 != null && res2.Count > 0)
                company_values_lastyear = res2.ToDictionary(c => c.ValueID, c => c.Value);
            else
                CV2_Error = true;



            var data = new Attributes();

            var A_prihodi_od_prodazba_prosecni_pobaruvanja = "";
            var denovi_na_obrt_na_pobaruvanje = "";
            var B_obrt_na_zalihi = "";
            var denovi_na_obrt_na_zalihi = "";
            var V_obrt_na_sredstva = "";
            var denovi_na_obrt_na_sredstva = "";
            var G_obrt_na_obvrski = "";
            var prosecni_denovi_na_plakanje_obvrski = "";
            //var D_bruto_operativna_dobivka = "";
            //var Gj_EBITDA = "";
            //var E_EBIT = "";
            var vkupni_obvrski_ebitda = "";
            var DSCR = "";
            var pokrienost_na_kamati = "";
            if (1 == 2)
            {
                //// A
                //// просек од 47 за тековната и претходната година
                //var prosek_47 = 0;
                //if (company_values_lastyear.ContainsKey(47))
                //{
                //    prosek_47 = (((int)company_values[47] + (int)company_values_lastyear[47]) / 2);
                //}
                //else
                //    prosek_47 = (int)company_values[47];

                //// Приходи од продажба/просечни побарувања
                //if (prosek_47 != 0)
                //{
                //    A_prihodi_od_prodazba_prosecni_pobaruvanja = ((int)company_values[202] / prosek_47).ToString();
                //}

                ////Денови на обрт на побарувања
                //denovi_na_obrt_na_pobaruvanje = (365 / Convert.ToDouble(A_prihodi_od_prodazba_prosecni_pobaruvanja)).ToString();

                ////Б
                ////просек од 37 за тековната и претходната година
                //var prosek_37 = 0;
                //if (company_values_lastyear.ContainsKey(37))
                //{
                //    prosek_37 = (((int)company_values[37] + (int)company_values_lastyear[37]) / 2);
                //}
                //else
                //    prosek_37 = (int)company_values[37];

                ////Обрт на залихи
                //if (prosek_37 != 0)
                //{
                //    //B_obrt_na_zalihi = (((int)company_values[208] + (int)company_values[209] + (int)company_values[210] + (int)company_values[211]) / prosek_37).ToString();
                //    B_obrt_na_zalihi = "";
                //}

                ////Денови на обрт на залихи
                //denovi_na_obrt_na_zalihi = (365 / Convert.ToDouble(B_obrt_na_zalihi)).ToString();

                ////В
                ////просек од 63 за тековната и претходната година
                //var prosek_63 = 0;
                //if (company_values_lastyear.ContainsKey(63))
                //{
                //    prosek_63 = (((int)company_values[63] + (int)company_values_lastyear[63]) / 2);
                //}
                //else
                //    prosek_63 = (int)company_values[63];

                ////Обрт на средствата
                //if (prosek_63 != 0)
                //{
                //    V_obrt_na_sredstva = ((int)company_values[202] / prosek_63).ToString();
                //}

                ////Денови на обрт на средствата
                //denovi_na_obrt_na_sredstva = (365 / Convert.ToDouble(V_obrt_na_sredstva)).ToString();

                //// Г
                ////(просек од 97 за тековната и претходната година)
                //var prosek_97 = 0;
                //if (company_values_lastyear.ContainsKey(97))
                //{
                //    prosek_97 = (((int)company_values[97] + (int)company_values_lastyear[97]) / 2);
                //}
                //else
                //    prosek_97 = (int)company_values[97];

                //// Обрт на обврските
                //if (prosek_97 != 0)
                //{
                //    G_obrt_na_obvrski = (((int)company_values[207] - (int)company_values[204] + (int)company_values[205]) / prosek_97).ToString();
                //}

                ////Просечни денови на плаќање на обврските спрема добавувачите
                //prosecni_denovi_na_plakanje_obvrski = (365 / Convert.ToDouble(G_obrt_na_obvrski)).ToString();


                ////Д
                ////Бруто оперативна добивка
                ////D_bruto_operativna_dobivka = ((int)company_values[202] - (int)company_values[208] - (int)company_values[209] - (int)company_values[210] - (int)company_values[211]).ToString();
                ////D_bruto_operativna_dobivka = "";

                ////Ѓ
                ////ЕБИТДА
                ////Gj_EBITDA = ((int)company_values[250] - (int)company_values[251] + (int)company_values[239] + (int)company_values[236] + (int)company_values[218] + (int)company_values[219] + (int)company_values[220]).ToString();
                ////Gj_EBITDA = "";

                ////Е
                ////ЕБИТ
                ////E_EBIT = ((int)company_values[250] - (int)company_values[251] + (int)company_values[239] + (int)company_values[236]).ToString();
                ////E_EBIT = "";


                ////Вкупни обврски / ЕБИТДА
                ////vkupni_obvrski_ebitda = ((int)company_values[95] + (int)company_values[85] / Convert.ToDouble(Gj_EBITDA)).ToString();
                //vkupni_obvrski_ebitda = "";

                ////Индикатор за способност за сервисирање на долг - DSCR(debt servicing coverage ratio)
                ////DSCR = ((Convert.ToDouble(D_bruto_operativna_dobivka) + (int)company_values[218] + (int)company_values[236] + (int)company_values[239]) / (((int)company_values[85] / (int)company_values[4]) + (int)company_values[103] + (int)company_values[104] + (int)company_values[236] + (int)company_values[239])).ToString();
                //DSCR = "";

                ////Покриеност на камати
                ////pokrienost_na_kamati = (Convert.ToDouble(E_EBIT) / ((int)company_values[236] + (int)company_values[239])).ToString()
                //pokrienost_na_kamati = "";
            }



            data.ime_firma = company.KratkoIme;
            data.drzava = "";
            data.datum_izdavanje = DateTime.Now.ToShortDateString();
            data.izdaden_za = "";

            data.uplaten_del = (COG_Error == false) ? company_osnovna_glavina.UplatenDel.ToString() : "";
            data.neparicen_vlog = (COG_Error == false) ? company_osnovna_glavina.NeparicenVlog.ToString() : "";

            data.tekovni_sopstvenici = "";
            data.tekovni_podruznici = "";
            data.tekovni_blokadi_status = "";

            data.sostojba_komentar = "";
            data.semafor_solventnost = "green";
            data.solventnost_komentar = "";

            data.promeni_minata_godina = "";
            data.solventnost_minata_godina = "";

            data.kazni_minata_godina = "";
            data.sankcii_minata_godina = "";

            data.naziv_firma = company.KratkoIme;
            data.celosen_naziv_firma = company.CelosenNazivNaSubjektot;
            data.adresa_firma = company.Sediste;
            data.pravna_forma = "";
            data.dejnost = (CD_Error == false) ? company_dejnost.PrioritetnaDejnost.ToString() : "";
            data.embs = company.EMBS;
            data.edb = company.EdinstvenDanocenBroj;
            data.golemina_na_subjekt = company.GoleminaNaDelovniotSubjekt;
            data.vkupna_osnovna_glavnina = (COG_Error == false) ? company_osnovna_glavina.VkupnoOsnovnaGlavina.ToString() : "";
            data.datum_osnovanje = company.DatumNaOsnovanje.ToString();
            data.registrirana_vo = "";
            data.prosecen_broj_vraboteni = (CRV_Error == false) ? company_rabotno_vreme.BrojNaVraboteni.ToString() : "";
            data.region = "";
            data.kapital = "";
            data.ddv_obvrznik = "";

            if (CP_Error == false)
            {
                data.ovlasteni_lica = company_ovlasteni_lica;
            }

            data.banka_smetka1 = "";
            data.banka_ime1 = "";
            data.banka_smetka2 = "";
            data.banka_ime2 = "";
            data.banka_smetka3 = "";
            data.banka_ime3 = "";
            data.banka_smetka4 = "";
            data.banka_ime4 = "";
            data.banka_smetka5 = "";
            data.banka_ime5 = "";
            data.banka_smetka6 = "";
            data.banka_ime6 = "";
            data.banka_smetka7 = "";
            data.banka_ime7 = "";
            data.banka_smetka8 = "";
            data.banka_ime8 = "";
            data.banka_smetka9 = "";
            data.banka_ime9 = "";

            data.vkupen_broj_sopstvenici = "";
            data.prikazan_broj_sopstvenici = "";

            data.vkupen_broj_ovlasteni = "";
            data.prikazan_broj_ovlasteni = "";

            data.prikazan_broj_podruznici = "";
            data.vkupen_broj_podruznici = "";

            data.kategorija1 = "2012";
            data.sredstva1 = (CV2_Error == false) ? company_values_lastyear[1].ToString() : "";
            data.kapital1 = (CV2_Error == false) ? company_values_lastyear[65].ToString() : "";
            data.vkupno_prihodi1 = (CV2_Error == false) ? (company_values_lastyear[201] + company_values_lastyear[223] + company_values_lastyear[244]).ToString() : "";
            //data.neto_dobivka_za_delovna_godina1 = (CV2_Error == false) ? (company_values_lastyear[255] - company_values_lastyear[256]).ToString() : ""
            data.neto_dobivka_za_delovna_godina1 = "";
            data.prosecen_broj_vraboteni1 = (CV2_Error == false) ? company_values_lastyear[257].ToString() : "";
            data.koeficient_na_zadolzensot1 = (CV2_Error == false) ? ((company_values_lastyear[85] + company_values_lastyear[95]) / company_values_lastyear[1]).ToString() : "";
            data.tekoven_pokazatel1 = (CV2_Error == false) ? (company_values_lastyear[32] / company_values_lastyear[95]).ToString() : "";
            //data.povrat_na_sredstva1 = (CV2_Error == false) ? ((company_values_lastyear[255] - company_values_lastyear[256]) / company_values_lastyear[1]).ToString() : "";
            //data.povrat_na_kapital1 = (CV2_Error == false) ? ((company_values_lastyear[255] - company_values_lastyear[256]) / company_values_lastyear[65]).ToString() : "";
            data.povrat_na_sredstva1 = "";
            data.povrat_na_kapital1 = "";

            data.kategorija2 = Year.ToString();
            data.sredstva2 = (CV1_Error == false) ? company_values[1].ToString() : "";
            data.kapital2 = (CV1_Error == false) ? company_values[65].ToString() : "";
            //data.vkupno_prihodi2 = (CV1_Error == false) ? (company_values[201] + company_values[223] + company_values[244]).ToString() : "";
            data.vkupno_prihodi2 = "";
            //data.neto_dobivka_za_delovna_godina2 = (CV1_Error == false) ? (company_values[255] - company_values[256]).ToString() : "";
            data.neto_dobivka_za_delovna_godina2 = "";
            data.prosecen_broj_vraboteni2 = (CV1_Error == false) ? company_values[257].ToString() : "";
            //data.koeficient_na_zadolzensot2 = (CV1_Error == false) ? ((company_values[85] + company_values[95]) / company_values[1]).ToString() : "";
            data.koeficient_na_zadolzensot2 = "";
            //data.tekoven_pokazatel2 = (CV1_Error == false) ? (company_values[32] / company_values[95]).ToString() : "";
            data.tekoven_pokazatel2 = "";
            //data.povrat_na_sredstva2 = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[1]).ToString() : "";
            //data.povrat_na_kapital2 = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[65]).ToString() : "";
            data.povrat_na_sredstva2 = "";
            data.povrat_na_kapital2 = "";


            if (COE_Error == false)
            {
                data.organizacioni_edinici = company_organizacioni_edinici;
            }

            data.finansiska_procenka_komentar = "";

            //data.likvidnost_koeficient_za_zadolzenost = (CV1_Error == false) ? ((company_values[85] + company_values[95]) / company_values[1]).ToString() : "";
            data.likvidnost_koeficient_za_zadolzenost = "";
            data.likvidnost_brz_pokazatel = (CV1_Error == false) ? ((company_values[36] - company_values[37]) / company_values[95]).ToString() : "";
            data.likvidnost_prosecni_denovi_na_plakanje_ovrski = (false) ? "" : prosecni_denovi_na_plakanje_obvrski.ToString();
            data.likvidnost_kreditna_izlozenost_od_rabotenje = (CV1_Error == false) ? (company_values[95] - company_values[202]).ToString() : "";
            data.likvidnost_opis = "Задоволително";

            //data.efikasnost_povrat_na_sredstva = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[1]).ToString() : "";
            //data.efikasnost_neto_profitna_marza = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[202]).ToString() : "";
            data.efikasnost_povrat_na_sredstva = "";
            data.efikasnost_neto_profitna_marza = "";
            data.efikasnost_opis = "Задоволително";

            data.profitabilnost_stavka1 = Year.ToString();
            //data.profitabilnost_bruto_operativna_dobivka1 = (CV1_Error == false) ? (company_values[202] - company_values[208] - company_values[209] - company_values[210] - company_values[211]).ToString() : "";
            data.profitabilnost_bruto_operativna_dobivka1 = "";
            data.profitabilnost_neto_dobivka_od_finansiranje1 = (CV1_Error == false) ? (company_values[223] - company_values[234]).ToString() : "";
            //data.profitabilnost_ebitda1 = (CV1_Error == false) ? (company_values[250] - company_values[251] + company_values[239] + company_values[236] + company_values[218] + company_values[219] + company_values[220]).ToString() : "";
            data.profitabilnost_ebitda1 = "";
            //data.profitabilnost_ebit1 = (CV1_Error == false) ? (company_values[250] - company_values[251] + company_values[239] + company_values[236]).ToString() : "";
            data.profitabilnost_ebit1 = "";

            data.profitabilnost_stavka2 = LastYear.ToString();
            data.profitabilnost_bruto_operativna_dobivka2 = (CV2_Error == false) ? (company_values_lastyear[202] - company_values_lastyear[208] - company_values_lastyear[209] - company_values_lastyear[210] - company_values_lastyear[211]).ToString() : "";
            data.profitabilnost_neto_dobivka_od_finansiranje2 = (CV2_Error == false) ? (company_values_lastyear[223] - company_values_lastyear[234]).ToString() : "";
            //data.profitabilnost_ebitda2 = (CV2_Error == false) ? (company_values_lastyear[250] - company_values_lastyear[251] + company_values_lastyear[239] + company_values_lastyear[236] + company_values_lastyear[218] + company_values_lastyear[219] + company_values_lastyear[220]).ToString() : "";
            //data.profitabilnost_ebit2 = (CV2_Error == false) ? (company_values_lastyear[250] - company_values_lastyear[251] + company_values_lastyear[239] + company_values_lastyear[236]).ToString() : "";
            data.profitabilnost_ebitda2 = "";
            data.profitabilnost_ebit2 = "";

            data.istoriski_promeni_datum1 = "";
            data.istoriski_promeni_vid1 = "";
            data.istoriski_promeni_opis1 = "";

            data.istoriski_promeni_datum2 = "";
            data.istoriski_promeni_vid2 = "";
            data.istoriski_promeni_opis2 = "";

            data.istoriski_promeni_datum3 = "";
            data.istoriski_promeni_vid3 = "";
            data.istoriski_promeni_opis3 = "";

            data.solventnost_datum1 = "";
            data.solventnost_opis1 = "";

            data.kazni_datum1 = "";
            data.kazni_opis1 = "";

            data.sankcii_datum1 = "";
            data.sankcii_opis1 = "";

            data.blokadi_datum1 = "";
            data.blokadi_opis1 = "";

            data.bilans_netekovni_sredstva = "A";
            data.bilans_netekovni_2013 = (CV1_Error == false) ? company_values[1].ToString() : "";
            data.bilans_netekovni_2012 = (CV2_Error == false) ? company_values_lastyear[1].ToString() : "";
            data.bilans_netekovni_ind = "";
            data.bilans_netekovni_2013_procent = "";
            data.bilans_netekovni_2012_procent = "";

            data.bilans_nematerijalni_sredstva = "1";
            data.bilans_nematerijalni_2013 = (CV1_Error == false) ? company_values[2].ToString() : "";
            data.bilans_nematerijalni_2012 = (CV2_Error == false) ? company_values_lastyear[2].ToString() : "";
            data.bilans_nematerijalni_ind = "";
            data.bilans_nematerijalni_2013_procent = "";
            data.bilans_nematerijalni_2012_procent = "";

            data.bilans_materijalni_sredstva = "2";
            data.bilans_materijalni_2013 = (CV1_Error == false) ? company_values[9].ToString() : "";
            data.bilans_materijalni_2012 = (CV2_Error == false) ? company_values_lastyear[9].ToString() : "";
            data.bilans_materijalni_ind = "";
            data.bilans_materijalni_2013_procent = "";
            data.bilans_materijalni_2012_procent = "";

            data.bilans_vlozuvanje_nedviznosti_sredstva = "3";
            //data.bilans_vlozuvanje_nedviznosti_2013 = (CV1_Error == false) ? company_values[20].ToString() : "";
            //data.bilans_vlozuvanje_nedviznosti_2012 = (CV2_Error == false) ? company_values_lastyear[20].ToString() : "";
            data.bilans_vlozuvanje_nedviznosti_2013 = "";
            data.bilans_vlozuvanje_nedviznosti_2012 = "";
            data.bilans_vlozuvanje_nedviznosti_ind = "";
            data.bilans_vlozuvanje_nedviznosti_2013_procent = "";
            data.bilans_vlozuvanje_nedviznosti_2012_procent = "";

            data.bilans_dolgorocni_sredstva_sredstva = "4";
            //data.bilans_dolgorocni_sredstva_2013 = (CV1_Error == false) ? company_values[21].ToString() : "";
            //data.bilans_dolgorocni_sredstva_2012 = (CV2_Error == false) ? company_values_lastyear[21].ToString() : ""
            data.bilans_dolgorocni_sredstva_2013 = "";
            data.bilans_dolgorocni_sredstva_2012 = "";
            data.bilans_dolgorocni_sredstva_ind = "";
            data.bilans_dolgorocni_sredstva_2013_procent = "";
            data.bilans_dolgorocni_sredstva_2012_procent = "";

            data.bilans_dolgorocni_pobaruvanja_sredstva = "5";
            //data.bilans_dolgorocni_pobaruvanja_2013 = (CV1_Error == false) ? company_values[31].ToString() : "";
            //data.bilans_dolgorocni_pobaruvanja_2012 = (CV2_Error == false) ? company_values_lastyear[31].ToString() : "";
            data.bilans_dolgorocni_pobaruvanja_2013 = "";
            data.bilans_dolgorocni_pobaruvanja_2012 = "";
            data.bilans_dolgorocni_pobaruvanja_ind = "";
            data.bilans_dolgorocni_pobaruvanja_2013_procent = "";
            data.bilans_dolgorocni_pobaruvanja_2012_procent = "";

            data.bilans_odlozeni_danocni_sredstva_sredstva = "B";
            //data.bilans_odlozeni_danocni_sredstva_2013 = (CV1_Error == false) ? company_values[35].ToString() : "";
            //data.bilans_odlozeni_danocni_sredstva_2012 = (CV2_Error == false) ? company_values_lastyear[35].ToString() : "";
            data.bilans_odlozeni_danocni_sredstva_2013 = "";
            data.bilans_odlozeni_danocni_sredstva_2012 = "";
            data.bilans_odlozeni_danocni_sredstva_ind = "";
            data.bilans_odlozeni_danocni_sredstva_2013_procent = "";
            data.bilans_odlozeni_danocni_sredstva_2012_procent = "";

            data.bilans_tekovni_sredstva_sredstva = "C";
            data.bilans_tekovni_sredstva_2013 = (CV1_Error == false) ? company_values[36].ToString() : "";
            data.bilans_tekovni_sredstva_2012 = (CV2_Error == false) ? company_values_lastyear[36].ToString() : "";
            data.bilans_tekovni_sredstva_ind = "";
            data.bilans_tekovni_sredstva_2013_procent = "";
            data.bilans_tekovni_sredstva_2012_procent = "";

            data.bilans_zalihi_sredstva = "1";
            data.bilans_zalihi_2013 = (CV1_Error == false) ? company_values[37].ToString() : "";
            data.bilans_zalihi_2012 = (CV2_Error == false) ? company_values_lastyear[37].ToString() : "";
            data.bilans_zalihi_ind = "";
            data.bilans_zalihi_2013_procent = "";
            data.bilans_zalihi_2012_procent = "";

            data.bilans_kratkorocni_pobaruvanja_sredstva = "2";
            data.bilans_kratkorocni_pobaruvanja_2013 = (CV1_Error == false) ? company_values[45].ToString() : "";
            data.bilans_kratkorocni_pobaruvanja_2012 = (CV2_Error == false) ? company_values_lastyear[45].ToString() : "";
            data.bilans_kratkorocni_pobaruvanja_ind = "";
            data.bilans_kratkorocni_pobaruvanja_2013_procent = "";
            data.bilans_kratkorocni_pobaruvanja_2012_procent = "";

            data.bilans_kratkorocni_sredstva_sredstva = "3";
            data.bilans_kratkorocni_sredstva_2013 = (CV1_Error == false) ? company_values[52].ToString() : "";
            data.bilans_kratkorocni_sredstva_2012 = (CV2_Error == false) ? company_values_lastyear[52].ToString() : "";
            data.bilans_kratkorocni_sredstva_ind = "";
            data.bilans_kratkorocni_sredstva_2013_procent = "";
            data.bilans_kratkorocni_sredstva_2012_procent = "";

            data.bilans_paricni_sredstva_sredstva = "4";
            //data.bilans_paricni_sredstva_2013 = (CV1_Error == false) ? company_values[44].ToString() : "";
            //data.bilans_paricni_sredstva_2012 = (CV2_Error == false) ? company_values_lastyear[44].ToString() : "";
            data.bilans_paricni_sredstva_2013 = "";
            data.bilans_paricni_sredstva_2012 = "";
            data.bilans_paricni_sredstva_ind = "";
            data.bilans_paricni_sredstva_2013_procent = "";
            data.bilans_paricni_sredstva_2012_procent = "";

            data.bilans_sredstva_grupa_sredstva = "D";
            data.bilans_sredstva_grupa_2013 = (CV1_Error == false) ? company_values[59].ToString() : "";
            data.bilans_sredstva_grupa_2012 = (CV2_Error == false) ? company_values_lastyear[59].ToString() : "";
            data.bilans_sredstva_grupa_ind = "";
            data.bilans_sredstva_grupa_2013_procent = "";
            data.bilans_sredstva_grupa_2012_procent = "";

            data.bilans_plateni_trosoci_sredstva = "E";
            data.bilans_plateni_trosoci_2013 = (CV1_Error == false) ? company_values[62].ToString() : "";
            data.bilans_plateni_trosoci_2012 = (CV2_Error == false) ? company_values_lastyear[62].ToString() : "";
            data.bilans_plateni_trosoci_ind = "";
            data.bilans_plateni_trosoci_2013_procent = "";
            data.bilans_plateni_trosoci_2012_procent = "";

            data.bilans_vkupna_aktiva_sredstva = "";
            data.bilans_vkupna_aktiva_2013 = (CV1_Error == false) ? company_values[63].ToString() : "";
            data.bilans_vkupna_aktiva_2012 = (CV2_Error == false) ? company_values_lastyear[63].ToString() : "";
            data.bilans_vkupna_aktiva_ind = "";
            data.bilans_vkupna_aktiva_2013_procent = "";
            data.bilans_vkupna_aktiva_2012_procent = "";

            data.bilans_glavnina_i_rezervi_sredstva = "A";
            data.bilans_glavnina_i_rezervi_2013 = (CV1_Error == false) ? company_values[65].ToString() : "";
            data.bilans_glavnina_i_rezervi_2012 = (CV2_Error == false) ? company_values_lastyear[65].ToString() : "";
            data.bilans_glavnina_i_rezervi_ind = "";
            data.bilans_glavnina_i_rezervi_2013_procent = "";
            data.bilans_glavnina_i_rezervi_2012_procent = "";

            data.bilans_osnovna_glavnina_sredstva = "1";
            data.bilans_osnovna_glavnina_2013 = (CV1_Error == false) ? company_values[66].ToString() : "";
            data.bilans_osnovna_glavnina_2012 = (CV2_Error == false) ? company_values_lastyear[66].ToString() : "";
            data.bilans_osnovna_glavnina_ind = "";
            data.bilans_osnovna_glavnina_2013_procent = "";
            data.bilans_osnovna_glavnina_2012_procent = "";

            data.bilans_premii_akcii_sredstva = "2";
            //data.bilans_premii_akcii_2013 = (CV1_Error == false) ? company_values[67].ToString() : "";
            //data.bilans_premii_akcii_2012 = (CV2_Error == false) ? company_values_lastyear[67].ToString() : "";
            data.bilans_premii_akcii_2013 = "";
            data.bilans_premii_akcii_2012 = "";
            data.bilans_premii_akcii_ind = "";
            data.bilans_premii_akcii_2013_procent = "";
            data.bilans_premii_akcii_2012_procent = "";

            data.bilans_sopstveni_akcii_sredstva = "3";
            //data.bilans_sopstveni_akcii_2013 = (CV1_Error == false) ? company_values[68].ToString() : "";
            //data.bilans_sopstveni_akcii_2012 = (CV2_Error == false) ? company_values_lastyear[68].ToString() : "";
            data.bilans_sopstveni_akcii_2013 = "";
            data.bilans_sopstveni_akcii_2012 = "";
            data.bilans_sopstveni_akcii_ind = "";
            data.bilans_sopstveni_akcii_2013_procent = "";
            data.bilans_sopstveni_akcii_2012_procent = "";

            data.bilans_zapisan_kapital_sredstva = "4";
            //data.bilans_zapisan_kapital_2013 = (CV1_Error == false) ? company_values[69].ToString() : "";
            //data.bilans_zapisan_kapital_2012 = (CV2_Error == false) ? company_values_lastyear[69].ToString() : "";
            data.bilans_zapisan_kapital_2013 = "";
            data.bilans_zapisan_kapital_2012 = "";
            data.bilans_zapisan_kapital_ind = "";
            data.bilans_zapisan_kapital_2013_procent = "";
            data.bilans_zapisan_kapital_2012_procent = "";

            data.bilans_revalorizaciska_rezerva_sredstva = "5";
            //data.bilans_revalorizaciska_rezerva_2013 = (CV1_Error == false) ? company_values[70].ToString() : "";
            //data.bilans_revalorizaciska_rezerva_2012 = (CV2_Error == false) ? company_values_lastyear[70].ToString() : "";
            data.bilans_revalorizaciska_rezerva_2013 = "";
            data.bilans_revalorizaciska_rezerva_2012 = "";
            data.bilans_revalorizaciska_rezerva_ind = "";
            data.bilans_revalorizaciska_rezerva_2013_procent = "";
            data.bilans_revalorizaciska_rezerva_2012_procent = "";

            data.bilans_rezervi_sredstva = "6";
            data.bilans_rezervi_2013 = (CV1_Error == false) ? company_values[71].ToString() : "";
            data.bilans_rezervi_2012 = (CV2_Error == false) ? company_values_lastyear[71].ToString() : "";
            data.bilans_rezervi_ind = "";
            data.bilans_rezervi_2013_procent = "";
            data.bilans_rezervi_2012_procent = "";

            data.bilans_akumulirana_dobivka_sredstva = "7";
            data.bilans_akumulirana_dobivka_2013 = (CV1_Error == false) ? company_values[75].ToString() : "";
            data.bilans_akumulirana_dobivka_2012 = (CV2_Error == false) ? company_values_lastyear[75].ToString() : "";
            data.bilans_akumulirana_dobivka_ind = "";
            data.bilans_akumulirana_dobivka_2013_procent = "";
            data.bilans_akumulirana_dobivka_2012_procent = "";

            data.bilans_prenesena_zaguba_sredstva = "8";
            //data.bilans_prenesena_zaguba_2013 = (CV1_Error == false) ? company_values[76].ToString() : "";
            //data.bilans_prenesena_zaguba_2012 = (CV2_Error == false) ? company_values_lastyear[76].ToString() : "";
            data.bilans_prenesena_zaguba_2013 = "";
            data.bilans_prenesena_zaguba_2012 = "";
            data.bilans_prenesena_zaguba_ind = "";
            data.bilans_prenesena_zaguba_2013_procent = "";
            data.bilans_prenesena_zaguba_2012_procent = "";

            data.bilans_dobivka_delovna_godina_sredstva = "9";
            data.bilans_dobivka_delovna_godina_2013 = (CV1_Error == false) ? company_values[77].ToString() : "";
            data.bilans_dobivka_delovna_godina_2012 = (CV2_Error == false) ? company_values_lastyear[77].ToString() : "";
            data.bilans_dobivka_delovna_godina_ind = "";
            data.bilans_dobivka_delovna_godina_2013_procent = "";
            data.bilans_dobivka_delovna_godina_2012_procent = "";

            data.bilans_zaguba_delovna_godina_sredstva = "10";
            //data.bilans_zaguba_delovna_godina_2013 = (CV1_Error == false) ? company_values[78].ToString() : "";
            //data.bilans_zaguba_delovna_godina_2012 = (CV2_Error == false) ? company_values_lastyear[78].ToString() : "";
            data.bilans_zaguba_delovna_godina_2013 = "";
            data.bilans_zaguba_delovna_godina_2012 = "";
            data.bilans_zaguba_delovna_godina_ind = "";
            data.bilans_zaguba_delovna_godina_2013_procent = "";
            data.bilans_zaguba_delovna_godina_2012_procent = "";

            data.bilans_obvrski_sredstva = "B";
            data.bilans_obvrski_2013 = (CV1_Error == false) ? company_values[81].ToString() : "";
            data.bilans_obvrski_2012 = (CV2_Error == false) ? company_values_lastyear[81].ToString() : "";
            data.bilans_obvrski_ind = "";
            data.bilans_obvrski_2013_procent = "";
            data.bilans_obvrski_2012_procent = "";

            data.bilans_dolgorocni_rezerviranja_sredstva = "1";
            //data.bilans_dolgorocni_rezerviranja_2013 = (CV1_Error == false) ? company_values[82].ToString() : "";
            //data.bilans_dolgorocni_rezerviranja_2012 = (CV2_Error == false) ? company_values_lastyear[82].ToString() : "";
            data.bilans_dolgorocni_rezerviranja_2013 = "";
            data.bilans_dolgorocni_rezerviranja_2012 = "";
            data.bilans_dolgorocni_rezerviranja_ind = "";
            data.bilans_dolgorocni_rezerviranja_2013_procent = "";
            data.bilans_dolgorocni_rezerviranja_2012_procent = "";

            data.bilans_dolgorocni_obvrski_sredstva = "2";
            //data.bilans_dolgorocni_obvrski_2013 = (CV1_Error == false) ? company_values[85].ToString() : "";
            //data.bilans_dolgorocni_obvrski_2012 = (CV2_Error == false) ? company_values_lastyear[85].ToString() : "";
            data.bilans_dolgorocni_obvrski_2013 = "";
            data.bilans_dolgorocni_obvrski_2012 = "";
            data.bilans_dolgorocni_obvrski_ind = "";
            data.bilans_dolgorocni_obvrski_2013_procent = "";
            data.bilans_dolgorocni_obvrski_2012_procent = "";

            data.bilans_kratkorocni_obvrski_sredstva = "3";
            data.bilans_kratkorocni_obvrski_2013 = (CV1_Error == false) ? company_values[95].ToString() : "";
            data.bilans_kratkorocni_obvrski_2012 = (CV2_Error == false) ? company_values_lastyear[95].ToString() : "";
            data.bilans_kratkorocni_obvrski_ind = "";
            data.bilans_kratkorocni_obvrski_2013_procent = "";
            data.bilans_kratkorocni_obvrski_2012_procent = "";

            data.bilans_odlozeni_obvrski_sredstva = "C";
            //data.bilans_odlozeni_obvrski_2013 = (CV1_Error == false) ? company_values[94].ToString() : "";
            //data.bilans_odlozeni_obvrski_2012 = (CV2_Error == false) ? company_values_lastyear[94].ToString() : "";
            data.bilans_odlozeni_obvrski_2013 = "";
            data.bilans_odlozeni_obvrski_2012 = "";
            data.bilans_odlozeni_obvrski_ind = "";
            data.bilans_odlozeni_obvrski_2013_procent = "";
            data.bilans_odlozeni_obvrski_2012_procent = "";

            data.bilans_odlozeno_plakanje_sredstva = "D";
            //data.bilans_odlozeno_plakanje_2013 = (CV1_Error == false) ? company_values[109].ToString() : "";
            //data.bilans_odlozeno_plakanje_2012 = (CV2_Error == false) ? company_values_lastyear[109].ToString() : "";
            data.bilans_odlozeno_plakanje_2013 = "";
            data.bilans_odlozeno_plakanje_2012 = "";
            data.bilans_odlozeno_plakanje_ind = "";
            data.bilans_odlozeno_plakanje_2013_procent = "";
            data.bilans_odlozeno_plakanje_2012_procent = "";

            data.bilans_obvrski_po_osnov_sredstva = "E";
            //data.bilans_obvrski_po_osnov_2013 = (CV1_Error == false) ? company_values[110].ToString() : "";
            //data.bilans_obvrski_po_osnov_2012 = (CV2_Error == false) ? company_values_lastyear[110].ToString() : "";
            data.bilans_obvrski_po_osnov_2013 = "";
            data.bilans_obvrski_po_osnov_2012 = "";
            data.bilans_obvrski_po_osnov_ind = "";
            data.bilans_obvrski_po_osnov_2013_procent = "";
            data.bilans_obvrski_po_osnov_2012_procent = "";

            data.bilans_vkupna_pasiva_sredstva = "";
            data.bilans_vkupna_pasiva_2013 = (CV1_Error == false) ? company_values[111].ToString() : "";
            data.bilans_vkupna_pasiva_2012 = (CV2_Error == false) ? company_values_lastyear[111].ToString() : "";
            data.bilans_vkupna_pasiva_ind = "";
            data.bilans_vkupna_pasiva_2013_procent = "";
            data.bilans_vkupna_pasiva_2012_procent = "";


            data.uspeh_prihodi_rabotenje_sredstva = "1";
            data.uspeh_prihodi_rabotenje_2013 = (CV1_Error == false) ? company_values[201].ToString() : "";
            data.uspeh_prihodi_rabotenje_2012 = (CV2_Error == false) ? company_values_lastyear[201].ToString() : "";
            data.uspeh_prihodi_rabotenje_ind = "";
            data.uspeh_prihodi_rabotenje_2013_procent = "";
            data.uspeh_prihodi_rabotenje_2012_procent = "";

            data.uspeh_finansiski_prihodi_sredstva = "2";
            data.uspeh_finansiski_prihodi_2013 = (CV1_Error == false) ? company_values[223].ToString() : "";
            data.uspeh_finansiski_prihodi_2012 = (CV2_Error == false) ? company_values_lastyear[223].ToString() : "";
            data.uspeh_finansiski_prihodi_ind = "";
            data.uspeh_finansiski_prihodi_2013_procent = "";
            data.uspeh_finansiski_prihodi_2012_procent = "";

            data.uspeh_vkupno_prihodi_sredstva = "";
            data.uspeh_vkupno_prihodi_2013 = "";
            data.uspeh_vkupno_prihodi_2012 = "";
            data.uspeh_vkupno_prihodi_ind = "";
            data.uspeh_vkupno_prihodi_2013_procent = "";
            data.uspeh_vkupno_prihodi_2012_procent = "";

            data.uspeh_rashodi_rabotenje_sredstva = "1";
            data.uspeh_rashodi_rabotenje_2013 = (CV1_Error == false) ? company_values[207].ToString() : "";
            data.uspeh_rashodi_rabotenje_2012 = (CV2_Error == false) ? company_values_lastyear[207].ToString() : "";
            data.uspeh_rashodi_rabotenje_ind = "";
            data.uspeh_rashodi_rabotenje_2013_procent = "";
            data.uspeh_rashodi_rabotenje_2012_procent = "";

            data.uspeh_rashod_osnovna_dejnost_sredstva = "";
            data.uspeh_rashod_osnovna_dejnost_2013 = "";
            data.uspeh_rashod_osnovna_dejnost_2012 = "";
            data.uspeh_rashod_osnovna_dejnost_ind = "";
            data.uspeh_rashod_osnovna_dejnost_2013_procent = "";
            data.uspeh_rashod_osnovna_dejnost_2012_procent = "";

            data.uspeh_ostanati_trosoci_sredstva = "";
            data.uspeh_ostanati_trosoci_2013 = (CV1_Error == false) ? company_values[212].ToString() : "";
            data.uspeh_ostanati_trosoci_2012 = (CV2_Error == false) ? company_values_lastyear[212].ToString() : "";
            data.uspeh_ostanati_trosoci_ind = "";
            data.uspeh_ostanati_trosoci_2013_procent = "";
            data.uspeh_ostanati_trosoci_2012_procent = "";

            data.uspeh_trosoci_za_vraboteni_sredstva = "";
            data.uspeh_trosoci_za_vraboteni_2013 = (CV1_Error == false) ? company_values[213].ToString() : "";
            data.uspeh_trosoci_za_vraboteni_2012 = (CV2_Error == false) ? company_values_lastyear[213].ToString() : "";
            data.uspeh_trosoci_za_vraboteni_ind = "";
            data.uspeh_trosoci_za_vraboteni_2013_procent = "";
            data.uspeh_trosoci_za_vraboteni_2012_procent = "";

            data.uspeh_amortizacija_sredstva_sredstva = "";
            data.uspeh_amortizacija_sredstva_2013 = (CV1_Error == false) ? company_values[218].ToString() : "";
            data.uspeh_amortizacija_sredstva_2012 = (CV2_Error == false) ? company_values_lastyear[218].ToString() : "";
            data.uspeh_amortizacija_sredstva_ind = "";
            data.uspeh_amortizacija_sredstva_2013_procent = "";
            data.uspeh_amortizacija_sredstva_2012_procent = "";

            data.uspeh_rezerviranje_trosoci_rizici_sredstva = "";
            //data.uspeh_rezerviranje_trosoci_rizici_2013 = (CV1_Error == false) ? company_values[221].ToString() : "";
            //data.uspeh_rezerviranje_trosoci_rizici_2012 = (CV2_Error == false) ? company_values_lastyear[221].ToString() : "";
            data.uspeh_rezerviranje_trosoci_rizici_2013 = "";
            data.uspeh_rezerviranje_trosoci_rizici_2012 = "";
            data.uspeh_rezerviranje_trosoci_rizici_ind = "";
            data.uspeh_rezerviranje_trosoci_rizici_2013_procent = "";
            data.uspeh_rezerviranje_trosoci_rizici_2012_procent = "";

            data.uspeh_zalihi_proizvodi_pocetok_sredstva = "";
            data.uspeh_zalihi_proizvodi_pocetok_2013 = "";
            data.uspeh_zalihi_proizvodi_pocetok_2012 = "";
            data.uspeh_zalihi_proizvodi_pocetok_ind = "";
            data.uspeh_zalihi_proizvodi_pocetok_2013_procent = "";
            data.uspeh_zalihi_proizvodi_pocetok_2012_procent = "";

            data.uspeh_zalihi_proizvodi_kraj_sredstva = "";
            data.uspeh_zalihi_proizvodi_kraj_2013 = "";
            data.uspeh_zalihi_proizvodi_kraj_2012 = "";
            data.uspeh_zalihi_proizvodi_kraj_ind = "";
            data.uspeh_zalihi_proizvodi_kraj_2013_procent = "";
            data.uspeh_zalihi_proizvodi_kraj_2012_procent = "";

            data.uspeh_ostanati_rashodi_sredstva = "";
            data.uspeh_ostanati_rashodi_2013 = (CV1_Error == false) ? company_values[222].ToString() : "";
            data.uspeh_ostanati_rashodi_2012 = (CV2_Error == false) ? company_values_lastyear[222].ToString() : "";
            data.uspeh_ostanati_rashodi_ind = "";
            data.uspeh_ostanati_rashodi_2013_procent = "";
            data.uspeh_ostanati_rashodi_2012_procent = "";

            data.uspeh_finansiski_rashodi_sredstva = "2";
            data.uspeh_finansiski_rashodi_2013 = (CV1_Error == false) ? company_values[234].ToString() : "";
            data.uspeh_finansiski_rashodi_2012 = (CV2_Error == false) ? company_values_lastyear[234].ToString() : "";
            data.uspeh_finansiski_rashodi_ind = "";
            data.uspeh_finansiski_rashodi_2013_procent = "";
            data.uspeh_finansiski_rashodi_2012_procent = "";

            data.uspeh_finansiski_povrzani_drustva_sredstva = "";
            //data.uspeh_finansiski_povrzani_drustva_2013 = (CV1_Error == false) ? company_values[235].ToString() : "";
            //data.uspeh_finansiski_povrzani_drustva_2012 = (CV2_Error == false) ? company_values_lastyear[235].ToString() : "";
            data.uspeh_finansiski_povrzani_drustva_2013 = "";
            data.uspeh_finansiski_povrzani_drustva_2012 = "";
            data.uspeh_finansiski_povrzani_drustva_ind = "";
            data.uspeh_finansiski_povrzani_drustva_2013_procent = "";
            data.uspeh_finansiski_povrzani_drustva_2012_procent = "";

            data.uspeh_rashodi_kamati_sredstva = "";
            //data.uspeh_rashodi_kamati_2013 = (CV1_Error == false) ? company_values[236].ToString() : "";
            data.uspeh_rashodi_kamati_2013 = "";
            //data.uspeh_rashodi_kamati_2012 = (CV2_Error == false) ? company_values_lastyear[236].ToString() : "";
            data.uspeh_rashodi_kamati_2012 = "";
            data.uspeh_rashodi_kamati_ind = "";
            data.uspeh_rashodi_kamati_2013_procent = "";
            data.uspeh_rashodi_kamati_2012_procent = "";

            data.uspeh_rashodi_finansiski_sredstva_sredstva = "";
            data.uspeh_rashodi_finansiski_sredstva_2013 = "";
            data.uspeh_rashodi_finansiski_sredstva_2012 = "";
            data.uspeh_rashodi_finansiski_sredstva_ind = "";
            data.uspeh_rashodi_finansiski_sredstva_2013_procent = "";
            data.uspeh_rashodi_finansiski_sredstva_2012_procent = "";

            data.uspeh_ostanati_finansiski_rashodi_sredstva = "";
            data.uspeh_ostanati_finansiski_rashodi_2013 = (CV1_Error == false) ? company_values[243].ToString() : "";
            data.uspeh_ostanati_finansiski_rashodi_2012 = (CV2_Error == false) ? company_values_lastyear[243].ToString() : "";
            data.uspeh_ostanati_finansiski_rashodi_ind = "";
            data.uspeh_ostanati_finansiski_rashodi_2013_procent = "";
            data.uspeh_ostanati_finansiski_rashodi_2012_procent = "";

            data.uspeh_udel_vo_zaguba_sredstva = "3";
            //data.uspeh_udel_vo_zaguba_2013 = (CV1_Error == false) ? company_values[245].ToString() : "";
            //data.uspeh_udel_vo_zaguba_2012 = (CV2_Error == false) ? company_values_lastyear[245].ToString() : "";
            data.uspeh_udel_vo_zaguba_2013 = "";
            data.uspeh_udel_vo_zaguba_2012 = "";
            data.uspeh_udel_vo_zaguba_ind = "";
            data.uspeh_udel_vo_zaguba_2013_procent = "";
            data.uspeh_udel_vo_zaguba_2012_procent = "";

            data.uspeh_vkupno_rashodi_sredstva = "";
            data.uspeh_vkupno_rashodi_2013 = "";
            data.uspeh_vkupno_rashodi_2012 = "";
            data.uspeh_vkupno_rashodi_ind = "";
            data.uspeh_vkupno_rashodi_2013_procent = "";
            data.uspeh_vkupno_rashodi_2012_procent = "";

            data.uspeh_dobivka_odanocuvanje_sredstva = "1";
            data.uspeh_dobivka_odanocuvanje_2013 = (CV1_Error == false) ? company_values[250].ToString() : "";
            data.uspeh_dobivka_odanocuvanje_2012 = (CV2_Error == false) ? company_values_lastyear[250].ToString() : "";
            data.uspeh_dobivka_odanocuvanje_ind = "";

            data.uspeh_zaguba_odanocuvanje_sredstva = "2";
            //data.uspeh_zaguba_odanocuvanje_2013 = (CV1_Error == false) ? company_values[251].ToString() : "";
            //data.uspeh_zaguba_odanocuvanje_2012 = (CV2_Error == false) ? company_values_lastyear[251].ToString() : "";
            data.uspeh_zaguba_odanocuvanje_2013 = "";
            data.uspeh_zaguba_odanocuvanje_2012 = "";
            data.uspeh_zaguba_odanocuvanje_ind = "";

            data.uspeh_danok_dobivka_sredstva = "3";
            data.uspeh_danok_dobivka_2013 = (CV1_Error == false) ? company_values[252].ToString() : "";
            data.uspeh_danok_dobivka_2012 = (CV2_Error == false) ? company_values_lastyear[252].ToString() : "";
            data.uspeh_danok_dobivka_ind = "";

            data.uspeh_neto_dobivka_sredstva = "4";
            data.uspeh_neto_dobivka_2013 = (CV1_Error == false) ? company_values[255].ToString() : "";
            data.uspeh_neto_dobivka_2012 = (CV2_Error == false) ? company_values_lastyear[255].ToString() : "";
            data.uspeh_neto_dobivka_ind = "";

            data.uspeh_neto_zaguba_sredstva = "5";
            //data.uspeh_neto_zaguba_2013 = (CV1_Error == false) ? company_values[256].ToString() : "";
            //data.uspeh_neto_zaguba_2012 = (CV2_Error == false) ? company_values_lastyear[256].ToString() : "";
            data.uspeh_neto_zaguba_2013 = "";
            data.uspeh_neto_zaguba_2012 = "";
            data.uspeh_neto_zaguba_ind = "";

            //data.indikatori_raboten_kapital_2013 = (CV1_Error == false) ? (company_values[35] - company_values[95]).ToString() : "";
            //data.indikatori_raboten_kapital_2012 = (CV2_Error == false) ? (company_values_lastyear[35] - company_values_lastyear[95]).ToString() : "";
            data.indikatori_raboten_kapital_2013 = "";
            data.indikatori_raboten_kapital_2012 = "";
            data.indikatori_raboten_kapital_rast = "";

            //data.indikatori_tekoven_pokazatel_2013 = (CV1_Error == false) ? (company_values[35] / company_values[95]).ToString() : "";
            //data.indikatori_tekoven_pokazatel_2012 = (CV2_Error == false) ? (company_values_lastyear[35] / company_values_lastyear[95]).ToString() : "";
            data.indikatori_tekoven_pokazatel_2013 = "";
            data.indikatori_tekoven_pokazatel_2012 = "";
            data.indikatori_tekoven_pokazatel_rast = "";

            data.indikatori_brz_pokazatel_2013 = (CV1_Error == false) ? ((company_values[36] - company_values[37]) / company_values[95]).ToString() : "";
            data.indikatori_brz_pokazatel_2012 = (CV2_Error == false) ? ((company_values_lastyear[36] - company_values_lastyear[37]) / company_values_lastyear[95]).ToString() : "";
            data.indikatori_brz_pokazatel_rast = "";

            data.indikatori_obrt_sredstva_2013 = (false) ? "" : V_obrt_na_sredstva.ToString();
            data.indikatori_obrt_sredstva_2012 = "";
            data.indikatori_obrt_sredstva_rast = "";

            data.indikatori_denovi_obrt_sredstva_2013 = (false) ? "" : denovi_na_obrt_na_sredstva.ToString();
            data.indikatori_denovi_obrt_sredstva_2012 = "";
            data.indikatori_denovi_obrt_sredstva_rast = "";

            data.indikatori_obrt_obvrski_2013 = (false) ? "" : G_obrt_na_obvrski.ToString();
            data.indikatori_obrt_obvrski_2012 = "";
            data.indikatori_obrt_obvrski_rast = "";

            data.indikatori_prosecni_denovi_obvrski_2013 = (false) ? "" : prosecni_denovi_na_plakanje_obvrski.ToString();
            data.indikatori_prosecni_denovi_obvrski_2012 = "";
            data.indikatori_prosecni_denovi_obvrski_rast = "";

            data.indikatori_obrt_pobaruvanja_2013 = (false) ? "" : A_prihodi_od_prodazba_prosecni_pobaruvanja.ToString();
            data.indikatori_obrt_pobaruvanja_2012 = "";
            data.indikatori_obrt_pobaruvanja_rast = "";

            data.indikatori_denovi_obrt_pobaruvanja_2013 = (false) ? "" : denovi_na_obrt_na_pobaruvanje.ToString();
            data.indikatori_denovi_obrt_pobaruvanja_2012 = "";
            data.indikatori_denovi_obrt_pobaruvanja_rast = "";

            data.indikatori_obrt_zalihi_2013 = (false) ? "" : B_obrt_na_zalihi.ToString();
            data.indikatori_obrt_zalihi_2012 = "";
            data.indikatori_obrt_zalihi_rast = "";

            data.indikatori_denovi_obrt_zalihi_2013 = (false) ? "" : denovi_na_obrt_na_zalihi.ToString();
            data.indikatori_denovi_obrt_zalihi_2012 = "";
            data.indikatori_denovi_obrt_zalihi_rast = "";

            //data.indikatori_povrat_kapital_2013 = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[65]).ToString() : "";
            //data.indikatori_povrat_kapital_2012 = (CV2_Error == false) ? ((company_values_lastyear[255] - company_values_lastyear[256]) / company_values_lastyear[65]).ToString() : "";
            data.indikatori_povrat_kapital_2013 = "";
            data.indikatori_povrat_kapital_2012 = "";
            data.indikatori_povrat_kapital_rast = "";

            //data.indikatori_povrat_sredstva_2013 = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[1]).ToString() : "";
            //data.indikatori_povrat_sredstva_2012 = (CV2_Error == false) ? ((company_values_lastyear[255] - company_values_lastyear[256]) / company_values_lastyear[1]).ToString() : "";
            data.indikatori_povrat_sredstva_2013 = "";
            data.indikatori_povrat_sredstva_2012 = "";
            data.indikatori_povrat_sredstva_rast = "";

            //data.indikatori_neto_profitna_margina_2013 = (CV1_Error == false) ? ((company_values[255] - company_values[256]) / company_values[202]).ToString() : "";
            //data.indikatori_neto_profitna_margina_2012 = (CV2_Error == false) ? ((company_values_lastyear[255] - company_values_lastyear[256]) / company_values_lastyear[202]).ToString() : "";
            data.indikatori_neto_profitna_margina_2013 = "";
            data.indikatori_neto_profitna_margina_2012 = "";
            data.indikatori_neto_profitna_margina_rast = "";

            //data.indikatori_finansiski_leviridz_2013 = (CV1_Error == false) ? ((company_values[95] + company_values[85]) / company_values[65]).ToString() : "";
            data.indikatori_finansiski_leviridz_2013 = "";
            data.indikatori_finansiski_leviridz_2012 = (CV2_Error == false) ? ((company_values_lastyear[95] + company_values_lastyear[85]) / company_values_lastyear[65]).ToString() : "";
            data.indikatori_finansiski_leviridz_rast = "";

            //data.indikatori_koeficient_zadolzenost_2013 = (CV1_Error == false) ? ((company_values[85] + company_values[95]) / company_values[1]).ToString() : "";
            data.indikatori_koeficient_zadolzenost_2013 = "";
            data.indikatori_koeficient_zadolzenost_2012 = (CV2_Error == false) ? ((company_values_lastyear[85] + company_values_lastyear[95]) / company_values_lastyear[1]).ToString() : "";
            data.indikatori_koeficient_zadolzenost_rast = "";

            data.indikatori_vkupni_obvrski_2013 = (false) ? "" : vkupni_obvrski_ebitda.ToString();
            data.indikatori_vkupni_obvrski_2012 = "";
            data.indikatori_vkupni_obvrski_rast = "";

            data.indikatori_pokrienost_servisiranje_2013 = (false) ? "" : DSCR.ToString();
            data.indikatori_pokrienost_servisiranje_2012 = "";
            data.indikatori_pokrienost_servisiranje_rast = "";

            data.indikatori_pokrienost_kamati_2013 = (false) ? "" : pokrienost_na_kamati.ToString();
            data.indikatori_pokrienost_kamati_2012 = "";
            data.indikatori_pokrienost_kamati_rast = "";

            //data.indikatori_kratkorocni_krediti_2013 = (CV1_Error == false) ? ((company_values[103] + company_values[104]) / company_values[202]).ToString() : "";
            //data.indikatori_kratkorocni_krediti_2012 = (CV2_Error == false) ? ((company_values_lastyear[103] + company_values_lastyear[104]) / company_values_lastyear[202]).ToString() : "";
            data.indikatori_kratkorocni_krediti_2013 = "";
            data.indikatori_kratkorocni_krediti_2012 = "";
            data.indikatori_kratkorocni_krediti_rast = "";

            data.indikatori_tekovni_obvrski_2013 = (CV1_Error == false) ? (company_values[95] / company_values[202]).ToString() : "";
            data.indikatori_tekovni_obvrski_2012 = (CV2_Error == false) ? (company_values_lastyear[95] / company_values_lastyear[202]).ToString() : "";
            data.indikatori_tekovni_obvrski_rast = "";


            data.tekovi_neto_profit_odanocuvanje = "";
            data.tekovi_zaguba_odanocuvanje = "";
            data.tekovi_amortizacija = "";
            data.tekovi_prilivi_gotovina_aktivnosti = "";
            data.tekovi_odlivi_gotovina_aktivnosti = "";
            data.tekovi_neto_prilivi_gotovina_aktivnosti = "";
            data.tekovi_neto_odlivi_gotovina_aktivnosti = "";
            data.tekovi_prilivi_gotovina_investicioni = "";
            data.tekovi_odlivi_gotovina_investicioni = "";
            data.tekovi_neto_prilivi_gotovina_investicioni = "";
            data.tekovi_neto_odlivi_gotovina_investicioni = "";
            data.tekovi_prilivi_gotovina_finansiski = "";
            data.tekovi_odlivi_gotovina_finansiski = "";
            data.tekovi_neto_prilivi_gotovina_finansiski = "";
            data.tekovi_neto_odlivi_gotovina_finansiski = "";
            data.tekovi_vkupno_prilivi_gotovina = "";
            data.tekovi_vkupno_odlivi_gotovina = "";
            data.tekovi_vkupno_neto_prilivi = "";
            data.tekovi_vkupno_neto_odlivi = "";
            data.tekovi_paricni_sredstva_pocetok = "";
            data.tekovi_paricni_sredstva_kraj = "";

            return data;

        }

        private static string OvlasteniLicaShortHTML(Attributes data)
        {
            var ovlasteni_lica = data.ovlasteni_lica;

            var HTML = "";

            if (ovlasteni_lica != null)
            {
                foreach (var item in ovlasteni_lica)
                {
                    HTML += "<p class=\"border_bottom\"><span>" + item.Ime + "</span><p>" + item.Povrzanost + "</p></p>";
                }
            }
            return HTML;
        }

        private static string OvlasteniLicaLongHTML(Attributes data)
        {
            var ovlasteni_lica = data.ovlasteni_lica;

            var total = 0;
            if(ovlasteni_lica != null)
                total = ovlasteni_lica.Count();
            var HTML = "<div class=\"p_wrapper_header\">" +
                            "<h2>Овластени лица</h2>" +
                            "<p class=\"black\">Прикажани " + total + " од " + total + "</p>" +
                        "</div>" +
                        "<div class=\"content\">";

            if (ovlasteni_lica != null)
            {
                foreach (var item in ovlasteni_lica)
                {
                    HTML += "<p><span>" + item.Ime + "</span></p>" +
                            "<p>Овластувања: " + item.Ovlastuvanja + "</p>" +
                            "<p>Тип овластување:</p>" +
                            "<p>- " + item.TipOvlastuvanja1 + "</p>" +
                            "<p>- " + item.TipOvlastuvanja2 + "</p>";
                }
            }
            HTML += "</div>";

            return HTML;
        }

        private static string SopstveniciHTML(Attributes data)
        {
            var ovlasteni_lica = data.ovlasteni_lica;

            var sopstvenici = new List<OvlastenoLiceObject>();
            if (ovlasteni_lica != null)
            {
                sopstvenici = ovlasteni_lica.Where(c => c.IsOwner == (FL_TipPovrzanost)1).ToList();
            }
            var HTML = "<div class=\"p_wrapper_header\">" +
                            "<h2>Сопственици</h2>" +
                            "<p class=\"black\">Прикажани " + sopstvenici.Count() + " од " + sopstvenici.Count() + "</p>" +
                        "</div>" +
                        "<div class=\"content\">" +
                            "<p><span>" + data.celosen_naziv_firma + "</span></p>";
            
            foreach (var item in sopstvenici)
            {
                HTML += "<p><span>" + item.Ime + "</span></p>";
            }

            HTML += "</div>";

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
                            "<h2>Подружници / филијали</h2>" +
                            "<p class=\"black\">Прикажани " + total + " од " + total + "</p>" +
                        "</div>" +
                        "<div class=\"content\">";
            if (podruznici_filijali != null)
            {
                foreach (var item in podruznici_filijali)
                {
                    HTML += "<p><span>Подброј:</span>" + item.Podbroj + "</p>" +
                            "<p><span>Назив:</span>" + item.Naziv + "</p>" +
                            "<p><span>Тип:</span>" + item.Tip + "</p>" +
                            "<p><span>Подтип:</span>" + item.Podtip + "</p>" +
                            "<p><span>Приоритетна дејност/Главна приходна шифра:</span>" + item.GlavnaPrihodnaSifra + "-" + item.PrioritetnaDejnost + "</p>" +
                            "<p><span>Адреса:</span>" + item.Adresa + "</p>" +
                            "<p><span>Овластено лице:</span>NOT SURE</p>";
                }
            }
            HTML += "</div>";

            return HTML;
        }

        private static string Footer(int page)
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
                                    "<p>" + page + "</p>" +
                                "</div>" +
                            "</div>";

            return html;
        }

        private static string CoverPage(Attributes data)
        {
            string html = "<!DOCTYPE html>" +
                        "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
                        "<head>" +
                            "<title></title>" +
                            "<style>body{height:auto;width:1040px;padding:0;margin:0 auto;font-family:Calibri}.page{height:1470px;width:1040px;padding:0;margin:0;float:left}#first_page_wrapper{width:100%;height:770px;float:left;background-color:#B9D431}#eigth_page_wrapger,#eleventh_page_wrapper,#fifth_page_wrapper,#fourth_page_wrapper,#ninth_page_wrapper,#second_page_wrapper,#seventh_page_wrapper,#sixth_page_wrapper,#tenth_page_wrapper,#third_page_wrapper{width:100%;height:1200px;float:left}h1{font-size:60px;margin-left:50px;margin-top:250px;padding:0;margin-bottom:0}h2{font-size:36px;display:inline-block;margin-left:50px;color:#444}#cover_wrapper{text-align:left;width:100%;float:left}p.inline_block{display:inline-block;font-size:52px;margin-left:150px;margin-top:0;margin-bottom:0;color:#444}p{font-size:22px;margin-left:50px}p span{font-weight:700}.spacer_100{width:100%;height:100px;float:left}.spacer_200{width:100%;height:200px;float:left}.footer{width:100%;height:125px;background-color:#B9D431;opacity:.7;float:left}.footer .img_wrapper img{padding-top:0}.footer .img_wrapper{float:left;background-color:#fff!important;height:125px;margin-top:0}.footer.cover_footer{height:200px}.footer div{float:left;margin-top:25px}.footer p{padding:0;margin:0 0 0 50px}.footer .pagination{margin:0;padding:0;width:120px;height:125px;text-align:center;float:left}.footer .pagination p{line-height:125px;color:#fff;font-size:35px;margin:0;padding:0}#white_cover{width:100%;height:100px;float:left}#white_cover p{font-weight:700}.footer.cover_footer div{float:left;margin-top:50px}.footer.cover_footer .img_wrapper{float:left;background-color:#fff!important;width:256px;height:200px;margin-left:220px;margin-top:0}.footer.cover_footer p{padding:0;margin:20px 0 0 50px}.footer.cover_footer .img_wrapper img{padding-top:67px}.header{background-color:#F6F6F8;width:100%;height:135px;opacity:.7;border-bottom:5px #000 solid;float:left;position:relative}.float_left{float:left}.float_left h2{font-size:50px;padding:0;margin:10px 0 0 50px;color:#000}.float_left h3{padding:0;margin:0 0 0 50px;color:#000;font-size:20px}.float_left p{margin:0 0 0 50px;font-size:16px}.float_right{float:right}.float_right img{margin-top:10px;margin-right:50px}.left_p{float:left;width:520px}.right_p{float:right;width:520px}.p_wrapper{width:490px;float:left;margin-top:20px}.p_wrapper.left{margin-right:0;margin-left:30px;padding-right:0}.p_wrapper.right{margin-right:30px;margin-left:0;padding-right:0}.p_wrapper.full{width:960px;display:block;margin-left:30px}.p_wrapper.full .p_wrapper_header{width:100%}.p_wrapper .p_wrapper_header{width:470px;float:left;padding-left:20px}.p_wrapper_header h2{margin:0;padding:0;float:left;text-transform:uppercase;color:#000;font-weight:700;font-size:30px}.p_wrapper_header p{margin:0 10px 0 0;padding:0;float:right}.p_wrapper_header p.black{background-color:#000;color:#fff;line-height:30px}.p_wrapper .content{float:left;padding:20px;width:450px;background-color:#fff}.p_wrapper.full .content{width:940px}.p_wrapper .content.gray{background-color:#F2F2F2}.p_wrapper .content p{margin:0;padding:0}.left_p.shorter,.shorter .p_wrapper.left,.shorter .p_wrapper.left .p_wrapper_header{width:340px}.shorter .p_wrapper.left .content{width:300px}.shorter .p_wrapper.left .content p{margin-bottom:40px}.right_p.longer{width:600px}.longer .p_wrapper.right{width:570px}.longer .p_wrapper.right .p_wrapper_header{width:560px}.longer .p_wrapper.right .content{width:520px}.spacer_50_600{width:600px;height:50px;float:left}.semaphore{width:72px;height:67px;float:left;display:inline-block}.semaphore_text{width:370px;float:left;margin-left:8px}.full .semaphore_text{width:600px;float:left;margin-left:200px}.rezultati_table{width:960px;margin:0;padding:0;border-spacing:0}.rezultati_table tr td{border-bottom:1px solid #000;line-height:32px}.rezultati_table .gray td{background-color:#D9D9D9;border-bottom:0}.rezultati_table tr td:nth-child(2),.rezultati_table tr td:nth-child(3){text-align:right;width:30%}.rezultati_table .information td{border-bottom:0}.rezultati_table .information td p{font-size:15px}.p_wrapper .content .gray{background-color:#F2F2F2;margin-left:-20px;padding-left:20px;margin-right:-20px}.p_wrapper .content .border_bottom{border-bottom:1px solid #000;margin-top:10px}.header .absolute_poglavje{position:absolute;bottom:-20px;right:50px;background-color:#7F7F7F}.header .absolute_poglavje p{margin:0;line-height:40px;font-size:20px;color:#fff;padding:0 20px}.finansii_table{width:550px;margin:0;padding:0;border-spacing:0}.longer .p_wrapper_header h2{padding-left:20px}.finansii_table tr td{border-bottom:1px solid #000;line-height:32px}.finansii_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:40px}.finansii_table .gray td:nth-child(1){text-align:left!important;padding-left:20px}.finansii_table .gray td:nth-child(2),.finansii_table .gray td:nth-child(3){text-align:center!important}.finansii_table tr td:nth-child(1){text-align:right}.finansii_table tr td:nth-child(2),.finansii_table tr td:nth-child(3){text-align:center;width:30%}.finansii_table .information td{border-bottom:0}.finansii_table .information td p{font-size:15px}.promeni_table{width:100%;margin:0;padding:0;border-spacing:0}.promeni_table tr td{border-bottom:1px solid #000;line-height:32px}.promeni_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:60px}.promeni_table tr td:nth-child(1){text-align:center}.promeni_table tr td{text-align:left}.promeni_table .information td{border-bottom:0}.promeni_table .information td p{font-size:15px}.promeni_table tr td:nth-child(1){width:5%}.promeni_table tr td:nth-child(2),.promeni_table tr td:nth-child(3){width:20%}.kazni_table{width:100%;margin:0;padding:0;border-spacing:0}.kazni_table tr td{border-bottom:1px solid #000;line-height:32px}.kazni_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:60px}.kazni_table tr td:nth-child(1){text-align:center}.kazni_table tr td{text-align:left}.kazni_table .information td{border-bottom:0}.kazni_table .information td p{font-size:15px}.kazni_table tr td:nth-child(1){width:5%}.kazni_table tr td:nth-child(2),.kazni_table tr td:nth-child(3){width:20%}.bilans_table{width:100%;margin:0;padding:0;border-spacing:0}.bilans_table tr td{line-height:20px;padding-left:20px}.bilans_table tr.has_border td{border-bottom:1px solid #000}.bilans_table tr td p{font-size:16px}.bilans_table .gray td{background-color:#D9D9D9;border-bottom:0}.bilans_table .gray.text_right td{text-align:right;padding-right:20px}.bilans_table .information td{border-bottom:0}.bilans_table .information td p{font-size:15px}.bilans_table .margin_top td{height:40px;vertical-align:bottom}.bilans_table.margin_top{margin-top:40px;width:850px} .uppercase_must{ text-transform: uppercase !important;} .no_transform{ text-transform: none !important;} </style>" +
                        "</head>" +
                        "<body>" +
                            "<div class=\"page\">" +
                                "<div class=\"spacer_200\"></div>" +
                                "<div id=\"first_page_wrapper\">" +
                                    "<h1>Бонитетен извештај</h1>" +
                                    "<div id=\"cover_wrapper\">" +
                                        "<h2>" + data.ime_firma + "</h2>" +
                                        "<p class=\"inline_block\">[" + data.drzava + "]</p>" +
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
                                   "<div class=\"img_wrapper\">" +
                                        "<img src=\"" + AbsoluteUrlPath + "\\img\\target_group.png\" />" +
                                    "</div>" +
                                "</div>" +
                            "</div>";
            return html;
        }

        private static string FirstPage(Attributes data)
        {

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
                                             "<p>Вкупна основна главнина:<span>" + data.vkupna_osnovna_glavnina + "</span></p>" +
                                             "<p>Уплатен дел:<span>" + data.uplaten_del + "</span></p>" +
                                             "<p>Непаричен влог:<span>" + data.neparicen_vlog + "</span></p>" +
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
                                             "<p>" + data.sostojba_komentar + "</p>" +
                                         "</div>" +
                                     "</div>" +
                                     "<div class=\"p_wrapper right\">" +
                                         "<div class=\"p_wrapper_header\">" +
                                             "<h2>Семафор</h2>" +
                                             "<p class=\"black\"></p>" +
                                         "</div>" +
                                         "<div class=\"content\">" +
                                             "<div class=\"semaphore\">" +
                                             (data.semafor_solventnost == "green" ? "<img src=\"" + AbsoluteUrlPath + "\\img\\green_light.jpg\" class=\"float_left\" />" : "") +
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
                                             "<p class=\"float_left\">Казни:<span>" + data.kazni_minata_godina + "</span></p>" +
                                             "<p class=\"float_right\">Санкции:<span>" + data.sankcii_minata_godina + "</span></p>" +
                                         "</div>" +
                                     "</div>" +
                                 "</div>" +
                                 "<div class=\"p_wrapper full\" style=\"margin-top:0;\">" +
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
                                 "</div>" +
                             "</div>" +
                             Footer(1) +
                         "</div>";

            return html;
        }

        private static string SecondPage(Attributes data)
        {
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
                                            "<p>Основана:<span>" + data.datum_osnovanje + "</span></p>" +
                                            "<p>ЕМБС:<span>" + data.embs + "</span></p>" +
                                            "<p>ЕДБ:<span>" + data.edb + "</span></p>" +
                                            "<p>Големина на субјектот:<span>" + data.golemina_na_subjekt + "</span></p>" +
                                            "<p>Просечен број на вработени:<span>" + data.prosecen_broj_vraboteni + "</span></p>" +
                                            "<p>Регистрирана во:<span>" + data.registrirana_vo + "</span></p>" +
                                            "<p>Регион:<span>" + data.region + "</span></p>" +
                                            "<p>Капитал:<span>" + data.kapital + "</span></p>" +
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
                                            "<p><span>" + data.celosen_naziv_firma + "</span></p>" +
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
                                            "<p>" + data.banka_smetka1 + "</p>" +
                                            "<p>" + data.banka_ime1 + "</p>" +
                                            "<p>" + data.banka_smetka2 + "</p>" +
                                            "<p>" + data.banka_ime2 + "</p>" +
                                            "<p>" + data.banka_smetka3 + "</p>" +
                                            "<p>" + data.banka_ime3 + "</p>" +
                                            "<p>" + data.banka_smetka4 + "</p>" +
                                            "<p>" + data.banka_ime4 + "</p>" +
                                            "<p>" + data.banka_smetka5 + "</p>" +
                                            "<p>" + data.banka_ime5 + "</p>" +
                                            "<p>" + data.banka_smetka6 + "</p>" +
                                            "<p>" + data.banka_ime6 + "</p>" +
                                            "<p>" + data.banka_smetka7 + "</p>" +
                                            "<p>" + data.banka_ime7 + "</p>" +
                                            "<p>" + data.banka_smetka8 + "</p>" +
                                            "<p>" + data.banka_ime8 + "</p>" +
                                            "<p>" + data.banka_smetka9 + "</p>" +
                                            "<p>" + data.banka_ime9 + "</p>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer(2) +
                        "</div>";

            return html;
        }

        private static string ThirdPage(Attributes data)
        {
            var html = "<div class=\"page\">" +
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
                            "</div>" + Footer(3) +
                        "</div>";

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
                                            "<img class=\"float_left\" src=\"" + AbsoluteUrlPath + "\\img\\green_light.jpg\" />" +
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
                                "<div class=\"spacer_50_600\"></div>" +
                                "<div class=\"right_p longer\">" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Ликвидност</h2>" +
                                            "<p class=\"black\">" + data.likvidnost_opis + "</p>" +
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
                                                        "<p>" + data.likvidnost_opis + "</p>" +
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
                                                        "<p>" + data.likvidnost_opis + "</p>" +
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
                                                        "<p>" + data.likvidnost_opis + "</p>" +
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
                                                        "<p>" + data.likvidnost_opis + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Ефикасност</h2>" +
                                            "<p class=\"black\">" + data.efikasnost_opis + "</p>" +
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
                                                        "<p>" + data.efikasnost_opis + "</p>" +
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
                                                        "<p>" + data.efikasnost_opis + "</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>" +
                                    "<div class=\"p_wrapper right\">" +
                                        "<div class=\"p_wrapper_header\">" +
                                            "<h2>Профитабилност</h2>" +
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
                            "</div>" + Footer(4) +
                        "</div>";

            return html;
        }

        private static string FifthPage(Attributes data)
        {
            var html = "<div class=\"page\">" +
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
                                                "<td>" +
                                                    "<p><span>Опис</span></p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>1</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_datum1 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_vid1 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_opis1 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>2</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_datum2 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_vid2 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_opis2 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>3</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_datum3 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_vid3 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.istoriski_promeni_opis3 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</div>" +
                                "</div>" +
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
                                                "<td>" +
                                                    "<p><span>Опис</span></p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>1</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.solventnost_datum1 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.solventnost_opis1 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer(5) +
                        "</div>";

            return html;
        }

        private static string SixthPage(Attributes data)
        {
            var html = "<div class=\"page\">" +
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
                                        "<h2>Казни</h2>" +
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
                                                    "<p>" + data.kazni_datum1 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.kazni_opis1 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</div>" +
                                "</div>" +
                                "<div class=\"p_wrapper full\">" +
                                    "<div class=\"p_wrapper_header\">" +
                                        "<h2>Санкции</h2>" +
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
                                                    "<p>" + data.sankcii_datum1 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.sankcii_opis1 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
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
                                                    "<p>Нема</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.blokadi_opis1 + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" + Footer(6) +
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
                                                    "<p><span>2013</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>2012</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>Инд.</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>2013</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>2012</span></p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Нетековни средства</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_netekovni_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_netekovni_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_netekovni_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_netekovni_ind + "</p>" +
                                                "</td>" +
                                               "<td>" +
                                                    "<p>" + data.bilans_netekovni_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_netekovni_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Нематеријални средства</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_nematerijalni_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_nematerijalni_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_nematerijalni_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_nematerijalni_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_nematerijalni_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_nematerijalni_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Материјални средства</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_materijalni_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_materijalni_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_materijalni_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_materijalni_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_materijalni_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_materijalni_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Вложувања во недвижности</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vlozuvanje_nedviznosti_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни финансиски средства</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни побарувања</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_pobaruvanja_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Одложени даночни средства</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_odlozeni_danocni_sredstva_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_danocni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Тековни средства</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_tekovni_sredstva_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_tekovni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_tekovni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                           "<tr>" +
                                                "<td>" +
                                                    "<p>Залихи</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zalihi_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zalihi_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zalihi_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zalihi_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zalihi_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zalihi_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Краткорочни побарувања</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_pobaruvanja_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Краткорочни финансиски средства</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Парични средства и парични еквиваленти</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_paricni_sredstva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_paricni_sredstva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_paricni_sredstva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_paricni_sredstva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_paricni_sredstva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_paricni_sredstva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Средства (или групи а отуѓување наменети за ...</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_sredstva_grupa_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sredstva_grupa_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sredstva_grupa_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sredstva_grupa_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sredstva_grupa_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sredstva_grupa_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Платени трошоци за идните периоди и пресметан ...</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_plateni_trosoci_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_plateni_trosoci_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_plateni_trosoci_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_plateni_trosoci_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_plateni_trosoci_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
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
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_aktiva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_aktiva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Главнина и резерви</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_glavnina_i_rezervi_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_glavnina_i_rezervi_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Основна главнина </p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_osnovna_glavnina_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_osnovna_glavnina_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_osnovna_glavnina_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Премии на емитирани акции</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_premii_akcii_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_premii_akcii_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_premii_akcii_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_premii_akcii_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_premii_akcii_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_premii_akcii_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Сопствени акции</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sopstveni_akcii_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sopstveni_akcii_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_sopstveni_akcii_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Запишан, неуплатен капитал</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zapisan_kapital_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zapisan_kapital_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zapisan_kapital_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zapisan_kapital_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zapisan_kapital_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zapisan_kapital_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Ревалоризациска резерва и разлики од вреднување на ...</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_revalorizaciska_rezerva_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Резерви</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_rezervi_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_rezervi_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_rezervi_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_rezervi_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_rezervi_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_rezervi_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Акумулирана добивка</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_akumulirana_dobivka_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Пренесена загуба</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_prenesena_zaguba_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_prenesena_zaguba_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_prenesena_zaguba_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Добивка за деловната година</p>" +
                                                "</td>" +
                                                "<td>" +
                                                   "<p>" + data.bilans_dobivka_delovna_godina_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dobivka_delovna_godina_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Загуба за деловната година</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_zaguba_delovna_godina_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Обврски</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_obvrski_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни резервирања на ризици и трошоци</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_rezerviranja_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Долгорочни обврски </p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_dolgorocni_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr>" +
                                                "<td>" +
                                                    "<p>Краткорочни обврски</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_sredstva + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_kratkorocni_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Одложени даночни обврски</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_odlozeni_obvrski_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeni_obvrski_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Одложено плаќање и приходи во идните периоди</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_odlozeno_plakanje_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_odlozeno_plakanje_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Обврски по основ на нетековни средства (или груп...</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p><span>" + data.bilans_obvrski_po_osnov_sredstva + "</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_obvrski_po_osnov_2012_procent + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                            "<tr class=\"has_border\">" +
                                                "<td>" +
                                                    "<p><span>Вкупна пасива</span></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p></p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2013 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2012 + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_pasiva_ind + "</p>" +
                                                "</td>" +
                                                "<td>" +
                                                    "<p>" + data.bilans_vkupna_pasiva_2013_procent + "</p>" +
                                                "</td>" +
                                                "<td>" +
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
                            "</div>" + Footer(7) +
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
                                    "<p class=\"date\">" + data.datum_izdavanje + "</p> " +
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
                                                    "<p><span>2013</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>2012</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>Инд.</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p><span>2013</span></p> " +
                                               " </td> " +
                                                "<td> " +
                                                    "<p><span>2012</span></p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Приходи од работењето</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_prihodi_rabotenje_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                   " <p>" + data.uspeh_prihodi_rabotenje_2012_procent + "</p> " +
                                               " </td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Финансиски приходи</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_finansiski_prihodi_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2012 + "</p> " +
                                               " </td> " +
                                                "<td> " +
                                                   " <p>" + data.uspeh_finansiski_prihodi_ind + "</p> " +
                                               " </td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_finansiski_prihodi_2012_procent + "</p> " +
                                                "</td> " +
                                           " </tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                    "<p><span>Вкупно приходи</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_vkupno_prihodi_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_vkupno_prihodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border margin_top\"> " +
                                                "<td> " +
                                                    "<p><span>Расходи од работењето</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashodi_rabotenje_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                    "<p>Расходи од основна дејност</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_rashod_osnovna_dejnost_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                    "<p>Останати трошоци од работењето</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p></p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_ostanati_trosoci_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_ostanati_trosoci_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                    "<p>" + data.uspeh_trosoci_za_vraboteni_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                    "<p>" + data.uspeh_trosoci_za_vraboteni_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_trosoci_za_vraboteni_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_trosoci_za_vraboteni_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_amortizacija_sredstva_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rezerviranje_trosoci_rizici_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_pocetok_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zalihi_proizvodi_kraj_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_rashodi_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_rashodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Финансиски расходи</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_rashodi_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_rashodi_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_finansiski_povrzani_drustva_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr> " +
                                                "<td> " +
                                                "    <p>Расходи по основ на камати и курсни разлики од работе...</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_kamati_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_kamati_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_kamati_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_kamati_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_rashodi_finansiski_sredstva_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_ostanati_finansiski_rashodi_2012_procent + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Удел во загуба на придружените друштва</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_udel_vo_zaguba_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_vkupno_rashodi_ind + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_vkupno_rashodi_2013_procent + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                                                "<td> " +
                                                "    <p><span>2013</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p><span>2012</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p><span>Инд.</span></p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Добивка пред оданочување</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_dobivka_odanocuvanje_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Загуба пред оданочување</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_zaguba_odanocuvanje_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Данок на добивка</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_danok_dobivka_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_danok_dobivka_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_danok_dobivka_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_danok_dobivka_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Нето добивка во деловната година</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_dobivka_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_dobivka_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_dobivka_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_dobivka_ind + "</p> " +
                                                "</td> " +
                                            "</tr> " +
                                            "<tr class=\"has_border\"> " +
                                                "<td> " +
                                                "    <p><span>Нето загуба во деловната година</span></p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_zaguba_sredstva + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_zaguba_2013 + "</p> " +
                                                "</td> " +
                                                "<td> " +
                                                "    <p>" + data.uspeh_neto_zaguba_2012 + "</p> " +
                                                "</td> " +
                                                "<td> " +
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
                            "</div> " + Footer(8) +
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
                                    "<p class=\"date\">" + data.datum_izdavanje + "</p> " +
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
                                            "        <p><span>2013</span></p> " +
                                            "    </td> " +
                                            "    <td> " +
                                            "        <p><span>2012</span></p> " +
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
                                            "        <p>Денови на обрт на средства</p> " +
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
                            "</div> " + Footer(9) +
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
                                    "<p class=\"date\">" + data.datum_izdavanje + "</p> " +
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
                                        "            <p><span>2013</span></p> " +
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
                            "</div> " + Footer(10) +
                       " </div>";

            return html;
        }




        private static string GetChart1()
        {
            return string.Empty;
        }

    }


    public class Attributes
    {

        public string ime_firma { get; set; }
        public string drzava { get; set; }
        public string datum_izdavanje { get; set; }
        public string izdaden_za { get; set; }
        public string uplaten_del { get; set; }
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
        public string sankcii_minata_godina { get; set; }

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
        public string registrirana_vo { get; set; }
        public string prosecen_broj_vraboteni { get; set; }
        public string region { get; set; }
        public string kapital { get; set; }
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

        public List<OvlastenoLiceObject> ovlasteni_lica { get; set; }

        public List<OrganizacionaEdinicaObject> organizacioni_edinici { get; set; }

        public string banka_smetka1 { get; set; }
        public string banka_ime1 { get; set; }
        public string banka_smetka2 { get; set; }
        public string banka_ime2 { get; set; }
        public string banka_smetka3 { get; set; }
        public string banka_ime3 { get; set; }
        public string banka_smetka4 { get; set; }
        public string banka_ime4 { get; set; }
        public string banka_smetka5 { get; set; }
        public string banka_ime5 { get; set; }
        public string banka_smetka6 { get; set; }
        public string banka_ime6 { get; set; }
        public string banka_smetka7 { get; set; }
        public string banka_ime7 { get; set; }
        public string banka_smetka8 { get; set; }
        public string banka_ime8 { get; set; }
        public string banka_smetka9 { get; set; }
        public string banka_ime9 { get; set; }

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

        public string likvidnost_koeficient_za_zadolzenost { get; set; }
        public string likvidnost_brz_pokazatel { get; set; }
        public string likvidnost_prosecni_denovi_na_plakanje_ovrski { get; set; }
        public string likvidnost_kreditna_izlozenost_od_rabotenje { get; set; }
        public string likvidnost_opis { get; set; }

        public string efikasnost_povrat_na_sredstva { get; set; }
        public string efikasnost_neto_profitna_marza { get; set; }
        public string efikasnost_opis { get; set; }

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

        public string istoriski_promeni_datum1 { get; set; }
        public string istoriski_promeni_vid1 { get; set; }
        public string istoriski_promeni_opis1 { get; set; }

        public string istoriski_promeni_datum2 { get; set; }
        public string istoriski_promeni_vid2 { get; set; }
        public string istoriski_promeni_opis2 { get; set; }

        public string istoriski_promeni_datum3 { get; set; }
        public string istoriski_promeni_vid3 { get; set; }
        public string istoriski_promeni_opis3 { get; set; }

        public string solventnost_datum1 { get; set; }
        public string solventnost_opis1 { get; set; }

        public string kazni_datum1 { get; set; }
        public string kazni_opis1 { get; set; }

        public string sankcii_datum1 { get; set; }
        public string sankcii_opis1 { get; set; }

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
    }

}

