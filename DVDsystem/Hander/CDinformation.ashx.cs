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
    public class CDinformation : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string CDid = context.Request["CDid"].ToString().Replace(" ","");

            if(CDid != null )
                if(!CDid.Equals(""))
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

                    DataTable CD = SqlHelper.ExecuteDataTable("select * from VCD_t as A where A.id=@CDid",new SqlParameter("@CDid",CDid));
                    DataTable CDtype = SqlHelper.ExecuteDataTable("select * from VCD_t");
                    DataTable otherproduct;
                    int[] rannumber;
                    int productnumber = (int)SqlHelper.ExecuteScalar("select count(*) from VCD_t");
                    if (productnumber < 4)
                    {
                        rannumber = getRandomNum(productnumber, 1, productnumber);//随机数取四个数字
                        if (productnumber == 1)
                        {
                            otherproduct = SqlHelper.ExecuteDataTable("select * from(select *,ROW_NUMBER() over( order by id asc) as num from VCD_t where num=@num1"
                             , new SqlParameter("@num1", rannumber[0]));//随机取
                        }
                        else if (productnumber == 2)
                        {
                           otherproduct = SqlHelper.ExecuteDataTable("select * from(select *,ROW_NUMBER() over( order by id asc) as num from VCD_t where num=@num1 or num=@num2 "
                           , new SqlParameter("@num1", rannumber[0]), new SqlParameter("@num2", rannumber[1]));//随机取
                        }
                        else
                        {
                            otherproduct = SqlHelper.ExecuteDataTable("select * from(select *,ROW_NUMBER() over( order by id asc) as num from VCD_t where num=@num1 or num=@num2 or num=@num3"
                           , new SqlParameter("@num1", rannumber[0]), new SqlParameter("@num2", rannumber[1]), new SqlParameter("@num3", rannumber[2]));//随机取
                        }
                     

                    }
                    else
                    {

                        rannumber = getRandomNum(4, 1, productnumber);//随机数取四个数字b
                        otherproduct = SqlHelper.ExecuteDataTable("select * from(select *,ROW_NUMBER() over( order by id asc) as num from VCD_t where num=@num1 or num=@num2 or num=@num3 or num=@num4"
                       , new SqlParameter("@num1", rannumber[0]), new SqlParameter("@num2", rannumber[1]), new SqlParameter("@num3", rannumber[2]), new SqlParameter("@num4", rannumber[3]));//随机取

                    }


                    if (CD.Rows.Count > 0)
                    {
                        var Data = new { CD = CD.Rows[0], CDtype = CDtype.Rows, otherCD = otherproduct, loginuser = loginuser };
                        string html = CommonHelper.RenderHtml("../template/CDinformation.html", Data);
                        context.Response.Write(html);
                    }
                    else
                        context.Response.Write("erro");
                }
          


        }
        //------------------------------随机数函数---------------------------------------------------
        public int[] getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                tmp = ra.Next(minValue, maxValue); //随机取数
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra); //取出值赋到数组中
            }
            return arrNum;
        }


        public int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //利用循环判断是否有重复
                {
                    tmp = ra.Next(minValue, maxValue); //重新随机获取。
                    getNum(arrNum, tmp, minValue, maxValue, ra);//递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
                }
                n++;
            }
            return tmp;
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