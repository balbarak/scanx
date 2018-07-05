using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Args
{
    public class DeviceTransferCompletedEventArgs : EventArgs
    {
        public int TotalPages { get; set; }

        public DeviceTransferCompletedEventArgs(int totalPages)
        {
            this.TotalPages = totalPages;
        }
    }
}
