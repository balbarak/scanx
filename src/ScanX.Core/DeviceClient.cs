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

namespace ScanX.Core
{
    //for more info https://ourcodeworld.com/articles/read/382/creating-a-scanning-application-in-winforms-with-csharp
    public class DeviceClient
    {
        public const uint WIA_ERROR_PAPER_EMPTY = 0x80210003;

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

            var deviceManager = new DeviceManager();

            var devices = deviceManager.DeviceInfos;

            for (int i = 0; i < devices.Count; i++)
            {
                try
                {
                    if (devices[i].Type == WiaDeviceType.ScannerDeviceType)
                    {
                        result.Add(new ScannerDevice()
                        {
                            Id = i,
                            Name = devices[i].Properties["Name"].get_Value().ToString(),
                            Description = devices[i].Properties["Description"]?.get_Value()?.ToString(),
                            Port = devices[i].Properties["Port"]?.get_Value()?.ToString()
                        });
                    }
                }
                catch (Exception)
                {

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

        public void ScanSinglePage(int deviceID)
        {
            var deviceManager = new DeviceManager();

            try
            {
                var device = deviceManager.DeviceInfos[deviceID];

                var connectedDevice = device.Connect();

                int page = 1;

                var img = (ImageFile)connectedDevice.Items[1].Transfer(FormatID.wiaFormatJPEG);

                byte[] data = (byte[])img.FileData.get_BinaryData();
                
                OnImageScanned?.Invoke(this, new DeviceImageScannedEventArgs(data, img.FileExtension, page));

                page++;

            }
            catch (Exception ex)
            {

            }
        }

        public void ScanWithUI(int deviceID)
        {
            CommonDialogClass dlg = new CommonDialogClass();


        }
    }
}
