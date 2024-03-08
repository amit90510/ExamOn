using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetAllTenantDeepInformation : GenericModel
    {
        public string Id { get; set; }
        public string TenantName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string TotalStudent { get; set; }
        public string TotalActiveStudent { get; set; }
        public string TotalBlockStudent { get; set; }
        public string TotalInstructor { get; set; }
        public string TotalAdmin { get; set; }
        public string TotalExam { get; set; }
    }
}