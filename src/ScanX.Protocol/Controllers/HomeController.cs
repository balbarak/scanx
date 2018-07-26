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
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Devices()
        {
            DeviceViewModel result = new DeviceViewModel();

            using (DeviceClient client = new DeviceClient())
            {
                result.Printers = client.GetAllPrinters();
                result.Scanners = client.GetAllScanners();
            }

            return View(result);
        }

        public IActionResult ScannerSample()
        {
            List<ScannerDevice> result = new List<ScannerDevice>();

            using (DeviceClient client = new DeviceClient())
            {
                result = client.GetAllScanners();
            }
            
            return View(result);
        }

        public IActionResult Viewerjs()
        {
            List<ScannerDevice> result = new List<ScannerDevice>();

            using (DeviceClient client = new DeviceClient())
            {
                result = client.GetAllScanners();
            }

            return View(result);
        }

        public IActionResult CropperJs()
        {
            List<ScannerDevice> result = new List<ScannerDevice>();

            using (DeviceClient client = new DeviceClient())
            {
                result = client.GetAllScanners();
            }

            return View(result);
        }
    }
}