using Microsoft.AspNetCore.SignalR;
using ScanX.Core;
using ScanX.Core.Args;
using ScanX.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScanX.Protocol.Protocol
{
    public class ScanXProtocol : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task ScanTest()
        {
            var data = File.ReadAllBytes("wwwroot\\images\\1.png");
            var data2 = File.ReadAllBytes("wwwroot\\images\\2.png");

            var imageBase64 = Convert.ToBase64String(data);
            var imageTwo = Convert.ToBase64String(data2);

            await Clients.Caller.SendAsync(ClientMethod.IMAGE_SCANNED, imageBase64);
            await Clients.Caller.SendAsync(ClientMethod.IMAGE_SCANNED, imageTwo);

            await Task.CompletedTask;
        }

        public async Task ScanSingle(string deviceId,ScanSetting settings)
        {
            DeviceClient client = new DeviceClient();

            client.OnImageScanned += async (sender, args) =>
            {
                var data = args as DeviceImageScannedEventArgs;

                var imageData = data.GetBitmapBinary();

                await Clients.Caller.SendAsync("ImageScanned", imageData);
            };



            client.ScanSinglePage(deviceId, settings);

            await Task.CompletedTask;
        }

        public async Task ScanMultiple(string deviceId,ScanSetting settings)
        {
            DeviceClient client = new DeviceClient();

            client.OnImageScanned += async (sender, args) =>
            {
                var data = args as DeviceImageScannedEventArgs;
                
                var imageBase64 = Convert.ToBase64String(data.ImageRawData);

                await Clients.Caller.SendAsync("ImageScanned", imageBase64);
            };

            
            
            client.ScanMultiple(deviceId,settings);
            
            await Task.CompletedTask;
        }
    }
}
