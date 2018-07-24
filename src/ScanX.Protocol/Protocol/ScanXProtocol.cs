using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ScanX.Core;
using ScanX.Core.Args;
using ScanX.Core.Exceptions;
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
        private readonly ILogger _logger;

        public ScanXProtocol(ILogger<ScanXProtocol> logger)
        {
            _logger = logger;
        }

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
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                await Clients.Caller.SendAsync(ClientMethod.ERROR, "Please select a scanner device first");

                return;
            }

            DeviceClient client = new DeviceClient();

            RegisterImageScannedEvents(client);

            await TryInvoke(() => client.ScanSinglePage(deviceId, settings));
            
            await Task.CompletedTask;
        }
        
        public async Task ScanMultiple(string deviceId,ScanSetting settings)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                await Clients.Caller.SendAsync(ClientMethod.ERROR, "Please select a scanner device first");

                return;
            }

            DeviceClient client = new DeviceClient();

            RegisterImageScannedEvents(client);

            await TryInvoke(() => client.ScanMultiple(deviceId, settings));

            await Task.CompletedTask;
        }
        
        private async Task TryInvoke(Action action)
        {
            try
            {
                action.Invoke();

            }
            catch (ScanXException ex)
            {
                _logger?.LogError(ex.ToString());

                await Clients.Caller.SendAsync(ClientMethod.ERROR, ex);
            }
        }

        private void RegisterImageScannedEvents(DeviceClient client)
        {
            client.OnImageScanned += async (sender, args) =>
            {
                var data = args as DeviceImageScannedEventArgs;

                await Clients.Caller.SendAsync("ImageScanned", data);
            };
        }

    }
}
