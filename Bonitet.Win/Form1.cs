using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Bonitet.DAL;

namespace Bonitet.Win
{
    public partial class Form1 : Form
    {
        public static string currentDirectory = Environment.CurrentDirectory;
        public Form1()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> DictAddItems(Dictionary<string, string> container, Dictionary<string, string> data)
        {
            foreach (var item in data)
            {
                container.Add(item.Key, item.Value);
            }

            return container;
        }


        int counterNew;
        int counterOld;
        int counterTotal;

        Dictionary<string, Dictionary<string, string>> listOldValues = new Dictionary<string, Dictionary<string, string>>();

        public Dictionary<string, string> GetAllValuesFromDocument(HtmlAgilityPack.HtmlDocument doc)
        {
            var StartXpath = "/html[1]/body[1]/div[7]/div[2]/div[1]/div[1]/table[1]/tbody[1]";
            var KriteriumXpath = StartXpath + "/tr[1]/td[1]";
            var ValuesXpath = StartXpath + "/tr[2]/td[1]";

            var KriteriumEMBSXpath = "/table[1]/tbody[1]/tr[1]/td[1]/span[1]/span[1]/strong[1]";
            var KriteriumYearXpath = "/table[1]/tbody[1]/tr[1]/td[1]/span[1]/span[1]/strong[2]";

            var KriteriumEMBSNode = doc.DocumentNode.SelectSingleNode(KriteriumXpath + KriteriumEMBSXpath);
            var KriteriumYearNode = doc.DocumentNode.SelectSingleNode(KriteriumXpath + KriteriumYearXpath);

            var EMBS = KriteriumEMBSNode.InnerText.Substring(6);
            var CurYear = KriteriumYearNode.InnerText.Substring(8);

            // check if exists 
            var res = DALHelper.CheckIfDataForShortReportExist(EMBS, Convert.ToInt32(CurYear));

            counterNew++;
            
            var Values = new Dictionary<string, string>();

            Values.Add("Year", CurYear);

            Values = DictAddItems(Values, GetCompanyInfoNodes(doc, ValuesXpath));

            Values = DictAddItems(Values, GetBilansNaUspehPrihodi(doc, ValuesXpath));

            Values = DictAddItems(Values, GetBilansNaUspehRashodi(doc, ValuesXpath));

            Values = DictAddItems(Values, GetFinansiskiRezultat(doc, ValuesXpath));

            Values = DictAddItems(Values, GetProsecenBrojVraboteni(doc, ValuesXpath));

            Values = DictAddItems(Values, GetBilansNaSostojbaAktiva(doc, ValuesXpath));

            Values = DictAddItems(Values, GetBilansNaSostojbaPasiva(doc, ValuesXpath));

            if (res != null)
            {
                counterOld++;
                if (listOldValues == null)
                    listOldValues = new Dictionary<string, Dictionary<string, string>>();

                listOldValues.Add(EMBS, Values);

                return null;
            }

            return Values;
                
        }

        public Dictionary<string, string> GetCompanyInfoNodes(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var OpstiInfomraciiXpath = ValuesXpath + "/table[1]/tbody[1]";

            var NazivNaPravnoLiceXPath = OpstiInfomraciiXpath + "/tr[1]/td[2]";
            var MestoXPath = OpstiInfomraciiXpath + "/tr[2]/td[2]";
            var MaticenBrojXPath = OpstiInfomraciiXpath + "/tr[3]/td[2]";

            var NazivNode = doc.DocumentNode.SelectSingleNode(NazivNaPravnoLiceXPath);
            var MestoNode = doc.DocumentNode.SelectSingleNode(MestoXPath);
            var MaticenBrojNode = doc.DocumentNode.SelectSingleNode(MaticenBrojXPath);


            var res = new Dictionary<string, string>();

            res.Add("Naziv", NazivNode.InnerText);
            res.Add("Mesto", MestoNode.InnerText);
            res.Add("EMBS", MaticenBrojNode.InnerText);

            return res;
        }

        public Dictionary<string, string> GetBilansNaUspehPrihodi(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var BilansNaUspehPrihodiXpath = ValuesXpath + "/table[4]/tbody[1]/tr[3]";

            var VkupniPrihodiLastYearXpath = BilansNaUspehPrihodiXpath + "/td[2]";
            var VkupniPrihodiCurYearXpath = BilansNaUspehPrihodiXpath + "/td[3]";
            var VkupniPrihodiIndCurYearXpath = BilansNaUspehPrihodiXpath + "/td[4]";
            var StruktraVoProcentPrihodiLastYearXpath = BilansNaUspehPrihodiXpath + "/td[5]";
            var StrukturaVoProcentPrihodiCurYearXpath = BilansNaUspehPrihodiXpath + "/td[6]";

            var VkupniPrihodiLastYearNode = doc.DocumentNode.SelectSingleNode(VkupniPrihodiLastYearXpath);
            var VkupniPrihodiCurYearNode = doc.DocumentNode.SelectSingleNode(VkupniPrihodiCurYearXpath);
            var VkupniPrihodiIndCurYearNode = doc.DocumentNode.SelectSingleNode(VkupniPrihodiIndCurYearXpath);
            var StruktraVoProcentPrihodiLastYearNode = doc.DocumentNode.SelectSingleNode(StruktraVoProcentPrihodiLastYearXpath);
            var StrukturaVoProcentPrihodiCurYearNode = doc.DocumentNode.SelectSingleNode(StrukturaVoProcentPrihodiCurYearXpath);

            var res = new Dictionary<string, string>();

            res.Add("Вкупни приходи_LastYear", VkupniPrihodiLastYearNode.InnerText);
            res.Add("Вкупни приходи", VkupniPrihodiCurYearNode.InnerText);
            res.Add("Вкупни приходи_Ind", VkupniPrihodiIndCurYearNode.InnerText);
            res.Add("Вкупни приходи_Procent_LastYear", StruktraVoProcentPrihodiLastYearNode.InnerText);
            res.Add("Вкупни приходи_Procent", StrukturaVoProcentPrihodiCurYearNode.InnerText);

            return res;

        }

        public Dictionary<string, string> GetBilansNaUspehRashodi(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var BilansNaUspehTrosociXpath = ValuesXpath + "/table[6]/tbody[1]/tr[2]";

            var VkupniRashodiLastYearXpath = BilansNaUspehTrosociXpath + "/td[2]";
            var VkupniRashodiCurYearXpath = BilansNaUspehTrosociXpath + "/td[3]";
            var VkupniRashodiIndCurYearXpath = BilansNaUspehTrosociXpath + "/td[4]";
            var StrukturaVoProcentRashodiLastYearXpath = BilansNaUspehTrosociXpath + "/td[5]";
            var StrukturaVoProcentRashodiCurYearXpath = BilansNaUspehTrosociXpath + "/td[6]";

            var VkupniRashodiLastYearNode = doc.DocumentNode.SelectSingleNode(VkupniRashodiLastYearXpath);
            var VkupniRashodiCurYearNode = doc.DocumentNode.SelectSingleNode(VkupniRashodiCurYearXpath);
            var VkupniRashodiIndCurYearNode = doc.DocumentNode.SelectSingleNode(VkupniRashodiIndCurYearXpath);
            var StruktraVoProcentRashodiLastYearNode = doc.DocumentNode.SelectSingleNode(StrukturaVoProcentRashodiLastYearXpath);
            var StrukturaVoProcentRashodiCurYearNode = doc.DocumentNode.SelectSingleNode(StrukturaVoProcentRashodiCurYearXpath);

            var res = new Dictionary<string, string>();

            res.Add("Вкупни расходи_LastYear", VkupniRashodiLastYearNode.InnerText);
            res.Add("Вкупни расходи", VkupniRashodiCurYearNode.InnerText);
            res.Add("Вкупни расходи_Ind", VkupniRashodiIndCurYearNode.InnerText);
            res.Add("Вкупни расходи_Procent_LastYear", StruktraVoProcentRashodiLastYearNode.InnerText);
            res.Add("Вкупни расходи_Procent", StrukturaVoProcentRashodiCurYearNode.InnerText);

            return res;

        }

        public Dictionary<string, string> GetFinansiskiRezultat(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var FinansiskiRezultatXpath = ValuesXpath + "/table[8]/tbody[1]";
            var DobivkaZaFinansiskaGodinaXpath = FinansiskiRezultatXpath + "/tr[2]";
            var ZagubaZaFinansiskaGodinaXpath = FinansiskiRezultatXpath + "/tr[3]";

            var DobivkaZaFinansiskaGodinaLastYearXpath = DobivkaZaFinansiskaGodinaXpath + "/td[2]";
            var DobivkaZaFinansiskaGodinaCurYearXpath = DobivkaZaFinansiskaGodinaXpath + "/td[3]";
            var DobivkaZaFinansiskaGodinaIndCurYearXpath = DobivkaZaFinansiskaGodinaXpath + "/td[4]";

            var ZagubaZaFinansiskaGodinaLastYearXpath = ZagubaZaFinansiskaGodinaXpath + "/td[2]";
            var ZagubaZaFinansiskaGodinaCurYearXpath = ZagubaZaFinansiskaGodinaXpath + "/td[3]";
            var ZagubaZaFinansiskaGodinaIndCurYearXpath = ZagubaZaFinansiskaGodinaXpath + "/td[4]";

            var DobivkaZaFinansiskaGodinaLastYearNode = doc.DocumentNode.SelectSingleNode(DobivkaZaFinansiskaGodinaLastYearXpath);
            var DobivkaZaFinansiskaGodinaCurYearNode = doc.DocumentNode.SelectSingleNode(DobivkaZaFinansiskaGodinaCurYearXpath);
            var DobivkaZaFinansiskaGodinaIndCurYearNode = doc.DocumentNode.SelectSingleNode(DobivkaZaFinansiskaGodinaIndCurYearXpath);

            var ZagubaZaFinansiskaGodinaLastYearNode = doc.DocumentNode.SelectSingleNode(ZagubaZaFinansiskaGodinaLastYearXpath);
            var ZagubaZaFinansiskaGodinaCurYearNode = doc.DocumentNode.SelectSingleNode(ZagubaZaFinansiskaGodinaCurYearXpath);
            var ZagubaZaFinansiskaGodinaIndCurYearNode = doc.DocumentNode.SelectSingleNode(ZagubaZaFinansiskaGodinaIndCurYearXpath);

            var res = new Dictionary<string, string>();

            res.Add("Добивка за финансиска година_LastYear", DobivkaZaFinansiskaGodinaLastYearNode.InnerText);
            res.Add("Добивка за финансиска година", DobivkaZaFinansiskaGodinaCurYearNode.InnerText);
            res.Add("Добивка за финансиска година_Ind", DobivkaZaFinansiskaGodinaIndCurYearNode.InnerText);

            res.Add("Загуба за финансиска година_LastYear", ZagubaZaFinansiskaGodinaLastYearNode.InnerText);
            res.Add("Загуба за финансиска година", ZagubaZaFinansiskaGodinaCurYearNode.InnerText);
            res.Add("Загуба за финансиска година_Ind", ZagubaZaFinansiskaGodinaIndCurYearNode.InnerText);

            return res;
        }

        public Dictionary<string, string> GetProsecenBrojVraboteni(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var BrojVraboteniXpath = ValuesXpath + "/table[10]/tbody[1]/tr[1]";

            var ProsecenBrojVraboteniLastYearXpath = BrojVraboteniXpath + "/td[2]";
            var ProsecenBrojVraboteniCurYearXpath = BrojVraboteniXpath + "/td[3]";

            var ProsecenBrojVraboteniLastYearNode = doc.DocumentNode.SelectSingleNode(ProsecenBrojVraboteniLastYearXpath);
            var ProsecenBrojVraboteniCurYearNode = doc.DocumentNode.SelectSingleNode(ProsecenBrojVraboteniCurYearXpath);

            var res = new Dictionary<string, string>();

            res.Add("Просечен број на вработени_LastYear", ProsecenBrojVraboteniLastYearNode.InnerText);
            res.Add("Просечен број на вработени", ProsecenBrojVraboteniCurYearNode.InnerText);

            return res;
        }
        
        public Dictionary<string, string> GetBilansNaSostojbaAktiva(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var BilansNaSostojbaXpath = ValuesXpath + "/table[13]/tbody[1]";

            var BilansNaSostojbaNetekovniSredstvaXpath = BilansNaSostojbaXpath + "/tr[3]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiXpath = BilansNaSostojbaXpath + "/tr[4]";
            var BilansNaSostojbaTekovniSredstvaXpath = BilansNaSostojbaXpath + "/tr[5]";
            var BilansNaSostojbaZalihiXpath = BilansNaSostojbaXpath + "/tr[6]";
            var BilansNaSostojbaSredstvaGrupiXpath = BilansNaSostojbaXpath + "/tr[7]";
            var BilansNaSostojbaPlateniTrosociXpath = BilansNaSostojbaXpath + "/tr[8]";
            var BilansNaSostojbaDVkupnaAktivaXpath = BilansNaSostojbaXpath + "/tr[9]";

            // 1
            var BilansNaSostojbaNetekovniSredstvaLastYearXpath = BilansNaSostojbaNetekovniSredstvaXpath + "/td[2]";
            var BilansNaSostojbaNetekovniSredstvaCurYearXpath = BilansNaSostojbaNetekovniSredstvaXpath + "/td[3]";
            var BilansNaSostojbaNetekovniSredstvaIndXpath = BilansNaSostojbaNetekovniSredstvaXpath + "/td[4]";
            var BilansNaSostojbaNetekovniSredstvaProcentLastYearXpath = BilansNaSostojbaNetekovniSredstvaXpath + "/td[5]";
            var BilansNaSostojbaNetekovniSredstvaProcentCurYearXpath = BilansNaSostojbaNetekovniSredstvaXpath + "/td[6]";

            var BilansNaSostojbaNetekovniSredstvaLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaNetekovniSredstvaLastYearXpath);
            var BilansNaSostojbaNetekovniSredstvaCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaNetekovniSredstvaCurYearXpath);
            var BilansNaSostojbaNetekovniSredstvaIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaNetekovniSredstvaIndXpath);
            var BilansNaSostojbaNetekovniSredstvaProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaNetekovniSredstvaProcentLastYearXpath);
            var BilansNaSostojbaNetekovniSredstvaProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaNetekovniSredstvaProcentCurYearXpath);

            // 2
            var BilansNaSostojbaOdlozeniDanocniObvrskiLastYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiXpath + "/td[2]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiCurYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiXpath + "/td[3]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiIndXpath = BilansNaSostojbaOdlozeniDanocniObvrskiXpath + "/td[4]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiProcentLastYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiXpath + "/td[5]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiProcentCurYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiXpath + "/td[6]";

            var BilansNaSostojbaOdlozeniDanocniObvrskiLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiLastYearXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiCurYearXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiIndXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiProcentLastYearXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiProcentCurYearXpath);

            // 3
            var BilansNaSostojbaTekovniSredstvaLastYearXpath = BilansNaSostojbaTekovniSredstvaXpath + "/td[2]";
            var BilansNaSostojbaTekovniSredstvaCurYearXpath = BilansNaSostojbaTekovniSredstvaXpath + "/td[3]";
            var BilansNaSostojbaTekovniSredstvaIndXpath = BilansNaSostojbaTekovniSredstvaXpath + "/td[4]";
            var BilansNaSostojbaTekovniSredstvaProcentLastYearXpath = BilansNaSostojbaTekovniSredstvaXpath + "/td[5]";
            var BilansNaSostojbaTekovniSredstvaProcentCurYearXpath = BilansNaSostojbaTekovniSredstvaXpath + "/td[6]";

            var BilansNaSostojbaTekovniSredstvaLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaTekovniSredstvaLastYearXpath);
            var BilansNaSostojbaTekovniSredstvaCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaTekovniSredstvaCurYearXpath);
            var BilansNaSostojbaTekovniSredstvaIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaTekovniSredstvaIndXpath);
            var BilansNaSostojbaTekovniSredstvaProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaTekovniSredstvaProcentLastYearXpath);
            var BilansNaSostojbaTekovniSredstvaProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaTekovniSredstvaProcentCurYearXpath);

            // 4
            var BilansNaSostojbaZalihiLastYearXpath = BilansNaSostojbaZalihiXpath + "/td[2]";
            var BilansNaSostojbaZalihiCurYearXpath = BilansNaSostojbaZalihiXpath + "/td[3]";
            var BilansNaSostojbaZalihiIndXpath = BilansNaSostojbaZalihiXpath + "/td[4]";
            var BilansNaSostojbaZalihiProcentLastYearXpath = BilansNaSostojbaZalihiXpath + "/td[5]";
            var BilansNaSostojbaZalihiProcentCurYearXpath = BilansNaSostojbaZalihiXpath + "/td[6]";

            var BilansNaSostojbaZalihiLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaZalihiLastYearXpath);
            var BilansNaSostojbaZalihiCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaZalihiCurYearXpath);
            var BilansNaSostojbaZalihiIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaZalihiIndXpath);
            var BilansNaSostojbaZalihiProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaZalihiProcentLastYearXpath);
            var BilansNaSostojbaZalihiProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaZalihiProcentCurYearXpath);

            // 5
            var BilansNaSostojbaSredstvaGrupiLastYearXpath = BilansNaSostojbaSredstvaGrupiXpath + "/td[2]";
            var BilansNaSostojbaSredstvaGrupiCurYearXpath = BilansNaSostojbaSredstvaGrupiXpath + "/td[3]";
            var BilansNaSostojbaSredstvaGrupiIndXpath = BilansNaSostojbaSredstvaGrupiXpath + "/td[4]";
            var BilansNaSostojbaSredstvaGrupiProcentLastYearXpath = BilansNaSostojbaSredstvaGrupiXpath + "/td[5]";
            var BilansNaSostojbaSredstvaGrupiProcentCurYearXpath = BilansNaSostojbaSredstvaGrupiXpath + "/td[6]";

            var BilansNaSostojbaSredstvaGrupiLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaSredstvaGrupiLastYearXpath);
            var BilansNaSostojbaSredstvaGrupiCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaSredstvaGrupiCurYearXpath);
            var BilansNaSostojbaSredstvaGrupiIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaSredstvaGrupiIndXpath);
            var BilansNaSostojbaSredstvaGrupiProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaSredstvaGrupiProcentLastYearXpath);
            var BilansNaSostojbaSredstvaGrupiProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaSredstvaGrupiProcentCurYearXpath);

            // 6
            var BilansNaSostojbaPlateniTrosociLastYearXpath = BilansNaSostojbaPlateniTrosociXpath + "/td[2]";
            var BilansNaSostojbaPlateniTrosociCurYearXpath = BilansNaSostojbaPlateniTrosociXpath + "/td[3]";
            var BilansNaSostojbaPlateniTrosociIndXpath = BilansNaSostojbaPlateniTrosociXpath + "/td[4]";
            var BilansNaSostojbaPlateniTrosociProcentLastYearXpath = BilansNaSostojbaPlateniTrosociXpath + "/td[5]";
            var BilansNaSostojbaPlateniTrosociProcentCurYearXpath = BilansNaSostojbaPlateniTrosociXpath + "/td[6]";

            var BilansNaSostojbaPlateniTrosociLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaPlateniTrosociLastYearXpath);
            var BilansNaSostojbaPlateniTrosociCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaPlateniTrosociCurYearXpath);
            var BilansNaSostojbaPlateniTrosociIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaPlateniTrosociIndXpath);
            var BilansNaSostojbaPlateniTrosociProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaPlateniTrosociProcentLastYearXpath);
            var BilansNaSostojbaPlateniTrosociProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaPlateniTrosociProcentCurYearXpath);

            // 7
            var BilansNaSostojbaDVkupnaAktivaLastYearXpath = BilansNaSostojbaDVkupnaAktivaXpath + "/td[2]";
            var BilansNaSostojbaDVkupnaAktivaCurYearXpath = BilansNaSostojbaDVkupnaAktivaXpath + "/td[3]";
            var BilansNaSostojbaDVkupnaAktivaIndXpath = BilansNaSostojbaDVkupnaAktivaXpath + "/td[4]";
            var BilansNaSostojbaDVkupnaAktivaProcentLastYearXpath = BilansNaSostojbaDVkupnaAktivaXpath + "/td[5]";
            var BilansNaSostojbaDVkupnaAktivaProcentCurYearXpath = BilansNaSostojbaDVkupnaAktivaXpath + "/td[6]";

            var BilansNaSostojbaDVkupnaAktivaLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaAktivaLastYearXpath);
            var BilansNaSostojbaDVkupnaAktivaCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaAktivaCurYearXpath);
            var BilansNaSostojbaDVkupnaAktivaIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaAktivaIndXpath);
            var BilansNaSostojbaDVkupnaAktivaProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaAktivaProcentLastYearXpath);
            var BilansNaSostojbaDVkupnaAktivaProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaAktivaProcentCurYearXpath);


            var res = new Dictionary<string, string>();

            res.Add("НЕТЕКОВНИ СРЕДСТВА_LastYear", BilansNaSostojbaNetekovniSredstvaLastYearNode.InnerText);
            res.Add("НЕТЕКОВНИ СРЕДСТВА", BilansNaSostojbaNetekovniSredstvaCurYearNode.InnerText);
            res.Add("НЕТЕКОВНИ СРЕДСТВА_Ind", BilansNaSostojbaNetekovniSredstvaIndNode.InnerText);
            res.Add("НЕТЕКОВНИ СРЕДСТВА_Procent_LastYear", BilansNaSostojbaNetekovniSredstvaProcentLastYearNode.InnerText);
            res.Add("НЕТЕКОВНИ СРЕДСТВА_Procent", BilansNaSostojbaNetekovniSredstvaProcentCurYearNode.InnerText);

            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_LastYear", BilansNaSostojbaOdlozeniDanocniObvrskiLastYearNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА", BilansNaSostojbaOdlozeniDanocniObvrskiCurYearNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_Ind", BilansNaSostojbaOdlozeniDanocniObvrskiIndNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_Procent_LastYear", BilansNaSostojbaOdlozeniDanocniObvrskiProcentLastYearNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА_Procent", BilansNaSostojbaOdlozeniDanocniObvrskiProcentCurYearNode.InnerText);

            res.Add("ТЕКОВНИ СРЕДСТВА_LastYear", BilansNaSostojbaTekovniSredstvaLastYearNode.InnerText);
            res.Add("ТЕКОВНИ СРЕДСТВА", BilansNaSostojbaTekovniSredstvaCurYearNode.InnerText);
            res.Add("ТЕКОВНИ СРЕДСТВА_Ind", BilansNaSostojbaTekovniSredstvaIndNode.InnerText);
            res.Add("ТЕКОВНИ СРЕДСТВА_Procent_LastYear", BilansNaSostojbaTekovniSredstvaProcentLastYearNode.InnerText);
            res.Add("ТЕКОВНИ СРЕДСТВА_Procent", BilansNaSostojbaTekovniSredstvaProcentCurYearNode.InnerText);

            res.Add("ЗАЛИХИ_LastYear", BilansNaSostojbaZalihiLastYearNode.InnerText);
            res.Add("ЗАЛИХИ", BilansNaSostojbaZalihiCurYearNode.InnerText);
            res.Add("ЗАЛИХИ_Ind", BilansNaSostojbaZalihiIndNode.InnerText);
            res.Add("ЗАЛИХИ_Procent_LastYear", BilansNaSostojbaZalihiProcentLastYearNode.InnerText);
            res.Add("ЗАЛИХИ_Procent", BilansNaSostojbaZalihiProcentCurYearNode.InnerText);

            res.Add("СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_LastYear", BilansNaSostojbaSredstvaGrupiLastYearNode.InnerText);
            res.Add("СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)", BilansNaSostojbaSredstvaGrupiCurYearNode.InnerText);
            res.Add("СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_Ind", BilansNaSostojbaSredstvaGrupiIndNode.InnerText);
            res.Add("СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_Procent_LastYear", BilansNaSostojbaSredstvaGrupiProcentLastYearNode.InnerText);
            res.Add("СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)_Procent", BilansNaSostojbaSredstvaGrupiProcentCurYearNode.InnerText);

            res.Add("ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_LastYear", BilansNaSostojbaPlateniTrosociLastYearNode.InnerText);
            res.Add("ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)", BilansNaSostojbaPlateniTrosociCurYearNode.InnerText);
            res.Add("ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_Ind", BilansNaSostojbaPlateniTrosociIndNode.InnerText);
            res.Add("ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_Procent_LastYear", BilansNaSostojbaPlateniTrosociProcentLastYearNode.InnerText);
            res.Add("ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)_Procent", BilansNaSostojbaPlateniTrosociProcentCurYearNode.InnerText);

            res.Add("Д. ВКУПНА АКТИВА_LastYear", BilansNaSostojbaDVkupnaAktivaLastYearNode.InnerText);
            res.Add("Д. ВКУПНА АКТИВА", BilansNaSostojbaDVkupnaAktivaCurYearNode.InnerText);
            res.Add("Д. ВКУПНА АКТИВА_Ind", BilansNaSostojbaDVkupnaAktivaIndNode.InnerText);
            res.Add("Д. ВКУПНА АКТИВА_Procent_LastYear", BilansNaSostojbaDVkupnaAktivaProcentLastYearNode.InnerText);
            res.Add("Д. ВКУПНА АКТИВА_Procent", BilansNaSostojbaDVkupnaAktivaProcentCurYearNode.InnerText);

            return res;
        }

        public Dictionary<string, string> GetBilansNaSostojbaPasiva(HtmlAgilityPack.HtmlDocument doc, string ValuesXpath)
        {
            var BilansNaSostojbaXpath = ValuesXpath + "/table[15]/tbody[1]";

            var BilansNaSostojbaGlavninaIRezerviXpath = BilansNaSostojbaXpath + "/tr[2]";
            var BilansNaSostojbaObvrskiXpath = BilansNaSostojbaXpath + "/tr[3]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaXpath = BilansNaSostojbaXpath + "/tr[4]";
            var BilansNaSostojbaOdlozenoPlakanjeTrosociXpath = BilansNaSostojbaXpath + "/tr[5]";
            var BilansNaSostojbaObvrskiPoOsnovXpath = BilansNaSostojbaXpath + "/tr[6]";
            var BilansNaSostojbaDVkupnaPasivaXpath = BilansNaSostojbaXpath + "/tr[7]";

            // 1
            var BilansNaSostojbaGlavninaIRezerviLastYearXpath = BilansNaSostojbaGlavninaIRezerviXpath + "/td[2]";
            var BilansNaSostojbaGlavninaIRezerviCurYearXpath = BilansNaSostojbaGlavninaIRezerviXpath + "/td[3]";
            var BilansNaSostojbaGlavninaIRezerviIndXpath = BilansNaSostojbaGlavninaIRezerviXpath + "/td[4]";
            var BilansNaSostojbaGlavninaIRezerviProcentLastYearXpath = BilansNaSostojbaGlavninaIRezerviXpath + "/td[5]";
            var BilansNaSostojbaGlavninaIRezerviProcentCurYearXpath = BilansNaSostojbaGlavninaIRezerviXpath + "/td[6]";

            var BilansNaSostojbaGlavninaIRezerviLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaGlavninaIRezerviLastYearXpath);
            var BilansNaSostojbaGlavninaIRezerviCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaGlavninaIRezerviCurYearXpath);
            var BilansNaSostojbaGlavninaIRezerviIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaGlavninaIRezerviIndXpath);
            var BilansNaSostojbaGlavninaIRezerviProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaGlavninaIRezerviProcentLastYearXpath);
            var BilansNaSostojbaGlavninaIRezerviProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaGlavninaIRezerviProcentCurYearXpath);

            // 2
            var BilansNaSostojbaObvrskiLastYearXpath = BilansNaSostojbaObvrskiXpath + "/td[2]";
            var BilansNaSostojbaObvrskiCurYearXpath = BilansNaSostojbaObvrskiXpath + "/td[3]";
            var BilansNaSostojbaObvrskiIndXpath = BilansNaSostojbaObvrskiXpath + "/td[4]";
            var BilansNaSostojbaObvrskiProcentLastYearXpath = BilansNaSostojbaObvrskiXpath + "/td[5]";
            var BilansNaSostojbaObvrskiProcentCurYearXpath = BilansNaSostojbaObvrskiXpath + "/td[6]";

            var BilansNaSostojbaObvrskiLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiLastYearXpath);
            var BilansNaSostojbaObvrskiCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiCurYearXpath);
            var BilansNaSostojbaObvrskiIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiIndXpath);
            var BilansNaSostojbaObvrskiProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiProcentLastYearXpath);
            var BilansNaSostojbaObvrskiProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiProcentCurYearXpath);

            // 3
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaLastYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiPasivaXpath + "/td[2]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaCurYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiPasivaXpath + "/td[3]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaIndXpath = BilansNaSostojbaOdlozeniDanocniObvrskiPasivaXpath + "/td[4]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentLastYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiPasivaXpath + "/td[5]";
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentCurYearXpath = BilansNaSostojbaOdlozeniDanocniObvrskiPasivaXpath + "/td[6]";

            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaLastYearXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaCurYearXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaIndXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentLastYearXpath);
            var BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentCurYearXpath);

            // 4
            var BilansNaSostojbaOdlozenoPlakanjeTrosociLastYearXpath = BilansNaSostojbaOdlozenoPlakanjeTrosociXpath + "/td[2]";
            var BilansNaSostojbaOdlozenoPlakanjeTrosociCurYearXpath = BilansNaSostojbaOdlozenoPlakanjeTrosociXpath + "/td[3]";
            var BilansNaSostojbaOdlozenoPlakanjeTrosociIndXpath = BilansNaSostojbaOdlozenoPlakanjeTrosociXpath + "/td[4]";
            var BilansNaSostojbaOdlozenoPlakanjeTrosociProcentLastYearXpath = BilansNaSostojbaOdlozenoPlakanjeTrosociXpath + "/td[5]";
            var BilansNaSostojbaOdlozenoPlakanjeTrosociProcentCurYearXpath = BilansNaSostojbaOdlozenoPlakanjeTrosociXpath + "/td[6]";

            var BilansNaSostojbaOdlozenoPlakanjeTrosociLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozenoPlakanjeTrosociLastYearXpath);
            var BilansNaSostojbaOdlozenoPlakanjeTrosociCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozenoPlakanjeTrosociCurYearXpath);
            var BilansNaSostojbaOdlozenoPlakanjeTrosociIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozenoPlakanjeTrosociIndXpath);
            var BilansNaSostojbaOdlozenoPlakanjeTrosociProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozenoPlakanjeTrosociProcentLastYearXpath);
            var BilansNaSostojbaOdlozenoPlakanjeTrosociProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaOdlozenoPlakanjeTrosociProcentCurYearXpath);

            // 5
            var BilansNaSostojbaObvrskiPoOsnovLastYearXpath = BilansNaSostojbaObvrskiPoOsnovXpath + "/td[2]";
            var BilansNaSostojbaObvrskiPoOsnovCurYearXpath = BilansNaSostojbaObvrskiPoOsnovXpath + "/td[3]";
            var BilansNaSostojbaObvrskiPoOsnovIndXpath = BilansNaSostojbaObvrskiPoOsnovXpath + "/td[4]";
            var BilansNaSostojbaObvrskiPoOsnovProcentLastYearXpath = BilansNaSostojbaObvrskiPoOsnovXpath + "/td[5]";
            var BilansNaSostojbaObvrskiPoOsnovProcentCurYearXpath = BilansNaSostojbaObvrskiPoOsnovXpath + "/td[6]";

            var BilansNaSostojbaObvrskiPoOsnovLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiPoOsnovLastYearXpath);
            var BilansNaSostojbaObvrskiPoOsnovCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiPoOsnovCurYearXpath);
            var BilansNaSostojbaObvrskiPoOsnovIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiPoOsnovIndXpath);
            var BilansNaSostojbaObvrskiPoOsnovProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiPoOsnovProcentLastYearXpath);
            var BilansNaSostojbaObvrskiPoOsnovProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaObvrskiPoOsnovProcentCurYearXpath);

            // 6
            var BilansNaSostojbaDVkupnaPasivaLastYearXpath = BilansNaSostojbaDVkupnaPasivaXpath + "/td[2]";
            var BilansNaSostojbaDVkupnaPasivaCurYearXpath = BilansNaSostojbaDVkupnaPasivaXpath + "/td[3]";
            var BilansNaSostojbaDVkupnaPasivaIndXpath = BilansNaSostojbaDVkupnaPasivaXpath + "/td[4]";
            var BilansNaSostojbaDVkupnaPasivaProcentLastYearXpath = BilansNaSostojbaDVkupnaPasivaXpath + "/td[5]";
            var BilansNaSostojbaDVkupnaPasivaProcentCurYearXpath = BilansNaSostojbaDVkupnaPasivaXpath + "/td[6]";

            var BilansNaSostojbaDVkupnaPasivaLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaPasivaLastYearXpath);
            var BilansNaSostojbaDVkupnaPasivaCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaPasivaCurYearXpath);
            var BilansNaSostojbaDVkupnaPasivaIndNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaPasivaIndXpath);
            var BilansNaSostojbaDVkupnaPasivaProcentLastYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaPasivaProcentLastYearXpath);
            var BilansNaSostojbaDVkupnaPasivaProcentCurYearNode = doc.DocumentNode.SelectSingleNode(BilansNaSostojbaDVkupnaPasivaProcentCurYearXpath);

            var res = new Dictionary<string, string>();

            res.Add("ГЛАВНИНА И РЕЗЕРВИ_LastYear", BilansNaSostojbaGlavninaIRezerviLastYearNode.InnerText);
            res.Add("ГЛАВНИНА И РЕЗЕРВИ", BilansNaSostojbaGlavninaIRezerviCurYearNode.InnerText);
            res.Add("ГЛАВНИНА И РЕЗЕРВИ_Ind", BilansNaSostojbaGlavninaIRezerviIndNode.InnerText);
            res.Add("ГЛАВНИНА И РЕЗЕРВИ_Procent_LastYear", BilansNaSostojbaGlavninaIRezerviProcentLastYearNode.InnerText);
            res.Add("ГЛАВНИНА И РЕЗЕРВИ_Procent", BilansNaSostojbaGlavninaIRezerviProcentCurYearNode.InnerText);

            res.Add("ОБВРСКИ_LastYear", BilansNaSostojbaObvrskiLastYearNode.InnerText);
            res.Add("ОБВРСКИ", BilansNaSostojbaObvrskiCurYearNode.InnerText);
            res.Add("ОБВРСКИ_Ind", BilansNaSostojbaObvrskiIndNode.InnerText);
            res.Add("ОБВРСКИ_Procent_LastYear", BilansNaSostojbaObvrskiProcentLastYearNode.InnerText);
            res.Add("ОБВРСКИ_Procent", BilansNaSostojbaObvrskiProcentCurYearNode.InnerText);

            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_LastYear", BilansNaSostojbaOdlozeniDanocniObvrskiPasivaLastYearNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА", BilansNaSostojbaOdlozeniDanocniObvrskiPasivaCurYearNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_Ind", BilansNaSostojbaOdlozeniDanocniObvrskiPasivaIndNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_Procent_LastYear", BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentLastYearNode.InnerText);
            res.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА_Procent", BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentCurYearNode.InnerText);

            res.Add("ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_LastYear", BilansNaSostojbaOdlozenoPlakanjeTrosociLastYearNode.InnerText);
            res.Add("ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)", BilansNaSostojbaOdlozenoPlakanjeTrosociCurYearNode.InnerText);
            res.Add("ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_Ind", BilansNaSostojbaOdlozenoPlakanjeTrosociIndNode.InnerText);
            res.Add("ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_Procent_LastYear", BilansNaSostojbaOdlozenoPlakanjeTrosociProcentLastYearNode.InnerText);
            res.Add("ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)_Procent", BilansNaSostojbaOdlozenoPlakanjeTrosociProcentCurYearNode.InnerText);
            
            res.Add("ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_LastYear", "0");
            if (BilansNaSostojbaObvrskiPoOsnovLastYearNode != null)
                res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_LastYear"] = BilansNaSostojbaObvrskiPoOsnovLastYearNode.InnerText;

            res.Add("ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА", "0");
            if (BilansNaSostojbaObvrskiPoOsnovCurYearNode != null && System.Text.RegularExpressions.Regex.IsMatch(BilansNaSostojbaObvrskiPoOsnovCurYearNode.InnerText, @"\d"))
                res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА"] = BilansNaSostojbaObvrskiPoOsnovCurYearNode.InnerText;

            res.Add("ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Ind", "0,0");
            if (BilansNaSostojbaObvrskiPoOsnovIndNode != null && System.Text.RegularExpressions.Regex.IsMatch(BilansNaSostojbaObvrskiPoOsnovIndNode.InnerText, @"\d"))
                res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Ind"] = BilansNaSostojbaObvrskiPoOsnovIndNode.InnerText;

            res.Add("ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Procent_LastYear", "0,0");
            if (BilansNaSostojbaObvrskiPoOsnovProcentLastYearNode != null && System.Text.RegularExpressions.Regex.IsMatch(BilansNaSostojbaObvrskiPoOsnovProcentLastYearNode.InnerText, @"\d"))
                res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Procent_LastYear"] = BilansNaSostojbaObvrskiPoOsnovProcentLastYearNode.InnerText;

            res.Add("ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Procent", "0,0");
            if (BilansNaSostojbaObvrskiPoOsnovProcentCurYearNode != null && System.Text.RegularExpressions.Regex.IsMatch(BilansNaSostojbaObvrskiPoOsnovProcentCurYearNode.InnerText, @"\d"))
                res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Procent"] = BilansNaSostojbaObvrskiPoOsnovProcentCurYearNode.InnerText;

            if (BilansNaSostojbaDVkupnaPasivaLastYearNode != null && System.Text.RegularExpressions.Regex.IsMatch(BilansNaSostojbaDVkupnaPasivaLastYearNode.InnerText, @"\d"))
                res["Д. ВКУПНО ПАСИВА_LastYear"] = BilansNaSostojbaDVkupnaPasivaLastYearNode.InnerText;
            else 
            {
                var g_z_ly = Convert.ToInt64(BilansNaSostojbaGlavninaIRezerviLastYearNode.InnerText.Replace(".", "").Trim());
                var o_ly = Convert.ToInt64(BilansNaSostojbaObvrskiLastYearNode.InnerText.Replace(".", "").Trim());
                var o_d_o_ly = Convert.ToInt64(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaLastYearNode.InnerText.Replace(".", "").Trim());
                var o_p_ly = Convert.ToInt64(BilansNaSostojbaOdlozenoPlakanjeTrosociLastYearNode.InnerText.Replace(".", "").Trim());
                var o_osnov_ly = Convert.ToInt64(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_LastYear"].Trim());

                var total = g_z_ly + o_ly + o_d_o_ly + o_p_ly + o_osnov_ly;

                res.Add("Д. ВКУПНО ПАСИВА_LastYear", total.ToString());
            }

            if (BilansNaSostojbaDVkupnaPasivaCurYearNode != null)
                res.Add("Д. ВКУПНО ПАСИВА", BilansNaSostojbaDVkupnaPasivaCurYearNode.InnerText);
            else
            {
                var g_z = Convert.ToInt64(BilansNaSostojbaGlavninaIRezerviCurYearNode.InnerText.Replace(".", "").Trim());
                var o = Convert.ToInt64(BilansNaSostojbaObvrskiCurYearNode.InnerText.Replace(".", "").Trim());
                var o_d_o = Convert.ToInt64(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaCurYearNode.InnerText.Replace(".", "").Trim());
                var o_p = Convert.ToInt64(BilansNaSostojbaOdlozenoPlakanjeTrosociCurYearNode.InnerText.Replace(".", "").Trim());
                var o_osnov = Convert.ToInt64(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА"].Trim());

                var total = g_z + o + o_d_o + o_p + o_osnov;

                res.Add("Д. ВКУПНО ПАСИВА", total.ToString());
            }

            if (BilansNaSostojbaDVkupnaPasivaIndNode != null)
                res.Add("Д. ВКУПНО ПАСИВА_Ind", BilansNaSostojbaDVkupnaPasivaIndNode.InnerText);
            else
            {
                var ly = Convert.ToInt64(res["Д. ВКУПНО ПАСИВА_LastYear"]);
                var cy = Convert.ToInt64(res["Д. ВКУПНО ПАСИВА"]);


                double total = ((double)cy / (double)ly) * 100;

                res.Add("Д. ВКУПНО ПАСИВА_Ind", Math.Round(total, 2).ToString());
            }

            if (BilansNaSostojbaDVkupnaPasivaProcentLastYearNode != null)
                res.Add("Д. ВКУПНО ПАСИВА_Procent_LastYear", BilansNaSostojbaDVkupnaPasivaProcentLastYearNode.InnerText);
            else
            {
                var p_ly = Convert.ToDouble(BilansNaSostojbaGlavninaIRezerviProcentLastYearNode.InnerText.Replace(".", "").Trim());
                var o_p_ly = Convert.ToDouble(BilansNaSostojbaObvrskiProcentLastYearNode.InnerText.Replace(".", "").Trim());
                var d_p_ly = Convert.ToDouble(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaLastYearNode.InnerText.Replace(".", "").Trim());
                var t_p_ly = Convert.ToDouble(BilansNaSostojbaOdlozenoPlakanjeTrosociProcentLastYearNode.InnerText.Replace(".", "").Trim());
                var o_o_p_ly = Convert.ToDouble(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Procent_LastYear"].Trim());

                var total = p_ly + o_p_ly + d_p_ly + t_p_ly + o_o_p_ly;

                res.Add("Д. ВКУПНО ПАСИВА_Procent_LastYear", total.ToString());
            }

            if (BilansNaSostojbaDVkupnaPasivaProcentCurYearNode != null)
                res.Add("Д. ВКУПНО ПАСИВА_Procent", BilansNaSostojbaDVkupnaPasivaProcentCurYearNode.InnerText);
            else
            {
                var g_z_ly = Convert.ToDouble(BilansNaSostojbaGlavninaIRezerviProcentCurYearNode.InnerText.Replace(".", "").Trim());
                var o_ly = Convert.ToDouble(BilansNaSostojbaObvrskiProcentCurYearNode.InnerText.Replace(".", "").Trim());
                var o_d_o_ly = Convert.ToDouble(BilansNaSostojbaOdlozeniDanocniObvrskiPasivaProcentCurYearNode.InnerText.Replace(".", "").Trim());
                var o_p_ly = Convert.ToDouble(BilansNaSostojbaOdlozenoPlakanjeTrosociProcentCurYearNode.InnerText.Replace(".", "").Trim());
                var o_osnov_ly = Convert.ToDouble(res["ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА_Procent"].Trim());

                var total = g_z_ly + o_ly + o_d_o_ly + o_p_ly + o_osnov_ly;

                res.Add("Д. ВКУПНО ПАСИВА_Procent", total.ToString());
            }
            res["Д. ВКУПНО ПАСИВА_LastYear"] = FormatCurrency(res["Д. ВКУПНО ПАСИВА_LastYear"]);
            res["Д. ВКУПНО ПАСИВА"] = FormatCurrency(res["Д. ВКУПНО ПАСИВА"]);

            return res;
        }

        private static string FormatCurrency(string Number)
        {
            long a = 0;
            if (long.TryParse(Number, out a))
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo("mk-MK"), "{0:N0}", a);

            return Number;
        }



        List<string[]> ErrorReports;

        string[] filenames;

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            Total.Text = "0";
            New.Text = "0";


            ErrorReports = new List<string[]>();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Html-Files(*.html)|*.html";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                filenames = openFileDialog1.FileNames;
                counterTotal = openFileDialog1.FileNames.Length;

                UpdateFormData();

                Task t = new Task(new Action(StartScraping));
                t.Start();

            }
            
        }


        public void StartScraping()
        {
            UpdateFormData();

            foreach (string file in filenames)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    using (StreamReader sr = new StreamReader(file))
                    {
                        String line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            sb.Append(line);
                        }
                    }

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

                    try
                    {
                        doc.LoadHtml(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }

                    var Values = GetAllValuesFromDocument(doc);

                    if (Values != null)
                        DALHelper.CreateCompanyReport(Values);

                }
                catch (Exception ex)
                {
                    ErrorReports.Add(new string[] { file, ex.Message });
                }

                UpdateFormData();
            }
        }

        private void UpdateFormData()
        {

            if (Total.InvokeRequired)
            {
                Total.BeginInvoke(new MethodInvoker(() => UpdateFormData()));
            }
            else
            {
                Error.Text = ErrorReports.Count.ToString();
                New.Text = counterNew.ToString();
                Old.Text = counterOld.ToString();
                Total.Text = counterTotal.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Error_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogMsg tmp = new DialogMsg();

            var report = "";
            if (ErrorReports != null)
            {
                var res = ErrorReports.Select(c => c[0] + " - " + c[1]);

                report = string.Join(Environment.NewLine, res);
            }
            tmp.ShowReports(report);

            tmp.ShowDialog();
        }

        private void ShowEditList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OldValues tmp = new OldValues();

            if (listOldValues != null)
            {
                tmp.listOldValues = listOldValues;
                tmp.PopulateList();
            }

            tmp.ShowDialog();
        }
    }
}
