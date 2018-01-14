using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.SessionState;

namespace DVDsystem.Hander
{
    /// <summary>
    /// rentstate 的摘要说明
    /// </summary>
    public class rentstate : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Regex check = new Regex(@"^\d*$");
            int pagenumber = 1;
            string search="";

            //-------------------------------------------登录检查-----------------------
            string username = "";
            string username2 = "";
            string loginuser = "";
            HttpCookie user = context.Request.Cookies["C005-7D5B-4231-8CEA-1693"];
            //不知道什么原因，之前取出来的cookie值的@变成了%40
            //思路有四个1.用户登录的时候先在cookie中取出最后一次登录的用户名，如果cookie中不存在那么为a123
            //2.当cookie中不为空，再从seesion中取出相应的密码
            //3.取消记住密码这个选项的时候从session中删除记录
            //4.登录成功之后再session中记录用户名，每次都从此处调取用户名，直到用户退出登录。

            if (context.Session["C151-7D5B-4231-8CEA-1693"] != null)
                if (!context.Session["C151-7D5B-4231-8CEA-1693"].ToString().Equals(""))
                    loginuser = context.Session["C151-7D5B-4231-8CEA-1693"].ToString();

            if (user != null)
            {
                username2 = user.Value.ToString();

                if (context.Session[username2] != null && !context.Session[username2].ToString().Equals(""))
                    username = user.Value.ToString();
            }


            if (context.Request["pagenumber"] != null && check.IsMatch(context.Request["pagenumber"]))//判断传过来的页码是否是符合要求
            {
                pagenumber = int.Parse(context.Request["pagenumber"]);
            }
            //-------------------------------------------------分-------页---------------------------------------

            int totalcount = 1;
            bool intjuge = true;
        
            if (context.Request["Rsearch"] != null)//搜索框不为空
            {
                search = context.Request["Rsearch"];
                try
                {
                    int var1 = Convert.ToInt32(search);
                }
                catch
                {
                    intjuge = false;
                }
                if (intjuge)
                    totalcount = (int)SqlHelper.ExecuteScalar("select count(*) from Record_t where vcdid=@temp",new SqlParameter("@temp",search));
                else
                { 
                    DataTable Uresult=SqlHelper.ExecuteDataTable("select id from User_t where name like '%" + @search + "%'", new SqlParameter("@search", search));
                    string trm = "";
                    foreach(DataRow dt in Uresult.Rows)
                        {
                        if( dt != null)
                        trm = trm + dt["id"].ToString().Replace(" ", "") + ",";
                         }
                    trm=trm.TrimEnd(',');
                    totalcount = (int)SqlHelper.ExecuteScalar("select count(*) from Record_t where userid in ("+trm+")");
                 }
            }
            else 
                totalcount = (int)SqlHelper.ExecuteScalar("select count(*) from Record_t");

            int pagecount = (int)Math.Ceiling(totalcount / 9.0);//页码取整

            object[] pagedata = new object[pagecount];
            

            if (context.Request["Rsearch"] != null)
            {
                for (int i = 0; i < pagecount; i++)//数组下标从0开始
                {
                    pagedata[i] = new { Href = "../Hander/rentstate.ashx?Rsearch=" + search + "&pagenumber=" + (i + 1), Title = i + 1 };//封装页码链接
                }
            }
            else
                for (int i = 0; i < pagecount; i++)//数组下标从0开始
                {
                    pagedata[i] = new { Href = "../Hander/rentstate.ashx?pagenumber=" + (i + 1), Title = i + 1 };//封装页码链接
                }


            var fy = new { pagedata = pagedata, totalcount = totalcount, pagenumber = pagenumber, pagecount = pagecount };//把分页的数据封装存好

            DataTable Rdata = new DataTable();

            if (context.Request["Rsearch"] != null)// 如果是从搜索框查询
            {
                search = context.Request["Rsearch"];
                if (intjuge)
                        Rdata = SqlHelper.ExecuteDataTable("select * from (select A.id,A.userid,A.vcdid,A.rentdate,A.returndate,A.rentprice,A.depositprice,A.state,ROW_NUMBER() over( order by A.id asc) as num from Record_t as A where  A.id=@search)as s where s.num>@start and s.num<@end",
                   new SqlParameter("@search", search), new SqlParameter("@start", (pagenumber - 1) * 9), new SqlParameter("@end", pagenumber * 10));
                else
                    {
                        DataTable Uresult = SqlHelper.ExecuteDataTable("select id from User_t where name like '%" + @search + "%'", new SqlParameter("@search",search));
                        string trm = "";
                        foreach (DataRow dt in Uresult.Rows)
                        {
                            if (dt != null)
                                trm = trm + dt["id"].ToString().Replace(" ", "") + ",";
                        }
                        trm=trm.TrimEnd(',');
                        Rdata = SqlHelper.ExecuteDataTable("select * from (select A.id,A.userid,A.vcdid,A.rentdate,A.returndate,A.rentprice,A.depositprice,A.state,ROW_NUMBER() over( order by A.id asc) as num from Record_t as A where A.userid in (" + trm + "))as s where s.num>@start and s.num<@end",
                         new SqlParameter("@start", (pagenumber - 1) * 9), new SqlParameter("@end", pagenumber * 10));
                    }

            }
            else
            {
                    Rdata = SqlHelper.ExecuteDataTable("select * from (select A.id,A.userid,A.vcdid,A.rentdate,A.returndate,A.rentprice,A.depositprice,A.state,ROW_NUMBER() over( order by A.id asc) as num from Record_t as A )as s where s.num>@start and s.num<@end",
                 new SqlParameter("@start", (pagenumber - 1) * 9), new SqlParameter("@end", pagenumber * 10));//取出需要显示的数据块
            }
            DataTable Username = SqlHelper.ExecuteDataTable("select * from User_t");
            DataTable DVD = SqlHelper.ExecuteDataTable("select * from VCD_t");

            var Data = new { Rdata = Rdata.Rows, Username= Username.Rows , DVD = DVD.Rows, loginuser = loginuser, fy = fy,search=search};
            string html = CommonHelper.RenderHtml("../template/rentstate.html", Data);
            context.Response.Write(html);
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