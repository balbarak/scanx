using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Models
{
    public class DeviceSetting
    {
        public const int WIA_PAGE_SIZE = 3097;

        public const int WIA_COLOR_MODE = 6146;

        public const int WIA_HORIZONTAL_EXTENT = 6151;

        public const int WIA_HORIZONTAL_RESOLUTION = 6148;

        public const int WIA_VERTICAL_EXTENT = 6152;

        public const int WIA_VERTICAL_RESOLUTION = 6148;

        public enum ColorModel
        {
            Grayscale = 2,
            Color = 1,
            Mask = 3
        }

        public enum PageSize
        {
            A4 = 0,
            Letter = 1,
        }
    }
}
