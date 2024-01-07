using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExamOn.Controllers
{
    public class SignOutController : Controller
    {
        // GET: SignOut
        public ActionResult Go(string isError)
        {
            if (!string.IsNullOrEmpty(isError) && !string.IsNullOrEmpty(ExamOn.MvcApplication.plateformError))
            {
                ViewBag.Error = isError;
                ExamOn.MvcApplication.plateformError = "";
                FormsAuthentication.SignOut();
                return View("index");
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Go", "Login");
        }
    }
}