using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.DataLayer.GetDataModel;
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
    public class TenantAdminDashboardController : Controller
    {
        [AuthorizeAction]
        public ActionResult Go()
        {
            return View("Index");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetLicenceExpire()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("Select top 1 SubscriptionEndDate, DATEDIFF(second,GETDATE(), SubscriptionEndDate) / (60 * 60 * 24) AS SubscriptionEndMessage, RechargeAmount from tbltenant", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                if ((tenantData.FirstOrDefault().SubscriptionEndMessage.Contains("-") || tenantData.FirstOrDefault().SubscriptionEndMessage == "0"))
                {
                    jsonData.Error = $"{tenantData.FirstOrDefault().SubscriptionEndMessage} <br/> (आपका लाइसेंस समाप्त हो गया है, कृपया वेब प्रशासक/ग्राहक सेवा से संपर्क करें।)";
                }
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetStudentHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<GetStudentStaticHistory>("SELECT SUM(CASE WHEN tl.Active = 0 THEN 1 ELSE 0 END) AS NonActive, SUM(CASE WHEN tl.Active = 1 THEN 1 ELSE 0 END) AS Active, SUM(CASE WHEN tl.BlockLogin = 1 THEN 1 ELSE 0 END) AS Blocked FROM tbllogin tl INNER JOIN tblloginType ty ON tl.LoginType = ty.id WHERE ty.TypeName = 'Student'", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetStudentEnrollmentHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<GetStudentEnrollmentStaticsHistory>("SELECT Count(id) AS Total, SUM(CASE WHEN status = 'Enrolled' THEN 1 ELSE 0 END) AS Enrolled, SUM(CASE WHEN status = 'Rejected' THEN 1 ELSE 0 END) AS NotEnrolled FROM tblStudentEnrollmentSignUp", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTeacherHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<GetStudentStaticHistory>("SELECT SUM(CASE WHEN tl.Active = 0 THEN 1 ELSE 0 END) AS NonActive, SUM(CASE WHEN tl.Active = 1 THEN 1 ELSE 0 END) AS Active, SUM(CASE WHEN tl.BlockLogin = 1 THEN 1 ELSE 0 END) AS Blocked FROM tbllogin tl INNER JOIN tblloginType ty ON tl.LoginType = ty.id WHERE ty.TypeName = 'Teacher'", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetClassBatchSectionHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<UserShiftAssociation>("SELECT (SELECT COUNT(id) FROM tblClass where active = 1) AS ClassCount, (SELECT COUNT(id) FROM tblBatch where active = 1) AS BatchCount, (SELECT COUNT(id) FROM tblshift where active = 1) AS ShiftCount;");
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetDatabaseStaticsHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<GetDatabaseSizeStatics>("SELECT DB_NAME(database_id) AS DatabaseName, IsNull((size * 8.0 / 1024),0) AS DBTotalSize, IsNull((size * 8.0 / 1024) - (FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024), 0) AS DBFreeSize, IsNull((FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024),0) AS DBUsedSize FROM sys.master_files WHERE type = 0 and DB_NAME(database_id) = @DataBase", new { Database = AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name) }, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetMailStaticsHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<GetAllMailCountStatics>("SELECT COUNT(*) AS TotalMails, SUM(CASE WHEN sendSuccess = 0 THEN 1 ELSE 0 END) AS TotalFailedMails, SUM(CASE WHEN sendSuccess = 1 THEN 1 ELSE 0 END) AS TotalSuccessMails FROM tblEmailsHistory;", null, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}