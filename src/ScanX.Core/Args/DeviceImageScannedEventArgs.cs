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
        public byte[] ImageRawData { get; set; }

        public string Extension { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Page { get; set; }

        public ScanSetting Settings { get; set; }

        public Image BitmapImage { get; set; }

        public DeviceImageScannedEventArgs()
        {

        }

        public DeviceImageScannedEventArgs(byte[] data, string ext, int page)
        {
            this.ImageRawData = data;
            this.Extension = ext;
            this.Page = page;
        }

        public byte[] GetBitmapBinary()
        {
            byte[] result = null;

            if (BitmapImage == null)
            {
                MemoryStream ms = new MemoryStream(ImageRawData);

                BitmapImage = Image.FromStream(ms);

                var size = ScanSetting.GetA4SizeByDpi(Settings.Dpi);
                var res = ScanSetting.GetResolution(Settings.Dpi);

                //BitmapImage.SetResolution(res, res);
                
                
            }

            
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapImage.Save(ms, ImageFormat.Jpeg);

                result = ms.ToArray();
            }


            return result;
        }

    }
}
