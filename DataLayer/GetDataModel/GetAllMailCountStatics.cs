using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetAllMailCountStatics : GenericModel
    {
        public long TotalMails { get; set; }
        public long TotalSuccessMails { get; set; }
        public long TotalFailedMails { get; set; }
    }
}