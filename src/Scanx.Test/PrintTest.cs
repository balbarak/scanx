using ScanX.Core;
using ScanX.Core.Models;
using System;
using System.IO;
using Xunit;

namespace Scanx.Test
{
    public class PrintTest
    {
        private readonly IPrinterClient _client;


        public PrintTest(IPrinterClient client)
        {
            _client = client;
        }

        [Fact]
        public void Should_Print_With_Settings()
        {
            var filePath = @"C:\Users\balba\Pictures\Cards\2.jpg";
            var imgData = File.ReadAllBytes(filePath);


            _client.Print(imgData, new PrintSettings("Microsoft Print to PDF"));
        }
    }
}
