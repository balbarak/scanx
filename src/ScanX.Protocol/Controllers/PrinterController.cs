﻿using System;
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

        [HttpPost]
        public IActionResult Post([FromBody] PrintRequest doc)
        {
            Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "docLocation", doc.location }
            };

            using (DeviceClient client = new DeviceClient())
            {
                client.Print(doc.location);
                result["printer"] = client.GetDefualtPrinter();
            }
            return Ok(result);
        }


        [Route("print")]
        public IActionResult Print()
        {


            return Ok("doc printeds");
        }
    }
}