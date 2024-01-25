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
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System;
using System.Web.Http;
using ExamOn.DataLayer.ViewPostData;

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
            JsonData jsonData = new JsonData();
            var loginHisory = DapperService.GetDapperData<tblLoginHistory>("select top 5 ip, browser, CONVERT(varchar,LoginDate,100) as loginDate from TblloginHistory where userName = @userName order by LoginDate desc", new { username = ViewBag.UserName });
            if (loginHisory != null && loginHisory.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = loginHisory.ToList(); 
            }
            return Json( jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetAssocaitionHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("select TenantName, TenantMobile, TenantEmail, SubscriptionEndDate from tblTenant where Id = @id", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if(tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetLicenceExpire()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("select SubscriptionEndMessage,TenantName from tblTenant where Id = @id and SubscriptionEndDate < GetDate()", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.Error = $"{tenantData.FirstOrDefault().SubscriptionEndMessage} <br/> (आपका लाइसेंस समाप्त हो गया है, कृपया अपने संस्थान/प्रतिष्ठान {tenantData.FirstOrDefault().TenantName} से संपर्क करें।)";
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<PartialViewResult> GetUpdateProfilePage()
        {
            return PartialView("UpdateProfile");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<PartialViewResult> GetUpdateProfilePasswordPage()
        {
            return PartialView("UpdatePassword");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetStatesInfo()
        {
            JsonData jsonData = new JsonData();
            using (StreamReader reader = new StreamReader(Server.MapPath("~/DataLayer/States.json")))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                jsonData.Data = serializer.Deserialize<dynamic>(reader.ReadToEnd());
                jsonData.StatusCode = 1;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetUserProfileDate()
        {
            JsonData jsonData = new JsonData();
            var userProfile = DapperService.GetDapperDataDynamic<dynamic>("select a.Id,a.username, a.EmailId,a.Mobile, a.Active, tp.RealName,tp.address,tp.City,tp.State, b.TypeName from tbllogin a inner join tblloginType b on a.logintype = b.id inner join tbluserProfile tp on a.UserName = tp.UserName and a.userName = @username",  new { userName = AuthorizeService.GetUserName(HttpContext.User.Identity.Name) });
            if(userProfile !=null && userProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userProfile;
            }
           // HubContext.Notify(false, "", "Profile Load Complete.", false, false, true, ViewBag.srKey);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> UserProfileDataUpdate([FromBody] UpdateProfile updateProfile)
        {
            JsonData jsonData = new JsonData();
            HubContext.Notify(false, "", "Please wait, while we are updating profile <b/> कृपया प्रतीक्षा करें, जब तक हम प्रोफ़ाइल अपडेट कर रहे हैं", true, false, false, ViewBag.srKey);
            var response = DapperService.ExecuteQueryMultiple("Update tbluserProfile set RealName = @realName, address = @address, city=@city, state = @state where username=@username; Update tbllogin set EMailID = @email, Mobile=@mobile where username = @username;",new { 
                realName = updateProfile.ProfileName,
                address = updateProfile.Address,
                city = updateProfile.City,
                state = updateProfile.State,
                username = updateProfile.UserName,
                email = updateProfile.Email,
                mobile = updateProfile.Mobile
            }).Result;
            if (!string.IsNullOrEmpty(response))
            {              
                HubContext.Notify(true, "ExamOn - Alert", $"Profile Can not be updated.<br/> प्रोफ़ाइल अपडेट नहीं किया जा सकता. <br/> {response}", false, true, false, ViewBag.srKey);
            }
            else
            {
                //update image if any
                byte[] fileData = null;
                if (updateProfile.ProfileImage != null && updateProfile.ProfileImage.ContentLength > 0)
                {
                    HubContext.Notify(false, "", "Please wait, while we are updating profile image <b/> कृपया प्रतीक्षा करें, जब तक हम प्रोफ़ाइल छवि अपडेट कर रहे हैं", true, false, false, ViewBag.srKey);
                                    
                    using (var binaryReader = new BinaryReader(updateProfile.ProfileImage.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(updateProfile.ProfileImage.ContentLength);
                    }
                }
                response = DapperService.ExecuteQueryMultiple("delete from tblUserProfileImage where username = @username; insert into tblUserProfileImage values(@username, @fileImage, @fileName)", new
                {
                    username = updateProfile.UserName,
                    fileImage = fileData,
                    fileName = (fileData != null) ? Path.GetExtension(updateProfile.ProfileImage.FileName) : null
                }).Result;
                jsonData.StatusCode = 1;
                if (!string.IsNullOrEmpty(response))
                {
                    HubContext.Notify(true, "ExamOn - Alert", $"Profile is updated, but Image can not be updated.<br/> प्रोफ़ाइल अपडेट हो गई है, लेकिन छवि अपडेट नहीं की जा सकती. <br/> {response}", false, true, false, ViewBag.srKey);
                }
                else
                {
                    HubContext.Notify(true, "ExamOn - Alert", "Profile has been updated.<br/> प्रोफ़ाइल अपडेट कर दी गई है.", false, true, false, ViewBag.srKey);
                }
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<FileResult> GetUserProfileImage()
        {
            var updateProfile = DapperService.GetDapperData<UpdateProfile>("select top 1 ProfileImage as 'ProfileImageByte', ProfileImageName from tblUserProfileImage where username = @username", new { 
             username = AuthorizeService.GetUserName(HttpContext.User.Identity.Name)
            });
            
            if(updateProfile!=null && updateProfile.Any())
            {
                return File(updateProfile.FirstOrDefault().ProfileImageByte, "Image/" + updateProfile.FirstOrDefault().ProfileImageName.Replace(".", ""));
            }
            
            return null;
        }
    }
}