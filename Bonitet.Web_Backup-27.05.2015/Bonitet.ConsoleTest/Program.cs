using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.IO;
using TuesPechkin;
using System.Drawing;
using System.Web;
using System.Net;
using System.Linq;
using System.Data.Linq;
using Bonitet.DAL;


namespace Bonitet.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TargetFinancialDataContext();
            var db1 = new Bonitet.DAL.BiznisMreza.DALDataContext();
            var db2 = new Bonitet.DAL.BiznisMrezaBeta.BetaDataContext();

            var subjekts = db2.Subjekts.ToList();

            var report_requests = db.ReportRequests.ToList();

            foreach (var item in report_requests)
            {
                var comp = db2.Subjekts.Where(c => c.PK_Subjekt == item.CompanyID).FirstOrDefault();
                if (comp != null)
                {
                    var new_comp = db1.Subjekts.Where(c => c.EMBS == comp.EMBS).FirstOrDefault();
                    if (new_comp != null)
                    {
                        item.CompanyID = new_comp.PK_Subjekt;
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
                else {
                    Console.WriteLine("error");
                }
            }

            db.SubmitChanges();

            db.Dispose();

            db = null;

            db1.Dispose();
            db2.Dispose();

            db1 = null;
            db2 = null;
        }
    }
}
