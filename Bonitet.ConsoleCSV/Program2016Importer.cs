﻿using Excel;
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
    class Program2016Importer
    {
        static void Main2(string[] args)
        {
            //DALHelper.FixDB();
            Init();

            //UpdateReportRequests();
        }

        public static void UpdateReportRequests()
        {
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
