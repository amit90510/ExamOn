using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using ExamOn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using NonActionAttribute = System.Web.Mvc.NonActionAttribute;

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
            string response = updateTenant(tblTenantMaster);
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

        [NonAction]
        public string updateTenant(tbltenant tblTenantMaster)
        {
           return DapperService.ExecuteQueryResponse("update tbltenant set TenantName = @Name, TenantAddress = @Address, TenantEmail = @email, TenantMobile = @mobile, RechargeAmount = @amount, SubscriptionEndMessage = @endmessage, LastRechargeOn = GETDATE(), SubscriptionEndDate = @subendDate where id =@id",
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
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> SaveTenantDetailsMail([FromBody] tbltenant tblTenantMaster)
        {
            JsonData jsonData = new JsonData();
            string response = updateTenant(tblTenantMaster);
            if (string.IsNullOrEmpty(response))
            {
                jsonData.StatusCode = 1;
                EmailService.SendEmail(new string[] { tblTenantMaster.TenantEmail }, "ExamOn : Tenant Profile Updated !!", $"Hi {tblTenantMaster.TenantName},<br/>Your profile has been updated by web admininstrator, Please find below details", $"<br> Subscription end date - {tblTenantMaster.SubscriptionEndDate.ToLongDateString()} <br/> Amount - {tblTenantMaster.RechargeAmount} <br/> Mobile - {tblTenantMaster.TenantMobile}");
            }
            else
            {
                jsonData.Error = response;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetNextRecordForUserName()
        {
            JsonData jsonData = new JsonData();
            var response = DapperService.GetDapperData<tbllogin>("select top 1 case when count(id) > 0 then (count(id)+1) else 1 end as id from tbllogin",null, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (response != null && response.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = response.FirstOrDefault().id;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantEmailCredientials()
        {
            JsonData jsonData = new JsonData();
            var response = DapperService.GetDapperData<tbllogin>("select top 1 EmailFromAddress as 'UserName', Password from tblEmailCredientials", null, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (response != null && response.Any())
            {
                jsonData.StatusCode = 1;
                foreach (var item in response)
                {
                    item.Password = EncryptionDecryption.DecryptString(item.Password);
                }
                jsonData.Data = response.FirstOrDefault();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> UpdateTenantEMailCredientials([FromBody] tbllogin tbllogin)
        {
            JsonData jsonData = new JsonData();
            string response = DapperService.ExecuteQueryResponse("delete from tblEmailCredientials; Insert into tblEmailCredientials values(@email, @password)",
                new
                {
                    email = tbllogin.UserName,
                    password = EncryptionDecryption.EncryptString(tbllogin.Password)
                }, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name)) ;
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

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantAvailablelogins()
        {
            JsonData jsonData = new JsonData();
            var response = DapperService.GetDapperData<tbllogin>("select tbl.Username, CONCAT(up.RealName,'>>',ty.typeName,'>>', EmailId,'>>', TenantToken) as emailId from tbllogin tbl inner join tbluserProfile up on tbl.UserName = up.UserName left join tblloginType ty on Logintype = ty.id", null, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (response != null && response.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = response.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}