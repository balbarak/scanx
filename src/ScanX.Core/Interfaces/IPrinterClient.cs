using ScanX.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;

namespace ScanX.Core
{
    public interface IPrinterClient
    {
        string GetDefaultPrinter();
        
        void Print(byte[] imageToPrint, PrintSettings settings);
    }
}
