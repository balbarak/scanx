using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;

namespace ScanX.Core
{
    public interface IPrinterClient
    {
        string GetDefaultPrinter();
        void Print(byte[] dataToPrint, string printerName);
        void Print(byte[] dataToPrint, PrinterSettings settings);
    }
}
