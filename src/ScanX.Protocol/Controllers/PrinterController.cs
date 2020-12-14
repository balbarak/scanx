using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanX.Core;
using ScanX.Core.Models;
using ScanX.Protocol.ViewModels;

namespace ScanX.Protocol.Controllers
{
    public class PrinterController : ApiBaseController
    {
        private readonly IPrinterClient _client;

        public PrinterController(IPrinterClient printerClient)
        {
            _client = printerClient;
        }

        public IActionResult Get()
        {
            List<string> result = new List<string>();

            using (DeviceClient client = new DeviceClient())
            {
                result = client.GetAllPrinters();
            }
            return Ok(result);
        }

        [Route("default")]
        public IActionResult GetDefaultPrinter()
        {
            var printerName = _client.GetDefaultPrinter();

            return Ok(printerName);
        }

        [Route("print")]
        [HttpPost]
        public IActionResult Print()
        {
            _client.Print(null,new PrintSettings("Zebra"));

            return Ok("doc printeds");
        }
    }
}