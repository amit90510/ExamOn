using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamOn.Controllers
{
    public class SignOutController : Controller
    {
        // GET: SignOut
        public ActionResult Go(string isError)
        {
            Session["S"] = "";
            Session["A"] = "";
            Session["W"] = "";
            Session["T"] = "";
            string PlateformError = (string)Session["PlateFormError"];
            if (!string.IsNullOrEmpty(isError) && !string.IsNullOrEmpty(PlateformError))
            {
                ViewBag.Error = isError;
                Session["PlateFormError"] = string.Empty;
                return View("index");
            }
            return RedirectToAction("Go", "Login");
        }
    }
}