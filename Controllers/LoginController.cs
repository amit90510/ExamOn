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
                using (IDbConnection db = new SqlConnection(DBConnection.GetConnectionString("ExamOn_Master")))
                {
                    string[] token = loginparams.UserName.Split('-');
                    var tenantMasters = db.Query<tblTenantMaster>("select * from tbltenantmaster where TenantUniqueKey = @keyvalue", new { keyvalue = token });
                    if (tenantMasters != null && tenantMasters.Any())
                    {
                        jsonData.StatusCode = 200;
                        using (IDbConnection mainDB = new SqlConnection(DBConnection.GetConnectionString(tenantMasters.Select(e => e.TenantDBName).FirstOrDefault())))
                        {
                            var tbllogin = mainDB.Query<tbllogin>("select * from tbllogin where username = @username and Password = @password", new { username = loginparams.UserName, password = loginparams.Password });
                            if(tbllogin != null && tbllogin.Any())
                            {

                            }
                            else
                            {
                                HubContext.Notify(true, "ExamOn Alert", "Invalid username and password. <br/> कृपया आपको दिया गया सही उपयोगकर्ता नाम और पासवर्ड दर्ज करें।", false, true, false);
                            }
                        }                        
                    }
                    else
                    {
                        jsonData.StatusCode = 500;
                        HubContext.Notify(true, "ExamOn Alert", "Invalid User Name <br/>(अमान्य उपयोगकर्ता नाम)", false, true, false);
                    }
                }
            }
            catch (Exception e) {
                jsonData.StatusCode = 500;
                jsonData.Error = e.Message;
                HubContext.Notify(true, "ExamOn Alert", e.Message,false, true, false);
            }     
            return Json(JsonResponse.JsonResponseData(jsonData), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ForgeryTokenAuthorize]
        [Route("Login/{ExamOnForgetPassword}")]
        public JsonResult ForgotMyPassword([System.Web.Http.FromBody] tbllogin loginparams)
        {
            JsonData jsonData = new JsonData();
            try
            {
                using (IDbConnection db = new SqlConnection(DBConnection.GetConnectionString("ExamOn_Master")))
                {
                    string[] token = loginparams.UserName.Split('-');
                    var tenantMasters = db.Query<tblTenantMaster>("select * from tbltenantmaster where TenantUniqueKey = @keyvalue", new { keyvalue = token });
                    if (tenantMasters != null && tenantMasters.Any())
                    {
                        jsonData.StatusCode = 200;
                        var _logindata = DapperService.GetDapperData<tbllogin>("select top 1 * from tbllogin where username = @username", new { username = loginparams.UserName}, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                        if(_logindata != null && _logindata.Any())
                        {
                            if(_logindata.FirstOrDefault().Active)
                            {
                                HubContext.Notify(false, "", $"Please wait, while we are sending your Pasword on {_logindata.FirstOrDefault().EmailId} <br/> कृपया प्रतीक्षा करें। हम आपका पासवर्ड भेज रहे हैं", true, false, false);
                               var response = EmailService.SendEmailResponse(new string[] { _logindata.FirstOrDefault().EmailId }, "Examon - Forgot/Retrieve Password", "पासवर्ड भूल गए/पुनः प्राप्त करें",$"Hi <b> {_logindata.FirstOrDefault().UserName} </b> <br/> your password is (your password is) - <b>{ EncryptionDecryption.DecryptString(_logindata.FirstOrDefault().Password)} </b>", tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                if(!string.IsNullOrEmpty(response))
                                {
                                    HubContext.Notify(true, "ExamOn- Alert", $"Email can not be sent due to {response}<br/> हम पासवर्ड नहीं भेज सकते", false, true, false);
                                }
                                else
                                {
                                    RedirectToAction("Go");
                                    HubContext.Notify(true, "ExamOn- Alert", $"We have send your password on your mail.<br/> हमने आपका पासवर्ड आपके मेल पर भेज दिया है।", false, true, false);
                                }
                            }
                            else
                            {
                                HubContext.Notify(false, "", "Please wait, while we are checking your instituion  <br/> कृपया प्रतीक्षा करें।", true, false, false);
                                var _tenantdata = DapperService.GetDapperData<tbltenant>("select top 1 * from tbltenant where id = @tenantToken", new { tenantToken = _logindata.FirstOrDefault().TenantToken }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                if(_tenantdata != null && _tenantdata.Any())
                                {
                                    jsonData.StatusCode = 500;
                                    HubContext.Notify(true, "ExamOn Alert", $"Your login is disabled, Please contact to {_tenantdata.FirstOrDefault().TenantName} <br/> कृपया अपने संस्थान से संपर्क करें।", false, true, false);

                                }
                                else
                                {
                                    jsonData.StatusCode = 500;
                                    HubContext.Notify(true, "ExamOn Alert", "We could not find your instituition, Please contact adminisitator. <br/> कृपया अपने संस्थान से संपर्क करें।", false, true, false);
                                }
                            }
                        }
                        else
                        {
                            HubContext.Notify(true, "ExamOn Alert", "Invalid username and password. <br/> कृपया आपको दिया गया सही उपयोगकर्ता नाम दर्ज करें।", false, true, false);
                        }
                    }
                    else
                    {
                        jsonData.StatusCode = 500;
                        HubContext.Notify(true, "ExamOn Alert", "Invalid User Name <br/>(अमान्य उपयोगकर्ता नाम)", false, true, false);
                    }
                }
            }
            catch (Exception e)
            {
                jsonData.StatusCode = 500;
                jsonData.Error = e.Message;
                HubContext.Notify(true, "ExamOn Alert", e.Message, false, true, false);
            }
            return Json(JsonResponse.JsonResponseData(jsonData), JsonRequestBehavior.AllowGet);
        }
    }
}