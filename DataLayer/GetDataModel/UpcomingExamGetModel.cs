using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class UpcomingExamGetModel
    {
        public string ExamName { get; set; }
        public DateTime StartExam { get; set; }
        public DateTime updatedOn { get; set; }
        public DateTime EntryAllowedTill { get; set; }

        public int SectionCount { get; set; }
    }
}