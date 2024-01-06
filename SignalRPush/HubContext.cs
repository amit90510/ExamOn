using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.SignalRPush
{
    public static class HubContext
    {
        public static void Notify(bool isHeader, string header, string body, bool showProgress, bool showFooter, bool closeModal)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hubContext.Clients.All.addNewMessageToPage(isHeader, header, $"<strong>{body}</strong>", showProgress, showFooter, closeModal);
        }
    }
}