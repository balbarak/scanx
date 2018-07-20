using Microsoft.AspNetCore.SignalR;
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

        public async Task Scan()
        {
            var data = File.ReadAllBytes("wwwroot\\images\\1.png");

            var imageBase64 = Convert.ToBase64String(data);

            await Clients.Caller.SendAsync("ImageScanned", imageBase64);

            await Task.CompletedTask;
        }
    }
}
