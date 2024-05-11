using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class UserShiftAssociation
    {
        public string ClassName { get; set; }
        public string Batch { get; set; }
        public string Shift { get; set; }

        public long ShiftID { get; set; }

        public long ClassCount { get; set; }
        public long BatchCount { get; set; }
        public long ShiftCount { get; set; }
    }
}