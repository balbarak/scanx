using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
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

        public void Print(byte[] imageToPrint,string printerName)
        {
            _printer.PrinterSettings = new PrinterSettings()
            {
                PrinterName = printerName
            };

            _printer.DocumentName = "test";
            _printer.PrinterSettings.CreateMeasurementGraphics();

            _printer.PrintPage += OnPrintingPage;
            _printer.Print();
        }

        public void Print(byte[] imageToPrint,PrinterSettings settings)
        {
            _printer.PrinterSettings = settings;

            _printer.DocumentName = "test";
            _printer.PrintPage += OnPrintingPage;
            _printer.Print();
        }

        private void OnPrintingPage(object sender, PrintPageEventArgs e)
        {
            var filePath = @"C:\Users\balbarak\Pictures\Material Icons\test.png";

            //var data = File.ReadAllBytes(filePath);

            var img = System.Drawing.Image.FromFile(filePath);

            var sizes = _printer.PrinterSettings.PaperSizes;

            var graphic = _printer.PrinterSettings.CreateMeasurementGraphics();

            graphic.PageUnit = System.Drawing.GraphicsUnit.Display;

            var clip = graphic.ClipBounds;

            e.Graphics.DrawImage(img,0,0,300,200);
        }

        public void Dispose()
        {

        }
    }
}
