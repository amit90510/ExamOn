using ExamOn.Models;
using ExamOn.Utility;
using System;
using System.Web.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ExamOn.ServiceLayer
{
    public static class EmailService
    {
        public static void SendEmail(string[] Toaddress, string subject, string bodyHeader, string bodyText)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    var _data = DapperService.GetDapperData<tblEmailCrediential>("select top 1 * from tblEmailCredientials");
                    if (_data != null && _data.Any())
                    {
                        mail.From = new MailAddress(_data.FirstOrDefault().EmailFromAddress);
                        foreach (var addr in Toaddress)
                        {
                            mail.To.Add(addr);
                        }
                        mail.Subject = subject;
                        StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/content/Template/EmailTemplate.html"));
                        string templatebody = sr.ReadToEnd();
                        sr.Close();
                        templatebody = templatebody.Replace("{title}", bodyHeader).Replace("{text}", bodyText).Replace("{HrefLink}", HttpContext.Current.Request.Url.OriginalString).Replace("{Version}", WebConfigurationManager.AppSettings["ExamOnVersion"].ToString());
                        mail.Body = templatebody;
                        mail.IsBodyHtml = true;
                        using (SmtpClient smtp = new SmtpClient(WebConfigurationManager.AppSettings["SMTPClient"].ToString(), Int32.Parse(WebConfigurationManager.AppSettings["SMTPPort"].ToString())))
                        {
                            smtp.Credentials = new NetworkCredential(_data.FirstOrDefault().EmailFromAddress, EncryptionDecryption.DecryptString(_data.FirstOrDefault().Password));
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch{ }
        }

        public static async void SendEmailAsync(string[] Toaddress, string subject, string bodyHeader, string bodyText)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    var _data = DapperService.GetDapperData<tblEmailCrediential>("select top 1 * from tblEmailCredientials");
                    if (_data != null && _data.Any())
                    {
                        mail.From = new MailAddress(_data.FirstOrDefault().EmailFromAddress);
                        foreach (var addr in Toaddress)
                        {
                            mail.To.Add(addr);
                        }
                        mail.Subject = subject;
                        StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/content/Template/EmailTemplate.html"));
                        string templatebody = sr.ReadToEnd();
                        sr.Close();
                        templatebody = templatebody.Replace("{title}", bodyHeader).Replace("{text}", bodyText).Replace("{HrefLink}", HttpContext.Current.Request.Url.OriginalString).Replace("{Version}", System.Web.Configuration.WebConfigurationManager.AppSettings["ExamOnVersion"].ToString());
                        mail.Body = templatebody;
                        mail.IsBodyHtml = true;
                        using (SmtpClient smtp = new SmtpClient(WebConfigurationManager.AppSettings["SMTPClient"].ToString(), Int32.Parse(WebConfigurationManager.AppSettings["SMTPPort"].ToString())))
                        {
                            smtp.Credentials = new NetworkCredential(_data.FirstOrDefault().EmailFromAddress, EncryptionDecryption.DecryptString(_data.FirstOrDefault().Password));
                            smtp.EnableSsl = true;
                            await smtp.SendMailAsync(mail);
                        }
                    }
                }
            }
            catch { }
        }

        public static string SendEmailResponse(string[] Toaddress, string subject, string bodyHeader, string bodyText, string dbName="")
        {
            string isError = string.Empty;
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    var _data = DapperService.GetDapperData<tblEmailCrediential>("select top 1 * from tblEmailCredientials",null, dbName);
                    if (_data != null && _data.Any())
                    {
                        mail.From = new MailAddress(_data.FirstOrDefault().EmailFromAddress);
                        foreach (var addr in Toaddress)
                        {
                            mail.To.Add(addr);
                        }
                        mail.Subject = subject;
                        StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/content/Template/EmailTemplate.html"));
                        string templatebody = sr.ReadToEnd();
                        sr.Close();
                        templatebody = templatebody.Replace("{title}", bodyHeader).Replace("{text}", bodyText).Replace("{HrefLink}", HttpContext.Current.Request.Url.OriginalString).Replace("{Version}", WebConfigurationManager.AppSettings["ExamOnVersion"].ToString());
                        mail.Body = templatebody;
                        mail.IsBodyHtml = true;
                        using (SmtpClient smtp = new SmtpClient(WebConfigurationManager.AppSettings["SMTPClient"].ToString(), Int32.Parse(WebConfigurationManager.AppSettings["SMTPPort"].ToString())))
                        {
                            smtp.Credentials = new NetworkCredential(_data.FirstOrDefault().EmailFromAddress, EncryptionDecryption.DecryptString(_data.FirstOrDefault().Password));
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch (Exception TT) {
                isError = TT.Message;
            }

            return isError;
        }

        public static void CreateEmailHistory(string fromAddress, string[] toAddress, string subject, string mailBody, string dbName)
        {

        }
        public static void UpdateEmailHistory(string fromAddress, string[] toAddress, string subject, string mailBody, string dbName, )
        {

        }
    }
}