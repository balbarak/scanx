using ScanX.App.Helpers;
using ScanX.App.Models;
using ScanX.App.Views;
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

        private Media _selectedMedia;
        
        public Media SelectedMedia
        {
            get
            {
                return _selectedMedia;
            }
            set
            {
                if (_selectedMedia != value)
                {
                    _selectedMedia = value;

                    RaisePropertyChanged();
                }
            }
        }

        private ScanSetting.DPI _selectedDpi = ScanSetting.DPI.DPI_72;

        public ScanSetting.DPI SelectedDpi
        {
            get { return _selectedDpi; }
            set { _selectedDpi = value; RaisePropertyChanged(); }
        }


        private ScanSetting.ColorModel _selectedColorMode = ScanSetting.ColorModel.Color;

        public ScanSetting.ColorModel SelectedColorMode
        {
            get { return _selectedColorMode; }
            set { _selectedColorMode = value; RaisePropertyChanged(); }
        }
        
        public IEnumerable<ScanSetting.DPI> Dpi
        {
            get
            {
                return Enum.GetValues(typeof(ScanSetting.DPI))
                    .Cast<ScanSetting.DPI>();
            }
        }

        public IEnumerable<ScanSetting.ColorModel> ColorMode
        {
            get
            {
                return Enum.GetValues(typeof(ScanSetting.ColorModel))
                    .Cast<ScanSetting.ColorModel>();
            }
        }

        public ObservableCollection<Media> Media { get; set; } = new ObservableCollection<Media>();
        
        public ICommand ListPrintersCommand { get; private set; }

        public ICommand ListScannersCommand { get; private set; }

        public ICommand ScanCommand { get; private set; }

        public ICommand ScanMultipleCommand { get; private set; }

        public ICommand ShowDeviceWindowCommand { get; private set; }

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
            ShowDeviceWindowCommand = new RelayCommand(async (arg) => { await ShowDeviceWindow(); });
            ScanMultipleCommand = new RelayCommand(async (arg) => { await ScanMultiple(); });
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

        private async Task ShowDeviceWindow()
        {
            DeviceWindow window = new DeviceWindow();
            window.Show();
            await Task.CompletedTask;
        }

        private async Task Scan()
        {
            if (SelectedDevice == null)
            {
                MessageBox.Show("Please select a scannr from scanner list", "Select scanner", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                return;
            }

            ScanSetting setting = new ScanSetting()
            {
                Color = SelectedColorMode,
                Dpi = SelectedDpi
            };

            Service.ScanSinglePage(SelectedDevice.DeviceId,setting);

            await Task.CompletedTask;
        }

        private async Task ScanMultiple()
        {
            if (SelectedDevice == null)
            {
                MessageBox.Show("Please select a scannr from scanner list", "Select scanner", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

                return;
            }

            ScanSetting setting = new ScanSetting()
            {
                Color = SelectedColorMode,
                Dpi = SelectedDpi
            };

            await Task.Run(()=> { Service.ScanMultiple(SelectedDevice.DeviceId, setting); });

            await Task.CompletedTask;
        }

        private async void OnImageScanned(object sender, EventArgs e)
        {
            var args = e as DeviceImageScannedEventArgs;
            
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {

                var media = new Media()
                {
                    Size = args.ImageBytes.Length,
                    Page = args.Page,
                    Source = await ImageConverter.ConvertToImageSource(args.ImageBytes)
                };



                Media.Add(media);
            }), System.Windows.Threading.DispatcherPriority.Normal);

            
        }
    }
}
