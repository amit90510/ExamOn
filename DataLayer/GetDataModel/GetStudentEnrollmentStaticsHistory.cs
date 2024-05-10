using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetStudentEnrollmentStaticsHistory : GenericModel
    {
        public int Total { get; set; }
        public int Enrolled { get; set; }
        public int NotEnrolled { get; set; }
    }
}