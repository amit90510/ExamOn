using ExamOn.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamOn.DataLayer.ViewPostData
{
    public class CreateUserLogins
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TenantToken { get; set; }
        public string ProfileName { get; set; }
        public string Email { get; set; }
        public int LoginType { get; set; }
        public string EncryptPassword 
        { 
            get { return EncryptionDecryption.EncryptString(Password); }
            set { }
        }
    }
}