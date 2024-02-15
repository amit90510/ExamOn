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
                List<tblTenantMaster> tenantMasterlist = new List<tblTenantMaster>();
                tenants.ForEach((tenant) => {
                    tblTenantMaster tenantMaster = new tblTenantMaster();
                    var tenantFullData = DapperService.GetDapperData<tbltenant>("Select top 1 TenantName from tblTenant", null, tenant.TenantDBName);
                    if(tenantFullData != null && tenantFullData.Any())
                    {
                        tenantMaster.TenantUniqueKey = tenant.TenantDBName;
                        tenantMaster.TenantDBName = tenantFullData.FirstOrDefault().TenantName;
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
    }
}