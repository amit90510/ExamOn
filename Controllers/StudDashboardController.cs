using Dapper;
using ExamOn.Authorize;
using ExamOn.DataLayer;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using ExamOn.Models;
using System.Linq;
using ExamOn.Utility;
using ExamOn.ServiceLayer;
using ExamOn.SignalRPush;
using System.Threading.Tasks;

namespace ExamOn.Controllers
{
    public class StudDashboardController : Controller
    {
        [AuthorizeAction]
        public ActionResult Go()
        {
            //using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString()))
            //{
            //    var tbllogin = mainDB.Query<tbllogin, tblloginType, tbllogin>("select a.Id, EmailId, Active, TenantToken, a.LoginType, b.Type, b.TypeName from tbllogin a inner join tblloginType b on a.logintype = b.id", (login, logintype) => { login.tblloginType = logintype; return login; }, splitOn: "LoginType");
            //    if (tbllogin != null && tbllogin.Any())
            //    {
            //        var px = tbllogin.ToList();
            //    }
            //}
            return View("Index");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetLoginHistory ()
        {
            HubContext.Notify(false, "", $"Please wait, Preparing your dashboard<br/> कृपया प्रतीक्षा करें, आपका डैशबोर्ड तैयार किया जा रहा है।", true, false, false, ViewBag.srKey);
            JsonData jsonData = new JsonData();
            var loginHisory = DapperService.GetDapperData<tblLoginHistory>("select top 5 ip, browser, CONVERT(varchar,LoginDate,100) as loginDate from TblloginHistory where userName = @userName order by LoginDate desc", new { username = ViewBag.UserName });
            if (loginHisory != null && loginHisory.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = loginHisory.ToList(); 
            }
            HubContext.Notify(false, "", $"Please wait, Preparing your dashboard<br/> कृपया प्रतीक्षा करें, आपका डैशबोर्ड तैयार किया जा रहा है।", true, false, true, ViewBag.srKey);
            return Json( jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAssocaitionHistory()
        {
            HubContext.Notify(false, "", $"Please wait, Preparing your dashboard<br/> कृपया प्रतीक्षा करें, आपका डैशबोर्ड तैयार किया जा रहा है।", true, false, false, ViewBag.srKey);
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("select * from tblTenant where Id = @id", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if(tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData.ToList();
            }
            HubContext.Notify(false, "", $"Please wait, Preparing your dashboard<br/> कृपया प्रतीक्षा करें, आपका डैशबोर्ड तैयार किया जा रहा है।", true, false, true, ViewBag.srKey);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}