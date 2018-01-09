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

            if (deletesesion == null)
            {
                if (!checkcode.Equals(context.Session["check"]))
                {
                    context.Response.Write("checkcodeerror");
                }
                else
                {
                    DataTable result;
                    if (username.Substring(0, 1).Equals("@"))
                    {
                        
                        result = SqlHelper.ExecuteDataTable("select * from administort where aID=@username and apassword=@password",
                           new SqlParameter("@username", username), new SqlParameter("@password", password));
                    }
                    else
                    {

                        result = SqlHelper.ExecuteDataTable("select * from account  where  uID=@username and upassword=@password ",
                           new SqlParameter("@username", username), new SqlParameter("@password", password));
                    }

                    if (result.Rows.Count > 0)
                    {
                        context.Session["username"] = username;
                        context.Session[username] = password;

                        if (!username.Substring(0, 1).Equals("@"))
                        {
                            int carnum = int.Parse(SqlHelper.ExecuteScalar("select ucar from account where uID=@username", new SqlParameter("@username", username)).ToString().Replace(" ", ""));

                            foreach (string item in context.Request.Cookies["ShoppingCart"].Values)
                            {
                                if (item != null)
                                {
                                    char[] sp = { '|' };

                                    string[] w = context.Request.Cookies["ShoppingCart"][item].Split(sp);//利用先前的|分隔的数据

                                    if (SqlHelper.ExecuteDataTable("select * from shoppingcar where @cID=cID and @pID=pID", new SqlParameter("@cID", carnum), new SqlParameter("@pID", w[0])).Rows.Count > 0)//账户的购物车已有该物品
                                    {
                                        int countpro = int.Parse(SqlHelper.ExecuteScalar("select pnum from shoppingcar where @cID=cID and @pID=pID", new SqlParameter("@cID", carnum), new SqlParameter("@pID", w[0])).ToString().Replace(" ", "")) + int.Parse(w[1]);
                                        SqlHelper.ExecuteDataTable("update shoppingcar set pnum=@countpro where @cID=cID and @pID=pID", new SqlParameter("@cID", carnum), new SqlParameter("@pID", w[0]), new SqlParameter("countpro", countpro));
                                    }
                                    else
                                    {
                                        int pprice = int.Parse(SqlHelper.ExecuteScalar("select pprice from products where pID =" + w[0]).ToString().Replace(" ", ""));
                                        SqlHelper.ExecuteDataTable("insert into shoppingcar(cID,pID,pnum,pprice) values(@cID," + w[0] + "," + w[1] + ",@pprice)", new SqlParameter("@cID", carnum), new SqlParameter("@pprice", pprice));
                                    }
                                }
                            }
                            context.Response.Write("ok");
                        }
                        else
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