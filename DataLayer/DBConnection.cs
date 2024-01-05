using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer
{
    public static class DBConnection
    {
        public static ApplicationDBContext GetDBContext(string db)
        {            
            return new ApplicationDBContext($"Server=AMIT-LAPTOP\\SQLEXPRESS; Database={db};user id=examon;password=Welcome@90510;asynchronous processing=True;trustservercertificate=True; Trusted_Connection = true");
        }
    }
}