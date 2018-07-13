using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanX.Core;
using ScanX.Protocol.ViewModels;

namespace ScanX.Protocol.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Devices()
        {
            DeviceViewModel result = new DeviceViewModel();

            DeviceClient client = new DeviceClient();

            result.Printers = client.GetAllPrinters();

            return View(result);
        }

        public IActionResult ScannerSample()
        {
            return View();
        }
    }
}