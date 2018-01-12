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
            context.Response.ContentType = "text/html";

            Regex check = new Regex(@"^\d*$");
            int pagenumber=1;
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
            if (context.Request["keyword"] != null)//搜索框不为空
            {
                string search = context.Request["keyword"];
                totalcount = (int)SqlHelper.ExecuteScalar("select count(*) from VCD_t where name like '%" + @search + "%' or id=@serch " , new SqlParameter("@search", search));

            }
            else if (context.Request["categori"] != null)//是否有类别选项     
                {
                string categori = context.Request["categori"];
                totalcount = (int)SqlHelper.ExecuteScalar("select count(*) from VCD_t where typid=@categori",new SqlParameter("@categori",categori));//商品总数
                }

            else if (context.Request["categori"] == null)

                totalcount = (int)SqlHelper.ExecuteScalar("select count(*) from VCD_t ");//商品总数


            int pagecount = (int)Math.Ceiling(totalcount / 9.0);//页码取整

            object[] pagedata = new object[pagecount];

            for (int i = 0; i < pagecount; i++)//数组下标从0开始
            {
                pagedata[i] = new { Href = "../template/rentstate.ashx?pagenumber=" + (i + 1), Title = i + 1 };//封装页码链接
            }

            var fy = new { pagedata = pagedata, totalcount = totalcount, pagenumber = pagenumber, pagecount = pagecount };//把分页的数据封装存好
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