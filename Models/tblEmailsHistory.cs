//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExamOn.Models
{
    using ExamOn.DataLayer;
    using System;
    using System.Collections.Generic;
    
    public partial class tblEmailsHistory : GenericModel
    {
        public long Id { get; set; }
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public string FromMethodName { get; set; }
        public int Attachments { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime SendTryAt { get; set; }
        public string Error { get; set; }
        public bool SendSuccess { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public string MailGuid { get; set; }
    }
}
