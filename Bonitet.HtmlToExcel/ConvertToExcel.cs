using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bonitet.HtmlToExcel
{
    public class ConvertToExcel 
    {
        public static int rowCounter = 1;
       // public static Dictionary<String, String> excelCurrentYearObject = new Dictionary<String, String>();
        //public static Dictionary<String, String> excelPrevYearObject = new Dictionary<String, String>();
     
        public static Dictionary<String, String> ValueNames;
        public static ExcelPackage pck;
        public static ExcelWorksheet worksheet;
        public SaveFileDialog saveFileDialog1;
        public ExcelObject excelCurrentYearObject;
        public ExcelObject excelPrevYearObject;

        private String EMBS = "rename_me";

        public ConvertToExcel()
        {
                excelCurrentYearObject = new ExcelObject();
                excelPrevYearObject = new ExcelObject();
                initExcel();
                ValueNames = generateValueNames();
        }

        public void addToExcel(Dictionary<string, string> dir)
        {
            var values = dir;
            MapKeysToProperValueIDInDB(dir);
            if (rowCounter == 1)
            {
                addHeaders();
            }
            addObjectInExcel(excelCurrentYearObject.getRow());
            addObjectInExcel(excelPrevYearObject.getRow());
        }

        public void initExcel()
        {
            pck = new ExcelPackage();
            worksheet = pck.Workbook.Worksheets.Add("Content");
            worksheet.View.ShowGridLines = true;
        }

        public void createExcel()
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save CSV File";
            saveFileDialog1.FileName = EMBS + ".xlsx";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    pck.SaveAs(fs);
                }
            }

        }

        public void addHeaders()
        {
            var counter = 1;
            foreach (var item in excelCurrentYearObject.getHeaders())
            {
                worksheet.Cells[rowCounter, counter++].Value = item;
            }
            rowCounter++;
        }

        public void addObjectInExcel(Dictionary<string, string> Object)
        {
            var counter = 1;
            foreach (var item in Object)
            {
                worksheet.Cells[rowCounter, counter++].Value = item.Value;
            }
            rowCounter++;
        }

        private void MapKeysToProperValueIDInDB(Dictionary<string, string> Values)
        {
            
            foreach (var item in Values)
            {
                //cur year
                if (item.Key.Contains("_LastYear") == false)
                {
                    var curKey = item.Key;

                    if (curKey.Equals("Year") || curKey.Equals("EMBS"))
                    {
                        if (curKey.Equals("EMBS"))
                        {
                            EMBS = item.Value;

                            excelCurrentYearObject.setElementValue(curKey,item.Value);
                            excelPrevYearObject.setElementValue(curKey, item.Value);
                        }
                        else
                        {
                            excelCurrentYearObject.setElementValue(curKey, item.Value);
                            excelPrevYearObject.setElementValue(curKey, Int32.Parse(item.Value) - 1 + "");
                        }
                    }
                    else if (ValueNames.Keys.Contains(curKey))
                    {
                        var valueID = ValueNames[curKey];
                        // add to map list
                        excelCurrentYearObject.setElementValue(valueID,item.Value);
                    }
                }
                else
                {
                    //last year
                    var curKey = item.Key.Split('_')[0];
                    if (ValueNames.Keys.Contains(curKey))
                    {
                        var valueID = ValueNames[curKey];
                        // add to map list
                        excelPrevYearObject.setElementValue(valueID, item.Value);
                    }
                }
            }

        }


        private Dictionary<string, string> generateValueNames()
        {
            /* 
               ID	Name	Type	Description
               x 301	Вкупни приходи	2	NULL
               x 302	Вкупни расходи	2	NULL
            */
            //[1] as v301,[35] as v302, [36] as v303, [37] as v304, [44] as v305, [62] as v306, [63] as v307, [65] as v308, [81] as v309, [94] as v310, [109] as v311, 
            //[110] as v312, [111] as v313, [255] as v314, [256] as v315, [257] as v316, [vk_prihodi] as v317, [vk_rashodi] as v318

            Dictionary<string, string> ValueNames = new Dictionary<string, string>();
            ValueNames.Add("Вкупни приходи", "301"); // 303	Добивка за финансиска година
            ValueNames.Add("Вкупни расходи", "302"); // 303	Добивка за финансиска година
            ValueNames.Add("Добивка за финансиска година", "303"); // 303	Добивка за финансиска година
            ValueNames.Add("Загуба за финансиска година", "304"); // 304	Добивка за финансиска година
            ValueNames.Add("Просечен број на вработени", "305"); //  305	Просечен број на вработени
            ValueNames.Add("НЕТЕКОВНИ СРЕДСТВА", "306"); // 306	НЕТЕКОВНИ СРЕДСТВА
            ValueNames.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА", "307"); //  307	ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ АКТИВА
            ValueNames.Add("ТЕКОВНИ СРЕДСТВА", "308"); // 308	ТЕКОВНИ СРЕДСТВА
            ValueNames.Add("ЗАЛИХИ", "309"); //  309	ЗАЛИХИ	
            ValueNames.Add("СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)", "310"); // 310	СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ НАМЕНЕТИ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА)
            ValueNames.Add("ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)", "311"); // 311	ПЛАТЕНИ ТРОШОЦИ ЗА ИДНИТЕ ПЕРИОДИ И ПРЕСМЕТАНИ ПРИХОДИ(АВР)
            ValueNames.Add("Д. ВКУПНА АКТИВА", "312"); // 312	Д. ВКУПНА АКТИВА
            ValueNames.Add("ГЛАВНИНА И РЕЗЕРВИ", "313"); // 313	ГЛАВНИНА И РЕЗЕРВИ
            ValueNames.Add("ОБВРСКИ", "314"); //  314	ОБВРСКИ	
            ValueNames.Add("ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА", "315"); // 315	ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ ПАСИВА
            ValueNames.Add("ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)", "316"); // 316	ОДЛОЖЕНО ПЛАЌАЊЕ НА ТРОШОЦИ И ПРИХОДИ ВО ИДНИТЕ ПЕРИОДИ (ПВР)
            ValueNames.Add("ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА", "317"); // 317	ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ) КОИ СЕ ЧУВААТ ЗА ПРОДАЖБА И ПРЕКИНАТИ РАБОТЕЊА
            ValueNames.Add("Д. ВКУПНО ПАСИВА", "318"); // 318	Д. ВКУПНО ПАСИВА*/

            return ValueNames;
        }



    }
}
