using ExamOn.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}