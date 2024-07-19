using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetStudentInformationdata : GenericModel
    {
        public double Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Mobile { get; set; }
        public string EmailId { get; set; }
        public string Active { get; set; }
        public string BlockLogin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}