using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Bonitet.Web.Classes
{
    public class HelperFunctions
    {
        public static void ShowAlert(Page page, Object sender, string message)
        {
            ClientScriptManager cs = page.ClientScript;
            cs.RegisterClientScriptBlock(
                sender.GetType(),
                " ",
                @"<script language=javascript>alert('" + message + "');</script>",
                true
            );
        }


        public static string ConvertDateTimeString(DateTime date)
        {
            string parsedDate = "";

            var day = date.ToString("dd", System.Globalization.CultureInfo.InvariantCulture);
            var month = date.ToString("MM", System.Globalization.CultureInfo.InvariantCulture);
            var year = date.ToString("yyyy", System.Globalization.CultureInfo.InvariantCulture);

            parsedDate += year + "-" + month + "-" + day;


            return parsedDate;
        }
        public static string ConvertToDateTimeFromString(string date)
        {
            string parsedDate = "";

            var splitData = date.Split(new Char[] { '-' });

            var d = Convert.ToInt32(splitData[0]);
            var day = "";
            if (d < 10)
                day = "0" + d;
            else
                day = d.ToString();
            var m = Convert.ToInt32(GetMonthNumber(splitData[1]));
            var month = "";
            if (m < 10)
                month = "0" + m;
            else
                month = m.ToString();
            var y = Convert.ToInt32(splitData[2]);

            return day + "-" + month + "-" + y;
        }

        public static int GetMonthNumber(string m)
        {
            var month = new List<Month>();

            month.Add(new Month { Number = 1, Name = "Jan" });
            month.Add(new Month { Number = 2, Name = "Feb" });
            month.Add(new Month { Number = 3, Name = "Mar" });
            month.Add(new Month { Number = 4, Name = "Apr" });
            month.Add(new Month { Number = 5, Name = "May" });
            month.Add(new Month { Number = 6, Name = "Jun" });
            month.Add(new Month { Number = 7, Name = "Jul" });
            month.Add(new Month { Number = 8, Name = "Aug" });
            month.Add(new Month { Number = 9, Name = "Sep" });
            month.Add(new Month { Number = 10, Name = "Oct" });
            month.Add(new Month { Number = 11, Name = "Nov" });
            month.Add(new Month { Number = 12, Name = "Dec" });

            return month.Where(c => c.Name == m).Select(c => c.Number).FirstOrDefault();
        }
    }

    public struct Month
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}