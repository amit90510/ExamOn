using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExamOn.Controllers
{
    public class InstructorTeacherDashboardController : Controller
    {
        [AuthorizeAction]
        public ActionResult Go()
        {
            return View("Index");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAssocaitionHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("select TenantName, TenantMobile, TenantEmail, SubscriptionEndDate from tblTenant where Id = @id", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}