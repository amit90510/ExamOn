using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.DataLayer.GetDataModel;
using ExamOn.Models;
using ExamOn.ServiceLayer;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

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
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetStudentHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<GetStudentStaticHistory>("SELECT SUM(CASE WHEN tl.Active = 0 THEN 1 ELSE 0 END) AS NonActive, SUM(CASE WHEN tl.Active = 1 THEN 1 ELSE 0 END) AS Active, SUM(CASE WHEN tl.BlockLogin = 1 THEN 1 ELSE 0 END) AS Blocked FROM tbllogin tl INNER JOIN tblloginType ty ON tl.LoginType = ty.id WHERE ty.TypeName = 'Student'", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetStudentEnrollmentHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<GetStudentEnrollmentStaticsHistory>("SELECT Count(id) AS Total, SUM(CASE WHEN status = 'Enrolled' THEN 1 ELSE 0 END) AS Enrolled, SUM(CASE WHEN status = 'Rejected' THEN 1 ELSE 0 END) AS NotEnrolled FROM tblStudentEnrollmentSignUp", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTeacherHistory()
        {
            JsonData jsonData = new JsonData();
            var tenantData = DapperService.GetDapperData<GetStudentStaticHistory>("SELECT SUM(CASE WHEN tl.Active = 0 THEN 1 ELSE 0 END) AS NonActive, SUM(CASE WHEN tl.Active = 1 THEN 1 ELSE 0 END) AS Active, SUM(CASE WHEN tl.BlockLogin = 1 THEN 1 ELSE 0 END) AS Blocked FROM tbllogin tl INNER JOIN tblloginType ty ON tl.LoginType = ty.id WHERE ty.TypeName = 'Teacher'", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) });
            if (tenantData != null && tenantData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = tenantData;
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetClassBatchSectionHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<UserShiftAssociation>("SELECT (SELECT COUNT(id) FROM tblClass where active = 1) AS ClassCount, (SELECT COUNT(id) FROM tblBatch where active = 1) AS BatchCount, (SELECT COUNT(id) FROM tblshift where active = 1) AS ShiftCount;");
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetDatabaseStaticsHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<GetDatabaseSizeStatics>("SELECT DB_NAME(database_id) AS DatabaseName, IsNull((size * 8.0 / 1024),0) AS DBTotalSize, IsNull((size * 8.0 / 1024) - (FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024), 0) AS DBFreeSize, IsNull((FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024),0) AS DBUsedSize FROM sys.master_files WHERE type = 0 and DB_NAME(database_id) = @DataBase", new { Database = AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name) }, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetMailStaticsHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<GetAllMailCountStatics>("SELECT COUNT(*) AS TotalMails, SUM(CASE WHEN sendSuccess = 0 THEN 1 ELSE 0 END) AS TotalFailedMails, SUM(CASE WHEN sendSuccess = 1 THEN 1 ELSE 0 END) AS TotalSuccessMails FROM tblEmailsHistory;", null, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetExamStaticsHistory()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<GetTotalExamCountStatics>("SELECT COUNT(*) AS TotalExam, SUM(CASE WHEN AnyoneCompleted = 1 THEN 1 ELSE 0 END) AS TotalCompletedExam, SUM(CASE WHEN AnyoneCompleted = 0 THEN 1 ELSE 0 END) AS TotalNotCompletedExam FROM tblExam where Active = 1;", null, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<PartialViewResult> GetDetailedSubscriptionPage()
        {
            return PartialView("dashboard_SubscriptionDetailedView");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantSubscriptionDetails()
        {
            JsonData jsonData = new JsonData();
            var tenants = DapperService.GetDapperData<tblTenantMaster>("Select * from tbltenantMaster where tenantUniqueKey = @id", new { id = AuthorizeService.GetDBToken(HttpContext.User.Identity.Name) }, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenants != null && tenants.Any())
            {
                jsonData.StatusCode = 1;
                List<tbltenant> tbltenants = new List<tbltenant>();
                foreach (var tblTenant in tenants)
                {
                    var tenantDetails = DapperService.GetDapperData<tbltenant>("SELECT [id] ,[TenantName], [TenantEmail], [TenantMobile],[SubscriptionEndDate] , [LastRechargeOn] ,[RechargeAmount] FROM [" + tblTenant.TenantDBName + "].dbo.tbltenant;", null, tblTenant.TenantDBName);
                    if (tenantDetails != null && tenantDetails.Any())
                    {
                        tbltenants.Add(tenantDetails.FirstOrDefault());
                    }
                }
                jsonData.Data = tbltenants.OrderByDescending(e => e.SubscriptionEndDate).ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetTenantSubscriptionHistory(string tid)
        {
            JsonData jsonData = new JsonData();
            var tenantDB = DapperService.GetDapperData<tblTenantMaster>("Select [TenantDBName] from tbltenantMaster where TenantUniqueKey = @tid", new { @tid = tid }, WebConfigurationManager.AppSettings["ExamOnMasterDB"]);
            if (tenantDB != null && tenantDB.Any())
            {
                var tenants = DapperService.GetDapperData<tblTenantRechargeHistory>("SELECT [id],[SubscptionStartFrom],[SubscriptionEndAt],[RechargeAmount],[CreatedDate] from [dbo].[tblTenantRechargeHistory] where TID = @td", new { @td = tid }, tenantDB.FirstOrDefault().TenantDBName);
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
        public ActionResult GetTenantSubscriptionHistoryPDF(string tid)
        {
            List<tblTenantRechargeHistory> tblTenantRechargeHistoryList = new List<tblTenantRechargeHistory>();
            var tenants = DapperService.GetDapperData<tblTenantRechargeHistory>("SELECT * from [dbo].[tblTenantRechargeHistory] where ID = @tid", new { @tid = tid }, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (tenants != null && tenants.Any())
            {
                tblTenantRechargeHistoryList = tenants.OrderByDescending(e => e.CreatedDate).ToList();
            }
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            List<IElement> pdfElement = new List<IElement>();
            Paragraph paragraph = new Paragraph("Subscription History", times);
            paragraph.Add(Environment.NewLine);
            paragraph.Add(Environment.NewLine);
            paragraph.Alignment = Element.ALIGN_CENTER;
            pdfElement.Add(paragraph);
            PdfPTable tableMain = new PdfPTable(6);
            float[] widths = new float[] { 10f, 60f, 30f, 30f, 40f, 50f };
            tableMain.SetWidths(widths);
            tableMain.HorizontalAlignment = Element.ALIGN_LEFT;
            tableMain.WidthPercentage = 100;
            tableMain.HeaderRows = 1;
            Phrase phraseSNo = new Phrase("SNO", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));            
            Phrase phrasePItem = new Phrase("Purchase Item", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));            
            Phrase phraseSubStart = new Phrase("Subscription Start Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));            
            Phrase phraseSubEnd = new Phrase("Subscription End Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));            
            Phrase phraseRecAmt = new Phrase("Recharge Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));            
            Phrase phraseDate = new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));            
            tableMain.AddCell(phraseSNo);
            tableMain.AddCell(phrasePItem);
            tableMain.AddCell(phraseSubStart);
            tableMain.AddCell(phraseSubEnd);
            tableMain.AddCell(phraseRecAmt);
            tableMain.AddCell(phraseDate);
            //add some empty row in pdfptable
            int sno = 1;
            foreach (var item in tblTenantRechargeHistoryList)
            {
                tableMain.AddCell(new Paragraph(sno.ToString(), times));
                tableMain.AddCell(new Paragraph("ExamOn - Tenant Subscription Renew/Purchase.", times));
                tableMain.AddCell(new Paragraph(item.SubscptionStartFrom.ToString("dd-MM-yyyy"), times));
                tableMain.AddCell(new Paragraph(item.SubscriptionEndAt.ToString("dd-MM-yyyy"), times));
                tableMain.AddCell(new Paragraph(item.RechargeAmount.ToString(), times));
                tableMain.AddCell(new Paragraph(item.CreatedDate.ToString(), times));
                sno++;
            }
            pdfElement.Add(tableMain);
            PdfPTable tableTotal = new PdfPTable(2);
            float[] widthsTotal = new float[] { 59f, 41f };
            tableTotal.SetWidths(widthsTotal);
            tableTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableTotal.WidthPercentage = 100;
            tableTotal.HeaderRows = 0;
            Phrase phraseTotal = new Phrase("Total Amount", times);
            double totalAmount = 0;
            var tenantsSum = DapperService.GetDapperData<tblTenantRechargeHistory>("SELECT top 1 Sum(RechargeAmount) as RechargeAmount from [dbo].[tblTenantRechargeHistory] where ID = @tid", new { @tid = tid }, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (tenantsSum != null && tenantsSum.Any())
            {
                totalAmount = tenantsSum.FirstOrDefault().RechargeAmount;
            }
            Phrase phraseTotalValue = new Phrase(totalAmount.ToString() + " INR", times);
            tableTotal.AddCell(phraseTotal);
            tableTotal.AddCell(phraseTotalValue);
            pdfElement.Add(tableTotal);
            iTextPdfExportService iTextPdfExportService = new iTextPdfExportService();
            byte[] pdfData = iTextPdfExportService.ExportPdfData(pdfElement);
            //handle null scenario in below code
            if (pdfData != null)
            {
                return File(pdfData, "application/pdf", "SubscriptionHistory.pdf");
            }
            else
            {
                return Content("ExamOn !! Alert - NO Pdf data.");
            }
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public ActionResult GetTenantSubscriptionFullHistoryPDF()
        {
            List<tblTenantRechargeHistory> tblTenantRechargeHistoryList = new List<tblTenantRechargeHistory>();
            var tenants = DapperService.GetDapperData<tblTenantRechargeHistory>("SELECT * from [dbo].[tblTenantRechargeHistory] order by createdDate desc", null, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (tenants != null && tenants.Any())
            {
                tblTenantRechargeHistoryList = tenants.OrderByDescending(e => e.CreatedDate).ToList();
            }
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            iTextSharp.text.Font times = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            List<IElement> pdfElement = new List<IElement>();
            Paragraph paragraph = new Paragraph("Subscription History", times);
            paragraph.Add(Environment.NewLine);
            paragraph.Add(Environment.NewLine);
            paragraph.Alignment = Element.ALIGN_CENTER;
            pdfElement.Add(paragraph);
            PdfPTable tableMain = new PdfPTable(6);
            float[] widths = new float[] { 10f, 60f, 30f, 30f, 40f, 50f };
            tableMain.SetWidths(widths);
            tableMain.HorizontalAlignment = Element.ALIGN_LEFT;
            tableMain.WidthPercentage = 100;
            tableMain.HeaderRows = 1;
            Phrase phraseSNo = new Phrase("SNO", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
            Phrase phrasePItem = new Phrase("Purchase Item", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
            Phrase phraseSubStart = new Phrase("Subscription Start Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
            Phrase phraseSubEnd = new Phrase("Subscription End Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
            Phrase phraseRecAmt = new Phrase("Recharge Amount", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
            Phrase phraseDate = new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
            tableMain.AddCell(phraseSNo);
            tableMain.AddCell(phrasePItem);
            tableMain.AddCell(phraseSubStart);
            tableMain.AddCell(phraseSubEnd);
            tableMain.AddCell(phraseRecAmt);
            tableMain.AddCell(phraseDate);
            //add some empty row in pdfptable
            int sno = 1;
            foreach (var item in tblTenantRechargeHistoryList)
            {
                tableMain.AddCell(new Paragraph(sno.ToString(), times));
                tableMain.AddCell(new Paragraph("ExamOn - Tenant Subscription Renew/Purchase.", times));
                tableMain.AddCell(new Paragraph(item.SubscptionStartFrom.ToString("dd-MM-yyyy"), times));
                tableMain.AddCell(new Paragraph(item.SubscriptionEndAt.ToString("dd-MM-yyyy"), times));
                tableMain.AddCell(new Paragraph(item.RechargeAmount.ToString(), times));
                tableMain.AddCell(new Paragraph(item.CreatedDate.ToString(), times));
                sno++;
            }
            pdfElement.Add(tableMain);
            PdfPTable tableTotal = new PdfPTable(2);
            float[] widthsTotal = new float[] { 59f, 41f };
            tableTotal.SetWidths(widthsTotal);
            tableTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableTotal.WidthPercentage = 100;
            tableTotal.HeaderRows = 0;
            Phrase phraseTotal = new Phrase("Total Amount", times);
            double totalAmount = 0;
            var tenantsSum = DapperService.GetDapperData<tblTenantRechargeHistory>("SELECT top 1 Sum(RechargeAmount) as RechargeAmount from [dbo].[tblTenantRechargeHistory]", null, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (tenantsSum != null && tenantsSum.Any())
            {
                totalAmount = tenantsSum.FirstOrDefault().RechargeAmount;
            }
            Phrase phraseTotalValue = new Phrase(totalAmount.ToString() + " INR", times);
            tableTotal.AddCell(phraseTotal);
            tableTotal.AddCell(phraseTotalValue);
            pdfElement.Add(tableTotal);
            iTextPdfExportService iTextPdfExportService = new iTextPdfExportService();
            byte[] pdfData = iTextPdfExportService.ExportPdfData(pdfElement);
            //handle null scenario in below code
            if (pdfData != null)
            {
                return File(pdfData, "application/pdf", "SubscriptionHistory.pdf");
            }
            else
            {
                return Content("ExamOn !! Alert - NO Pdf data.");
            }
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<PartialViewResult> GetRegistredStudentsPage()
        {
            return PartialView("dashboard_AvailableRegisteredStudents");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetloadRegisteredStudentsGrid()
        {
            JsonData jsonData = new JsonData();
            var studentData = DapperService.GetDapperData<GetStudentInformationdata>("select ROW_NUMBER() OVER (ORDER BY tl.username) Id, tp.UserName, tp.RealName, tp.address, tp.City,tp.State, EmailId,Mobile, case when Active = 0 then 'No' else 'Yes' end as Active, case when BlockLogin = 0 then 'No' else 'Yes' end as BlockLogin, CreatedOn, tt.TypeName from tbllogin tl\r\n  inner join tblloginType tt on tl.LoginType = tt.id and tt.Type = 'S' \r\n  inner join tbluserProfile tp on tl.UserName = tp.UserName", null, AuthorizeService.GetUserDBName(HttpContext.User.Identity.Name));
            if (studentData != null && studentData.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = studentData.OrderByDescending(e => e.CreatedOn).ToList();
                using (StreamReader reader = new StreamReader(Server.MapPath("~/DataLayer/States.json")))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var stateInfo = serializer.Deserialize<dynamic>(reader.ReadToEnd());
                    foreach (var studentitem in studentData.Where(e => !string.IsNullOrEmpty(e.State)))
                    {
                        stateInfo.TryGetValue(studentitem.State, out dynamic stateName);
                        studentitem.State = stateName;
                    }
                }
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}