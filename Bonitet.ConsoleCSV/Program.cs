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

namespace Bonitet.ConsoleCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            //DALHelper.FixDB();
            parralelFunc();

            //UpdateReportRequests();

            var checkProblematic = 1;
        }
        public class ErrorType{
            private Exception e;
            private int id;

            public ErrorType(int ID, Exception ex) {
                id = ID;
                e = ex;
            }
        }
        public static void parralelFunc() { 
            using (var dc = new DAL.TargetFinancialDataContext())
            {
                // Get all the ids of interest.
                // I assume you mark successfully updated rows in some way
                // in the update transaction.
                List<int> ids = new List<int>();
                ids.Add(1);
                ids.Add(2);
                ids.Add(3);
                ids.Add(4);
                ids.Add(5);
                ids.Add(6);
                ids.Add(7);
                ids.Add(8);

                var problematicIds = new List<ErrorType>();

                // Either allow the TaskParallel library to select what it considers
                // as the optimum degree of parallelism by omitting the 
                // ParallelOptions parameter, or specify what you want.
                //Parallel.ForEach(ids, new ParallelOptions {MaxDegreeOfParallelism = 8},
                //                    id => CalculateDetails(id));
                Parallel.ForEach(ids, i =>
                {
                    //this.Update(i);
                    // commented out because you'll probably want to Invoke it
                    // depending on what it does exactly.
                    CalculateDetails(i);

                });
            }
        }
        private static void CalculateDetails(int id, List<ErrorType> problematicIds)
        {
            try
            {
                // Handle deadlocks
                DeadlockRetryHelper.Execute(() => CalculateDetails(id));
            }
            catch (Exception e)
            {
                // Too many deadlock retries (or other exception). 
                // Record so we can diagnose problem or retry later
                problematicIds.Add(new ErrorType(id, e));
            }
        }
        private static void CalculateDetails(int id)
        {
            try
            {
                // Creating a new DeviceContext is not expensive.
                // No need to create outside of this method.
                using (var dc = new DAL.TargetFinancialDataContext())
                {
                    Init(dc);
                    // work done here

                    dc.SubmitChanges();
                }
            }
            catch (Exception e) {
                var a = e;
            }
        }

        public static void UpdateReportRequests()
        {
            DALHelper.UpdateReportRequest();
        }

        public static void Init(TargetFinancialDataContext db)
        {
            var CompanyList = new List<DAL.Company>();
            var CompanyDetails = new DAL.Company();

            //var db = new DAL.TargetFinancialDataContext();

            var CheckData = new List<CVTemp>();
            //vid_na_rabota_550.xlsx
            //listing
            var res = Workbook.Worksheets(@"C:\Nix\Project_Dev\Bonitet.ConsoleCSV\bin\Debug\vid_na_rabota_450.xlsx");
            foreach (var worksheet in res)
            {
                var AOP = false;
                var Tekovna = 0;
                var Oznaka = 0;

                var rowCounter = 0;
                for (rowCounter = 0; rowCounter < worksheet.Rows.Count(); rowCounter++)
                {
                    var row = worksheet.Rows[rowCounter];

                    //}
                    //foreach (var row in worksheet.Rows)
                    //{
                    var cellCounter = 0;
                    foreach (var cell in row.Cells)
                    {
                        if (cell != null)
                        {
                            if (cell.Text == "*******" || cell.Text == "ПРЕГЛЕД НА ПОДАТОЦИ ОД ГОДИШНА СМЕТКА ЗА 2016 ГОДИНА - ВИД НА РАБОТА 450")
                            {
                                CompanyList.Add(CompanyDetails);

                                if (CompanyDetails.EMBS != null)
                                {
                                    Console.WriteLine(CompanyList.Count());

                                    if (CheckData != null && CheckData.Count() > 0)
                                        break;
                                    DALHelper.InsertCompanyValues1Temp(CompanyDetails);
                                }

                                CompanyDetails = new DAL.Company();

                                CompanyDetails.CYTemps.Add(new DAL.CYTemp
                                {
                                    Company = CompanyDetails,
                                    Year = 2016
                                });

                                AOP = false;
                                Tekovna = 0;
                                Oznaka = 0;

                            }

                            if (cell.Text == "ЕМБС")
                            {
                                CompanyDetails.EMBS = worksheet.Rows[rowCounter + 1].Cells[cellCounter].Text.TrimStart('0').Replace(".", "");
                                //CompanyDetails.EMBS = row.Cells[cellCounter + 1].Text.TrimStart('0');

                                CheckData = DALHelper.GetCompanyValuesByEMBSTemp(CompanyDetails.EMBS);
                                if (CheckData != null && CheckData.Count() > 0)
                                    break;
                            }
                            if (cell.Text == "НАЗИВ")
                            {
                                CompanyDetails.Name = worksheet.Rows[rowCounter + 1].Cells[cellCounter].Text;
                                //CompanyDetails.Name = row.Cells[cellCounter + 1].Text;
                            }

                            if (cell.Text == "МЕСТО")
                            {
                                CompanyDetails.Mesto = worksheet.Rows[rowCounter + 1].Cells[cellCounter].Text;
                                //CompanyDetails.Mesto = row.Cells[cellCounter + 1].Text;
                            }
                            else
                            {
                                if (CheckData != null && CheckData.Count() > 0)
                                    break;
                            }

                            if (cell.Text == "Ознака за АОП")
                            {
                                Oznaka = cell.ColumnIndex;
                            }

                            if (cell.Text == "Тековна година")
                            {
                                Tekovna = cell.ColumnIndex;
                            }

                            if (AOP)
                            {
                                if (CompanyDetails.CYTemps.First().ID == 0)
                                {
                                    CompanyDetails = DALHelper.InsertCompanyWithYearsTemp(CompanyDetails);
                                    CompanyDetails.CVTemp = new List<CVTemp>();
                                }

                                if (row.Cells[Oznaka] != null)
                                {
                                    var curOznaka = row.Cells[Oznaka].Text;

                                    var newValID = db.Values.Where(c => c.Type == 1 && c.Name == curOznaka).Select(c => c.ID).FirstOrDefault();

                                    var year16 = CompanyDetails.CYTemps.Where(c => c.Year == 2016).Select(c => c.ID).FirstOrDefault();

                                    if (CompanyDetails.CVTemp.Where(c => c.ValueID == newValID && c.YearID == year16).FirstOrDefault() == null)
                                    {
                                        if (row.Cells[Tekovna] != null)
                                        {
                                            var curValue = row.Cells[Tekovna].Text;

                                            if (string.IsNullOrEmpty(curValue) == false)
                                            {
                                                curValue = curValue.Replace(".", "");

                                                var newCompanyValue = new DAL.CVTemp();

                                                newCompanyValue.CompanyID = CompanyDetails.ID;
                                                newCompanyValue.YearID = year16;


                                                double tmpVal = 0;
                                                if (double.TryParse(curValue, out tmpVal))
                                                {
                                                    newCompanyValue.Value = tmpVal;

                                                    newCompanyValue.ValueID = newValID;


                                                    CompanyDetails.CVTemp.Add(newCompanyValue);
                                                }

                                            }
                                        }
                                    }
                                }
                            }

                            if (Tekovna > 0 && Oznaka > 0)
                            {
                                AOP = true;
                            }
                        }
                        cellCounter++;
                    }
                }


            }

            db.Dispose();
            db = null;
        }

        public static void CompanyDetails()
        {

        }
    }
}
