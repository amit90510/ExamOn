using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.Utility
{
    public static class LoginStatics
    {
        public static string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        public static string GetBrowser()
        {
            return HttpContext.Current.Request.Browser.Browser + (HttpContext.Current.Request.Browser.IsMobileDevice ? "- Mobile" : "- System/Laptop");
        }
    }
}