using Dapper;
using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using ExamOn.SignalRPush;
using ExamOn.Utility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace ExamOn.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Go()
        {
            // User.Identity.Name
            ViewBag.Version = WebConfigurationManager.AppSettings["ExamOnVersion"];
            return View("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ForgeryTokenAuthorize]
        [Route("Login/{ExamOnSignIn}")]
        public JsonResult LoginIn([System.Web.Http.FromBody] tbllogin loginparams)
        {
            JsonData jsonData = new JsonData();
            try
            {
                using (IDbConnection db = new SqlConnection(DBConnection.GetConnectionString(WebConfigurationManager.AppSettings["ExamOnMasterDB"])))
                {
                    string[] token = loginparams.UserName.Split('-');
                    var tenantMasters = db.Query<tblTenantMaster>("select * from tbltenantmaster where TenantUniqueKey = @keyvalue", new { keyvalue = token[0] });
                    if (tenantMasters != null && tenantMasters.Any())
                    {
                        jsonData.StatusCode = 200;
                        using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(tenantMasters.Select(e => e.TenantDBName).FirstOrDefault())))
                        {
                            var tbllogin = mainDB.Query<tbllogin>("select id, EmailId, Active, TenantToken, LoginType, blockLogin from tbllogin where username = @username and Password = @password", new { username = loginparams.UserName,password = EncryptionDecryption.EncryptString(loginparams.Password) });
                            if(tbllogin != null && tbllogin.Any())
                            {
                                HubContext.Notify(false, "", $"Checking your active profile <br/> कृपया प्रतीक्षा करें, आपकी सक्रिय प्रोफ़ाइल की जाँच की जा रही है।", true, false, false, ViewBag.srKey);
                                if (tbllogin.FirstOrDefault().Active)
                                {
                                    var tblloginType = mainDB.Query<tblloginType>("select Type from tblloginType where id = @typeId", new { typeId = tbllogin.FirstOrDefault().LoginType });
                                    switch (tblloginType.FirstOrDefault().Type.ToUpper())
                                    {
                                        case "S":
                                            jsonData.Error = "StudDashboard";
                                            break;
                                        case "W":
                                            jsonData.Error = "WebAdminDashboard";
                                            break;
                                        case "T":
                                            jsonData.Error = "InstructorTeacherDashboard";
                                            break;
                                        case "A":
                                            jsonData.Error = "TenantAdminDashboard";
                                            break;
                                        default:
                                            jsonData.Error = "";
                                            break;
                                    }
                                    //check for block login
                                    if (tbllogin.FirstOrDefault().BlockLogin.Value)
                                    {
                                        jsonData.StatusCode = 500;
                                        HubContext.Notify(true, "ExamOn Alert", "Temporary blocked login, Please contact adminisitator. <br/>अस्थायी रूप से अवरुद्ध लॉगिन, कृपया अपने संस्थान से संपर्क करें।", false, true, false, ViewBag.srKey);
                                    }
                                    else
                                    {
                                        jsonData.StatusCode = 1;
                                        FormsAuthentication.SetAuthCookie(AuthorizeService.SetIdentityCookieValue(tbllogin.FirstOrDefault().id, tbllogin.FirstOrDefault().TenantToken, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault().ToString(), loginparams.UserName), false);
                                        HubContext.Notify(false, "", $"We have verified you, preparing your dashboard.<br/> हमने आपका सत्यापन कर लिया है, आपका डैशबोर्ड तैयार किया जा रहा है, कृपया प्रतीक्षा करें", true, false, false, ViewBag.srKey);
                                        DapperService.ExecuteQueryMultiple("Delete from TblloginHistory where userName = @userName and LoginDate <= DATEADD(DAY, -5, GETDATE());Insert into TblloginHistory values(@username, @Ip, @browser, GetDate())", new { username = loginparams.UserName, Ip = LoginStatics.GetIp(), browser = LoginStatics.GetBrowser() }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault().ToString()).ConfigureAwait(false);
                                    }
                                }
                                else
                                {
                                    HubContext.Notify(false, "", "Please wait, while we are checking your instituion  <br/> कृपया प्रतीक्षा करें।", true, false, false, ViewBag.srKey);
                                    var _tenantdata = DapperService.GetDapperData<tbltenant>("select top 1 * from tbltenant where id = @tenantToken", new { tenantToken = tbllogin.FirstOrDefault().TenantToken }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                    if (_tenantdata != null && _tenantdata.Any())
                                    {
                                        jsonData.StatusCode = 500;
                                        HubContext.Notify(true, "ExamOn Alert", $"Your Account is disabled, Please contact to {_tenantdata.FirstOrDefault().TenantName} <br/> आपको ब्लॉक कर दिया गया है, कृपया अपने संस्थान से संपर्क करें।", false, true, false, ViewBag.srKey);

                                    }
                                    else
                                    {
                                        jsonData.StatusCode = 500;
                                        HubContext.Notify(true, "ExamOn Alert", "We could not find your instituition, Please contact adminisitator. <br/> कृपया अपने संस्थान से संपर्क करें।", false, true, false, ViewBag.srKey);
                                    }
                                }
                            }
                            else
                            {
                                HubContext.Notify(true, "ExamOn Alert", "Invalid username and password. <br/> कृपया आपको दिया गया सही उपयोगकर्ता नाम और पासवर्ड दर्ज करें।", false, true, false, ViewBag.srKey);
                            }
                        }                        
                    }
                    else
                    {
                        jsonData.StatusCode = 500;
                        HubContext.Notify(true, "ExamOn Alert", "Invalid User Name <br/>(अमान्य उपयोगकर्ता नाम)", false, true, false, ViewBag.srKey);
                    }
                }
            }
            catch (Exception e) {
                jsonData.StatusCode = 500;
                jsonData.Error = e.Message;
                HubContext.Notify(true, "ExamOn Alert", e.Message,false, true, false, ViewBag.srKey);
            }     
            return Json(JsonResponse.JsonResponseData(jsonData), JsonRequestBehavior.AllowGet);
        }
        
    }
}