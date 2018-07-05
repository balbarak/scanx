using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Args
{
    public class DeviceImageScannedEventArgs : EventArgs
    {
        public byte[] ImageData { get; set; }

        public string Extension { get; set; }

        public int Page { get; set; }

        public DeviceImageScannedEventArgs()
        {

        }

        public DeviceImageScannedEventArgs(byte[] data,string ext,int page)
        {
            this.ImageData = data;
            this.Extension = ext;
            this.Page = page;
        }

    }
}
