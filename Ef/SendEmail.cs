using algoriza_internship_288.Domain.AccountModels;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Repository
{
    public static class SendEmailExtension
    {
        public static bool SendEmail(this Login login , string userEmail)
        {
            if (login is null && userEmail is null)
                return false;
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("Abdelazizmahmoudkarm@gmail.com"));
            mail.To.Add(MailboxAddress.Parse(userEmail));
            mail.Subject = "ClinicApp";
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<div> Your UserName : <b> {login.UserName}</b> </div>" +
                                                               $"<br>" +
                                                               $"<div> Your Password : <b> {login.Password}</b> </div>"
            };
            using var stmp = new SmtpClient();
            stmp.Connect("smtp.gmail.com", 587);
            stmp.Authenticate("Abdelazizmahmoudkarm@gmail.com", "mwcwnhaxuryhclim");
            stmp.Send(mail);
            stmp.Disconnect(true);
            return true;
        }

        public static bool SendExceptionsToAdmin(string message)
        {
            if (message is null)
                return false;
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse("Abdelazizmahmoudkarm@gmail.com"));
            mail.To.Add(MailboxAddress.Parse("abdelazizmahmoukarm@gmail.com"));
            mail.Subject = "ClinicApp";
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = $"Exceptions  {message} at {DateTime.Now}",
            };
            using var stmp = new SmtpClient();
            stmp.Connect("smtp.gmail.com", 587);
            stmp.Authenticate("Abdelazizmahmoudkarm@gmail.com", "mwcwnhaxuryhclim");
            stmp.Send(mail);
            stmp.Disconnect(true);
            return true;
        }
    }
}
