using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bonitet.Web.Classes;

namespace Bonitet.Web.MasterPages
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;


            if (!userobj.isAuthenticated && Request.Url.AbsolutePath.ToLower().Contains("authenticated"))
            {
                Response.Redirect("/Default.aspx?path="+HttpUtility.HtmlEncode(Request.RawUrl));
            }


            li_companies.Visible = userobj.isAuthenticated;
            li_profile.Visible = userobj.isAuthenticated;
            li_logout.Visible = userobj.isAuthenticated;
            li_home.Visible = !userobj.isAuthenticated;

            SetNavActivePage();

        }

        public void SetNavActivePage() 
        {
            var curUrl = Request.Url.AbsolutePath.ToLower();
            var userobj = UserHelper.instance;

            if (curUrl.ToLower().Equals(a_home.HRef.ToLower()) || curUrl.Equals("/"))
            {
                if (userobj.isAuthenticated)
                {
                    if (userobj.Type == 1)
                    {
                        li_profile.Attributes["class"] = "active";
                        li_home.Attributes["class"] = "";

                        Response.Redirect("/Authenticated/UserProfile.aspx");
                    }
                    else if (userobj.Type == 2)
                    {
                        Response.Redirect("/Admin/UserListing.aspx");
                    }
                }
                else
                {
                    li_home.Attributes["class"] = "active";
                    li_profile.Attributes["class"] = "";
                }
                li_about.Attributes["class"] = "";
                li_contact.Attributes["class"] = "";
                li_companies.Attributes["class"] = "";
                li_cenovnik.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_profile.HRef.ToLower()))
            {
                li_profile.Attributes["class"] = "active";
                li_home.Attributes["class"] = "";
                li_about.Attributes["class"] = "";
                li_contact.Attributes["class"] = "";
                li_companies.Attributes["class"] = "";
                li_cenovnik.Attributes["class"] = "";
                
            }
            else if (curUrl.ToLower().Equals(a_about.HRef.ToLower()))
            {
                li_home.Attributes["class"] = "";
                li_about.Attributes["class"] = "active";
                li_contact.Attributes["class"] = "";
                li_companies.Attributes["class"] = "";
                li_cenovnik.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_contact.HRef.ToLower()))
            {
                li_home.Attributes["class"] = "";
                li_about.Attributes["class"] = "";
                li_contact.Attributes["class"] = "active";
                li_companies.Attributes["class"] = "";
                li_cenovnik.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_companies.HRef.ToLower()))
            {
                li_home.Attributes["class"] = "";
                li_about.Attributes["class"] = "";
                li_contact.Attributes["class"] = "";
                li_companies.Attributes["class"] = "active";
                li_cenovnik.Attributes["class"] = "";
            }
            else if (curUrl.ToLower().Equals(a_cenovnik.HRef.ToLower()))
            {
                li_home.Attributes["class"] = "";
                li_about.Attributes["class"] = "";
                li_contact.Attributes["class"] = "";
                li_companies.Attributes["class"] = "";
                li_cenovnik.Attributes["class"] = "active";
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            var userobj = UserHelper.instance;

            userobj.Logout();

            Response.Redirect("/Default.aspx");
        }
    }
}