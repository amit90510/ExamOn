using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.ViewPostData
{
    public class UpdateProfile
    {
        public string UserName { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}