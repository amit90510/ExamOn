using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;

namespace ExamOn.Controllers
{
    public class WebAdminDashboardController : Controller
    {
        [AuthorizeAction]
        public ActionResult Go()
        {
            return View("Index");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAllTenants()
        {
            JsonData jsonData = new JsonData();
            var tenants = DapperService.GetDapperData<tblTenantMaster>("select *  from tbltenantMaster", null, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenants != null && tenants.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenants.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantDetails([FromBody] tblTenantMaster tblTenantMaster)
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("select top 1 *  from tbltenant where id = @tenantid", new { tenantid = tblTenantMaster.TenantUniqueKey}, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData.FirstOrDefault();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAllLoginType()
        {
            JsonData jsonData = new JsonData();
            var logintype = DapperService.GetDapperData<tblloginType>("select type, TypeName  from tblloginType", null, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (logintype != null && logintype.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = logintype.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> SaveTenantDetails([FromBody] tbltenant tblTenantMaster)
        {
            JsonData jsonData = new JsonData();
            string response = DapperService.ExecuteQueryResponse("update tbltenant set TenantName = @Name, TenantAddress = @Address, TenantEmail = @email, TenantMobile = @mobile, RechargeAmount = @amount, SubscriptionEndMessage = @endmessage, LastRechargeOn = GETDATE(), SubscriptionEndDate = @subendDate where id =@id",
                new
                {
                    id = tblTenantMaster.id,
                    Name = tblTenantMaster.TenantName,
                    Address = tblTenantMaster.TenantAddress,
                    email = tblTenantMaster.TenantEmail,
                    amount = tblTenantMaster.RechargeAmount,
                    mobile = tblTenantMaster.TenantMobile,
                    endmessage = tblTenantMaster.SubscriptionEndMessage,
                    subendDate = tblTenantMaster.SubscriptionEndDate.ToString("yyyy-MM-dd")
                }, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (string.IsNullOrEmpty(response))
            {
                jsonData.StatusCode = 1;
            }
            else
            {
                jsonData.Error = response;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}