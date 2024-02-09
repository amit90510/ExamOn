using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.GetDataModel
{
    public class UserTypeAccessPermission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string Route { get; set; }
        public bool IsActive { get; set; } = false;
    }
}