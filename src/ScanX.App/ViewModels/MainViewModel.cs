using ScanX.App.Helpers;
using ScanX.Core;
using ScanX.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScanX.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<string> Printers { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<ScannerDevice> Scanners { get; set; } = new ObservableCollection<ScannerDevice>();

        public ICommand ListPrintersCommand { get; private set; }

        public ICommand ListScannersCommand { get; private set; }

        public MainViewModel()
        {
            SetCommands();
        }

        private void SetCommands()
        {
            ListPrintersCommand = new RelayCommand(async (arg) =>{ await ListPrinters(); });
            ListScannersCommand = new RelayCommand(async (arg) => { await ListScanners(); });
        }

        private async Task ListScanners()
        {
            Scanners.Clear();

            var result = DeviceService.Instance.GetAllScanners();

            foreach (var item in result)
            {
                Scanners.Add(item);
            }

            await Task.CompletedTask;
        }

        private async Task ListPrinters()
        {
            Printers.Clear();

            var result = DeviceService.Instance.GetAllPrinters();

            foreach (var item in result)
            {
                Printers.Add(item);
            }

            await Task.CompletedTask;
        }
    }
}
