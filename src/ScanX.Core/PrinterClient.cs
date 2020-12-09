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

        public PrinterClient(ILogger<PrinterClient> logger)
        {
            _printerSettings = new PrinterSettings();
            _logger = logger;
        }

        public string GetDefaultPrinter()
        {
            return _printerSettings.PrinterName;
        }

        public void Dispose()
        {

        }
    }
}
