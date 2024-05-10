using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetStudentStaticHistory : GenericModel
    {
        public int Active { get; set; }
        public int NonActive { get; set; }
        public int Blocked { get; set; }
    }
}