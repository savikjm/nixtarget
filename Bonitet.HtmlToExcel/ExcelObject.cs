using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonitet.HtmlToExcel
{
    public class ExcelObject
    {
        public Dictionary<String, String> row;

        public ExcelObject()
        {
            row = new Dictionary<string, string>();
            setHeaders();
        }

        public void setHeaders()
        {
            var  listOfHeaders = new List<String>{"EMBS","Year","301","302","303","304","305","306","307","308","309","310","311","312","313","314","315","316","317","318"};

            foreach (var item in listOfHeaders)
            {
                row.Add(item, 0+"");
            }
        }
        public Dictionary<String, String> getRow()
        {
            return row;
        }


        public List<String> getHeaders()
        {
            return row.Keys.ToList<String>();
        }

        public void setElementValue(String key, String value)
        {
            if (row.Keys.Contains(key))
            {
                row[key] = value;
            }
        }


    }
}
