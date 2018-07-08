using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScanX.App.Models
{
    public class Media
    {
        public ImageSource Source { get; set; }

        public int Size { get; set; }

        public int Page { get; set; }


        public string SizeIn
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };

                double len = (double)Size;

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
}
