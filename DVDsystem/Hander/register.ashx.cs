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
    /// register 的摘要说明
    /// </summary>
    public class register : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string  userid=context.Request["userid"].ToString().Replace(" ","");
            string username = context.Request["username"].ToString().Replace(" ", "");
            string sex = context.Request["sex"].ToString().Replace(" ", "");
            string pass = context.Request["pass"].ToString().Replace(" ", "");
            string phone = context.Request["phone"].ToString().Replace(" ", "");
            string checkcode = context.Request["yzheng"].ToString().Replace(" ", "");
            if (!checkcode.Equals(context.Session["Rcheck"]))
            {
                context.Response.Write("checkerro");
            }
            else
            {
                SqlHelper.ExecuteDataTable("insert into User_t(name,password,sex,idcard,phone) value(@name,@password,@sex,@idcard,@phone)", new SqlParameter("@name", username), new SqlParameter("@password", pass), new SqlParameter("@sex", sex), new SqlParameter("@idcard", userid), new SqlParameter("@phone", phone));
                context.Session["C151-7D5B-4231-8CEA-1693"]=username;
                context.Response.Write("complement");
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