using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetTotalExamCountStatics : GenericModel
    {
        public long TotalExam { get; set; }
        public long TotalCompletedExam { get; set; }
        public long TotalNotCompletedExam { get; set; }
    }
}