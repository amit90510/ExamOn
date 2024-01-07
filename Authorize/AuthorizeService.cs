using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.Authorize
{
    public static class AuthorizeService
    {
        public static string GetUserName(string userKey)
        {
            string userName = string.Empty;
            if(!string.IsNullOrEmpty(userKey))
            {
                try
                {
                    userName = userKey.Split('?')[0].ToString();
                }
                catch
                { }
            }
            return userName;
        }

        public static string GetUserRole(string userKey)
        {
            string userRole = string.Empty;
            if (!string.IsNullOrEmpty(userKey))
            {
                try
                {
                    userRole = userKey.Split('?')[1].ToString();
                }
                catch
                { }
            }
            return userRole;
        }

        public static string GetUserDBName(string userKey)
        {
            string userDBName = string.Empty;
            if (!string.IsNullOrEmpty(userKey))
            {
                try
                {
                    userDBName = userKey.Split('?')[2].ToString();
                }
                catch
                { }
            }
            return userDBName;
        }
    }
}