using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.ViewPostData
{
    public class CreateStudentSignupRequest : GenericModel
    {
        public string tid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int highSchoolMarks { get; set; }
        public string highSchoolCollege { get; set; }
        public int interMarks { get; set; }
        public string interCollege { get; set; }
        public int GradMarks { get; set; }
        public string GradCollege { get; set; }
        public int PostGradMarks { get; set; }
        public string PostGradCollege { get; set; }
        public List<int> shiftIntrest { get; set; }
        public bool isHigh { get; set; }
        public bool Gender { get; set; }
        public bool isInter { get; set; }
        public bool isGrad { get; set; }
        public bool isPostGrad { get; set; }
    }
}