using ScanX.App.Helpers;
using ScanX.App.Models;
using ScanX.Core;
using ScanX.Core.Args;
using ScanX.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScanX.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ScannerDevice _selectedDevice;

        public ScannerDevice SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                if (_selectedDevice != value)
                {
                    _selectedDevice = value;

                    RaisePropertyChanged();
                }
            }
        }

        private DeviceClient _service;

        public DeviceClient Service
        {
            get
            {
                if (_service == null)
                    _service = new DeviceClient();

                return _service;
            }
            set { _service = value; }
        }


        public ObservableCollection<string> Printers { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<ScannerDevice> Scanners { get; set; } = new ObservableCollection<ScannerDevice>();

        public ObservableCollection<Media> Media { get; set; } = new ObservableCollection<Media>();


        public ICommand ListPrintersCommand { get; private set; }

        public ICommand ListScannersCommand { get; private set; }

        public ICommand ScanCommand { get; private set; }

        public MainViewModel()
        {
            SetCommands();
            Service.OnImageScanned += OnImageScanned;
        }

        

        private void SetCommands()
        {
            ListPrintersCommand = new RelayCommand(async (arg) =>{ await ListPrinters(); });
            ListScannersCommand = new RelayCommand(async (arg) => { await ListScanners(); });
            ScanCommand = new RelayCommand(async (arg) => { await Scan(); });
        }

        private async Task ListScanners()
        {
            Scanners.Clear();

            var result = Service.GetAllScanners();

            foreach (var item in result)
            {
                Scanners.Add(item);
            }

            await Task.CompletedTask;
        }

        private async Task ListPrinters()
        {
            Printers.Clear();

            var result = Service.GetAllPrinters();

            foreach (var item in result)
            {
                Printers.Add(item);
            }

            await Task.CompletedTask;
        }

        private async Task Scan()
        {
            if (SelectedDevice == null)
            {
                MessageBox.Show("Please select a scannr from scanner list", "Select scanner", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                return;
            }

            Service.ScanSinglePage(SelectedDevice.DeviceId);

            await Task.CompletedTask;
        }

        private async void OnImageScanned(object sender, EventArgs e)
        {
            var args = e as DeviceImageScannedEventArgs;

            var media = new Media()
            {
                Size = args.ImageData.Length,
                Page = args.Page,
                Source = await ImageConverter.ConvertToImageSource(args.ImageData)
            };

            //File.WriteAllBytes($@"C:\Users\b.albarrak\Documents\SampleImages\{args.Page}{args.Extension}", args.ImageData);

            Media.Add(media);
            
        }
    }
}
