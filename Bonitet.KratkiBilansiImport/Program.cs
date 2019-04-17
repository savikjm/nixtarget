using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Bonitet.DAL;

namespace Bonitet.KratkiBilansiImport
{
    class Program
    {
        static void Main(string[] args)
        {
            //DALHelper.FixDB();
            //Delete();

            //UpdateReportRequests();

            newestReportValueImporter();
        }

        public static void UpdateReportRequests()
        {
            DALHelper.UpdateReportRequest();
        }

        //public static void Delete()
        //{

        //    var CompanyList = new List<DAL.Company>();
        //    var CompanyDetails = new DAL.Company();

        //    var db = new DAL.TargetFinancialDataContext();

        //    var CheckData = new List<CVTemp>();

        //    var res = Workbook.Worksheets(@"C:\Nix\Project\Bonitet.KratkiBilansiImport\bin\Debug\Dopolnuvanjenakratkibilansi.xlsx");
        //    var hasValues = 0;
        //    var newValues = false;
        //    var total = 0;
        //    foreach (var worksheet in res)
        //    {
        //        var rowCounter = 0;
        //        foreach (var row in worksheet.Rows)
        //        {
        //            if (rowCounter < 3)
        //            {
        //                rowCounter++;
        //                continue;
        //            }
        //            var cellCounter = 0;
        //            var company = new Company();
        //            var reportValues = new List<ReportValue>();
        //            foreach (var cell in row.Cells)
        //            {
        //                var EMBS = "";
        //                if (cellCounter == 0)
        //                {
        //                    EMBS = row.Cells[cellCounter].Value;

        //                    company = DALHelper.GetCompanyByEMBSTF(EMBS);

        //                    var report = db.Reports.Where(c => c.ReportType == 2 && c.CompanyID == company.ID).FirstOrDefault();
        //                    if (report == null)
        //                    {

        //                        var repv = db.ReportValues.Where(c => c.CompanyID == company.ID).ToList();

        //                        db.ReportValues.DeleteAllOnSubmit(repv);
        //                        db.SubmitChanges();

        //                        newValues = true;

        //                        cellCounter++;
        //                        total++;
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        cellCounter++;
        //                        total++;
        //                        continue;
        //                    }

        //                }

        //                Console.Write(total);

        //                cellCounter++;
        //            }

        //            rowCounter++;
        //        }


        //    }

        //    db.Dispose();
        //    db = null;

        //}

        public static void newestReportValueImporter()
        {
            var CompanyList = new Dictionary<int, List<ReportValuesBackup>>();
            var CompanyDetails = new DAL.Company();

            var db = new DAL.TargetFinancialDataContext();

            var res = Workbook.Worksheets(@"C:\Nix\Project_Dev\Bonitet.KratkiBilansiImport\bin\Debug\Kratki 2017.xlsx");

            var notFoundTotal = 0;
            var existing = 0;
            var newValues = 0;
            foreach (var worksheet in res)
            {
                var rowCounter = 0;
                foreach (var row in worksheet.Rows)
                {
                    if (rowCounter < 1)
                    {
                        rowCounter++;
                        continue;
                    }
                    var cellCounter = 0;
                    var company = new Company();
                    var reportValues = new List<ReportValuesBackup>();

                    var EMBS = "";
                    var vID = "";
                    var rValue = "";
                    var yearVal = "";

                    EMBS = row.Cells[0].Value;
                    vID = row.Cells[1].Value;
                    rValue = row.Cells[2].Value;
                    yearVal = row.Cells[3].Value;

                    company = DALHelper.GetCompanyByEMBSTF(EMBS);
                    if (company != null)
                    {
                        var reportValue = new ReportValuesBackup();

                        reportValue.CompanyID = company.ID;
                        reportValue.ValueID = Int32.Parse(vID);

                        var tmpVal = "";
                        if (rValue != null)
                        {
                            reportValue.Value = rValue;
                        }
                        else
                        {
                            reportValue.Value = "0";
                        }
                        var existingVal = DALHelper.GetReportValue(company.EMBS, reportValue);
                        if (existingVal.Count > 0)
                        {
                            existing++;
                            Console.WriteLine("existing values " + existing);
                            continue;
                        }
                        var curYearObj = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == Convert.ToInt32(yearVal)).FirstOrDefault();

                        if (curYearObj != null)
                        {
                            reportValue.YearID = curYearObj.ID;
                        }
                        else
                        {
                            var year_obj = new CompanyYear();

                            year_obj.Year = Convert.ToInt32(yearVal);
                            year_obj.CompanyID = company.ID;

                            db.CompanyYears.InsertOnSubmit(year_obj);
                            db.SubmitChanges();

                            curYearObj = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == Convert.ToInt32(yearVal)).FirstOrDefault();

                            reportValue.YearID = curYearObj.ID;
                        }

                        reportValues.Add(reportValue);

                        db.ReportValuesBackups.InsertAllOnSubmit(reportValues);

                        db.SubmitChanges();

                        if (CompanyList.Keys.Where(c=> c == company.ID).Count() > 0)
                        {
                            CompanyList[company.ID].AddRange(reportValues);
                        }
                        else
                        {

                            CompanyList.Add(company.ID, reportValues);
                        }

                    }
                    else {
                        notFoundTotal++;
                        Console.WriteLine("not found: " + notFoundTotal);
                    }
                    rowCounter++;
                }


            }

            db.Dispose();
            db = null;

        }

        private static Dictionary<int, int> Ids = new Dictionary<int, int>
        {
            {1, 301},
            {2, 302},
            {3, 306},
            {4, 307},
            {5, 308},
            {6, 309},
            {7, 310},
            {8, 311},
            {9, 312},
            {10, 313},
            {11, 314},
            {12, 315},
            {13, 316},
            {14, 317},
            {15, 318},
            {16, 303},
            {17, 304},
            {18, 305},
            {19, 301},
            {20, 302},
            {21, 306},
            {22, 307},
            {23, 308},
            {24, 309},
            {25, 310},
            {26, 311},
            {27, 312},
            {28, 313},
            {29, 314},
            {30, 315},
            {31, 316},
            {32, 317},
            {33, 318},
            {34, 303},
            {35, 304},
            {36, 305}
        };
    }
}
