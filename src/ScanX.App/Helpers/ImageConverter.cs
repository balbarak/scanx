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

            result.BeginInit();
            result.CreateOptions = BitmapCreateOptions.None;
            result.CacheOption = BitmapCacheOption.Default;
            result.StreamSource = new MemoryStream(data);
            result.EndInit();

            await Task.CompletedTask;

            return result;

        }
    }
}
