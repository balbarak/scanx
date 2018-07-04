using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Linq;
using System.Management;
using WIA;
using ScanX.Core.Models;

namespace ScanX.Core
{
    //for more info https://ourcodeworld.com/articles/read/382/creating-a-scanning-application-in-winforms-with-csharp
    public class DeviceService : ServiceBase<DeviceService>
    {
        public List<string> GetAllPrinters()
        {
            List<string> result = new List<string>();

            var printers = PrinterSettings.InstalledPrinters;

            foreach (string item in printers)
            {
                result.Add(item);
            }

            return result;
            
        }

        public List<ScannerDevice> GetAllScanners()
        {
            var result = new List<ScannerDevice>();

            var deviceManager = new DeviceManager();
            var devices = deviceManager.DeviceInfos;

            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].Type == WiaDeviceType.ScannerDeviceType)
                {
                    result.Add(new ScannerDevice()
                    {
                        Name = devices[i].Properties["Name"].get_Value().ToString(),
                        Description = devices[i].Properties["Description"]?.get_Value()?.ToString(),
                        Port = devices[i].Properties["Port"]?.get_Value()?.ToString()
                    });
                }
            }

            return result;
        }
    }
}
