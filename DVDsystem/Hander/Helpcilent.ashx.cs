using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DVDsystem.Hander
{
    /// <summary>
    /// Helpcilent 的摘要说明
    /// </summary>
    public class Helpcilent : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //-------------------------------------------登录检查-----------------------
            string username = "";
            string username2 = "";
            string loginuser = "";
            string html;
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
            var Data = new { loginuser = loginuser };
            if (context.Request["rentstate"] != null)
                       html = CommonHelper.RenderHtml("../template/Helpcilentrent.html", Data);
                    else if (context.Request["usercilent"] != null)
                             html = CommonHelper.RenderHtml("../template/Helpcilentintro.html", Data);
                        else
                                 html = CommonHelper.RenderHtml("../template/Helpcilent.html", Data);
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