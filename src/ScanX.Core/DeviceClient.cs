using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Linq;
using System.Management;
using WIA;
using ScanX.Core.Models;
using System.Runtime.InteropServices;
using ScanX.Core.Args;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using ScanX.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace ScanX.Core
{
    //for more info https://ourcodeworld.com/articles/read/382/creating-a-scanning-application-in-winforms-with-csharp
    public class DeviceClient
    {
        public const uint WIA_ERROR_PAPER_EMPTY = 0x80210003;
        public const uint WIA_ERROR_COVER_OPEN = 0x80210016;
        public const uint WIA_ERROR_DEVICE_COMMUNICATION = 0x8021000A;
        public const uint WIA_ERROR_DEVICE_LOCKED = 0x8021000D;

        public object WIA_IPS_BRIGHTNESS { get; private set; }
        
        public event EventHandler OnImageScanned;

        private readonly ILogger _logger;

        private readonly DeviceManager _deviceManager;

        public DeviceClient()
        {
            _deviceManager = new DeviceManager();
        }

        public DeviceClient(ILogger logger) : this()
        {
            _logger = logger;
        }

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

            var deviceInfos = _deviceManager.DeviceInfos;

            for (int i = 0; i < deviceInfos.Count; i++)
            {
                var info = deviceInfos[i + 1];

                if (info.Type == WiaDeviceType.ScannerDeviceType)
                {
                    result.Add(new ScannerDevice()
                    {
                        DeviceId = info.DeviceID,
                        Name = info.Properties["Name"].get_Value().ToString(),
                        Description = info.Properties["Description"]?.get_Value()?.ToString(),
                        Port = info.Properties["Port"]?.get_Value()?.ToString()
                    });
                }
            }

            return result;
        }

        public void ScanMultiple(string deviceID, ScanSetting setting = null)
        {
            var deviceManager = new DeviceManager();

            if (setting == null)
                setting = new ScanSetting();

            IDeviceInfo device = GetDeviceById(deviceID);

            var connectedDevice = device.Connect();

            SetDeviceSettings(connectedDevice, setting);

            int page = 1;

            do
            {
                try
                {
                    page = ScanImage(connectedDevice, page, setting);
                }
                catch (COMException ex) when ((uint)ex.HResult == WIA_ERROR_PAPER_EMPTY)
                {
                    if (page == 1)
                        throw new ScanXException("No paper inserted", ScanXExceptionCodes.NoPaper);
                }
            }
            while (true);

        }

        public void ScanSinglePage(string deviceID, ScanSetting setting = null)
        {
            if (setting == null)
                setting = new ScanSetting();

            int page = 1;

            IDeviceInfo device = GetDeviceById(deviceID);
            Device connectedDevice = null;

            try
            {
                connectedDevice = device.Connect();
                
                SetDeviceSettings(connectedDevice, setting);

                page = ScanImage(connectedDevice, page, setting);
            }
            catch (COMException ex) when ((uint)ex.HResult == WIA_ERROR_PAPER_EMPTY)
            {
                if (page == 1)
                    throw new ScanXException("No paper inserted", ScanXExceptionCodes.NoPaper);
            }
            catch (Exception ex)
            {
                throw new ScanXException($"Error: {ex.ToString()}", ex);
            }
            finally
            {
                if (device != null)
                    Marshal.ReleaseComObject(device);

                if (connectedDevice != null)
                    Marshal.ReleaseComObject(connectedDevice);
            }


        }

        private int ScanImage(Device connectedDevice, int page, ScanSetting setting)
        {
            var img = (ImageFile)connectedDevice.Items[1].Transfer(FormatID.wiaFormatJPEG);

            byte[] data = (byte[])img.FileData.get_BinaryData();

            byte[] dataConverted = null;

            dataConverted = CompressImageBytes(data);

            var args = new DeviceImageScannedEventArgs(dataConverted, img.FileExtension, page)
            {
                Height = img.Height,
                Width = img.Width,
                Settings = setting
            };

            OnImageScanned?.Invoke(this, args);

            page++;
            return page;
        }

        private byte[] CompressImageBytes(byte[] data)
        {
            byte[] dataConverted;
            using (MemoryStream writeMs = new MemoryStream())
            using (MemoryStream ms = new MemoryStream(data))
            {
                Bitmap bit = new Bitmap(ms);
                bit.Save(writeMs, ImageFormat.Jpeg);
                dataConverted = writeMs.ToArray();
            }

            return dataConverted;
        }

        public void ScanWithUI(int deviceID)
        {
            CommonDialogClass dlg = new CommonDialogClass();


        }

        public List<DeviceProperty> GetDeviceProperties(string id)
        {
            List<DeviceProperty> result = new List<DeviceProperty>();

            IDeviceInfo device = GetDeviceById(id);

            foreach (IProperty item in device.Properties)
            {
                result.Add(new DeviceProperty()
                {
                    Id = item.PropertyID,
                    Name = item.Name,
                    Value = item.get_Value()
                });
            }

            return result;
        }

        public List<DeviceProperty> GetDeviceConnectProperties(string id)
        {
            List<DeviceProperty> result = new List<DeviceProperty>();

            IDeviceInfo device = GetDeviceById(id);

            var connectedDevice = device.Connect();

            foreach (IProperty item in connectedDevice.Properties)
            {
                result.Add(new DeviceProperty()
                {
                    Id = item.PropertyID,
                    Name = item.Name,
                    Value = item.get_Value()
                });
            }

            result = result.OrderBy(a => a.Name).ToList();

            return result;
        }

        public List<DeviceProperty> GetItemDeviceConnectProperties(string id)
        {
            List<DeviceProperty> result = new List<DeviceProperty>();

            IDeviceInfo device = GetDeviceById(id);

            var connectedDevice = device.Connect();

            foreach (IProperty item in connectedDevice.Items[1].Properties)
            {
                result.Add(new DeviceProperty()
                {
                    Id = item.PropertyID,
                    Name = item.Name,
                    Value = item.get_Value()
                });
            }

            return result;
        }

        private IDeviceInfo GetDeviceById(string deviceID)
        {
            if (string.IsNullOrWhiteSpace(deviceID))
                throw new ScanXException("Please select a scanner device", ScanXExceptionCodes.NoDevice);

            var deviceManager = new DeviceManager();

            var count = deviceManager.DeviceInfos.Count;

            for (int i = 0; i < count; i++)
            {
                IDeviceInfo device = deviceManager.DeviceInfos[i + 1];

                if (device.DeviceID == deviceID)
                {
                    return device;
                }
            }

            throw new ScanXException($"No scanner device named: {deviceID} found", ScanXExceptionCodes.NoDevice);
        }

        private void SetDeviceSettings(Device connectedDevice, ScanSetting setting)
        {

            var (width, height) = ScanSetting.GetA4SizeByDpi((int)setting.Dpi);

            var resoultions = ScanSetting.GetResolution(setting.Dpi);

            var properties = connectedDevice.Items[1].Properties;

            SetWIAProperty(properties, ScanSetting.WIA_PAGE_SIZE, 5865);

            //SetWIAProperty(properties, ScanSetting.WIA_HORIZONTAL_EXTENT, width);

            //SetWIAProperty(properties, ScanSetting.WIA_VERTICAL_EXTENT, height);
            
            SetWIAProperty(properties, ScanSetting.WIA_HORIZONTAL_RESOLUTION, resoultions);

            SetWIAProperty(properties, ScanSetting.WIA_VERTICAL_RESOLUTION, resoultions);

            SetWIAProperty(properties, ScanSetting.WIA_COLOR_MODE, (int)setting.Color);

        }

        private void SetWIAProperty(IProperties properties, int propertyId, object value)
        {
            try
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    var index = i + 1;

                    if (properties[index].PropertyID.Equals(propertyId))
                    {
                        properties[index].set_Value(value);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = $"unable to set properties: {ex}";

                _logger?.LogWarning(msg);

                Debug.WriteLine(msg);
            }

        }

    }
}
