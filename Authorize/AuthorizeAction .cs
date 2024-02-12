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
                var userProfile = DapperService.GetDapperData<tbluserProfile>("select top 1 tu.RealName,tu.UserName,tl.LoginType from tbluserProfile tu inner join tblLogin tl on tu.username = tl.username and tl.username = @username", new { @username = AuthorizeService.GetUserName(context.HttpContext.User.Identity.Name) });
                if (userProfile != null && userProfile.Any())
                {
                    context.Controller.ViewBag.RealName = userProfile.FirstOrDefault().RealName.ToUpperInvariant();
                    context.Controller.ViewBag.UserName = userProfile.FirstOrDefault().UserName;
                    if (userProfile.FirstOrDefault().LoginType.HasValue && context.ActionDescriptor.ActionName.EndsWith("Go", StringComparison.OrdinalIgnoreCase))
                    {
                        var UserAccess = DapperService.GetDapperData<tblUserTypeAccess>("select top 1 id from tblUserTypeAccess where TypeId = @typeID and UserPath = @path", new { @typeID = userProfile.FirstOrDefault().LoginType.Value, @path = context.ActionDescriptor.ControllerDescriptor.ControllerType.Name + "/" + context.ActionDescriptor.ActionName });
                        if(UserAccess == null || !UserAccess.Any())
                        {
                            throw new HttpAntiForgeryException("You are not authorized to use this service as you don't have permission for this page. (आपको दोबारा लॉग इन करना होगा, आप इस सेवा का उपयोग करने के लिए अधिकृत नहीं हैं क्योंकि आपके पास इस पृष्ठ के लिए अनुमति नहीं है.)");
                        }
                    }
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Login"
                    }));
                }
            }
        }
    }
}