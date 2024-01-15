using ExamOn.Models;
using ExamOn.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExamOn.Authorize
{
    public class AuthorizeAction : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            
            if (string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login"
                }));
            }
            else
            {
                var userProfile = DapperService.GetDapperData<tbluserProfile>("select top 1 RealName,UserName from tbluserProfile where username = @username", new { @username = AuthorizeService.GetUserName(context.HttpContext.User.Identity.Name) });
                context.Controller.ViewBag.RealName = userProfile.FirstOrDefault().RealName.ToUpperInvariant();
                context.Controller.ViewBag.UserName = userProfile.FirstOrDefault().UserName;
            }
        }
    }
}