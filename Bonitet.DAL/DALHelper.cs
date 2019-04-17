using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bonitet.DAL;
using Bonitet.DAL.BiznisMreza;
using System.Data.Linq;

namespace Bonitet.DAL
{
    public class DALHelper
    {

        public static bool InsertPhoneToCompany(string EMBS, string phone)
        {
            var db = new BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();

            if (res != null)
            {
                var tmpPhone = res.Telefon;
                if (tmpPhone == null)
                {
                    res.Telefon = phone;

                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
            }
            else
            {
                db.Dispose();

                db = null;

                return false;
            }

            return true;
        }

        public static bool InsertVidSubjektNaUpisToCompany(string EMBS, string val)
        {
            var db = new BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();

            if (res != null)
            {
                var tmpVal = res.VidNaSubjektNaUpis;
                if (tmpVal == null)
                {
                    res.VidNaSubjektNaUpis = val;

                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
            }
            else
            {
                db.Dispose();

                db = null;

                return false;
            }

            return true;
        }

        public static bool InsertEDBToCompany(string EMBS, string val)
        {
            var db = new BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();

            if (res != null)
            {
                var tmpVal = res.EdinstvenDanocenBroj;
                if (tmpVal == null)
                {
                    res.EdinstvenDanocenBroj = val;

                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
            }
            else
            {
                db.Dispose();

                db = null;

                return false;
            }

            return true;
        }

        public static bool InsertGoleminaNaDelToCompany(string EMBS, string val)
        {
            var db = new BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();

            if (res != null)
            {
                var tmpVal = res.GoleminaNaDelovniotSubjekt;
                if (tmpVal == null)
                {
                    res.GoleminaNaDelovniotSubjekt = val;

                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
            }
            else
            {
                db.Dispose();

                db = null;

                return false;
            }

            return true;
        }

        public static bool InsertZiroSmetkaToCompany(string EMBS, string val)
        {
            var db = new BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();

            if (res != null)
            {
                var tmpVal = res.ZiroSmetka;
                if (tmpVal == null)
                {
                    res.ZiroSmetka = val;

                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
            }
            else
            {
                db.Dispose();

                db = null;

                return false;
            }

            return true;
        }

        public static bool CheckIfCRMHasData(string EMBS)
        {

            var db = new TargetFinancialDataContext();

            var comp = GetCompanyByEMBSTF(EMBS);

            if (comp != null)
            {
                var res = GetXmlResuls(comp.ID);

                if (res.Contains("Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година."))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string GetXmlResuls(int CompanyID)
        {
            var db = new TargetFinancialDataContext();

            DataLoadOptions opts = new DataLoadOptions();
            opts.LoadWith<XmlResponse>(c => c.CRMResponseID);

            db.LoadOptions = opts;

            var res = db.CRMResponses.Where(c => c.CompanyID == CompanyID && c.XmlResponse == null).FirstOrDefault();

            var text = "";
            if (res != null)
            {
                foreach (var item in res.XmlResponses.OrderBy(c => c.Part))
                {
                    text += item.Text;
                }
            }
            return text;
        }

        public static void InsertCrmResponse(string EMBS, int Type, string response)
        {
            var db = new TargetFinancialDataContext();

            var company = GetCompanyByEMBS(EMBS);
            if (company != null)
            {
                var CurYear = db.GlobalConfigs.Select(c => c.GlobalYear).First();

                var newResponse = new CRMResponse
                {
                    CompanyID = company.ID,
                    Year = CurYear,
                    Type = Type,
                    ResponseData = DateTime.Now
                };
                db.CRMResponses.InsertOnSubmit(newResponse);


                var chunkSize = 40000;
                var i = 0;
                while (response.Length > 0)
                {
                    if (response.Length < chunkSize)
                    {
                        chunkSize = response.Length;
                    }
                    var tmpText = response.Substring(0, chunkSize);
                    response = response.Remove(0, chunkSize);
                    var xmlResponse = new XmlResponse
                    {
                        CRMResponse = newResponse,
                        Text = tmpText,
                        Part = i
                    };
                    db.XmlResponses.InsertOnSubmit(xmlResponse);
                    i++;

                }

                db.SubmitChanges();


                db.Dispose();

                db = null;
            }

        }

        public static List<CompanyValuesBak> GetCompanyValuesByEMBSTemp(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault().GlobalYear;

            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {

                db.Dispose();

                db = null;
                return null;
            }

            if (company.CompanyYearsBaks.Count() > 0)
            {
                var res = db.CompanyValuesBaks.Where(c => c.CompanyID == company.ID && c.YearID == company.CompanyYearsBaks[0].ID).ToList();
                db.Dispose();

                db = null;
                if (res.Count() > 0)
                {
                    return res;
                }
                return null;
            }
            else
            {
                db.Dispose();

                db = null;
                return null;
            }
        }


        public static List<CompanyValue> GetCompanyValuesByEMBSAll(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault().GlobalYear;

            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {

                db.Dispose();

                db = null;

                return null;
            }
            var valList = new List<CompanyValue>();

            var compYears = db.CompanyYears.Where(c => c.CompanyID == company.ID).ToList();
            foreach (var item in compYears)
            {
                var tmpY = item;


                var values1 = db.CompanyValues.Where(c => c.CompanyID == company.ID && c.YearID == tmpY.ID).ToList();

                valList.AddRange(values1);

            }

            db.Dispose();

            db = null;

            return valList;
        }

        public static List<ReportValue> GetReportValuesByEMBS(string EMBS)
        {

            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault().GlobalYear;

            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {

                db.Dispose();

                db = null;

                return null;
            }

            var cyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == curYear).FirstOrDefault();
            var clastyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == (curYear - 1)).FirstOrDefault();
            if (cyear != null && clastyear != null)
            {
                var valList = new List<ReportValue>();

                var values1 = db.ReportValues.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID).ToList();
                var values2 = db.ReportValues.Where(c => c.CompanyID == company.ID && c.YearID == clastyear.ID).ToList();

                valList.AddRange(values1);
                valList.AddRange(values2);

                db.Dispose();

                db = null;

                return valList;
            }

            db.Dispose();

            db = null;

            return null;
        }

        public static List<ReportValuesBackup> GetReportValue(string EMBS, ReportValuesBackup reportValue)
        {

            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault().GlobalYear;

            curYear = 2017;

            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {

                db.Dispose();

                db = null;

                return null;
            }

            var cyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == curYear).FirstOrDefault();
            if (cyear != null)
            {
                var valList = new List<ReportValuesBackup>();

                var values1 = db.ReportValuesBackups.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID && c.ValueID == reportValue.ValueID).ToList();

                return values1;
            }

            db.Dispose();

            db = null;

            return null;
        }

        public static List<Company> GetAllCompanies() {
            var db = new TargetFinancialDataContext();

            return db.Companies.ToList();
        }


        public static List<CompanyValue> GetCompanyValuesByEMBS(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault().GlobalYear;

            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {

                db.Dispose();

                db = null;

                return null;
            }

            var cyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == curYear).FirstOrDefault();
            var clastyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == (curYear - 1)).FirstOrDefault();
            if (cyear != null && clastyear != null)
            {
                var valList = new List<CompanyValue>();

                var values1 = db.CompanyValues.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID).ToList();
                var values2 = db.CompanyValues.Where(c => c.CompanyID == company.ID && c.YearID == clastyear.ID).ToList();

                valList.AddRange(values1);
                valList.AddRange(values2);

                db.Dispose();

                db = null;

                return valList;
            }

            db.Dispose();

            db = null;

            return null;
        }



        public static bool InsertReportValuesImport(Dictionary<int, string> data, String EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();
            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {
                var db1 = new DALDataContext();
                var subjekt = db1.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();
                if (subjekt == null)
                {
                    return false;
                }
                else
                {
                    Company newCompany = new Company();

                    newCompany.Name = subjekt.CelosenNazivNaSubjektot;
                    newCompany.Mesto = subjekt.Sediste;
                    newCompany.EMBS = subjekt.EMBS;


                    db.Companies.InsertOnSubmit(newCompany);

                    db.SubmitChanges();

                    company = newCompany;
                }
            }


            var cyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == Year).FirstOrDefault();

            if (cyear == null)
            {
                //create year
                cyear = new CompanyYear
                {
                    CompanyID = company.ID,
                    Year = Year
                };
                db.CompanyYears.InsertOnSubmit(cyear);

                db.SubmitChanges();
            }

            if (company != null)
            {
                var check = db.ReportValues.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID).ToList();
                if (check.Count() == 0)
                {
                    var reportValues = new List<ReportValue>();
                    foreach (var item in data)
                    {
                        //var valID = db.Values.Where(c => c.Name == item.Key.ToString()).Select(c => c.ID).FirstOrDefault();
                        //if (valID > 0)
                        //{
                        reportValues.Add(new ReportValue
                        {
                            CompanyID = company.ID,
                            Value = item.Value.ToString(),
                            ValueID = item.Key,
                            YearID = cyear.ID
                        });
                        //}
                    }

                    db.ReportValues.InsertAllOnSubmit(reportValues);

                    db.SubmitChanges();
                }
                else
                {
                    var reportValues = new List<ReportValue>();
                    foreach (var item in data)
                    {
                        //var valID = db.Values.Where(c => c.Name == item.Key.ToString()).Select(c => c.ID).FirstOrDefault();
                        //if (valID > 0)
                        //{
                        var tmpValues = check.Where(c => c.ValueID == item.Key).ToList();
                        db.ReportValues.DeleteAllOnSubmit(tmpValues);

                        reportValues.Add(new ReportValue
                        {
                            CompanyID = company.ID,
                            Value = item.Value.ToString(),
                            ValueID = item.Key,
                            YearID = cyear.ID
                        });
                        //}
                    }
                    db.ReportValues.InsertAllOnSubmit(reportValues);

                    db.SubmitChanges();
                }
            }

            db.Dispose();

            db = null;

            return true;
        }
        public static bool InsertCompanyValuesImport(Dictionary<int, double> data, String EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();
            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {
                var db1 = new DALDataContext();
                var subjekt = db1.Subjekts.Where(c => c.EMBS == EMBS).FirstOrDefault();
                if (subjekt == null)
                {
                    return false;
                }
                else
                {
                    Company newCompany = new Company();

                    newCompany.Name = subjekt.CelosenNazivNaSubjektot;
                    newCompany.Mesto = subjekt.Sediste;
                    newCompany.EMBS = subjekt.EMBS;


                    db.Companies.InsertOnSubmit(newCompany);

                    db.SubmitChanges();

                    company = newCompany;
                }
            }


            var cyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == Year).FirstOrDefault();

            if (cyear == null)
            {
                //create year
                cyear = new CompanyYear
                {
                    CompanyID = company.ID,
                    Year = Year
                };
                db.CompanyYears.InsertOnSubmit(cyear);

                db.SubmitChanges();
            }

            if (company != null)
            {
                var check = db.CompanyValues.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID).ToList();
                if (check.Count() == 0)
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in data)
                    {
                        var valID = db.Values.Where(c => c.Name == item.Key.ToString()).Select(c => c.ID).FirstOrDefault();
                        if (valID > 0)
                        {
                            CompanyValues.Add(new CompanyValue
                            {
                                CompanyID = company.ID,
                                Value = item.Value,
                                ValueID = valID,
                                YearID = cyear.ID
                            });
                        }
                    }

                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
                else
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in data)
                    {
                        var valID = db.Values.Where(c => c.Name == item.Key.ToString()).Select(c => c.ID).FirstOrDefault();
                        if (valID > 0)
                        {
                            var tmpValues = check.Where(c => c.ValueID == valID).ToList();
                            db.CompanyValues.DeleteAllOnSubmit(tmpValues);

                            CompanyValues.Add(new CompanyValue
                            {
                                CompanyID = company.ID,
                                Value = item.Value,
                                ValueID = valID,
                                YearID = cyear.ID
                            });
                        }
                    }
                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
            }

            db.Dispose();

            db = null;

            return true;
        }

        public static void InsertCompanyValues(Company newCompany, Dictionary<int, double> data, string EMBS, int Year, string xmlResponse)
        {
            var db = new TargetFinancialDataContext();

            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {
                db.Companies.InsertOnSubmit(newCompany);

                db.SubmitChanges();

                company = newCompany;
            }


            var cyear = db.CompanyYears.Where(c => c.CompanyID == company.ID && c.Year == Year).FirstOrDefault();

            if (cyear == null)
            {
                //create year
                cyear = new CompanyYear
                {
                    CompanyID = company.ID,
                    Year = Year
                };
                db.CompanyYears.InsertOnSubmit(cyear);

                db.SubmitChanges();
            }

            if (cyear.Year == db.GlobalConfigs.Select(c => c.GlobalYear).FirstOrDefault())
                InsertCrmResponse(EMBS, 1, xmlResponse);

            if (company != null)
            {
                var check = db.CompanyValues.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID).ToList();
                if (check.Count() == 0)
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in data)
                    {
                        var valID = db.Values.Where(c => c.Name == item.Key.ToString()).Select(c => c.ID).FirstOrDefault();
                        if (valID > 0)
                        {
                            CompanyValues.Add(new CompanyValue
                            {
                                CompanyID = company.ID,
                                Value = item.Value,
                                ValueID = valID,
                                YearID = cyear.ID
                            });
                        }
                    }

                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
                else
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in data)
                    {
                        var valID = db.Values.Where(c => c.Name == item.Key.ToString()).Select(c => c.ID).FirstOrDefault();
                        if (valID > 0)
                        {
                            var tmpValues = check.Where(c => c.ValueID == valID).ToList();
                            db.CompanyValues.DeleteAllOnSubmit(tmpValues);

                            CompanyValues.Add(new CompanyValue
                            {
                                CompanyID = company.ID,
                                Value = item.Value,
                                ValueID = valID,
                                YearID = cyear.ID
                            });
                        }
                    }
                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
            }

            db.Dispose();

            db = null;
        }

        public static void FixDB()
        {
            var db = new TargetFinancialDataContext();

            DataLoadOptions opts = new DataLoadOptions();
            opts.LoadWith<Company>(c => c.CompanyYears);

            db.LoadOptions = opts;

            var companies = db.Companies.Where(c => !c.EMBS.StartsWith("0")).ToList();

            foreach (var company in companies)
            {
                var tmpEmbs = "0" + company.EMBS;

                var curCompany = db.Companies.Where(c => c.EMBS == tmpEmbs).FirstOrDefault();

                if (curCompany != null)
                {
                    var compValues = db.CompanyValues.Where(c => c.CompanyID == curCompany.ID).ToList();
                    var compYears = db.CompanyYears.Where(c => c.CompanyID == curCompany.ID).ToList();

                    foreach (var year in compYears)
                    {
                        var tmpY = company.CompanyYears.Where(c => c.Year == 2013).FirstOrDefault();

                        if (tmpY == null)
                        {
                            if (year.Year == 2013)
                            {
                                year.CompanyID = company.ID;
                            }

                        }

                        tmpY = company.CompanyYears.Where(c => c.Year == 2014).FirstOrDefault();

                        if (tmpY == null)
                        {
                            if (year.Year == 2014)
                            {
                                year.CompanyID = company.ID;
                            }


                        }
                    }

                    if (compValues.Count() > 0)
                    {
                        foreach (var val in compValues)
                        {
                            var curYear = curCompany.CompanyYears.Where(c => c.ID == val.YearID).FirstOrDefault();

                            var newYear = company.CompanyYears.Where(c => c.Year == curYear.Year).FirstOrDefault();



                            // db.CompanyValues.InsertOnSubmit(tmpVal);
                        }

                        db.CompanyValues.DeleteAllOnSubmit(compValues);

                        db.CompanyYears.DeleteAllOnSubmit(curCompany.CompanyYears);

                        company.EMBS = tmpEmbs;

                        db.SubmitChanges();
                    }
                }
            }

            db.Dispose();

            db = null;
        }

        public static Company InsertCompanyWithYears(Company newCompany)
        {
            var db = new TargetFinancialDataContext();

            DataLoadOptions opts = new DataLoadOptions();
            opts.LoadWith<Company>(c => c.CompanyYears);

            db.LoadOptions = opts;

            var company = db.Companies.Where(c => c.EMBS == newCompany.EMBS).FirstOrDefault();
            if (company == null)
            {
                db.Companies.InsertOnSubmit(newCompany);

                db.SubmitChanges();

                company = newCompany;
            }

            var curYear = newCompany.CompanyYears.Where(c => c.Year == db.GlobalConfigs.Select(d => d.GlobalYear).First()).FirstOrDefault();
            var lastYear = newCompany.CompanyYears.Where(c => c.Year == (db.GlobalConfigs.Select(d => d.GlobalYear).First() - 1)).FirstOrDefault();

            if (curYear != null)
            {
                if (company.CompanyYears.Where(c => c.Year == curYear.Year).FirstOrDefault() == null)
                {
                    company.CompanyYears.Add(curYear);
                }
            }
            if (lastYear != null)
            {
                if (company.CompanyYears.Where(c => c.Year == lastYear.Year).FirstOrDefault() == null)
                {
                    company.CompanyYears.Add(lastYear);
                }
            }

            db.SubmitChanges();

            db.Dispose();

            db = null;

            return company;
        }
        public static Company InsertCompanyWithYearsTemp(Company newCompany)
        {
            var db = new TargetFinancialDataContext();

            DataLoadOptions opts = new DataLoadOptions();
            opts.LoadWith<Company>(c => c.CompanyYears);

            db.LoadOptions = opts;

            var company = db.Companies.Where(c => c.EMBS == newCompany.EMBS).FirstOrDefault();
            if (company == null)
            {
                db.Companies.InsertOnSubmit(newCompany);

                db.SubmitChanges();

                company = newCompany;
            }
            var globalYear = db.GlobalConfigs.Select(d => d.GlobalYear).First();
            globalYear = 2017;
            var curYear = newCompany.CompanyYears.Where(c => c.Year == globalYear).FirstOrDefault();
            var lastYear = newCompany.CompanyYears.Where(c => c.Year == globalYear - 1).FirstOrDefault();

            if (curYear != null)
            {
                if (company.CompanyYears.Where(c => c.Year == curYear.Year).FirstOrDefault() == null)
                {
                    company.CompanyYears.Add(curYear);
                }
            }
            if (lastYear != null)
            {
                if (company.CompanyYears.Where(c => c.Year == lastYear.Year).FirstOrDefault() == null)
                {
                    company.CompanyYears.Add(lastYear);
                }
            }

            db.SubmitChanges();

            db.Dispose();

            db = null;

            return company;
        }

        public static void InsertCompanyValues1(Company newCompany)
        {
            var db = new TargetFinancialDataContext();


            if (newCompany != null)
            {
                var check = db.CompanyValues.Where(c => c.CompanyID == newCompany.ID).ToList();
                if (check.Count() == 0)
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in newCompany.CompanyValues)
                    {
                        CompanyValues.Add(new CompanyValue
                        {
                            CompanyID = newCompany.ID,
                            Value = item.Value,
                            ValueID = item.ValueID,
                            YearID = item.YearID
                        });
                    }

                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
                else
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in newCompany.CompanyValues)
                    {
                        var tmpValues = check.Where(c => c.ValueID == item.ValueID).ToList();
                        db.CompanyValues.DeleteAllOnSubmit(tmpValues);

                        CompanyValues.Add(new CompanyValue
                        {
                            CompanyID = newCompany.ID,
                            Value = item.Value,
                            ValueID = item.ValueID,
                            YearID = item.YearID
                        });
                    }

                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
            }

            db.Dispose();

            db = null;
        }

        public static List<CompanyValuesBak> GetCV2016(string EMBS)
        {

            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault().GlobalYear;
            curYear = 2016;
            var company = db.Companies.Where(c => c.EMBS == EMBS).FirstOrDefault();
            if (company == null)
            {
                db.Dispose();

                db = null;

                return null;
            }

            var cyear = db.CompanyYearsBaks.Where(c => c.CompanyID == company.ID && c.Year == curYear).FirstOrDefault();
            //var clastyear = db.CYTemps.Where(c => c.CompanyID == company.ID && c.Year == (curYear - 1)).FirstOrDefault();
            if (cyear != null)
            {
                var valList = new List<CompanyValuesBak>();

                valList = db.CompanyValuesBaks.Where(c => c.CompanyID == company.ID && c.YearID == cyear.ID).ToList();
                //var values2 = db.CompanyValues.Where(c => c.CompanyID == company.ID && c.YearID == clastyear.ID).ToList();

                //valList.AddRange(values1);
                //valList.AddRange(values2);

                db.Dispose();

                db = null;

                return valList;
            }

            db.Dispose();

            db = null;

            return null;
        }
        
        public static void InsertCompanyValues1Temp(Company newCompany)
        {
            var db = new TargetFinancialDataContext();


            if (newCompany != null)
            {
                var check = db.CompanyValues.Where(c => c.CompanyID == newCompany.ID).ToList();
                if (check.Count() == 0)
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in newCompany.CompanyValues)
                    {
                        CompanyValues.Add(new CompanyValue
                        {
                            CompanyID = newCompany.ID,
                            Value = item.Value,
                            ValueID = item.ValueID,
                            YearID = item.YearID
                        });
                    }

                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
                else
                {
                    var CompanyValues = new List<CompanyValue>();
                    foreach (var item in newCompany.CompanyValues)
                    {
                        var tmpValues = check.Where(c => c.ValueID == item.ValueID).ToList();
                        //db.CompanyValuesBaks.DeleteAllOnSubmit(tmpValues);

                        CompanyValues.Add(new CompanyValue
                        {
                            CompanyID = newCompany.ID,
                            Value = item.Value,
                            ValueID = item.ValueID,
                            YearID = item.YearID
                        });
                    }

                    db.CompanyValues.InsertAllOnSubmit(CompanyValues);

                    db.SubmitChanges();
                }
            }

            db.Dispose();

            db = null;
        }


        public static List<ReportRequest> GetAllReportsByFilter(int filter)
        {
            var db = new TargetFinancialDataContext();

            var tmpRes = db.ReportRequests.GroupBy(c => new { c.CompanyID, c.UserID, c.ReportType, c.Year }).Select(c => c.First()).ToList();
            var newList = new List<ReportRequest>();
            if (filter > 1)
            {
                newList = GetRequestsByFilter(tmpRes, filter).ToList();
            }
            else
                newList = tmpRes.ToList();

            db.Dispose();

            db = null;

            return newList;
        }

        public static int GetTotalReportRequestLogs(int filter)
        {

            var db = new TargetFinancialDataContext();

            var res = db.GetReportRequests();

            var counter = 0;
            foreach (var item in res)
            {
                if (filter != item.Filter.Value)
                {
                    if (filter != 1)
                        continue;
                }
                counter++;
            }

            return counter;
        }

        public static int GetTotalReportRequests(int filter)
        {

            var db = new TargetFinancialDataContext();

            var res = db.GetReportRequests();

            var counter = 0;
            foreach (var item in res)
            {
                if (filter != item.Filter.Value)
                {
                    if (filter != 1)
                        continue;
                }
                if (item.Deleted == true)
                    continue;
                counter++;
            }

            return counter;
        }

        public static int GetTotalReportRequests1(int filter)
        {
            var db = new TargetFinancialDataContext();

            var count = 0;
            var tmpRes = db.ReportRequests.GroupBy(c => new { c.CompanyID, c.UserID, c.ReportType, c.Year }).Select(c => c.First()).ToList();
            if (filter > 1)
            {
                count = GetRequestsByFilter(tmpRes, filter).Count();
            }
            else
                count = tmpRes.Count();

            db.Dispose();

            db = null;

            return count;
        }

        public static int GetTotalClientReports(string EMBS, DateTime StartDate, DateTime EndDate, string TypeSelector)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<UserReport>(c => c.User);
            LoadOptions.LoadWith<UserReport>(c => c.PrepayPack);
            LoadOptions.LoadWith<UserReport>(c => c.Report);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            var returnRes = new List<Extensions.ClientReports>();

            var tmpRes = new List<UserReport>();


            if (TypeSelector == "-1")
            {
                if (EMBS != null)
                    tmpRes = db.UserReports.Where(c => c.User.EMBS == EMBS && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
                else
                    tmpRes = db.UserReports.Where(c => c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
            }
            else if (TypeSelector == "1")
            {
                if (EMBS != null)
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == true && c.User.EMBS == EMBS && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
                else
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == true && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
            }
            else
            {
                if (EMBS != null)
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == false && c.User.EMBS == EMBS && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
                else
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == false && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
            }



            foreach (var item in tmpRes)
            {
                var CurItem = new Extensions.ClientReports();

                var tmpItem = returnRes.Where(c => c.EMBS == item.User.EMBS).FirstOrDefault();
                if (tmpItem == null)
                {
                    CurItem.EMBS = item.User.EMBS;
                }
                else
                    CurItem = tmpItem;

                if (tmpItem == null)
                    returnRes.Add(CurItem);
            }


            return returnRes.Count();

        }


        public static List<Extensions.ClientReports> GetClientsReports(int page, int skip, int pageSize, string EMBS, DateTime StartDate, DateTime EndDate, string TypeSelector)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<UserReport>(c => c.User);
            LoadOptions.LoadWith<UserReport>(c => c.PrepayPack);
            LoadOptions.LoadWith<UserReport>(c => c.Report);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            var returnRes = new List<Extensions.ClientReports>();

            var tmpRes = new List<UserReport>();


            if (TypeSelector == "-1")
            {
                if (EMBS != null)
                    tmpRes = db.UserReports.Where(c => c.User.EMBS == EMBS && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
                else
                    tmpRes = db.UserReports.Where(c => c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
            }
            else if (TypeSelector == "1")
            {
                if (EMBS != null)
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == true && c.User.EMBS == EMBS && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
                else
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == true && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
            }
            else
            {
                if (EMBS != null)
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == false && c.User.EMBS == EMBS && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
                else
                    tmpRes = db.UserReports.Where(c => c.PrepayPack.IsPostPaid == false && c.DateCreated >= StartDate && c.DateCreated <= EndDate).ToList();
            }



            foreach (var item in tmpRes)
            {
                var CurItem = new Extensions.ClientReports();

                var tmpItem = returnRes.Where(c => c.EMBS == item.User.EMBS).FirstOrDefault();
                if (tmpItem == null)
                {
                    CurItem.ID = item.ID;
                    CurItem.EMBS = item.User.EMBS;
                    CurItem.Username = item.User.Username;
                }
                else
                    CurItem = tmpItem;

                if (item.Report.ReportType == (int)UserReportType.Blokada)
                {
                    CurItem.BlokadiTotal++;
                    if (CurItem.BlokadiIsPostpaid == false)
                        CurItem.BlokadiIsPostpaid = item.PrepayPack.IsPostPaid;
                }
                if (item.Report.ReportType == (int)UserReportType.BonitetenIzvestaj)
                {
                    CurItem.BonitetiTotal++;
                    if (CurItem.BonitetiIsPostpaid == false)
                        CurItem.BonitetiIsPostpaid = item.PrepayPack.IsPostPaid;
                }
                if (item.Report.ReportType == (int)UserReportType.FinansiskiPregled)
                {
                    CurItem.FinansiskiTotal++;
                    if (CurItem.FinansiskiIsPostpaid == false)
                        CurItem.FinansiskiIsPostpaid = item.PrepayPack.IsPostPaid;
                }

                if (tmpItem == null)
                    returnRes.Add(CurItem);
            }


            return returnRes.Skip(skip).Take(pageSize).OrderByDescending(c => c.BonitetiTotal).ThenBy(c => c.FinansiskiTotal).ThenBy(c => c.BlokadiTotal).ToList();

        }

        public static List<ReportRequest> GetRequestsByFilter(List<ReportRequest> requests, int filter)
        {
            var newList = new List<ReportRequest>();
            foreach (var item in requests)
            {
                var check = CheckIfCompanyHasDataForReport(item.EMBS, item.ReportType);

                if (filter == 2 && check == false)
                {
                    newList.Add(item);
                }

                if (filter == 3 && check && item.ReportType == (int)UserReportType.FinansiskiPregled)
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, item.EMBS, item.ReportType, item.UserID);
                    if (user_report_check != 1)
                        newList.Add(item);
                }
                if (filter == 4 && check && item.ReportType == (int)UserReportType.FinansiskiPregled)
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, item.EMBS, item.ReportType, item.UserID);
                    if (user_report_check == 1)
                        newList.Add(item);
                }
                if (filter == 5 && check && item.ReportType == (int)UserReportType.FinansiskiPregled && item.NoData == true)
                {
                    newList.Add(item);
                }
            }
            return newList;
        }
        public static List<ReportRequest> FixRequestItems(List<ReportRequest> tmpList, int skip, int pageSize, int filter)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<User>(c => c.Username);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            foreach (var item in tmpList)
            {
                var comp = GetCompanyByID(item.CompanyID);

                item.CompanyName = comp[0].Naziv;

                if (item.ReportType == 1)
                    item.ReportTypeString = "Бонитетен извештај";
                else if (item.ReportType == 2)
                    item.ReportTypeString = "Финансиски преглед";


                var check = CheckIfCompanyHasDataForReport(item.EMBS, item.ReportType);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;
                }
                else
                {
                    if (item.ReportType == 2)
                    {
                        var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, item.EMBS, item.ReportType, item.UserID);
                        if (user_report_check == 1)
                        {
                            item.StatusText = "Downloaded";
                            item.SendMail = false;
                        }
                        else
                        {
                            item.StatusText = "Report Available";
                            item.SendMail = true;
                        }

                        if (item.ReportType == 1 && item.Paid.HasValue && item.Paid == true)
                        {
                            item.StatusText = "Downloaded";
                            item.SendMail = false;
                        }
                    }
                }
            }

            return tmpList.Skip(skip).Take(pageSize).ToList();
        }

        public enum ReportRequestActions
        {
            Delete = 1,
            NoData = 2,
            SendMail = 3,
            Paid = 4
        }

        public static void UpdateReportRequestFunc(int ID, ReportRequestActions Type)
        {
            var db = new TargetFinancialDataContext();

            var cur = db.ReportRequests.Where(c => c.ID == ID).FirstOrDefault();


            if (cur != null)
            {
                var items = db.ReportRequests.Where(c =>
                    c.EMBS == cur.EMBS && c.UserID == cur.UserID && c.ReportType == cur.ReportType && c.Year == cur.Year).ToList();

                foreach (var item in items)
                {
                    switch (Type)
                    {
                        case ReportRequestActions.Delete:
                            item.Deleted = true;
                            break;
                        case ReportRequestActions.NoData:
                            item.NoData = true;
                            break;
                        case ReportRequestActions.SendMail:
                            item.MailSent = true;
                            break;
                        case ReportRequestActions.Paid:
                            item.Paid = true;
                            break;
                    }
                }
            }

            db.SubmitChanges();

            db.Dispose();

            db = null;
        }

        public static List<ReportRequest> GetRequestLogsByPage(int skip, int pageSize, int filter)
        {
            var db = new TargetFinancialDataContext();

            var res = db.GetReportRequests();

            var list = new List<ReportRequest>();
            foreach (var item in res)
            {
                if (filter != item.Filter.Value)
                {
                    if (filter != 1)
                        continue;
                }

                if (item.CompanyName == null)
                {
                    var comp = GetCompanyByID(item.CompanyID);
                    if (comp.Count() > 0)
                    {
                        item.CompanyName = comp[0].Naziv;
                    }
                }

                var newItem = new ReportRequest
                {
                    ID = item.ID,
                    CompanyID = item.CompanyID,
                    CompanyName = item.CompanyName,
                    CreatedOn = item.CreatedOn,
                    EMBS = item.EMBS,
                    NoData = item.NoData,
                    ReportType = item.ReportType,
                    ReportTypeString = item.ReportTypeString,
                    StatusText = item.StatusText,
                    Username = item.Username,
                    Year = item.Year,
                    UserID = item.UserID,
                    Deleted = item.Deleted,
                    MailSent = item.MailSent
                };

                if (item.Paid.HasValue)
                {
                    if (item.Paid.Value == true)
                        newItem.Paid = true;
                    else
                        newItem.Paid = false;
                }
                else
                    newItem.Paid = false;

                if (item.SendMail.HasValue)
                {
                    if (item.SendMail == 1)
                        newItem.SendMail = true;
                    else
                        newItem.SendMail = false;
                }
                else
                    newItem.SendMail = false;

                list.Add(newItem);
            }

            return list.Skip(skip).Take(pageSize).ToList();

        }

        public static List<ReportRequest> GetRequestsByPage(int skip, int pageSize, int filter)
        {
            var db = new TargetFinancialDataContext();

            var res = db.GetReportRequests();

            var list = new List<ReportRequest>();
            foreach (var item in res)
            {
                if (filter != item.Filter.Value)
                {
                    if (filter != 1)
                        continue;
                }

                if (item.Deleted.HasValue)
                {
                    if (item.Deleted.Value == true)
                        continue;
                }

                if (item.CompanyName == null)
                {
                    var comp = GetCompanyByID(item.CompanyID);
                    if (comp.Count() > 0)
                    {
                        item.CompanyName = comp[0].Naziv;
                    }
                }

                var newItem = new ReportRequest
                {
                    ID = item.ID,
                    CompanyID = item.CompanyID,
                    CompanyName = item.CompanyName,
                    CreatedOn = item.CreatedOn,
                    EMBS = item.EMBS,
                    NoData = item.NoData,
                    ReportType = item.ReportType,
                    ReportTypeString = item.ReportTypeString,
                    StatusText = item.StatusText,
                    Username = item.Username,
                    Year = item.Year,
                    UserID = item.UserID,
                    Deleted = item.Deleted,
                    MailSent = item.MailSent
                };

                if (item.Paid.HasValue)
                {
                    if (item.Paid.Value == true)
                        newItem.Paid = true;
                    else
                        newItem.Paid = false;
                }
                else
                    newItem.Paid = false;

                if (item.SendMail.HasValue)
                {
                    if (item.SendMail.Value == 1)
                        newItem.SendMail = true;
                    else
                        newItem.SendMail = false;
                }
                else
                    newItem.SendMail = false;

                list.Add(newItem);
            }

            return list.Skip(skip).Take(pageSize).ToList();
        }

        public static List<ReportRequest> GetRequestsByPage1(int skip, int pageSize, int filter)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<User>(c => c.Username);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            var tmpRes = db.ReportRequests.Where(c => c.ReportType != 3).GroupBy(c => new { c.CompanyID, c.UserID, c.ReportType, c.Year }).Select(c => c.First()).OrderByDescending(c => c.CreatedOn).ToList();

            var res = new List<ReportRequest>();

            foreach (var item in tmpRes)
            {
                var comp = GetCompanyByID(item.CompanyID);

                item.CompanyName = comp[0].Naziv;

                if (item.ReportType == 1)
                    item.ReportTypeString = "Бонитетен извештај";
                else if (item.ReportType == 2)
                    item.ReportTypeString = "Финансиски преглед";

                if (item.ReportType == 2 && item.NoData == true && filter == 5)
                {
                    res.Add(item);
                    continue;
                }

                var check = CheckIfCompanyHasDataForReport(item.EMBS, item.ReportType);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;

                    if (filter == 2)
                        res.Add(item);
                }
                else
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, item.EMBS, item.ReportType, item.UserID);
                    if (user_report_check == 1)
                    {
                        item.StatusText = "Downloaded";
                        item.SendMail = false;
                        if (filter == 4)
                            res.Add(item);
                    }
                    else
                    {
                        item.StatusText = "Report Available";
                        item.SendMail = true;
                        if (filter == 3)
                            res.Add(item);
                    }

                    if (item.ReportType == 1 && item.Paid.HasValue && item.Paid == true)
                    {
                        item.StatusText = "Downloaded";
                        item.SendMail = false;
                        if (filter == 4)
                            res.Add(item);
                    }
                }
            }
            if (filter == 1)
                res.AddRange(tmpRes);

            return res.Skip(skip).Take(pageSize).ToList();
        }


        // not used
        public static void UpdateReportRequest()
        {
            var db = new TargetFinancialDataContext();
            var db1 = new BiznisMreza.DALDataContext();

            var rr = db.ReportRequests.ToList();
            var errorList = new List<ReportRequest>();
            foreach (var item in rr)
            {
                var comp = db1.Subjekts.Where(c => c.PK_Subjekt == item.CompanyID).FirstOrDefault();

                if (comp != null)
                {
                    item.EMBS = comp.EMBS;
                }
                else
                {
                    var comp1 = db.Companies.Where(c => c.ID == item.CompanyID).FirstOrDefault();

                    if (comp1 != null)
                    {
                        item.EMBS = comp1.EMBS;
                    }
                    else
                    {
                        errorList.Add(item);
                    }
                }
            }

            db.SubmitChanges();

            db.Dispose();

            db = null;
        }

        public static List<ReportRequest> GetRequestsByCompanyEMBS(int skip, int pageSize, string EMBS)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<User>(c => c.Username);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            var comp = GetCompanyByEMBS_BM(EMBS);

            var res = db.ReportRequests.Where(c => c.CreatedOn > DateTime.Now.AddDays(-3).Date && c.CompanyID == comp.PK_Subjekt).Skip(skip).Take(pageSize).ToList();

            foreach (var item in res)
            {
                var check = CheckIfCompanyHasDataForReport(item.EMBS, item.ReportType
                    );
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;
                }
                else
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, item.EMBS, item.ReportType, item.UserID);
                    if (user_report_check == 1)
                    {
                        item.StatusText = "Downloaded";
                        item.SendMail = false;
                    }
                    else
                    {
                        item.StatusText = "Report Available";
                        item.SendMail = true;
                    }

                    if (item.ReportType == 1 && item.Paid.HasValue && item.Paid == true)
                    {
                        item.StatusText = "Downloaded";
                        item.SendMail = false;
                    }
                }

                item.CompanyName = comp.CelosenNazivNaSubjektot;
                item.EMBS = comp.EMBS;
            }

            return res;
        }

        public static List<ReportRequest> GetRequestsByUser(int skip, int pageSize, string name)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<User>(c => c.Username);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            var res = db.ReportRequests.Where(c => c.CreatedOn > DateTime.Now.AddDays(-3).Date && db.Users.Where(d => c.Username.ToLower().Contains(name.ToLower())).Any(d => d.ID == c.UserID)).Skip(skip).Take(pageSize).ToList();


            foreach (var item in res)
            {

                var check = CheckIfCompanyHasDataForReport(item.EMBS, item.ReportType);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;
                }
                else
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, item.EMBS, item.ReportType, item.UserID);
                    if (user_report_check == 1)
                    {
                        item.StatusText = "Downloaded";
                        item.SendMail = false;
                    }
                    else
                    {
                        item.StatusText = "Report Available";
                        item.SendMail = true;
                    }

                    if (item.ReportType == 1 && item.Paid.HasValue && item.Paid == true)
                    {
                        item.StatusText = "Downloaded";
                        item.SendMail = false;
                    }
                }

                var comp = GetCompanyByID(item.CompanyID);

                item.CompanyName = comp[0].Naziv;
            }

            return res;
        }

        public static void CreateReportRequest(int UserID, int ReportType, string EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();

            //var comp = GetCompanyByEMBS_BM(EMBS);

            var TFComp = GetCompanyByEMBS(EMBS);

            if (TFComp != null)
            {
                var data = new ReportRequest();

                data.CreatedOn = DateTime.Now;
                data.UserID = UserID;
                data.ReportType = ReportType;
                data.CompanyID = TFComp.ID;
                data.EMBS = TFComp.EMBS;
                data.Year = Year;
                data.Deleted = false;
                data.Paid = false;

                db.ReportRequests.InsertOnSubmit(data);

                try
                {
                    db.SubmitChanges();

                    db.Dispose();

                    db = null;
                }
                catch (Exception)
                {
                    db.Dispose();

                    db = null;
                    throw;
                }
            }
        }


        public static User GetUserByEMBS(string EMBS)
        {
            TargetFinancialDataContext db = new TargetFinancialDataContext();

            var user = db.Users.Where(c => c.EMBS == EMBS).FirstOrDefault();

            return user;
        }

        public static User GetUserByID(int id)
        {
            TargetFinancialDataContext db = new TargetFinancialDataContext();

            var user = db.Users.Where(c => c.ID == id).FirstOrDefault();

            return user;
        }

        public static string SetUserActivationCode(int ID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.ID == ID).ToList();

            if (res.Count() > 0)
            {
                res[0].ActivationCode = Guid.NewGuid();

                try
                {
                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return res[0].ActivationCode.ToString();
                }
                catch (Exception)
                {
                    db.Dispose();

                    db = null;

                    return null;
                }

            }

            return null;
        }


        public static User UpdateUserPassword(int ID, Guid code)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.ID == ID && c.ActivationCode == code).ToList();

            if (res.Count() > 0)
            {
                res[0].Password = code.ToString().Substring(0, 8);
                res[0].ActivationCode = null;

                try
                {
                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return res[0];
                }
                catch (Exception)
                {

                    db.Dispose();

                    db = null;

                    return null;
                }
            }

            return null;
        }

        public static User GetUserByEmail(string email)
        {
            TargetFinancialDataContext db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.Email == email).ToList();

            if (res.Count() > 0)
            {
                var user = db.Users.Where(c => c.Email == email).ToList();



                db.Dispose();

                db = null;

                return user[0];
            }

            return null;
        }



        public static List<User> GetUsersByName(int skip, int pageSize, string name)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.Username.ToLower().Contains(name.ToLower())).Skip(skip).Take(pageSize).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static List<User> GetUsersByEMBS(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.EMBS == EMBS).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static int GetTotalUsers()
        {
            var db = new TargetFinancialDataContext();

            var total = db.Users.Count();

            db.Dispose();
            db = null;

            return total;
        }
        public static int GetTotalUsersName(string name)
        {
            var db = new TargetFinancialDataContext();

            var total = 0;
            if (name != null)
                total = db.Users.Where(c => c.Username.ToLower().Contains(name.ToLower())).Count();
            else
                total = db.Users.Count();

            db.Dispose();
            db = null;

            return total;
        }

        public static int TipIzvestaj(string tip_izvestaj)
        {
            switch (tip_izvestaj.ToLower())
            {
                case "бонитетен извештај":
                case "бонитетен":
                case "boniteten izvestaj":
                case "boniteten":
                    return 1;
                case "краток извештај":
                case "краток":
                case "kratok izvestaj":
                case "kratok":
                    return 2;
                case "блокада":
                case "blokada":
                    return 3;
                default:
                    return 1;
            }
        }

        public static int GetTotalUserReports1(int UserID, string name, string tip_izvestaj)
        {
            var db = new TargetFinancialDataContext();


            var total = 0;
            if (name != null)
                total = db.UserReports.Where(c => c.UserID == UserID &&
                    db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.Name).FirstOrDefault().ToLower().Contains(name.ToLower())).Count();
            else if (tip_izvestaj != null)
            {
                var tip = TipIzvestaj(tip_izvestaj);
                total = db.UserReports.Where(c => c.UserID == UserID && c.PrepayPack.PackType == tip).Count();
            }
            else
                total = db.UserReports.Where(c => c.UserID == UserID).Count();

            db.Dispose();
            db = null;

            return total;
        }


        public static int GetTotalUserReports(int UserID)
        {
            var db = new TargetFinancialDataContext();

            var total = db.UserReports.Where(c => c.UserID == UserID).Count();

            db.Dispose();
            db = null;

            return total;
        }

        public static int GetTotalPrepayPacks(string name)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<PrepayPack>(c => c.User);

            var db = new TargetFinancialDataContext();

            var tmpRes = new List<PrepayPack>();
            if (name != null)
                tmpRes = db.PrepayPacks.Where(c => c.User.Username.Contains(name)).ToList();
            else
                tmpRes = db.PrepayPacks.ToList();

            var returnRes = new List<c_PrepayPackObj>();
            foreach (var item in tmpRes)
            {
                var tmpItem = returnRes.Where(c => c.EMBS == item.User.EMBS).FirstOrDefault();
                var tmp = new c_PrepayPackObj();

                if (tmpItem == null)
                {
                    tmp.EMBS = item.User.EMBS;
                }
                else
                    tmp = tmpItem;

                if (tmpItem == null)
                    returnRes.Add(tmp);
            }
            db.Dispose();
            db = null;

            return returnRes.Count();
        }


        public static List<c_PrepayPackObj> GetPrepayPackByUsername(int skip, int pageSize, string name)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<PrepayPack>(c => c.User);

            var db = new TargetFinancialDataContext();

            var tmpRes = db.PrepayPacks.Where(c => c.User.Username.Contains(name)).ToList();

            var returnRes = new List<c_PrepayPackObj>();
            foreach (var item in tmpRes)
            {
                var tmpItem = returnRes.Where(c => c.EMBS == item.User.EMBS).FirstOrDefault();
                var tmp = new c_PrepayPackObj();

                if (tmpItem == null)
                {
                    tmp.ID = item.ID;
                    tmp.EMBS = item.User.EMBS;
                    tmp.Username = item.User.Username;
                    tmp.UserID = item.UserID;
                }
                else
                    tmp = tmpItem;

                tmp.Total += item.Pack;
                tmp.Used += item.Used;

                if (tmpItem == null)
                    returnRes.Add(tmp);
            }
            db.Dispose();
            db = null;

            return returnRes.Skip(skip).Take(pageSize).ToList();

        }

        public static List<c_PrepayPackObj> GetPrepayPackByUserEMBS(string EMBS)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<PrepayPack>(c => c.User);

            var db = new TargetFinancialDataContext();

            var tmpRes = db.PrepayPacks.Where(c => c.User.EMBS == EMBS).ToList();

            var returnRes = new List<c_PrepayPackObj>();
            foreach (var item in tmpRes)
            {
                var tmpItem = returnRes.Where(c => c.EMBS == item.User.EMBS).FirstOrDefault();
                var tmp = new c_PrepayPackObj();

                if (tmpItem == null)
                {
                    tmp.ID = item.ID;
                    tmp.EMBS = item.User.EMBS;
                    tmp.Username = item.User.Username;
                    tmp.UserID = item.UserID;
                }
                else
                    tmp = tmpItem;

                tmp.Total += item.Pack;
                tmp.Used += item.Used;

                if (tmpItem == null)
                    returnRes.Add(tmp);
            }
            db.Dispose();
            db = null;

            return returnRes;
        }


        public static List<c_PrepayPackObj> GetPrepayPacksByPage(int skip, int pageSize)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<PrepayPack>(c => c.User);

            var db = new TargetFinancialDataContext();

            var tmpRes = db.PrepayPacks.ToList();

            var returnRes = new List<c_PrepayPackObj>();
            foreach (var item in tmpRes)
            {
                var tmpItem = returnRes.Where(c => c.EMBS == item.User.EMBS).FirstOrDefault();
                var tmp = new c_PrepayPackObj();

                if (tmpItem == null)
                {
                    tmp.ID = item.ID;
                    tmp.EMBS = item.User.EMBS;
                    tmp.Username = item.User.Username;
                    tmp.UserID = item.UserID;
                }
                else
                    tmp = tmpItem;

                tmp.Total += item.Pack;
                tmp.Used += item.Used;

                if (tmpItem == null)
                    returnRes.Add(tmp);
            }
            db.Dispose();
            db = null;

            return returnRes.Skip(skip).Take(pageSize).ToList();
        }

        public static int GetTotalUserPrepayPacks(int UserID)
        {
            var db = new TargetFinancialDataContext();

            var total = db.PrepayPacks.Where(c => c.UserID == UserID).Count();

            db.Dispose();
            db = null;

            return total;
        }


        public static int GetTotalUserReportsByPackID(int UserID, int PackID)
        {
            var db = new TargetFinancialDataContext();

            var total = db.UserReports.Where(c => c.UserID == UserID && c.PackID == PackID).Count();

            db.Dispose();
            db = null;

            return total;
        }



        public static int CheckReportByUserCompanyYearAndReport(int Year, string EMBS, int ReportType, int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = 0;
            res = db.UserReports.Where(c =>
                    db.Reports.Where(d =>
                        db.Companies.Where(e => e.EMBS == EMBS).Any(e =>
                            e.ID == d.CompanyID) && d.Year == Year && d.ReportType == ReportType).Any(d =>
                                d.ID == c.ReportID) && c.UserID == UserID).Count();

            if (res > 0)
                return 1;

            return 0;
        }

        public static List<CompanyDetails> GetCompanyByID(int id)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.PK_Subjekt == id).Select(c => new CompanyDetails()
            {
                ID = c.PK_Subjekt,
                EMBS = c.EMBS,
                Naziv = c.CelosenNazivNaSubjektot,
                KratokNaziv = c.KratkoIme,
                Datum = (c.DatumNaOsnovanje != null ? c.DatumNaOsnovanje.ToString() : "")
            }).ToList();

            db.Dispose();
            db = null;

            return res;
        }
        public static Company GetCompanyByID1(int id)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Companies.Where(c => c.ID == id).ToList();

            db.Dispose();
            db = null;

            if (res.Count() > 0)
                return res[0];
            return null;
        }

        public static int GetTotalSubjektCount(string name, string sediste)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var total = 0;
            if (name != null)
                total = db.Subjekts.Where(c => c.CelosenNazivNaSubjektot.ToLower().Contains(name.ToLower())).Count();
            else if (sediste != null)
                total = db.Subjekts.Where(c => c.Sediste.ToLower().Contains(sediste.ToLower())).Count();
            else
                total = db.Subjekts.Count();

            db.Dispose();
            db = null;

            return total;
        }

        public static List<c_UserReportObj> GetUserReportByEMBS(int UserID, string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var curCompanyID = 0;
            var curCompanyName = "";
            var tmpCompany = GetCompanyByEMBSTF(EMBS);
            var tmpCompany1 = new DAL.BiznisMreza.Subjekt();
            if (tmpCompany == null)
            {
                tmpCompany1 = GetCompanyByEMBS_BM(EMBS);
                if (tmpCompany1 == null)
                {
                    return new List<c_UserReportObj>();
                }
                curCompanyID = tmpCompany1.PK_Subjekt;
                curCompanyName = tmpCompany1.CelosenNazivNaSubjektot;
            }
            else
            {
                curCompanyID = tmpCompany.ID;
                curCompanyName = tmpCompany.Name;
            }

            var res = db.UserReports.Where(c => c.UserID == UserID && db.Reports.Where(f => f.ID == c.ReportID && f.CompanyID == curCompanyID).Count() > 0)
                .OrderByDescending(c => c.ID).FirstOrDefault();

            var returnRes = new List<c_UserReportObj>();
            //var res = db.UserReports.Where(c => c.UserID == UserID && 
            //    db.Companies.Where(d => d.EMBS == EMBS && db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Count() > 0)
            //    .OrderByDescending(c => c.ID).Select(c => new c_UserReportObj
            //{
            //    ID = c.ID,
            //    EMBS = db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.EMBS).FirstOrDefault(),
            //    PackID = (int)c.PackID,
            //    ReportID = c.ReportID,
            //    UserID = c.UserID,
            //    Downloads = c.Downloads,
            //    DateCreated = c.DateCreated,
            //    CompanyName = db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.Name).FirstOrDefault(),
            //    UID = (Guid)db.Reports.Where(d => d.ID == c.ReportID).Select(d => d.UID).FirstOrDefault(),
            //    PackTypeName = c.PrepayPack.PackTypeName,
            //    IsPostPaid = c.PrepayPack.IsPostPaid
            //}).FirstOrDefault();


            if (res != null)
            {

                var newUserRepObj = new c_UserReportObj
                {
                    ID = res.ID,
                    EMBS = EMBS,
                    PackID = (int)res.PackID,
                    ReportID = res.ReportID,
                    UserID = res.UserID,
                    Downloads = res.Downloads,
                    DateCreated = res.DateCreated,
                    CompanyName = curCompanyName,
                    UID = (Guid)db.Reports.Where(d => d.ID == res.ReportID).Select(d => d.UID).FirstOrDefault(),
                    PackTypeName = res.PrepayPack.PackTypeName,
                    IsPostPaid = res.PrepayPack.IsPostPaid
                };

                db.Dispose();
                db = null;

                returnRes.Add(newUserRepObj);
            }

            return returnRes;
        }

        public static List<c_UserReportObj> GetUserReportByName(int UserID, int skip, int pageSize, string name, string sort)
        {
            var db = new TargetFinancialDataContext();
            var db1 = new BiznisMreza.DALDataContext();

            var user_reports = db.UserReports.Where(c => c.UserID == UserID).ToList();
            var res = new List<c_UserReportObj>();
            foreach (var item in user_reports)
            {
                var embs = "";
                var company_name = "";
                var company = db.Companies.Where(d => d.Name.ToLower().Contains(name.ToLower()) && db.Reports.Where(f => f.ID == item.ReportID).Any(f => f.CompanyID == d.ID)).FirstOrDefault();

                var company2 = new BiznisMreza.Subjekt();
                if (company == null)
                {
                    var reports = db.Reports.Where(f => f.ID == item.ReportID).ToList();
                    foreach (var item2 in reports)
                    {
                        var tmp1 = db1.Subjekts.Where(d => d.PK_Subjekt == item2.CompanyID && d.CelosenNazivNaSubjektot.ToLower().Contains(name.ToLower())).FirstOrDefault();
                        if (tmp1 != null)
                        {
                            company2 = tmp1;
                            break;
                        }
                    }
                    if (company2.EMBS == null)
                        continue;

                    embs = company2.EMBS;
                    company_name = company2.CelosenNazivNaSubjektot;
                    if (company_name == null)
                        company_name = company2.KratkoIme;
                }
                else
                {
                    embs = company.EMBS;
                    company_name = company.Name;
                }
                var tmp = new c_UserReportObj
                {

                    ID = item.ID,
                    EMBS = embs,
                    PackID = (int)item.PackID,
                    ReportID = item.ReportID,
                    UserID = item.UserID,
                    Downloads = item.Downloads,
                    DateCreated = item.DateCreated,
                    CompanyName = company_name,
                    UID = (Guid)db.Reports.Where(d => d.ID == item.ReportID).Select(d => d.UID).FirstOrDefault(),
                    PackTypeName = item.PrepayPack.PackTypeName,
                    IsPostPaid = item.PrepayPack.IsPostPaid
                };

                res.Add(tmp);
            }
            var order_type = sort.Split('-');

            var property = order_type[1];

            var returnRes = new List<c_UserReportObj>();
            if (order_type[0] == "asc")
            {
                returnRes = res.OrderBy(c => c.GetType().GetProperty(property).GetValue(c, null)).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                returnRes = res.OrderByDescending(c => c.GetType().GetProperty(property).GetValue(c, null)).Skip(skip).Take(pageSize).ToList();
            }

            db.Dispose();
            db = null;
            db1.Dispose();
            db1 = null;

            return returnRes;

        }

        public static List<c_UserReportObj> GetUserReportByTip(int UserID, int skip, int pageSize, string tip_izvestaj, string sort)
        {
            var db = new TargetFinancialDataContext();
            var db1 = new BiznisMreza.DALDataContext();

            var tip = TipIzvestaj(tip_izvestaj);

            var user_reports = db.UserReports.Where(c => c.UserID == UserID && c.PrepayPack.PackType == tip).ToList();
            var res = new List<c_UserReportObj>();
            foreach (var item in user_reports)
            {
                var embs = "";
                var company_name = "";
                var company = db.Companies.Where(d => db.Reports.Where(f => f.ID == item.ReportID).Any(f => f.CompanyID == d.ID)).FirstOrDefault();
                var company2 = new BiznisMreza.Subjekt();
                if (company == null)
                {
                    var reports = db.Reports.Where(f => f.ID == item.ReportID).ToList();
                    foreach (var item2 in reports)
                    {
                        var tmp1 = db1.Subjekts.Where(d => d.PK_Subjekt == item2.CompanyID).FirstOrDefault();
                        if (tmp1 != null)
                        {
                            company2 = tmp1;
                            break;
                        }
                    }
                    embs = company2.EMBS;
                    company_name = company2.CelosenNazivNaSubjektot;
                    if (company_name == null)
                        company_name = company2.KratkoIme;
                }
                else
                {
                    embs = company.EMBS;
                    company_name = company.Name;
                }
                var tmp = new c_UserReportObj
                {

                    ID = item.ID,
                    EMBS = embs,
                    PackID = (int)item.PackID,
                    ReportID = item.ReportID,
                    UserID = item.UserID,
                    Downloads = item.Downloads,
                    DateCreated = item.DateCreated,
                    CompanyName = company_name,
                    UID = (Guid)db.Reports.Where(d => d.ID == item.ReportID).Select(d => d.UID).FirstOrDefault(),
                    PackTypeName = item.PrepayPack.PackTypeName,
                    IsPostPaid = item.PrepayPack.IsPostPaid
                };

                res.Add(tmp);
            }
            var order_type = sort.Split('-');

            var property = order_type[1];

            var returnRes = new List<c_UserReportObj>();
            if (order_type[0] == "asc")
            {
                returnRes = res.OrderBy(c => c.GetType().GetProperty(property).GetValue(c, null)).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                returnRes = res.OrderByDescending(c => c.GetType().GetProperty(property).GetValue(c, null)).Skip(skip).Take(pageSize).ToList();
            }

            db.Dispose();
            db = null;
            db1.Dispose();
            db1 = null;

            return returnRes;
        }

        public static List<c_UserReportObj> GetUserReportsByPage(int skip, int pageSize, int UserID, string sort)
        {
            var db = new TargetFinancialDataContext();
            var db1 = new BiznisMreza.DALDataContext();

            var user_reports = db.UserReports.Where(c => c.UserID == UserID).ToList();
            var res = new List<c_UserReportObj>();
            foreach (var item in user_reports)
            {
                var embs = "";
                var company_name = "";
                var company = db.Companies.Where(d => db.Reports.Where(f => f.ID == item.ReportID).Any(f => f.CompanyID == d.ID)).FirstOrDefault();
                var company2 = new BiznisMreza.Subjekt();
                if (company == null)
                {
                    var reports = db.Reports.Where(f => f.ID == item.ReportID).ToList();
                    foreach (var item2 in reports)
                    {
                        var tmp1 = db1.Subjekts.Where(d => d.PK_Subjekt == item2.CompanyID).FirstOrDefault();
                        if (tmp1 != null)
                        {
                            company2 = tmp1;
                            break;
                        }
                    }
                    embs = company2.EMBS;
                    company_name = company2.CelosenNazivNaSubjektot;
                    if (company_name == null)
                        company_name = company2.KratkoIme;
                }
                else
                {
                    embs = company.EMBS;
                    company_name = company.Name;
                }
                var tmp = new c_UserReportObj
                {

                    ID = item.ID,
                    EMBS = embs,
                    PackID = (int)item.PackID,
                    ReportID = item.ReportID,
                    UserID = item.UserID,
                    Downloads = item.Downloads,
                    DateCreated = item.DateCreated,
                    CompanyName = company_name,
                    UID = (Guid)db.Reports.Where(d => d.ID == item.ReportID).Select(d => d.UID).FirstOrDefault(),
                    PackTypeName = item.PrepayPack.PackTypeName,
                    IsPostPaid = item.PrepayPack.IsPostPaid
                };

                res.Add(tmp);
            }
            var order_type = sort.Split('-');

            var property = order_type[1];

            var returnRes = new List<c_UserReportObj>();
            if (order_type[0] == "asc")
            {
                returnRes = res.OrderBy(c => c.GetType().GetProperty(property).GetValue(c, null)).Skip(skip).Take(pageSize).ToList();
            }
            else
            {
                returnRes = res.OrderByDescending(c => c.GetType().GetProperty(property).GetValue(c, null)).Skip(skip).Take(pageSize).ToList();
            }

            db.Dispose();
            db = null;
            db1.Dispose();
            db1 = null;

            return returnRes;
        }

        public static List<c_UserReportObj> GetAllUserReportsByType(int UserID, int ReportType)
        {

            var db = new TargetFinancialDataContext();
            var db1 = new BiznisMreza.DALDataContext();

            var user_reports = db.UserReports.Where(c => c.UserID == UserID).ToList();
            var res = new List<c_UserReportObj>();
            foreach (var item in user_reports)
            {
                if (item.PrepayPack.PackType != 1)
                    continue;
                var embs = "";
                var company_name = "";
                var company = db.Companies.Where(d => db.Reports.Where(f => f.ID == item.ReportID && f.ReportType == ReportType).Any(f => f.CompanyID == d.ID)).FirstOrDefault();
                var company2 = new BiznisMreza.Subjekt();
                if (company == null)
                {
                    var reports = db.Reports.Where(f => f.ID == item.ReportID && f.ReportType == ReportType).ToList();
                    foreach (var item2 in reports)
                    {
                        var tmp1 = db1.Subjekts.Where(d => d.PK_Subjekt == item2.CompanyID).FirstOrDefault();
                        if (tmp1 != null)
                        {
                            company2 = tmp1;
                            break;
                        }
                    }
                    embs = company2.EMBS;
                    company_name = company2.CelosenNazivNaSubjektot;
                    if (company_name == null)
                        company_name = company2.KratkoIme;
                }
                else
                {
                    embs = company.EMBS;
                    company_name = company.Name;
                }
                if (string.IsNullOrEmpty(embs) || string.IsNullOrEmpty(company_name))
                    continue;

                var tmp = new c_UserReportObj
                {

                    ID = item.ID,
                    EMBS = embs,
                    PackID = (int)item.PackID,
                    ReportID = item.ReportID,
                    UserID = item.UserID,
                    Downloads = item.Downloads,
                    DateCreated = item.DateCreated,
                    CompanyName = company_name,
                    UID = (Guid)db.Reports.Where(d => d.ID == item.ReportID && d.ReportType == ReportType).Select(d => d.UID).FirstOrDefault(),
                    PackTypeName = item.PrepayPack.PackTypeName,
                    IsPostPaid = item.PrepayPack.IsPostPaid
                };

                res.Add(tmp);
            }

            db.Dispose();
            db = null;
            db1.Dispose();
            db1 = null;

            return res;
        }

        public static List<PrepayPack> GetUserPrepayPacksByPage(int skip, int pageSize, int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.UserID == UserID).Skip(skip).Take(pageSize).OrderByDescending(c => c.DateCreated).ToList();

            foreach (var item in res)
            {
                var tmp = item.PackTypeName;
            }

            db.Dispose();
            db = null;

            return res;
        }

        public static List<c_UserReportObj> GetUserReportsByPackIDByPage(int skip, int pageSize, int UserID, int PackID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.UserReports.Where(c => c.UserID == UserID && c.PackID == PackID).Skip(skip).Take(pageSize).Select(c => new c_UserReportObj
            {
                ID = c.ID,
                PackID = (int)c.PackID,
                ReportID = c.ReportID,
                UserID = c.UserID,
                EMBS = db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.EMBS).FirstOrDefault(),
                Downloads = c.Downloads,
                DateCreated = c.DateCreated,
                CompanyName = db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.Name).FirstOrDefault(),
                UID = (Guid)db.Reports.Where(d => d.ID == c.ReportID).Select(d => d.UID).FirstOrDefault(),
                PackTypeName = c.PrepayPack.PackTypeName,
                IsPostPaid = c.PrepayPack.IsPostPaid
            }).ToList();
            db.Dispose();
            db = null;

            return res;
        }

        public static void DisableUser(int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.ID == UserID).ToList();

            if (res.Count() > 0)
            {
                res[0].IsActive = false;

                db.SubmitChanges();
            }

            db.Dispose();
            db = null;

        }

        public static void EnableUser(int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.ID == UserID).ToList();

            if (res.Count() > 0)
            {
                res[0].IsActive = true;

                db.SubmitChanges();
            }

            db.Dispose();
            db = null;

        }

        public static void DisablePack(int PackID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.ID == PackID).ToList();

            if (res.Count() > 0)
            {
                res[0].Active = false;

                db.SubmitChanges();
            }

            db.Dispose();
            db = null;

        }

        public static void ActivatePack(int PackID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.ID == PackID).ToList();

            if (res.Count() > 0)
            {
                res[0].Active = true;

                db.SubmitChanges();
            }

            db.Dispose();
            db = null;

        }
        public static int GetCurrentYear(bool force)
        {
            var db = new TargetFinancialDataContext();

            var curYear = db.GlobalConfigs.FirstOrDefault();
            if (force && curYear.ForcedYear.HasValue)
            {
                db.Dispose();

                db = null;

                return curYear.ForcedYear.Value;
            }

            db.Dispose();

            db = null;

            return curYear.GlobalYear;
        }

        public static void DeleteForcedYear()
        {
            var db = new TargetFinancialDataContext();

            var tmpYear = db.GlobalConfigs.FirstOrDefault();

            tmpYear.ForcedYear = null;

            db.SubmitChanges();
            db.Dispose();

            db = null;
        }
        public static void UpdateForcedYear(int year)
        {
            var db = new TargetFinancialDataContext();

            var tmpYear = db.GlobalConfigs.FirstOrDefault();

            tmpYear.ForcedYear = year;

            db.SubmitChanges();
            db.Dispose();

            db = null;
        }

        public static bool UpdateGlobalYear(int year)
        {
            var db = new TargetFinancialDataContext();

            var curYear = new GlobalConfig { GlobalYear = year };

            var tmpYear = db.GlobalConfigs.FirstOrDefault();
            if (tmpYear == null)
            {
                db.GlobalConfigs.InsertOnSubmit(curYear);
            }
            else
            {
                tmpYear = curYear;
            }

            db.SubmitChanges();
            db.Dispose();

            db = null;

            return true;
        }

        public static bool UpdateAdminUser(string password)
        {
            var db = new TargetFinancialDataContext();

            var res = db.AdminUsers.Where(c => c.Username == "Admin").ToList();

            if (res.Count() > 0)
            {
                res[0].Password = password;

                try
                {
                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
                catch (Exception ex)
                {

                    var a = ex;
                    db.Dispose();

                    db = null;
                    return false;
                }
            }
            return false;
        }

        public static bool UpdateUser(User user)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.ID == user.ID).ToList();

            if (res.Count() > 0)
            {
                res[0].Username = user.Username;
                res[0].Password = user.Password;
                res[0].Email = user.Email;
                res[0].EMBS = user.EMBS;

                try
                {
                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return true;
                }
                catch (Exception ex)
                {

                    var a = ex;
                    db.Dispose();

                    db = null;
                    return false;
                }
            }
            return false;
        }

        public static bool CheckIfUserEMBSExists(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Where(c => c.EMBS == EMBS).FirstOrDefault();

            if (res != null)
                return true;
            return false;
        }

        public static User CreateUser(string Username, string Password, string Email, string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var new_user = new User
            {
                Username = Username,
                Password = Password,
                Email = Email,
                EMBS = EMBS,
                IsActive = true
            };


            db.Users.InsertOnSubmit(new_user);

            try
            {
                db.SubmitChanges();

                db.Dispose();

                db = null;

                return new_user;
            }
            catch (Exception ex)
            {

                var a = ex;
                db.Dispose();

                db = null;

                return null;
            }
        }

        public static PrepayPack GetPrepayPackByID(int PackID, int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.UserID == UserID && c.ID == PackID).ToList();

            if (res.Count() > 0)
            {
                return res[0];
            }
            return null;
        }

        public static bool UpdatePrepayPack(int PackID, int UserID, DateTime DateStart, DateTime DateEnd, int Pack, string Comment, bool IsPostPaid)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.UserID == UserID && c.ID == PackID).ToList();

            if (res.Count() > 0)
            {
                var prepay_pack = res[0];

                prepay_pack.Pack = Pack;
                prepay_pack.DateStart = DateStart;
                prepay_pack.DateEnd = DateEnd;
                prepay_pack.Comment = Comment;
                prepay_pack.IsPostPaid = IsPostPaid;
            }

            try
            {
                db.SubmitChanges();

                db.Dispose();

                db = null;

                return true;
            }
            catch (Exception ex)
            {

                var a = ex;
                db.Dispose();

                db = null;

                return false;
            }
        }

        public static bool CreatePrepayPack(int UserID, DateTime DateStart, DateTime DateEnd, int Pack, string Comment, int PackType, bool IsPostPaid)
        {
            var db = new TargetFinancialDataContext();

            var new_pack = new PrepayPack
            {
                DateStart = DateStart,
                DateEnd = DateEnd,
                DateCreated = DateTime.Now,
                Pack = Pack,
                Used = 0,
                UserID = UserID,
                Comment = Comment,
                Active = true,
                PackType = PackType,
                IsPostPaid = IsPostPaid
            };

            db.PrepayPacks.InsertOnSubmit(new_pack);

            try
            {
                db.SubmitChanges();

                db.Dispose();

                db = null;

                return true;
            }
            catch (Exception ex)
            {
                var a = ex;
                db.Dispose();

                db = null;

                return false;
            }
        }

        public static List<User> GetUsersByPage(int skip, int pageSize)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Users.Skip(skip).Take(pageSize).ToList();

            db.Dispose();
            db = null;

            return res;
        }



        public static List<Subjekt> GetSubjektsByPage(int skip, int pageSize)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Subjekts.Skip(skip).Take(pageSize).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static List<Subjekt> GetSubjektByEMBS(string EMBS)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS.Contains(EMBS)).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static List<Subjekt> GetSubjektByName(int skip, int pageSize, string name)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.CelosenNazivNaSubjektot.ToLower().Contains(name.ToLower())).Skip(skip).Take(pageSize).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static List<Subjekt> GetSubjektBySediste(int skip, int pageSize, string sediste)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.Sediste.ToLower().Contains(sediste.ToLower())).Skip(skip).Take(pageSize).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static CRMResponse GetResponse(int CompanyID, int Year, int Type)
        {
            var db = new TargetFinancialDataContext();

            var crmResponse = db.CRMResponses.Where(c => c.CompanyID == CompanyID && c.Year == Year && c.Type == Type).OrderByDescending(c => c.ID).FirstOrDefault();


            db.Dispose();

            db = null;

            return crmResponse;
        }
        public static void SaveReportToDatabase(string filename, int CompanyID, int Year, Guid uid, int Type, int PackID)
        {
            var db = new TargetFinancialDataContext();

            var data = new Report()
            {
                Path = filename,
                CompanyID = CompanyID,
                Year = Year,
                UID = uid,
                ReportType = Type
            };

            db.Reports.InsertOnSubmit(data);

            var crmResponse = db.CRMResponses.Where(c => c.CompanyID == CompanyID && c.Year == Year && c.Type == Type).FirstOrDefault();
            if (crmResponse != null)
            {
                crmResponse.ReportID = data.ID;
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                var a = ex;

            }

            db.Dispose();

            db = null;
        }

        public static Report GetReportByIDFromDB(int ReportID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Reports.Where(c => c.ID == ReportID).ToList();

            db.Dispose();

            db = null;

            if (res.Count() > 0)
                return res[0];
            return null;

        }

        public static Report GetReportByUIDFromDB(Guid uid)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Reports.Where(c => c.UID == uid).ToList();

            db.Dispose();

            db = null;

            if (res.Count() > 0)
                return res[0];
            return null;

        }

        public static bool CheckReportByCompanyID(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var company = db.Companies.Where(c => c.EMBS == EMBS).ToList();

            var Year = DateTime.Now.Year - 2;

            if (company.Count() > 0)
            {
                var res = db.Reports.Where(c => c.CompanyID == company[0].ID && c.Year == Year).ToList();

                db.Dispose();

                db = null;

                if (res.Count() > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfCompanyHasDataForReport(string EMBS, int Type)
        {
            var db = new TargetFinancialDataContext();

            var company = db.Companies.Where(c => c.EMBS == EMBS).ToList();

            if (company.Count() > 0)
            {
                if (Type == 1)
                {
                    var compYears = GetLastYearByCompanyID(company[0].ID);
                    var curYear = GetCurrentYear(false);
                    if (compYears != null)
                    {
                        if (curYear != compYears.Year)
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                    var res = db.CompanyValues.Where(c => c.CompanyID == company[0].ID).Count();

                    db.Dispose();

                    db = null;

                    if (res > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    var compYears = GetLastYearByCompanyID(company[0].ID);
                    var curYear = GetCurrentYear(false);
                    if (compYears != null)
                    {
                        if (curYear != compYears.Year)
                            return false;
                    }
                    else
                    {
                        return false;
                    }

                    var res = db.ReportValues.Where(c => c.CompanyID == company[0].ID).Count();

                    db.Dispose();

                    db = null;

                    if (res > 0)
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                db.Dispose();

                db = null;

                return false;
            }
        }

        public static Report GetReportByUserCompanyYearAndReportFromDB(int UserID, string EMBS, int Year, int ReportType)
        {
            var db = new TargetFinancialDataContext();


            var res = db.Reports.Where(c =>
                db.Companies.Where(d => d.EMBS == EMBS).Any(d => d.ID == c.CompanyID) &&
                c.Year == Year &&
                c.ReportType == ReportType).ToList();

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
            return null;
        }

        public static PrepayPack CheckPrepayLicence(int UserID, int PackType, int UserType)
        {
            if (UserType == 2)
                return new PrepayPack();

            var db = new TargetFinancialDataContext();

            var date = DateTime.Now.Date;

            var res = db.PrepayPacks.Where(c => c.UserID == UserID && c.Active == true && c.DateStart <= date &&
                c.DateEnd.Date >= date && c.Pack > c.Used && c.PackType == PackType).FirstOrDefault();

            db.Dispose();

            db = null;

            if (res != null)
                return res;
            return null;
        }

        public static void UpdateUserPrepay(int PrepayID, int UserID, int PackType)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.ID == PrepayID && c.UserID == UserID && c.PackType == PackType).ToList();


            if (res.Count() > 0)
            {
                res[0].Used = (res[0].Used + 1);

                db.SubmitChanges();
            }

            db.Dispose();

            db = null;
        }

        public static void SaveDownloadInLog(Guid uid)
        {
            var db = new TargetFinancialDataContext();

            var new_log = new ReportLog
            {
                DownloadDate = DateTime.Now,
                ReportID = db.Reports.Where(c => c.UID == uid).Select(c => c.ID).ToList()[0]
            };

            db.ReportLogs.InsertOnSubmit(new_log);

            db.SubmitChanges();

            db.Dispose();

            db = null;
        }

        public static void CreateUserReport(int Year, int UserID, string EMBS, int ReportType, int PackID, string uid)
        {
            var db = new TargetFinancialDataContext();

            var UID = new Guid(uid);

            var report = db.Reports.Where(d => d.UID == UID).ToList();

            if (report.Count() > 0)
            {
                var new_report = new UserReport
                {
                    ReportID = report[0].ID,
                    UserID = UserID,
                    Downloads = 0,
                    DateCreated = DateTime.Now,
                    PackID = PackID
                };

                db.UserReports.InsertOnSubmit(new_report);

                db.SubmitChanges();
            }
            db.Dispose();

            db = null;

        }

        public static void UpdateUserReport(int UserID, Guid uid)
        {
            var db = new TargetFinancialDataContext();

            var report = GetReportByUIDFromDB(uid);

            var res = db.UserReports.Where(c => c.ReportID == report.ID && c.UserID == UserID).ToList();

            if (res.Count > 0)
            {
                res[0].Downloads = (res[0].Downloads + 1);

                db.SubmitChanges();
            }

            db.Dispose();

            db = null;

        }

        public static Company CheckIfDataForShortReportExist(string EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Companies.Where(c => c.EMBS == EMBS).ToList();

            if (res.Count > 0)
            {
                var curYear = db.CompanyYears.Where(c => c.Year == Year && c.CompanyID == res[0].ID).FirstOrDefault();
                if (curYear == null)
                {
                    db.CompanyYears.InsertOnSubmit(new CompanyYear
                    {
                        CompanyID = res[0].ID,
                        Year = Year
                    });

                    db.SubmitChanges();

                    db.Dispose();

                    db = null;

                    return null;
                }
                else
                {
                    var report = db.ReportValues.Where(c => c.CompanyID == res[0].ID && c.YearID == curYear.ID).ToList();

                    if (report.Count() > 0)
                    {
                        db.Dispose();

                        db = null;

                        return res[0];
                    }
                    else
                    {
                        db.Dispose();

                        db = null;

                        return null;
                    }
                }
            }

            db.Dispose();

            db = null;

            return null;
        }

        public static Company GetCompanyByEMBSTF(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var res = db.Companies.Where(c => c.EMBS == EMBS).ToList();

            db.Dispose();

            db = null;

            if (res.Count() > 0)
            {
                return res[0];
            }

            return null;
        }

        public static Company GetCompanyByEMBS(string EMBS)
        {
            var db = new TargetFinancialDataContext();
            var db1 = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Companies.Where(c => c.EMBS == EMBS).ToList();

            if (res.Count > 0)
            {

                db.Dispose();

                db = null;
                return res[0];
            }
            else
            {
                var r = db1.Subjekts.Where(c => c.EMBS == EMBS).ToList();

                if (r.Count > 0)
                {
                    res.Add(new Company
                    {
                        Name = r[0].CelosenNazivNaSubjektot,
                        Mesto = r[0].Sediste,
                        EMBS = r[0].EMBS
                    });
                }

                db.Companies.InsertAllOnSubmit(res);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception e)
                {
                    return null;
                }
                db.Dispose();
                db = null;

                db1.Dispose();

                db1 = null;
                if (res.Count > 0)
                    return res[0];
                else
                    return null;
            }
        }

        public static List<OrganizacionaEdinicaObject> GetOrganizacionaEdinicaByID(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var oe = db.OrganizacionaEdinicas.Where(c => c.FK_Subjekt == ID && c.PodatociteSeIzbrisani == false).ToList();

            var ds = db.DejnostSifras.Where(c => db.OrganizacionaEdinicas.Where(d => d.PodatociteSeIzbrisani == false && d.FK_Subjekt == ID).Any(d => d.FK_DejnostSifra == c.PK_DejnostSifra)).ToList();



            var organizacioni_edinici = new List<OrganizacionaEdinicaObject>();

            foreach (var item in oe)
            {
                var newOrgEd = new OrganizacionaEdinicaObject
                {
                    ID = item.PK_OrganizacionaEdinica,
                    Naziv = item.Naziv,
                    Podbroj = item.Podbroj,
                    Podtip = item.Podtip,
                    Tip = item.Tip,
                    Adresa = item.Adresa,
                    FK_DejnostSifra = Convert.ToInt32(item.FK_DejnostSifra),
                    FK_Subjekt = Convert.ToInt32(item.FK_Subjekt),
                    PrioritetnaDejnost = ds.Where(c => c.PK_DejnostSifra == item.FK_DejnostSifra).Select(c => c.PrioritetnaDejnost).FirstOrDefault(),
                    GlavnaPrihodnaSifra = ds.Where(c => c.PK_DejnostSifra == item.FK_DejnostSifra).Select(c => c.GlavnaPrihodnaShifra).FirstOrDefault(),
                    OvlastenoLice = "",
                    Ovlastuvanja = ""
                };

                var povrzanost = db.Povrzanosts.Where(c => c.PodatociteSeIzbrisani == false && c.FK_OrganizacionaEdinica == item.PK_OrganizacionaEdinica).FirstOrDefault();
                if (povrzanost != null)
                {
                    var fizicko_lice = db.FizickoLices.Where(c => c.PK_FizickoLice == povrzanost.FK_FizickoLice).FirstOrDefault();

                    if (fizicko_lice != null)
                    {
                        newOrgEd.OvlastenoLice = fizicko_lice.Ime;
                    }
                    newOrgEd.Ovlastuvanja = povrzanost.Ovlastuvanja;
                }

                organizacioni_edinici.Add(newOrgEd);
            }

            db.Dispose();

            db = null;

            if (organizacioni_edinici.Count > 0)
                return organizacioni_edinici;
            return null;
        }

        public static Subjekt GetCompanyByEMBS_BM(string EMBS)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Subjekts.Where(c => c.EMBS == EMBS).ToList();

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
            return null;
        }

        public static void GetFullSubjekt(int ID)
        {
            var db = new BiznisMreza.DALDataContext();

            var subjekt = db.Subjekts.Where(c => c.PK_Subjekt == ID).FirstOrDefault();

            var tmpD = db.Dejnosts.Where(c => c.PodatociteSeIzbrisani == false && c.FK_Subjekt == ID && (c.Tip == null || c.Tip == 1)).OrderByDescending(c => c.PK_Dejnost).FirstOrDefault();
            var dejnost = db.DejnostSifras.Where(c => c.PK_DejnostSifra == tmpD.FK_DejnostSifra).FirstOrDefault();

            var osnovna_glavnina = db.OsnovnaGlavinas.Where(c => c.FK_Subjekt == ID).FirstOrDefault();

            db.Dispose();

            db = null;
        }

        public static DopolnitelniInformacii GetCompanyAddInfoByEMBS_BM(string EMBS)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();


            var company = db.Subjekts.Where(c => c.EMBS == EMBS).ToList();

            if (company.Count > 0)
            {
                var res = db.DopolnitelniInformaciis.Where(c => c.FK_Subjekt == company[0].PK_Subjekt).ToList();

                db.Dispose();

                db = null;

                if (res.Count() > 0)
                    return res[0];
            }
            return null;
        }

        public static OsnovnaGlavina GetOsnovnaGalvinaForCompany(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.OsnovnaGlavinas.Where(c => c.FK_Subjekt == ID).ToList();

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
            return null;

        }

        public static DejnostSifra GetDejnostForCompany(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Dejnosts.Where(c => c.PodatociteSeIzbrisani == false && c.FK_Subjekt == ID).OrderByDescending(c => c.Datum).ToList();

            foreach (var item in res)
            {
                var DejnostSifraID = item.FK_DejnostSifra;
                if (DejnostSifraID.HasValue)
                {
                    var res1 = db.DejnostSifras.Where(c => c.PK_DejnostSifra == DejnostSifraID.Value).FirstOrDefault();

                    db.Dispose();

                    db = null;

                    return res1;
                }
            }

            return null;

        }

        public static DejnostSifra GetDejnostSifraForCompany(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.DejnostSifras.Where(c => c.PK_DejnostSifra == ID).ToList();

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
            return null;

        }

        public static RabotnoVreme GetRabotnoVremeForCompany(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.RabotnoVremes.Where(c => c.FK_Subjekt == ID).ToList();

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
            return null;

        }

        public static List<OvlastenoLiceObject> GetPovrzanostForCompany(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var povrzanost = db.Povrzanosts.Where(c => c.FK_Subjekt == ID && c.PodatociteSeIzbrisani == false).ToList();

            List<OvlastenoLiceObject> list = new List<OvlastenoLiceObject>();

            foreach (var item in povrzanost)
            {
                var tmp = db.FizickoLices.Where(c => c.PK_FizickoLice == item.FK_FizickoLice).ToList()[0];

                var tmp1 = db.TipPovrzanosts.Where(c => c.PK_TipPovrzanost == item.FK_TipPovrzanost).ToList()[0];

                var existing = list.FirstOrDefault(c => c.Ime.Equals(tmp.Ime));
                if (existing != null)
                {
                    if (existing.Povrzanost != null)
                        existing.Povrzanost += "/" + tmp1.Povrzanost;
                    else
                        existing.Povrzanost = tmp1.Povrzanost;

                    existing.Ovlastuvanja = item.Ovlastuvanja;

                    if (item.Ogranicuvanja != null && item.Ogranicuvanja.Contains("Податоците се избришани") == false)
                    {
                        if (existing.Ogranicuvanja != null)
                            existing.Ogranicuvanja += ", " + item.Ogranicuvanja;
                        else
                            existing.Ogranicuvanja = item.Ogranicuvanja;
                    }

                    if (existing.TipOvlastuvanja1 != null)
                        existing.TipOvlastuvanja1 += ", " + item.TipNaOvlastuvanje1;
                    else
                        existing.TipOvlastuvanja1 = item.TipNaOvlastuvanje1;

                    if (existing.TipOvlastuvanja2 != null)
                        existing.TipOvlastuvanja2 += "/" + item.TipNaOvlastuvanje2;
                    else
                        existing.TipOvlastuvanja2 = item.TipNaOvlastuvanje2;

                    if ((FL_TipPovrzanost)Enum.ToObject(typeof(FL_TipPovrzanost), tmp1.PK_TipPovrzanost) != FL_TipPovrzanost.Sopstvenik &&
                        (FL_TipPovrzanost)Enum.ToObject(typeof(FL_TipPovrzanost), tmp1.PK_TipPovrzanost) != FL_TipPovrzanost.OvlastenoLice)
                        existing.IsOwner = (FL_TipPovrzanost)Enum.ToObject(typeof(FL_TipPovrzanost), tmp1.PK_TipPovrzanost);
                }
                else
                {
                    var tmpItem = new OvlastenoLiceObject
                    {
                        ID = tmp.PK_FizickoLice,
                        Ime = tmp.Ime,
                        Povrzanost = tmp1.Povrzanost,
                        Ovlastuvanja = item.Ovlastuvanja,
                        TipOvlastuvanja1 = item.TipNaOvlastuvanje1,
                        TipOvlastuvanja2 = item.TipNaOvlastuvanje2,
                        IsOwner = (FL_TipPovrzanost)Enum.ToObject(typeof(FL_TipPovrzanost), tmp1.PK_TipPovrzanost)
                    };
                    if (item.Ogranicuvanja != null && item.Ogranicuvanja.Contains("Податоците се избришани") == false)
                    {
                        tmpItem.Ogranicuvanja = item.Ogranicuvanja;
                    }

                    list.Add(tmpItem);
                }

            }
            db.Dispose();

            db = null;

            if (list.Count > 0)
                return list;
            return null;

        }

        public static int GetSemaphore(int EMBS)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.GetSemaphoreResult(EMBS);

            var semaforeres = res.FirstOrDefault();

            db.Dispose();

            db = null;

            if (semaforeres == null)
            {
                return -1;
            }

            return semaforeres.result;
        }

        public static List<PromeniCompany> GetPromeniForCompany(int EMBS, bool force_crm)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var CurYear = DALHelper.GetCurrentYear(force_crm);
            var curFullDate = DateTime.Now;
            var lastYear = DateTime.Now.AddYears(-1);

            var list = new List<PromeniCompany>();

            var historyList = db.GetCompanyHistoryCombined(EMBS);

            foreach (var item in historyList)
            {
                if (item.VidNaPromena.Contains("Упис на промена"))
                    continue;
                if (item.VidNaPromena.Contains("Документ за регистрирање на работно време"))
                    continue;
                list.Add(new PromeniCompany
                {
                    OdobruvanjeNaPrijava = item.OdobruvanjeNaPrijava.Value,
                    VidNaPromena = item.VidNaPromena,
                    Opis = item.Opis
                });
            }

            db.Dispose();

            db = null;

            list = list.OrderByDescending(c => c.OdobruvanjeNaPrijava).ToList();
            if (list.Count > 0)
                return list;
            return null;
        }

        public static List<SolventnostCompany> GetSolventnostForCompany(int ID, bool force_crm)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var tmpRes = db.GetUpisSubjektById(ID);

            var included = new List<int>() { 2, 3, 4, 6, 8, 9, 22, 23, 25, 26, 27 };
            var list = new List<SolventnostCompany>();
            var CurYear = DALHelper.GetCurrentYear(force_crm);
            var curFullDate = DateTime.Now;
            var lastYear = DateTime.Now.AddYears(-1);

            foreach (var item in tmpRes)
            {
                var tmpDate = Convert.ToDateTime(item.OdobruvanjeNaPrijava);
                if (tmpDate >= lastYear && tmpDate <= curFullDate && CurYear >= tmpDate.Year)
                {
                    if (included.Contains(item.FK_TipNaUpis))
                    {
                        list.Add(new SolventnostCompany
                        {
                            ID = item.PK_Upis,
                            OdobruvanjeNaPrijava = item.OdobruvanjeNaPrijava.ToString(),
                            TipNaPostapka = item.TipNaPostapka,
                            Dispozitiv = item.Dispozitiv
                        });
                    }
                }
            }

            //var delovodniks = db.Delovodniks.Where(c => c.FK_Subjekt == ID && included.Contains(c.FK_TipNaUpis.Value)).OrderByDescending(c => c.OdobruvanjeNaPrijava).ToList();

            //foreach (var item in delovodniks)
            //{
            //    var tmpDate = Convert.ToDateTime(item.OdobruvanjeNaPrijava);
            //    if (tmpDate >= lastYear && tmpDate <= curFullDate && CurYear >= tmpDate.Year)
            //    {
            //        var tmp = db.Upis_Subjekts.Where(c => c.FK_TipNaUpis == item.FK_TipNaUpis).FirstOrDefault();
            //        if (tmp != null)
            //        {
            //            list.Add(new SolventnostCompany
            //            {
            //                ID = item.PK_Deleovodnik,
            //                OdobruvanjeNaPrijava = item.OdobruvanjeNaPrijava,
            //                TipNaPostapka = tmp.TipNaPostapka,
            //                Dispozitiv = tmp.Dispozitiv
            //            });
            //        }
            //    }
            //}

            db.Dispose();

            db = null;

            if (list.Count > 0)
                return list;
            return null;
        }


        public static List<KazniISankcii> GetKazniISankciiForCompany(int ID, bool force_crm)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();
            var included = new List<int>() { 7, 10 };

            var delovodniks = db.Delovodniks.Where(c => c.FK_Subjekt == ID && included.Contains(c.FK_TipNaUpis.Value)).OrderByDescending(c => c.OdobruvanjeNaPrijava).ToList();

            var list = new List<KazniISankcii>();
            var CurYear = DALHelper.GetCurrentYear(force_crm);
            var curFullDate = DateTime.Now;
            var lastYear = DateTime.Now.AddYears(-1);
            foreach (var item in delovodniks)
            {
                var tmpDate = Convert.ToDateTime(item.OdobruvanjeNaPrijava);
                if (tmpDate >= lastYear && tmpDate <= curFullDate && CurYear >= tmpDate.Year)
                {
                    var tmp = db.Upis_Subjekts.Where(c => c.FK_TipNaUpis == item.FK_TipNaUpis).FirstOrDefault();
                    if (tmp != null)
                    {
                        list.Add(new KazniISankcii
                        {
                            ID = item.PK_Deleovodnik,
                            OdobruvanjeNaPrijava = item.OdobruvanjeNaPrijava,
                            TipNaPostapka = tmp.TipNaPostapka,
                            PovrzanostStatusVoPKD = tmp.Povrzanost_StatusVoPKD
                        });
                    }
                }
            }

            db.Dispose();

            db = null;

            if (list.Count > 0)
                return list;
            return null;
        }


        public static CompanyYearsBak GetCYTempByID(int ID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.CompanyYearsBaks.Where(c => c.ID == ID).FirstOrDefault();

            db.Dispose();

            db = null;

            return res;
        }

        public static CompanyYear GetYearByID(int ID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.CompanyYears.Where(c => c.ID == ID).FirstOrDefault();

            db.Dispose();

            db = null;

            return res;
        }

        public static CompanyYear GetLastYearByCompanyID(int ID)
        {
            var db = new TargetFinancialDataContext();

            //var values = 0;
            //if(Type == 2)
            //    values = db.ReportValues.Where(c => c.CompanyID == ID).ToList().Count();
            //else
            //    values = db.CompanyValues.Where(c=>c.CompanyID == ID).ToList().Count();


            var res = db.CompanyYears.Where(c => c.CompanyID == ID).OrderByDescending(c => c.Year).FirstOrDefault();

            //var r_v = db.ReportValues.Where(c => c.YearID == res.Year).ToList();
            //if (r_v.Count() > 0)
            //    return res;

            db.Dispose();

            db = null;

            return res;
        }

        public static bool DeleteCompanyValuesForCurrentYear(string EMBS, int Year)
        {

            var db = new TargetFinancialDataContext();
            var company = GetCompanyByEMBS(EMBS);

            var CurCompanyYear = db.CompanyYears.Where(c => c.Year == Year && c.CompanyID == company.ID).ToList();

            if (CurCompanyYear.Count > 0)
            {
                var CurCompanyYearID = CurCompanyYear[0].ID;

                var company_values = db.CompanyValues.Where(
                    c => c.CompanyID == company.ID && c.YearID == CurCompanyYearID).ToList();

                db.CompanyValues.DeleteAllOnSubmit(company_values);
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    db = null;
                    return false;
                }
                db.Dispose();

                db = null;
                return true;
            }

            db.Dispose();

            db = null;

            return true;
        }

        public static List<c_CompanyValues> GetCompanyValuesForCurrentYear(string EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();
            var company = GetCompanyByEMBS(EMBS);

            var CurCompanyYear = db.CompanyYears.Where(c => c.Year == Year && c.CompanyID == company.ID).ToList();

            if (CurCompanyYear.Count > 0)
            {
                var CurCompanyYearID = CurCompanyYear[0].ID;

                var company_values = db.CompanyValues.Where(
                    c => c.CompanyID == company.ID && c.YearID == CurCompanyYearID).GroupBy(c => c.ValueID)
                    .Select(c => new c_CompanyValues
                    {
                        ValueID = c.First().ValueID,
                        Value = c.First().Value
                    }).ToList();

                db.Dispose();

                db = null;
                return company_values;
            }

            db.Dispose();

            db = null;

            return null;

        }

        public static List<c_CompanyValues> GetCompanyValuesForLastYear(string EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();

            var company = GetCompanyByEMBS(EMBS);

            var CurCompanyYear = db.CompanyYears.Where(c => c.Year == Year && c.CompanyID == company.ID).ToList();

            if (CurCompanyYear.Count > 0)
            {
                var CurCompanyYearID = CurCompanyYear[0].ID;

                var company_values = db.CompanyValues.Where(
                    c => c.CompanyID == company.ID && c.YearID == CurCompanyYearID).GroupBy(c => c.ValueID)
                    .Select(c => new c_CompanyValues
                    {
                        ValueID = c.First().ValueID,
                        Value = c.First().Value
                    }).ToList();

                db.Dispose();

                db = null;

                return company_values;
            }

            db.Dispose();

            db = null;

            return null;

        }

        public static bool UpdateCompanyReport(Dictionary<string, string> Values)
        {
            var completed = true;
            var db = new TargetFinancialDataContext();

            var CompanyID = 0;
            var YearID = 0;
            var LastYearID = 0;

            var tmpCompany = GetCompanyByEMBSTF(Values["EMBS"]);
            if (tmpCompany != null)
            {
                CompanyID = tmpCompany.ID;

                var tmpYear = db.CompanyYears.Where(c => c.CompanyID == CompanyID && c.Year == Convert.ToInt32(Values["Year"])).FirstOrDefault();
                if (tmpYear != null)
                {
                    YearID = tmpYear.ID;
                }
                else
                {
                    completed = false;

                }

                var tmpLastYear = db.CompanyYears.Where(c => c.CompanyID == CompanyID && c.Year == (Convert.ToInt32(Values["Year"]) - 1)).FirstOrDefault();
                if (tmpLastYear != null)
                {
                    LastYearID = tmpLastYear.ID;
                }
                else
                {
                    var year_obj = new CompanyYear();

                    year_obj.Year = (Convert.ToInt32(Values["Year"]) - 1);
                    year_obj.CompanyID = CompanyID;

                    db.CompanyYears.InsertOnSubmit(year_obj);

                    db.SubmitChanges();

                    LastYearID = year_obj.ID;
                }
            }
            else
            {
                completed = false;
            }

            var report_values_obj = new List<ReportValue>();

            var DB_Values = db.Values.Where(c => c.Type == 2).ToDictionary(c => c.Name, c => c.ID);

            foreach (var item in DB_Values)
            {
                var cur_key = item.Key;
                var cur_key_ind = item.Key + "_Ind";
                var cur_key_procent = item.Key + "_Procent";

                if (Values.ContainsKey(cur_key))
                {
                    var cur_value = Values[cur_key].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");

                    string cur_value_ind = null;
                    string cur_value_procent = null;

                    if (Values.ContainsKey(cur_key_ind))
                    {
                        cur_value_ind = Values[cur_key_ind].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");
                    }

                    if (Values.ContainsKey(cur_key_procent))
                    {
                        cur_value_procent = Values[cur_key_procent].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");
                    }

                    try
                    {
                        report_values_obj.Add(new ReportValue
                        {
                            CompanyID = CompanyID,
                            YearID = YearID,
                            ValueID = item.Value,
                            Value = cur_value,
                            Ind = cur_value_ind,
                            PercentValue = cur_value_procent

                        });
                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                        completed = false;
                    }

                    var cur_key2 = item.Key + "_LastYear";
                    var cur_key_procent2 = item.Key + "_Procent_LastYear";

                    if (Values.ContainsKey(cur_key2))
                    {
                        var cur_value2 = Values[cur_key2].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");

                        string cur_value_ind2 = null;
                        string cur_value_procent2 = null;

                        if (Values.ContainsKey(cur_key_procent2))
                        {
                            cur_value_procent2 = Values[cur_key_procent2].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");
                        }

                        try
                        {
                            report_values_obj.Add(new ReportValue
                            {
                                CompanyID = CompanyID,
                                YearID = LastYearID,
                                ValueID = item.Value,
                                Value = cur_value2,
                                Ind = cur_value_ind2,
                                PercentValue = cur_value_procent2

                            });
                        }
                        catch (Exception ex)
                        {
                            var a = ex.Message;
                            completed = false;
                        }
                    }
                }
            }
            //delete existing data for current company by CompanID, YearID and LastYearID

            db.ReportValues.DeleteAllOnSubmit(db.ReportValues.Where(c => c.CompanyID == CompanyID && (c.YearID == YearID || c.YearID == LastYearID)).ToList());

            // insert new data for YearID and LastYearID
            db.ReportValues.InsertAllOnSubmit(report_values_obj);

            db.SubmitChanges();

            db.Dispose();

            db = null;

            return completed;

        }


        public static void CreateCompanyReport(Dictionary<string, string> Values)
        {
            var db = new TargetFinancialDataContext();

            var company_obj = new Company();

            company_obj.Name = Values["Naziv"];
            company_obj.Mesto = Values["Mesto"];
            company_obj.EMBS = Values["EMBS"];

            var CompanyID = 0;
            var YearID = 0;
            var LastYearID = 0;

            var tmpCompany = GetCompanyByEMBSTF(Values["EMBS"]);
            if (tmpCompany != null)
            {
                CompanyID = tmpCompany.ID;

                var tmpYear = db.CompanyYears.Where(c => c.CompanyID == CompanyID && c.Year == Convert.ToInt32(Values["Year"])).FirstOrDefault();
                if (tmpYear != null)
                {
                    YearID = tmpYear.ID;
                }
                else
                {
                    var year_obj = new CompanyYear();

                    year_obj.Year = Convert.ToInt32(Values["Year"]);
                    year_obj.CompanyID = CompanyID;

                    db.CompanyYears.InsertOnSubmit(year_obj);

                    db.SubmitChanges();

                    YearID = year_obj.ID;

                }

                var tmpLastYear = db.CompanyYears.Where(c => c.CompanyID == CompanyID && c.Year == (Convert.ToInt32(Values["Year"]) - 1)).FirstOrDefault();
                if (tmpLastYear != null)
                {
                    LastYearID = tmpLastYear.ID;
                }
                else
                {
                    var year_obj = new CompanyYear();

                    year_obj.Year = (Convert.ToInt32(Values["Year"]) - 1);
                    year_obj.CompanyID = CompanyID;

                    db.CompanyYears.InsertOnSubmit(year_obj);

                    db.SubmitChanges();

                    LastYearID = year_obj.ID;
                }
            }
            else
            {
                db.Companies.InsertOnSubmit(company_obj);

                db.SubmitChanges();

                CompanyID = company_obj.ID;

                var year_obj = new List<CompanyYear>();

                year_obj.Add(new CompanyYear
                {
                    Year = Convert.ToInt32(Values["Year"]),
                    CompanyID = CompanyID
                });

                year_obj.Add(new CompanyYear
                {
                    Year = (Convert.ToInt32(Values["Year"]) - 1),
                    CompanyID = CompanyID
                });

                db.CompanyYears.InsertAllOnSubmit(year_obj);

                db.SubmitChanges();

                YearID = year_obj[0].ID;
                LastYearID = year_obj[1].ID;
            }

            var report_values_obj = new List<ReportValue>();

            var DB_Values = db.Values.Where(c => c.Type == 2).ToDictionary(c => c.Name, c => c.ID);

            foreach (var item in DB_Values)
            {
                var cur_key = item.Key;
                var cur_key_ind = item.Key + "_Ind";
                var cur_key_procent = item.Key + "_Procent";

                if (Values.ContainsKey(cur_key))
                {
                    var cur_value = Values[cur_key].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");

                    string cur_value_ind = null;
                    string cur_value_procent = null;

                    if (Values.ContainsKey(cur_key_ind))
                    {
                        cur_value_ind = Values[cur_key_ind].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");
                    }

                    if (Values.ContainsKey(cur_key_procent))
                    {
                        cur_value_procent = Values[cur_key_procent].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");
                    }

                    try
                    {
                        report_values_obj.Add(new ReportValue
                        {
                            CompanyID = CompanyID,
                            YearID = YearID,
                            ValueID = item.Value,
                            Value = cur_value,
                            Ind = cur_value_ind,
                            PercentValue = cur_value_procent

                        });
                    }
                    catch (Exception ex)
                    {
                        var a = ex.Message;
                    }

                    var cur_key2 = item.Key + "_LastYear";
                    var cur_key_procent2 = item.Key + "_Procent_LastYear";

                    if (Values.ContainsKey(cur_key2))
                    {
                        var cur_value2 = Values[cur_key2].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");

                        string cur_value_ind2 = null;
                        string cur_value_procent2 = null;

                        if (Values.ContainsKey(cur_key_procent2))
                        {
                            cur_value_procent2 = Values[cur_key_procent2].Replace(" ", String.Empty).Replace(".", String.Empty).Replace(",", ".");
                        }

                        try
                        {
                            report_values_obj.Add(new ReportValue
                            {
                                CompanyID = CompanyID,
                                YearID = LastYearID,
                                ValueID = item.Value,
                                Value = cur_value2,
                                Ind = cur_value_ind2,
                                PercentValue = cur_value_procent2

                            });
                        }
                        catch (Exception ex)
                        {
                            var a = ex.Message;
                        }
                    }
                }
            }

            db.ReportValues.InsertAllOnSubmit(report_values_obj);

            db.SubmitChanges();

            db.Dispose();

            db = null;
        }

        public static Dictionary<string, c_ReportValues> GetShortReportValues(string EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();

            var LastYear = Year - 1;

            var company = db.Companies.Where(c => c.EMBS == EMBS).ToList()[0];

            var YearID = db.CompanyYears.Where(c => c.Year == Year && c.CompanyID == company.ID).Select(c => c.ID).FirstOrDefault();
            var LastYearID = db.CompanyYears.Where(c => c.Year == LastYear && c.CompanyID == company.ID).Select(c => c.ID).FirstOrDefault();


            var res = new Dictionary<string, c_ReportValues>();

            var data = db.ReportValues.Where(c => c.CompanyID == company.ID && (c.YearID == YearID || c.YearID == LastYearID)).Select(c => new c_ReportValues
            {
                ID = c.ID,
                Value = c.Value,
                ValueID = c.ValueID,
                Ind = c.Ind,
                PercentValue = c.PercentValue,
                YearID = c.YearID,
                CompanyID = c.CompanyID,
                ValueName = db.Values.Where(d => d.ID == c.ValueID).Select(d => d.Name).FirstOrDefault()
            }).ToList();

            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (item.YearID == LastYearID)
                    {
                        if (res.ContainsKey(item.ValueName + "_LastYear") == false)
                        {
                            res.Add(item.ValueName + "_LastYear", item);
                        }

                    }
                    else
                    {
                        if (res.ContainsKey(item.ValueName) == false)
                        {
                            res.Add(item.ValueName, item);
                        }

                    }
                }

                db.Dispose();

                db = null;

                return res;
            }

            db.Dispose();

            db = null;

            return null;
        }
    }

    public class KazniISankcii
    {
        public int ID { get; set; }
        public string OdobruvanjeNaPrijava { get; set; }
        public string TipNaPostapka { get; set; }
        public string PovrzanostStatusVoPKD { get; set; }
    }

    public class SolventnostCompany
    {
        public int ID { get; set; }
        public string OdobruvanjeNaPrijava { get; set; }
        public string TipNaPostapka { get; set; }
        public string Dispozitiv { get; set; }
    }
    public class PromeniCompany
    {
        public DateTime OdobruvanjeNaPrijava { get; set; }
        public string Opis { get; set; }
        public string VidNaPromena { get; set; }
    }

    public class c_PrepayPackObj
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Total { get; set; }
        public int Used { get; set; }
        public string Username { get; set; }
        public string EMBS { get; set; }
        public string PackTypeName { get; set; }
        public bool IsPostPaid { get; set; }
    }

    public struct c_UserReportObj
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ReportID { get; set; }
        public DateTime DateCreated { get; set; }
        public int PackID { get; set; }
        public int Downloads { get; set; }
        public string CompanyName { get; set; }
        public string EMBS { get; set; }
        public Guid UID { get; set; }
        public string PackTypeName { get; set; }
        public bool IsPostPaid { get; set; }
    }

    public struct c_ReportValues
    {

        public int ID { get; set; }
        public int ValueID { get; set; }
        public int YearID { get; set; }
        public int CompanyID { get; set; }
        public string Value { get; set; }
        public string Ind { get; set; }
        public string PercentValue { get; set; }
        public string ValueName { get; set; }
    }

    public enum FL_TipPovrzanost
    {
        Sopstvenik = 1,
        Upravitel = 2,
        OvlasteniLicaNaPodruznica = 3,
        OvlastenoLice = 4,
        Likvidator = 6,
        Prokurist = 7,
        ClenNaNadzorenOdbor = 8,
        ClenNaUpravenOdbor = 9,
        IzvrsenClenNaOdbNaDirekt = 10,
        NeizvrClenNaOdbNaDirekt = 11,
        Kontroler = 12,
        SopstvenikUpravitel = 13
    }

    public struct c_CompanyValues
    {
        public int ValueID { get; set; }
        public double Value { get; set; }
    }

    public struct OrganizacionaEdinicaObject
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Podbroj { get; set; }
        public string Tip { get; set; }
        public string Podtip { get; set; }
        public string Adresa { get; set; }
        public string PrioritetnaDejnost { get; set; }
        public string GlavnaPrihodnaSifra { get; set; }
        public string OvlastenoLice { get; set; }
        public string Ovlastuvanja { get; set; }
        public int FK_DejnostSifra { get; set; }
        public int FK_Subjekt { get; set; }
    }

    public struct BankarskaSmetka
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Smetka { get; set; }
    }

    public class OvlastenoLiceObject
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Ovlastuvanja { get; set; }
        public string TipOvlastuvanja1 { get; set; }
        public string TipOvlastuvanja2 { get; set; }
        public string Ogranicuvanja { get; set; }
        public string Povrzanost { get; set; }
        public FL_TipPovrzanost IsOwner { get; set; }
    }

    public struct UserProfile
    {
        public int ID;
        public string Username;
        public string Email;
        public string Password { get; set; }
        public string EMBS { get; set; }
    }

    public struct CompanyDetails
    {
        public int ID;
        public string EMBS;
        public string Naziv;
        public string KratokNaziv { get; set; }
        public string Datum;
    }

    public enum UserReportType
    {
        BonitetenIzvestaj = 1,
        FinansiskiPregled = 2,
        Blokada = 3
    }
}