using Dapper;
using ExamOn.Authorize;
using ExamOn.DataLayer;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using ExamOn.Models;
using System.Linq;

namespace ExamOn.Controllers
{
    public class StudDashboardController : Controller
    {

        [AuthorizeAction]
        public ActionResult Go()
        {
            using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString()))
            {
                var tbllogin = mainDB.Query<tbllogin, tblloginType, tbllogin>("select a.Id, EmailId, Active, TenantToken, a.LoginType, b.Type, b.TypeName from tbllogin a inner join tblloginType b on a.logintype = b.id", (login, logintype)=> { login.tblloginType = logintype; return login; }, splitOn: "LoginType");
                if(tbllogin != null && tbllogin.Any())
                {
                    var px = tbllogin.ToList();
                }
            }
            return View("Index");
        }
    }
}