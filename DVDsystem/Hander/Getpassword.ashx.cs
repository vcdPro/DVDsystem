using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DVDsystem.Hander
{
    /// <summary>
    /// Getpassword 的摘要说明
    /// </summary>
    public class Getpassword : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = context.Request["textusername"].ToString().Replace(" ","");
            string password = context.Session[username].ToString().Replace(" ", "");


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