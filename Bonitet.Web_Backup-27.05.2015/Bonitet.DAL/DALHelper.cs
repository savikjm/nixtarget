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

        public static int GetTotalReportRequests(int filter)
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

        public static List<ReportRequest> GetRequestsByFilter(List<ReportRequest> requests, int filter)
        {
            var newList = new List<ReportRequest>();
            foreach (var item in requests)
            {
                var comp = GetCompanyByID(item.CompanyID);

                var check = CheckIfCompanyHasDataForReport(comp[0].EMBS);

                if (filter == 2 && check == false)
                {
                    newList.Add(item);
                }

                if (filter == 3 && check && item.ReportType == 2)
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, comp[0].EMBS, item.ReportType, item.UserID);
                    if (user_report_check != 1)
                        newList.Add(item);
                }
                if (filter == 4 && check && item.ReportType == 2)
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, comp[0].EMBS, item.ReportType, item.UserID);
                    if (user_report_check == 1)
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
                item.EMBS = comp[0].EMBS;

                if (item.ReportType == 1)
                    item.ReportTypeString = "Бонитетен извештај";
                else if (item.ReportType == 2)
                    item.ReportTypeString = "Финансиски преглед";


                var check = CheckIfCompanyHasDataForReport(comp[0].EMBS);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;
                }
                else
                {
                    if (item.ReportType == 2)
                    {
                        var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, comp[0].EMBS, item.ReportType, item.UserID);
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
                    }
                }
            }

            return tmpList.Skip(skip).Take(pageSize).ToList();
        }
        public static List<ReportRequest> GetRequestsByPage(int skip, int pageSize, int filter)
        {
            DataLoadOptions LoadOptions = new DataLoadOptions();

            LoadOptions.LoadWith<User>(c => c.Username);
            LoadOptions.LoadWith<ReportRequest>(c => c.EMBS);

            var db = new TargetFinancialDataContext();

            db.LoadOptions = LoadOptions;

            var tmpRes = db.ReportRequests.GroupBy(c => new { c.CompanyID, c.UserID, c.ReportType, c.Year }).Select(c => c.First()).OrderByDescending(c => c.CreatedOn).ToList();

            var res = new List<ReportRequest>();

            foreach (var item in tmpRes)
            {
                var tmpComp = GetCompanyByID(item.CompanyID);
                var comp = tmpComp[0];
                //var comp = GetCompanyByEMBS_BM(item.EMBS);

                item.CompanyName = comp.Naziv;
                item.EMBS = comp.EMBS;

                if (item.ReportType == 1)
                    item.ReportTypeString = "Бонитетен извештај";
                else if (item.ReportType == 2)
                    item.ReportTypeString = "Финансиски преглед";


                var check = CheckIfCompanyHasDataForReport(comp.EMBS);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;

                    if (filter == 2)
                        res.Add(item);
                }
                else
                {
                    if (item.ReportType == 2)
                    {
                        var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, comp.EMBS, item.ReportType, item.UserID);
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
                    }
                }
            }
            if (filter == 1)
                res.AddRange(tmpRes);

            return res.Skip(skip).Take(pageSize).ToList();
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
                var check = CheckIfCompanyHasDataForReport(comp.EMBS);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;
                }
                else
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, comp.EMBS, item.ReportType, item.UserID);
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
                var comp = GetCompanyByID(item.CompanyID);

                var check = CheckIfCompanyHasDataForReport(comp[0].EMBS);
                if (check == false)
                {
                    item.StatusText = "Pending Data";
                    item.SendMail = false;
                }
                else
                {
                    var user_report_check = CheckReportByUserCompanyYearAndReport(item.Year, comp[0].EMBS, item.ReportType, item.UserID);
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
                }

                item.CompanyName = comp[0].Naziv;
                item.EMBS = comp[0].EMBS;
            }

            return res;
        }

        public static void CreateReportRequest(int UserID, int ReportType, string EMBS, int Year)
        {
            var db = new TargetFinancialDataContext();

            var comp = GetCompanyByEMBS_BM(EMBS);

            if (comp != null)
            {
                var data = new ReportRequest();

                data.CreatedOn = DateTime.Now;
                data.UserID = UserID;
                data.ReportType = ReportType;
                data.CompanyID = comp.PK_Subjekt;
                data.Year = Year;

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


        public static List<UserProfile> GetUserByID(int id)
        {
            TargetFinancialDataContext db = new TargetFinancialDataContext();

            var user = db.Users.Where(c => c.ID == id).Select(c => new UserProfile()
            {
                ID = c.ID,
                Username = c.Username,
                Email = c.Email,
                Password = c.Password,
                EMBS = c.EMBS
            }).ToList();

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

        public static List<c_PrepayPackObj> GetPrepayPackByUsername(int skip, int pageSize, string name)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.User.Username.Contains(name)).Skip(skip).Take(pageSize).Select(c => new c_PrepayPackObj
            {
                ID = c.ID,
                UserID = c.UserID,
                Total = c.Pack,
                Used = c.Used,
                EMBS = db.Users.Where(d => d.ID == c.UserID).Select(d => d.EMBS).FirstOrDefault(),
                Username = db.Users.Where(d => d.ID == c.UserID).Select(d => d.Username).FirstOrDefault()
            }).ToList();

            db.Dispose();
            db = null;

            return res;

        }

        public static List<c_PrepayPackObj> GetPrepayPackByUserEMBS(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.User.EMBS.Equals(EMBS)).Select(c => new c_PrepayPackObj
            {
                ID = c.ID,
                UserID = c.UserID,
                Total = c.Pack,
                Used = c.Used,
                EMBS = db.Users.Where(d => d.ID == c.UserID).Select(d => d.EMBS).FirstOrDefault(),
                Username = db.Users.Where(d => d.ID == c.UserID).Select(d => d.Username).FirstOrDefault()
            }).ToList();

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
            var db = new TargetFinancialDataContext();

            var total = 0;
            if (name != null)
                total = db.PrepayPacks.Where(c => c.User.Username.ToLower().Contains(name.ToLower())).Count();
            else
                total = db.PrepayPacks.Count();

            db.Dispose();
            db = null;
            return total;
        }


        public static List<c_PrepayPackObj> GetPrepayPacksByPage(int skip, int pageSize)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Skip(skip).Take(pageSize).Select(c => new c_PrepayPackObj
            {
                ID = c.ID,
                UserID = c.UserID,
                Total = c.Pack,
                Used = c.Used,
                EMBS = db.Users.Where(d => d.ID == c.UserID).Select(d => d.EMBS).FirstOrDefault(),
                Username = db.Users.Where(d => d.ID == c.UserID).Select(d => d.Username).FirstOrDefault()
            }).ToList();

            db.Dispose();
            db = null;

            return res;
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
            if (ReportType == 1){
                string test = "1";
            }
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

        public static List<c_UserReportObj> GetUserReportsByPage(int skip, int pageSize, int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.UserReports.Where(c => c.UserID == UserID).OrderByDescending(c => c.DateCreated).Skip(skip).Take(pageSize).Select(c => new c_UserReportObj
            {
                ID = c.ID,
                EMBS = db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.EMBS).FirstOrDefault(),
                PackID = (int)c.PackID,
                ReportID = c.ReportID,
                UserID = c.UserID,
                Downloads = c.Downloads,
                DateCreated = c.DateCreated,
                CompanyName = db.Companies.Where(d => db.Reports.Where(f => f.ID == c.ReportID).Any(f => f.CompanyID == d.ID)).Select(d => d.Name).FirstOrDefault(),
                UID = (Guid)db.Reports.Where(d => d.ID == c.ReportID).Select(d => d.UID).FirstOrDefault(),
                PackTypeName = c.PrepayPack.PackTypeName
            }).ToList();

            db.Dispose();
            db = null;

            return res;
        }

        public static List<PrepayPack> GetUserPrepayPacksByPage(int skip, int pageSize, int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.UserID == UserID).Skip(skip).Take(pageSize).ToList();

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
                PackTypeName = c.PrepayPack.PackTypeName
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

        public static bool UpdatePrepayPack(int PackID, int UserID, DateTime DateStart, DateTime DateEnd, int Pack, string Comment)
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

        public static bool CreatePrepayPack(int UserID, DateTime DateStart, DateTime DateEnd, int Pack, string Comment)
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
                Active = true
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

        public static bool CheckIfCompanyHasDataForReport(string EMBS)
        {
            var db = new TargetFinancialDataContext();

            var company = db.Companies.Where(c => c.EMBS == EMBS).ToList();


            if (company.Count() > 0)
            {
                var compYears = GetLastYearByCompanyID(company[0].ID);
                if (compYears.Year < 2014)
                    return false;

                var res = db.ReportValues.Where(c => c.CompanyID == company[0].ID).ToList();

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

        public static PrepayPack CheckPrepayLicence(int UserID)
        {
            var db = new TargetFinancialDataContext();

            var date = DateTime.Now.Date;

            var res = db.PrepayPacks.Where(c => c.UserID == UserID && c.Active == true && c.DateStart <= date && 
                c.DateEnd.Date >= date && c.Pack > c.Used).FirstOrDefault();

            db.Dispose();

            db = null;

            if (res != null)
                return res;
            return null;
        }

        public static void UpdateUserPrepay(int PrepayID, int UserID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.PrepayPacks.Where(c => c.ID == PrepayID && c.UserID == UserID).ToList();


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
                    db.CompanyYears.InsertOnSubmit(new CompanyYear{
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
                    var report = db.ReportValues.Where(c => c.CompanyID == res[0].ID).ToList();

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

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
            else
            {
                var r = db1.Subjekts.Where(c => c.EMBS == EMBS).ToList();

                if (r.Count > 0)
                {
                    res.Add(new Company
                    {
                        ID = r[0].PK_Subjekt,
                        Name = r[0].CelosenNazivNaSubjektot,
                        Mesto = r[0].Sediste,
                        EMBS = r[0].EMBS
                    });
                }

                db1.Dispose();

                db1 = null;

                return res[0];
            }
        }

        public static List<OrganizacionaEdinicaObject> GetOrganizacionaEdinicaByID(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var oe = db.OrganizacionaEdinicas.Where(c => c.FK_Subjekt == ID).ToList();

            var ds = db.DejnostSifras.Where(c => db.OrganizacionaEdinicas.Where(d => d.FK_Subjekt == ID).Any(d => d.FK_DejnostSifra == c.PK_DejnostSifra)).ToList();

            var organizacioni_edinici = new List<OrganizacionaEdinicaObject>();

            foreach (var item in oe)
            {
                organizacioni_edinici.Add(new OrganizacionaEdinicaObject
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
                    GlavnaPrihodnaSifra = ds.Where(c => c.PK_DejnostSifra == item.FK_DejnostSifra).Select(c => c.GlavnaPrihodnaShifra).FirstOrDefault()

                });
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

        public static Dejnost GetDejnostForCompany(int ID)
        {
            var db = new Bonitet.DAL.BiznisMreza.DALDataContext();

            var res = db.Dejnosts.Where(c => c.FK_Subjekt == ID).OrderByDescending(c => c.Datum).ToList();

            db.Dispose();

            db = null;

            if (res.Count > 0)
                return res[0];
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

            var povrzanost = db.Povrzanosts.Where(c => c.FK_Subjekt == ID).ToList();

            List<OvlastenoLiceObject> list = new List<OvlastenoLiceObject>();

            foreach (var item in povrzanost)
            {
                var tmp = db.FizickoLices.Where(c => c.PK_FizickoLice == item.FK_FizickoLice).ToList()[0];

                var tmp1 = db.TipPovrzanosts.Where(c => c.PK_TipPovrzanost == item.FK_TipPovrzanost).ToList()[0];


                list.Add(new OvlastenoLiceObject
                {
                    ID = tmp.PK_FizickoLice,
                    Ime = tmp.Ime,
                    Povrzanost = tmp1.Povrzanost,
                    Ovlastuvanja = item.Ovlastuvanja,
                    Ogranicuvanja = item.Ogranicuvanja,
                    TipOvlastuvanja1 = item.TipNaOvlastuvanje1,
                    TipOvlastuvanja2 = item.TipNaOvlastuvanje2,
                    IsOwner = (FL_TipPovrzanost)Enum.ToObject(typeof(FL_TipPovrzanost), tmp1.PK_TipPovrzanost)
                });

            }
            db.Dispose();

            db = null;

            if (list.Count > 0)
                return list;
            return null;

        }

        public static CompanyYear GetLastYearByCompanyID(int ID)
        {
            var db = new TargetFinancialDataContext();

            var res = db.CompanyYears.Where(c => c.CompanyID == ID).OrderByDescending(c => c.Year).FirstOrDefault();

            return res;
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

                var tmpYear = db.CompanyYears.Where(c => c.CompanyID == CompanyID && c.Year == Convert.ToInt32(Values["Year"])).ToList();
                if (tmpYear.Count() > 0)
                {
                    YearID = tmpYear[0].ID;
                }

                var tmpLastYear = db.CompanyYears.Where(c => c.CompanyID == CompanyID && c.Year == (Convert.ToInt32(Values["Year"]) - 1)).ToList();
                if (tmpLastYear.Count() > 0)
                {
                    LastYearID = tmpLastYear[0].ID;
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

            var DB_Values = db.Values.Where(c => c.Type == 1).ToDictionary(c => c.Name, c => c.ID);

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

            var data = db.ReportValues.Where(c => c.CompanyID == company.ID && (c.YearID == YearID || c.YearID == LastYearID)).Select(c=> new c_ReportValues
            {
                ID = c.ID,
                Value = c.Value,
                ValueID = c.ValueID,
                Ind = c.Ind,
                PercentValue = c.PercentValue,
                YearID = c.YearID,
                CompanyID = c.CompanyID,
                ValueName = db.Values.Where(d=>d.ID == c.ValueID).Select(d=>d.Name).FirstOrDefault()
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

    public struct c_PrepayPackObj
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Total { get; set; }
        public int Used { get; set; }
        public string Username { get; set; }
        public string EMBS { get; set; }
        public string PackTypeName { get; set; }
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
        Kontroler = 12
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
        public int FK_DejnostSifra { get; set; }
        public int FK_Subjekt { get; set; }
    }

    public struct OvlastenoLiceObject
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
}