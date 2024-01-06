using ExamOn.SignalRPush;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamOn.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Go()
        {
            return View("Index");
        }

        public JsonResult LongRunningProcess()
        {
            HubContext.Notify(true, "yes, this is working", "this is working too animate css", true, true, false);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}