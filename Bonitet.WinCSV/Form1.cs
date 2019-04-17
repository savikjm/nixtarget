﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bonitet.DAL;
using System.IO;

namespace Bonitet.WinCSV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var embs = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(embs) == false) {
                GenerateCSV(embs);
            }
        }

        public void GenerateCSV(string EMBS)
        {
            var curyear = DALHelper.GetCurrentYear(false);

            var values = DALHelper.GetCompanyValuesByEMBS(EMBS);
            if (values == null) {
                return;
            }
            var res = "Year^EMBS^AccountNo^AccountName^Previous^CurrentYear\r\n";

            foreach (var item in _myDict)
	        {
		        var curItems = values.Where(c=>c.ValueID == item.Key).ToList();

                var Value = 0.0;
                var LastYearValue = 0.0;

                CompanyYear curYear = null;
                CompanyYear lastYear = null;
                foreach (var curVal in curItems)
	            {

                    var tmpYear = DALHelper.GetYearByID(curVal.YearID);
                    if(tmpYear.Year == curyear)
                    {
                        curYear = tmpYear;
                        Value = curVal.Value;
                    }
                    else
                    {
                        lastYear = tmpYear;
                        LastYearValue = curVal.Value;
                    }
	            }

                if (curItems.Count() > 0)
                {
                    //if (curYear == null) {
                    //    var tmp = lastYear;
                    //    var tmp1 = LastYearValue;

                    //    curYear = tmp;
                    //    Value = tmp1;

                    //    lastYear = null;
                    //    LastYearValue = 0.0;
                    //}
                    res += GenerateCSVItem(curYear.Year, EMBS, item.Key, item.Value, LastYearValue, Value);
                }
	        }

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.Title = "Save CSV File"; 
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, res);
            }
        }

        public string GenerateCSVItem(int? Year, string EMBS, int AccountNo, string AccountName, double LastYearValue, double Value)
        {
            var Text = Year + "^" + EMBS + "^" + AccountNo + "^" + AccountName + "^" + LastYearValue + "^" + Value + "\r\n";
            return Text;
        }

        private static Dictionary<int, string> _myDict = new Dictionary<int, string>
        {
            {1, "АКТИВА: А.НЕТЕКОВНИ СРЕДСТВА (002+009+020+021+031)"},
            {2, "I.НЕМАТЕРИЈАЛНИ СРЕДСТВА (003+004+005+006+007+008)"},
            {3, "Издатоци за развој"},
            {4, "Kонцесии, патенти, лиценци, заштитни знаци и слични права"},
            {5, "Гудвил"},
            {6, "Аванси за набавка на  нематеријални средства"},
            {7, "Нематеријални средства во подготовка"},
            {8, "Останати нематеријални средства"},
            {9, "II. МАТЕРИЈАЛНИ СРЕДСТВА (010+013+014+015+016+017+018+019)"},
            {10, "Недвижности (011+012)"},
            {11, "Земјиште"},
            {12, "Градежни објекти"},
            {13, "Постројки и опрема"},
            {14, "Транспортни средства"},
            {15, "Алат, погонски и канцелариски инвентар и мебел"},
            {16, "Биолошки средства"},
            {17, "Аванси за набавка на материјални средства"},
            {18, "Материјални средства во подготовка"},
            {19, "Останати материјални средства"},
            {20, "III. ВЛОЖУВАЊА ВО НЕДВИЖНОСТИ"},
            {21, "IV. ДОЛГОРОЧНИ ФИНАНСИСКИ СРЕДСТВА (022+023+024+025+026+030)"},
            {22, "Вложувања во подружници"},
            {23, "Вложувања во придружени друштва и учества во заеднички вложувања"},
            {24, "Побарувања по дадени долгорочни заеми на поврзани друштва"},
            {25, "Побарувања по дадени долгорочни заеми"},
            {26, "Вложувања во долгорочни хартии од вредност (027+028+029)"},
            {27, "Вложувања во хартии од вредност кои се чуваат до доспевање"},
            {28, "Вложувања во хартии од вредност расположливи за продажба"},
            {29, "Вложувања во хартии од вредност според објективната вредност преку добивката или загубата"},
            {30, "Останати долгорочни финансиски средства"},
            {31, "V. ДОЛГОРОЧНИ ПОБАРУВАЊА (032+033+034)"},
            {32, "Побарувања од поврзани друштва"},
            {33, "Побарувања од купувачи"},
            {34, "Останати долгорочни побарувања"},
            {35, "VI. ОДЛОЖЕНИ ДАНОЧНИ СРЕДСТАВА"},
            {36, "Б. ТЕКОВНИ СРЕДСТВА (037+045+052+059)"},
            {37, "I. ЗАЛИХИ   (038+039+040+041+042+043)"},
            {38, "Залихи на суровини и материјали"},
            {39, "Залихи на резервни делови, ситен инвентар, амбалажа и автогуми"},
            {40, "Залихи на недовршени производи и полупроизводи"},
            {41, "Залихи на  готови производи"},
            {42, "Залихи на трговски стоки"},
            {43, "Залихи на биолошки средства"},
            {44, "II.СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)"},
            {45, "III. КРАТКОРОЧНИ ПОБАРУВАЊА (046+047+048+049+050+051)"},
            {46, "Побарувања од поврзани друштва"},
            {47, "Побарувања од купувачи"},
            {48, "Побарувања за дадени аванси на добавувачи"},
            {49, "Побарувања од државата по основ на даноци, придонеси, царина, акцизи и за останати давачки кон државата (претплати)"},
            {50, "Побарувања од вработените"},
            {51, "Останати краткорочни побарувања"},
            {52, "IV. КРАТКОРОЧНИ ФИНАНСИСКИ СРЕДСТВА (053+056+057+058)"},
            {53, "Вложувања во хартии од вредност (054+055)"},
            {54, "Вложувања кои се чуваат до доспевање"},
            {55, "Вложувања според објективната вредност преку добивката и загубата"},
            {56, "Побарувања по дадени заеми од поврзани друштва"},
            {57, "Побарувања по дадени заеми"},
            {58, "Останати краткорочни финансиски средства"},
            {59, "V. ПАРИЧНИ СРЕДСТВА И  ПАРИЧНИ ЕКВИВАЛЕНТИ (060+061)"},
            {60, "Парични средства"},
            {61, "Парични еквиваленти"},
            {62, "VI. ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ (АВР)"},
            {63, "ВКУПНА АКТИВА:                           СРЕДСТВА (001+035+036+044+062)"},
            {64, "В. ВОНБИЛАНСНА ЕВИДЕНЦИЈА - АКТИВА"},
            {65, "ПАСИВА : А. ГЛАВНИНА И РЕЗЕРВИ (066+067-068-069+070+071+075-076+077-078)"},
            {66, "I. ОСНОВНА ГЛАВНИНА"},
            {67, "II. ПРЕМИИ НА ЕМИТИРАНИ АКЦИИ"},
            {68, "III. СОПСТВЕНИ АКЦИИ (-)"},
            {69, "IV. ЗАПИШАН, НЕУПЛАТЕН КАПИТАЛ (-)"},
            {70, "V. РЕВАЛОРИЗАЦИСКА РЕЗЕРВА И РАЗЛИКИ ОД ВРЕДНУВАЊЕ НА КОМПОНЕНТИ НА ОСТАНАТА СЕОПФАТНА ДОБИВКА"},
            {71, "VI. РЕЗЕРВИ (072+073+074)"},
            {72, "Законски резерви"},
            {73, "Статутарни резерви"},
            {74, "Останати резерви"},
            {75, "VII. АКУМУЛИРАНА ДОБИВКА"},
            {76, "VIII. ПРЕНЕСЕНА ЗАГУБА (-)"},
            {77, "IX. ДОБИВКА ЗА ДЕЛОВНАТА ГОДИНА"},
            {78, "X. ЗАГУБА ЗА ДЕЛОВНАТА ГОДИНА"},
            {79, "XI. ГЛАВНИНА НА СОПСТВЕНИЦИТЕ НА МАТИЧНОТО ДРУШТВО"},
            {80, "XII. НЕКОНТРОЛИРАНО УЧЕСТВО"},
            {81, "Б. ОБВРСКИ                           (082+085+095)"},
            {82, "I. ДОЛГОРОЧНИ РЕЗЕРВИРАЊА ЗА РИЗИЦИ И ТРОШОЦИ     (083+084)"},
            {83, "Резервирања за пензии, отпремнини и слични обврски кон вработените"},
            {84, "Останати долгорочни резервирања за ризици и трошоци"},
            {85, "II. ДОЛГОРОЧНИ ОБВРСКИ                         (од 086 до 093)"},
            {86, "Обврски спрема поврзани друштва"},
            {87, "Обврски спрема добавувачи"},
            {88, "Обврски за аванси, депозити и кауции"},
            {89, "Обврски по заеми и кредити спрема поврзани друштва"},
            {90, "Обврски по заеми и кредити"},
            {91, "Обврски по хартии од вредност"},
            {92, "Останати финансиски обврски"},
            {93, "Останати долгорочни обврски"},
            {94, "III. ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ"},
            {95, "IV. КРАТКОРОЧНИ ОБВРСКИ                    (од 096 до 108)"},
            {96, "Обврски спрема поврзани друштва"},
            {97, "Обврски спрема добавувачи"},
            {98, "Обврски за аванси, депозити и кауции"},
            {99, "Обврски за даноци и придонеси на плата и на надомести на плати"},
            {100, "Обврски кон вработените"},
            {101, "Тековни даночни обврски"},
            {102, "Краткорочни резервирања за ризици и трошоци"},
            {103, "Обврски по заеми и кредити спрема поврзани друштва"},
            {104, "Обврски по заеми и кредити"},
            {105, "Обврски по хартии од вредност"},
            {106, "Обврски по основ на учество во резултатот"},
            {107, "Останати финансиски обврски"},
            {108, "Останати краткорочни обврски"},
            {109, "V. ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ  ПЕРИОДИ (ПВР)"},
            {110, "VI. ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА"},
            {111, "ВКУПНО ПАСИВА : ГЛАВНИНА, РЕЗЕРВИ И ОБВРСКИ (065+081+094+109+110)"},
            {112, "В. ВОНБИЛАНСНА ЕВИДЕНЦИЈА-ПАСИВА"},
            {201, "I. ПРИХОДИ ОД РАБОТЕЊЕТО (202+203+206)"},
            {202, "Приходи од продажба"},
            {203, "Останати приходи"},
            {204, "Залихи на  готови производи и недовршено производство на почетокот на годината"},
            {205, "Залихи на  готови производи и недовршено производство на крајот на годината"},
            {206, "Капитализирано сопственo производство и услуги"},
            {207, "II. РАСХОДИ ОД РАБОТЕЊЕТО (208+209+210+211+212+213+218+219+220+221+222)"},
            {208, "Трошоци за суровини и други материјали"},
            {209, "Набавна вредност на продадените стоки"},
            {210, "Набавна вредност на продадените материјали, резервни делови, ситен инвентар, амбалажа и автогуми"},
            {211, "Услуги со карактер на материјални трошоци"},
            {212, "Останати трошоци од работењето"},
            {213, "Трошоци за вработени (214+215+216+217)"},
            {214, "Плати и надоместоци на плата (нето)"},
            {215, "Трошоци за даноци на плати и надоместоци на плата"},
            {216, "Придонеси од задолжително социјално осигурување"},
            {217, "Останати трошоци за вработените"},
            {218, "Амортизација на материјалните и нематеријалните средства"},
            {219, "Вредносно усогласување (обезвреднување) на нетековни средства"},
            {220, "Вредносно усогласување (обезвреднување)  на тековните средства"},
            {221, "Резервирања за трошоци и ризици"},
            {222, "Останати расходи од работењето"},
            {223, "III. ФИНАНСИСКИ ПРИХОДИ (224+229+230+231+232+233)"},
            {224, "Финансиски приходи од односи со поврзани друштва (225+226+227+228)"},
            {225, "Приходи од вложувања во поврзани друштва"},
            {226, "Приходи по основ на камати од работење со поврзани  друштва"},
            {227, "Приходи по основ на курсни разлики од работење со поврзани  друштва"},
            {228, "Останати финансиски приходи од работење со поврзани  друштва"},
            {229, "Приходи од вложувања во неповрзани друштва"},
            {230, "Приходи по основ на камати од работење со неповрзани  друштва"},
            {231, "Приходи по основ на курсни разлики од работење со неповрзани  друштва"},
            {232, "Нереализирани добивки (приходи) од финансиски средства"},
            {233, "Останати финансиски приходи"},
            {234, "IV. ФИНАНСИСКИ РАСХОДИ (235+239+240+241+242+243)"},
            {235, "Финансиски расходи од односи со поврзани друштва (236+237+238)"},
            {236, "Расходи по основ на камати од работење со поврзани  друштва"},
            {237, "Расходи по основ на курсни разлики од работење со поврзани  друштва"},
            {238, "Останати финансиски расходи од поврзани  друштва"},
            {239, "Расходи по основ на камати од работење со неповрзани  друштва"},
            {240, "Расходи по основ на курсни разлики од работење со неповрзани  друштва"},
            {241, "Нереализирани загуба (расходи) од финансиски средства"},
            {242, "Вредносно усогласување на финансиски средства и вложувања"},
            {243, "Останати финансиски расходи"},
            {244, "Удел во добивката на придружените друштва"},
            {245, "Удел во загуба на придружените друштва"},
            {246, "Добивка од редовното работење (201+223+244)-(204-205+207+234+245)"},
            {247, "Загуба од редовното работење               (204-205+207+234+245)-(201+223+244)"},
            {248, "Нето добивка од прекинати работења"},
            {249, "Нето загуба од прекинати работења"},
            {250, "Добивка пред оданочување (246+248) или (246-249)"},
            {251, "Загуба пред оданочување (247+249) или (247-248)"},
            {252, "Данок на добивка"},
            {253, "Одложени даночни приходи"},
            {254, "Одложени даночни расходи"},
            {255, "НЕТО ДОБИВКА ЗА ДЕЛОВНАТА ГОДИНА (250-252+253-254)"},
            {256, "НЕТО ЗАГУБА ЗА ДЕЛОВНАТА ГОДИНА (251+252-253+254)"},
            {257, "Просечен број на вработени врз основа на часови на работа во пресметковниот период (во апсолутен износ)"},
            {258, "Број на месеци на работење (во апсолутен износ)"},
            {259, "ДОБИВКА/ЗАГУБА  ЗА ПЕРИОД"},
            {260, "Добивка која им припаѓа на имателите на акции на матичното друштво"},
            {261, "Добивка која им припаѓа на неконтролирано учество"},
            {262, "Загуба која се однесува на имателите на акции на матичното друштво"},
            {263, "Загуба која се однесува на неконтролирано учество"},
            {264, "ЗАРАБОТУВАЧКА ПО АКЦИЈА"},
            {265, "Вкупна основна заработувачка по акција"},
            {266, "Вкупна разводнета заработувачка по акција"},
            {267, "Основна заработувачка по акција од прекинато работење"},
            {268, "Разводнета заработувачка по акција од прекинато работење"},
            {269, " Добивка за годината"},
            {270, "Загуба за годината"},
            {271, "Останата сеопфатна добивка (273+275+277+279+281+283) - (274+276+278+280+282+284)"},
            {272, "Останата сеопфатна загуба (274+276+278+280+282+284) - (273+275+277+279+281+283)"},
            {273, "Добивки кои произлегуваат од преведување од странско работење"},
            {274, "Загуби кои произлегуваат од преведување од странско работење"},
            {275, "Добивки од повторно мерење на финансиски средства расположливи за продажба"},
            {276, "Загуби од повторно мерење на финансиски средства расположливи за продажба"},
            {277, "Ефективен дел од добивки од хеџинг инструменти за хеџирање на парични текови"},
            {278, "Ефективен дел од загуби од хеџинг инструменти за хеџирање на парични текови"},
            {279, "Промени на ревалоризациските резерви за нетековни средства"},
            {280, "Промени на ревалоризациските резерви за нетековни средства"},
            {281, "Актуарски добивки на дефинирани планови за користи на вработените"},
            {282, "Актуарски загуби на дефинирани планови за користи на вработените"},
            {283, "Удел во останата сеопфатна добивка на придружени друштва (само за потреби на консолидација)"},
            {284, "Удел во останата сеопфатна загуби на придружени друштва (само за потреби на консолидација)"},
            {285, "Данок на добивка на компоненти на останата сеопфатна добивка"},
            {286, "Нето останата сеопфатна добивка               (271-285)"},
            {287, "Нето останата сеопфатна загуби                (285-271) или (272+285)"},
            {288, "Вкупна сеопфатна добивка за годината (269+286) или  (286-270)"},
            {289, "Сеопфатна добивка која им припаѓа на имателите на акции на матичното друштво"},
            {290, "Сеопфатна добивка која припаѓа на неконтролираното учество"},
            {291, "Вкупна сеопфатна загуба за годината (270+287) или (270-286) или (287-269)"},
            {292, "Сеопфатна загуба која им припаѓа на имателите на акции на матичното друштво"},
            {293, "Сеопфатна загуба која припаѓа на неконтролираното учество"}
        };
    }
}
