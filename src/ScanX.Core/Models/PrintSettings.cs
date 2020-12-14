using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Models
{
    public class PrintSettings
    {
        public string PrinterName { get; set; }

        public int Height { get; set; } = 200;

        public int Width { get; set; } = 300;

        public PageMargin Margin { get; set; } = new PageMargin();

        public PrintSettings()
        {

        }

        public PrintSettings(string printerName)
        {
            PrinterName = printerName;
        }

    }
}
