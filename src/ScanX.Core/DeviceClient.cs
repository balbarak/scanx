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

namespace ScanX.Core
{
    //for more info https://ourcodeworld.com/articles/read/382/creating-a-scanning-application-in-winforms-with-csharp
    public class DeviceClient
    {
        public const uint WIA_ERROR_PAPER_EMPTY = 0x80210003;
        
        public object WIA_IPS_BRIGHTNESS { get; private set; }

        public event EventHandler OnTransferCompleted;
        public event EventHandler OnImageScanned;

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

            foreach (IDeviceInfo info in new DeviceManagerClass().DeviceInfos)
            {
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

        public void Scan(int deviceID)
        {
            var deviceManager = new DeviceManager();

            try
            {
                var device = deviceManager.DeviceInfos[deviceID];

                var connectedDevice = device.Connect();

                int page = 1;

                do
                {
                    try
                    {
                        var img = (ImageFile)connectedDevice.Items[1].Transfer(FormatID.wiaFormatJPEG);

                        byte[] data = (byte[])img.FileData.get_BinaryData();

                        OnImageScanned?.Invoke(this, new DeviceImageScannedEventArgs(data, img.FileExtension, page));

                        page++;
                    }
                    catch (COMException ex)
                    {
                        if ((uint)ex.HResult != WIA_ERROR_PAPER_EMPTY)
                        {
                            OnTransferCompleted?.Invoke(this, new DeviceTransferCompletedEventArgs(page));
                            break;
                        }


                        throw;
                    }
                }
                while (true);



            }
            catch (Exception ex)
            {

            }
        }

        public void ScanSinglePage(string deviceID,ScanSetting setting = null)
        {
            if (setting == null)
                setting = new ScanSetting();
            

            IDeviceInfo device = GetDeviceById(deviceID);
            
            if (device == null)
                throw new Exception($"Unable to find device id: {deviceID}");

            var connectedDevice = device.Connect();

            SetDeviceSettings(connectedDevice, setting);

            int page = 1;

            try
            {
                var img = (ImageFile)connectedDevice.Items[1].Transfer(FormatID.wiaFormatJPEG);

                byte[] data = (byte[])img.FileData.get_BinaryData();

                OnImageScanned?.Invoke(this, new DeviceImageScannedEventArgs(data, img.FileExtension, page));

                page++;
            }
            catch (COMException ex) when( (uint) ex.HResult == WIA_ERROR_PAPER_EMPTY)
            {
                Debug.WriteLine(ex);
            }


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
            IDeviceInfo device = null;

            foreach (IDeviceInfo info in new DeviceManagerClass().DeviceInfos)
            {
                if (info.DeviceID == deviceID)
                {
                    device = info;
                    break;
                }
            }

            return device;
        }

        private void SetDeviceSettings(Device connectedDevice,ScanSetting setting)
        {
            var pageSize = ScanSetting.GetA4SizeByDpi(setting.Dpi);
            var resoultions = ScanSetting.GetResolution(setting.Dpi);

            var properties = connectedDevice.Items[1].Properties;

            SetWIAProperty(properties, ScanSetting.WIA_HORIZONTAL_RESOLUTION, resoultions);
            SetWIAProperty(properties, ScanSetting.WIA_VERTICAL_RESOLUTION, resoultions);


            SetWIAProperty(properties, ScanSetting.WIA_HORIZONTAL_EXTENT, pageSize.width);
            SetWIAProperty(properties, ScanSetting.WIA_VERTICAL_EXTENT, pageSize.height);
            
            SetWIAProperty(properties, ScanSetting.WIA_COLOR_MODE, setting.Color);


        }

        private void SetWIAProperty(IProperties properties, int propertyId, object value)
        {
            foreach (IProperty item in properties)
            {
                if (item.PropertyID.Equals(propertyId))
                {
                    item.set_Value(value);
                }

            }
        }
        
    }
}
