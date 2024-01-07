using ExamOn.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer
{
    public static class DBConnection
    {
        public static ApplicationDBContext GetDBContext(string db = null)
        {
            if (!string.IsNullOrEmpty(db))
                db = AuthorizeService.GetUserDBName(HttpContext.Current.User.Identity.Name);
            return new ApplicationDBContext($"Server=AMIT-LAPTOP\\SQLEXPRESS; Database={db};user id=examon;password=Welcome@90510;asynchronous processing=True;trustservercertificate=True; Trusted_Connection = true");
        }
    }
}