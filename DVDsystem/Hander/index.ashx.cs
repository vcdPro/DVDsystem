using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;
using System.Text;
using System.Security.Cryptography;

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

            //--------------------------------------------------------登录检查----------------------------------------------

           
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

                
            }
        //----------------------------------------------------------------------
                if()
      
    //private string Md5jiami (string encryptString)//md5加密是不可逆的过程,只需将每次用户输入的密码进行比对一次即可,可以加盐值进行加密

    //{

    //    byte[] result = Encoding.Default.GetBytes(encryptString);

    //    MD5 md5 = new MD5CryptoServiceProvider();

    //    byte[] output = md5.ComputeHash(result);

    //    string encryptResult = BitConverter.ToString(output).Replace("-", "");

    //    return encryptResult;

    //}

    public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}