using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.DataLayer.GetDataModel;
using ExamOn.DataLayer.ViewPostData;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using ExamOn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        [NonActionAttribute]
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
        public async Task<JsonResult> GetAllTenantSubscription()
        {
            JsonData jsonData = new JsonData();
            var tenants = DapperService.GetDapperData<tblTenantMaster>("Select * from tbltenantMaster", null, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenants != null && tenants.Any())
            {
                jsonData.StatusCode = 1;
                List<tbltenant> tbltenants = new List<tbltenant>();
                foreach (var tblTenant in tenants)
                {
                    var tenantDetails = DapperService.GetDapperData<tbltenant>("SELECT [id] ,[TenantName], [TenantEmail], [TenantMobile],[SubscriptionEndDate] , [LastRechargeOn] ,[RechargeAmount] FROM [" + tblTenant.TenantDBName + "].dbo.tbltenant;", null, tblTenant.TenantDBName);
                    if(tenantDetails != null && tenantDetails.Any())
                    {
                        tbltenants.Add(tenantDetails.FirstOrDefault());
                    }
                }
                jsonData.Data = tbltenants.OrderByDescending(e=>e.SubscriptionEndDate).ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAllTenantSubscriptionHistory(string tid)
        {
            JsonData jsonData = new JsonData();
            var tenantDB = DapperService.GetDapperData<tblTenantMaster>("Select [TenantDBName] from tbltenantMaster where TenantUniqueKey = @tid", new { @tid = tid}, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if(tenantDB != null && tenantDB.Any())
            {
                var tenants = DapperService.GetDapperData<tblTenantRechargeHistory>("SELECT [SubscptionStartFrom],[SubscriptionEndAt],[RechargeAmount],[CreatedDate] from [dbo].[tblTenantRechargeHistory] where TID = '" + tid + "'", null, tenantDB.FirstOrDefault().TenantDBName);
                if (tenants != null && tenants.Any())
                {
                    jsonData.StatusCode = 1;
                    jsonData.Data = tenants.OrderByDescending(e => e.CreatedDate).ToList();
                }
            }            
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantDetails([FromBody] tblTenantMaster tblTenantMaster)
        {
            JsonData jsonData = new JsonData();
            var tenantDB = DapperService.GetDapperData<tblTenantMaster>("Select [TenantDBName] from tbltenantMaster where TenantUniqueKey = @tid", new { @tid = tblTenantMaster.TenantUniqueKey }, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenantDB != null && tenantDB.Any())
            {
                var tenantData = DapperService.GetDapperData<tbltenant>("select top 1 *  from tbltenant where id = @tenantid", new { tenantid = tblTenantMaster.TenantUniqueKey }, tenantDB.FirstOrDefault().TenantDBName);
                if (tenantData != null && tenantData.Any())
                {
                    jsonData.StatusCode = 1;
                    jsonData.Data = tenantData.FirstOrDefault();
                }
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAllLoginType()
        {
            JsonData jsonData = new JsonData();
            var logintype = DapperService.GetDapperData<tblloginType>("select id as 'type', TypeName  from tblloginType", null, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (logintype != null && logintype.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = logintype.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> SaveTenantDetails([FromBody] tbltenant tblTenantMaster, bool history = false)
        {
            JsonData jsonData = new JsonData();
            string response = updateTenant(tblTenantMaster);
            if (string.IsNullOrEmpty(response))
            {
                jsonData.StatusCode = 1;
                if(history)
                {
                    updateTenantHistory(tblTenantMaster);
                }
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
                }, GetDBNameFromId(tblTenantMaster.id));
        }

        [NonAction]
        public string updateTenantHistory(tbltenant tblTenantMaster)
        {
            return DapperService.ExecuteQueryResponse("Insert into tblTenantRechargeHistory(SubscriptionEndAt,RechargeAmount, TID) values(@subEnd, @rech, @tid)",
                 new
                 {
                     tid = tblTenantMaster.id,
                     rech = tblTenantMaster.RechargeAmount,
                     subEnd = tblTenantMaster.SubscriptionEndDate.ToString("yyyy-MM-dd")
                 }, GetDBNameFromId(tblTenantMaster.id));
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> SaveTenantDetailsMail([FromBody] tbltenant tblTenantMaster, bool history = false)
        {
            JsonData jsonData = new JsonData();
            string response = updateTenant(tblTenantMaster);
            if (string.IsNullOrEmpty(response))
            {
                jsonData.StatusCode = 1;
                if (history)
                {
                    updateTenantHistory(tblTenantMaster);
                }
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
        public async Task<JsonResult> GetNextRecordForUserName(string tid)
        {
            JsonData jsonData = new JsonData();
            var response = DapperService.GetDapperData<tbllogin>("select top 1 case when count(id) > 0 then (count(id)+1) else 1 end as id from tbllogin",null, GetDBNameFromId(tid));
            if (response != null && response.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = response.FirstOrDefault().id;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [NonActionAttribute]
        public string GetDBNameFromId(string id)
        {
            string dbName = "";
            var tenantDB = DapperService.GetDapperData<tblTenantMaster>("Select [TenantDBName] from tbltenantMaster where TenantUniqueKey = @tid", new { @tid = id }, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenantDB != null && tenantDB.Any())
            {
                dbName =  tenantDB.FirstOrDefault().TenantDBName;
            }
            return dbName;
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantEmailCredientials(string tid)
        {
            JsonData jsonData = new JsonData();
            var response = DapperService.GetDapperData<tbllogin>("select top 1 EmailFromAddress as 'UserName', Password from tblEmailCredientials", null, GetDBNameFromId(tid));
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
        public async Task<JsonResult> UpdateTenantEMailCredientials([FromBody] tbllogin tbllogin, string tid)
        {
            JsonData jsonData = new JsonData();
            string response = DapperService.ExecuteQueryResponse("delete from tblEmailCredientials; Insert into tblEmailCredientials values(@email, @password)",
                new
                {
                    email = tbllogin.UserName,
                    password = EncryptionDecryption.EncryptString(tbllogin.Password)
                }, GetDBNameFromId(tid)) ;
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

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetUserLoginStatus([FromBody] tbllogin tbllogin)
        {
            JsonData jsonData = new JsonData();
            var response = DapperService.GetDapperData<tbllogin>("select top 1 Active, BlockLogin, BlockMessage from tbllogin where username = @username", new { username = tbllogin.UserName}, AuthorizeService.GetUserDBName(System.Web.HttpContext.Current.User.Identity.Name));
            if (response != null && response.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = response.FirstOrDefault();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> UpdateUserLoginStatus([FromBody] tbllogin tbllogin)
        {
            JsonData jsonData = new JsonData();
            string response = DapperService.ExecuteQueryResponse("update tbllogin set Active = @act, blockLogin = @bl where username = @username",
                new
                {
                    username = tbllogin.UserName,
                    act = tbllogin.Active,
                    bl = tbllogin.BlockLogin,
                    bm = tbllogin.BlockMessage
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

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> CreateUserLogin([FromBody] CreateUserLogins createUserLogins, string Isemail = "0", string tid = "")
        {
            JsonData jsonData = new JsonData();
            string response = DapperService.ExecuteQueryResponse("Insert into tbllogin(username, password, EmailId, TenantToken, LoginType) values(@username, @password, @EmailId, @TenantToken, @LoginType); insert into tbluserProfile(username,RealName) values(@username,@RealName);",
                new
                {
                    username = createUserLogins.UserName,
                    password = createUserLogins.EncryptPassword,
                    EmailId = createUserLogins.Email,
                    TenantToken = createUserLogins.TenantToken,
                    LoginType = createUserLogins.LoginType,
                    RealName = createUserLogins.ProfileName
                }, GetDBNameFromId(tid));
            if (string.IsNullOrEmpty(response))
            {
                jsonData.StatusCode = 1;
                if(Isemail == "1")
                {
                    EmailService.SendEmail(new string[] { createUserLogins.Email }, "ExamOn : Congratulation !!", $"Hi {createUserLogins.ProfileName},<br/>Your profile has been created by web admininstrator, Please find below details", $"<br> Username - {createUserLogins.UserName} <br/> Password - {createUserLogins.Password}");
                }
            }
            else
            {
                jsonData.Error = response;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetEncryptDecryptionString(string phrase, bool isDecrypt = false)
        {
            JsonData jsonData = new JsonData();
            try
            {
                if (isDecrypt)
                {
                    jsonData.Data = EncryptionDecryption.DecryptString(phrase);
                    jsonData.StatusCode = 1;
                }
                else
                {
                    jsonData.Data = EncryptionDecryption.EncryptString(phrase);
                    jsonData.StatusCode = 1;
                }
            }
            catch(Exception excEnc)
            {
                jsonData.Error = excEnc.Message;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetUserAccessTypePermission(int userType, string tid)
        {
            JsonData jsonData = new JsonData();
            Assembly asm = Assembly.GetExecutingAssembly();
           var controllers =  asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));
            jsonData.StatusCode = 1;
            int counter = 1;
            List<UserTypeAccessPermission> userTypeAccessPermissionsList = new List<UserTypeAccessPermission>();
            var response = DapperService.GetDapperData<tblUserTypeAccess>("select * from tblUserTypeAccess where TypeId = @typeID", new { typeID = userType }, GetDBNameFromId(tid));
                     
            foreach (var paths in controllers.Where(e => !string.IsNullOrEmpty(e.DeclaringType.Name) 
            && !string.IsNullOrEmpty(e.Name) 
            && e.DeclaringType.Name != "Controller"
            && e.DeclaringType.Name != "ControllerBase"
            && e.DeclaringType.Name != "Object"
            && e.Name.EndsWith("Go", StringComparison.OrdinalIgnoreCase)
            && e.CustomAttributes.Any(p=>p.AttributeType.Name.Equals("AuthorizeAction", StringComparison.OrdinalIgnoreCase))
            ))
            {
                UserTypeAccessPermission userTypeAccessPermission = new UserTypeAccessPermission();
                if (response != null && response.Any())
                {
                    var isAvailable = response.ToList().Where(e => e.UserPath.Equals(paths.DeclaringType.Name + "/" + paths.Name, StringComparison.OrdinalIgnoreCase));
                    if (isAvailable != null && isAvailable.Any())
                    {
                        userTypeAccessPermission.IsActive = true;
                    }
                }
                userTypeAccessPermission.ID = counter;
                userTypeAccessPermission.Route = paths.DeclaringType.Name + "/" + paths.Name;
                userTypeAccessPermissionsList.Add(userTypeAccessPermission);
                counter++;
            }
            jsonData.Data = userTypeAccessPermissionsList.ToArray();

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> UpdatePermissions([FromBody] UserTypeAccessPermission[] userTypeAccessPermissions, int TypeId, string tid)
        {
            JsonData jsonData = new JsonData();
            string response = string.Empty;
            foreach (var item in userTypeAccessPermissions)
            {
               response = DapperService.ExecuteQueryResponse("IF NOT EXISTS ( SELECT * FROM tblUserTypeAccess WHERE userPath = @path and typeId = @typeId ) BEGIN IF(@active = 1) Begin INSERT INTO tblUserTypeAccess(typeId,UserPath) values(@typeId, @path) End END ELSE Begin IF(@active = 0) Begin Delete from tblUserTypeAccess where userPath = @path and typeId = @typeId End END",
               new
               {
                   @path = item.Route,
                   @typeId = TypeId,
                   @active = item.IsActive ? 1 : 0
               }, GetDBNameFromId(tid));               
            }
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