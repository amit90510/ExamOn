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