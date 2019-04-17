using Bonitet.DAL;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Bonitet.Web
{
    /// <summary>
    /// Summary description for LostPasswordHelper
    /// </summary>
    public class LostPasswordHelper : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var lost_password = context.Request.Form["lost_password"];

            var reset_password = context.Request.Form["reset_password"];

            var activation_code = context.Request.Form["activation_code"];

            var userid = context.Request.Form["userid"];

            if (lost_password != null)
            {
                var email = context.Request.Form["email"];

                var user = DALHelper.GetUserByEmail(email);

                if (user != null)
                {
                    var code = DALHelper.SetUserActivationCode(user.ID);

                    if (code != null)
                    {
                        int defaultPort = context.Request.IsSecureConnection ? 443 : 80;

                        var url = context.Request.Url.Scheme + System.Uri.SchemeDelimiter + context.Request.Url.Host 
                            + (context.Request.Url.Port != defaultPort ? ":" + context.Request.Url.Port : "");

                        string requested = url + "/PasswordReset.aspx?userid=" + user.ID + "&activation_code=" + code;

                        var message = "Почитувани <br/><br/>Ако сте регистрирани на Ебонитети.мк и побаравте промена на лозинка кликнете <a href=\"" + requested + "\">тука</a>.";

                        MailHelper.SendMail(email, "Заборавена лозинка", message, true);

                        context.Response.Write("Mail Sent");
                    }
                    else
                    {
                        context.Response.Write("error");
                    }
                }
                else
                    context.Response.Write("error");
            }
            else if (reset_password != null)
            {
                if (activation_code != null && userid != null)
                {
                    var res = DALHelper.UpdateUserPassword(Convert.ToInt32(userid), new Guid(activation_code));

                    if (res != null)
                    {
                        var message = "Почитувани <br/><br/>Вашата нова лозинка е: " + res.Password + "</a>.";

                        MailHelper.SendMail(res.Email, "Нова лозинка", message, true);

                        context.Response.Write("reset_ok");
                    }
                    else
                        context.Response.Write("reset_expired");
                }
                else
                    context.Response.Write("reset_problem");
            }
            else
                context.Response.Write("error");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}