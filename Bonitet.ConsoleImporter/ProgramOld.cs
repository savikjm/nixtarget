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

namespace Bonitet.ConsoleImporter
{
    class ProgramOld
    {
        static void Main3(string[] args)
        {
            //DALHelper.FixDB();
            Init();

            //UpdateReportRequests();
        }

        public static void UpdateReportRequests() {
            DALHelper.UpdateReportRequest();
        }

        public static void Init()
        {
            var CompanyList = new List<DAL.Company>();
            var CompanyDetails = new DAL.Company();

            var db = new DAL.TargetFinancialDataContext();

            var CheckData = new List<CVTemp>();
            //vid_na_rabota_550.xlsx
            //listing
            var res = Workbook.Worksheets(@"C:\Nix\Project_Dev\Bonitet.ConsoleImporter\bin\Debug\vid_na_rabota_550.xlsx");
            foreach (var worksheet in res)
            {
                var AOP = false;
                var Tekovna = 0;
                var Prethodna = 0;
                var Oznaka = 0;

                foreach (var row in worksheet.Rows)
                {
                    var cellCounter = 0;
                    foreach (var cell in row.Cells)
                    {
                        if (cell != null)
                        {
                            if (cell.Text == "*******")
                            //if (cell.Text == "ПРЕГЛЕД НА ПОДАТОЦИ ОД ГОДИШНА СМЕТКА ЗА 2016 ГОДИНА - ВИД НА РАБОТА 550")
                            {
                                CompanyList.Add(CompanyDetails);

                                if (CompanyDetails.EMBS != null) {
                                    Console.WriteLine(CompanyList.Count());

                                    if (CheckData != null)
                                        break;
                                   DALHelper.InsertCompanyValues1Temp(CompanyDetails);
                                }

                                CompanyDetails = new DAL.Company();

                                CompanyDetails.CYTemps.Add(new DAL.CYTemp
                                {
                                    Company = CompanyDetails,
                                    Year = 2014
                                });
                                CompanyDetails.CYTemps.Add(new DAL.CYTemp
                                {
                                    Company = CompanyDetails,
                                    Year = 2013
                                });


                                AOP = false;
                                Tekovna = 0;
                                Prethodna = 0;
                                Oznaka = 0;

                            }

                            if (cell.Text == "Назив на правното лице:")
                            {
                                CompanyDetails.Name = row.Cells[cellCounter + 1].Text;
                            }

                            if (cell.Text == "Место:")
                            {
                                CompanyDetails.Mesto = row.Cells[cellCounter + 1].Text;
                            }

                            if (cell.Text == "Матичен Број:")
                            {
                                CompanyDetails.EMBS = row.Cells[cellCounter + 1].Text.TrimStart('0');

                                CheckData = DALHelper.GetCompanyValuesByEMBSTemp(CompanyDetails.EMBS);
                                if (CheckData != null)
                                    break;
                            }
                            else
                            {
                                if (CheckData != null)
                                    break;
                            }

                            if (cell.Text == "Ознака за АОП") {
                                Oznaka = cell.ColumnIndex;
                            }

                            if (cell.Text == "Тековна година")
                            {
                                Tekovna = cell.ColumnIndex;
                            }

                            if (cell.Text == "Претходна година")
                            {
                                Prethodna = cell.ColumnIndex;
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
                                    
                                    var year4 = CompanyDetails.CYTemps.Where(c => c.Year == 2014).Select(c => c.ID).FirstOrDefault();
                                    var year3 = CompanyDetails.CYTemps.Where(c => c.Year == 2013).Select(c => c.ID).FirstOrDefault();

                                    if (CompanyDetails.CVTemp.Where(c => c.ValueID == newValID && c.YearID == year4).FirstOrDefault() == null)
                                    {
                                        if (row.Cells[Tekovna] != null)
                                        {
                                            var curValue = row.Cells[Tekovna].Text;

                                            if (string.IsNullOrEmpty(curValue) == false)
                                            {
                                                curValue = curValue.Replace(".", "");

                                                var newCompanyValue = new DAL.CVTemp();

                                                newCompanyValue.CompanyID = CompanyDetails.ID;
                                                newCompanyValue.YearID = year4;


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

                                    if (CompanyDetails.CVTemp.Where(c => c.ValueID == newValID && c.YearID == year3).FirstOrDefault() == null)
                                    {
                                        if (row.Cells[Prethodna] != null)
                                        {
                                            var curValue = row.Cells[Prethodna].Text;

                                            if (string.IsNullOrEmpty(curValue) == false)
                                            {
                                                curValue = curValue.Replace(".", "");

                                                var newCompanyValue = new DAL.CVTemp();

                                                newCompanyValue.CompanyID = CompanyDetails.ID;
                                                newCompanyValue.YearID = year3;


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



                            if (Tekovna > 0 && Prethodna > 0 && Oznaka > 0)
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

        public static void CompanyDetails() {

        }
    }
}
