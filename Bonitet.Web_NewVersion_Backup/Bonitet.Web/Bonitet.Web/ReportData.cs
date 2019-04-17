using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bonitet.Web
{
	public class ReportData
	{
		public static string AbsoluteUrlPath = "http://localhost:9000";

		public static string TestData()
		{
			var data = new Attributes();

			data.ime_firma = "test";
			data.drzava = "Makedonija";
			data.datum_izdavanje = "24-Dec-2014";
			data.izdaden_za = "Test";

			data.uplaten_del = "557.800,00 ЕУР";
			data.neparicen_vlog = "57.800,00 ЕУР";

			data.tekovni_sopstvenici = "4";
			data.tekovni_podruznici = "2";
			data.tekovni_blokadi_status = "Нема блокада";

			data.sostojba_komentar = "Компанијата има задоволителни ликвидносни индикатори. Работниот капитал на компанијата во 2013 е значително зголемен што ја прави и самата компанија по стабилна и по ликвидна. Компанијата работи со над просечна ефикасност и остварува позитивен раст.";
			data.semafor_solventnost = "green";
			data.solventnost_komentar = "Во нашата база не се евидентирани активности кои можат да влијаат на солвентноста на компанијата.";

			data.promeni_minata_godina = "3";
			data.solventnost_minata_godina = "0";

			data.kazni_minata_godina = "0";
			data.sankcii_minata_godina = "0";

			data.naziv_firma = "Виталиа ДОО Скопје";
			data.celosen_naziv_firma = "Друштво за производство и промет со здрава храна Виталиа Никола и др. ДОО експорт-импорт Скопје";
			data.adresa_firma = "516 10 Гази Баба,Скопје";
			data.pravna_forma = "Друштво со ограничена одговорност  (ДОО)";
			data.dejnost = "10.89-Производство на останати прехранбени производи неспоменати на друго место";
			data.embs = "5130123";
			data.edb = "MK4030996114920";
			data.golemina_na_subjekt = "Среден";
			data.vkupna_osnovna_glavnina = "557.800,00 ЕУР";
			data.datum_osnovanje = "05.06.1996";
			data.registrirana_vo = "Скопје";
			data.prosecen_broj_vraboteni = "138";
			data.region = "Скопје";
			data.kapital = "Домашен";
			data.ddv_obvrznik = "ДА";

			data.ovlasteno_lice1 = "Никола Ветаџокоски";
			data.ovlasteno_lice1_pozicija = "Основач/Сопственик";

			data.ovlasteno_lice2 = "Искра Ветаџокоска-Муцунска";
			data.ovlasteno_lice2_pozicija = "Основач/Сопственик, Управител";
			data.ovlasteno_lice2_ovlastuvanja = "Овластувања:Управител-занимање:машински инжинер";
			data.ovlasteno_lice2_tip_ovlastuvanja = "-Неограничени овластувања во внатрешниот и надворешниот промет";
			data.ovlasteno_lice2_ovlastuvanja2 = "Управител";

			data.ovlasteno_lice3 = "Димитар Ветаџока";
			data.ovlasteno_lice3_pozicija = "Основач/Сопственик, Управител";
			data.ovlasteno_lice3_ovlastuvanja = "Овластувања:Управител-занимање:ВСС";
			data.ovlasteno_lice3_tip_ovlastuvanja = "-Неограничени овластувања во внатрешниот и надворешниот промет";
			data.ovlasteno_lice3_ovlastuvanja2 = "Управител";

			data.banka_smetka1 = "200-0007998160-14";
			data.banka_ime1 = "Стопанска Банка АД Скопје";
			data.banka_smetka2 = "240-0100014253-95";
			data.banka_ime2 = "УНИ Банка";
			data.banka_smetka3 = "530-0101016495-31";
			data.banka_ime3 = "Охридска Банка АД Охрид";
			data.banka_smetka4 = "210-0513012301-91";
			data.banka_ime4 = "НЛБ Тутунска Банка АД Скопје";
			data.banka_smetka5 = "250-1010002822-45";
			data.banka_ime5 = "Шпаркасе Банка Македонија АД Скопје";
			data.banka_smetka6 = "270-0513012301-98";
			data.banka_ime6 = "ХАЛКБАНК АД скопје";
			data.banka_smetka7 = "280-1081019428-12";
			data.banka_ime7 = "Алфа банка АД Скопје";
			data.banka_smetka8 = "300-0000033144-29";
			data.banka_ime8 = "Комерцијална Банка АД Скопје";
			data.banka_smetka9 = "380-1196926001-20";
			data.banka_ime9 = "Procredit Bank";

			data.vkupen_broj_sopstvenici = "4";
			data.prikazan_broj_sopstvenici = "4";

			data.vkupen_broj_ovlasteni = "2";
			data.prikazan_broj_ovlasteni = "2";

			data.prikazan_broj_podruznici = "2";
			data.vkupen_broj_podruznici = "2";

			data.kategorija1 = "2012";
			data.sredstva1 = "107,195,238";
			data.kapital1 = "196,894,871";
			data.vkupno_prihodi1 = "374,058,913";
			data.neto_dobivka_za_delovna_godina1 = "50,842,853";
			data.prosecen_broj_vraboteni1 = "115";
			data.koeficient_na_zadolzensot1 = "0.24";
			data.tekoven_pokazatel1 = "2.45";
			data.povrat_na_sredstva1 = "20%";
			data.povrat_na_kapital1 = "26%";

			data.kategorija2 = "2013";
			data.sredstva2 = "103,748,764";
			data.kapital2 = "246,359,528";
			data.vkupno_prihodi2 = "545,781,0068";
			data.neto_dobivka_za_delovna_godina2 = "91,426,843";
			data.prosecen_broj_vraboteni2 = "138";
			data.koeficient_na_zadolzensot2 = "0.22";
			data.tekoven_pokazatel2 = "3.03";
			data.povrat_na_sredstva2 = "29%";
			data.povrat_na_kapital2 = "37%";

			data.podbroj1 = "5130123/2";
			data.celosen_naziv_firma2 = "Друштво за производство и промет со здрава храна ВИТАЛИА Никола и др.ДОО експорт-импорт Скопје Подружница 1 КЕРАМИДНИЦА Скопје";
			data.tip1 = "Подружница";
			data.podtip1 = "Подружница";
			data.prioritetna_dejnost1 = "47.11-Трговија на мало во неспецијализирани продавници претежно со храна,пијалаци и тутун";
			data.adresa1 = "516 10 ГАЗИ БАБА";
			data.podbroj2 = "5130123/3";
			data.celosen_naziv_firma3 = "Друштво за производство и промет со здрава храна ВИТАЛИА Никола и др.ДОО експорт-импорт Скопје Подружница 2 ДРЖАВНА БОЛНИЦА Скопје";
			data.tip2 = "Подружница";
			data.podtip1 = "Подружница";
			data.prioritetna_dejnost2 = "47.11-Трговија на мало во неспецијализирани продавници претежно со храна,пијалаци и тутун";
			data.adresa2 = "50-ТА Дивизија 16 Центар";


			data.finansiska_procenka_komentar = "Компанијата има задоволителни ликвидносни индикатори. Работниот капитал на компанијата во 2013 е значително зголемен што ја прави и самата компанија по стабилна и по ликвидна. Компанијата работи со над просечна ефикасност и остварува позитивен раст, со нето добивка од 91,4 милиони денари во 2013, односно 79,8% раст во споредба со 2012.";

			data.likvidnost_koeficient_za_zadolzenost = "0.22";
			data.likvidnost_brz_pokazatel = "2.24";
			data.likvidnost_prosecni_denovi_na_plakanje_ovrski = "49";
			data.likvidnost_kreditna_izlozenost_od_rabotenje = "14%";
			data.likvidnost_opis = "Задоволително";

			data.efikasnost_povrat_na_sredstva = "29%";
			data.efikasnost_neto_profitna_marza = "18%";
			data.efikasnost_opis = "Задоволително";

			data.profitabilnost_stavka1 = "2013";
			data.profitabilnost_bruto_operativna_dobivka1 = "143,640,930";
			data.profitabilnost_neto_dobivka_od_finansiranje1 = "554,064";
			data.profitabilnost_ebitda1 = "103,173,021";
			data.profitabilnost_ebit1 = "91,349,888";

			data.profitabilnost_stavka2 = "2012";
			data.profitabilnost_bruto_operativna_dobivka2 = "116,575,851";
			data.profitabilnost_neto_dobivka_od_finansiranje2 = "-1,698,620";
			data.profitabilnost_ebitda2 = "63,112,486";
			data.profitabilnost_ebit2 = "53,360,664";

			data.istoriski_promeni_datum1 = "30-Jul-13";
			data.istoriski_promeni_vid1 = "Сопственик";
			data.istoriski_promeni_opis1 = "Европска банка за обнова и развој е избришана како сопственик на компанијата";

			data.istoriski_promeni_datum2 = "30-Jul-13";
			data.istoriski_promeni_vid2 = "Сопственик";
			data.istoriski_promeni_opis2 = "Друштво за производство и промет со здрава храна Виталиа Никола и др. ДОО експорт-импорт Скопје е назначено за сопственик на компанијата";

			data.istoriski_promeni_datum3 = "27- Feb-14";
			data.istoriski_promeni_vid3 = "Oсновна главнина";
			data.istoriski_promeni_opis3 = "Одлука за намалување на основната главнина на компанијата за износ од 500.000,00 ЕУР";

			data.solventnost_datum1 = "14-Oct-14";
			data.solventnost_opis1 = "Нема евидентирани промени во солвентноста на компанијата";
			
			data.kazni_datum1 = "14-Oct-14";
			data.kazni_opis1 = "Нема евидентирано казни во последните 365 дена";

			data.sankcii_datum1 = "14-Oct-14";
			data.sankcii_opis1 = "Нема евидентирано казни во последните 365 дена";

			data.blokadi_datum1 = "14-Oct-14";
			data.blokadi_opis1 = "Нема блокади";

			data.bilans_netekovni_sredstva = "A";
			data.bilans_netekovni_2013 = "103,748,764";
			data.bilans_netekovni_2012 = "107,195,238";
			data.bilans_netekovni_ind = "96.8";
			data.bilans_netekovni_2013_procent = "32.8";
			data.bilans_netekovni_2012_procent = "41.4";

			data.bilans_nematerijalni_sredstva = "1";
			data.bilans_nematerijalni_2013 = "3,534,841";
			data.bilans_nematerijalni_2012 = "2,682,830";
			data.bilans_nematerijalni_ind = "131.8";
			data.bilans_nematerijalni_2013_procent = "1.1";
			data.bilans_nematerijalni_2012_procent = "1.0";

			data.bilans_materijalni_sredstva = "2";
			data.bilans_materijalni_2013 = "100,213,923";
			data.bilans_materijalni_2012 = "104,512,408";
			data.bilans_materijalni_ind = "95.9";
			data.bilans_materijalni_2013_procent = "31.7";
			data.bilans_materijalni_2012_procent = "40.4";

			data.bilans_vlozuvanje_nedviznosti_sredstva = "3";
			data.bilans_vlozuvanje_nedviznosti_2013 = "0";
			data.bilans_vlozuvanje_nedviznosti_2012 = "0";
			data.bilans_vlozuvanje_nedviznosti_ind = "0.0";
			data.bilans_vlozuvanje_nedviznosti_2013_procent = "0.0";
			data.bilans_vlozuvanje_nedviznosti_2012_procent = "0.0";

			data.bilans_dolgorocni_sredstva_sredstva = "4";
			data.bilans_dolgorocni_sredstva_2013 = "0";
			data.bilans_dolgorocni_sredstva_2012 = "0";
			data.bilans_dolgorocni_sredstva_ind = "0.0";
			data.bilans_dolgorocni_sredstva_2013_procent = "0.0";
			data.bilans_dolgorocni_sredstva_2012_procent = "0.0";

			data.bilans_dolgorocni_pobaruvanja_sredstva = "5";
			data.bilans_dolgorocni_pobaruvanja_2013 = "0";
			data.bilans_dolgorocni_pobaruvanja_2012 = "0";
			data.bilans_dolgorocni_pobaruvanja_ind = "0.0";
			data.bilans_dolgorocni_pobaruvanja_2013_procent = "0.0";
			data.bilans_dolgorocni_pobaruvanja_2012_procent = "0.0";

			data.bilans_odlozeni_danocni_sredstva_sredstva = "B";
			data.bilans_odlozeni_danocni_sredstva_2013 = "0";
			data.bilans_odlozeni_danocni_sredstva_2012 = "0";
			data.bilans_odlozeni_danocni_sredstva_ind = "0.0";
			data.bilans_odlozeni_danocni_sredstva_2013_procent = "0.0";
			data.bilans_odlozeni_danocni_sredstva_2012_procent = "0.0";

			data.bilans_tekovni_sredstva_sredstva = "C";
			data.bilans_tekovni_sredstva_2013 = "212,780,421";
			data.bilans_tekovni_sredstva_2012 = "151,659,939";
			data.bilans_tekovni_sredstva_ind = "140.3";
			data.bilans_tekovni_sredstva_2013_procent = "67.2";
			data.bilans_tekovni_sredstva_2012_procent = "58.6";

			data.bilans_zalihi_sredstva = "1";
			data.bilans_zalihi_2013 = "55,237,840";
			data.bilans_zalihi_2012 = "29,409,100";
			data.bilans_zalihi_ind = "187.8";
			data.bilans_zalihi_2013_procent = "17.4";
			data.bilans_zalihi_2012_procent = "11.4";

			data.bilans_kratkorocni_pobaruvanja_sredstva = "2";
			data.bilans_kratkorocni_pobaruvanja_2013 = "100,139,764";
			data.bilans_kratkorocni_pobaruvanja_2012 = "69,272,000";
			data.bilans_kratkorocni_pobaruvanja_ind = "144.6";
			data.bilans_kratkorocni_pobaruvanja_2013_procent = "31.6";
			data.bilans_kratkorocni_pobaruvanja_2012_procent = "26.8";

			data.bilans_kratkorocni_sredstva_sredstva = "3";
			data.bilans_kratkorocni_sredstva_2013 = "28,373,901";
			data.bilans_kratkorocni_sredstva_2012 = "34,911,784";
			data.bilans_kratkorocni_sredstva_ind = "81.3";
			data.bilans_kratkorocni_sredstva_2013_procent = "9.0";
			data.bilans_kratkorocni_sredstva_2012_procent = "13.5";

			data.bilans_paricni_sredstva_sredstva = "4";
			data.bilans_paricni_sredstva_2013 = "29,028,916";
			data.bilans_paricni_sredstva_2012 = "18,067,055";
			data.bilans_paricni_sredstva_ind = "160.7";
			data.bilans_paricni_sredstva_2013_procent = "9.2";
			data.bilans_paricni_sredstva_2012_procent = "7.0";

			data.bilans_sredstva_grupa_sredstva = "D";
			data.bilans_sredstva_grupa_2013 = "0";
			data.bilans_sredstva_grupa_2012 = "0";
			data.bilans_sredstva_grupa_ind = "0.0";
			data.bilans_sredstva_grupa_2013_procent = "0.0";
			data.bilans_sredstva_grupa_2012_procent = "0.0";

			data.bilans_plateni_trosoci_sredstva = "E";
			data.bilans_plateni_trosoci_2013 = "97,355";
			data.bilans_plateni_trosoci_2012 = "0";
			data.bilans_plateni_trosoci_ind = "0.0";
			data.bilans_plateni_trosoci_2013_procent = "0.0";
			data.bilans_plateni_trosoci_2012_procent = "0.0";

			data.bilans_vkupna_aktiva_sredstva = "";
			data.bilans_vkupna_aktiva_2013 = "316,626,540";
			data.bilans_vkupna_aktiva_2012 = "258,855,177";
			data.bilans_vkupna_aktiva_ind = "122.3";
			data.bilans_vkupna_aktiva_2013_procent = "100.0";
			data.bilans_vkupna_aktiva_2012_procent = "100.";

			data.bilans_glavnina_i_rezervi_sredstva = "A";
			data.bilans_glavnina_i_rezervi_2013 = "246,359,528";
			data.bilans_glavnina_i_rezervi_2012 = "196,894,871";
			data.bilans_glavnina_i_rezervi_ind = "125.1";
			data.bilans_glavnina_i_rezervi_2013_procent = "77.8";
			data.bilans_glavnina_i_rezervi_2012_procent = "76.1";

			data.bilans_osnovna_glavnina_sredstva = "1";
			data.bilans_osnovna_glavnina_2013 = "34,231,850";
			data.bilans_osnovna_glavnina_2012 = "34,231,850";
			data.bilans_osnovna_glavnina_ind = "100.";
			data.bilans_osnovna_glavnina_2013_procent = "10.8";
			data.bilans_osnovna_glavnina_2012_procent = "13.2";

			data.bilans_premii_akcii_sredstva = "2";
			data.bilans_premii_akcii_2013 = "0";
			data.bilans_premii_akcii_2012 = "0";
			data.bilans_premii_akcii_ind = "0.0";
			data.bilans_premii_akcii_2013_procent = "0.0";
			data.bilans_premii_akcii_2012_procent = "0.0";

			data.bilans_sopstveni_akcii_sredstva = "3";
			data.bilans_sopstveni_akcii_2013 = "0";
			data.bilans_sopstveni_akcii_2012 = "0";
			data.bilans_sopstveni_akcii_ind = "0.0";
			data.bilans_sopstveni_akcii_2013_procent = "0.0";
			data.bilans_sopstveni_akcii_2012_procent = "0.0";

			data.bilans_zapisan_kapital_sredstva = "4";
			data.bilans_zapisan_kapital_2013 = "0";
			data.bilans_zapisan_kapital_2012 = "0";
			data.bilans_zapisan_kapital_ind = "0.0";
			data.bilans_zapisan_kapital_2013_procent = "0.0";
			data.bilans_zapisan_kapital_2012_procent = "0.0";

			data.bilans_revalorizaciska_rezerva_sredstva = "5";
			data.bilans_revalorizaciska_rezerva_2013 = "0";
			data.bilans_revalorizaciska_rezerva_2012 = "0";
			data.bilans_revalorizaciska_rezerva_ind = "0.0";
			data.bilans_revalorizaciska_rezerva_2013_procent = "0.0";
			data.bilans_revalorizaciska_rezerva_2012_procent = "0.0";

			data.bilans_rezervi_sredstva = "6";
			data.bilans_rezervi_2013 = "21,456,523";
			data.bilans_rezervi_2012 = "52,162,972";
			data.bilans_rezervi_ind = "41.1";
			data.bilans_rezervi_2013_procent = "6.8";
			data.bilans_rezervi_2012_procent = "20.2";

			data.bilans_akumulirana_dobivka_sredstva = "7";
			data.bilans_akumulirana_dobivka_2013 = "99,244,312";
			data.bilans_akumulirana_dobivka_2012 = "59,657,196";
			data.bilans_akumulirana_dobivka_ind = "166.4";
			data.bilans_akumulirana_dobivka_2013_procent = "31.3";
			data.bilans_akumulirana_dobivka_2012_procent = "23.0";

			data.bilans_prenesena_zaguba_sredstva = "8";
			data.bilans_prenesena_zaguba_2013 = "0";
			data.bilans_prenesena_zaguba_2012 = "0";
			data.bilans_prenesena_zaguba_ind = "0.0";
			data.bilans_prenesena_zaguba_2013_procent = "0.0";
			data.bilans_prenesena_zaguba_2012_procent = "0.0";

			data.bilans_dobivka_delovna_godina_sredstva = "9";
			data.bilans_dobivka_delovna_godina_2013 = "91,426,843";
			data.bilans_dobivka_delovna_godina_2012 = "50,842,853";
			data.bilans_dobivka_delovna_godina_ind = "179.8";
			data.bilans_dobivka_delovna_godina_2013_procent = "28.9";
			data.bilans_dobivka_delovna_godina_2012_procent = "19.6";

			data.bilans_zaguba_delovna_godina_sredstva = "10";
			data.bilans_zaguba_delovna_godina_2013 = "0";
			data.bilans_zaguba_delovna_godina_2012 = "0";
			data.bilans_zaguba_delovna_godina_ind = "0.0";
			data.bilans_zaguba_delovna_godina_2013_procent = "0.0";
			data.bilans_zaguba_delovna_godina_2012_procent = "0.0";

			data.bilans_obvrski_sredstva = "B";
			data.bilans_obvrski_2013 = "70,267,012";
			data.bilans_obvrski_2012 = "61,960,306";
			data.bilans_obvrski_ind = "113.4";
			data.bilans_obvrski_2013_procent = "22.2";
			data.bilans_obvrski_2012_procent = "23.9";

			data.bilans_dolgorocni_rezerviranja_sredstva = "1";
			data.bilans_dolgorocni_rezerviranja_2013 = "0";
			data.bilans_dolgorocni_rezerviranja_2012 = "0";
			data.bilans_dolgorocni_rezerviranja_ind = "0.0";
			data.bilans_dolgorocni_rezerviranja_2013_procent = "0.0";
			data.bilans_dolgorocni_rezerviranja_2012_procent = "0.0";
  
			data.bilans_dolgorocni_obvrski_sredstva = "2";
			data.bilans_dolgorocni_obvrski_2013 = "0";
			data.bilans_dolgorocni_obvrski_2012 = "0";
			data.bilans_dolgorocni_obvrski_ind = "0.0";
			data.bilans_dolgorocni_obvrski_2013_procent = "0.0";
			data.bilans_dolgorocni_obvrski_2012_procent = "0.0";

			data.bilans_kratkorocni_obvrski_sredstva = "3";
			data.bilans_kratkorocni_obvrski_2013 = "70,267,012";
			data.bilans_kratkorocni_obvrski_2012 = "61,960,306";
			data.bilans_kratkorocni_obvrski_ind = "113.4";
			data.bilans_kratkorocni_obvrski_2013_procent = "22.2";
			data.bilans_kratkorocni_obvrski_2012_procent = "23.9";
  
			data.bilans_odlozeni_obvrski_sredstva = "C";
			data.bilans_odlozeni_obvrski_2013 = "0";
			data.bilans_odlozeni_obvrski_2012 = "0";
			data.bilans_odlozeni_obvrski_ind = "0.0";
			data.bilans_odlozeni_obvrski_2013_procent = "0.0";
			data.bilans_odlozeni_obvrski_2012_procent = "0.0";

			data.bilans_odlozeno_plakanje_sredstva = "D";
			data.bilans_odlozeno_plakanje_2013 = "0";
			data.bilans_odlozeno_plakanje_2012 = "0";
			data.bilans_odlozeno_plakanje_ind = "0.0";
			data.bilans_odlozeno_plakanje_2013_procent = "0.0";
			data.bilans_odlozeno_plakanje_2012_procent = "0.0";

			data.bilans_obvrski_po_osnov_sredstva = "E";
			data.bilans_obvrski_po_osnov_2013 = "0";
			data.bilans_obvrski_po_osnov_2012 = "0";
			data.bilans_obvrski_po_osnov_ind = "0.0";
			data.bilans_obvrski_po_osnov_2013_procent = "0.0";
			data.bilans_obvrski_po_osnov_2012_procent = "0.0";

			data.bilans_vkupna_pasiva_sredstva = "";
			data.bilans_vkupna_pasiva_2013 = "316,626,540";
			data.bilans_vkupna_pasiva_2012 = "258,855,177";
			data.bilans_vkupna_pasiva_ind = "122.3";
			data.bilans_vkupna_pasiva_2013_procent = "100.0";
			data.bilans_vkupna_pasiva_2012_procent = "100.0";
			

			data.uspeh_prihodi_rabotenje_sredstva = "1";
			data.uspeh_prihodi_rabotenje_2013 = "544,472,882";
			data.uspeh_prihodi_rabotenje_2012 = "373,688,874";
			data.uspeh_prihodi_rabotenje_ind = "145.7";
			data.uspeh_prihodi_rabotenje_2013_procent = "99.8";
			data.uspeh_prihodi_rabotenje_2012_procent = "99.9";

			data.uspeh_finansiski_prihodi_sredstva = "2";
			data.uspeh_finansiski_prihodi_2013 = "1,308,124";
			data.uspeh_finansiski_prihodi_2012 = "370,039";
			data.uspeh_finansiski_prihodi_ind = "353.5";
			data.uspeh_finansiski_prihodi_2013_procent = "0.2";
			data.uspeh_finansiski_prihodi_2012_procent = "0.1";

			data.uspeh_vkupno_prihodi_sredstva = "";
			data.uspeh_vkupno_prihodi_2013 = "545,781,006";
			data.uspeh_vkupno_prihodi_2012 = "374,058,913";
			data.uspeh_vkupno_prihodi_ind = "145.9";
			data.uspeh_vkupno_prihodi_2013_procent = "100.0";
			data.uspeh_vkupno_prihodi_2012_procent = "100.0";

			data.uspeh_rashodi_rabotenje_sredstva = "1";
			data.uspeh_rashodi_rabotenje_2013 = "453,122,994";
			data.uspeh_rashodi_rabotenje_2012 = "320,328,210";
			data.uspeh_rashodi_rabotenje_ind = "141.5";
			data.uspeh_rashodi_rabotenje_2013_procent = "99.8";
			data.uspeh_rashodi_rabotenje_2012_procent = "99.4";

			data.uspeh_rashod_osnovna_dejnost_sredstva = "";
			data.uspeh_rashod_osnovna_dejnost_2013 = "366,611,031";
			data.uspeh_rashod_osnovna_dejnost_2012 = "252,685,383";
			data.uspeh_rashod_osnovna_dejnost_ind = "145.1";
			data.uspeh_rashod_osnovna_dejnost_2013_procent = "80.8";
			data.uspeh_rashod_osnovna_dejnost_2012_procent = "78.4";

			data.uspeh_ostanati_trosoci_sredstva = "";
			data.uspeh_ostanati_trosoci_2013 = "14,892,682";
			data.uspeh_ostanati_trosoci_2012 = "6,790,661";
			data.uspeh_ostanati_trosoci_ind = "219.3";
			data.uspeh_ostanati_trosoci_2013_procent = "3.3";
			data.uspeh_ostanati_trosoci_2012_procent = "2.1";

			data.uspeh_trosoci_za_vraboteni_sredstva = "";
			data.uspeh_trosoci_za_vraboteni_2013 = "43,381,372";
			data.uspeh_trosoci_za_vraboteni_2012 = "34,640,288";
			data.uspeh_trosoci_za_vraboteni_ind = "125.2";
			data.uspeh_trosoci_za_vraboteni_2013_procent = "9.6";
			data.uspeh_trosoci_za_vraboteni_2012_procent = "10.7";

			data.uspeh_amortizacija_sredstva_sredstva = "";
			data.uspeh_amortizacija_sredstva_2013 = "15,860,709";
			data.uspeh_amortizacija_sredstva_2012 = "21,608,496";
			data.uspeh_amortizacija_sredstva_ind = "73.4";
			data.uspeh_amortizacija_sredstva_2013_procent = "3.5";
			data.uspeh_amortizacija_sredstva_2012_procent = "6.7";

			data.uspeh_rezerviranje_trosoci_rizici_sredstva = "";
			data.uspeh_rezerviranje_trosoci_rizici_2013 = "0";
			data.uspeh_rezerviranje_trosoci_rizici_2012 = "0";
			data.uspeh_rezerviranje_trosoci_rizici_ind = "0.0";
			data.uspeh_rezerviranje_trosoci_rizici_2013_procent = "0.0";
			data.uspeh_rezerviranje_trosoci_rizici_2012_procent = "0.0";

			data.uspeh_zalihi_proizvodi_pocetok_sredstva = "";
			data.uspeh_zalihi_proizvodi_pocetok_2013 = "11,211,410";
			data.uspeh_zalihi_proizvodi_pocetok_2012 = "6,847,438";
			data.uspeh_zalihi_proizvodi_pocetok_ind = "163.7";
			data.uspeh_zalihi_proizvodi_pocetok_2013_procent = "2.5";
			data.uspeh_zalihi_proizvodi_pocetok_2012_procent = "2.1";
 
			data.uspeh_zalihi_proizvodi_kraj_sredstva = "";
			data.uspeh_zalihi_proizvodi_kraj_2013 = "11,570,772";
			data.uspeh_zalihi_proizvodi_kraj_2012 = "11,211,411";
			data.uspeh_zalihi_proizvodi_kraj_ind = "103.2";
			data.uspeh_zalihi_proizvodi_kraj_2013_procent = "2.5";
			data.uspeh_zalihi_proizvodi_kraj_2012_procent = "3.5";

			data.uspeh_ostanati_rashodi_sredstva = "";
			data.uspeh_ostanati_rashodi_2013 = "12,736,562";
			data.uspeh_ostanati_rashodi_2012 = "8,967,355";
			data.uspeh_ostanati_rashodi_ind = "142.0";
			data.uspeh_ostanati_rashodi_2013_procent = "2.8";
			data.uspeh_ostanati_rashodi_2012_procent = "2.8";

			data.uspeh_finansiski_rashodi_sredstva = "2";
			data.uspeh_finansiski_rashodi_2013 = "754,060";
			data.uspeh_finansiski_rashodi_2012 = "2,068,659";
			data.uspeh_finansiski_rashodi_ind = "36.5";
			data.uspeh_finansiski_rashodi_2013_procent = "0.2";
			data.uspeh_finansiski_rashodi_2012_procent = "0.6";

			data.uspeh_finansiski_povrzani_drustva_sredstva = "";
			data.uspeh_finansiski_povrzani_drustva_2013 = "0";
			data.uspeh_finansiski_povrzani_drustva_2012 = "0";
			data.uspeh_finansiski_povrzani_drustva_ind = "0.0";
			data.uspeh_finansiski_povrzani_drustva_2013_procent = "0.0";
			data.uspeh_finansiski_povrzani_drustva_2012_procent = "0.0";

			data.uspeh_rashodi_kamati_sredstva = "";
			data.uspeh_rashodi_kamati_2013 = "316,626,540";
			data.uspeh_rashodi_kamati_2012 = "258,855,177";
			data.uspeh_rashodi_kamati_ind = "122.3";
			data.uspeh_rashodi_kamati_2013_procent = "100.0";
			data.uspeh_rashodi_kamati_2012_procent = "100.0";

			data.uspeh_rashodi_finansiski_sredstva_sredstva = "";
			data.uspeh_rashodi_finansiski_sredstva_2013 = "246,359,528";
			data.uspeh_rashodi_finansiski_sredstva_2012 = "196,894,871";
			data.uspeh_rashodi_finansiski_sredstva_ind = "125.1";
			data.uspeh_rashodi_finansiski_sredstva_2013_procent = "77.8";
			data.uspeh_rashodi_finansiski_sredstva_2012_procent = "76.1";

			data.uspeh_ostanati_finansiski_rashodi_sredstva = "";
			data.uspeh_ostanati_finansiski_rashodi_2013 = "34,231,850";
			data.uspeh_ostanati_finansiski_rashodi_2012 = "34,231,850";
			data.uspeh_ostanati_finansiski_rashodi_ind = "100.0";
			data.uspeh_ostanati_finansiski_rashodi_2013_procent = "10.8";
			data.uspeh_ostanati_finansiski_rashodi_2012_procent = "13.2";

			data.uspeh_udel_vo_zaguba_sredstva = "3";
			data.uspeh_udel_vo_zaguba_2013 = "0";
			data.uspeh_udel_vo_zaguba_2012 = "0";
			data.uspeh_udel_vo_zaguba_ind = "0.0";
			data.uspeh_udel_vo_zaguba_2013_procent = "0.0";
			data.uspeh_udel_vo_zaguba_2012_procent = "0.0";

			data.uspeh_vkupno_rashodi_sredstva = "";
			data.uspeh_vkupno_rashodi_2013 = "34,231,850";
			data.uspeh_vkupno_rashodi_2012 = "34,231,850";
			data.uspeh_vkupno_rashodi_ind = "100.0";
			data.uspeh_vkupno_rashodi_2013_procent = "10.8";
			data.uspeh_vkupno_rashodi_2012_procent = "13.2";

			data.uspeh_dobivka_odanocuvanje_sredstva = "1";
			data.uspeh_dobivka_odanocuvanje_2013 = "544,472,882";
			data.uspeh_dobivka_odanocuvanje_2012 = "373,688,874";
			data.uspeh_dobivka_odanocuvanje_ind = "96.8";

			data.uspeh_zaguba_odanocuvanje_sredstva = "2";
			data.uspeh_zaguba_odanocuvanje_2013 = "544,472,882";
			data.uspeh_zaguba_odanocuvanje_2012 = "373,688,874";
			data.uspeh_zaguba_odanocuvanje_ind = "96.8";

			data.uspeh_danok_dobivka_sredstva = "3";
			data.uspeh_danok_dobivka_2013 = "544,472,882";
			data.uspeh_danok_dobivka_2012 = "373,688,874";
			data.uspeh_danok_dobivka_ind = "96.8";

			data.uspeh_neto_dobivka_sredstva = "4";
			data.uspeh_neto_dobivka_2013 = "544,472,882";
			data.uspeh_neto_dobivka_2012 = "373,688,874";
			data.uspeh_neto_dobivka_ind = "96.8";

			data.uspeh_neto_zaguba_sredstva = "5";
			data.uspeh_neto_zaguba_2013 = "544,472,882";
			data.uspeh_neto_zaguba_2012 = "373,688,874";
			data.uspeh_neto_zaguba_ind = "96.8";

			data.indikatori_raboten_kapital_2013 = "142,513,409";
			data.indikatori_raboten_kapital_2012 = "89,699,633";
			data.indikatori_raboten_kapital_rast = "52,813,776 (0.59)";

			data.indikatori_tekoven_pokazatel_2013 = "3.03";
			data.indikatori_tekoven_pokazatel_2012 = "2.45";
			data.indikatori_tekoven_pokazatel_rast = "0.58 (0.24)";

			data.indikatori_brz_pokazatel_2013 = "2.24";
			data.indikatori_brz_pokazatel_2012 = "1.97";
			data.indikatori_brz_pokazatel_rast = "0.27 (0.14)";
 
			data.indikatori_obrt_sredstva_2013 = "177.33%";
			data.indikatori_obrt_sredstva_2012 = "143%";
			data.indikatori_obrt_sredstva_rast = "0.67 (0.11)";

			data.indikatori_denovi_obrt_sredstva_2013 = "206";
			data.indikatori_denovi_obrt_sredstva_2012 = "256";
			data.indikatori_denovi_obrt_sredstva_rast = "-5.92 (-0.1)";

			data.indikatori_obrt_obvrski_2013 = "7.5";
			data.indikatori_obrt_obvrski_2012 = "6.0";
			data.indikatori_obrt_obvrski_rast = "0.07 (0.01)";

			data.indikatori_prosecni_denovi_obvrski_2013 = "49";
			data.indikatori_prosecni_denovi_obvrski_2012 = "61";
			data.indikatori_prosecni_denovi_obvrski_rast = "-0.34 (-0.01)";

			data.indikatori_obrt_pobaruvanja_2013 = "676.6%";
			data.indikatori_obrt_pobaruvanja_2012 = "609.7%";
			data.indikatori_obrt_pobaruvanja_rast = "0.35 (0.24)";

			data.indikatori_denovi_obrt_pobaruvanja_2013 = "54";
			data.indikatori_denovi_obrt_pobaruvanja_2012 = "60";
			data.indikatori_denovi_obrt_pobaruvanja_rast = "-50.04 (-0.2)";

			data.indikatori_obrt_zalihi_2013 = "866%";
			data.indikatori_obrt_zalihi_2012 = "859%";
			data.indikatori_obrt_zalihi_rast = "1.47 (0.24)";

			data.indikatori_denovi_obrt_zalihi_2013 = "42";
			data.indikatori_denovi_obrt_zalihi_2012 = "42";
			data.indikatori_denovi_obrt_zalihi_rast = "-11.9 (-0.2)";

			data.indikatori_povrat_kapital_2013 = "37.11%";
			data.indikatori_povrat_kapital_2012 = "25.82%";
			data.indikatori_povrat_kapital_rast = "0.11 (0.44)";

			data.indikatori_povrat_sredstva_2013 = "29%";
			data.indikatori_povrat_sredstva_2012 = "20%";
			data.indikatori_povrat_sredstva_rast = "0.09 (0.47)";

			data.indikatori_neto_profitna_margina_2013 = "17.9%";
			data.indikatori_neto_profitna_margina_2012 = "13.8%";
			data.indikatori_neto_profitna_margina_rast = "0.04 (0.3)";

			data.indikatori_finansiski_leviridz_2013 = "0.29";
			data.indikatori_finansiski_leviridz_2012 = "0.31";
			data.indikatori_finansiski_leviridz_rast = "-0.03 (-0.09)";

			data.indikatori_koeficient_zadolzenost_2013 = "0.22";
			data.indikatori_koeficient_zadolzenost_2012 = "0.24";
			data.indikatori_koeficient_zadolzenost_rast = "-0.02 (-0.07)";

			data.indikatori_vkupni_obvrski_2013 = "0.7";
			data.indikatori_vkupni_obvrski_2012 = "1.0";
			data.indikatori_vkupni_obvrski_rast = "-0.3 (-0.31)";

			data.indikatori_pokrienost_servisiranje_2013 = "165.72";
			data.indikatori_pokrienost_servisiranje_2012 = "14.62";
			data.indikatori_pokrienost_servisiranje_rast = "151.1 (10.34)";

			data.indikatori_pokrienost_kamati_2013 = "136.82";
			data.indikatori_pokrienost_kamati_2012 = "30.51";
			data.indikatori_pokrienost_kamati_rast = "106.31 (3.48)";

			data.indikatori_kratkorocni_krediti_2013 = "0%";
			data.indikatori_kratkorocni_krediti_2012 = "2%";
			data.indikatori_kratkorocni_krediti_rast = "-0.02 (-0.98) ";

			data.indikatori_tekovni_obvrski_2013 = "14%";
			data.indikatori_tekovni_obvrski_2012 = "17%";
			data.indikatori_tekovni_obvrski_rast = "-0.03 (-0.18) ";
 

			data.tekovi_neto_profit_odanocuvanje = "91,426,843";
			data.tekovi_zaguba_odanocuvanje = "0";
			data.tekovi_amortizacija = "15,860,709";
			data.tekovi_prilivi_gotovina_aktivnosti = "14,844,589";
			data.tekovi_odlivi_gotovina_aktivnosti = "56,793,859";
			data.tekovi_neto_prilivi_gotovina_aktivnosti = "0";
			data.tekovi_neto_odlivi_gotovina_aktivnosti = "41,949,270";
			data.tekovi_prilivi_gotovina_investicioni = "0";
			data.tekovi_odlivi_gotovina_investicioni = "12,414,235";
			data.tekovi_neto_prilivi_gotovina_investicioni = "0";
			data.tekovi_neto_odlivi_gotovina_investicioni = "12,414,235";
			data.tekovi_prilivi_gotovina_finansiski = "39,587,116";
			data.tekovi_odlivi_gotovina_finansiski = "81,549,302";
			data.tekovi_neto_prilivi_gotovina_finansiski = "0";
			data.tekovi_neto_odlivi_gotovina_finansiski = "41,962,186";
			data.tekovi_vkupno_prilivi_gotovina = "107,287,552";
			data.tekovi_vkupno_odlivi_gotovina = "96,325,691";
			data.tekovi_vkupno_neto_prilivi = "10,961,861";
			data.tekovi_vkupno_neto_odlivi = "0";
			data.tekovi_paricni_sredstva_pocetok = "18,067,055";
			data.tekovi_paricni_sredstva_kraj = "29,028,916";

			var html = ReportData.CoverPage(data);

			html += ReportData.FirstPage(data);
			html += ReportData.SecondPage(data);
			html += ReportData.ThirdPage(data);
			html += ReportData.FourthPage(data);
			html += ReportData.FifthPage(data);
			html += ReportData.SixthPage(data);
			html += ReportData.SeventhPage(data);
			html += ReportData.EighthPage(data);
			html += ReportData.NinthPage(data);
			html += ReportData.TenthPage(data);

			return html;
		}

		private static string Footer(int page)
		{
			string html = "<div class=\"footer\">"+
								"<div>"+
									"<p>© CREDIT REPORT, all rights reserved</p>"+
									"<p>www.targetgroup.mk, tel/fax: +389 (2) 3117 - 100</p>"+
								"</div>"+
								"<div class=\"img_wrapper\">"+
									"<img src=\""+AbsoluteUrlPath+"/img/target_group.png\" />"+
								"</div>"+
								"<div class=\"pagination\">"+
									"<p>" + page + "</p>"+
								"</div>"+
							"</div>";

			return html;
		}

		private static string CoverPage(Attributes data)
		{
			string html = "<!DOCTYPE html>" +
						"<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
						"<head>"+
							"<title></title>"+
							"<style>body{height:auto;width:1040px;padding:0;margin:0 auto;font-family:Calibri}.page{height:1470px;width:1040px;padding:0;margin:0;float:left}#first_page_wrapper{width:100%;height:770px;float:left;background-color:#B9D431}#eigth_page_wrapger,#eleventh_page_wrapper,#fifth_page_wrapper,#fourth_page_wrapper,#ninth_page_wrapper,#second_page_wrapper,#seventh_page_wrapper,#sixth_page_wrapper,#tenth_page_wrapper,#third_page_wrapper{width:100%;height:1200px;float:left}h1{font-size:60px;margin-left:50px;margin-top:250px;padding:0;margin-bottom:0}h2{font-size:36px;display:inline-block;margin-left:50px;color:#444}#cover_wrapper{text-align:left;width:100%;float:left}p.inline_block{display:inline-block;font-size:52px;margin-left:150px;margin-top:0;margin-bottom:0;color:#444}p{font-size:22px;margin-left:50px}p span{font-weight:700}.spacer_100{width:100%;height:100px;float:left}.spacer_200{width:100%;height:200px;float:left}.footer{width:100%;height:125px;background-color:#B9D431;opacity:.7;float:left}.footer .img_wrapper img{padding-top:0}.footer .img_wrapper{float:left;background-color:#fff!important;height:125px;margin-top:0}.footer.cover_footer{height:200px}.footer div{float:left;margin-top:25px}.footer p{padding:0;margin:0 0 0 50px}.footer .pagination{margin:0;padding:0;width:120px;height:125px;text-align:center;float:left}.footer .pagination p{line-height:125px;color:#fff;font-size:35px;margin:0;padding:0}#white_cover{width:100%;height:100px;float:left}#white_cover p{font-weight:700}.footer.cover_footer div{float:left;margin-top:50px}.footer.cover_footer .img_wrapper{float:left;background-color:#fff!important;width:256px;height:200px;margin-left:220px;margin-top:0}.footer.cover_footer p{padding:0;margin:20px 0 0 50px}.footer.cover_footer .img_wrapper img{padding-top:67px}.header{background-color:#F6F6F8;width:100%;height:135px;opacity:.7;border-bottom:5px #000 solid;float:left;position:relative}.float_left{float:left}.float_left h2{font-size:50px;padding:0;margin:10px 0 0 50px;color:#000}.float_left h3{padding:0;margin:0 0 0 50px;color:#000;font-size:20px}.float_left p{margin:0 0 0 50px;font-size:16px}.float_right{float:right}.float_right img{margin-top:10px;margin-right:50px}.left_p{float:left;width:520px}.right_p{float:right;width:520px}.p_wrapper{width:490px;float:left;margin-top:20px}.p_wrapper.left{margin-right:0;margin-left:30px;padding-right:0}.p_wrapper.right{margin-right:30px;margin-left:0;padding-right:0}.p_wrapper.full{width:960px;display:block;margin-left:30px}.p_wrapper.full .p_wrapper_header{width:100%}.p_wrapper .p_wrapper_header{width:470px;float:left;padding-left:20px}.p_wrapper_header h2{margin:0;padding:0;float:left;text-transform:uppercase;color:#000;font-weight:700;font-size:30px}.p_wrapper_header p{margin:0 10px 0 0;padding:0;float:right}.p_wrapper_header p.black{background-color:#000;color:#fff;line-height:30px}.p_wrapper .content{float:left;padding:20px;width:450px;background-color:#fff}.p_wrapper.full .content{width:940px}.p_wrapper .content.gray{background-color:#F2F2F2}.p_wrapper .content p{margin:0;padding:0}.left_p.shorter,.shorter .p_wrapper.left,.shorter .p_wrapper.left .p_wrapper_header{width:340px}.shorter .p_wrapper.left .content{width:300px}.shorter .p_wrapper.left .content p{margin-bottom:40px}.right_p.longer{width:600px}.longer .p_wrapper.right{width:570px}.longer .p_wrapper.right .p_wrapper_header{width:560px}.longer .p_wrapper.right .content{width:520px}.spacer_50_600{width:600px;height:50px;float:left}.semaphore{width:72px;height:67px;float:left;display:inline-block}.semaphore_text{width:370px;float:left;margin-left:8px}.full .semaphore_text{width:600px;float:left;margin-left:200px}.rezultati_table{width:960px;margin:0;padding:0;border-spacing:0}.rezultati_table tr td{border-bottom:1px solid #000;line-height:32px}.rezultati_table .gray td{background-color:#D9D9D9;border-bottom:0}.rezultati_table tr td:nth-child(2),.rezultati_table tr td:nth-child(3){text-align:right;width:30%}.rezultati_table .information td{border-bottom:0}.rezultati_table .information td p{font-size:15px}.p_wrapper .content .gray{background-color:#F2F2F2;margin-left:-20px;padding-left:20px;margin-right:-20px}.p_wrapper .content .border_bottom{border-bottom:1px solid #000;margin-top:10px}.header .absolute_poglavje{position:absolute;bottom:-20px;right:50px;background-color:#7F7F7F}.header .absolute_poglavje p{margin:0;line-height:40px;font-size:20px;color:#fff;padding:0 20px}.finansii_table{width:550px;margin:0;padding:0;border-spacing:0}.longer .p_wrapper_header h2{padding-left:20px}.finansii_table tr td{border-bottom:1px solid #000;line-height:32px}.finansii_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:40px}.finansii_table .gray td:nth-child(1){text-align:left!important;padding-left:20px}.finansii_table .gray td:nth-child(2),.finansii_table .gray td:nth-child(3){text-align:center!important}.finansii_table tr td:nth-child(1){text-align:right}.finansii_table tr td:nth-child(2),.finansii_table tr td:nth-child(3){text-align:center;width:30%}.finansii_table .information td{border-bottom:0}.finansii_table .information td p{font-size:15px}.promeni_table{width:100%;margin:0;padding:0;border-spacing:0}.promeni_table tr td{border-bottom:1px solid #000;line-height:32px}.promeni_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:60px}.promeni_table tr td:nth-child(1){text-align:center}.promeni_table tr td{text-align:left}.promeni_table .information td{border-bottom:0}.promeni_table .information td p{font-size:15px}.promeni_table tr td:nth-child(1){width:5%}.promeni_table tr td:nth-child(2),.promeni_table tr td:nth-child(3){width:20%}.kazni_table{width:100%;margin:0;padding:0;border-spacing:0}.kazni_table tr td{border-bottom:1px solid #000;line-height:32px}.kazni_table .gray td{background-color:#D9D9D9;border-bottom:0;line-height:60px}.kazni_table tr td:nth-child(1){text-align:center}.kazni_table tr td{text-align:left}.kazni_table .information td{border-bottom:0}.kazni_table .information td p{font-size:15px}.kazni_table tr td:nth-child(1){width:5%}.kazni_table tr td:nth-child(2),.kazni_table tr td:nth-child(3){width:20%}.bilans_table{width:100%;margin:0;padding:0;border-spacing:0}.bilans_table tr td{line-height:20px;padding-left:20px}.bilans_table tr.has_border td{border-bottom:1px solid #000}.bilans_table tr td p{font-size:16px}.bilans_table .gray td{background-color:#D9D9D9;border-bottom:0}.bilans_table .gray.text_right td{text-align:right;padding-right:20px}.bilans_table .information td{border-bottom:0}.bilans_table .information td p{font-size:15px}.bilans_table .margin_top td{height:40px;vertical-align:bottom}.bilans_table.margin_top{margin-top:40px;width:850px}</style>" +
						"</head>"+
						"<body>"+
							"<div class=\"page\">"+
								"<div class=\"spacer_200\"></div>"+
								"<div id=\"first_page_wrapper\">"+
									"<h1>Бонитетен извештај</h1>"+
									"<div id=\"cover_wrapper\">"+
										"<h2>" + data.ime_firma + "</h2>"+
										"<p class=\"inline_block\">[" + data.drzava + "]</p>" +
										"<p>Датум на издавање: <span>" + data.datum_izdavanje + "</span></p>" +
										"<p>Издаден за: <span>" + data.izdaden_za + "</span></p>" +
									"</div>"+
								"</div>"+
								"<div id=\"white_cover\">"+
									"<p>Таргет Груп ДОО Скопје</p>"+
								"</div>"+
								"<div class=\"spacer_200\"></div>"+
								"<div class=\"footer cover_footer\">"+
									"<div>"+
										"<p>© CREDIT REPORT, all rights reserved</p>"+
										"<p>www.targetgroup.mk, tel/fax: +389 (2) 3117 - 100</p>"+
									"</div>"+
								   "<div class=\"img_wrapper\">"+
										"<img src=\""+AbsoluteUrlPath+"/img/target_group.png\" />" +
									"</div>"+
								"</div>"+
							"</div>";
			return html;
		}

		private static string FirstPage(Attributes data)
		{

		   string html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Кредитен извештај</h2>"+
									"<h3>"+ data.ime_firma +"</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>" +
								"</div>"+
								"<div class=\"float_right\">"+
									"<img src=\""+ AbsoluteUrlPath +"/img/bonitet_mk.png\" />"+
								"</div>"+
							"</div>"+
							"<div id=\"second_page_wrapper\">"+
								"<div class=\"left_p\">"+
									"<div class=\"p_wrapper left\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Профил</h2>"+
											"<p class=\"black\">Поглавје 1</p>"+
										"</div>"+
										"<div class=\"content gray\">"+
											"<p>Назив:<span>" + data.naziv_firma + "</span></p>"+
											"<p>Адреса:<span>"+ data.adresa_firma +  "</span></p>"+
											"<p>Дејност:<span>" + data.dejnost + "</span></p>" +
											"<p>ЕМБС:<span>" + data.embs + "</span></p>" +
											"<p>ЕДБ:<span>" + data.edb + "</span></p>" +
											"<p>Големина на субјектот:<span>" + data.golemina_na_subjekt + "</span></p>" +
											"<p>Вкупна основна главнина:<span>" + data.vkupna_osnovna_glavnina + "</span></p>" +
											"<p>Уплатен дел:<span>" + data.uplaten_del + "</span></p>" +
											"<p>Непаричен влог:<span>" + data.neparicen_vlog + "</span></p>" +
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper left\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Лица & подружници</h2>"+
											"<p class=\"black\">Поглавје 2</p>"+
										"</div>"+
										"<div class=\"content gray\">"+
											"<p><span>Тековни</span></p>"+
											"<p class=\"float_left\">Сопственици:<span>" + data.tekovni_sopstvenici + "</span></p>" +
											"<p class=\"float_right\">Подружници:<span>" + data.tekovni_podruznici + "</span></p>" +
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper left\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Блокади</h2>"+
											"<p class=\"black\"></p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p><span>Тековни</span></p>"+
											"<p class=\"float_left\">Статус:<span>" + data.tekovni_blokadi_status + "</span></p>" +
										"</div>"+
									"</div>"+
								"</div>"+
								"<div class=\"right_p\">"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Состојба</h2>"+
											"<p class=\"black\">Поглавје 3</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p>" + data.sostojba_komentar + "</p>" +
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Семафор</h2>"+
											"<p class=\"black\"></p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<div class=\"semaphore\">"+
											(data.semafor_solventnost == "green" ? "<img src=\""+AbsoluteUrlPath+"/img/green_light.jpg\" class=\"float_left\" />" : "")  +
											"</div>"+
											"<div class=\"semaphore_text\">"+
												"<p class=\"float_right\">" + data.solventnost_komentar + "</p>" +
											"</div>"+
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Промени & солвентност</h2>"+
											"<p class=\"black\">Поглавје 4</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p><span>Мината година</span></p>"+
											"<p class=\"float_left\">Промени:<span>" + data.promeni_minata_godina + "</span></p>" +
											"<p class=\"float_right\">Солвентност:<span>" + data.solventnost_minata_godina + "</span></p>" +
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Казни & санкции</h2>"+
											"<p class=\"black\">Поглавје 5</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p><span>Мината година</span></p>"+
											"<p class=\"float_left\">Казни:<span>" + data.kazni_minata_godina + "</span></p>" +
											"<p class=\"float_right\">Санкции:<span>" + data.sankcii_minata_godina + "</span></p>"+
										"</div>"+
									"</div>"+
								"</div>"+
								"<div class=\"p_wrapper full\" style=\"margin-top:0;\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Резултати од работењето</h2>"+
										"<p class=\"black\">Поглавје 6</p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"rezultati_table\">"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>Категории</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.kategorija1 + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.kategorija2 + "</span></p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td> "+
													"<p><span>Средства</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.sredstva1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.sredstva2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Капитал</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.kapital1 + "</p>"+
											   "</td>"+
												"<td>"+
													"<p>" + data.kapital2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Вкупни приходи</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.vkupno_prihodi1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.vkupno_prihodi2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
											   "<td>"+
													"<p><span>Нето добивка за деловната година</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.neto_dobivka_za_delovna_godina1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.neto_dobivka_za_delovna_godina2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Просечен број на вработени</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.prosecen_broj_vraboteni1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.prosecen_broj_vraboteni2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Коефициент на задолженост</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.koeficient_na_zadolzensot1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.koeficient_na_zadolzensot2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Тековен показател</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.tekoven_pokazatel1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.tekoven_pokazatel2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Поврат на средства (ROA)</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.povrat_na_sredstva1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.povrat_na_sredstva2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p><span>Поврат на капитал (ROE)</span></p>"+
											   "</td>"+
												"<td>"+
													"<p>" + data.povrat_na_kapital1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.povrat_na_kapital2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"information\">"+
												"<td colspan=\"3\">"+
													"<p>Data are shown in 1 MKD (1 EUR = 61.5 MKD +/- 0.2) see <a href=\"www.nbrm.mk\">www.nbrm.mk</a> for exact exchange rates</p>"+
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
							"</div>"+
							Footer(1)+
						"</div>";

			return html;
		}

		private static string SecondPage(Attributes data)
		{
			string html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Профил</h2>"+
									"<h3>" + data.ime_firma + "</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>"+
								"</div>"+
								"<div class=\"absolute_poglavje\">"+
									"<p>Поглавје 1</p>"+
								"</div>"+
							"</div>"+
							"<div id=\"third_page_wrapper\">"+
								"<div class=\"left_p\">" +
									"<div class=\"p_wrapper left\">" +
										"<div class=\"p_wrapper_header\">"+
											"<h2>Профил</h2>"+
											"<p class=\"black\"></p>"+
										"</div>"+
										"<div class=\"content gray\">"+
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
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper left\">" +
										"<div class=\"p_wrapper_header\">" +
											"<h2>Овластени лица</h2>" +
											"<p class=\"black\"></p>" +
										"</div>" +
										"<div class=\"content\">"+
											"<p class=\"gray\"><span>Име</span></p>"+
											"<p><span>" + data.celosen_naziv_firma + "</span></p>" +
											"<p class=\"border_bottom\"><span>" + data.ovlasteno_lice1 + "</span><p>" + data.ovlasteno_lice1_pozicija + "</p></p>" +
											"<p class=\"border_bottom\"><span>" + data.ovlasteno_lice2 + "</span><p>" + data.ovlasteno_lice2_pozicija + "</p></p>" +
											"<p class=\"border_bottom\"><span>" + data.ovlasteno_lice3 + "</span><p>" + data.ovlasteno_lice3_pozicija + "</p></p>" +
										"</div>"+
									"</div>"+
								"</div>"+
								"<div class=\"right_p\">" +
									"<div class=\"p_wrapper right\">" +
										"<div class=\"p_wrapper_header\">"+
											"<h2>Банкарски сметки</h2>"+
											"<p class=\"black\"></p>"+
										"</div>"+
										"<div class=\"content\">"+
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
										"</div>"+
									"</div>"+
								"</div>"+
							"</div>"+Footer(2)+
						"</div>";

			return html;
		}

		private static string ThirdPage(Attributes data)
		{
			var html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Лица и  подружници</h2>"+
									"<h3>" + data.ime_firma + "</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>"+
								"</div>"+
								"<div class=\"absolute_poglavje\">"+
									"<p>Поглавје 2</p>"+
								"</div>"+
							"</div>"+
							"<div id=\"fourth_page_wrapper\">"+
								"<div class=\"left_p\">"+
									"<div class=\"p_wrapper left\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Сопственици</h2>"+
											"<p class=\"black\">Прикажани " + data.prikazan_broj_sopstvenici + " од " + data.vkupen_broj_sopstvenici + "</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p><span>" + data.celosen_naziv_firma + "</span></p>"+
											"<p><span>" + data.ovlasteno_lice1 + "</span></p>"+
											"<p><span>" + data.ovlasteno_lice2 + "</span></p>" +
											"<p><span>" + data.ovlasteno_lice3 + "</span></p>" +
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper left\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Овластени лица</h2>"+
											"<p class=\"black\">Прикажани " + data.prikazan_broj_ovlasteni + " од " + data.vkupen_broj_ovlasteni + "</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p><span>" + data.ovlasteno_lice3 + "</span></p>" +
											"<p>" + data.ovlasteno_lice3_ovlastuvanja + "</p>"+
											"<p>Тип овластување:</p>"+
											"<p>" + data.ovlasteno_lice3_tip_ovlastuvanja + "</p>"+
											"<p>" + data.ovlasteno_lice3_ovlastuvanja2+ "</p>"+
											"<p><span>" + data.ovlasteno_lice2 + "</span></p>" +
											"<p>" + data.ovlasteno_lice2_ovlastuvanja + "</p>" +
											"<p>Тип овластување:</p>"+
											"<p>" + data.ovlasteno_lice2_tip_ovlastuvanja + "</p>" +
											"<p>" + data.ovlasteno_lice2_ovlastuvanja2 + "</p>" +
										"</div>"+
									"</div>"+
								"</div>"+
								"<div class=\"right_p\">"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Подружници / филијали</h2>"+
											"<p class=\"black\">Прикажани " + data.prikazan_broj_podruznici + " од " + data.vkupen_broj_podruznici + "</p>" +
										"</div>"+
										"<div class=\"content\">"+
											"<p><span>Подброј:</span>" + data.podbroj1 + "</p>"+
											"<p><span>Назив:</span>" + data.celosen_naziv_firma2 + "</p>"+
											"<p><span>Тип:</span>" + data.tip1 + "</p>"+
											"<p><span>Подтип:</span>" + data.podtip1 + "</p>"+
											"<p><span>Приоритетна дејност/Главна приходна шифра:</span>" + data.prioritetna_dejnost1 + "</p>"+
											"<p><span>Адреса:</span>" + data.adresa1 + "</p>"+
											"<p><span>Овластено лице:</span>" + data.ovlasteno_lice2 + "</p>"+
											"<p><span>Подброј</span>" + data.podbroj2 + "</p>"+
											"<p><span>Назив:</span>" + data.celosen_naziv_firma3 + "</p>" +
											"<p><span>Тип:</span>" + data.tip2 + "</p>" +
											"<p><span>Подтип:</span>" + data.podtip1 + "</p>" +
											"<p><span>Приоритетна дејност/Главна приходна шифра:</span>" + data.prioritetna_dejnost2 + "</p>" +
											"<p><span>Адреса:</span>" + data.adresa2 + "</p>" +
											"<p><span>Овластено лице:</span>" + data.ovlasteno_lice2 + "</p>" +
										"</div>"+
									"</div>"+
								"</div>"+
							"</div>"+Footer(3)+
						"</div>";

			return html;
		}

		private static string FourthPage(Attributes data)
		{
			var html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Состојба на компанија</h2>"+
									"<h3>" + data.ime_firma +"</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>"+
								"</div>"+
								"<div class=\"absolute_poglavje\">"+
									"<p>Поглавје 3</p>"+
								"</div>"+
							"</div>"+
							"<div id=\"fifth_page_wrapper\">"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Семафор</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content gray\">"+
										"<div class=\"semaphore\">"+
											"<img class=\"float_left\" src=\""+AbsoluteUrlPath+"/img/green_light.jpg\" />"+
										"</div>"+
										"<div class=\"semaphore_text\">"+
											"<p>" + data.solventnost_komentar + "</p>"+
										"</div>"+
									"</div>"+
								"</div>"+
								"<div class=\"left_p shorter\">"+
									"<div class=\"p_wrapper left\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Финансиска проценка</h2>"+
											"<p class=\"black\"></p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<p>" + data.finansiska_procenka_komentar + "</p>"+
										"</div>"+
									"</div>"+
								"</div>"+
								"<div class=\"spacer_50_600\"></div>"+
								"<div class=\"right_p longer\">"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Ликвидност</h2>"+
											"<p class=\"black\">" + data.likvidnost_opis + "</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<table class=\"finansii_table\">"+
												"<tr class=\"gray\">"+
													"<td>"+
														"<p><span>Показател</span></p>"+
													"</td>"+
													"<td>"+
														"<p><span>Вредност</span></p>"+
													"</td>"+
													"<td>"+
														"<p><span>Опис</span></p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Коефициент за задолженост</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_koeficient_za_zadolzenost + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_opis + "</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Брз показател</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_brz_pokazatel + " </p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_opis + "</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Просечни денови на плаќање на обврските</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_prosecni_denovi_na_plakanje_ovrski + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_opis + "</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Кредитна изложеност од работење</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_kreditna_izlozenost_od_rabotenje + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.likvidnost_opis + "</p>"+
													"</td>"+
												"</tr>"+
											"</table>"+
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Ефикасност</h2>"+
											"<p class=\"black\">" + data.efikasnost_opis + "</p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<table class=\"finansii_table\">"+
												"<tr class=\"gray\">"+
													"<td>"+
														"<p><span>Показател</span></p>"+
													"</td>"+
													"<td>"+
														"<p><span>Вредност</span></p>"+
													"</td>"+
													"<td>"+
														"<p><span>Опис</span></p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Поврат на средства (ROA)</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.efikasnost_povrat_na_sredstva + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.efikasnost_opis + "</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Нето профитна маржа</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.efikasnost_neto_profitna_marza + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.efikasnost_opis + "</p>"+
													"</td>"+
												"</tr>"+
											"</table>"+
										"</div>"+
									"</div>"+
									"<div class=\"p_wrapper right\">"+
										"<div class=\"p_wrapper_header\">"+
											"<h2>Профитабилност</h2>"+
											"<p class=\"black\"></p>"+
										"</div>"+
										"<div class=\"content\">"+
											"<table class=\"finansii_table\">"+
												"<tr class=\"gray\">"+
													"<td>"+
														"<p><span>Ставка</span></p>"+
													"</td>"+
													"<td>"+
														"<p><span>" + data.profitabilnost_stavka1 + "</span></p>"+
													"</td>"+
													"<td>"+
														"<p><span>" + data.profitabilnost_stavka2 + "</span></p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Бруто оперативна добивка</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_bruto_operativna_dobivka1+ "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_bruto_operativna_dobivka2+ "</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>Нето добивка од финансирање</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_neto_dobivka_od_finansiranje1 + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_neto_dobivka_od_finansiranje2 + "</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>EBITDA</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_ebitda1 +"</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_ebitda2 +"</p>"+
													"</td>"+
												"</tr>"+
												"<tr>"+
													"<td>"+
														"<p><span>EBIT</span></p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_ebit1 + "</p>"+
													"</td>"+
													"<td>"+
														"<p>" + data.profitabilnost_ebit2 + "</p>"+
													"</td>"+
												"</tr>"+
											"</table>"+
										"</div>"+
									"</div>"+
								"</div>"+
							"</div>"+Footer(4)+
						"</div>";

			return html;
		}

		private static string FifthPage(Attributes data)
		{
			var html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Промени & Солвентност</h2>"+
									"<h3>" + data.ime_firma + "</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>"+
								"</div>"+
								"<div class=\"absolute_poglavje\">"+
									"<p>Поглавје 4</p>"+
								"</div>"+
							"</div>"+
							"<div id=\"sixth_page_wrapper\">"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Историски промени</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"promeni_table\">"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>#</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Датум</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Вид</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Опис</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>1</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_datum1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_vid1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_opis1 + "</p>"+
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>2</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_datum2 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_vid2 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_opis2 + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>3</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_datum3 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_vid3 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.istoriski_promeni_opis3 + "</p>" +
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Солвентност</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"promeni_table\">"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>#</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Датум</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Вид</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Опис</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>1</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.solventnost_datum1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.solventnost_opis1 + "</p>"+
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
							"</div>"+Footer(5)+
						"</div>";

			return html;
		}

		private static string SixthPage(Attributes data)
		{
			var html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Казни & Санкции</h2>"+
									"<h3>" + data.ime_firma + "</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>"+
								"</div>"+
								"<div class=\"absolute_poglavje\">"+
									"<p>Поглавје 5</p>"+
								"</div>"+
							"</div>"+
							"<div id=\"seventh_page_wrapper\">"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Казни</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"kazni_table\">"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>#</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Датум</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Вид</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Опис</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>1</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.kazni_datum1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.kazni_opis1 + "</p>"+
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Санкции</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"kazni_table\">"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>#</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Датум</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Вид</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Опис</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>1</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.sankcii_datum1 + "</p>"+
												"</td>"+
												"<td>"+
													"<p></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.sankcii_opis1 + "</p>"+
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Блокади</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"kazni_table\">"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>#</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Датум</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Вид</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Опис</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>1</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.blokadi_datum1 +"</p>"+
												"</td>"+
												"<td>"+
													"<p>Нема</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.blokadi_opis1+ "</p>"+
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
							"</div>"+Footer(6)+
						"</div>";

			return html;
		}

		private static string SeventhPage(Attributes data)
		{
			var html = "<div class=\"page\">"+
							"<div class=\"header\">"+
								"<div class=\"float_left\">"+
									"<h2>Финансиски податоци</h2>"+
									"<h3>" + data.ime_firma + "</h3>"+
									"<p class=\"date\">Издаден " + data.datum_izdavanje + "</p>"+
								"</div>"+
								"<div class=\"absolute_poglavje\">"+
									"<p>Поглавје 6</p>"+
								"</div>"+
							"</div>"+
							"<div id=\"eigth_page_wrapger\">"+
								"<div class=\"p_wrapper full\">"+
									"<div class=\"p_wrapper_header\">"+
										"<h2>Биланс на состојба</h2>"+
										"<p class=\"black\"></p>"+
									"</div>"+
									"<div class=\"content\">"+
										"<table class=\"bilans_table\">"+
											"<tr class=\"gray text_right\">"+
												"<td colspan=\"7\">"+
													"<p><span>% во вкупно</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr class=\"gray\">"+
												"<td>"+
													"<p><span>Категорија</span></p>"+
												"</td>"+
												"<td>"+
													"<p></p>"+
												"</td>"+
												"<td>"+
													"<p><span>2013</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>2012</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>Инд.</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>2013</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>2012</span></p>"+
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Нетековни средства</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_netekovni_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_netekovni_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_netekovni_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_netekovni_ind + "</p>" +
												"</td>"+
											   "<td>"+
													"<p>" + data.bilans_netekovni_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_netekovni_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Нематеријални средства</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_nematerijalni_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_nematerijalni_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_nematerijalni_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_nematerijalni_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_nematerijalni_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_nematerijalni_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Материјални средства</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_materijalni_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_materijalni_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_materijalni_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_materijalni_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_materijalni_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_materijalni_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Вложувања во недвижности</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vlozuvanje_nedviznosti_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vlozuvanje_nedviznosti_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vlozuvanje_nedviznosti_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vlozuvanje_nedviznosti_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vlozuvanje_nedviznosti_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vlozuvanje_nedviznosti_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Долгорочни финансиски средства</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_sredstva_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_sredstva_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_sredstva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_sredstva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_sredstva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_sredstva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Долгорочни побарувања</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_pobaruvanja_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_pobaruvanja_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_pobaruvanja_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_pobaruvanja_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_pobaruvanja_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_pobaruvanja_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Одложени даночни средства</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_odlozeni_danocni_sredstva_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_danocni_sredstva_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_danocni_sredstva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_danocni_sredstva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_danocni_sredstva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_danocni_sredstva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Тековни средства</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_tekovni_sredstva_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_tekovni_sredstva_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_tekovni_sredstva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_tekovni_sredstva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_tekovni_sredstva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_tekovni_sredstva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
										   "<tr>"+
												"<td>"+
													"<p>Залихи</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zalihi_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zalihi_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zalihi_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zalihi_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zalihi_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zalihi_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Краткорочни побарувања</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_pobaruvanja_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_pobaruvanja_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_pobaruvanja_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_pobaruvanja_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_pobaruvanja_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_pobaruvanja_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Краткорочни финансиски средства</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_sredstva_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_sredstva_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_sredstva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_sredstva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_sredstva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_sredstva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Парични средства и парични еквиваленти</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_paricni_sredstva_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_paricni_sredstva_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_paricni_sredstva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_paricni_sredstva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_paricni_sredstva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_paricni_sredstva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Средства (или групи а отуѓување наменети за ...</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_sredstva_grupa_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sredstva_grupa_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sredstva_grupa_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sredstva_grupa_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sredstva_grupa_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sredstva_grupa_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Платени трошоци за идните периоди и пресметан ...</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_plateni_trosoci_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_plateni_trosoci_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_plateni_trosoci_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_plateni_trosoci_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_plateni_trosoci_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_plateni_trosoci_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Вкупна актива</span></p>"+
												"</td>"+
												"<td>"+
													"<p></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_aktiva_2013 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_aktiva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_aktiva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_aktiva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_aktiva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Главнина и резерви</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_glavnina_i_rezervi_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_glavnina_i_rezervi_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_glavnina_i_rezervi_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_glavnina_i_rezervi_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_glavnina_i_rezervi_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_glavnina_i_rezervi_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Основна главнина </p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_osnovna_glavnina_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_osnovna_glavnina_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_osnovna_glavnina_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_osnovna_glavnina_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_osnovna_glavnina_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_osnovna_glavnina_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Премии на емитирани акции</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_premii_akcii_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_premii_akcii_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_premii_akcii_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_premii_akcii_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_premii_akcii_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_premii_akcii_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Сопствени акции</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sopstveni_akcii_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sopstveni_akcii_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sopstveni_akcii_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sopstveni_akcii_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sopstveni_akcii_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_sopstveni_akcii_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Запишан, неуплатен капитал</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zapisan_kapital_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zapisan_kapital_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zapisan_kapital_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zapisan_kapital_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zapisan_kapital_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zapisan_kapital_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Ревалоризациска резерва и разлики од вреднување на ...</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_revalorizaciska_rezerva_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_revalorizaciska_rezerva_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_revalorizaciska_rezerva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_revalorizaciska_rezerva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_revalorizaciska_rezerva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_revalorizaciska_rezerva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Резерви</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_rezervi_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_rezervi_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_rezervi_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_rezervi_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_rezervi_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_rezervi_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Акумулирана добивка</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_akumulirana_dobivka_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_akumulirana_dobivka_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_akumulirana_dobivka_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_akumulirana_dobivka_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_akumulirana_dobivka_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_akumulirana_dobivka_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Пренесена загуба</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_prenesena_zaguba_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_prenesena_zaguba_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_prenesena_zaguba_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_prenesena_zaguba_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_prenesena_zaguba_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_prenesena_zaguba_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Добивка за деловната година</p>"+
												"</td>"+
												"<td>"+
												   "<p>" + data.bilans_dobivka_delovna_godina_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dobivka_delovna_godina_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dobivka_delovna_godina_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dobivka_delovna_godina_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dobivka_delovna_godina_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dobivka_delovna_godina_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Загуба за деловната година</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zaguba_delovna_godina_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zaguba_delovna_godina_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zaguba_delovna_godina_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zaguba_delovna_godina_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zaguba_delovna_godina_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_zaguba_delovna_godina_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Обврски</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_obvrski_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Долгорочни резервирања на ризици и трошоци</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_rezerviranja_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_rezerviranja_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_rezerviranja_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_rezerviranja_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_rezerviranja_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_rezerviranja_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Долгорочни обврски </p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_obvrski_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_obvrski_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_obvrski_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_obvrski_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_obvrski_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_dolgorocni_obvrski_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr>"+
												"<td>"+
													"<p>Краткорочни обврски</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_obvrski_sredstva + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_obvrski_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_obvrski_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_obvrski_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_obvrski_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_kratkorocni_obvrski_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Одложени даночни обврски</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_odlozeni_obvrski_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_obvrski_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_obvrski_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_obvrski_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_obvrski_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeni_obvrski_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Одложено плаќање и приходи во идните периоди</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_odlozeno_plakanje_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeno_plakanje_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeno_plakanje_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeno_plakanje_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeno_plakanje_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_odlozeno_plakanje_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Обврски по основ на нетековни средства (или груп...</span></p>"+
												"</td>"+
												"<td>"+
													"<p><span>" + data.bilans_obvrski_po_osnov_sredstva + "</span></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_po_osnov_2013 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_po_osnov_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_po_osnov_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_po_osnov_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_obvrski_po_osnov_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"has_border\">"+
												"<td>"+
													"<p><span>Вкупна пасива</span></p>"+
												"</td>"+
												"<td>"+
													"<p></p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_pasiva_2013 + "</p>"+
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_pasiva_2012 + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_pasiva_ind + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_pasiva_2013_procent + "</p>" +
												"</td>"+
												"<td>"+
													"<p>" + data.bilans_vkupna_pasiva_2012_procent + "</p>" +
												"</td>"+
											"</tr>"+
											"<tr class=\"information\">"+
												"<td colspan=\"7\">"+
													"<p>Data are shown in 1 MKD (1 EUR = 61.5 MKD +/- 0.2) see <a href=\"www.nbrm.mk\">www.nbrm.mk</a> for exact exchange rates</p>"+
												"</td>"+
											"</tr>"+
										"</table>"+
									"</div>"+
								"</div>"+
							"</div>"+Footer(7)+
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
													"<p>" + data.uspeh_trosoci_za_vraboteni_2013 +"</p> " +
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
												"    <p>" + data.uspeh_neto_dobivka_sredstva +"</p> " +
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
											"        <p>" + data.indikatori_raboten_kapital_2013 +"</p> " +
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
	
	}

	public class Attributes
	{
	
		public string ime_firma { get; set; }
		public string drzava { get; set; }
		public string datum_izdavanje { get; set; }
		public string izdaden_za { get; set; }
		public string uplaten_del { get; set; }
		public string neparicen_vlog { get; set; }

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

		public string ovlasteno_lice1 { get; set; }
		public string ovlasteno_lice1_pozicija { get; set; }

		public string ovlasteno_lice2 { get; set; }
		public string ovlasteno_lice2_pozicija { get; set; }
		public string ovlasteno_lice2_ovlastuvanja { get; set; }
		public string ovlasteno_lice2_tip_ovlastuvanja { get; set; }
		public string ovlasteno_lice2_ovlastuvanja2 { get; set; }

		public string ovlasteno_lice3 { get; set; }
		public string ovlasteno_lice3_pozicija { get; set; }
		public string ovlasteno_lice3_ovlastuvanja { get; set; }
		public string ovlasteno_lice3_tip_ovlastuvanja { get; set; }
		public string ovlasteno_lice3_ovlastuvanja2 { get; set; }

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


		public string podbroj1 { get; set; }
		public string celosen_naziv_firma2 { get; set; }
		public string tip1 { get; set; }
		public string podtip1 { get; set; }
		public string prioritetna_dejnost1 { get; set; }
		public string adresa1 { get; set; }
		public string podbroj2 { get; set; }
		public string celosen_naziv_firma3 { get; set; }
		public string tip2 { get; set; }
		public string podtip2 { get; set; }
		public string prioritetna_dejnost2 { get; set; }
		public string adresa2 { get; set; }

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
		public string tekovi_neto_odlivi_gotovina_finansiski{ get; set; }
		public string tekovi_vkupno_prilivi_gotovina { get; set; }
		public string tekovi_vkupno_odlivi_gotovina { get; set; }
		public string tekovi_vkupno_neto_prilivi { get; set; }
		public string tekovi_vkupno_neto_odlivi { get; set; }
		public string tekovi_paricni_sredstva_pocetok { get; set; }
		public string tekovi_paricni_sredstva_kraj { get; set; }
	}


}