using ExamOn.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamOn.Models
{
    public class tblTenantMaster : GenericModel
    {
        [Key]
        public long Id { get; set; }
        public string TenantDBName { get; set; }
        public string TenantUniqueKey { get; set; }
    }
}