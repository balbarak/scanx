using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScanX.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.DependencyInjection;

namespace Scanx.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPrinterClient, PrinterClient>();
        }
    }
}
