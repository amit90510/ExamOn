using ExamOn.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ExamOn.DataLayer
{
    public static class DBConnection
    {
        public static ApplicationDBContext GetDBContext(string db = null)
        {
            return new ApplicationDBContext(GetConnectionString(db));
        }

        public static string GetConnectionString(string db = null)
        {
            if (string.IsNullOrEmpty(db))
                db = AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name);

            var dbSever = WebConfigurationManager.AppSettings["DBServer"];
            if (!string.IsNullOrEmpty(dbSever))
            {
                return ($"Server={dbSever}; Database={db};user id=examon;password=Welcome@90510;asynchronous processing=True;trustservercertificate=True; Trusted_Connection = false");
            }

            return ($"Server=AMIT-LAPTOP\\SQLEXPRESS; Database={db};user id=examon;password=Welcome@90510;asynchronous processing=True;trustservercertificate=True; Trusted_Connection = false");
        }       
    }
}