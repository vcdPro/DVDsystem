using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;

namespace DVDsystem.Hander
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string deletesesion = context.Request["deletesesion"];

            string username = context.Request["username"];
            string password = context.Request["password"];
            string checkcode = context.Request["checkcode"];
            string Admin = context.Request["Admin"].ToString().Replace(" ","");

            if (deletesesion == null)
            {
                if (!checkcode.Equals(context.Session["check"]))
                {
                    context.Response.Write("checkcodeerror");
                }
                else
                {
                    DataTable result;
                    if (Admin.Equals("ture"))
                    {
                        
                        result = SqlHelper.ExecuteDataTable("select * from Admin_t where name=@username and password=@password",
                        new SqlParameter("@username", username), new SqlParameter("@password", password));
                    }
                    else
                    {

                        result = SqlHelper.ExecuteDataTable("select * from User_t  where  name=@username and password=@password ",
                           new SqlParameter("@username", username), new SqlParameter("@password", password));
                    }

                    if (result.Rows.Count > 0)
                    {
                        context.Session["C151-7D5B-4231-8CEA-1693"] = username;
                        context.Session[username] = password;
                        context.Response.Write("ok");
                    }

                    else
                    {
                        context.Response.Write("usererror");

                    }
                }
            }
            else
            {              
                context.Session["C151-7D5B-4231-8CEA-1693"] = null;//注销登录
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