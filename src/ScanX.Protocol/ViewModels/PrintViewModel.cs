using ScanX.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanX.Protocol.ViewModels
{
    public class PrintViewModel
    {
        public string PrinterName { get; set; }

        public int Height { get; set; } = 200;

        public int Width { get; set; } = 300;

        public int MarginTop { get; set; }

        public int MarginBottom { get; set; }

        public int MarginRight { get; set; }

        public int MarginLeft { get; set; }

        public PrintSettings ToModel()
        {
            return new PrintSettings()
            {
                PrinterName = PrinterName,
                Height = Height,
                Width = Width,
                Margin = new PageMargin()
                {
                    Bottom = MarginBottom,
                    Top = MarginTop,
                    Left = MarginLeft,
                    Right = MarginRight
                }
            };
        }
    }
}
