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
        
        public const int WIA_VERTICAL_EXTENT = 6152;

        public const int WIA_VERTICAL_RESOLUTION = 6148;

        public const int WIA_HORIZONTAL_RESOLUTION = 6147;

        public const int WIA_PAGE_WIDTH = 3098;

        public const int WIA_PAGE_HEIGHT = 3099;

        public enum DPI
        {
            DPI_75 = 1,
            DPI_96 = 2,
            DPI_150 = 3,
            DPI_300 = 4,
        }

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

        public DPI Dpi { get; set; }

        public ColorModel Color { get; set; }


        //for more info https://www.papersizes.org/a-sizes-in-pixels.htm
        public static (int width,int height) GetA4SizeByDpi(DPI dpi)
        {
            switch (dpi)
            {
                case DPI.DPI_75:

                    return (619, 876);

                case DPI.DPI_96:

                    return (794, 1123);

                case DPI.DPI_150:

                    return (1240, 1754);

                case DPI.DPI_300:

                    return (2480, 3508);

                default:

                    return (794,1123);
            }
        }

        public static int GetResolution(DPI dpi)
        {
            switch (dpi)
            {
                case DPI.DPI_75:

                    return 75;

                case DPI.DPI_96:

                    return 96;

                case DPI.DPI_150:

                    return 150;

                case DPI.DPI_300:

                    return 300;

                default:
                    return 150;
            }
        }
    }
}
