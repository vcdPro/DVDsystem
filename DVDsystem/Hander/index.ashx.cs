using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;
using System.Text;

namespace DVDsystem.Hander
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler ,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string search = context.Request["keyword"];

            HttpCookie user;
            string username = "";
            string username2 = "";

            if (context.Request["username"] != null)//第一次加载请求必定为空.
            {
                if (context.Session[context.Request["username"]] != null)//已经
                    username = context.Request["username"];
            }
            else
            {
                user = context.Request.Cookies["C005-7D5B-4231-8CEA-1693"];//不知道什么原因，之前取出来的cookie值的@变成了%40
                if (user != null)
                {
                    username2 = user.Value.ToString();

                    if (context.Session[username2] != null && !context.Session[username2].ToString().Equals(""))
                        username = user.Value.ToString();

                }
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