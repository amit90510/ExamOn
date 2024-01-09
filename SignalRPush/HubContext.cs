using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.SignalRPush
{
    public class HubContext
    {
        public static void Notify(bool isHeader, string header, string body, bool showProgress, bool showFooter, bool closeModal, string connectionId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                hubContext.Clients.Client(connectionId).addNewMessageToPage(isHeader, header, $"<strong>{body}</strong>", showProgress, showFooter, closeModal);
            }
        }
    }
}