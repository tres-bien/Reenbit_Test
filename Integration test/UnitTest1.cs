using Newtonsoft.Json;
using System.Text;
using Moq;
using FunctionApp1;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MimeKit;
using Reenbit_Test.Services;
using Reenbit_Test.Shared;
using Reenbit_Test;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs.Models;

namespace Integration_test
{
    public class UnitTest1
    {
        //[Fact]
        //public async Task Run_ValidRequest_ReturnsOkResult()
        //{
        //    // Arrange
        //    var smtpClientMock = new Mock<SmtpClient>();
        //    var loggerMock = new Mock<ILogger<EmailSenderFunction>>();
        //    var azureBlobServiceMock = new Mock<IAzureBlobService>();

        //    var emailSenderFunction = new EmailSenderFunction(smtpClientMock.Object, loggerMock.Object);
        //    var emailModel = new EmailModel
        //    {
        //        To = "recipient@example.com",
        //        Subject = "Test Email",
        //    };

        //    var requestBody = JsonConvert.SerializeObject(emailModel);
        //    var req = CreateTestHttpRequestData(requestBody);

        //    // Act
        //    var result = await emailSenderFunction.Run(req, Mock.Of<FunctionContext>());

        //    // Assert
        //    Assert.IsType<OkResult>(result);

        //    smtpClientMock.Verify(smtp => smtp.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), default));
        //    smtpClientMock.Verify(smtp => smtp.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), default));
        //    smtpClientMock.Verify(smtp => smtp.SendAsync(It.IsAny<MimeMessage>(), default, default));
        //    smtpClientMock.Verify(smtp => smtp.DisconnectAsync(true, default));
        //}

        public class BlobInfo
        {
            public string Name { get; set; }
            public Uri Uri { get; set; }
        }


        [Fact]
        public async Task Run_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var httpRequestMock = new DefaultHttpRequest(new DefaultHttpContext());
            var emailModel = new EmailModel
            {
                To = "test@example.com",
                Subject = "Test Subject",
            };

            var mockBlob1 = new BlobInfo
            {
                Name = "file1.txt",
                Uri = new Uri("https://yourstorageaccount.blob.core.windows.net/container/file1.txt"),
            };

            var azureBlobServiceMock = new Mock<AzureBlobService>();
            azureBlobServiceMock.Setup(s => s.ListAsync()).ReturnsAsync(mockBlob1);

            var smtpClientMock = new Mock<ISmtpClient>();
            smtpClientMock.Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>()))
                .Returns(Task.CompletedTask);
            smtpClientMock.Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            smtpClientMock.Setup(s => s.SendAsync(It.IsAny<MimeMessage>()))
                .Returns(Task.CompletedTask);
            smtpClientMock.Setup(s => s.DisconnectAsync(It.IsAny<bool>()))
                .Returns(Task.CompletedTask);

            var function = new SendEmailFunction(loggerMock.Object, azureBlobServiceMock.Object, smtpClientMock.Object);

            // Act
            var result = await function.Run(httpRequestMock, emailModel);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Run_InvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var httpRequestMock = new DefaultHttpRequest(new DefaultHttpContext());
            var emailModel = new EmailModel(); // Invalid request

            var azureBlobServiceMock = new Mock<AzureBlobService>();
            var smtpClientMock = new Mock<ISmtpClient>();

            var function = new SendEmailFunction(loggerMock.Object, azureBlobServiceMock.Object, smtpClientMock.Object);

            // Act
            var result = await function.Run(httpRequestMock, emailModel);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}