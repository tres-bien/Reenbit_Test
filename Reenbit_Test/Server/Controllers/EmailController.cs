using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reenbit_Test.Shared;
using MimeKit;
using MailKit.Net.Smtp;

namespace Reenbit_Test.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailModel emailModel)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("deshiro.chi@gmail.com"));
                email.To.Add(MailboxAddress.Parse(emailModel.To));
                email.Subject = emailModel.Subject;
                email.Body = new TextPart(emailModel.Body);

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("deshiro.chi@gmail.com", "nvssaodpvcvyehap");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending email: {ex.Message}");
            }
        }
    }
}
