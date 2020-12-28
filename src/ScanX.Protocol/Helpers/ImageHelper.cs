using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScanX.Protocol.Helpers
{
    public class ImageHelper
    {
        public static byte[] FromBase64(string data, out string type)
        {
            byte[] result = null;
            try
            {
                var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                type = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["type"].Value.Replace(";base64", "");
                result = Convert.FromBase64String(base64Data);

                type = type.Insert(0, "image/");
            }
            catch (Exception)
            {
                type = "";
            }

            return result;
        }
    }
}
