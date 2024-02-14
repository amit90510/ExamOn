using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}