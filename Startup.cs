using ExamOn.SignalRPush;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Optimization;

[assembly: OwinStartup(typeof(ExamOn.Startup))]

namespace ExamOn
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }

    }
}
