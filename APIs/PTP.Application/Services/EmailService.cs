using System.Net;
using System.Net.Mail;

namespace PTP.Application.Services;
public class EmailService : IEmailService
{
    private readonly AppSettings appSettings;
    public EmailService(AppSettings appSettings)
    {
        this.appSettings = appSettings;
    }
    public async Task<bool> SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var _email = appSettings.Email.Email;
            var _epass = appSettings.Email.Password;
            var _dispName = appSettings.Email.DisplayName;
            MailMessage myMessage = new MailMessage();
            myMessage.IsBodyHtml = true;
            myMessage.To.Add(email);
            myMessage.From = new MailAddress(_email!, _dispName);
            myMessage.Subject = subject;
            myMessage.Body = message;
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_email, _epass);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                await smtp.SendMailAsync(myMessage);
            }
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
            throw;
        }
    }
}