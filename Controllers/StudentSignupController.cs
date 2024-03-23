using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.DataLayer.GetDataModel;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using ExamOn.SignalRPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ExamOn.Controllers
{
    public class StudentSignupController : Controller
    {
        [AllowAnonymous]
        public ActionResult Go()
        {
            ViewBag.Version = WebConfigurationManager.AppSettings["ExamOnVersion"];
            return View("Index");
        }

        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAllInstituitionsLinked()
        {
            JsonData jsonData = new JsonData();
            var tenants = DapperService.GetDapperData<tblTenantMaster>("select *  from tbltenantMaster", null, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenants != null && tenants.Any())
            {
                jsonData.StatusCode = 1;
                List<GetTenantMasterMoreInformation> tenantMasterlist = new List<GetTenantMasterMoreInformation>();
                tenants.ForEach((tenant) => {
                    GetTenantMasterMoreInformation tenantMaster = new GetTenantMasterMoreInformation();
                    var tenantFullData = DapperService.GetDapperData<GetTenantMasterMoreInformation>("Select top 1 * from tblTenant", null, tenant.TenantDBName);
                    if(tenantFullData != null && tenantFullData.Any())
                    {
                        tenantMaster.TenantUniqueKey = tenant.TenantDBName;
                        tenantMaster.TenantDBName = tenantFullData.FirstOrDefault().TenantName;
                        tenantMaster.TenantAddress = tenantFullData.FirstOrDefault().TenantAddress;
                        tenantMaster.TenantEmail = tenantFullData.FirstOrDefault().TenantEmail;
                    }
                    tenantMasterlist.Add(tenantMaster);
                });
                jsonData.Data = tenantMasterlist;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetCourseShiftAvailable(string tenantId)
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<UserShiftAssociation>("select tc.ClassName, bt.Batch, sf.Shift, sf.id as ShiftID  from tblusershift usf inner join tblshift sf on usf.shiftId = sf.id and usf.active = 1 and sf.Active = 1 inner join tblbatch bt on sf.batch = bt.id and bt.Active =1 inner join tblclass tc on bt.class = tc.id and tc.Active = 1", null, tenantId);
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetEnrollmentStatus(string eid)
        {
            JsonData jsonData = new JsonData();
            string[] tenant = eid.Split('-');
            HubContext.Notify(false, "", $"Please wait while we are Checking institution details on {eid} <br/>कृपया प्रतीक्षा करें जब तक हम इस आईडी पर संस्थान के विवरण की जाँच कर रहे हैं", true, false, false, ViewBag.srKey);
            var tenants = DapperService.GetDapperData<tblTenantMaster>("select top 1 TenantDBName  from tbltenantMaster where TenantUniqueKey = @tenantKey", new { tenantKey = tenant[0]}, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenants !=null && tenants.Any())
            {
                HubContext.Notify(false, "", $"Please wait while we are Checking your enrollment id <br/> जब तक हम आपकी नामांकन आईडी की जाँच कर रहे हैं, कृपया प्रतीक्षा करें", true, false, false, ViewBag.srKey);
                var userenrollment = DapperService.GetDapperDataDynamic<tblStudentEnrollmentSignUp>("select top 1 id, status from tblStudentEnrollmentSignUp where EnrollmentNumber = @enid", new { enid = eid }, tenants.FirstOrDefault().TenantDBName);
                if (userenrollment != null && userenrollment.Any())
                {
                    jsonData.StatusCode = 1;
                    HubContext.Notify(true, "ExamOn Alert", $"We have found {eid} in our records and status - <div class='alert alert-danger d-flex align-items-center' role='alert'><div><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> { userenrollment.FirstOrDefault().Status} </div></div> <br/> हमें इस नामांकन आईडी की स्थिति मिल गई है।", false, true, false, ViewBag.srKey);
                    jsonData.Data = userenrollment.FirstOrDefault();
                }
                else
                {
                    HubContext.Notify(true, "ExamOn Alert", $"We are unable to locate the status of {eid}<br/> क्षमा करें, हम इस नामांकन आईडी की स्थिति का पता लगाने में असमर्थ हैं|", false, true, false, ViewBag.srKey);
                }
            }
            else
            {
                HubContext.Notify(true, "ExamOn Alert", $"We are unable to find instituitions details on {eid}.<br/> क्षमा करें, हम संस्थानों का विवरण ढूंढने में असमर्थ हैं, कृपया सही आईडी दर्ज करें|", false, true, false, ViewBag.srKey);
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}