using ExamOn.Authorize;
using ExamOn.DataLayer;
using ExamOn.DataLayer.GetDataModel;
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
    public class InstructorTeacherDashboardController : Controller
    {
        [AuthorizeAction]
        public ActionResult Go()
        {
            return View("Index");
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetActiveTeacherAssociatedShift()
        {
            JsonData jsonData = new JsonData();
            var userShiftProfile = DapperService.GetDapperDataDynamic<UserShiftAssociation>("select top 5 tc.ClassName, bt.Batch, sf.Shift  from tblTeacherInstructorShifts usf inner join tblshift sf on usf.shiftId = sf.id and usf.active = 1 and sf.Active = 1 and usf.Userid = @userID inner join tblbatch bt on sf.batch = bt.id and bt.Active =1 inner join tblclass tc on bt.class = tc.id and tc.Active = 1", new { userID = ViewBag.LoginId });
            if (userShiftProfile != null && userShiftProfile.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = userShiftProfile.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAction]
        [ForgeryTokenAuthorize]
        public async Task<JsonResult> GetUpcomingExamsSetByTeacher()
        {
            JsonData jsonData = new JsonData();
            var upcomingExamGetModel = DapperService.GetDapperDataDynamic<UpcomingExamGetModel>("select  top 5 e.ExamName, e.StartExam, e.updatedOn, e.EntryAllowedTill, count(sec.id) as 'SectionCount' from tblexamStudents es inner join tblexam e on es.exam = e.id and e.Active = 1 and e.SetByUserId = @userID left join tblexamSections sec on e.id = sec.exam group by e.ExamName, e.StartExam, e.updatedOn, e.EntryAllowedTill", new { userID = ViewBag.LoginId });
            if (upcomingExamGetModel != null && upcomingExamGetModel.Any())
            {
                jsonData.StatusCode = 1;
                jsonData.Data = upcomingExamGetModel.ToList();
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}