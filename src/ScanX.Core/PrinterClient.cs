using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;

namespace ScanX.Core
{
    public class PrinterClient : IDisposable , IPrinterClient
    {
        private readonly PrinterSettings _printerSettings;
        private readonly ILogger<PrinterClient> _logger;
        private readonly PrintDocument _printer;

        public PrinterClient(ILogger<PrinterClient> logger)
        {
            _printerSettings = new PrinterSettings();
            _printer = new PrintDocument();
            _logger = logger;
        }

        public string GetDefaultPrinter()
        {
            return _printerSettings.PrinterName;
        }

        public void Print(byte[] dataToPrint,string printerName)
        {
            _printer.PrinterSettings = new PrinterSettings()
            {
                PrinterName = printerName
            };

            _printer.DocumentName = "test";
            _printer.PrintPage += OnPrintingPage;
            _printer.Print();
        }

        public void Print(byte[] dataToPrint,PrinterSettings settings)
        {
            _printer.PrinterSettings = settings;

            _printer.DocumentName = "test";
            _printer.PrintPage += OnPrintingPage;
            _printer.Print();
        }

        private void OnPrintingPage(object sender, PrintPageEventArgs e)
        {
            var filePath = @"C:\Users\balbarak\Desktop\test.html";

            //e.Graphics.DrawImage();
        }

        public void Dispose()
        {

        }
    }
}
