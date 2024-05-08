using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExamOn.Controllers
{
    public class TenantAdminDashboardController : Controller
    {
        [AuthorizeAction]
        public ActionResult Go()
        {
            return View("Index");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetLicenceExpire()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<tbltenant>("Select top 1 SubscriptionEndDate, DATEDIFF(second,GETDATE(), SubscriptionEndDate) / (60 * 60 * 24) AS SubscriptionEndMessage, RechargeAmount from tbltenant", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                if ((tenantData.FirstOrDefault().SubscriptionEndMessage.Contains("-") || tenantData.FirstOrDefault().SubscriptionEndMessage == "0"))
                {
                    jsonData.Error = $"{tenantData.FirstOrDefault().SubscriptionEndMessage} <br/> (आपका लाइसेंस समाप्त हो गया है, कृपया वेब प्रशासक/ग्राहक सेवा से संपर्क करें।)";
                }
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}