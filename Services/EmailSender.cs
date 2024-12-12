using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using MimeKit;
using MailKit.Security;
// Create a service to handle email sending
namespace NikeStyle.Services;
public class EmailSender(IConfiguration configuration) : IEmailSender
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("NikeStyle",emailSettings["SenderEmail"]));
                message.To.Add(new MailboxAddress("",email));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder {HtmlBody = htmlMessage};
                message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            // Connect to Gmail's SMTP server with STARTTLS on port 587
        await client.ConnectAsync("smtp.gmail.com",587,SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
        }
    }
}