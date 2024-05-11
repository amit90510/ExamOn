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
            string Mailguid = string.Empty;
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
                            //Mailguid = CreateEmailHistory(_data.FirstOrDefault().EmailFromAddress, Toaddress, subject, templatebody, "SendEmail");
                            smtp.Credentials = new NetworkCredential(_data.FirstOrDefault().EmailFromAddress, EncryptionDecryption.DecryptString(_data.FirstOrDefault().Password));
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                            if (!string.IsNullOrEmpty(Mailguid))
                            {
                               // UpdateEmailHistory(Mailguid, string.Empty);
                            }
                        }
                    }
                }
            }
            catch(Exception TT){
                if(!string.IsNullOrEmpty(Mailguid))
                {
                   // UpdateEmailHistory(Mailguid, !string.IsNullOrEmpty(TT.Message) ? TT.Message : "MailSending Failed without an exception.");
                }
            }
        }

        public static async void SendEmailAsync(string[] Toaddress, string subject, string bodyHeader, string bodyText)
        {
            string Mailguid = string.Empty;
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
                            //Mailguid = CreateEmailHistory(_data.FirstOrDefault().EmailFromAddress, Toaddress, subject, templatebody, "SendEmailAsync");
                            smtp.Credentials = new NetworkCredential(_data.FirstOrDefault().EmailFromAddress, EncryptionDecryption.DecryptString(_data.FirstOrDefault().Password));
                            smtp.EnableSsl = true;
                            await smtp.SendMailAsync(mail);
                            if (!string.IsNullOrEmpty(Mailguid))
                            {
                               // UpdateEmailHistory(Mailguid, string.Empty, dbName);
                            }
                        }
                    }
                }
            }
            catch (Exception TT)
            {
                if (!string.IsNullOrEmpty(Mailguid))
                {
                   // UpdateEmailHistory(Mailguid, !string.IsNullOrEmpty(TT.Message) ? TT.Message : "MailSending Failed without an exception.");
                }
            }
        }

        public static string SendEmailResponse(string[] Toaddress, string subject, string bodyHeader, string bodyText, string dbName="")
        {
            string isError = string.Empty;
            string Mailguid = string.Empty;
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
                            Mailguid = CreateEmailHistory(_data.FirstOrDefault().EmailFromAddress, Toaddress, subject, templatebody, "SendEmailResponse", dbName);
                            smtp.Credentials = new NetworkCredential(_data.FirstOrDefault().EmailFromAddress, EncryptionDecryption.DecryptString(_data.FirstOrDefault().Password));
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch (Exception TT) {
                isError = TT.Message;
                if (!string.IsNullOrEmpty(Mailguid))
                {
                    UpdateEmailHistory(Mailguid, !string.IsNullOrEmpty(TT.Message)? TT.Message : "MailSending Failed without an exception.", dbName);
                }
            }

            return isError;
        }

        public static string CreateEmailHistory(string fromAddress, string[] toAddress, string subject, string mailBody, string fromMethod, string dbname)
        {
            try
            {
                var guidMail = Guid.NewGuid();
                DapperService.ExecuteQuery("Insert into tblEmailsHistory([MailFrom], [MailTo] , [Subject] , [MailBody], [SendSuccess], MailGuid, FromMethodName) values(@MailFrom, @MailTo , @Subject , @MailBody, @SendSuccess, @mailGuid, @fromMethod)", new
                {
                    @MailFrom = fromAddress,
                    @MailTo = string.Join(";", toAddress.SelectMany(s => s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))),
                    @Subject = subject,
                    @MailBody = mailBody,
                    @SendSuccess = 0,
                    @mailGuid = guidMail.ToString(),
                    @fromMethod = fromMethod
                }, dbname).ConfigureAwait(true);
                return guidMail.ToString();
            }
            catch {
                return string.Empty;
            }
        }

        public static void UpdateEmailHistory(string MailGUID,string error, string dbName)
        {
            try
            {
                DapperService.ExecuteQuery($"Update tblEmailsHistory set Error = @error, SendSuccess = @SendSuccess, SendTryAt = GetDate(), LastUpdate = GetDate() where MailGuid = @mailGuid", new
                {
                    @error = !string.IsNullOrEmpty(error) ? error : string.Empty,
                    @SendSuccess = !string.IsNullOrEmpty(error) ? 0 : 1,
                    @mailGuid = MailGUID
                }, dbName).ConfigureAwait(true);
            }
            catch { }
        }
    }
}