//using Newtonsoft.Json;
//using System.Text;
//using Moq;
//using FunctionApp1;
//using MailKit.Net.Smtp;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.Extensions.Logging;
//using MimeKit;
//using Reenbit_Test.Services;
//using Reenbit_Test.Shared;
//using Reenbit_Test;
//using MailKit.Security;
//using Microsoft.AspNetCore.Http;
//using Azure.Storage.Blobs.Models;
//using System.Reflection;
//using System.Net.Mail;

//namespace Integration_test
//{
//    public class UnitTest1
//    {
//        //[Fact]
//        //public async Task Run_ValidRequest_ReturnsOkResult()
//        //{
//        //    // Arrange
//        //    var smtpClientMock = new Mock<SmtpClient>();
//        //    var loggerMock = new Mock<ILogger<EmailSenderFunction>>();
//        //    var azureBlobServiceMock = new Mock<IAzureBlobService>();

//        //    var emailSenderFunction = new EmailSenderFunction(smtpClientMock.Object, loggerMock.Object);
//        //    var emailModel = new EmailModel
//        //    {
//        //        To = "recipient@example.com",
//        //        Subject = "Test Email",
//        //    };

//        //    var requestBody = JsonConvert.SerializeObject(emailModel);
//        //    var req = CreateTestHttpRequestData(requestBody);

//        //    // Act
//        //    var result = await emailSenderFunction.Run(req, Mock.Of<FunctionContext>());

//        //    // Assert
//        //    Assert.IsType<OkResult>(result);

//        //    smtpClientMock.Verify(smtp => smtp.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), default));
//        //    smtpClientMock.Verify(smtp => smtp.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), default));
//        //    smtpClientMock.Verify(smtp => smtp.SendAsync(It.IsAny<MimeMessage>(), default, default));
//        //    smtpClientMock.Verify(smtp => smtp.DisconnectAsync(true, default));
//        //}

//        public class BlobInfo
//        {
//            public string Name { get; set; } = string.Empty;
//            public Uri? Uri { get; set; }
//        }


//        //[Fact]
//        //public async Task Run_ValidRequest_ReturnsOkResult()
//        //{
//        //    // Arrange
//        //    var loggerMock = new Mock<ILogger>();
//        //    var httpRequestMock = new DefaultHttpRequest(new DefaultHttpContext());
//        //    var emailModel = new EmailModel
//        //    {
//        //        To = "test@example.com",
//        //        Subject = "Test Subject",
//        //    };

//        //    var mockBlob1 = new BlobInfo
//        //    {
//        //        Name = "file1.txt",
//        //        Uri = new Uri("https://yourstorageaccount.blob.core.windows.net/container/file1.txt"),
//        //    };

//        //    var azureBlobServiceMock = new Mock<AzureBlobService>();
//        //    azureBlobServiceMock.Setup(s => s.ListAsync()).ReturnsAsync(mockBlob1);

//        //    var smtpClientMock = new Mock<ISmtpClient>();
//        //    smtpClientMock.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>()))
//        //        .Returns(Task.CompletedTask);
//        //    smtpClientMock.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
//        //        .Returns(Task.CompletedTask);
//        //    smtpClientMock.Setup(s => s.SendAsync(It.IsAny<MimeMessage>()))
//        //        .Returns(Task.CompletedTask);
//        //    smtpClientMock.Setup(s => s.DisconnectAsync(It.IsAny<bool>()))
//        //        .Returns(Task.CompletedTask);

//        //    var function = new SendEmailFunction(loggerMock.Object, azureBlobServiceMock.Object, smtpClientMock.Object);

//        //    // Act
//        //    var result = await function.Run(httpRequestMock, emailModel);

//        //    // Assert
//        //    Assert.IsType<OkResult>(result);
//        //}

//        public class SendEmailFunctionTests
//        {
//            [Fact]
//            public async Task Run_ValidRequest_ReturnsOkResult()
//            {
//                // Arrange
//                var loggerMock = new Mock<ILogger<EmailSenderFunction>>();
//                var httpRequestMock = new Mock<HttpRequestData>();
//                httpRequestMock.SetupGet(r => r.Body).Returns(new MemoryStream());
//                var smtpClient = new MailKit.Net.Smtp.SmtpClient();

//                var emailModel = new EmailModel
//                {
//                    To = "test@example.com",
//                    Subject = "Test Subject",
//                };

//                var mockBlob1 = new BlobInfo
//                {
//                    Name = "file1.txt",
//                    Uri = new Uri("https://yourstorageaccount.blob.core.windows.net/container/file1.txt"),
//                };

//                var mockBlob2 = new BlobInfo
//                {
//                    Name = "file2.txt",
//                    Uri = new Uri("https://yourstorageaccount.blob.core.windows.net/container/file2.txt"),
//                };

//                var mockBlobList = new List<BlobInfo>
//                {
//                    mockBlob1,
//                    mockBlob2,
//                };

//                var azureBlobServiceMock = new Mock<IAzureBlobService>();
//                azureBlobServiceMock.Setup(s => s.ListAsync())
//                    .ReturnsAsync(mockBlobList.Select(b => new BlobDto
//                    {
//                        Name = b.Name,
//                        Uri = b.Uri!.ToString(),
//                    }).ToList());

//                //var email = new MimeMessage();
//                //email.From.Add(MailboxAddress.Parse("SMTP_USERNAME_TEST"));
//                //email.To.Add(MailboxAddress.Parse(emailModel.To));
//                //email.Subject = emailModel.Subject;
//                //email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "Success" };

//                //string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST_TEST")!;
//                ////int smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")!)!;
//                //string smtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME_TEST")!;
//                //string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD_TEST")!;

//                //await smtpClient.ConnectAsync(smtpHost, 587, MailKit.Security.SecureSocketOptions.StartTls);
//                //await smtpClient.AuthenticateAsync(smtpUsername, smtpPassword);
//                //await smtpClient.SendAsync(email);
//                //await smtpClient.DisconnectAsync(true);

//                var cancellationTokenCaptured = CancellationToken.None;

//                var smtpClientMock = new Mock<ISmtpClient>();
//                smtpClientMock.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>()))
//                    .Returns(Task.CompletedTask);
//                smtpClientMock.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
//                    .Returns(Task.CompletedTask);
//                smtpClientMock.Setup(s => s.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()))
//                    .Callback<MimeMessage, CancellationToken>((message, cancellationToken) =>
//                    {
//                        // Capture the provided CancellationToken
//                        cancellationTokenCaptured = cancellationToken;
//                    })
//                    .Returns((Task<string>)Task.CompletedTask);
//                smtpClientMock.Setup(s => s.DisconnectAsync(true, It.IsAny<CancellationToken>()))
//                    .Returns(Task.CompletedTask);

//                var function = new EmailSenderFunction(smtpClient, loggerMock.Object);

//                // Act
//                var result = await function.Run(httpRequestMock.Object);

//                // Assert
//                Assert.IsType<OkResult>(result);
//                //smtpClientMock.Verify(smtp => smtp.SendAsync(It.IsAny<MimeMessage>(), cancellationTokenCaptured));
//            }
//        }
//    }


//    public interface IAuthenticationService
//    {
//        Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken);
//    }

//    public class AuthenticationService : IAuthenticationService
//    {
//        public async Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
//        {
//            try
//            {
//                // Your authentication logic here.
//                // If authentication succeeds, no need to return anything, as it's a fire-and-forget operation.
//            }
//            catch (Exception ex)
//            {
//                // Handle authentication failure or exceptions here.
//                // You should still return a Task to fulfill the asynchronous contract.
//                // For example, you can throw an exception or log the error.
//                throw new AuthenticationException("Authentication failed.", ex);
//            }
//        }
//        public interface ISmtpClient
//        {
//            Task ConnectAsync(string host, int port, SecureSocketOptions options, CancellationToken cancellationToken);
//            Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken);
//            Task SendAsync(MimeMessage message, CancellationToken cancellationToken);
//            Task DisconnectAsync(bool quit, CancellationToken cancellationToken);
//        }

//    }


//    //[Fact]
//    //public async Task Run_InvalidRequest_ReturnsBadRequestResult()
//    //{
//    //    // Arrange
//    //    var loggerMock = new Mock<ILogger>();
//    //    var httpRequestMock = new DefaultHttpRequest(new DefaultHttpContext());
//    //    var emailModel = new EmailModel(); // Invalid request

//    //    var azureBlobServiceMock = new Mock<AzureBlobService>();
//    //    var smtpClientMock = new Mock<ISmtpClient>();

//    //    var function = new SendEmailFunction(loggerMock.Object, azureBlobServiceMock.Object, smtpClientMock.Object);

//    //    // Act
//    //    var result = await function.Run(httpRequestMock, emailModel);

//    //    // Assert
//    //    Assert.IsType<BadRequestResult>(result);
//    //}
//}
