using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.Authorize
{
    public static class AuthorizeService
    {
        //format UserId?DBTOKEN?DBNAME?UserName

        public static string SetIdentityCookieValue(long userId, string DBToken, string dbName, string userName)
        {
            return $"{userId}?{DBToken}?{dbName}?{userName}";
        }
        public static string GetUserId(string userKey)
        {
            string userId = string.Empty;
            if(!string.IsNullOrEmpty(userKey))
            {
                try
                {
                    userId = userKey.Split('?')[0].ToString();
                }
                catch
                { }
            }
            return userId;
        }

        public static string GetDBToken(string userKey)
        {
            string userDBToken = string.Empty;
            if (!string.IsNullOrEmpty(userKey))
            {
                try
                {
                    userDBToken = userKey.Split('?')[1].ToString();
                }
                catch
                { }
            }
            return userDBToken;
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

        public static string GetUserName(string userKey)
        {
            string getUserName = string.Empty;
            if (!string.IsNullOrEmpty(userKey))
            {
                try
                {
                    getUserName = userKey.Split('?')[3].ToString();
                }
                catch
                { }
            }
            return getUserName;
        }
    }
}