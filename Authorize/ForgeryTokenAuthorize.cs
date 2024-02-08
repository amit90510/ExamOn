using ExamOn.SignalRPush;
using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ExamOn.Authorize
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ForgeryTokenAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (request.IsAjaxRequest())
                {
                    var signalRConnectionCookie = request.Headers["__RequestVerificationSRKey"];
                    var signalRConnectionCookieValue = signalRConnectionCookie != null ? signalRConnectionCookie.ToString() : null;
                   // HubContext.Notify(false, "", "Please wait, while we verify your request.", true, false, false, signalRConnectionCookieValue);

                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;
                    try
                    {
                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                        filterContext.Controller.ViewBag.srKey = signalRConnectionCookieValue;
                    }
                    catch
                    {
                        throw new HttpAntiForgeryException("You need to login again, we are not able to verify you. Looks like someone is doing forgery. <br/> (आपको दोबारा लॉग इन करना होगा, हम आपको सत्यापित नहीं कर पा रहे हैं। ऐसा लग रहा है जैसे कोई फर्जीवाड़ा कर रहा है.)");
                       //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "SignOut", action = "Go" }));
                    }
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }
}