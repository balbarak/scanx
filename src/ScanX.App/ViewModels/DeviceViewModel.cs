using ScanX.App.Helpers;
using ScanX.App.Views;
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
    public class DeviceViewModel : BaseViewModel
    {
        public ObservableCollection<ScannerDevice> Scanners { get; set; } = new ObservableCollection<ScannerDevice>();

        public ObservableCollection<DeviceProperty> Properties { get; set; } = new ObservableCollection<DeviceProperty>();

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

        public ICommand ListScannersCommand { get; private set; }

        public ICommand ListDevicePropertiesCommand { get; private set; }
        
        public DeviceViewModel()
        {
            SetCommands();
        }

        private void SetCommands()
        {
            ListScannersCommand = new RelayCommand(async (arg) => { await ListScanners(); });

            ListDevicePropertiesCommand = new RelayCommand(async (arg) => { await ListDeviceProperties(); });

            


        }

        private async Task ListDeviceProperties()
        {
            var client = new DeviceClient();

            Properties.Clear();

            var result = client.GetItemDeviceConnectProperties(SelectedDevice.DeviceId);

            foreach (var item in result)
            {
                Properties.Add(item);
            }
        }
        
        private async Task ListScanners()
        {
            Scanners.Clear();

            var client = new DeviceClient();

            var result = client.GetAllScanners();

            foreach (var item in result)
            {
                Scanners.Add(item);
            }

            await Task.CompletedTask;
        }

    }
}
