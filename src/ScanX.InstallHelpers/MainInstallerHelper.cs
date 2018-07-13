using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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
            Debugger.Launch();

            base.Install(stateSaver);

            string path = this.Context.Parameters["targetdir"];

            File.Create($"{path}\\FuckOff.txt");

        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            
            base.OnAfterInstall(savedState);
        }
    }
}
