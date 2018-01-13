using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DVDsystem.Hander
{
    /// <summary>
    /// rememberme 的摘要说明
    /// </summary>
    public class rememberme : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            if (context.Request["remember"] != null)
            {
                string username = context.Request["username"].ToString().Replace(" ", "");
                string password = context.Request["password"].ToString().Replace(" ", "");
                context.Session[username] = password;
                context.Response.Write("ok");
            }
            else if (context.Request["Search"] != null)
            {
                string user = context.Request["username"].ToString().Replace(" ", "");
                string password = "";
                HttpCookie pass = context.Request.Cookies["user"];
                 if (pass != null)
                    password = pass.Value.ToString().Replace(" ", "");
                 if (context.Session[user] != null)
                    password = context.Session[user].ToString().Replace(" ","");
                context.Response.Write(password);

            }
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