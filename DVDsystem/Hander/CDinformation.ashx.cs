using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace DVDsystem.Hander
{
    /// <summary>
    /// CDinformation 的摘要说明
    /// </summary>
    public class CDinformation : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string CDid = context.Request["CDid"].ToString().Replace(" ", "");

            if (CDid != null)
                if (!CDid.Equals(""))
                {

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
                    //-----------------------------------------------------

                    DataTable CD = SqlHelper.ExecuteDataTable("select * from VCD_t as A where A.id=@CDid", new SqlParameter("@CDid", CDid));
                    DataTable CDtype = SqlHelper.ExecuteDataTable("select * from VCD_t");
                    string trm = "";
                    DataTable otherproduct;

                    int productnumber = (int)SqlHelper.ExecuteScalar("select count(*) from Record_t");

                    if (productnumber > 4) productnumber = 4;//大于四个最近借出只拿四个
                      
                    DataTable result = SqlHelper.ExecuteDataTable("select distinct top "+productnumber+" vcdid, rentdate from Record_t order by rentdate DESC");

                        foreach (DataRow temp in result.Rows)
                        {
                            if (temp != null)
                            {
                                string data = temp["vcdid"].ToString().Replace(" ", "");
                                trm = trm + data + ",";
                            }
                        }
                        trm = trm.TrimEnd(',');
                        otherproduct = SqlHelper.ExecuteDataTable("select * from VCD_t where id in (" + trm + ")");//取出对应的vcd信息

                    if (CD.Rows.Count > 0)
                    {
                        var Data = new { CD = CD.Rows[0], CDtype = CDtype.Rows, otherCD = otherproduct.Rows, loginuser = loginuser };
                        string html = CommonHelper.RenderHtml("../template/CDinformation.html", Data);
                        context.Response.Write(html);
                    }
                    else
                        context.Response.Write("erro");
                }
                else
                    context.Response.Write("哎呀！不知道到了什么异世界去了.");



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