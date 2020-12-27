using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.IO.Compression;

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

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            string path = this.Context.Parameters["targetdir"];
            string servicePath = $"{path}\\Protocol\\ScanX.Protocol.exe";
            
            //ServiceHelper.InstallService(servicePath);

            //ServiceHelper.StartService();
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            //ServiceHelper.StopService();
            
            base.OnBeforeUninstall(savedState);
        }

        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);

            //ServiceHelper.DeleteService();

            string path = this.Context.Parameters["targetdir"];
            var protocolDir = $"{path}\\Protocol";

            //if (Directory.Exists(protocolDir))
            //    Directory.Delete(protocolDir,true);
            
        }
    }
}
