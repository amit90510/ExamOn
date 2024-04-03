using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.DataLayer.GetDataModel;
using ExamOn.DataLayer.ViewPostData;
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

        [ForgeryTokenAuthorize]
        public async Task<JsonResult> CreateUserEnrollment(CreateStudentSignupRequest createStudentSignupRequest)
        {
            JsonData jsonData = new JsonData();
            HubContext.Notify(false, "", $"Please wait while we are getting your institution Details your details<br/>कृपया प्रतीक्षा करें जब तक हमें आपके संस्थान का विवरण मिल रहा है", true, false, false, ViewBag.srKey);
            string tenantKey = string.Empty;
            var tenants = DapperService.GetDapperData<tblTenantMaster>("select top 1 TenantUniqueKey from tbltenantMaster where TenantDBName = @tdb", new { @tdb = createStudentSignupRequest.tid}, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenants != null && tenants.Any())
            {
                HubContext.Notify(false, "", $"Please wait while we are inspecting provided details exist in institution<br/>कृपया प्रतीक्षा करें जब तक हम यह निरीक्षण नहीं कर रहे हैं कि आपके द्वारा प्रदान किया गया विवरण संस्थान के रिकॉर्ड में मौजूद है या नहीं", true, false, false, ViewBag.srKey);
                tenantKey = tenants.FirstOrDefault().TenantUniqueKey;
                var enrollmentData = DapperService.GetDapperData<tblStudentEnrollmentSignUp>("select top 1 EnrollmentNumber,Status  from tblStudentEnrollmentSignUp where TenantId = @tenKey and profileName = @pfName", new { tenKey = tenantKey, pfName = createStudentSignupRequest.Name.Trim() }, createStudentSignupRequest.tid);
                bool InsertEnrollment = true;
                if(enrollmentData != null && enrollmentData.Any())
                {
                    switch(enrollmentData.FirstOrDefault().Status)
                    {
                        case "InProcess":
                            InsertEnrollment = false;
                            HubContext.Notify(true, "ExamOn Alert", $"Enrollment request <span style='color:red'> {enrollmentData.FirstOrDefault().EnrollmentNumber} </span> is already exist for same records. Please contact to your institution to approve it.- <div class='alert alert-danger d-flex align-items-center' role='alert'><div><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> { createStudentSignupRequest.Name + " - " + createStudentSignupRequest.Email} </div></div> <br/> इन विवरणों के लिए नामांकन अनुरोध पहले से ही मौजूद है। कृपया अपने संस्थान से इसे स्वीकृत करने के लिए कहें।।", false, true, false, ViewBag.srKey);
                            break;
                        default:
                            break;
                    }
                }
                if (InsertEnrollment)
                {
                    HubContext.Notify(false, "", $"Please wait while we are uploading your details<br/>कृपया प्रतीक्षा करें जब तक हमें आपका विवरण प्राप्त/अपलोड हो रहा है", true, false, false, ViewBag.srKey);
                    string response = DapperService.ExecuteQueryResponse("DECLARE @enrollmentNumber NVARCHAR(50);SELECT @enrollmentNumber = '" + tenantKey + "-' + CAST(COUNT(*) + 1 AS NVARCHAR(50)) FROM[dbo].[tblStudentEnrollmentSignUp]; INSERT INTO [dbo].[tblStudentEnrollmentSignUp]([TenantId], [ProfileName], [Email], [Mobile], [Address], [City], [State], [EnrollmentNumber], [IsHighSchool], [HighSchoolPercentageCGPA], [IsInter], [InterPercentageCGPA], [HighSchoolCollege], [InterCollege], [IsGraduate], [GradutePercentageCGPA], [GraduateCollege], [IsPostGradute], [PostGradutePercentageCGPA], [InterestedInShift], [Gender], [PostGraduateCollege]) VALUES ( @tid, @pname,@email, @mob,@address, @city,@state,@enrollmentNumber,@isHigh,@highSchoolPercentage, @isinter,@interPercentage,@highSchoolCollege, @interCollege, @isGrad, @gradPercentage, @gradCollege,@isPG,@pgPercentage, null, @gender, @pgCollege)",
                        new
                        {
                            @tid = tenantKey,
                            @pname = createStudentSignupRequest.Name,
                            @email = createStudentSignupRequest.Email,
                            @mob = createStudentSignupRequest.Mobile,
                            @address = createStudentSignupRequest.Address,
                            @city = createStudentSignupRequest.City,
                            @state = createStudentSignupRequest.State,
                            @isHigh = createStudentSignupRequest.isHigh,
                            @highSchoolPercentage = createStudentSignupRequest.highSchoolMarks,
                            @isinter = createStudentSignupRequest.isInter,
                            @interPercentage = createStudentSignupRequest.interMarks,
                            @highSchoolCollege = createStudentSignupRequest.highSchoolCollege,
                            @interCollege = createStudentSignupRequest.interCollege,
                            @isGrad = createStudentSignupRequest.isGrad,
                            @gradPercentage = createStudentSignupRequest.GradMarks,
                            @gradCollege = createStudentSignupRequest.GradCollege,
                            @isPG = createStudentSignupRequest.isPostGrad,
                            @pgPercentage = createStudentSignupRequest.PostGradMarks,
                            @gender = createStudentSignupRequest.Gender,
                            @pgCollege = createStudentSignupRequest.PostGradCollege
                        }, createStudentSignupRequest.tid);
                    if (string.IsNullOrEmpty(response))
                    {
                        jsonData.StatusCode = 1;
                        enrollmentData = DapperService.GetDapperData<tblStudentEnrollmentSignUp>("select top 1 id,EnrollmentNumber,Status  from tblStudentEnrollmentSignUp where TenantId = @tenKey and profileName = @pfName", new { tenKey = tenantKey, pfName = createStudentSignupRequest.Name.Trim() }, createStudentSignupRequest.tid);
                        if(enrollmentData != null && enrollmentData.Any() && enrollmentData.FirstOrDefault().id > 0 && createStudentSignupRequest.shiftIntrest != null && createStudentSignupRequest.shiftIntrest.Length>0)
                        {
                            string query = $"Declare @sfid bigint; Declare @eid bigint = {enrollmentData.FirstOrDefault().id};";
                            foreach (var shift in createStudentSignupRequest.shiftIntrest)
                            {
                                query += $"Set @sfid = {shift};";
                                await DapperService.ExecuteQuery(query + " Insert into tblStudentEnrollmentShifts values(@eid,@sfid);", null, createStudentSignupRequest.tid);
                            }
                        }
                        HubContext.Notify(true, "ExamOn Alert", $"Your registration/enrollment has been completed. Please note down below enrollment number - <div class='alert alert-success d-flex align-items-center' role='alert'><div><i class='fa fa-user-circle-o' aria-hidden='true'></i> { enrollmentData.FirstOrDefault().EnrollmentNumber} </div></div> <br/> आपका पंजीकरण/नामांकन पूरा हो गया है। कृपया ऊपर नामांकन संख्या नोट कर लें", false, true, false, ViewBag.srKey);
                    }
                    else
                    {
                        jsonData.Error = response;
                        HubContext.Notify(true, "ExamOn Alert", $"Your registration/enrollment can not be completed. - <div class='alert alert-danger d-flex align-items-center' role='alert'><div><i class='fa fa-exclamation-triangle' aria-hidden='true'></i> { response} </div></div> <br/> क्षमा करें, आपका पंजीकरण/नामांकन पूरा नहीं किया जा सका।", false, true, false, ViewBag.srKey);
                    }
                }
            }
            else
            {
                HubContext.Notify(true, "ExamOn Alert", $"We are unable to find instituitions details.<br/> क्षमा करें, हम संस्थानों का विवरण ढूंढने में असमर्थ हैं|", false, true, false, ViewBag.srKey);
            }            
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}