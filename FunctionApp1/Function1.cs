using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using Reenbit_Test.Shared;
using Reenbit_Test.Services;
using System.Text;

namespace FunctionApp1
{
    public class EmailSenderFunction
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger<EmailSenderFunction> _logger;

        public EmailSenderFunction(SmtpClient smtpClient, ILogger<EmailSenderFunction> logger)
        {
            _smtpClient = smtpClient;
            _logger = logger;
        }

        [Function("SendEmailWithUrl")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var emailModel = JsonConvert.DeserializeObject<EmailModel>(requestBody);

                if (emailModel == null)
                {
                    return new BadRequestResult();
                }

                var azureBlobService = new AzureBlobService();
                var filesWithSasTokens = await azureBlobService.ListAsync();

                var emailBody = new StringBuilder();
                emailBody.AppendLine("File(s) uploaded successfully. Click the links below to access the files:");

                foreach (var file in filesWithSasTokens)
                {
                    emailBody.AppendLine($"<a href=\"{file.Uri}\">{file.Name}</a>");
                }

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("deshiro.chi@gmail.com"));
                email.To.Add(MailboxAddress.Parse(emailModel.To));
                email.Subject = emailModel.Subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailBody.ToString() };

                await _smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await _smtpClient.AuthenticateAsync("deshiro.chi@gmail.com", "nvssaodpvcvyehap");
                await _smtpClient.SendAsync(email);
                await _smtpClient.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully.");
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return new BadRequestObjectResult($"Error sending email: {ex.Message}");
            }
        }
    }
}
