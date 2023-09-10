using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using Reenbit_Test.Shared;

public static class SendEmailFunction
{
    [FunctionName("SendEmail")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var emailModel = JsonConvert.DeserializeObject<EmailModel>(requestBody);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("deshiro.chi@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailModel!.To));
            email.Subject = emailModel.Subject;
            email.Body = new TextPart(emailModel.Body);

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync("deshiro.chi@gmail.com", "nvssaodpvcvyehap");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return new OkObjectResult("Email sent successfully");
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"Error sending email: {ex.Message}");
        }
    }
}
