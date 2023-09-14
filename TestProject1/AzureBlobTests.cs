using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Reenbit_Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class AzureBlobServiceTests
    {
        private AzureBlobService _azureBlobService;
        private Mock<IFormFile> _formFileMock;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            _azureBlobService = new AzureBlobService();

            // Arrange
            _formFileMock = new Mock<IFormFile>();
            _formFileMock.Setup(f => f.FileName).Returns("testfile.txt");
            _formFileMock.Setup(f => f.OpenReadStream()).Returns(() => new MemoryStream(new byte[] { 1, 2, 3 }));
        }

        [TestMethod]
        public async Task UploadAsync_UploadsFileSuccessfully()
        {
            // Act
            var result = await _azureBlobService.UploadAsync(_formFileMock.Object);

            // Assert
            Assert.IsFalse(result.Error);
            Assert.AreEqual($"File {_formFileMock.Object.FileName} Uploaded Successfully", result.Status);
            Assert.AreEqual(_formFileMock.Object.FileName, result.Blob.Name);
        }
    }
}
