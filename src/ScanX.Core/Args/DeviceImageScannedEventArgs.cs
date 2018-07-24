using ScanX.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ScanX.Core.Args
{
    public class DeviceImageScannedEventArgs : EventArgs
    {
        public byte[] ImageBytes { get; set; }

        public string Extension { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Page { get; set; }

        public string Size { get; private set; }

        public ScanSetting Settings { get; set; }

        public DeviceImageScannedEventArgs()
        {

        }

        public DeviceImageScannedEventArgs(byte[] data, string ext, int page)
        {
            this.ImageBytes = data;
            this.Extension = ext;
            this.Page = page;

            Size = CalculateSize();
        }

        private string CalculateSize()
        {
            if (ImageBytes == null)
                return "";

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            double len = (double)ImageBytes.Length;

            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }
        
    }
}
