using Bonitet.DAL;
using Bonitet.Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Bonitet.Web.Admin
{
    /// <summary>
    /// Summary description for PackHelper
    /// </summary>
    public class PackHelper : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var userobj = UserHelper.instance;

            if (userobj.isAuthenticated)
            {
                if (userobj.Type == 2)
                {
                    var stop_pack = context.Request.Form["stop_pack"];
                    var start_pack = context.Request.Form["start_pack"];

                    context.Response.ContentType = "text/plain";

                    if (stop_pack != null)
                    {
                        var packid = context.Request.Form["packid"];

                        DALHelper.DisablePack(Convert.ToInt32(packid));

                        context.Response.Write("1");
                    }
                    else if (start_pack != null)
                    {
                        var packid = context.Request.Form["packid"];

                        DALHelper.ActivatePack(Convert.ToInt32(packid));

                        context.Response.Write("1");
                    }
                    else
                        context.Response.Write("0");
                }
                else
                    context.Response.Write("0");
            }
            else
                context.Response.Write("0");
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