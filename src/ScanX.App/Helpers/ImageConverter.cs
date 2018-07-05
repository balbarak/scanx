using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScanX.App.Helpers
{
    public class ImageConverter
    {
        public static async Task<ImageSource> ConvertToImageSource(byte[] data)
        {
            BitmapImage result = new BitmapImage();
            
            using (MemoryStream ms = new MemoryStream())
            {
                var buffer = data;

                await ms.WriteAsync(buffer,0,buffer.Length);

                ms.Position = 0;

                result.StreamSource = ms;
            }

            return result;

        }
    }
}
