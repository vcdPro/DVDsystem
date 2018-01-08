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
            string username = "";
            string username2 = "";
            
            HttpCookie user = context.Request.Cookies["C005-7D5B-4231-8CEA-1693"];//不知道什么原因，之前取出来的cookie值的@变成了%40
                if (user != null)
                {
                    username2 = user.Value.ToString();

                    if (context.Session[username2] != null && !context.Session[username2].ToString().Equals(""))
                        username = user.Value.ToString();
                }
                
                else if()
            }
      
    private string Md5jiami (string encryptString)

    {

        byte[] result = Encoding.Default.GetBytes(encryptString);

        MD5 md5 = new MD5CryptoServiceProvider();

        byte[] output = md5.ComputeHash(result);

        string encryptResult = BitConverter.ToString(output).Replace("-", "");

        return encryptResult;

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