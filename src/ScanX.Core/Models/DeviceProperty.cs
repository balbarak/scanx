using System;
using System.Collections.Generic;
using System.Text;

namespace ScanX.Core.Models
{
    public class DeviceProperty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public object Value { get; set; }
    }
}
