using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer
{
    public static class LoginToDashboardMapper
    {
        public static string WhichDashboard(string typeName)
        {
            string dashboard = "";
            switch (typeName.ToUpper())
            {
                case "S":
                    dashboard = "StudDashboard";
                    break;
                case "W":
                    dashboard = "WebAdminDashboard";
                    break;
                case "T":
                    dashboard = "InstructorTeacherDashboard";
                    break;
                case "A":
                    dashboard = "TenantAdminDashboard";
                    break;
                default:
                    dashboard = "";
                    break;
            }
            return dashboard;
        }
    }
}