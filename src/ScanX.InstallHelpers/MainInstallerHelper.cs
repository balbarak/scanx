using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
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
            base.OnBeforeInstall(savedState);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            string path = GetPath();

            string servicePath = Path.Combine(path,"ScanX.Protocol.exe");
            
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
        }

        private string GetPath()
        {
            var result = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return result;
        }
    }
}
