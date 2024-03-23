using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class GetTenantMasterMoreInformation : GenericModel
    {
        [Key]
        public string Id { get; set; }
        public string TenantDBName { get; set; }
        public string TenantUniqueKey { get; set; }
        public string TenantAddress { get; set; }
        public string TenantEmail { get; set; }
        public string TenantName { get; set; }


    }
}