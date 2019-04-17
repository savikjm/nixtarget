using Bonitet.Charts;
using Bonitet.CRM;
using Bonitet.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bonitet.Document
{
    public partial class DocumentClass
    {

        private static void SetDecimalSymbol(string sign)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = sign;

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }
        // report 1
        public static Attributes SetReport1Data(int UserID, string EMBS, int Year, string clientEMBS, bool force_crm)
        {
            SetDecimalSymbol(".");

            var COG_Error = false;
            var CD_Error = false;
            var CRV_Error = false;
            var CP_Error = false;
            var COE_Error = false;

            var company = DALHelper.GetCompanyByEMBS_BM(EMBS);
            var t_company = DALHelper.GetCompanyByEMBS(EMBS);

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
                company_dejnost = dejnost;
            }
            else
            {
                CD_Error = true;
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


            var kazni_i_sankcii = DALHelper.GetKazniISankciiForCompany(CompanyID, force_crm);
            var promeni = DALHelper.GetPromeniForCompany(Int32.Parse(EMBS), force_crm);
            var solventnost = DALHelper.GetSolventnostForCompany(CompanyID, force_crm);

            var res1 = DALHelper.GetCompanyValuesForCurrentYear(EMBS, Year);

            var company_values = new Dictionary<int, double>();

            if (res1 != null && res1.Count > 0)
                company_values = res1.ToDictionary(c => c.ValueID, c => c.Value);

            var LastYear = Year - 1;

            var res2 = DALHelper.GetCompanyValuesForLastYear(EMBS, LastYear);

            var company_values_lastyear = new Dictionary<int, double>();

            if (res2 != null && res2.Count > 0)
                company_values_lastyear = res2.ToDictionary(c => c.ValueID, c => c.Value);

            //FixCompanyValues(company_values, company_values_lastyear);


            var data = new Attributes();
            if ((res1 == null || res1.Count == 0) && (res2 == null || res2.Count == 0))
            {
                data.GodisnaSmetka = false;
            }
            else
            {
                data.GodisnaSmetka = true;
            }
            data.IndexHelper = 0;
            data.CurYear = Year.ToString();
            data.LastYear = LastYear.ToString();
            var A_prihodi_od_prodazba_prosecni_pobaruvanja = "";
            var A_prihodi_od_prodazba_prosecni_pobaruvanja_1 = "";
            var denovi_na_obrt_na_pobaruvanje = "";
            var denovi_na_obrt_na_pobaruvanje_1 = "";
            var B_obrt_na_zalihi = "";
            var B_obrt_na_zalihi_1 = "";
            var denovi_na_obrt_na_zalihi = "";
            var denovi_na_obrt_na_zalihi_1 = "";
            var V_obrt_na_sredstva = "";
            var V_obrt_na_sredstva_1 = "";
            var denovi_na_obrt_na_sredstva = "";
            var denovi_na_obrt_na_sredstva_1 = "";
            var G_obrt_na_obvrski = "";
            var G_obrt_na_obvrski_1 = "";
            var prosecni_denovi_na_plakanje_obvrski = "";
            var prosecni_denovi_na_plakanje_obvrski_1 = "";
            var D_bruto_operativna_dobivka = "";
            var D_bruto_operativna_dobivka_1 = "";
            var Gj_EBITDA = "";
            var Gj_EBITDA_1 = "";
            var E_EBIT = "";
            var E_EBIT_1 = "";
            var vkupni_obvrski_ebitda = "";
            var vkupni_obvrski_ebitda_1 = "";
            var DSCR = "";
            var DSCR_1 = "";
            var pokrienost_na_kamati = "";
            var pokrienost_na_kamati_1 = "";


            data.uspeh_vkupno_prihodi_2013 = "";
            data.uspeh_vkupno_prihodi_2012 = "";
            data.uspeh_vkupno_rashodi_2013 = "";
            data.uspeh_vkupno_rashodi_2012 = "";

            if(company.ZiroSmetka != null)
            {
                var vodecki_broj = company.ZiroSmetka.Substring(0, 3);

                var bankName = GetBankFromZS(vodecki_broj);

                var tmpBank = new BankarskaSmetka(){
                    ID = 1,
                    Smetka = company.ZiroSmetka,
                    Ime = bankName
                };
                data.bankarski_smetki = new List<BankarskaSmetka>();
                data.bankarski_smetki.Add(tmpBank);
            }
            

            if(kazni_i_sankcii != null)
                data.kazni_i_sankcii = kazni_i_sankcii;

            if (solventnost != null)
                data.solventnost = solventnost;

            if(promeni != null)
                data.promeni = promeni;

            //// A
            //// просек од 47 за тековната и претходната година
            var prosek_47 = 0.0;
            var prosek_47_1 = 0.0;
            if (company_values_lastyear.ContainsKey(47) && company_values.ContainsKey(47))
            {
                var tmp = company_values[47] + company_values_lastyear[47];
                prosek_47 = (tmp / 2);
            }
            else if (company_values.ContainsKey(47))
            {
                prosek_47 = company_values[47];
            }

            if (company_values_lastyear.ContainsKey(47)) 
                prosek_47_1 = company_values_lastyear[47];




            // Приходи од продажба/просечни побарувања
            var tmpA_prihodi_od_prodazba_prosecni_pobaruvanja = 0.00;
            if (prosek_47 != 0)
            {
                if (company_values.ContainsKey(202))
                {
                    var resVal = ((double)company_values[202] / prosek_47);
                    A_prihodi_od_prodazba_prosecni_pobaruvanja = (resVal * 100).ToString("0.00") + "%";
                    tmpA_prihodi_od_prodazba_prosecni_pobaruvanja = (resVal * 100);

                    if (string.IsNullOrEmpty(A_prihodi_od_prodazba_prosecni_pobaruvanja))
                        A_prihodi_od_prodazba_prosecni_pobaruvanja = "";
                }
                if (string.IsNullOrEmpty(A_prihodi_od_prodazba_prosecni_pobaruvanja))
                    A_prihodi_od_prodazba_prosecni_pobaruvanja = "";

            }

            var tmpA_prihodi_od_prodazba_prosecni_pobaruvanja_1 = 0.00;
            if (prosek_47_1 != 0) {

                if (company_values_lastyear.ContainsKey(202))
                {
                    var resVal = ((double)company_values_lastyear[202] / prosek_47_1);
                    A_prihodi_od_prodazba_prosecni_pobaruvanja_1 = (resVal * 100).ToString("0.00") + "%";
                    tmpA_prihodi_od_prodazba_prosecni_pobaruvanja_1 = (resVal * 100);

                    if (string.IsNullOrEmpty(A_prihodi_od_prodazba_prosecni_pobaruvanja_1))
                        A_prihodi_od_prodazba_prosecni_pobaruvanja = "";
                }
                if (string.IsNullOrEmpty(A_prihodi_od_prodazba_prosecni_pobaruvanja_1))
                    A_prihodi_od_prodazba_prosecni_pobaruvanja_1 = "";
            }

            ////Денови на обрт на побарувања
            if (A_prihodi_od_prodazba_prosecni_pobaruvanja != "")
            {
                var tmpRes = tmpA_prihodi_od_prodazba_prosecni_pobaruvanja;
                denovi_na_obrt_na_pobaruvanje = ((365.0 / tmpRes) * 100.0).ToString("0");

                if (string.IsNullOrEmpty(denovi_na_obrt_na_pobaruvanje))
                    denovi_na_obrt_na_pobaruvanje = "";
            }
            if (A_prihodi_od_prodazba_prosecni_pobaruvanja_1 != "")
            {
                var tmpRes = tmpA_prihodi_od_prodazba_prosecni_pobaruvanja_1;
                denovi_na_obrt_na_pobaruvanje_1 = ((365.0 / tmpRes) * 100.0).ToString("0");

                if (string.IsNullOrEmpty(denovi_na_obrt_na_pobaruvanje_1))
                    denovi_na_obrt_na_pobaruvanje_1 = "";
            }


            //Б
            //просек од 37 за тековната и претходната година
            var prosek_37 = 0.0;
            var prosek_37_1 = 0.0;
            if (company_values_lastyear.ContainsKey(37) && company_values.ContainsKey(37))
            {
                prosek_37 = ((company_values[37] + company_values_lastyear[37]) / 2);
            }
            else if (company_values.ContainsKey(37))
                prosek_37 = company_values[37];

            if (company_values_lastyear.ContainsKey(37))
                prosek_37_1 = company_values_lastyear[37];

            //Обрт на залихи
            var tmpB_obrt_na_zalihi = 0.0;
            if (prosek_37 != 0)
            {
                var resVal = (SumValues(company_values, new int[] { 208, 209, 210, 211 }) / prosek_37);
                B_obrt_na_zalihi = (resVal * 100).ToString("0.00") + "%";
                tmpB_obrt_na_zalihi = (resVal * 100);

                if (string.IsNullOrEmpty(B_obrt_na_zalihi))
                    B_obrt_na_zalihi = "";

            }
            var tmpB_obrt_na_zalihi_1 = 0.0;
            if (prosek_37_1 != 0) {

                var resVal = (SumValues(company_values_lastyear, new int[] { 208, 209, 210, 211 }) / prosek_37_1);
                B_obrt_na_zalihi_1 = (resVal * 100).ToString("0.00") + "%";
                tmpB_obrt_na_zalihi_1 = (resVal * 100);

                if (string.IsNullOrEmpty(B_obrt_na_zalihi_1))
                    B_obrt_na_zalihi_1 = "";
            }

            //Денови на обрт на залихи
            if (B_obrt_na_zalihi != "")
            {
                var tmpRes = tmpB_obrt_na_zalihi;
                denovi_na_obrt_na_zalihi = ((365.0 / tmpRes) * 100.0).ToString("0");
                if (string.IsNullOrEmpty(denovi_na_obrt_na_zalihi))
                    denovi_na_obrt_na_zalihi = "";
            }
            if (B_obrt_na_zalihi_1 != "")
            {
                var tmpRes = tmpB_obrt_na_zalihi_1;
                denovi_na_obrt_na_zalihi_1 = ((365.0 / tmpRes) * 100.0).ToString("0");
                if (string.IsNullOrEmpty(denovi_na_obrt_na_zalihi_1))
                    denovi_na_obrt_na_zalihi_1 = "";
            }

            //В
            //просек од 63 за тековната и претходната година
            var prosek_63 = 0.0;
            var prosek_63_1 = 0.0;
            if (company_values_lastyear.ContainsKey(63) && company_values.ContainsKey(63))
            {
                prosek_63 = (company_values[63] + company_values_lastyear[63]) / 2.0;
            }
            else if (company_values.ContainsKey(63))
                prosek_63 = company_values[63];

            if(company_values_lastyear.ContainsKey(63))
                prosek_63_1 = company_values_lastyear[63];

            //Обрт на средствата
            var tmpV_obrt_na_sredstva = 0.0;
            if (prosek_63 != 0)
            {
                if (company_values.ContainsKey(202))
                {
                    if (company_values[202] > 0)
                    {
                        var resVal = ((double)company_values[202] / prosek_63);
                        V_obrt_na_sredstva = (resVal * 100).ToString("0.00") + "%";
                        tmpV_obrt_na_sredstva = (resVal * 100);
                    }
                }

                if (string.IsNullOrEmpty(V_obrt_na_sredstva))
                    V_obrt_na_sredstva = "";
            }
            var tmpV_obrt_na_sredstva_1 = 0.0;
            if (prosek_63_1 != 0)
            {
                if (company_values_lastyear.ContainsKey(202))
                {
                    if (company_values_lastyear[202] > 0)
                    {
                        var resVal = ((double)company_values_lastyear[202] / prosek_63_1);
                        V_obrt_na_sredstva_1 = (resVal * 100).ToString("0.00") + "%";
                        tmpV_obrt_na_sredstva_1 = (resVal * 100);
                    }
                }

                if (string.IsNullOrEmpty(V_obrt_na_sredstva_1))
                    V_obrt_na_sredstva_1 = "";
            }

            //Денови на обрт на средствата
            if (V_obrt_na_sredstva != "")
            {
                var tmpVal1 = tmpV_obrt_na_sredstva;
                denovi_na_obrt_na_sredstva = ((365.0 / tmpVal1) * 100.0).ToString("0");
                if (string.IsNullOrEmpty(denovi_na_obrt_na_sredstva))
                    denovi_na_obrt_na_sredstva = "";
            }
            if (V_obrt_na_sredstva_1 != "")
            {
                var tmpVal1 = tmpV_obrt_na_sredstva_1;
                denovi_na_obrt_na_sredstva_1 = ((365.0 / tmpVal1) * 100.0).ToString("0");
                if (string.IsNullOrEmpty(denovi_na_obrt_na_sredstva_1))
                    denovi_na_obrt_na_sredstva_1 = "";
            }

            // Г
            //(просек од 97 за тековната и претходната година)
            var prosek_97 = 0.0;
            var prosek_97_1 = 0.0;
            if (company_values_lastyear.ContainsKey(97) && company_values.ContainsKey(97))
            {
                prosek_97 = ((company_values[97] + company_values_lastyear[97]) / 2);
            }
            else if (company_values.ContainsKey(97))
                prosek_97 = company_values[97];

            if (company_values_lastyear.ContainsKey(97))
                prosek_97_1 = company_values_lastyear[97];

            // Обрт на обврските
            var tmpG_obrt_na_obvrski = 0.0;
            if (prosek_97 != 0)
            { 
                var tmpGobrt = CalculateValues(company_values, new int[] { 207, 204, 205, 218 }, new string[] { "+", "-", "-" });

                G_obrt_na_obvrski = (tmpGobrt / prosek_97).ToString("0.00");
                tmpG_obrt_na_obvrski = (tmpGobrt / prosek_97);


                if (string.IsNullOrEmpty(G_obrt_na_obvrski))
                    G_obrt_na_obvrski = "";
            }
            var tmpG_obrt_na_obvrski_1= 0.0;
            if (prosek_97_1 != 0) {

                var tmpGobrt = CalculateValues(company_values_lastyear, new int[] { 207, 204, 205, 218 }, new string[] { "+", "-", "-" });

                G_obrt_na_obvrski_1 = (tmpGobrt / prosek_97_1).ToString("0.00");
                tmpG_obrt_na_obvrski_1 = (tmpGobrt / prosek_97_1);

                if (string.IsNullOrEmpty(G_obrt_na_obvrski_1))
                    G_obrt_na_obvrski_1 = "";
            }

            //Просечни денови на плаќање на обврските спрема добавувачите
            if (G_obrt_na_obvrski != "")
            {
                var resVal = 365.0 / tmpG_obrt_na_obvrski;

                prosecni_denovi_na_plakanje_obvrski = Math.Round(resVal).ToString();

                if (string.IsNullOrEmpty(prosecni_denovi_na_plakanje_obvrski))
                    prosecni_denovi_na_plakanje_obvrski = "";

            }
            if (G_obrt_na_obvrski_1 != "")
            {
                var resVal = 365.0 / tmpG_obrt_na_obvrski_1;

                prosecni_denovi_na_plakanje_obvrski_1 = Math.Ceiling(resVal).ToString();

                if (string.IsNullOrEmpty(prosecni_denovi_na_plakanje_obvrski_1))
                    prosecni_denovi_na_plakanje_obvrski_1 = "";

            }


            //Д
            //Бруто оперативна добивка

            D_bruto_operativna_dobivka = SubtractValues(company_values, new int[] { 202, 208, 209, 210, 211 }).ToString("0.00");

            if (string.IsNullOrEmpty(D_bruto_operativna_dobivka))
                D_bruto_operativna_dobivka = "0";

            D_bruto_operativna_dobivka_1 = SubtractValues(company_values_lastyear, new int[] { 202, 208, 209, 210, 211 }).ToString("0.00");

            if (string.IsNullOrEmpty(D_bruto_operativna_dobivka_1))
                D_bruto_operativna_dobivka_1 = "0";
            //Ѓ
            //ЕБИТДА
            //var tmpSum1 = SumValues(company_values, new int[] { 250, 230, 226 });
            //var tmpSub1 = SubtractValues(company_values, new int[] { 251, 239, 236, 226, 218, 219, 220 });


            //Gj_EBITDA = (tmpSum1 - tmpSub1).ToString("0.00");
            Gj_EBITDA = CalculateValues(company_values, new int[] { 250, 251, 244, 245, 234, 223, 218 }, new string[] { "-", "-", "+", "+", "-", "+" }).ToString("0.00");

            if (string.IsNullOrEmpty(Gj_EBITDA))
                Gj_EBITDA = "0";

            //var tmpSum2 = SumValues(company_values_lastyear, new int[] { 250, 230, 226 });
            //var tmpSub2 = SubtractValues(company_values_lastyear, new int[] { 251, 239, 236, 226, 218, 219, 220 });

            //Gj_EBITDA_1 = (tmpSum2 - tmpSub2).ToString("0.00");
            Gj_EBITDA_1 = CalculateValues(company_values_lastyear, new int[] { 250, 251, 244, 245, 234, 223, 218 }, new string[] { "-", "-", "+", "+", "-", "+" }).ToString("0.00");

            if (string.IsNullOrEmpty(Gj_EBITDA_1))
                Gj_EBITDA_1 = "0";

            //Gj_EBITDA = "";

            //Е
            //ЕБИТ

           // E_EBIT = (SubtractValues(company_values, new int[] { 250 }) - SumValues(company_values, new int[] { 251, 239, 236 })).ToString("0.00");
            E_EBIT = CalculateValues(company_values, new int[] { 250, 251, 244, 245, 234, 223 }, new string[] { "-", "-", "+", "+", "-" }).ToString("0.00");

            if (string.IsNullOrEmpty(E_EBIT))
                E_EBIT = "";

            //E_EBIT_1 = (SubtractValues(company_values_lastyear, new int[] { 250 }) - SumValues(company_values_lastyear, new int[] { 251, 239, 236 })).ToString("0.00");
            E_EBIT_1 = CalculateValues(company_values_lastyear, new int[] { 250, 251, 244, 245, 234, 223 }, new string[] { "-", "-", "+", "+", "-" }).ToString("0.00");

            if (string.IsNullOrEmpty(E_EBIT_1))
                E_EBIT_1 = "";



            //Вкупни обврски / ЕБИТДА
            if (Gj_EBITDA != "")
            {
                vkupni_obvrski_ebitda = (SumValues(company_values, new int[] { 95, 85, 82 }) / Convert.ToDouble(Gj_EBITDA)).ToString("0.00");
            }

            if (string.IsNullOrEmpty(vkupni_obvrski_ebitda))
                vkupni_obvrski_ebitda = "0";

            if (Gj_EBITDA_1 != "")
            {
                vkupni_obvrski_ebitda_1 = (SumValues(company_values_lastyear, new int[] { 95, 85, 82 }) / Convert.ToDouble(Gj_EBITDA_1)).ToString("0.00");
            }

            if (string.IsNullOrEmpty(vkupni_obvrski_ebitda_1))
                vkupni_obvrski_ebitda_1 = "0";


            //Индикатор за способност за сервисирање на долг - DSCR(debt servicing coverage ratio)
           
            var d_bruto_int = Convert.ToDouble(D_bruto_operativna_dobivka);

            var tmpResSum = d_bruto_int + SumValues(company_values, new int[] { 218, 234 });

            var tmpDevRes1 = (SumValues(company_values, new int[] { 85, 82 }) / 4);
            var tmpResSum2 = SumValues(company_values, new int[] { 103, 104, 234 });

            DSCR = "0";
            DSCR = (tmpResSum / (tmpDevRes1 + tmpResSum2)).ToString("0.00");
            //if (tmpDevRes1 != "/" && tmpDevRes1 != "n. def.")
            //{
            //    DSCR = (tmpResSum / (tmpDevRes1 + tmpResSum2)).ToString("0.00");
            //}
            //else
            //{
            //    if (tmpResSum2 != 0)
            //        DSCR = (tmpResSum / tmpResSum2).ToString("0.00");
            //}
            //645550 / 67870
            var d_bruto_int_1 = Convert.ToDouble(D_bruto_operativna_dobivka_1);

            var tmpResSum_1 = d_bruto_int_1 + SumValues(company_values_lastyear, new int[] { 218, 234 });

            var tmpDevRes1_1 = (SumValues(company_values_lastyear, new int[] { 85, 82 }) / 4);
            var tmpResSum2_1 = SumValues(company_values_lastyear, new int[] { 103, 104, 234 });

            DSCR_1 = "0";
            DSCR_1 = (tmpResSum_1 / (tmpDevRes1_1 + tmpResSum2_1)).ToString("0.00");
            //if (tmpDevRes1_1 != "/" && tmpDevRes1_1 != "n. def.")
            //{
            //    DSCR_1 = (tmpResSum_1 / (tmpDevRes1_1 + tmpResSum2_1)).ToString("0.00");
            //}
            //else {
            //    if(tmpResSum2_1 != 0)
            //        DSCR_1 = (tmpResSum_1 / tmpResSum2_1).ToString("0.00");
            //}


            //Покриеност на камати
            if (E_EBIT != "") {
                var tmpRes = SumValues(company_values, new int[] { 234 });
                if(tmpRes != 0)
                    pokrienost_na_kamati = (Convert.ToDouble(E_EBIT) / tmpRes).ToString("0.00");
            }

            if (string.IsNullOrEmpty(pokrienost_na_kamati))
                pokrienost_na_kamati = "";

            if (E_EBIT_1 != "")
            {
                var tmpRes = SumValues(company_values_lastyear, new int[] { 234 });
                if (tmpRes != 0)
                    pokrienost_na_kamati_1 = (Convert.ToDouble(E_EBIT_1) / tmpRes).ToString("0.00");
            }

            if (string.IsNullOrEmpty(pokrienost_na_kamati_1))
                pokrienost_na_kamati_1 = "";


            if (company.KratkoIme != null)
                data.ime_firma = company.KratkoIme;
            else if (company.CelosenNazivNaSubjektot != null)
                data.ime_firma = company.CelosenNazivNaSubjektot;
            else
                data.ime_firma = t_company.Name;
            data.drzava = "[Р.Македонија]";

            //if(t_company.Mesto != null)
            //    data.drzava = "[" + t_company.Mesto + "]";
            //else
            //    data.drzava = "[" + company.Sediste + "]";
            
            var CurDate = FormatStringDate(DateTime.Now.ToString());
            data.datum_izdavanje = CurDate;

            if (string.IsNullOrEmpty(clientEMBS) == false)
            {
                var curUser = DALHelper.GetUserByEMBS(clientEMBS);
                data.izdaden_za = curUser.Username;
            }
            else
            {
                var curUser = DALHelper.GetUserByID(UserID);
                data.izdaden_za = curUser.Username;
            }


            data.uplaten_del = "";
            data.neparicen_vlog = "";
            data.paricen_vlog = "";
            if (COG_Error == false)
            {
                if (company_osnovna_glavina.UplatenDel != null && company_osnovna_glavina.UplatenDel > 0)
                {
                    data.uplaten_del = company_osnovna_glavina.UplatenDel.ToString();
                    data.uplaten_del = "<p>Уплатен дел:<span>" + FormatCurrencyComma(data.uplaten_del) + ".00 " + GetCurrencyName(company_osnovna_glavina.UplatenDelValuta) + "</span></p>";
                }
                if (company_osnovna_glavina.ParicenVlog != null && company_osnovna_glavina.ParicenVlog > 0)
                {
                    data.paricen_vlog = company_osnovna_glavina.ParicenVlog.ToString();
                    data.paricen_vlog = "<p>Паричен влог:<span>" + FormatCurrencyComma(data.paricen_vlog) + ".00 " + GetCurrencyName(company_osnovna_glavina.ParicenVlogValuta) + "</span></p>";
                }
                if (company_osnovna_glavina.NeparicenVlog != null && company_osnovna_glavina.NeparicenVlog > 0)
                {
                    data.neparicen_vlog = company_osnovna_glavina.NeparicenVlog.ToString();
                    data.neparicen_vlog = "<p>Непаричен влог:<span>" + FormatCurrencyComma(data.neparicen_vlog) + ".00 " + GetCurrencyName(company_osnovna_glavina.NeparicenVlogValuta) + "</span></p>";
                }
            }
            

            data.tekovni_sopstvenici = "";
            data.tekovni_podruznici = "";

            var blokadaRes = new Dictionary<string, string>();

            var blokadaTicket = "BonitetenIzvestaj_" + DateTime.Now.Ticks;
            blokadaRes.Add("AccInfo", "Нема блокада");
            blokadaRes.Add("TimeStamp", DateTime.Now.ToString());
            //blokadaRes = CRM_DocumentClass.GetCRM_BlokadaStatus(CRM_ServiceHelper.GetLiveCRM_AccountStatus(EMBS, UserID, blokadaTicket));
            // need to handle error messages
            if(blokadaRes != null)
                data.tekovni_blokadi_status = blokadaRes["AccInfo"];


            data.sostojba_komentar = "";
            data.semafor_solventnost = "";
            data.solventnost_komentar = "";

            data.naziv_firma = data.ime_firma;
            data.celosen_naziv_firma = "";
            if (company.CelosenNazivNaSubjektot != null)
                data.celosen_naziv_firma = company.CelosenNazivNaSubjektot;

            if (company.Sediste != null)
                data.adresa_firma = company.Sediste;
            else
                data.adresa_firma = t_company.Mesto;

            data.pravna_forma = "";
            if(company.VidNaSubjektNaUpis != null)
                data.pravna_forma = company.VidNaSubjektNaUpis;
            if(CD_Error == false){
                data.dejnost = company_dejnost.GlavnaPrihodnaShifra.ToString() + "-" + company_dejnost.PrioritetnaDejnost.ToString();
                data.dejnost = ConvertChars(data.dejnost, "lower");
            }

            data.embs = EMBS;

            data.edb = "";
            if (company.EdinstvenDanocenBroj != null)
                data.edb = company.EdinstvenDanocenBroj;

            data.golemina_na_subjekt = "";
            if (company.GoleminaNaDelovniotSubjekt != null)
                data.golemina_na_subjekt = FirstCharToUpper(company.GoleminaNaDelovniotSubjekt);


            data.vkupna_osnovna_glavnina = (COG_Error == false) ? company_osnovna_glavina.VkupnoOsnovnaGlavina.ToString() : "";
            
            if (string.IsNullOrEmpty(data.vkupna_osnovna_glavnina) == false)
                data.vkupna_osnovna_glavnina = "<p>Вкупна основна главнина:<span>" + FormatCurrencyComma(data.vkupna_osnovna_glavnina) + ".00 " + GetCurrencyName(company_osnovna_glavina.VkupnoOsnovnaGlavinaValuta) + "</span></p>";

            data.datum_osnovanje = "";
            if (company.DatumNaOsnovanje != null)
                data.datum_osnovanje = "<p>Основана:<span>" + FormatStringDate(company.DatumNaOsnovanje.ToString()) + "</span></p>";
            
            //data.prosecen_broj_vraboteni = (CRV_Error == false) ? company_rabotno_vreme.BrojNaVraboteni.ToString() : "";
            data.prosecen_broj_vraboteni = SumValues(company_values, new int[] { 257 }).ToString();

            data.ddv_obvrznik = "НЕ";
            if (data.edb.Contains("MK"))
                data.ddv_obvrznik = "ДА";


            data.ovlasteni_lica = company_ovlasteni_lica;

            data.vkupen_broj_sopstvenici = "";
            data.prikazan_broj_sopstvenici = "";

            data.vkupen_broj_ovlasteni = "";
            data.prikazan_broj_ovlasteni = "";

            data.prikazan_broj_podruznici = "";
            data.vkupen_broj_podruznici = "";

            data.kategorija1 = LastYear.ToString();

            data.sredstva1 = "";
            data.kapital1 = "";
            data.vkupno_prihodi1 = "";
            data.prosecen_broj_vraboteni1 = "";
            data.neto_dobivka_za_delovna_godina1 = "";
            data.koeficient_na_zadolzensot1 = "";
            data.tekoven_pokazatel1 = "";
            data.povrat_na_sredstva1 = "";
            data.povrat_na_kapital1 = "";
            data.indikatori_tekovni_obvrski_2012 = "";
            data.indikatori_kratkorocni_krediti_2012 = "";
            data.indikatori_koeficient_zadolzenost_2012 = "";
            data.indikatori_finansiski_leviridz_2012 = "";
            data.indikatori_neto_profitna_margina_2012 = "";
            data.indikatori_povrat_sredstva_2012 = "";
            data.indikatori_povrat_kapital_2012 = "";
            data.profitabilnost_bruto_operativna_dobivka2 = "";
            data.profitabilnost_neto_dobivka_od_finansiranje2 = "";
            data.profitabilnost_ebitda2 = "";
            data.profitabilnost_ebit2 = "";

            data.indikatori_raboten_kapital_2012 = "";
            data.indikatori_tekoven_pokazatel_2012 = "";
            data.indikatori_brz_pokazatel_2012 = "";

            data.uspeh_prihodi_rabotenje_2012 = "";
            data.uspeh_finansiski_prihodi_2012 = "";
            data.uspeh_udel_vo_dobivka_2012 = "";
            data.uspeh_rashodi_rabotenje_2012 = "";
            data.uspeh_rashod_osnovna_dejnost_2012 = "";
            data.uspeh_ostanati_trosoci_2012 = "";
            data.uspeh_trosoci_za_vraboteni_2012 = "";
            data.uspeh_amortizacija_sredstva_2012 = "";
            data.uspeh_rezerviranje_trosoci_rizici_2012 = "";
            data.uspeh_zalihi_proizvodi_pocetok_2012 = "";
            data.uspeh_zalihi_proizvodi_kraj_2012 = "";
            data.uspeh_ostanati_rashodi_2012 = "";
            data.uspeh_finansiski_rashodi_2012 = "";
            data.uspeh_finansiski_povrzani_drustva_2012 = "";
            data.uspeh_rashodi_kamati_2012 = "";
            data.uspeh_ostanati_finansiski_rashodi_2012 = "";
            data.uspeh_udel_vo_zaguba_2012 = "";

            data.uspeh_dobivka_odanocuvanje_2012 = "";
            data.uspeh_zaguba_odanocuvanje_2012 = "";
            data.uspeh_danok_dobivka_2012 = "";
            data.uspeh_neto_dobivka_2012 = "";
            data.uspeh_neto_zaguba_2012 = "";

            data.bilans_netekovni_2012 = "";
            data.bilans_nematerijalni_2012 = "";
            data.bilans_materijalni_2012 = "";
            data.bilans_vlozuvanje_nedviznosti_2012 = "";
            data.bilans_dolgorocni_sredstva_2012 = "";
            data.bilans_dolgorocni_pobaruvanja_2012 = "";
            data.bilans_odlozeni_danocni_sredstva_2012 = "";
            data.bilans_tekovni_sredstva_2012 = "";
            data.bilans_zalihi_2012 = "";
            data.bilans_kratkorocni_pobaruvanja_2012 = "";
            data.bilans_kratkorocni_sredstva_2012 = "";
            data.bilans_paricni_sredstva_2012 = "";
            data.bilans_sredstva_grupa_2012 = "";
            data.bilans_plateni_trosoci_2012 = "";
            data.bilans_vkupna_aktiva_2012 = "";
            data.bilans_glavnina_i_rezervi_2012 = "";
            data.bilans_osnovna_glavnina_2012 = "";
            data.bilans_premii_akcii_2012 = "";
            data.bilans_sopstveni_akcii_2012 = "";
            data.bilans_zapisan_kapital_2012 = "";
            data.bilans_revalorizaciska_rezerva_2012 = "";
            data.bilans_rezervi_2012 = "";
            data.bilans_akumulirana_dobivka_2012 = "";
            data.bilans_prenesena_zaguba_2012 = "";
            data.bilans_dobivka_delovna_godina_2012 = "";
            data.bilans_zaguba_delovna_godina_2012 = "";
            data.bilans_dolgorocni_obvrski_2012 = "";
            data.bilans_kratkorocni_obvrski_2012 = "";
            data.bilans_odlozeni_obvrski_2012 = "";
            data.bilans_odlozeno_plakanje_2012 = "";
            data.bilans_obvrski_po_osnov_2012 = "";
            data.bilans_vkupna_pasiva_2012 = "";
            data.bilans_dolgorocni_rezerviranja_2012 = "";
            data.bilans_obvrski_2012 = "";

            data.uspeh_rashod_osnovna_dejnost_2012 = SumValues(company_values_lastyear, new int[] { 208, 209, 210, 211 }).ToString();
            data.uspeh_rashod_osnovna_dejnost_2012 = FormatCurrencyComma(data.uspeh_rashod_osnovna_dejnost_2012);

            if (company_values_lastyear.ContainsKey(63))
            {
                data.sredstva1 = company_values_lastyear[63].ToString();
                data.sredstva1 = FormatCurrencyComma(data.sredstva1);
            }

            if (company_values_lastyear.ContainsKey(65))
            {
                data.kapital1 = company_values_lastyear[65].ToString();
                data.kapital1 = FormatCurrencyComma(data.kapital1);
            }

            data.vkupno_prihodi1 = SumValues(company_values_lastyear, new int[]{201,223,244}).ToString();
            data.vkupno_prihodi1 = FormatCurrencyComma(data.vkupno_prihodi1);
            
            if (company_values_lastyear.ContainsKey(257))
            {
                data.prosecen_broj_vraboteni1 = company_values_lastyear[257].ToString();
                data.prosecen_broj_vraboteni1 = FormatCurrencyComma(data.prosecen_broj_vraboteni1);
            }

            data.neto_dobivka_za_delovna_godina1 = SubtractValues(company_values_lastyear, new int[] { 255, 256 }).ToString();
            data.neto_dobivka_za_delovna_godina1 = FormatCurrencyComma(data.neto_dobivka_za_delovna_godina1);

            data.koeficient_na_zadolzensot1 = DevideValues(SumValues(company_values_lastyear, new int[] { 82, 85, 95 }), company_values_lastyear, new int[] {63});

            data.tekoven_pokazatel1 = DevideValues(company_values_lastyear, new int[] { 36, 95 });

            if (company_values_lastyear.ContainsKey(63))
            {
                var tmp = SubtractValues(company_values_lastyear, new int[] { 255, 256 });

                var resVal = tmp / company_values_lastyear[63];

                resVal = resVal * 100;

                data.povrat_na_sredstva1 = resVal.ToString("0.00") + "%";
            }

            if (company_values_lastyear.ContainsKey(65))
            {
                var tmp = SubtractValues(company_values_lastyear, new int[] { 255, 256 });

                var resVal = tmp / company_values_lastyear[65];

                resVal = resVal * 100;

                data.povrat_na_kapital1 = resVal.ToString("0.00") + "%";
            }

            if (company_values_lastyear.ContainsKey(202) && company_values_lastyear.ContainsKey(95))
            {
                var tmp = company_values_lastyear[95] / company_values_lastyear[202];

                tmp = tmp * 100;

                data.indikatori_tekovni_obvrski_2012 = tmp.ToString("0.00") + "%";
            }

            if (company_values_lastyear.ContainsKey(202))
            {
                var tmp = SumValues(company_values_lastyear, new int[] { 103, 104 });

                var resVal = tmp / company_values_lastyear[202];

                resVal = resVal * 100;

                data.indikatori_kratkorocni_krediti_2012 = resVal.ToString("0.00") + "%";
            }


            data.indikatori_koeficient_zadolzenost_2012 = DevideValues(SumValues(company_values_lastyear, new int[] { 82, 85, 95 }), company_values_lastyear, new int[] { 63 });

            data.indikatori_finansiski_leviridz_2012 = DevideValues(SumValues(company_values_lastyear, new int[] { 85, 95, 82 }), company_values_lastyear, new int[] { 65 });

            if (company_values_lastyear.ContainsKey(202))
            {
                var tmp = SubtractValues(company_values_lastyear, new int[] { 255, 256 });

                var resVal = tmp / company_values_lastyear[202];

                resVal = resVal * 100;

                data.indikatori_neto_profitna_margina_2012 = resVal.ToString("0.00") + "%";
            }
            
            if (company_values_lastyear.ContainsKey(63))
            {
                var tmp = SubtractValues(company_values_lastyear, new int[] { 255, 256 });

                var resVal = tmp / company_values_lastyear[63];

                resVal = resVal * 100;

                data.indikatori_povrat_sredstva_2012 = resVal.ToString("0.00") + "%";
            }

            if (company_values_lastyear.ContainsKey(65))
            {
                var tmp = SubtractValues(company_values_lastyear, new int[] { 255, 256 });

                var resVal = tmp / company_values_lastyear[65];

                resVal = resVal * 100;

                data.indikatori_povrat_kapital_2012 = resVal.ToString("0.00") + "%";
            }


            data.indikatori_brz_pokazatel_2012 = DevideValues(SubtractValues(company_values_lastyear, new int[] { 36, 37 }), company_values_lastyear, new int[] {95});

            data.indikatori_tekoven_pokazatel_2012 = DevideValues(company_values_lastyear, new int[] { 36, 95 });

            data.indikatori_raboten_kapital_2012 = SubtractValues(company_values_lastyear, new int[] { 36, 95 }).ToString("0.00");
            data.indikatori_raboten_kapital_2012 = FormatCurrencyComma(data.indikatori_raboten_kapital_2012);

            if (company_values_lastyear.ContainsKey(256))
            {
                data.uspeh_neto_zaguba_2012 = company_values_lastyear[256].ToString();
                data.uspeh_neto_zaguba_2012 = FormatCurrencyComma(data.uspeh_neto_zaguba_2012);
            }

            if (company_values_lastyear.ContainsKey(255))
            {
                data.uspeh_neto_dobivka_2012 = company_values_lastyear[255].ToString();
                data.uspeh_neto_dobivka_2012 = FormatCurrencyComma(data.uspeh_neto_dobivka_2012);
            }

            if (company_values_lastyear.ContainsKey(252))
            {
                data.uspeh_danok_dobivka_2012 = company_values_lastyear[252].ToString();
                data.uspeh_danok_dobivka_2012 = FormatCurrencyComma(data.uspeh_danok_dobivka_2012);
            }

            if (company_values_lastyear.ContainsKey(251))
            {
                data.uspeh_zaguba_odanocuvanje_2012 = company_values_lastyear[251].ToString();
                data.uspeh_zaguba_odanocuvanje_2012 = FormatCurrencyComma(data.uspeh_zaguba_odanocuvanje_2012);
            }

            if (company_values_lastyear.ContainsKey(250))
            {
                data.uspeh_dobivka_odanocuvanje_2012 = company_values_lastyear[250].ToString();
                data.uspeh_dobivka_odanocuvanje_2012 = FormatCurrencyComma(data.uspeh_dobivka_odanocuvanje_2012);
            }

            if (company_values_lastyear.ContainsKey(245))
            {
                data.uspeh_udel_vo_zaguba_2012 = company_values_lastyear[245].ToString();
                data.uspeh_udel_vo_zaguba_2012 = FormatCurrencyComma(data.uspeh_udel_vo_zaguba_2012);
            }

            if (company_values_lastyear.ContainsKey(244))
            {
                data.uspeh_udel_vo_dobivka_2012 = company_values_lastyear[244].ToString();
                data.uspeh_udel_vo_dobivka_2012 = FormatCurrencyComma(data.uspeh_udel_vo_dobivka_2012);
            }

            if (company_values_lastyear.ContainsKey(243))
            {
                data.uspeh_ostanati_finansiski_rashodi_2012 = company_values_lastyear[243].ToString();
                data.uspeh_ostanati_finansiski_rashodi_2012 = FormatCurrencyComma(data.uspeh_ostanati_finansiski_rashodi_2012);
            }

            data.uspeh_rashodi_kamati_2012 = SumValues(company_values_lastyear, new int[] { 239, 240 }).ToString();
            data.uspeh_rashodi_kamati_2012 = FormatCurrencyComma(data.uspeh_rashodi_kamati_2012);
            

            if (company_values_lastyear.ContainsKey(235))
            {
                data.uspeh_finansiski_povrzani_drustva_2012 = company_values_lastyear[235].ToString();
                data.uspeh_finansiski_povrzani_drustva_2012 = FormatCurrencyComma(data.uspeh_finansiski_povrzani_drustva_2012);
            }

            if (company_values_lastyear.ContainsKey(234))
            {
                data.uspeh_finansiski_rashodi_2012 = company_values_lastyear[234].ToString();
                data.uspeh_finansiski_rashodi_2012 = FormatCurrencyComma(data.uspeh_finansiski_rashodi_2012);
            }

            if (company_values_lastyear.ContainsKey(222))
            {
                data.uspeh_ostanati_rashodi_2012 = company_values_lastyear[222].ToString();
                data.uspeh_ostanati_rashodi_2012 = FormatCurrencyComma(data.uspeh_ostanati_rashodi_2012);
            }

            if (company_values_lastyear.ContainsKey(221))
            {
                data.uspeh_rezerviranje_trosoci_rizici_2012 = company_values_lastyear[221].ToString();
                data.uspeh_rezerviranje_trosoci_rizici_2012 = FormatCurrencyComma(data.uspeh_rezerviranje_trosoci_rizici_2012);
            }

            data.uspeh_amortizacija_sredstva_2012 = SumValues(company_values_lastyear, new int[] { 218, 219, 220 }).ToString();
            data.uspeh_amortizacija_sredstva_2012 = FormatCurrencyComma(data.uspeh_amortizacija_sredstva_2012);

            if (company_values_lastyear.ContainsKey(213))
            {
                data.uspeh_trosoci_za_vraboteni_2012 = company_values_lastyear[213].ToString();
                data.uspeh_trosoci_za_vraboteni_2012 = FormatCurrencyComma(data.uspeh_trosoci_za_vraboteni_2012);
            }

            if (company_values_lastyear.ContainsKey(212))
            {
                data.uspeh_ostanati_trosoci_2012 = company_values_lastyear[212].ToString();
                data.uspeh_ostanati_trosoci_2012 = FormatCurrencyComma(data.uspeh_ostanati_trosoci_2012);
            }


            if (company_values_lastyear.ContainsKey(204))
            {
                data.uspeh_zalihi_proizvodi_pocetok_2012 = company_values_lastyear[204].ToString();
                data.uspeh_zalihi_proizvodi_pocetok_2012 = FormatCurrencyComma(data.uspeh_zalihi_proizvodi_pocetok_2012);
            }

            if (company_values_lastyear.ContainsKey(205))
            {
                data.uspeh_zalihi_proizvodi_kraj_2012 = company_values_lastyear[205].ToString();
                data.uspeh_zalihi_proizvodi_kraj_2012 = FormatCurrencyComma(data.uspeh_zalihi_proizvodi_kraj_2012);
            }




            data.uspeh_rashodi_rabotenje_2012 = CalculateValues(company_values_lastyear, new int[] { 207, 204, 205 }, new string[] { "+", "-" }).ToString();
            data.uspeh_rashodi_rabotenje_2012 = FormatCurrencyComma(data.uspeh_rashodi_rabotenje_2012);


            data.uspeh_vkupno_rashodi_2012 = (SumValues(company_values_lastyear, new int[] { 207, 234, 245 }) + SubtractValues(company_values_lastyear, new int[] { 204, 205 })).ToString();
            data.uspeh_vkupno_rashodi_2012 = FormatCurrencyComma(data.uspeh_vkupno_rashodi_2012);

            if (company_values_lastyear.ContainsKey(223))
            {
                data.uspeh_finansiski_prihodi_2012 = company_values_lastyear[223].ToString();
                data.uspeh_finansiski_prihodi_2012 = FormatCurrencyComma(data.uspeh_finansiski_prihodi_2012);
            }

            if (company_values_lastyear.ContainsKey(201))
            {
                data.uspeh_prihodi_rabotenje_2012 = company_values_lastyear[201].ToString();
                data.uspeh_prihodi_rabotenje_2012 = FormatCurrencyComma(data.uspeh_prihodi_rabotenje_2012);
            }

            if (company_values_lastyear.ContainsKey(111))
            {
                data.bilans_vkupna_pasiva_2012 = company_values_lastyear[111].ToString();
                data.bilans_vkupna_pasiva_2012 = FormatCurrencyComma(data.bilans_vkupna_pasiva_2012);
            }

            if (company_values_lastyear.ContainsKey(110))
            {
                data.bilans_obvrski_po_osnov_2012 = company_values_lastyear[110].ToString();
                data.bilans_obvrski_po_osnov_2012 = FormatCurrencyComma(data.bilans_obvrski_po_osnov_2012);
            }

            if (company_values_lastyear.ContainsKey(109))
            {
                data.bilans_odlozeno_plakanje_2012 = company_values_lastyear[109].ToString();
                data.bilans_odlozeno_plakanje_2012 = FormatCurrencyComma(data.bilans_odlozeno_plakanje_2012);
            }

            if (company_values_lastyear.ContainsKey(94))
            {
                data.bilans_odlozeni_obvrski_2012 = company_values_lastyear[94].ToString();
                data.bilans_odlozeni_obvrski_2012 = FormatCurrencyComma(data.bilans_odlozeni_obvrski_2012);
            }

            if (company_values_lastyear.ContainsKey(95))
            {
                data.bilans_kratkorocni_obvrski_2012 = company_values_lastyear[95].ToString();
                data.bilans_kratkorocni_obvrski_2012 = FormatCurrencyComma(data.bilans_kratkorocni_obvrski_2012);
            }

            if (company_values_lastyear.ContainsKey(85))
            {
                data.bilans_dolgorocni_obvrski_2012 = company_values_lastyear[85].ToString();
                data.bilans_dolgorocni_obvrski_2012 = FormatCurrencyComma(data.bilans_dolgorocni_obvrski_2012);
            }

            if (company_values_lastyear.ContainsKey(82))
            {
                data.bilans_dolgorocni_rezerviranja_2012 = company_values_lastyear[82].ToString();
                data.bilans_dolgorocni_rezerviranja_2012 = FormatCurrencyComma(data.bilans_dolgorocni_rezerviranja_2012);
            }

            if (company_values_lastyear.ContainsKey(81))
            {
                data.bilans_obvrski_2012 = company_values_lastyear[81].ToString();
                data.bilans_obvrski_2012 = FormatCurrencyComma(data.bilans_obvrski_2012);
            }

            if (company_values_lastyear.ContainsKey(78))
            {
                data.bilans_zaguba_delovna_godina_2012 = company_values_lastyear[78].ToString();
                data.bilans_zaguba_delovna_godina_2012 = FormatCurrencyComma(data.bilans_zaguba_delovna_godina_2012);
            }

            if (company_values_lastyear.ContainsKey(77))
            {
                data.bilans_dobivka_delovna_godina_2012 = company_values_lastyear[77].ToString();
                data.bilans_dobivka_delovna_godina_2012 = FormatCurrencyComma(data.bilans_dobivka_delovna_godina_2012);
            }

            if (company_values_lastyear.ContainsKey(76))
            {
                data.bilans_prenesena_zaguba_2012 = company_values_lastyear[76].ToString();
                data.bilans_prenesena_zaguba_2012 = FormatCurrencyComma(data.bilans_prenesena_zaguba_2012);
            }

            if (company_values_lastyear.ContainsKey(75))
            {
                data.bilans_akumulirana_dobivka_2012 = company_values_lastyear[75].ToString();
                data.bilans_akumulirana_dobivka_2012 = FormatCurrencyComma(data.bilans_akumulirana_dobivka_2012);
            }

            if (company_values_lastyear.ContainsKey(71))
            {
                data.bilans_rezervi_2012 = company_values_lastyear[71].ToString();
                data.bilans_rezervi_2012 = FormatCurrencyComma(data.bilans_rezervi_2012);
            }

            if (company_values_lastyear.ContainsKey(70))
            {
                data.bilans_revalorizaciska_rezerva_2012 = company_values_lastyear[70].ToString();
                data.bilans_revalorizaciska_rezerva_2012 = FormatCurrencyComma(data.bilans_revalorizaciska_rezerva_2012);
            }

            data.profitabilnost_bruto_operativna_dobivka2 = SubtractValues(company_values_lastyear, new int[] { 202, 208, 209, 210, 211 }).ToString("0.00");
            data.profitabilnost_bruto_operativna_dobivka2 = FormatCurrencyComma(data.profitabilnost_bruto_operativna_dobivka2);

            //check
            //var tmpRes2 = SubtractValues(company_values_lastyear, new int[] { 223, 234, 209, 210, 211 });
            var tmpRes2 = SubtractValues(company_values_lastyear, new int[] { 223, 234 });
            data.profitabilnost_neto_dobivka_od_finansiranje2 = "0";
            //if (tmpRes2 > 0)
            //{

                data.profitabilnost_neto_dobivka_od_finansiranje2 = tmpRes2.ToString("0.00");
                data.profitabilnost_neto_dobivka_od_finansiranje2 = FormatCurrencyComma(data.profitabilnost_neto_dobivka_od_finansiranje2);
            //}

                data.profitabilnost_ebitda2 = Gj_EBITDA_1;
            data.profitabilnost_ebitda2 = FormatCurrencyComma(data.profitabilnost_ebitda2);

                data.profitabilnost_ebit2 = E_EBIT_1;
            data.profitabilnost_ebit2 = FormatCurrencyComma(data.profitabilnost_ebit2);

            if (company_values_lastyear.ContainsKey(1))
            {
                data.bilans_netekovni_2012 = company_values_lastyear[1].ToString();
                data.bilans_netekovni_2012 = FormatCurrencyComma(data.bilans_netekovni_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(2))
            {
                data.bilans_nematerijalni_2012 = company_values_lastyear[2].ToString();
                data.bilans_nematerijalni_2012 = FormatCurrencyComma(data.bilans_nematerijalni_2012);
            }

            if (company_values_lastyear.ContainsKey(9))
            {
                data.bilans_materijalni_2012 = company_values_lastyear[9].ToString();
                data.bilans_materijalni_2012 = FormatCurrencyComma(data.bilans_materijalni_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(20))
            {
                data.bilans_vlozuvanje_nedviznosti_2012 = company_values_lastyear[20].ToString();
                data.bilans_vlozuvanje_nedviznosti_2012 = FormatCurrencyComma(data.bilans_vlozuvanje_nedviznosti_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(21))
            {
                data.bilans_dolgorocni_sredstva_2012 = company_values_lastyear[21].ToString();
                data.bilans_dolgorocni_sredstva_2012 = FormatCurrencyComma(data.bilans_dolgorocni_sredstva_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(31))
            {
                data.bilans_dolgorocni_pobaruvanja_2012 = company_values_lastyear[31].ToString();
                data.bilans_dolgorocni_pobaruvanja_2012 = FormatCurrencyComma(data.bilans_dolgorocni_pobaruvanja_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(35))
            {
                data.bilans_odlozeni_danocni_sredstva_2012 = company_values_lastyear[35].ToString();
                data.bilans_odlozeni_danocni_sredstva_2012 = FormatCurrencyComma(data.bilans_odlozeni_danocni_sredstva_2012);
            }

            if (company_values_lastyear.ContainsKey(36))
            {
                data.bilans_tekovni_sredstva_2012 = company_values_lastyear[36].ToString();
                data.bilans_tekovni_sredstva_2012 = FormatCurrencyComma(data.bilans_tekovni_sredstva_2012);
            }

            if (company_values_lastyear.ContainsKey(37))
            {
                data.bilans_zalihi_2012 = company_values_lastyear[37].ToString();
                data.bilans_zalihi_2012 = FormatCurrencyComma(data.bilans_zalihi_2012);
            }

            if (company_values_lastyear.ContainsKey(45))
            {
                data.bilans_kratkorocni_pobaruvanja_2012 = company_values_lastyear[45].ToString();
                data.bilans_kratkorocni_pobaruvanja_2012 = FormatCurrencyComma(data.bilans_kratkorocni_pobaruvanja_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(52))
            {
                data.bilans_kratkorocni_sredstva_2012 = company_values_lastyear[52].ToString();
                data.bilans_kratkorocni_sredstva_2012 = FormatCurrencyComma(data.bilans_kratkorocni_sredstva_2012);
            }

            if (company_values_lastyear.ContainsKey(59))
            {
                data.bilans_paricni_sredstva_2012 = company_values_lastyear[59].ToString();
                data.bilans_paricni_sredstva_2012 = FormatCurrencyComma(data.bilans_paricni_sredstva_2012);
            }

            if (company_values_lastyear.ContainsKey(44))
            {
                data.bilans_sredstva_grupa_2012 = company_values_lastyear[44].ToString();
                data.bilans_sredstva_grupa_2012 = FormatCurrencyComma(data.bilans_sredstva_grupa_2012);
            }

            if (company_values_lastyear.ContainsKey(62))
            {
                data.bilans_plateni_trosoci_2012 = company_values_lastyear[62].ToString();
                data.bilans_plateni_trosoci_2012 = FormatCurrencyComma(data.bilans_plateni_trosoci_2012);
            }

            if (company_values_lastyear.ContainsKey(63))
            {
                data.bilans_vkupna_aktiva_2012 = company_values_lastyear[63].ToString();
                data.bilans_vkupna_aktiva_2012 = FormatCurrencyComma(data.bilans_vkupna_aktiva_2012);
            }

            if (company_values_lastyear.ContainsKey(65))
            {
                data.bilans_glavnina_i_rezervi_2012 = company_values_lastyear[65].ToString();
                data.bilans_glavnina_i_rezervi_2012 = FormatCurrencyComma(data.bilans_glavnina_i_rezervi_2012);
            }

            if (company_values_lastyear.ContainsKey(66))
            {
                data.bilans_osnovna_glavnina_2012 = company_values_lastyear[66].ToString();
                data.bilans_osnovna_glavnina_2012 = FormatCurrencyComma(data.bilans_osnovna_glavnina_2012);
            }

            //check
            if (company_values_lastyear.ContainsKey(67))
            {
                data.bilans_premii_akcii_2012 = company_values_lastyear[67].ToString();
                data.bilans_premii_akcii_2012 = FormatCurrencyComma(data.bilans_premii_akcii_2012);
            }

            if (company_values_lastyear.ContainsKey(68))
            {
                data.bilans_sopstveni_akcii_2012 = company_values_lastyear[68].ToString();
                data.bilans_sopstveni_akcii_2012 = FormatCurrencyComma(data.bilans_sopstveni_akcii_2012);
            }

            if (company_values_lastyear.ContainsKey(69))
            {
                data.bilans_zapisan_kapital_2012 = company_values_lastyear[69].ToString();
                data.bilans_zapisan_kapital_2012 = FormatCurrencyComma(data.bilans_zapisan_kapital_2012);
            }

            data.sredstva2 = "";
            data.kapital2 = "";
            data.vkupno_prihodi2 = "";
            data.neto_dobivka_za_delovna_godina2 = "";
            data.prosecen_broj_vraboteni2 = "";
            data.koeficient_na_zadolzensot2 = "";
            data.tekoven_pokazatel2 = "";
            data.povrat_na_sredstva2 = "";
            data.povrat_na_kapital2 = "";
            data.likvidnost_koeficient_za_zadolzenost = "";
            data.likvidnost_brz_pokazatel = "";
            data.likvidnost_kreditna_izlozenost_od_rabotenje = "";
            data.efikasnost_povrat_na_sredstva = "";
            data.efikasnost_neto_profitna_marza = "";
            data.profitabilnost_bruto_operativna_dobivka1 = "";
            data.profitabilnost_neto_dobivka_od_finansiranje1 = "";
            data.profitabilnost_ebitda1 = "";
            data.profitabilnost_ebit1 = "";
            data.indikatori_tekovni_obvrski_2013 = "";
            data.indikatori_kratkorocni_krediti_2013 = "";
            data.indikatori_koeficient_zadolzenost_2013 = "";
            data.indikatori_finansiski_leviridz_2013 = "";
            data.indikatori_neto_profitna_margina_2013 = "";
            data.indikatori_povrat_sredstva_2013 = "";
            data.indikatori_povrat_kapital_2013 = "";
            data.indikatori_brz_pokazatel_2013 = "";
            data.indikatori_tekoven_pokazatel_2013 = "";
            data.indikatori_raboten_kapital_2013 = "";
            data.uspeh_neto_zaguba_2013 = "";
            data.uspeh_neto_dobivka_2013 = "";
            data.uspeh_danok_dobivka_2013 = "";
            data.uspeh_zaguba_odanocuvanje_2013 = "";
            data.uspeh_dobivka_odanocuvanje_2013 = "";
            data.uspeh_udel_vo_zaguba_2013 = "";
            data.uspeh_udel_vo_dobivka_2013 = "";
            data.uspeh_ostanati_finansiski_rashodi_2013 = "";
            data.uspeh_rashodi_kamati_2013 = "";
            data.uspeh_finansiski_povrzani_drustva_2013 = "";

            data.uspeh_finansiski_rashodi_2013 = "";
            data.uspeh_ostanati_rashodi_2013 = "";
            data.uspeh_rezerviranje_trosoci_rizici_2013 = "";
            data.uspeh_amortizacija_sredstva_2013 = "";
            data.uspeh_ostanati_trosoci_2013 = "";
            data.uspeh_rashodi_rabotenje_2013 = "";
            data.uspeh_finansiski_prihodi_2013 = "";
            data.uspeh_prihodi_rabotenje_2013 = "";
            data.bilans_vkupna_pasiva_2013 = "";
            data.bilans_obvrski_po_osnov_2013 = "";
            data.bilans_odlozeno_plakanje_2013 = "";
            data.bilans_odlozeni_obvrski_2013 = "";
            data.bilans_kratkorocni_obvrski_2013 = "";
            data.bilans_dolgorocni_obvrski_2013 = "";
            data.bilans_dolgorocni_rezerviranja_2013 = "";
            data.bilans_obvrski_2013 = "";
            data.bilans_dobivka_delovna_godina_2013 = "";
            data.bilans_prenesena_zaguba_2013 = "";
            data.bilans_akumulirana_dobivka_2013 = "";
            data.bilans_rezervi_2013 = "";
            data.bilans_revalorizaciska_rezerva_2013 = "";
            data.bilans_zapisan_kapital_2013 = "";
            data.bilans_zaguba_delovna_godina_2013 = "";
            data.bilans_sopstveni_akcii_2013 = "";
            data.bilans_premii_akcii_2013 = "";
            data.bilans_osnovna_glavnina_2013 = "";
            data.bilans_glavnina_i_rezervi_2013 = "";
            data.bilans_vkupna_aktiva_2013 = "";
            data.bilans_sredstva_grupa_2013 = "";
            data.bilans_paricni_sredstva_2013 = "";
            data.bilans_kratkorocni_sredstva_2013 = "";
            data.bilans_kratkorocni_pobaruvanja_2013 = "";
            data.bilans_zalihi_2013 = "";
            data.bilans_tekovni_sredstva_2013 = "";
            data.bilans_odlozeni_danocni_sredstva_2013 = "";
            data.bilans_dolgorocni_pobaruvanja_2013 = "";
            data.bilans_dolgorocni_sredstva_2013 = "";
            data.bilans_vlozuvanje_nedviznosti_2013 = "";
            data.bilans_materijalni_2013 = "";
            data.bilans_nematerijalni_2013 = "";
            data.bilans_netekovni_2013 = "";
            data.bilans_plateni_trosoci_2013 = "";
            data.uspeh_trosoci_za_vraboteni_2013 = "";
            data.uspeh_rashod_osnovna_dejnost_2013 = "";
            data.uspeh_zalihi_proizvodi_pocetok_2013 = "";
            data.uspeh_zalihi_proizvodi_kraj_2013 = "";

            data.uspeh_rashod_osnovna_dejnost_2013 = SumValues(company_values, new int[] { 208, 209, 210, 211 }).ToString();
            data.uspeh_rashod_osnovna_dejnost_2013 = FormatCurrencyComma(data.uspeh_rashod_osnovna_dejnost_2013);

            data.likvidnost_prosecni_denovi_na_plakanje_ovrski = "";
            if (prosecni_denovi_na_plakanje_obvrski != null)
                data.likvidnost_prosecni_denovi_na_plakanje_ovrski = prosecni_denovi_na_plakanje_obvrski.ToString();

            if (company_values.ContainsKey(63)) {

                data.sredstva2 = company_values[63].ToString();
                data.sredstva2 = FormatCurrencyComma(data.sredstva2);
            }

            if (company_values.ContainsKey(65))
            {
                data.kapital2 = company_values[65].ToString();
                data.kapital2 = FormatCurrencyComma(data.kapital2);
            
            }

            var tmpVP = SumValues(company_values, new int[] { 201, 223, 244 }) > 1000000;
            data.vkupno_prihodi2 = SumValues(company_values, new int[] { 201, 223, 244 }).ToString();
            data.vkupno_prihodi2 = FormatCurrencyComma(data.vkupno_prihodi2);

            if (tmpVP)
                data.ddv_obvrznik = "ДА";


            data.neto_dobivka_za_delovna_godina2 = SubtractValues(company_values, new int[] { 255, 256 }).ToString();
            data.neto_dobivka_za_delovna_godina2 = FormatCurrencyComma(data.neto_dobivka_za_delovna_godina2);

            if (company_values.ContainsKey(257))
                data.prosecen_broj_vraboteni2 = company_values[257].ToString();


            data.koeficient_na_zadolzensot2 = DevideValues(SumValues(company_values, new int[] { 82, 85, 95 }), company_values, new int[] { 63 });

            data.tekoven_pokazatel2 = DevideValues(company_values, new int[] { 36, 95 });


            if (company_values.ContainsKey(63))
            {
                var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                var resVal = tmp / company_values[63];

                resVal = resVal * 100;

                data.povrat_na_sredstva2 = resVal.ToString("0.00") + "%";
            }

            // check
            if (company_values.ContainsKey(65))
            {
                var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                var resVal = tmp / company_values[65];

                resVal = resVal * 100;

                data.povrat_na_kapital2 = resVal.ToString("0.00") + "%";
            }

            data.likvidnost_koeficient_za_zadolzenost = DevideValues(SumValues(company_values, new int[] { 82, 85, 95 }), company_values, new int[] { 63 });

            data.likvidnost_brz_pokazatel = DevideValues(SubtractValues(company_values, new int[] { 36, 37 }), company_values, new int[] { 95 });

            //check

            if (company_values.ContainsKey(95))
            {
                if (company_values.ContainsKey(202))
                {
                    if (company_values[202] == 0.0)
                        data.likvidnost_kreditna_izlozenost_od_rabotenje = "/";
                    else
                    {
                        var tmp = company_values[95] / company_values[202];

                        tmp = tmp * 100;

                        data.likvidnost_kreditna_izlozenost_od_rabotenje = tmp.ToString("0") + "%";
                    }
                }
            }


            data.likvidnost_opis_main = "";
            data.likvidnost_opis_koeficient_za_zadolzenost = "Незадоволително";
            data.likvidnost_opis_brz_pokazatel = "Незадоволително";
            data.likvidnost_opis_prosecni_denovi = "Незадоволително";
            data.likvidnost_opis_kreditna_izlozenost = "Незадоволително";

            data.short_komentar = "";
            
            var tmp_finansiskaVal1 = 1.0;
            var tmp_finansiskaVal2 = 1.0;
            var tmp_finansiskaVal3 = 1.0;
            var tmp_finansiskaVal4 = 1.0;
            double tmpVal = 0.0;

            if (data.likvidnost_koeficient_za_zadolzenost.Equals("") == false && data.likvidnost_koeficient_za_zadolzenost.Equals("/") == false)
            {
                var str = data.likvidnost_koeficient_za_zadolzenost.Replace(",", "").Replace("%", "");
                tmpVal = double.Parse(str);

                if (tmpVal > 0 && tmpVal <= 0.5)
                {
                    data.likvidnost_opis_koeficient_za_zadolzenost = "Задоволително";
                    data.finansiska_procenka_komentar = "Компанијата има ниска задолженост.";
                    tmp_finansiskaVal1 = 3.0;
                }
                else if (tmpVal > 0.5 && tmpVal <= 0.7001)
                {
                    data.likvidnost_opis_koeficient_za_zadolzenost = "Просечно";
                    data.finansiska_procenka_komentar = "Компанијата има просечна задолженост.";
                    tmp_finansiskaVal1 = 2.0;
                }
                else if (tmpVal > 0.7001)
                {
                    data.likvidnost_opis_koeficient_za_zadolzenost = "Незадоволително";
                    data.finansiska_procenka_komentar = "Компанијата има висока задолженост.";
                    tmp_finansiskaVal1 = 1.0;
                }
                else if (tmpVal == 0)
                {
                    data.likvidnost_opis_koeficient_za_zadolzenost = "Незадоволително";
                    data.finansiska_procenka_komentar = "Компанијата има висока задолженост.";
                    tmp_finansiskaVal1 = 1.0;
                }
                data.short_komentar = data.finansiska_procenka_komentar;
            }
            else {

                data.likvidnost_opis_koeficient_za_zadolzenost = "Незадоволително";
                data.finansiska_procenka_komentar = "Компанијата има висока задолженост.";
                tmp_finansiskaVal1 = 1.0;
                data.short_komentar = data.finansiska_procenka_komentar;
            }
            //check
            if (data.likvidnost_brz_pokazatel.Equals("") == false && data.likvidnost_brz_pokazatel.Equals("/") == false)
            {
                var str = data.likvidnost_brz_pokazatel.Replace(",", "");
                tmpVal = double.Parse(str);

                if (tmpVal < 1)
                {
                    data.likvidnost_opis_brz_pokazatel = "Незадоволително";
                    data.finansiska_procenka_komentar += " Компанијата е неликвидна и не може да ги сервисира тековните обврски со тековните средства (без залихи).";
                    data.short_komentar += " Компанијата е неликвидна и не може да ги сервисира тековните обврски со тековните средства (без залихи).";
                    tmp_finansiskaVal2 = 1.0;
                }
                else if (tmpVal >= 1 && tmpVal <= 1.5)
                {
                    data.likvidnost_opis_brz_pokazatel = "Просечно";
                    data.finansiska_procenka_komentar += " Компанијата има просечна ликвидност и е во состојба да ги сервисира тековните обврски со тековните средства (без залихи) без сериозни застои.";
                    data.short_komentar += " Компанијата има просечна ликвидност и е во состојба да ги сервисира тековните обврски со тековните средства (без залихи) без сериозни застои.";
                    tmp_finansiskaVal2 = 2.0;
                }
                else if (tmpVal > 1.5)
                {
                    data.likvidnost_opis_brz_pokazatel = "Задоволително";
                    data.finansiska_procenka_komentar += " Компанијата е ликвидна и може да ги сервисира тековните обврски со тековните средства (без залихи).";
                    data.short_komentar += " Компанијата е ликвидна и може да ги сервисира тековните обврски со тековните средства (без залихи).";
                    tmp_finansiskaVal2 = 3.0;
                }

            }
            else {

                data.likvidnost_opis_brz_pokazatel = "Незадоволително";
                data.finansiska_procenka_komentar += " Компанијата е неликвидна и не може да ги сервисира тековните обврски со тековните средства (без залихи).";
                data.short_komentar += " Компанијата е неликвидна и не може да ги сервисира тековните обврски со тековните средства (без залихи).";
                tmp_finansiskaVal2 = 1.0;
            }
            if (data.likvidnost_prosecni_denovi_na_plakanje_ovrski.Equals("") == false && data.likvidnost_prosecni_denovi_na_plakanje_ovrski.Equals("/") == false)
            {
                var str = data.likvidnost_prosecni_denovi_na_plakanje_ovrski.Replace(",", "");
                tmpVal = double.Parse(str);

                if (tmpVal > 0 && tmpVal <= 90)
                {
                    data.likvidnost_opis_prosecni_denovi = "Задоволително";
                    data.finansiska_procenka_komentar += " Просечните денови на плаќање се задоволителни на ниво на економија.";
                    tmp_finansiskaVal3 = 3.0;
                }
                else if (tmpVal > 90 && tmpVal <= 120)
                {
                    data.likvidnost_opis_prosecni_denovi = "Просечно";
                    data.finansiska_procenka_komentar += " Просечните денови на плаќање се во прифатливите граници на ниво на економија.";
                    tmp_finansiskaVal3 = 2.0;
                }
                else if (tmpVal > 120)
                {
                    data.likvidnost_opis_prosecni_denovi = "Незадоволително";
                    data.finansiska_procenka_komentar += " Просечните денови на плаќање се под прифатливите граници на ниво на економија.";
                    tmp_finansiskaVal3 = 1.0;
                }
                else if (tmpVal == 0)
                {
                    data.likvidnost_opis_prosecni_denovi = "Незадоволително";
                    data.finansiska_procenka_komentar += " Просечните денови на плаќање се под прифатливите граници на ниво на економија.";
                    tmp_finansiskaVal3 = 1.0;
                }
            }
            else {
                data.likvidnost_opis_prosecni_denovi = "Незадоволително";
                data.finansiska_procenka_komentar += " Просечните денови на плаќање се под прифатливите граници на ниво на економија.";
                tmp_finansiskaVal3 = 1.0;
            }
            if (data.likvidnost_kreditna_izlozenost_od_rabotenje.Equals("") == false && data.likvidnost_kreditna_izlozenost_od_rabotenje.Equals("/") == false)
            {

                var str = data.likvidnost_kreditna_izlozenost_od_rabotenje.Replace(",", "").Replace("%", "");
                tmpVal = double.Parse(str) / 100;

                if (tmpVal > 0 && tmpVal <= 0.8)
                {
                    data.likvidnost_opis_kreditna_izlozenost = "Задоволително";
                    data.finansiska_procenka_komentar += " Компанијата има ниска кредитна изложеност од работењето.";
                    tmp_finansiskaVal4 = 3.0;
                }
                else if (tmpVal > 0.8 && tmpVal <= 1.01)
                {
                    data.likvidnost_opis_kreditna_izlozenost = "Просечно";
                    data.finansiska_procenka_komentar += " Компанијата има просечна кредитна изложеност од работењето.";
                    tmp_finansiskaVal4 = 2.0;
                }
                else if (tmpVal > 1.01)
                {
                    data.likvidnost_opis_kreditna_izlozenost = "Незадоволително";
                    data.finansiska_procenka_komentar += " Компанијата има висока кредитна изложеност од работењето.";
                    tmp_finansiskaVal4 = 1.0;
                }
                else if (tmpVal == 0)
                {
                    data.likvidnost_opis_kreditna_izlozenost = "Незадоволително";
                    data.finansiska_procenka_komentar += " Компанијата има висока кредитна изложеност од работењето.";
                    tmp_finansiskaVal4 = 1.0;
                }

            }
            else
            {
                data.likvidnost_opis_kreditna_izlozenost = "Незадоволително";
                data.finansiska_procenka_komentar += " Компанијата има висока кредитна изложеност од работењето.";
                tmp_finansiskaVal4 = 1.0;
            }

            var tmp_finansiskaVal = (tmp_finansiskaVal1 + tmp_finansiskaVal2 + tmp_finansiskaVal3 + tmp_finansiskaVal4) / 4;

            if (tmp_finansiskaVal < 1.5)
                data.likvidnost_opis_main = "Незадоволително";
            else if (tmp_finansiskaVal >= 1.5 && tmp_finansiskaVal < 2.5)
                data.likvidnost_opis_main = "Просечно";
            else if (tmp_finansiskaVal >= 2.5)
                data.likvidnost_opis_main = "Задоволително";

            
            //tmp_finansiskaVal1 finansiska procena
            //tmp_finansiskaVal2 brz pokazatel
            //tmp_finansiskaVal3 prosecni denovi
            //tmp_finansiskaVal4 kreditna izlozenost

            data.likvidnost_opis_main = OpisNaLikvidnost(tmp_finansiskaVal1, tmp_finansiskaVal2, tmp_finansiskaVal3,tmp_finansiskaVal4);

           
            //check

            if (company_values.ContainsKey(63))
            {
                if (company_values[63] == 0.0)
                {
                    data.efikasnost_povrat_na_sredstva = "";
                }
                else
                {
                    var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                    var resVal = tmp / company_values[63];

                    resVal = resVal * 100;

                    data.efikasnost_povrat_na_sredstva = resVal.ToString("0.00") + "%";
                }
            }


            //check
            if (company_values.ContainsKey(202))
            {
                if (company_values[202] == 0.0)
                {
                    data.efikasnost_neto_profitna_marza = "/";
                }
                else
                {
                    var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                    var resVal = tmp / company_values[202];

                    resVal = resVal * 100;

                    data.efikasnost_neto_profitna_marza = resVal.ToString("0.00") + "%";
                }
            }

            data.efikasnost_opis_main = "";
            data.efikasnost_opis_povrat_na_sredstva = "Незадоволително";
            data.efikasnost_opis_profitna_marza = "Незадоволително";

            var tmp_efikasnostVal1 = 1.0;
            var tmp_efikasnostVal2 = 1.0;
            if (data.efikasnost_povrat_na_sredstva.Equals("") == false && data.efikasnost_povrat_na_sredstva.Equals("/") == false)
            {
                var str = data.efikasnost_povrat_na_sredstva.Replace(",", "").Replace("%", "");
                tmpVal = (double.Parse(str) / 100);

                if (tmpVal <= 0.06)
                {
                    data.efikasnost_opis_povrat_na_sredstva = "Незадоволително";
                    data.finansiska_procenka_komentar += "<br/><br/>Компанијата има незадоволителна стапка на поврат на средства.";
                    tmp_efikasnostVal1 = 1.0;
                }
                else if (tmpVal > 0.06 && tmpVal <= 0.0901)
                {
                    data.efikasnost_opis_povrat_na_sredstva = "Просечно";
                    data.finansiska_procenka_komentar += "<br/><br/>Компанијата има просечна стапка на поврат на средства.";
                    tmp_efikasnostVal1 = 2.0;
                }
                else if (tmpVal > 0.0901)
                {
                    data.efikasnost_opis_povrat_na_sredstva = "Задоволително";
                    data.finansiska_procenka_komentar += "<br/><br/>Компанијата има задоволителна стапка на поврат на средства.";
                    tmp_efikasnostVal1 = 3.0;
                }

            }
            else {

                data.efikasnost_opis_povrat_na_sredstva = "Незадоволително";
                data.finansiska_procenka_komentar += "<br/><br/>Компанијата има незадоволителна стапка на поврат на средства.";
                tmp_efikasnostVal1 = 1.0;
            }
            if (data.efikasnost_neto_profitna_marza.Equals("") == false && data.efikasnost_neto_profitna_marza.Equals("/") == false)
            {
                var str = data.efikasnost_neto_profitna_marza.Replace(",", "").Replace("%", "");
                tmpVal = double.Parse(str) / 100;

                if (tmpVal <= 0.03501)
                {
                    data.efikasnost_opis_profitna_marza = "Незадоволително";
                    data.finansiska_procenka_komentar += "<br/><br/>Компанијата работи со многу ниска нето профитна маржа.";
                    tmp_efikasnostVal2 = 1.0;
                }
                else if (tmpVal > 0.03501 && tmpVal < 0.0601)
                {
                    data.efikasnost_opis_profitna_marza = "Просечно";
                    data.finansiska_procenka_komentar += "<br/><br/>Компанијата работи со просечна стапка на нето профитна маржа.";
                    tmp_efikasnostVal2 = 2.0;
                }
                else if (tmpVal > 0.0600)
                {
                    data.efikasnost_opis_profitna_marza = "Задоволително";
                    data.finansiska_procenka_komentar += "<br/><br/>Компанијата работи со висока нето профитна маржа.";
                    tmp_efikasnostVal2 = 3.0;
                }
            }
            else {

                data.efikasnost_opis_profitna_marza = "Незадоволително";
                data.finansiska_procenka_komentar += "<br/><br/>Компанијата работи со многу ниска нето профитна маржа.";
                tmp_efikasnostVal2 = 1.0;
            }

            var tmp_efikasnostVal = (tmp_efikasnostVal1 + tmp_efikasnostVal2) / 2;

            data.efikasnost_opis_main = "Незадоволително";
            if(tmp_efikasnostVal >= 1 && tmp_efikasnostVal < 1.5)
                data.efikasnost_opis_main = "Незадоволително";
            else if (tmp_efikasnostVal >= 1.5 && tmp_efikasnostVal < 2.5)
                data.efikasnost_opis_main = "Просечно";
            else if (tmp_efikasnostVal >= 2.5)
                data.efikasnost_opis_main = "Задоволително";

            //zadovolitelno - ROA
            if (tmp_efikasnostVal1 == 3.0) { 
                //zadovolitelno
                if (tmp_efikasnostVal2 == 3.0) {
                    data.efikasnost_opis_main = "Задоволително";
                }
                //zadovolitelno
                if (tmp_efikasnostVal2 == 2.0)
                {
                    data.efikasnost_opis_main = "Задоволително";
                }
                //nezadovolitelno
                if (tmp_efikasnostVal2 == 1.0)
                {
                    data.efikasnost_opis_main = "Просечно";
                }
            }
            //prosecno - ROA
            else if (tmp_efikasnostVal1 == 2.0)
            {
                //prosecno
                if (tmp_efikasnostVal2 == 3.0)
                {
                    data.efikasnost_opis_main = "Просечно";
                }
                //prosecno
                if (tmp_efikasnostVal2 == 2.0)
                {
                    data.efikasnost_opis_main = "Просечно";
                }
                //nezadovolitelno
                if (tmp_efikasnostVal2 == 1.0)
                {
                    data.efikasnost_opis_main = "Незадоволително";
                }

            }
            //nezadovolitelno - ROA
            else if (tmp_efikasnostVal1 == 1.0) {

                //nezadovolitelno
                if (tmp_efikasnostVal2 == 1.0)
                {
                    data.efikasnost_opis_main = "Незадоволително";
                }
                //nezadovolitelno
                if (tmp_efikasnostVal2 == 3.0)
                {
                    data.efikasnost_opis_main = "Незадоволително";
                }
                //nezadovolitelno
                if (tmp_efikasnostVal2 == 2.0)
                {
                    data.efikasnost_opis_main = "Незадоволително";
                }
            }

            if (data.efikasnost_opis_main == "Незадоволително")
                data.short_komentar += " Компанијата има незадоволителна ефикасност.";
            if (data.efikasnost_opis_main == "Просечно")
                data.short_komentar += " Компанијата има просечна ефикасност.";
            if (data.efikasnost_opis_main == "Задоволително")
                data.short_komentar += " Компанијата има задоволителна ефикасност.";

            data.profitabilnost_bruto_operativna_dobivka1 = SubtractValues(company_values, new int[] { 202, 208, 209, 210, 211 }).ToString("0.00");
            data.profitabilnost_bruto_operativna_dobivka1 = FormatCurrencyComma(data.profitabilnost_bruto_operativna_dobivka1);
            
            //check

            var tmpRes1 = SubtractValues(company_values, new int[] { 223, 234 });
            data.profitabilnost_neto_dobivka_od_finansiranje1 = "0";
            //if (tmpRes1 > 0)
            //{

                data.profitabilnost_neto_dobivka_od_finansiranje1 = tmpRes1.ToString("0.00");
                data.profitabilnost_neto_dobivka_od_finansiranje1 = FormatCurrencyComma(data.profitabilnost_neto_dobivka_od_finansiranje1);
            //}

                data.profitabilnost_ebitda1 = Gj_EBITDA;
           data.profitabilnost_ebitda1 = FormatCurrencyComma(data.profitabilnost_ebitda1);


                data.profitabilnost_ebit1 = E_EBIT;
           data.profitabilnost_ebit1 = FormatCurrencyComma(data.profitabilnost_ebit1);


            if (company_values.ContainsKey(202) && company_values.ContainsKey(95))
            {
                var tmp = company_values[95] / company_values[202];

                tmp = tmp * 100;

                data.indikatori_tekovni_obvrski_2013 = tmp.ToString("0.00") + "%";
            }


            if (company_values.ContainsKey(202))
            {
                var tmp = SumValues(company_values, new int[] { 103, 104 });

                var resVal = tmp / company_values[202];

                resVal = resVal * 100;

                data.indikatori_kratkorocni_krediti_2013 = resVal.ToString("0.00") + "%";
            }


            data.indikatori_koeficient_zadolzenost_2013 = DevideValues(SumValues(company_values, new int[] { 82, 85, 95 }), company_values, new int[] { 63 });

            data.indikatori_finansiski_leviridz_2013 = DevideValues(SumValues(company_values, new int[] { 85, 95, 82 }), company_values, new int[] { 65 });

            if (company_values.ContainsKey(202))
            {
                var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                var resVal = tmp / company_values[202];

                resVal = resVal * 100;

                data.indikatori_neto_profitna_margina_2013 = resVal.ToString("0.00") + "%";
            }

            if (company_values.ContainsKey(63))
            {
                var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                var resVal = tmp / company_values[63];

                resVal = resVal * 100;

                data.indikatori_povrat_sredstva_2013 = resVal.ToString("0.00") + "%";
            }

            if (company_values.ContainsKey(65))
            {
                var tmp = SubtractValues(company_values, new int[] { 255, 256 });

                var resVal = tmp / company_values[65];

                resVal = resVal * 100;

                data.indikatori_povrat_kapital_2013 = resVal.ToString("0.00") + "%";
            }

            data.indikatori_brz_pokazatel_2013 = DevideValues(SubtractValues(company_values, new int[] { 36, 37 }), company_values, new int[] { 95 });

            data.indikatori_tekoven_pokazatel_2013 = DevideValues(company_values, new int[] { 36, 95 });

            data.indikatori_raboten_kapital_2013 = SubtractValues(company_values, new int[] { 36, 95 }).ToString("0.00");
            data.indikatori_raboten_kapital_2013 = FormatCurrencyComma(data.indikatori_raboten_kapital_2013);

            if (company_values.ContainsKey(256))
            {
                data.uspeh_neto_zaguba_2013 = company_values[256].ToString();
                data.uspeh_neto_zaguba_2013 = FormatCurrencyComma(data.uspeh_neto_zaguba_2013);
            }

            //check
            if (company_values.ContainsKey(255))
            {
                data.uspeh_neto_dobivka_2013 = company_values[255].ToString();
                data.uspeh_neto_dobivka_2013 = FormatCurrencyComma(data.uspeh_neto_dobivka_2013);
            }

            if (company_values.ContainsKey(252))
            {
                data.uspeh_danok_dobivka_2013 = company_values[252].ToString();
                data.uspeh_danok_dobivka_2013 = FormatCurrencyComma(data.uspeh_danok_dobivka_2013);
            }

            if (company_values.ContainsKey(251))
            {

                data.uspeh_zaguba_odanocuvanje_2013 = company_values[251].ToString();
                data.uspeh_zaguba_odanocuvanje_2013 = FormatCurrencyComma(data.uspeh_zaguba_odanocuvanje_2013);
            }

            if (company_values.ContainsKey(250))
            {
                data.uspeh_dobivka_odanocuvanje_2013 = company_values[250].ToString();
                data.uspeh_dobivka_odanocuvanje_2013 = FormatCurrencyComma(data.uspeh_dobivka_odanocuvanje_2013);
            }

            if (company_values.ContainsKey(245))
            {
                data.uspeh_udel_vo_zaguba_2013 = company_values[245].ToString();
                data.uspeh_udel_vo_zaguba_2013 = FormatCurrencyComma(data.uspeh_udel_vo_zaguba_2013);
            }

            if (company_values.ContainsKey(244))
            {
                data.uspeh_udel_vo_dobivka_2013 = company_values[244].ToString();
                data.uspeh_udel_vo_dobivka_2013 = FormatCurrencyComma(data.uspeh_udel_vo_dobivka_2013);
            }

            //data.uspeh_ostanati_finansiski_rashodi_2013 = SumValues(company_values, new int[] { 241, 242 }).ToString();
            //data.uspeh_ostanati_finansiski_rashodi_2013 = FormatCurrencyComma(data.uspeh_ostanati_finansiski_rashodi_2013);


            if (company_values.ContainsKey(243))
            {
                data.uspeh_ostanati_finansiski_rashodi_2013 = company_values[243].ToString();
                data.uspeh_ostanati_finansiski_rashodi_2013 = FormatCurrencyComma(data.uspeh_ostanati_finansiski_rashodi_2013);
            }

            data.uspeh_rashodi_kamati_2013 = SumValues(company_values, new int[] { 239, 240 }).ToString();
            data.uspeh_rashodi_kamati_2013 = FormatCurrencyComma(data.uspeh_rashodi_kamati_2013);

            if (company_values.ContainsKey(235))
            {
                data.uspeh_finansiski_povrzani_drustva_2013 = company_values[235].ToString();
                data.uspeh_finansiski_povrzani_drustva_2013 = FormatCurrencyComma(data.uspeh_finansiski_povrzani_drustva_2013);
            }

            if (company_values.ContainsKey(234))
            {
                data.uspeh_finansiski_rashodi_2013 = company_values[234].ToString();
                data.uspeh_finansiski_rashodi_2013 = FormatCurrencyComma(data.uspeh_finansiski_rashodi_2013);
            }

            if (company_values.ContainsKey(222))
            {
                data.uspeh_ostanati_rashodi_2013 = company_values[222].ToString();
                data.uspeh_ostanati_rashodi_2013 = FormatCurrencyComma(data.uspeh_ostanati_rashodi_2013);
            }

            if (company_values.ContainsKey(221))
            {
                data.uspeh_rezerviranje_trosoci_rizici_2013 = company_values[221].ToString();
                data.uspeh_rezerviranje_trosoci_rizici_2013 = FormatCurrencyComma(data.uspeh_rezerviranje_trosoci_rizici_2013);
            }

            data.uspeh_amortizacija_sredstva_2013 = SumValues(company_values, new int[] { 218, 219, 220 }).ToString();
            data.uspeh_amortizacija_sredstva_2013 = FormatCurrencyComma(data.uspeh_amortizacija_sredstva_2013);
            

            if (company_values.ContainsKey(212))
            {
                data.uspeh_ostanati_trosoci_2013 = company_values[212].ToString();
                data.uspeh_ostanati_trosoci_2013 = FormatCurrencyComma(data.uspeh_ostanati_trosoci_2013);
            }

            if (company_values.ContainsKey(204))
            {
                data.uspeh_zalihi_proizvodi_pocetok_2013 = company_values[204].ToString();
                data.uspeh_zalihi_proizvodi_pocetok_2013 = FormatCurrencyComma(data.uspeh_zalihi_proizvodi_pocetok_2013);
            }

            if (company_values.ContainsKey(205))
            {
                data.uspeh_zalihi_proizvodi_kraj_2013 = company_values[205].ToString();
                data.uspeh_zalihi_proizvodi_kraj_2013 = FormatCurrencyComma(data.uspeh_zalihi_proizvodi_kraj_2013);
            }

            data.uspeh_rashodi_rabotenje_2013 = CalculateValues(company_values, new int[] { 207, 204, 205 }, new string[] { "+", "-" }).ToString();
            data.uspeh_rashodi_rabotenje_2013 = FormatCurrencyComma(data.uspeh_rashodi_rabotenje_2013);

            data.uspeh_vkupno_rashodi_2013 = (SumValues(company_values, new int[] { 207, 234, 245 }) + SubtractValues(company_values, new int[] { 204, 205})).ToString();
            data.uspeh_vkupno_rashodi_2013 = FormatCurrencyComma(data.uspeh_vkupno_rashodi_2013);

            if (company_values.ContainsKey(223))
            {
                data.uspeh_finansiski_prihodi_2013 = company_values[223].ToString();
                data.uspeh_finansiski_prihodi_2013 = FormatCurrencyComma(data.uspeh_finansiski_prihodi_2013);
            }

            if (company_values.ContainsKey(201))
            {
                data.uspeh_prihodi_rabotenje_2013 = company_values[201].ToString();
                data.uspeh_prihodi_rabotenje_2013 = FormatCurrencyComma(data.uspeh_prihodi_rabotenje_2013);
            }

            if (company_values.ContainsKey(111))
            {
                data.bilans_vkupna_pasiva_2013 = company_values[111].ToString();
                data.bilans_vkupna_pasiva_2013 = FormatCurrencyComma(data.bilans_vkupna_pasiva_2013);
            }

            if (company_values.ContainsKey(110))
            {
                data.bilans_obvrski_po_osnov_2013 = company_values[110].ToString();
                data.bilans_obvrski_po_osnov_2013 = FormatCurrencyComma(data.bilans_obvrski_po_osnov_2013);

            }

            if (company_values.ContainsKey(109))
            {
                data.bilans_odlozeno_plakanje_2013 = company_values[109].ToString();
                data.bilans_odlozeno_plakanje_2013 = FormatCurrencyComma(data.bilans_odlozeno_plakanje_2013);
            }

            if (company_values.ContainsKey(94))
            {
                data.bilans_odlozeni_obvrski_2013 = company_values[94].ToString();
                data.bilans_odlozeni_obvrski_2013 = FormatCurrencyComma(data.bilans_odlozeni_obvrski_2013);
            }

            if (company_values.ContainsKey(95))
            {
                data.bilans_kratkorocni_obvrski_2013 = company_values[95].ToString();
                data.bilans_kratkorocni_obvrski_2013 = FormatCurrencyComma(data.bilans_kratkorocni_obvrski_2013);
            }

            if (company_values.ContainsKey(85))
            {
                data.bilans_dolgorocni_obvrski_2013 = company_values[85].ToString();
                data.bilans_dolgorocni_obvrski_2013 = FormatCurrencyComma(data.bilans_dolgorocni_obvrski_2013);
            }

            if (company_values.ContainsKey(82))
            {
                data.bilans_dolgorocni_rezerviranja_2013 = company_values[82].ToString();
                data.bilans_dolgorocni_rezerviranja_2013 = FormatCurrencyComma(data.bilans_dolgorocni_rezerviranja_2013);
            }

            if (company_values.ContainsKey(81))
            {
                data.bilans_obvrski_2013 = company_values[81].ToString();
                data.bilans_obvrski_2013 = FormatCurrencyComma(data.bilans_obvrski_2013);
            }

            if (company_values.ContainsKey(77))
            {
                data.bilans_dobivka_delovna_godina_2013 = company_values[77].ToString();
                data.bilans_dobivka_delovna_godina_2013 = FormatCurrencyComma(data.bilans_dobivka_delovna_godina_2013);
            }

            if (company_values.ContainsKey(76))
            {
                data.bilans_prenesena_zaguba_2013 = company_values[76].ToString();
                data.bilans_prenesena_zaguba_2013 = FormatCurrencyComma(data.bilans_prenesena_zaguba_2013);
            }

            if (company_values.ContainsKey(75))
            {
                data.bilans_akumulirana_dobivka_2013 = company_values[75].ToString();
                data.bilans_akumulirana_dobivka_2013 = FormatCurrencyComma(data.bilans_akumulirana_dobivka_2013);
            }

            if (company_values.ContainsKey(71))
            {
                data.bilans_rezervi_2013 = company_values[71].ToString();
                data.bilans_rezervi_2013 = FormatCurrencyComma(data.bilans_rezervi_2013);
            }

            if (company_values.ContainsKey(70))
            {
                data.bilans_revalorizaciska_rezerva_2013 = company_values[70].ToString();
                data.bilans_revalorizaciska_rezerva_2013 = FormatCurrencyComma(data.bilans_revalorizaciska_rezerva_2013);
            }

            if (company_values.ContainsKey(69))
            {
                data.bilans_zapisan_kapital_2013 = company_values[69].ToString();
                data.bilans_zapisan_kapital_2013 = FormatCurrencyComma(data.bilans_zapisan_kapital_2013);

            }

            if (company_values.ContainsKey(78))
            {
                data.bilans_zaguba_delovna_godina_2013 = company_values[78].ToString();
                data.bilans_zaguba_delovna_godina_2013 = FormatCurrencyComma(data.bilans_zaguba_delovna_godina_2013);
            }

            if (company_values.ContainsKey(68))
            {
                data.bilans_sopstveni_akcii_2013 = company_values[68].ToString();
                data.bilans_sopstveni_akcii_2013 = FormatCurrencyComma(data.bilans_sopstveni_akcii_2013);
            }

            //check
            if (company_values.ContainsKey(67))
            {
                data.bilans_premii_akcii_2013 = company_values[67].ToString();
                data.bilans_premii_akcii_2013 = FormatCurrencyComma(data.bilans_premii_akcii_2013);
            }

            if (company_values.ContainsKey(66))
            {
                data.bilans_osnovna_glavnina_2013 = company_values[66].ToString();
                data.bilans_osnovna_glavnina_2013 = FormatCurrencyComma(data.bilans_osnovna_glavnina_2013);
            }

            if (company_values.ContainsKey(65))
            {
                data.bilans_glavnina_i_rezervi_2013 = company_values[65].ToString();
                data.bilans_glavnina_i_rezervi_2013 = FormatCurrencyComma(data.bilans_glavnina_i_rezervi_2013);

            }

            if (company_values.ContainsKey(63))
            {
                data.bilans_vkupna_aktiva_2013 = company_values[63].ToString();
                data.bilans_vkupna_aktiva_2013 = FormatCurrencyComma(data.bilans_vkupna_aktiva_2013);
            }

            if (company_values.ContainsKey(44))
            {
                data.bilans_sredstva_grupa_2013 = company_values[44].ToString();
                data.bilans_sredstva_grupa_2013 = FormatCurrencyComma(data.bilans_sredstva_grupa_2013);
            }

            if (company_values.ContainsKey(59))
            {
                data.bilans_paricni_sredstva_2013 = company_values[59].ToString();
                data.bilans_paricni_sredstva_2013 = FormatCurrencyComma(data.bilans_paricni_sredstva_2013);
            }

            //check
            if (company_values.ContainsKey(52))
            {
                data.bilans_kratkorocni_sredstva_2013 = company_values[52].ToString();
                data.bilans_kratkorocni_sredstva_2013 = FormatCurrencyComma(data.bilans_kratkorocni_sredstva_2013);
            }

            if (company_values.ContainsKey(45))
            {
                data.bilans_kratkorocni_pobaruvanja_2013 = company_values[45].ToString();
                data.bilans_kratkorocni_pobaruvanja_2013 = FormatCurrencyComma(data.bilans_kratkorocni_pobaruvanja_2013);

            }

            if (company_values.ContainsKey(37))
            {
                data.bilans_zalihi_2013 = company_values[37].ToString();
                data.bilans_zalihi_2013 = FormatCurrencyComma(data.bilans_zalihi_2013);
            }

            if (company_values.ContainsKey(36))
            {
                data.bilans_tekovni_sredstva_2013 = company_values[36].ToString();
                data.bilans_tekovni_sredstva_2013 = FormatCurrencyComma(data.bilans_tekovni_sredstva_2013);
            }

            //check
            if (company_values.ContainsKey(35))
            {
                data.bilans_odlozeni_danocni_sredstva_2013 = company_values[35].ToString();

                data.bilans_odlozeni_danocni_sredstva_2013 = FormatCurrencyComma(data.bilans_odlozeni_danocni_sredstva_2013);
            }

            //check
            if (company_values.ContainsKey(31))
            {
                data.bilans_dolgorocni_pobaruvanja_2013 = company_values[31].ToString();
                data.bilans_dolgorocni_pobaruvanja_2013 = FormatCurrencyComma(data.bilans_dolgorocni_pobaruvanja_2013);
            }

            //check
            if (company_values.ContainsKey(21))
            {
                data.bilans_dolgorocni_sredstva_2013 = company_values[21].ToString();
                data.bilans_dolgorocni_sredstva_2013 = FormatCurrencyComma(data.bilans_dolgorocni_sredstva_2013);
            }

            //check
            if (company_values.ContainsKey(20))
            {
                data.bilans_vlozuvanje_nedviznosti_2013 = company_values[20].ToString();
                data.bilans_vlozuvanje_nedviznosti_2013 = FormatCurrencyComma(data.bilans_vlozuvanje_nedviznosti_2013);
            }

            if (company_values.ContainsKey(9))
            {
                data.bilans_materijalni_2013 = company_values[9].ToString();
                data.bilans_materijalni_2013 = FormatCurrencyComma(data.bilans_materijalni_2013);
            }

            //check
            if (company_values.ContainsKey(2))
            {
                data.bilans_nematerijalni_2013 = company_values[2].ToString();
                data.bilans_nematerijalni_2013 = FormatCurrencyComma(data.bilans_nematerijalni_2013);
            }

            if (company_values.ContainsKey(1))
            {
                data.bilans_netekovni_2013 = company_values[1].ToString();
                data.bilans_netekovni_2013 = FormatCurrencyComma(data.bilans_netekovni_2013);
            }

            if (company_values.ContainsKey(62))
            {
                data.bilans_plateni_trosoci_2013 = company_values[62].ToString();
                data.bilans_plateni_trosoci_2013 = FormatCurrencyComma(data.bilans_plateni_trosoci_2013);
            }

            if (company_values.ContainsKey(213))
            {
                data.uspeh_trosoci_za_vraboteni_2013 = company_values[213].ToString();

                data.uspeh_trosoci_za_vraboteni_2013 = FormatCurrencyComma(data.uspeh_trosoci_za_vraboteni_2013);
            }



            data.kategorija2 = Year.ToString();

            data.profitabilnost_stavka1 = Year.ToString();

            if (COE_Error == false)
            {
                data.organizacioni_edinici = company_organizacioni_edinici;
            }

            data.profitabilnost_stavka2 = LastYear.ToString();

            if (blokadaRes != null)
            {
                data.blokadi_datum1 = FormatStringDate(blokadaRes["TimeStamp"]);
                data.blokadi_opis1 = blokadaRes["AccInfo"];
            }
            data.bilans_netekovni_sredstva = "A";

            data.bilans_netekovni_ind = GetIndikatorInd(data.bilans_netekovni_2013, data.bilans_netekovni_2012);
            data.bilans_netekovni_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_netekovni_2013);
            data.bilans_netekovni_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_netekovni_2012);

            data.bilans_nematerijalni_sredstva = "1";

            data.bilans_nematerijalni_ind = GetIndikatorInd(data.bilans_nematerijalni_2013, data.bilans_nematerijalni_2012);
            data.bilans_nematerijalni_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_nematerijalni_2013);
            data.bilans_nematerijalni_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_nematerijalni_2012);

            data.bilans_materijalni_sredstva = "2";

            data.bilans_materijalni_ind = GetIndikatorInd(data.bilans_materijalni_2013, data.bilans_materijalni_2012);
            data.bilans_materijalni_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_materijalni_2013);
            data.bilans_materijalni_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_materijalni_2012);

            data.bilans_vlozuvanje_nedviznosti_sredstva = "3";


            data.bilans_vlozuvanje_nedviznosti_ind = GetIndikatorInd(data.bilans_vlozuvanje_nedviznosti_2013, data.bilans_vlozuvanje_nedviznosti_2012);
            data.bilans_vlozuvanje_nedviznosti_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_vlozuvanje_nedviznosti_2013);
            data.bilans_vlozuvanje_nedviznosti_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_vlozuvanje_nedviznosti_2012);

            data.bilans_dolgorocni_sredstva_sredstva = "4";


            data.bilans_dolgorocni_sredstva_ind = GetIndikatorInd(data.bilans_dolgorocni_sredstva_2013, data.bilans_dolgorocni_sredstva_2012);
            data.bilans_dolgorocni_sredstva_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_dolgorocni_sredstva_2013);
            data.bilans_dolgorocni_sredstva_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_dolgorocni_sredstva_2012);

            data.bilans_dolgorocni_pobaruvanja_sredstva = "5";


            data.bilans_dolgorocni_pobaruvanja_ind = GetIndikatorInd(data.bilans_dolgorocni_pobaruvanja_2013, data.bilans_dolgorocni_pobaruvanja_2012);
            data.bilans_dolgorocni_pobaruvanja_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_dolgorocni_pobaruvanja_2013);
            data.bilans_dolgorocni_pobaruvanja_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_dolgorocni_pobaruvanja_2012);

            data.bilans_odlozeni_danocni_sredstva_sredstva = "B";


            data.bilans_odlozeni_danocni_sredstva_ind = GetIndikatorInd(data.bilans_odlozeni_danocni_sredstva_2013, data.bilans_odlozeni_danocni_sredstva_2012);
            data.bilans_odlozeni_danocni_sredstva_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_odlozeni_danocni_sredstva_2013);
            data.bilans_odlozeni_danocni_sredstva_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_odlozeni_danocni_sredstva_2012);

            data.bilans_tekovni_sredstva_sredstva = "C";

            data.bilans_tekovni_sredstva_ind = GetIndikatorInd(data.bilans_tekovni_sredstva_2013, data.bilans_tekovni_sredstva_2012);
            data.bilans_tekovni_sredstva_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_tekovni_sredstva_2013);
            data.bilans_tekovni_sredstva_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_tekovni_sredstva_2012);

            data.bilans_zalihi_sredstva = "1";

            data.bilans_zalihi_ind = GetIndikatorInd(data.bilans_zalihi_2013, data.bilans_zalihi_2012);
            data.bilans_zalihi_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_zalihi_2013);
            data.bilans_zalihi_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_zalihi_2012);

            data.bilans_kratkorocni_pobaruvanja_sredstva = "2";

            data.bilans_kratkorocni_pobaruvanja_ind = GetIndikatorInd(data.bilans_kratkorocni_pobaruvanja_2013, data.bilans_kratkorocni_pobaruvanja_2012);
            data.bilans_kratkorocni_pobaruvanja_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_kratkorocni_pobaruvanja_2013);
            data.bilans_kratkorocni_pobaruvanja_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_kratkorocni_pobaruvanja_2012);

            data.bilans_kratkorocni_sredstva_sredstva = "3";
            
            data.bilans_kratkorocni_sredstva_ind = GetIndikatorInd(data.bilans_kratkorocni_sredstva_2013, data.bilans_kratkorocni_sredstva_2012);
            data.bilans_kratkorocni_sredstva_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_kratkorocni_sredstva_2013);
            data.bilans_kratkorocni_sredstva_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_kratkorocni_sredstva_2012);

            data.bilans_paricni_sredstva_sredstva = "4";


            data.bilans_paricni_sredstva_ind = GetIndikatorInd(data.bilans_paricni_sredstva_2013, data.bilans_paricni_sredstva_2012);
            data.bilans_paricni_sredstva_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_paricni_sredstva_2013);
            data.bilans_paricni_sredstva_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_paricni_sredstva_2012);

            data.bilans_sredstva_grupa_sredstva = "D";

            data.bilans_sredstva_grupa_ind = GetIndikatorInd(data.bilans_sredstva_grupa_2013, data.bilans_sredstva_grupa_2012);
            data.bilans_sredstva_grupa_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_sredstva_grupa_2013);
            data.bilans_sredstva_grupa_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_sredstva_grupa_2012);

            data.bilans_plateni_trosoci_sredstva = "E";

            data.bilans_plateni_trosoci_ind = GetIndikatorInd(data.bilans_plateni_trosoci_2013, data.bilans_plateni_trosoci_2012);
            data.bilans_plateni_trosoci_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2013, data.bilans_plateni_trosoci_2013);
            data.bilans_plateni_trosoci_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_aktiva_2012, data.bilans_plateni_trosoci_2012);

            data.bilans_vkupna_aktiva_sredstva = "";

            data.bilans_vkupna_aktiva_ind = GetIndikatorInd(data.bilans_vkupna_aktiva_2013, data.bilans_vkupna_aktiva_2012);

            if (CheckIfValueAvailable(data.bilans_netekovni_2013_procent) ||
                CheckIfValueAvailable(data.bilans_nematerijalni_2013_procent) ||
                CheckIfValueAvailable(data.bilans_materijalni_2013_procent) ||
                CheckIfValueAvailable(data.bilans_vlozuvanje_nedviznosti_2013_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_pobaruvanja_2013_procent) ||
                CheckIfValueAvailable(data.bilans_odlozeni_danocni_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.bilans_tekovni_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.bilans_zalihi_2013_procent) ||
                CheckIfValueAvailable(data.bilans_kratkorocni_pobaruvanja_2013_procent) ||
                CheckIfValueAvailable(data.bilans_kratkorocni_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.bilans_paricni_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.bilans_sredstva_grupa_2013_procent) ||
                CheckIfValueAvailable(data.bilans_plateni_trosoci_2013_procent))
            {
                data.bilans_vkupna_aktiva_2013_procent = "100.0";
            }
            else {
                data.bilans_vkupna_aktiva_2013_procent = "/";
            }

            if (CheckIfValueAvailable(data.bilans_netekovni_2012_procent) ||
                CheckIfValueAvailable(data.bilans_nematerijalni_2012_procent) ||
                CheckIfValueAvailable(data.bilans_materijalni_2012_procent) ||
                CheckIfValueAvailable(data.bilans_vlozuvanje_nedviznosti_2012_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_pobaruvanja_2012_procent) ||
                CheckIfValueAvailable(data.bilans_odlozeni_danocni_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.bilans_tekovni_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.bilans_zalihi_2012_procent) ||
                CheckIfValueAvailable(data.bilans_kratkorocni_pobaruvanja_2012_procent) ||
                CheckIfValueAvailable(data.bilans_kratkorocni_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.bilans_paricni_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.bilans_sredstva_grupa_2012_procent) ||
                CheckIfValueAvailable(data.bilans_plateni_trosoci_2012_procent))
            {
                data.bilans_vkupna_aktiva_2012_procent = "100.0";
            }
            else
            {
                data.bilans_vkupna_aktiva_2012_procent = "/";
            }
            
            data.bilans_glavnina_i_rezervi_sredstva = "A";

            data.bilans_glavnina_i_rezervi_ind = GetIndikatorInd(data.bilans_glavnina_i_rezervi_2013, data.bilans_glavnina_i_rezervi_2012);
            data.bilans_glavnina_i_rezervi_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_glavnina_i_rezervi_2013);
            data.bilans_glavnina_i_rezervi_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_glavnina_i_rezervi_2012);

            data.bilans_osnovna_glavnina_sredstva = "1";

            data.bilans_osnovna_glavnina_ind = GetIndikatorInd(data.bilans_osnovna_glavnina_2013, data.bilans_osnovna_glavnina_2012);
            data.bilans_osnovna_glavnina_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_osnovna_glavnina_2013);
            data.bilans_osnovna_glavnina_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_osnovna_glavnina_2012);

            data.bilans_premii_akcii_sredstva = "2";


            data.bilans_premii_akcii_ind = GetIndikatorInd(data.bilans_premii_akcii_2013, data.bilans_premii_akcii_2012);
            data.bilans_premii_akcii_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_premii_akcii_2013);
            data.bilans_premii_akcii_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_premii_akcii_2012);

            data.bilans_sopstveni_akcii_sredstva = "3";


            data.bilans_sopstveni_akcii_ind = GetIndikatorInd(data.bilans_sopstveni_akcii_2013, data.bilans_sopstveni_akcii_2012);
            data.bilans_sopstveni_akcii_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_sopstveni_akcii_2013);
            data.bilans_sopstveni_akcii_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_sopstveni_akcii_2012);

            data.bilans_zapisan_kapital_sredstva = "4";


            data.bilans_zapisan_kapital_ind = GetIndikatorInd(data.bilans_zapisan_kapital_2013, data.bilans_zapisan_kapital_2012);
            data.bilans_zapisan_kapital_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_zapisan_kapital_2013);
            data.bilans_zapisan_kapital_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_zapisan_kapital_2012);

            data.bilans_revalorizaciska_rezerva_sredstva = "5";
            
            data.bilans_revalorizaciska_rezerva_ind = GetIndikatorInd(data.bilans_revalorizaciska_rezerva_2013, data.bilans_revalorizaciska_rezerva_2012);
            data.bilans_revalorizaciska_rezerva_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_revalorizaciska_rezerva_2013);
            data.bilans_revalorizaciska_rezerva_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_revalorizaciska_rezerva_2012);

            data.bilans_rezervi_sredstva = "6";


            data.bilans_rezervi_ind = GetIndikatorInd(data.bilans_rezervi_2013, data.bilans_rezervi_2012);
            data.bilans_rezervi_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_rezervi_2013);
            data.bilans_rezervi_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_rezervi_2012);

            data.bilans_akumulirana_dobivka_sredstva = "7";


            data.bilans_akumulirana_dobivka_ind = GetIndikatorInd(data.bilans_akumulirana_dobivka_2013, data.bilans_akumulirana_dobivka_2012);
            data.bilans_akumulirana_dobivka_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_akumulirana_dobivka_2013);
            data.bilans_akumulirana_dobivka_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_akumulirana_dobivka_2012);

            data.bilans_prenesena_zaguba_sredstva = "8";


            data.bilans_prenesena_zaguba_ind = GetIndikatorInd(data.bilans_prenesena_zaguba_2013, data.bilans_prenesena_zaguba_2012);
            data.bilans_prenesena_zaguba_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_prenesena_zaguba_2013);
            data.bilans_prenesena_zaguba_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_prenesena_zaguba_2012);

            data.bilans_dobivka_delovna_godina_sredstva = "9";


            data.bilans_dobivka_delovna_godina_ind = GetIndikatorInd(data.bilans_dobivka_delovna_godina_2013, data.bilans_dobivka_delovna_godina_2012);
            data.bilans_dobivka_delovna_godina_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_dobivka_delovna_godina_2013);
            data.bilans_dobivka_delovna_godina_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_dobivka_delovna_godina_2012);

            data.bilans_zaguba_delovna_godina_sredstva = "10";

            data.bilans_zaguba_delovna_godina_ind = GetIndikatorInd(data.bilans_zaguba_delovna_godina_2013, data.bilans_zaguba_delovna_godina_2012);
            data.bilans_zaguba_delovna_godina_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_zaguba_delovna_godina_2013);
            data.bilans_zaguba_delovna_godina_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_zaguba_delovna_godina_2012);

            data.bilans_obvrski_sredstva = "B";


            data.bilans_obvrski_ind = GetIndikatorInd(data.bilans_obvrski_2013, data.bilans_obvrski_2012);
            data.bilans_obvrski_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_obvrski_2013);
            data.bilans_obvrski_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_obvrski_2012);

            data.bilans_dolgorocni_rezerviranja_sredstva = "1";



            data.bilans_dolgorocni_rezerviranja_ind = GetIndikatorInd(data.bilans_dolgorocni_rezerviranja_2013, data.bilans_dolgorocni_rezerviranja_2012);
            data.bilans_dolgorocni_rezerviranja_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_dolgorocni_rezerviranja_2013);
            data.bilans_dolgorocni_rezerviranja_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_dolgorocni_rezerviranja_2012);

            data.bilans_dolgorocni_obvrski_sredstva = "2";



            data.bilans_dolgorocni_obvrski_ind = GetIndikatorInd(data.bilans_dolgorocni_obvrski_2013, data.bilans_dolgorocni_obvrski_2012);
            data.bilans_dolgorocni_obvrski_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_dolgorocni_obvrski_2013);
            data.bilans_dolgorocni_obvrski_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_dolgorocni_obvrski_2012);

            data.bilans_kratkorocni_obvrski_sredstva = "3";


            data.bilans_kratkorocni_obvrski_ind = GetIndikatorInd(data.bilans_kratkorocni_obvrski_2013, data.bilans_kratkorocni_obvrski_2012);
            data.bilans_kratkorocni_obvrski_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_kratkorocni_obvrski_2013);
            data.bilans_kratkorocni_obvrski_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_kratkorocni_obvrski_2012);

            data.bilans_odlozeni_obvrski_sredstva = "C";



            data.bilans_odlozeni_obvrski_ind = GetIndikatorInd(data.bilans_odlozeni_obvrski_2013, data.bilans_odlozeni_obvrski_2012);
            data.bilans_odlozeni_obvrski_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_odlozeni_obvrski_2013);
            data.bilans_odlozeni_obvrski_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_odlozeni_obvrski_2012);

            data.bilans_odlozeno_plakanje_sredstva = "D";



            data.bilans_odlozeno_plakanje_ind = GetIndikatorInd(data.bilans_odlozeno_plakanje_2013, data.bilans_odlozeno_plakanje_2012);
            data.bilans_odlozeno_plakanje_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_odlozeno_plakanje_2013);
            data.bilans_odlozeno_plakanje_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_odlozeno_plakanje_2012);

            data.bilans_obvrski_po_osnov_sredstva = "E";



            data.bilans_obvrski_po_osnov_ind = GetIndikatorInd(data.bilans_obvrski_po_osnov_2013, data.bilans_obvrski_po_osnov_2012);
            data.bilans_obvrski_po_osnov_2013_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2013, data.bilans_obvrski_po_osnov_2013);
            data.bilans_obvrski_po_osnov_2012_procent = CalculatePercentIndikator(data.bilans_vkupna_pasiva_2012, data.bilans_obvrski_po_osnov_2012);

            data.bilans_vkupna_pasiva_sredstva = "";


            data.bilans_vkupna_pasiva_ind = GetIndikatorInd(data.bilans_vkupna_pasiva_2013, data.bilans_vkupna_pasiva_2012);


            if (CheckIfValueAvailable(data.bilans_glavnina_i_rezervi_2013_procent) ||
                CheckIfValueAvailable(data.bilans_osnovna_glavnina_2013_procent) ||
                CheckIfValueAvailable(data.bilans_premii_akcii_2013_procent) ||
                CheckIfValueAvailable(data.bilans_sopstveni_akcii_2013_procent) ||
                CheckIfValueAvailable(data.bilans_zapisan_kapital_2013_procent) ||
                CheckIfValueAvailable(data.bilans_revalorizaciska_rezerva_2013_procent) ||
                CheckIfValueAvailable(data.bilans_rezervi_2013_procent) ||
                CheckIfValueAvailable(data.bilans_akumulirana_dobivka_2013_procent) ||
                CheckIfValueAvailable(data.bilans_prenesena_zaguba_2013_procent) ||
                CheckIfValueAvailable(data.bilans_dobivka_delovna_godina_2013_procent) ||
                CheckIfValueAvailable(data.bilans_zaguba_delovna_godina_2013_procent) ||
                CheckIfValueAvailable(data.bilans_obvrski_2013_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_rezerviranja_2013_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_obvrski_2013_procent) ||
                CheckIfValueAvailable(data.bilans_kratkorocni_obvrski_2013_procent) ||
                CheckIfValueAvailable(data.bilans_odlozeni_obvrski_2013_procent) ||
                CheckIfValueAvailable(data.bilans_odlozeno_plakanje_2013_procent) ||
                CheckIfValueAvailable(data.bilans_obvrski_po_osnov_2013_procent))
            {
                data.bilans_vkupna_pasiva_2013_procent = "100.0";
            }
            else
            {
                data.bilans_vkupna_pasiva_2013_procent = "/";
            }

            if (CheckIfValueAvailable(data.bilans_glavnina_i_rezervi_2012_procent) ||
                CheckIfValueAvailable(data.bilans_osnovna_glavnina_2012_procent) ||
                CheckIfValueAvailable(data.bilans_premii_akcii_2012_procent) ||
                CheckIfValueAvailable(data.bilans_sopstveni_akcii_2012_procent) ||
                CheckIfValueAvailable(data.bilans_zapisan_kapital_2012_procent) ||
                CheckIfValueAvailable(data.bilans_revalorizaciska_rezerva_2012_procent) ||
                CheckIfValueAvailable(data.bilans_rezervi_2012_procent) ||
                CheckIfValueAvailable(data.bilans_akumulirana_dobivka_2012_procent) ||
                CheckIfValueAvailable(data.bilans_prenesena_zaguba_2012_procent) ||
                CheckIfValueAvailable(data.bilans_dobivka_delovna_godina_2012_procent) ||
                CheckIfValueAvailable(data.bilans_zaguba_delovna_godina_2012_procent) ||
                CheckIfValueAvailable(data.bilans_obvrski_2012_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_rezerviranja_2012_procent) ||
                CheckIfValueAvailable(data.bilans_dolgorocni_obvrski_2012_procent) ||
                CheckIfValueAvailable(data.bilans_kratkorocni_obvrski_2012_procent) ||
                CheckIfValueAvailable(data.bilans_odlozeni_obvrski_2012_procent) ||
                CheckIfValueAvailable(data.bilans_odlozeno_plakanje_2012_procent) ||
                CheckIfValueAvailable(data.bilans_obvrski_po_osnov_2012_procent))
            {
                data.bilans_vkupna_pasiva_2012_procent = "100.0";
            }
            else
            {
                data.bilans_vkupna_pasiva_2012_procent = "/";
            }


            data.uspeh_prihodi_rabotenje_sredstva = "1";


            data.uspeh_vkupno_prihodi_sredstva = "";
            data.uspeh_vkupno_prihodi_2013 = data.vkupno_prihodi2;
            data.uspeh_vkupno_prihodi_2012 = data.vkupno_prihodi1;
            data.uspeh_vkupno_prihodi_ind = GetIndikatorInd(data.uspeh_vkupno_prihodi_2013, data.uspeh_vkupno_prihodi_2012);


            data.uspeh_prihodi_rabotenje_ind = GetIndikatorInd(data.uspeh_prihodi_rabotenje_2013, data.uspeh_prihodi_rabotenje_2012);
            data.uspeh_prihodi_rabotenje_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_prihodi_2013, data.uspeh_prihodi_rabotenje_2013);
            data.uspeh_prihodi_rabotenje_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_prihodi_2012, data.uspeh_prihodi_rabotenje_2012);

            data.uspeh_finansiski_prihodi_sredstva = "2";


            data.uspeh_finansiski_prihodi_ind = GetIndikatorInd(data.uspeh_finansiski_prihodi_2013, data.uspeh_finansiski_prihodi_2012);
            data.uspeh_finansiski_prihodi_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_prihodi_2013, data.uspeh_finansiski_prihodi_2013);
            data.uspeh_finansiski_prihodi_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_prihodi_2012, data.uspeh_finansiski_prihodi_2012);


            data.uspeh_rashodi_rabotenje_sredstva = "1";

            data.uspeh_vkupno_rashodi_sredstva = "";
            data.uspeh_vkupno_rashodi_ind = GetIndikatorInd(data.uspeh_vkupno_rashodi_2013, data.uspeh_vkupno_rashodi_2012);

            data.uspeh_rashodi_rabotenje_ind = GetIndikatorInd(data.uspeh_rashodi_rabotenje_2013, data.uspeh_rashodi_rabotenje_2012);
            data.uspeh_rashodi_rabotenje_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_rashodi_rabotenje_2013);
            data.uspeh_rashodi_rabotenje_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_rashodi_rabotenje_2012);

            data.uspeh_rashod_osnovna_dejnost_sredstva = "";
            data.uspeh_rashod_osnovna_dejnost_ind = GetIndikatorInd(data.uspeh_rashod_osnovna_dejnost_2013, data.uspeh_rashod_osnovna_dejnost_2012);
            data.uspeh_rashod_osnovna_dejnost_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_rashod_osnovna_dejnost_2013);
            data.uspeh_rashod_osnovna_dejnost_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_rashod_osnovna_dejnost_2012);

            data.uspeh_ostanati_trosoci_sredstva = "";



            data.uspeh_ostanati_trosoci_ind = GetIndikatorInd(data.uspeh_ostanati_trosoci_2013, data.uspeh_ostanati_trosoci_2012);
            data.uspeh_ostanati_trosoci_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_ostanati_trosoci_2013);
            data.uspeh_ostanati_trosoci_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_ostanati_trosoci_2012);

            data.uspeh_trosoci_za_vraboteni_sredstva = "";



            data.uspeh_trosoci_za_vraboteni_ind = GetIndikatorInd(data.uspeh_trosoci_za_vraboteni_2013, data.uspeh_trosoci_za_vraboteni_2012);
            data.uspeh_trosoci_za_vraboteni_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_trosoci_za_vraboteni_2013);
            data.uspeh_trosoci_za_vraboteni_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_trosoci_za_vraboteni_2012);

            data.uspeh_amortizacija_sredstva_sredstva = "";


            data.uspeh_amortizacija_sredstva_ind = GetIndikatorInd(data.uspeh_amortizacija_sredstva_2013, data.uspeh_amortizacija_sredstva_2012);
            data.uspeh_amortizacija_sredstva_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_amortizacija_sredstva_2013);
            data.uspeh_amortizacija_sredstva_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_amortizacija_sredstva_2012);

            data.uspeh_rezerviranje_trosoci_rizici_sredstva = "";



            data.uspeh_rezerviranje_trosoci_rizici_ind = GetIndikatorInd(data.uspeh_rezerviranje_trosoci_rizici_2013, data.uspeh_rezerviranje_trosoci_rizici_2012);
            data.uspeh_rezerviranje_trosoci_rizici_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_rezerviranje_trosoci_rizici_2013);
            data.uspeh_rezerviranje_trosoci_rizici_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_rezerviranje_trosoci_rizici_2012);

            data.uspeh_zalihi_proizvodi_pocetok_sredstva = "";
            data.uspeh_zalihi_proizvodi_pocetok_ind = GetIndikatorInd(data.uspeh_zalihi_proizvodi_pocetok_2013, data.uspeh_zalihi_proizvodi_pocetok_2012);
            data.uspeh_zalihi_proizvodi_pocetok_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_zalihi_proizvodi_pocetok_2013);
            data.uspeh_zalihi_proizvodi_pocetok_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_zalihi_proizvodi_pocetok_2012);

            data.uspeh_zalihi_proizvodi_kraj_sredstva = "";
            data.uspeh_zalihi_proizvodi_kraj_ind = GetIndikatorInd(data.uspeh_zalihi_proizvodi_kraj_2013, data.uspeh_zalihi_proizvodi_kraj_2012);
            data.uspeh_zalihi_proizvodi_kraj_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_zalihi_proizvodi_kraj_2013);
            data.uspeh_zalihi_proizvodi_kraj_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_zalihi_proizvodi_kraj_2012);

            data.uspeh_ostanati_rashodi_sredstva = "";


            data.uspeh_ostanati_rashodi_ind = GetIndikatorInd(data.uspeh_ostanati_rashodi_2013, data.uspeh_ostanati_rashodi_2012);
            data.uspeh_ostanati_rashodi_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_ostanati_rashodi_2013);
            data.uspeh_ostanati_rashodi_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_ostanati_rashodi_2012);

            data.uspeh_finansiski_rashodi_sredstva = "2";

            data.uspeh_finansiski_rashodi_ind = GetIndikatorInd(data.uspeh_finansiski_rashodi_2013, data.uspeh_finansiski_rashodi_2012);
            data.uspeh_finansiski_rashodi_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_finansiski_rashodi_2013);
            data.uspeh_finansiski_rashodi_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_finansiski_rashodi_2012);

            data.uspeh_finansiski_povrzani_drustva_sredstva = "";



            data.uspeh_finansiski_povrzani_drustva_ind = GetIndikatorInd(data.uspeh_finansiski_povrzani_drustva_2013, data.uspeh_finansiski_povrzani_drustva_2012);
            data.uspeh_finansiski_povrzani_drustva_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_finansiski_povrzani_drustva_2013);
            data.uspeh_finansiski_povrzani_drustva_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_finansiski_povrzani_drustva_2012);

            data.uspeh_rashodi_kamati_sredstva = "";


            data.uspeh_rashodi_kamati_ind = GetIndikatorInd(data.uspeh_rashodi_kamati_2013, data.uspeh_rashodi_kamati_2012);
            data.uspeh_rashodi_kamati_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_rashodi_kamati_2013);
            data.uspeh_rashodi_kamati_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_rashodi_kamati_2012);
            
            data.uspeh_rashodi_finansiski_sredstva_sredstva = "";
            data.uspeh_rashodi_finansiski_sredstva_2013 = SumValues(company_values, new int[] {241, 242 }).ToString();
            data.uspeh_rashodi_finansiski_sredstva_2012 = SumValues(company_values_lastyear, new int[] { 241, 242 }).ToString();
            data.uspeh_rashodi_finansiski_sredstva_ind = GetIndikatorInd(data.uspeh_rashodi_finansiski_sredstva_2013, data.uspeh_rashodi_finansiski_sredstva_2012);
            data.uspeh_rashodi_finansiski_sredstva_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_rashodi_finansiski_sredstva_2013);
            data.uspeh_rashodi_finansiski_sredstva_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_rashodi_finansiski_sredstva_2012);

            data.uspeh_ostanati_finansiski_rashodi_sredstva = "";


            data.uspeh_ostanati_finansiski_rashodi_ind = GetIndikatorInd(data.uspeh_ostanati_finansiski_rashodi_2013, data.uspeh_ostanati_finansiski_rashodi_2012);
            data.uspeh_ostanati_finansiski_rashodi_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_ostanati_finansiski_rashodi_2013);
            data.uspeh_ostanati_finansiski_rashodi_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_ostanati_finansiski_rashodi_2012);

            data.uspeh_udel_vo_zaguba_sredstva = "3";



            data.uspeh_udel_vo_zaguba_ind = GetIndikatorInd(data.uspeh_udel_vo_zaguba_2013, data.uspeh_udel_vo_zaguba_2012);
            data.uspeh_udel_vo_zaguba_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_udel_vo_zaguba_2013);
            data.uspeh_udel_vo_zaguba_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_udel_vo_zaguba_2012);

            data.uspeh_udel_vo_dobivka_sredstva = "3";



            data.uspeh_udel_vo_dobivka_ind = GetIndikatorInd(data.uspeh_udel_vo_dobivka_2013, data.uspeh_udel_vo_dobivka_2012);
            data.uspeh_udel_vo_dobivka_2013_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2013, data.uspeh_udel_vo_dobivka_2013);
            data.uspeh_udel_vo_dobivka_2012_procent = CalculatePercentIndikator(data.uspeh_vkupno_rashodi_2012, data.uspeh_udel_vo_dobivka_2012);



            if (CheckIfValueAvailable(data.uspeh_prihodi_rabotenje_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_finansiski_prihodi_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_udel_vo_dobivka_2013_procent))
            {
                data.uspeh_vkupno_prihodi_2013_procent = "100.0";
            }
            else
            {
                data.uspeh_vkupno_prihodi_2013_procent = "/";
            }
            if (CheckIfValueAvailable(data.uspeh_prihodi_rabotenje_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_finansiski_prihodi_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_udel_vo_dobivka_2012_procent))
            {
                data.uspeh_vkupno_prihodi_2012_procent = "100.0";
            }
            else
            {
                data.uspeh_vkupno_prihodi_2012_procent = "/";
            }









            if (CheckIfValueAvailable(data.uspeh_rashodi_rabotenje_2013_procent) || 
                CheckIfValueAvailable(data.uspeh_rashod_osnovna_dejnost_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_ostanati_trosoci_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_trosoci_za_vraboteni_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_amortizacija_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_rezerviranje_trosoci_rizici_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_zalihi_proizvodi_pocetok_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_zalihi_proizvodi_kraj_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_ostanati_rashodi_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_finansiski_rashodi_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_finansiski_povrzani_drustva_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_rashodi_kamati_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_rashodi_finansiski_sredstva_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_ostanati_finansiski_rashodi_2013_procent) ||
                CheckIfValueAvailable(data.uspeh_udel_vo_zaguba_2013_procent))
            {
                data.uspeh_vkupno_rashodi_2013_procent = "100.0";
            }
            else
            {
                data.uspeh_vkupno_rashodi_2013_procent = "/";
            }
            if (CheckIfValueAvailable(data.uspeh_rashodi_rabotenje_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_rashod_osnovna_dejnost_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_ostanati_trosoci_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_trosoci_za_vraboteni_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_amortizacija_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_rezerviranje_trosoci_rizici_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_zalihi_proizvodi_pocetok_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_zalihi_proizvodi_kraj_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_ostanati_rashodi_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_finansiski_rashodi_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_finansiski_povrzani_drustva_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_rashodi_kamati_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_rashodi_finansiski_sredstva_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_ostanati_finansiski_rashodi_2012_procent) ||
                CheckIfValueAvailable(data.uspeh_udel_vo_zaguba_2012_procent))
            {
                data.uspeh_vkupno_rashodi_2012_procent = "100.0";
            }
            else
            {
                data.uspeh_vkupno_rashodi_2012_procent = "/";
            }



            data.uspeh_dobivka_odanocuvanje_sredstva = "1";



            data.uspeh_dobivka_odanocuvanje_ind = GetIndikatorInd(data.uspeh_dobivka_odanocuvanje_2013, data.uspeh_dobivka_odanocuvanje_2012);

            data.uspeh_zaguba_odanocuvanje_sredstva = "2";



            data.uspeh_zaguba_odanocuvanje_ind = GetIndikatorInd(data.uspeh_zaguba_odanocuvanje_2013, data.uspeh_zaguba_odanocuvanje_2012);

            data.uspeh_danok_dobivka_sredstva = "3";


            data.uspeh_danok_dobivka_ind = GetIndikatorInd(data.uspeh_danok_dobivka_2013, data.uspeh_danok_dobivka_2012);

            data.uspeh_neto_dobivka_sredstva = "4";


            data.uspeh_neto_dobivka_ind = GetIndikatorInd(data.uspeh_neto_dobivka_2013, data.uspeh_neto_dobivka_2012);

            data.uspeh_neto_zaguba_sredstva = "5";


            data.uspeh_neto_zaguba_ind = GetIndikatorInd(data.uspeh_neto_zaguba_2013, data.uspeh_neto_zaguba_2012);


            data.indikatori_raboten_kapital_rast = GetRast(data.indikatori_raboten_kapital_2013, data.indikatori_raboten_kapital_2012);


            data.indikatori_tekoven_pokazatel_rast = GetRast(data.indikatori_tekoven_pokazatel_2013, data.indikatori_tekoven_pokazatel_2012);


            data.indikatori_brz_pokazatel_rast = GetRast(data.indikatori_brz_pokazatel_2013, data.indikatori_brz_pokazatel_2012);

            data.indikatori_obrt_sredstva_2013 = V_obrt_na_sredstva.ToString();

            data.indikatori_obrt_sredstva_2012 = V_obrt_na_sredstva_1.ToString();

            data.indikatori_obrt_sredstva_rast = GetRastPercent(data.indikatori_obrt_sredstva_2013, data.indikatori_obrt_sredstva_2012);

            data.indikatori_denovi_obrt_sredstva_2013 = denovi_na_obrt_na_sredstva.ToString();
            if (string.IsNullOrEmpty(data.indikatori_denovi_obrt_sredstva_2013) == false && data.indikatori_denovi_obrt_sredstva_2013 != "")
                data.indikatori_denovi_obrt_sredstva_2013 = FormatCurrencyComma(data.indikatori_denovi_obrt_sredstva_2013);

            data.indikatori_denovi_obrt_sredstva_2012 = denovi_na_obrt_na_sredstva_1.ToString();
            if (string.IsNullOrEmpty(data.indikatori_denovi_obrt_sredstva_2012) == false && data.indikatori_denovi_obrt_sredstva_2012 != "")
                data.indikatori_denovi_obrt_sredstva_2012 = FormatCurrencyComma(data.indikatori_denovi_obrt_sredstva_2012);

            data.indikatori_denovi_obrt_sredstva_rast = GetRast(data.indikatori_denovi_obrt_sredstva_2013, data.indikatori_denovi_obrt_sredstva_2012);

            data.indikatori_obrt_obvrski_2013 = G_obrt_na_obvrski;

            data.indikatori_obrt_obvrski_2012 = G_obrt_na_obvrski_1;

            data.indikatori_obrt_obvrski_rast = GetRast(data.indikatori_obrt_obvrski_2013, data.indikatori_obrt_obvrski_2012);

            data.indikatori_prosecni_denovi_obvrski_2013 = prosecni_denovi_na_plakanje_obvrski.ToString();
            if (string.IsNullOrEmpty(data.indikatori_prosecni_denovi_obvrski_2013) == false && data.indikatori_prosecni_denovi_obvrski_2013 != "")
                data.indikatori_prosecni_denovi_obvrski_2013 = FormatCurrencyComma(data.indikatori_prosecni_denovi_obvrski_2013);

            data.indikatori_prosecni_denovi_obvrski_2012 = prosecni_denovi_na_plakanje_obvrski_1.ToString();
            if (string.IsNullOrEmpty(data.indikatori_prosecni_denovi_obvrski_2012) == false && data.indikatori_prosecni_denovi_obvrski_2012 != "")
                data.indikatori_prosecni_denovi_obvrski_2012 = FormatCurrencyComma(data.indikatori_prosecni_denovi_obvrski_2012);

            data.indikatori_prosecni_denovi_obvrski_rast = GetRast(data.indikatori_prosecni_denovi_obvrski_2013, data.indikatori_prosecni_denovi_obvrski_2012);

            data.indikatori_obrt_pobaruvanja_2013 = A_prihodi_od_prodazba_prosecni_pobaruvanja.ToString();


            data.indikatori_obrt_pobaruvanja_2012 = A_prihodi_od_prodazba_prosecni_pobaruvanja_1.ToString();

            data.indikatori_obrt_pobaruvanja_rast = GetRastPercent(data.indikatori_obrt_pobaruvanja_2013, data.indikatori_obrt_pobaruvanja_2012);

            data.indikatori_denovi_obrt_pobaruvanja_2013 = denovi_na_obrt_na_pobaruvanje.ToString();
            if (string.IsNullOrEmpty(data.indikatori_denovi_obrt_pobaruvanja_2013) == false && data.indikatori_denovi_obrt_pobaruvanja_2013 != "")
                data.indikatori_denovi_obrt_pobaruvanja_2013 = FormatCurrencyComma(data.indikatori_denovi_obrt_pobaruvanja_2013);

            data.indikatori_denovi_obrt_pobaruvanja_2012 = denovi_na_obrt_na_pobaruvanje_1.ToString();
            if (string.IsNullOrEmpty(data.indikatori_denovi_obrt_pobaruvanja_2012) == false && data.indikatori_denovi_obrt_pobaruvanja_2012 != "")
                data.indikatori_denovi_obrt_pobaruvanja_2012 = FormatCurrencyComma(data.indikatori_denovi_obrt_pobaruvanja_2012);

            data.indikatori_denovi_obrt_pobaruvanja_rast = GetRast(data.indikatori_denovi_obrt_pobaruvanja_2013, data.indikatori_denovi_obrt_pobaruvanja_2012);

            data.indikatori_obrt_zalihi_2013 = B_obrt_na_zalihi.ToString();

            data.indikatori_obrt_zalihi_2012 = B_obrt_na_zalihi_1.ToString();

            data.indikatori_obrt_zalihi_rast = GetRastPercent(data.indikatori_obrt_zalihi_2013, data.indikatori_obrt_zalihi_2012);

            data.indikatori_denovi_obrt_zalihi_2013 = denovi_na_obrt_na_zalihi.ToString();
            if (string.IsNullOrEmpty(data.indikatori_denovi_obrt_zalihi_2013) == false && data.indikatori_denovi_obrt_zalihi_2013 != "")
                data.indikatori_denovi_obrt_zalihi_2013 = FormatCurrencyComma(data.indikatori_denovi_obrt_zalihi_2013);

            data.indikatori_denovi_obrt_zalihi_2012 = denovi_na_obrt_na_zalihi_1.ToString();
            if (string.IsNullOrEmpty(data.indikatori_denovi_obrt_zalihi_2012) == false && data.indikatori_denovi_obrt_zalihi_2012 != "")
                data.indikatori_denovi_obrt_zalihi_2012 = FormatCurrencyComma(data.indikatori_denovi_obrt_zalihi_2012);

            data.indikatori_denovi_obrt_zalihi_rast = GetRast(data.indikatori_denovi_obrt_zalihi_2013, data.indikatori_denovi_obrt_zalihi_2012);


            data.indikatori_povrat_kapital_rast = GetRastPercent(data.indikatori_povrat_kapital_2013, data.indikatori_povrat_kapital_2012);


            data.indikatori_povrat_sredstva_rast = GetRastPercent(data.indikatori_povrat_sredstva_2013, data.indikatori_povrat_sredstva_2012);




            data.indikatori_neto_profitna_margina_rast = GetRastPercent(data.indikatori_neto_profitna_margina_2013, data.indikatori_neto_profitna_margina_2012);



            data.indikatori_finansiski_leviridz_rast = GetRast(data.indikatori_finansiski_leviridz_2013, data.indikatori_finansiski_leviridz_2012);


            data.indikatori_koeficient_zadolzenost_rast = GetRast(data.indikatori_koeficient_zadolzenost_2013, data.indikatori_koeficient_zadolzenost_2012);

            data.indikatori_vkupni_obvrski_2013 = vkupni_obvrski_ebitda;


            data.indikatori_vkupni_obvrski_2012 = vkupni_obvrski_ebitda_1;

            data.indikatori_vkupni_obvrski_rast = GetRast(data.indikatori_vkupni_obvrski_2013, data.indikatori_vkupni_obvrski_2012);

            data.indikatori_pokrienost_servisiranje_2013 = DSCR;

            data.indikatori_pokrienost_servisiranje_2012 = DSCR_1;

            data.indikatori_pokrienost_servisiranje_rast = GetRast(data.indikatori_pokrienost_servisiranje_2013, data.indikatori_pokrienost_servisiranje_2012);

            data.indikatori_pokrienost_kamati_2013 = pokrienost_na_kamati.ToString();


            data.indikatori_pokrienost_kamati_2012 = pokrienost_na_kamati_1.ToString();

            data.indikatori_pokrienost_kamati_rast = GetRast(data.indikatori_pokrienost_kamati_2013, data.indikatori_pokrienost_kamati_2012);

            data.indikatori_kratkorocni_krediti_rast = GetRastPercent(data.indikatori_kratkorocni_krediti_2013, data.indikatori_kratkorocni_krediti_2012);

            data.indikatori_tekovni_obvrski_rast = GetRastPercent(data.indikatori_tekovni_obvrski_2013, data.indikatori_tekovni_obvrski_2012);


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

            //pie chart colors
            data.pie_chart_colors = new List<int[]>();
            data.pie_chart_colors.Add(new int[] { 113, 137, 63 });
            data.pie_chart_colors.Add(new int[] { 132, 159, 75 });
            data.pie_chart_colors.Add(new int[] { 148, 178, 85 });
            data.pie_chart_colors.Add(new int[] { 169, 195, 121 });
            data.pie_chart_colors.Add(new int[] { 192, 210, 164 });
            data.pie_chart_colors.Add(new int[] { 213, 224, 196 });
            data.pie_chart_colors.Add(new int[] { 213, 229, 196 });

            // pie chart Aktiva
            var netekovni_sredstva = 0.0;
            var odlozeni_danocni_sredstva = 0.0;
            var tekovni_sredstva = 0.0;
            var sredstva_grupi = 0.0;
            var pretplateni_trosoci = 0.0;

            Double.TryParse(data.bilans_netekovni_2013.Replace(",",""), out netekovni_sredstva);
            Double.TryParse(data.bilans_odlozeni_danocni_sredstva_2013.Replace(",", ""), out odlozeni_danocni_sredstva);
            Double.TryParse(data.bilans_tekovni_sredstva_2013.Replace(",", ""), out tekovni_sredstva);
            Double.TryParse(data.bilans_sredstva_grupa_2013.Replace(",", ""), out sredstva_grupi);
            Double.TryParse(data.bilans_plateni_trosoci_2013.Replace(",", ""), out pretplateni_trosoci);

            data.pie_chart_aktiva = new Dictionary<string, List<double>>();
            data.pie_chart_aktiva["Средства"] = new List<double>();
            data.pie_chart_aktiva["Одложени дан. средства"] = new List<double>();
            data.pie_chart_aktiva["Тековни средства"] = new List<double>();
            data.pie_chart_aktiva["Средства (или груп...)"] = new List<double>();
            data.pie_chart_aktiva["Претплатени трошоци..."] = new List<double>();

            SetPieChartValues(data.pie_chart_aktiva, new double[] {netekovni_sredstva, odlozeni_danocni_sredstva, tekovni_sredstva, sredstva_grupi, pretplateni_trosoci});

            var piechart1 = DocumentChart.CreatePieChartNew((int)F_PieCharts.Aktiva, data.pie_chart_aktiva, data.pie_chart_colors);

            data.pie_chart_filename1 = piechart1;


            // pie chart Pasiva
            var glavnina_i_rezervi = 0.0;
            var dolgorocni_obvrski = 0.0;
            var kratkorocni_obvrski = 0.0;
            var dolgorocni_rezerviranja = 0.0;
            var obvrski_po_osnov = 0.0;
            var odlozeni_danocni_obvrski = 0.0;
            var odlozeno_plakanje = 0.0;

            Double.TryParse(data.bilans_glavnina_i_rezervi_2013.Replace(",", ""), out glavnina_i_rezervi);
            Double.TryParse(data.bilans_dolgorocni_obvrski_2013.Replace(",", ""), out dolgorocni_obvrski);
            Double.TryParse(data.bilans_kratkorocni_obvrski_2013.Replace(",", ""), out kratkorocni_obvrski);
            Double.TryParse(data.bilans_obvrski_po_osnov_2013.Replace(",", ""), out obvrski_po_osnov);
            Double.TryParse(data.bilans_odlozeni_obvrski_2013.Replace(",", ""), out odlozeni_danocni_obvrski);
            Double.TryParse(data.bilans_odlozeno_plakanje_2013.Replace(",", ""), out odlozeno_plakanje);
            Double.TryParse(data.bilans_dolgorocni_rezerviranja_2013.Replace(",", ""), out dolgorocni_rezerviranja);


            data.pie_chart_pasiva = new Dictionary<string, List<double>>();
            data.pie_chart_pasiva["Главнина и резерви"] = new List<double>();
            data.pie_chart_pasiva["Долгорочни обврски"] = new List<double>();
            data.pie_chart_pasiva["Краткорочни обврски"] = new List<double>();
            data.pie_chart_pasiva["Одложени даночни обврски"] = new List<double>();
            data.pie_chart_pasiva["Обврски по основ на нетековни средства..."] = new List<double>();
            data.pie_chart_pasiva["Одложени плаќања на..."] = new List<double>();
            data.pie_chart_pasiva["Долгорочни резервирања за..."] = new List<double>();

            SetPieChartValues(data.pie_chart_pasiva, new double[] { 
                glavnina_i_rezervi, dolgorocni_obvrski, kratkorocni_obvrski, odlozeni_danocni_obvrski, obvrski_po_osnov , odlozeno_plakanje, dolgorocni_rezerviranja});

            data.pie_chart_filename2 = DocumentChart.CreatePieChartNew((int)F_PieCharts.Pasiva, data.pie_chart_pasiva, data.pie_chart_colors);

            // pie chart Prihodi
            var uspeh_finansiski_prihodi = 0.0;
            var uspeh_prihodi_rabotenje = 0.0;
            var uspeh_udel_vo_dobivka = 0.0;

            Double.TryParse(data.uspeh_finansiski_prihodi_2013.Replace(",", ""), out uspeh_finansiski_prihodi);
            Double.TryParse(data.uspeh_prihodi_rabotenje_2013.Replace(",", ""), out uspeh_prihodi_rabotenje);
            Double.TryParse(data.uspeh_udel_vo_dobivka_2013.Replace(",", ""), out uspeh_udel_vo_dobivka);

            data.pie_chart_prihodi = new Dictionary<string, List<double>>();
            data.pie_chart_prihodi["Приходи од работењето"] = new List<double>();
            data.pie_chart_prihodi["Финансиски приходи"] = new List<double>();
            data.pie_chart_prihodi["Друго"] = new List<double>();

            SetPieChartValues(data.pie_chart_prihodi, new double[] { 
                uspeh_prihodi_rabotenje, uspeh_finansiski_prihodi, uspeh_udel_vo_dobivka });

            data.pie_chart_filename3 = DocumentChart.CreatePieChartNew((int)F_PieCharts.Prihodi, data.pie_chart_prihodi, data.pie_chart_colors);


            // pie chart Rashodi
            var finansiski_rashodi = 0.0;
            var trosoci_za_vraboteni = 0.0;
            var rashodi_od_osnovna_dejnost = CalculateValues(company_values, new int[] { 207, 213, 212, 222 }, new string[] { "-", "-", "-" });
            var drugo = CalculateValues(company_values, new int[] { 212, 222, 245 }, new string[] { "+", "+" });

            Double.TryParse(data.uspeh_finansiski_rashodi_2013.Replace(",", ""), out finansiski_rashodi);
            Double.TryParse(data.uspeh_trosoci_za_vraboteni_2013.Replace(",", ""), out trosoci_za_vraboteni);

            data.pie_chart_rashodi = new Dictionary<string, List<double>>();
            data.pie_chart_rashodi["Расходи од основна дејност"] = new List<double>();
            data.pie_chart_rashodi["Трошоци за вработените"] = new List<double>();
            data.pie_chart_rashodi["Финансиски расходи"] = new List<double>();
            data.pie_chart_rashodi["Друго"] = new List<double>();

            SetPieChartValues(data.pie_chart_rashodi, new double[] { 
                rashodi_od_osnovna_dejnost, trosoci_za_vraboteni, finansiski_rashodi, drugo });

            data.pie_chart_filename4 = DocumentChart.CreatePieChartNew((int)F_PieCharts.Rashodi, data.pie_chart_rashodi, data.pie_chart_colors);

            Attributes tmpAttributes = new Attributes();

            PropertyInfo[] properties = typeof(Attributes).GetProperties();
            try
            {
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(data) != null)
                    {
                        if (property.GetValue(data).GetType().ToString() == "System.String")
                        {
                            if (property.GetValue(data).ToString() == "n. def." ||
                                property.GetValue(data).ToString() == "n. def. (n. def.)" ||
                                property.GetValue(data).ToString() == "n. def.%" ||
                                property.GetValue(data).ToString().Contains("+unendlich"))
                            {
                                property.SetValue(data, "/");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var a = ex;
            }

            SetSemafor(data);
            FixEmptyValues(data);

            return data;

        }

        public static void FixEmptyValues(Attributes data) {
            var validProperties = new List<string>() { 
                "bilans_netekovni_2012",
                "bilans_nematerijalni_2012",
                "bilans_materijalni_2012",
                "bilans_vlozuvanje_nedviznosti_2012",
                "bilans_dolgorocni_sredstva_2012",
                "bilans_dolgorocni_pobaruvanja_2012",
                "bilans_odlozeni_danocni_sredstva_2012",
                "bilans_tekovni_sredstva_2012",
                "bilans_zalihi_2012",
                "bilans_kratkorocni_pobaruvanja_2012",
                "bilans_kratkorocni_sredstva_2012",
                "bilans_paricni_sredstva_2012",
                "bilans_sredstva_grupa_2012",
                "bilans_plateni_trosoci_2012",
                "bilans_vkupna_aktiva_2012",
                "bilans_glavnina_i_rezervi_2012",
                "bilans_osnovna_glavnina_2012",
                "bilans_premii_akcii_2012",
                "bilans_sopstveni_akcii_2012",
                "bilans_zapisan_kapital_2012",
                "bilans_revalorizaciska_rezerva_2012",
                "bilans_rezervi_2012",
                "bilans_akumulirana_dobivka_2012",
                "bilans_prenesena_zaguba_2012",
                "bilans_dobivka_delovna_godina_2012",
                "bilans_zaguba_delovna_godina_2012",
                "bilans_dolgorocni_obvrski_2012",
                "bilans_kratkorocni_obvrski_2012",
                "bilans_odlozeni_obvrski_2012",
                "bilans_odlozeno_plakanje_2012",
                "bilans_obvrski_po_osnov_2012",
                "bilans_vkupna_pasiva_2012",
                "bilans_dolgorocni_rezerviranja_2012",
                "bilans_obvrski_2012",
                "uspeh_prihodi_rabotenje_2012",
                "uspeh_finansiski_prihodi_2012",
                "uspeh_udel_vo_dobivka_2012",
                "uspeh_rashodi_rabotenje_2012",
                "uspeh_rashod_osnovna_dejnost_2012",
                "uspeh_ostanati_trosoci_2012",
                "uspeh_trosoci_za_vraboteni_2012",
                "uspeh_amortizacija_sredstva_2012",
                "uspeh_rezerviranje_trosoci_rizici_2012",
                "uspeh_zalihi_proizvodi_pocetok_2012",
                "uspeh_zalihi_proizvodi_kraj_2012",
                "uspeh_ostanati_rashodi_2012",
                "uspeh_finansiski_rashodi_2012",
                "uspeh_finansiski_povrzani_drustva_2012",
                "uspeh_rashodi_kamati_2012",
                "uspeh_ostanati_finansiski_rashodi_2012",
                "uspeh_udel_vo_zaguba_2012",
                "uspeh_dobivka_odanocuvanje_2012",
                "uspeh_zaguba_odanocuvanje_2012",
                "uspeh_danok_dobivka_2012",
                "uspeh_neto_dobivka_2012",
                "uspeh_neto_zaguba_2012",

                "bilans_netekovni_2013",
                "bilans_nematerijalni_2013",
                "bilans_materijalni_2013",
                "bilans_vlozuvanje_nedviznosti_2013",
                "bilans_dolgorocni_sredstva_2013",
                "bilans_dolgorocni_pobaruvanja_2013",
                "bilans_odlozeni_danocni_sredstva_2013",
                "bilans_tekovni_sredstva_2013",
                "bilans_zalihi_2013",
                "bilans_kratkorocni_pobaruvanja_2013",
                "bilans_kratkorocni_sredstva_2013",
                "bilans_paricni_sredstva_2013",
                "bilans_sredstva_grupa_2013",
                "bilans_plateni_trosoci_2013",
                "bilans_vkupna_aktiva_2013",
                "bilans_glavnina_i_rezervi_2013",
                "bilans_osnovna_glavnina_2013",
                "bilans_premii_akcii_2013",
                "bilans_sopstveni_akcii_2013",
                "bilans_zapisan_kapital_2013",
                "bilans_revalorizaciska_rezerva_2013",
                "bilans_rezervi_2013",
                "bilans_akumulirana_dobivka_2013",
                "bilans_prenesena_zaguba_2013",
                "bilans_dobivka_delovna_godina_2013",
                "bilans_zaguba_delovna_godina_2013",
                "bilans_dolgorocni_obvrski_2013",
                "bilans_kratkorocni_obvrski_2013",
                "bilans_odlozeni_obvrski_2013",
                "bilans_odlozeno_plakanje_2013",
                "bilans_obvrski_po_osnov_2013",
                "bilans_vkupna_pasiva_2013",
                "bilans_dolgorocni_rezerviranja_2013",
                "bilans_obvrski_2013",
                "uspeh_prihodi_rabotenje_2013",
                "uspeh_finansiski_prihodi_2013",
                "uspeh_udel_vo_dobivka_2013",
                "uspeh_rashodi_rabotenje_2013",
                "uspeh_rashod_osnovna_dejnost_2013",
                "uspeh_ostanati_trosoci_2013",
                "uspeh_trosoci_za_vraboteni_2013",
                "uspeh_amortizacija_sredstva_2013",
                "uspeh_rezerviranje_trosoci_rizici_2013",
                "uspeh_zalihi_proizvodi_pocetok_2013",
                "uspeh_zalihi_proizvodi_kraj_2013",
                "uspeh_ostanati_rashodi_2013",
                "uspeh_finansiski_rashodi_2013",
                "uspeh_finansiski_povrzani_drustva_2013",
                "uspeh_rashodi_kamati_2013",
                "uspeh_ostanati_finansiski_rashodi_2013",
                "uspeh_udel_vo_zaguba_2013",
                "uspeh_dobivka_odanocuvanje_2013",
                "uspeh_zaguba_odanocuvanje_2013",
                "uspeh_danok_dobivka_2013",
                "uspeh_neto_dobivka_2013",
                "uspeh_neto_zaguba_2013",
            };

            var validIndikatori = new List<string>() { 
                "indikatori_raboten_kapital_2012",
                "indikatori_tekoven_pokazatel_2012",
                "indikatori_brz_pokazatel_2012",
                "indikatori_obrt_sredstva_2012",
                "indikatori_denovi_obrt_sredstva_2012",
                "indikatori_obrt_zalihi_2012",
                "indikatori_denovi_obrt_zalihi_2012",
                "indikatori_obrt_pobaruvanja_2012",
                "indikatori_denovi_obrt_pobaruvanja_2012",
                "indikatori_obrt_obvrski_2012",
                "indikatori_prosecni_denovi_obvrski_2012",
                "indikatori_obrt_sredstva_2012",
                "indikatori_denovi_obrt_sredstva_2012",
                "indikatori_povrat_kapital_2012",
                "indikatori_povrat_sredstva_2012",
                "indikatori_neto_profitna_margina_2012",
                "indikatori_finansiski_leviridz_2012",
                "indikatori_koeficient_zadolzenost_2012",
                "indikatori_vkupni_obvrski_2012",
                "indikatori_pokrienost_servisiranje_2012",
                "indikatori_pokrienost_kamati_2012",
                "indikatori_kratkorocni_krediti_2012",
                "indikatori_tekovni_obvrski_2012",
                
                "indikatori_raboten_kapital_2013",
                "indikatori_tekoven_pokazatel_2013",
                "indikatori_brz_pokazatel_2013",
                "indikatori_obrt_sredstva_2013",
                "indikatori_denovi_obrt_sredstva_2013",
                "indikatori_obrt_zalihi_2013",
                "indikatori_denovi_obrt_zalihi_2013",
                "indikatori_obrt_pobaruvanja_2013",
                "indikatori_denovi_obrt_pobaruvanja_2013",
                "indikatori_obrt_obvrski_2013",
                "indikatori_prosecni_denovi_obvrski_2013",
                "indikatori_obrt_sredstva_2013",
                "indikatori_denovi_obrt_sredstva_2013",
                "indikatori_povrat_kapital_2013",
                "indikatori_povrat_sredstva_2013",
                "indikatori_neto_profitna_margina_2013",
                "indikatori_finansiski_leviridz_2013",
                "indikatori_koeficient_zadolzenost_2013",
                "indikatori_vkupni_obvrski_2013",
                "indikatori_pokrienost_servisiranje_2013",
                "indikatori_pokrienost_kamati_2013",
                "indikatori_kratkorocni_krediti_2013",
                "indikatori_tekovni_obvrski_2013",
            };


            Attributes tmpAttributes = new Attributes();

            PropertyInfo[] properties = typeof(Attributes).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                foreach (var validProperty in validProperties)
                {
                    if (property.Name == validProperty)
                    {
                        if (property.GetValue(data) == null)
                        {
                            property.SetValue(data, "0");
                        }
                        else if (property.GetValue(data).ToString() == "") {
                            property.SetValue(data, "0");
                        }
                    }
                }
                foreach (var validProperty in validIndikatori)
                {
                    if (property.Name == validProperty)
                    {
                        if (property.GetValue(data) == null)
                        {
                            property.SetValue(data, "/");
                        }
                        else if (property.GetValue(data).ToString() == "")
                        {
                            property.SetValue(data, "/");
                        }
                    }
                }
            }

            
        }

        public static void SetSemafor(Attributes data) {
            // nema godisna smetka = red
            // if = red
            var semafor = "red";
            if (data.GodisnaSmetka == false || data.naziv_firma.Contains("во стечај") || data.naziv_firma.Contains("во ликвидација") ||
                data.celosen_naziv_firma.Contains("во стечај") || data.celosen_naziv_firma.Contains("во ликвидација"))
            {
                semafor = "red";
            }
            else { 
                // if = red
                var check1 = false;
                var check2 = false;
                var raboten_kapital2 = 0.0;
                var raboten_kapital1 = 0.0;
                var check3 = false;
                var ebitda1 = 0.0;
                var ebitda12 = 0.0;
                if (data.likvidnost_opis_koeficient_za_zadolzenost == "Незадоволително")
                    check1 = true;
                if(data.indikatori_raboten_kapital_2012 != "/" && data.indikatori_raboten_kapital_2012 != ""){ 
                    Double.TryParse(data.indikatori_raboten_kapital_2012.Replace(",",""), out raboten_kapital1);
                }
                if(data.indikatori_raboten_kapital_2013 != "/" && data.indikatori_raboten_kapital_2013 != ""){
                    Double.TryParse(data.indikatori_raboten_kapital_2013, out raboten_kapital2);
                }
                   
              
                if(raboten_kapital1 < 0 || raboten_kapital2 < 0){
                    check2 = true;
                }else{
                    check2 = false;
                }
                if(data.profitabilnost_ebitda1 != "/" && data.profitabilnost_ebitda1 != ""){
                    Double.TryParse(data.profitabilnost_ebitda1.Replace(",",""), out ebitda1);
                } 
                if(data.profitabilnost_ebitda2 != "/" && data.profitabilnost_ebitda2 != ""){
                    Double.TryParse(data.profitabilnost_ebitda2.Replace(",",""), out ebitda12);
                }
                    
                if(ebitda1 < 0 || ebitda12 < 0){
                    check3 = true;
                }else{
                    check3 = false;
                }

                if (check1 && check2 && check3)
                {
                    semafor = "red";
                }
                else {
                    semafor = "yellow";

                    var years = CheckHowOldIs(data.datum_osnovanje.ToString());

                    //yellow
                    if (years <= 2)
                    {
                        semafor = "yellow";
                    }
                    else
                    {
                        // green
                        semafor = "green";
                        if(data.efikasnost_opis_povrat_na_sredstva == "Незадоволително")
                            semafor = "yellow";
                        if(data.efikasnost_opis_profitna_marza == "Незадоволително")
                            semafor = "yellow";
                        if(data.likvidnost_opis_koeficient_za_zadolzenost == "Незадоволително")
                            semafor = "yellow";
                        if(data.likvidnost_opis_brz_pokazatel == "Незадоволително")
                            semafor = "yellow";
                        if(data.likvidnost_opis_prosecni_denovi == "Незадоволително")
                            semafor = "yellow";
                        if (data.likvidnost_opis_kreditna_izlozenost == "Незадоволително")
                            semafor = "yellow";
                    }
                }
            }


            var semaforeInt = DALHelper.GetSemaphore(Int32.Parse(data.embs));


            if (semaforeInt == 1)
            {
                semafor = "red";
                data.solventnost_komentar = "Евидентирани настани кои укажуваат на негативна промена на солвентноста на компанијата";
            }
            else if (semaforeInt == 2)
            {
                semafor = "yellow";
                data.solventnost_komentar = "Во нашата база се евидентирани активности кои можат да влијаат на солвентноста на компанијата";
            }else{
                semafor = "green";
                data.solventnost_komentar = "Во нашата база не се евидентирани активности кои можат да влијаат на солвентноста на компанијата";
            }


            if (data.tekovni_blokadi_status.Contains("Има блокада"))
            {
                semafor = "red";
                data.solventnost_komentar = "Евидентирани настани кои укажуваат на негативна промена на солвентноста на компанијата";
            }
            data.semafor_solventnost = semafor;
        }

        public static void SetPieChartValues(Dictionary<string, List<double>> pie_chart, double[] values)
        {
            var length = values.Length;
            var total = 0.0;
            foreach (var item in values)
	        {
                var CurValue = item;
                if (CurValue < 0)
                    CurValue = CurValue * (-1);
                total += CurValue;
	        }
            var counter = 0;
            foreach (var item in pie_chart)
            {
                var CurValue = values[counter];
                if (CurValue < 0)
                    CurValue = CurValue * (-1);

                var percent = Math.Round((values[counter] / total) * 100, 1);
                item.Value.Add(CurValue);
                item.Value.Add(percent);
                counter++;
            }
        }


        public static string GetCurrencyName(string name)
        {
            switch (name)
            {
                case "MKD":
                    return "МКД";
                case "EUR":
                    return "ЕУР";
                default:
                    return string.Empty;
            }

        }

        public static bool ExcludedProperties(PropertyInfo property)
        {
            string[] names = new string[] {"paricen_vlog", "neparicen_vlog","uplaten_del","vkupna_osnovna_glavnina", "drzava", "izdaden_za", "solventnost_komentar", "semafor_solventnost", "dejnost",
                "pravna_forma", "tekovni_blokadi_status", "sostojba_komentar", "datum_osnovanje", "registrirana_vo", "region", "kapital", "ddv_obvrznik", 
                "likvidnost_opis_main", "efikasnost_opis_main", "blokadi_datum1", "blokadi_opis1"};
            foreach (var item in names)
            {
                if (item == property.Name)
                    return false;
            }
            return true;
        }


        public static double SumValues(Dictionary<int,double> values, int[] valuePos)
        {
            double res = 0;

            foreach (var valueP in valuePos)
            {
                if (values.ContainsKey(valueP))
                {
                    res += values[valueP];
                }
            }

            return res;
        }

        public static double DoCalculation(double res, double val, string sign)
        {
            switch(sign)
            {
                case "+":
                    res += val;
                    break;
                case "-":
                    res -= val;
                    break;
                case "*":
                    res *= val;
                    break;
                case "/":
                    res /= val;
                    break;
            }

            return res;
        }

        public static double CalculateValues(Dictionary<int, double> values, int[] valuePos, string[] signs) 
        {
            double res = 0;

            bool isFirst = true;

            var counter = -1;

            foreach (var valueP in valuePos)
            {
                if (isFirst)
                {
                    if (values.ContainsKey(valueP))
                    {
                        res = DoCalculation(res, values[valueP], "+");
                    }
                    else
                        res = 0;

                    isFirst = false;
                }
                else if (values.ContainsKey(valueP))
                {
                    res = DoCalculation(res, values[valueP], signs[counter]);
                }
                counter++;
            }

            return res;
        }

        public static double SubtractValues(Dictionary<int, double> values, int[] valuePos)
        {
            double res = 0;

            bool isFirst = true;

            foreach (var valueP in valuePos)
            {
                if (isFirst)
                {
                    if (values.ContainsKey(valueP))
                    {
                        res = values[valueP];
                    }
                    else
                        res = 0;

                    isFirst = false;
                }
                else if (values.ContainsKey(valueP))
                {
                    res -= values[valueP];
                }
            }

            return res;
        }

        public static string DevideValues(double val1, Dictionary<int, double> values, int[] valuePos)
        {
            double res = val1;

            foreach (var valueP in valuePos)
            {
                if (values.ContainsKey(valueP))
                {
                    res = res / values[valueP];
                }
                else
                    return "/";
            }

            return res.ToString("0.00");
        }

        public static string DevideValues(Dictionary<int, double> values, int[] valuePos)
        {
            double res = 0;

            bool isFirst = true;

            foreach (var valueP in valuePos)
            {
                if (isFirst)
                {
                    if (values.ContainsKey(valueP))
                    {
                        res = values[valueP];
                    }
                    else
                        res = 0;

                    isFirst = false;
                }
                else if (values.ContainsKey(valueP))
                {
                    res = res / values[valueP];
                }
                else
                    return "/";
            }

            return res.ToString("0.00");
        }


        public static double GetValue(Dictionary<int, double> values, int valuePos)
        {
            if (values.ContainsKey(valuePos))
            {
                return values[valuePos];
            }

            return 0;
        }

        public static string GetValueStr(Dictionary<int, double> values, int valuePos)
        {
            if (values.ContainsKey(valuePos))
            {
                return values[valuePos].ToString();
            }

            return "/";
        }

        public static string GetRast(string xval, string yval)
        {
            var x = 0.0;
            var y = 0.0;

            if (xval != "" && xval != "/")
                x = double.Parse(xval.Replace("%", "").Replace(",",""));
            else
                return "0.00";

            if (yval != "" && yval != "/")
                y = double.Parse(yval.Replace("%", "").Replace(",", ""));
            else
                return "0.00";

            if (y == 0.0)
                return "/";

            var tmpRes1 = (x - y);

            var tmpRes2 = ((x - y) / y);

            var res1 = "";
            var res2 = "";
            if (IsDecimalNumber(tmpRes1))
            {
                res1 = tmpRes1.ToString("0.00");
            }
            else
            {
                res1 = FormatCurrencyComma(tmpRes1.ToString());
            }

            if (IsDecimalNumber(tmpRes2))
            {
                res2 = tmpRes2.ToString("0.00");
            }
            else
            {
                res2 = FormatCurrencyComma(tmpRes2.ToString());
            }

            return res1 + " (" + res2 + ")";
        }

        public static string GetRastPercent(string xval, string yval)
        {
            var x = 0.0;
            var y = 0.0;

            if (xval != "" && xval != "/")
                x = double.Parse(xval.Replace("%", "").Replace(",", ""));
            else
                return "0.00";

            if (yval != "" && yval != "/")
                y = double.Parse(yval.Replace("%", "").Replace(",", ""));
            else
                return "0.00";

            if (y == 0.0)
                return "/";

            var tmpRes1 = ((x / 100) - (y / 100));

            var tmpRes2 = ((x - y) / y);

            var res1 = "";
            var res2 = "";
            if (IsDecimalNumber(tmpRes1))
            {
                res1 = tmpRes1.ToString("0.00"); 
            }
            else {
                res1 = FormatCurrencyComma(tmpRes1.ToString());
            }

            if (IsDecimalNumber(tmpRes2))
            {
                res2 = tmpRes2.ToString("0.00");
            }
            else
            {
                res2 = FormatCurrencyComma(tmpRes2.ToString());
            }


            return res1 + " (" + res2 + ")";
        }

        public static string GetIndikatorInd(string xval, string yval)
        {
            var x = 0.0;
            var y = 0.0;

            if (xval != "" && xval != "/")
                x = double.Parse(xval.Replace("%", "").Replace(",", ""));
            else
                return "0.0";

            if (yval != "" && yval != "/")
                y = double.Parse(yval.Replace("%", "").Replace(",", ""));
            else
                return "0.0";

            if (y == 0.0)
                return "/";

            var tmpRes = (100 * x) / y;

            var res = "";
            if (IsDecimalNumber(tmpRes))
            {
                res = tmpRes.ToString("0.0");
            }
            else
            {
                res = FormatCurrencyComma(tmpRes.ToString());
            }


            return res;
        }

        public static string CalculatePercentIndikator(string total, string indikator)
        {
            var x = 0.0;
            var y = 0.0;

            if (total != "" && total != null)
                x = double.Parse(total.Replace("%", "").Replace(",", ""));
            else
                return "0.0";

            if (x == 0.0)
                return "/";

            if (indikator != "" && total != null)
                y = double.Parse(indikator.Replace("%", "").Replace(",", ""));

            var res = ((y / x) * 100).ToString("0.0");

            return res;
        }

        public static bool IsDecimalNumber(double number)
        {
            if ((number % 1) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string FormatStringDate(string input)
        {
            if (string.IsNullOrEmpty(input) == false)
            {
                var tmpDate = Convert.ToDateTime(input);

                CultureInfo ci = new CultureInfo("en-GB");

                var year = tmpDate.ToString("yyyy", ci);
                var month = tmpDate.ToString("MMM", ci);
                var date = tmpDate.Day;

                return date + "-" + month + "-" + year;
            }
            return input;
        }

        public static decimal CheckHowOldIs(string input)
        {
            input = input = input.Replace("<p>Основана:<span>", "").Replace("</span></p>", "");
            if (string.IsNullOrEmpty(input) == false)
            {
                var tmpDate = Convert.ToDateTime(input);
                var curDate = DateTime.Now;


                int days = (curDate - tmpDate).Days;

                //assume 365.25 days per year
                decimal years = days / 365.25m;


                return years;
            }
            return 0;
        }


        public static string OpisNaLikvidnost(double finansiska_procenka, double brz_pokazatel, double prosecni_denovi, double kreditna_izlozenost)
        {

            var fp = finansiska_procenka.ToString("").Replace(".", "");
            var bp = brz_pokazatel.ToString("").Replace(".", "");
            var pd = prosecni_denovi.ToString("").Replace(".", "");
            var ki = kreditna_izlozenost.ToString("").Replace(".", "");
            // zadovolitelno
            var tmpRes = fp + bp + pd + ki;
            switch (tmpRes) {
                case "3333":
                case "3332":
                case "3323":
                case "3322":
                case "3233":
                case "3232":
                case "3223":
                case "3222":
                case "2333":
                case "2332":
                case "2323":
                case "2233":
                    // nezadovolitelno
                    return "Задоволително";
                    break;
                case "3311":
                case "3331":
                case "2322":
                case "2232":
                case "2223":
                case "2222":
                case "2221":
                case "3313":
                case "3133":
                case "2212":
                case "2313":
                case "2312":
                case "3231":
                case "2123":
                case "2213":
                case "3312":
                    //prosecno
                    return "Просечно";
                    break;
                case "1111":
                case "1333":
                case "1133":
                case "1113":
                case "1222":
                case "1122":
                case "1112":
                case "3113":
                case "2211":
                case "2111":
                case "3111":
                case "3131":
                case "1313":
                case "2112":
                case "2122":
                case "2121":
                case "1212":
                case "3213":
                case "3212":
                case "3211":
                case "2132":
                case "2131":
                case "2113":
                case "3112":
                case "3221":
                case "3121":
                case "3122":
                case "3123":
                case "2311":
                case "1321":
                case "1322":
                case "1323":
                case "1231":
                case "1232":
                case "1233":
                case "2331":
                case "1223":
                case "1332":
                case "3321":
                case "1123":
                case "1132":
                case "2231":
                case "1121":
                case "2321":
                case "1312":
                case "1211":
                case "1131":
                case "1311":
                case "3132":
                case "1221":
                case "1331":
                    return "Незадоволително";
                    break;
                default:
                    return "Незадоволително";
            }
            return "";
        }

        public static bool CheckIfValueAvailable(string param){
            if(param == "/")
                return false;
            return true;
        }
    }



}
