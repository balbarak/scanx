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

            var imageBase64 = Convert.ToBase64String(data);

            await Clients.Caller.SendAsync(ClientMethod.IMAGE_SCANNED, imageBase64);

            await Task.CompletedTask;
        }

        public async Task ScanSingle()
        {
            var data = File.ReadAllBytes("wwwroot\\images\\1.png");

            var imageBase64 = Convert.ToBase64String(data);

            await Clients.Caller.SendAsync(ClientMethod.IMAGE_SCANNED, imageBase64);

            await Task.CompletedTask;
        }

        public async Task ScanMultiple()
        {
            DeviceClient client = new DeviceClient();

            client.OnImageScanned += async (sender, args) =>
            {
                var data = args as DeviceImageScannedEventArgs;
                
                var imageBase64 = Convert.ToBase64String(data.ImageData);

                await Clients.Caller.SendAsync("ImageScanned", imageBase64);
            };

            client.ScanMultiple("{6BDD1FC6-810F-11D0-BEC7-08002BE2092F}\\0000");
            
            await Task.CompletedTask;
        }
    }
}
