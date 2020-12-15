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
            var filePath = @"C:\Users\balbarak\Pictures\Material Icons\test.PNG";
            var imgData = File.ReadAllBytes(filePath);

            var settings = new PrintSettings("Zebra")
            {
                Width = 300,
                Height = 200,
                Margin = new PageMargin()
                {
                    Top = 15,
                }
            };

            _client.Print(imgData, settings);
        }
    }
}
