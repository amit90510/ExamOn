﻿using Dapper;
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
    public class ForgotPassWordController : Controller
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
        [Route("ForgotPassWord/{ExamOnForgetPassword}")]
        public JsonResult ForgotMyPassword([System.Web.Http.FromBody] tbllogin loginparams)
        {
            JsonData jsonData = new JsonData();
            try
            {
                using (IDbConnection db = new SqlConnection(DBConnection.GetConnectionString("ExamOn_Master")))
                {
                    string[] token = loginparams.UserName.Split('-');
                    var tenantMasters = db.Query<tblTenantMaster>("select * from tbltenantmaster where TenantUniqueKey = @keyvalue", new { keyvalue = token[0] });
                    if (tenantMasters != null && tenantMasters.Any())
                    {
                        jsonData.StatusCode = 200;
                        var _logindata = DapperService.GetDapperData<tbllogin>("select top 1 * from tbllogin where username = @username", new { username = loginparams.UserName }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                        if (_logindata != null && _logindata.Any())
                        {
                            if (_logindata.FirstOrDefault().Active)
                            {
                                //code for checking maximum limit
                                HubContext.Notify(false, "", $"Please wait, while we are checking your daily limit for mail <br/> कृपया प्रतीक्षा करें, जब तक हम मेल के लिए आपकी दैनिक सीमा की जाँच कर रहे हैं", true, false, false, ViewBag.srKey);
                                var getDailyLimit = DapperService.GetDapperData<tblForgotPasswordMailCounter>("select id from tblForgotPasswordMailCounter where UserName = @username and CreatedDate = CONVERT(date, GETDATE())", new { @username = loginparams.UserName }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                if (getDailyLimit != null && getDailyLimit.Any() && getDailyLimit.Count == Int32.Parse(WebConfigurationManager.AppSettings["RetrievePasswordMaximumLimit"]))
                                {
                                    jsonData.StatusCode = 500;
                                    HubContext.Notify(true, "ExamOn Alert", "Maximum limit reached, <br/> आज अधिकतम सीमा पूरी हो जाने के कारण मेल नहीं भेजा जा सकता", false, true, false, ViewBag.srKey);
                                }
                                else
                                {
                                    //check for tenant subscription also
                                    HubContext.Notify(false, "", $"Please wait, while we are checking your instituition validity <br/> कृपया प्रतीक्षा करें। जबकि हम आपके संस्थान की वैधता की जांच कर रहे हैं", true, false, false, ViewBag.srKey);
                                    var tenantSubscription = DapperService.GetDapperData<tbltenant>("select SubscriptionEndMessage,TenantName from tblTenant where Id = @id and SubscriptionEndDate < GetDate()", new { id = token[0] }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                    if (tenantSubscription != null && tenantSubscription.Any())
                                    {
                                        HubContext.Notify(true, "ExamOn- Alert", $"{tenantSubscription.FirstOrDefault().SubscriptionEndMessage} <br/> (आपका लाइसेंस समाप्त हो गया है, कृपया अपने संस्थान/प्रतिष्ठान {tenantSubscription.FirstOrDefault().TenantName} से संपर्क करें।)", false, true, false, ViewBag.srKey);
                                    }
                                    else
                                    {
                                        HubContext.Notify(false, "", $"Please wait, while we are sending your Pasword on {_logindata.FirstOrDefault().EmailId} <br/> कृपया प्रतीक्षा करें। हम आपका पासवर्ड भेज रहे हैं", true, false, false, ViewBag.srKey);
                                        var response = EmailService.SendEmailResponse(new string[] { _logindata.FirstOrDefault().EmailId }, "Examon - Forgot/Retrieve Password", "पासवर्ड भूल गए/पुनः प्राप्त करें", $"Hello, <br/> your password is (आपका पासवर्ड है) - <b>{ EncryptionDecryption.DecryptString(_logindata.FirstOrDefault().Password)} </b>", tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                        if (!string.IsNullOrEmpty(response))
                                        {
                                            HubContext.Notify(true, "ExamOn- Alert", $"Email can not be sent due to {response}<br/> हम पासवर्ड नहीं भेज सकते", false, true, false, ViewBag.srKey);
                                        }
                                        else
                                        {
                                            jsonData.StatusCode = 1;
                                            //adding record for mailcounter here
                                            var deletePreviousrecords = DapperService.ExecuteQuery($"Delete from tblForgotPasswordMailCounter where CreatedDate = CONVERT(date, GETDATE() - {WebConfigurationManager.AppSettings["DeletePreviousDayPasswordMailCounter"]})", null, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                            var execute = DapperService.ExecuteQuery("Insert into tblForgotPasswordMailCounter(UserName) values(@username)", new { @username = loginparams.UserName }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                            HubContext.Notify(true, "ExamOn- Alert", $"We have send your password on your mail.<br/> हमने आपका पासवर्ड आपके मेल पर भेज दिया है।", false, true, false, ViewBag.srKey);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                HubContext.Notify(false, "", "Please wait, while we are checking your instituion  <br/> कृपया प्रतीक्षा करें।", true, false, false, ViewBag.srKey);
                                var _tenantdata = DapperService.GetDapperData<tbltenant>("select top 1 * from tbltenant where id = @tenantToken", new { tenantToken = _logindata.FirstOrDefault().TenantToken }, tenantMasters.Select(e => e.TenantDBName).FirstOrDefault());
                                if (_tenantdata != null && _tenantdata.Any())
                                {
                                    jsonData.StatusCode = 500;
                                    HubContext.Notify(true, "ExamOn Alert", $"Your Account is disabled, Please contact to {_tenantdata.FirstOrDefault().TenantName} <br/> कृपया अपने संस्थान से संपर्क करें।", false, true, false, ViewBag.srKey);

                                }
                                else
                                {
                                    jsonData.StatusCode = 500;
                                    HubContext.Notify(true, "ExamOn Alert", "We could not find your instituition, Please contact administrator. <br/> कृपया अपने संस्थान से संपर्क करें।", false, true, false, ViewBag.srKey);
                                }
                            }
                        }
                        else
                        {
                            HubContext.Notify(true, "ExamOn Alert", "Invalid User Name<br/> कृपया आपको दिया गया सही उपयोगकर्ता नाम दर्ज करें।", false, true, false, ViewBag.srKey);
                        }
                    }
                    else
                    {
                        jsonData.StatusCode = 500;
                        HubContext.Notify(true, "ExamOn Alert", "Invalid User Name <br/>(अमान्य उपयोगकर्ता नाम)", false, true, false, ViewBag.srKey);
                    }
                }
            }
            catch (Exception e)
            {
                jsonData.StatusCode = 500;
                jsonData.Error = e.Message;
                HubContext.Notify(true, "ExamOn Alert", e.Message, false, true, false, ViewBag.srKey);
            }
            return Json(JsonResponse.JsonResponseData(jsonData), JsonRequestBehavior.AllowGet);
        }
    }
}