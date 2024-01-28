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
using ExamOn.DataLayer.GetDataModel;

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

                    response = DapperService.ExecuteQueryMultiple("delete from tblUserProfileImage where username = @username; insert into tblUserProfileImage values(@username, @fileImage, @fileName)", new
                    {
                        username = updateProfile.UserName,
                        fileImage = fileData,
                        fileName = (fileData != null) ? Path.GetExtension(updateProfile.ProfileImage.FileName) : null
                    }).Result;
                }
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
            
            if(updateProfile!=null && updateProfile.Any() && updateProfile.FirstOrDefault().ProfileImageByte != null)
            {
                return File(updateProfile.FirstOrDefault().ProfileImageByte, "Image/" + updateProfile.FirstOrDefault().ProfileImageName.Replace(".", ""));
            }
            
            return null;
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> UpdateUserProfilePassword(UpdateProfile profile)
        {
            JsonData jsonData = new JsonData();
            var updateProfile = DapperService.GetDapperData<UpdateProfile>("select top 1 UserName from tbllogin where username = @username and password = @oldPass", new
            {
                username = AuthorizeService.GetUserName(HttpContext.User.Identity.Name),
                oldPass = EncryptionDecryption.EncryptString(profile.UserName)
            });

            if (updateProfile != null && updateProfile.Any() && updateProfile.FirstOrDefault().UserName != null)
            {
                var response = DapperService.ExecuteQueryResponse("update tbllogin set password = @password where username = @username", new { 
                     username = AuthorizeService.GetUserName(HttpContext.User.Identity.Name),
                     password = EncryptionDecryption.EncryptString(profile.ProfileName)
                }, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
               
                if (!string.IsNullOrEmpty(response))
                {
                    HubContext.Notify(true, "ExamOn - Alert", $"Password can not be updated.<br/> पासवर्ड बदला नहीं जा सकता. <br/> {response}", false, true, false, ViewBag.srKey);
                }
                else
                {
                    jsonData.StatusCode = 1;
                    HubContext.Notify(true, "ExamOn - Alert", "Password has been updated.<br/> <hr/>पासवर्ड अपडेट कर दिया गया है, अब आप दोबारा लॉगइन पेज पर जाएंगे", false,false, false, ViewBag.srKey);
                }
            }
            else
            {
                HubContext.Notify(true, "ExamOn - Alert", $"Password Can not be updated. There can be two reason - <br/> 1) You are not providing correct Old password </br> 2) We can not validate your user credientials. <br/> <hr/>पासवर्ड बदला नहीं जा सकता.", false, true, false, ViewBag.srKey);
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet); 
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetShiftAssociation()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<UserShiftAssociation>("select top 5 tc.ClassName, bt.Batch, sf.Shift  from tblusershift usf inner join tblshift sf on usf.shiftId = sf.id and usf.active = 1 and sf.Active = 1 and usf.Userid = (select top 1 id from tbluserProfile where UserName =@username) inner join tblbatch bt on sf.batch = bt.id and bt.Active =1 inner join tblclass tc on bt.class = tc.id and tc.Active = 1", new { username = AuthorizeService.GetUserName(HttpContext.User.Identity.Name) });
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetUpcomingExams()
        {
            JsonData jsonData = new JsonData();
            var upcomingExamGetModel = DapperService.GetDapperDataDynamic<UpcomingExamGetModel>("select  top 5 e.ExamName, e.StartExam, e.updatedOn, e.EntryAllowedTill, count(sec.id) as 'SectionCount' from tblexamStudents es inner join tblexam e on es.exam = e.id and e.Active = 1 and es.userName = (select top 1 id from tbllogin where UserName =@username) left join tblexamSections sec on e.id = sec.exam group by e.ExamName, e.StartExam, e.updatedOn, e.EntryAllowedTill", new { username = AuthorizeService.GetUserName(HttpContext.User.Identity.Name) });
            if (upcomingExamGetModel != null && upcomingExamGetModel.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = upcomingExamGetModel.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}