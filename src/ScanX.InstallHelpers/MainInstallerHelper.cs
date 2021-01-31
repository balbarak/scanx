using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ScanX.InstallHelpers
{
    [RunInstaller(true)]
    public partial class MainInstallerHelper : System.Configuration.Install.Installer
    {
        public MainInstallerHelper()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            CleanUp();

            base.OnBeforeInstall(savedState);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            var zipPath = Path.Combine(GetPath(), "Scanx.zip");

            var protocolFolderPath = GetProtocolFolderPath();

            ZipFile.ExtractToDirectory(zipPath, protocolFolderPath);


            string servicePath = Path.Combine(protocolFolderPath, "ScanX.Protocol.exe");
            
            ServiceHelper.InstallService(servicePath);
            ServiceHelper.StartService();
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {

            ServiceHelper.StopService();

            base.OnBeforeUninstall(savedState);
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);

            
            ServiceHelper.DeleteService();

            try
            {
                var path = GetProtocolFolderPath();

                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception)
            {

            }
        }

        private string GetPath()
        {
            var result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return result;
        }
        private string GetProtocolFolderPath()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var result = Path.Combine(baseDir, "Protocol");

            return result;
        }

        private void CleanUp()
        {
            try
            {
                ServiceHelper.StopService();
                ServiceHelper.DeleteService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
