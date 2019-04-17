using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bonitet.DAL;

namespace Bonitet.Web.Classes
{
    public class UserHelper
    {
        public bool isAuthenticated { get; set; }

        public int Type { get; set; }

        public int UserID { get; set; }

        public UserHelper()
        {
            isAuthenticated = false;
            Type = 0;
            UserID = -1;
        }

        public int Login(string username, string password, int UserType)
        {
            var db = new TargetFinancialDataContext();
            if (UserType == 1)
            {
                var user = db.Users.Where(c => c.Username.Equals(username) && c.Password.Equals(password)).ToList();


                db.Dispose();
                db = null;

                if (user.Count > 0)
                {
                    if (user[0].IsActive == true)
                    {
                        isAuthenticated = true;
                        UserID = user[0].ID;
                        Type = UserType;
                        return 1;
                    }
                    else
                        return 2;
                }
                return 0;
            }
            else if (UserType == 2)
            {
                var user = db.AdminUsers.Where(c => c.Username.Equals(username) && c.Password.Equals(password)).ToList();

                db.Dispose();
                db = null;

                if (user.Count > 0)
                {
                    isAuthenticated = true;
                    UserID = user[0].ID;
                    Type = UserType;
                    return 1;
                }
                return 0;
            }

            return 0;
        }

        public void Logout()
        {
            HttpContext.Current.Session.RemoveAll();
        }


        public static UserHelper instance
        {
            get
            {
                if (HttpContext.Current.Session["userobj"] == null)
                {
                    HttpContext.Current.Session.Add("userobj", new UserHelper());
                }

                return (UserHelper)HttpContext.Current.Session["userobj"];
            }
        }
    }
}