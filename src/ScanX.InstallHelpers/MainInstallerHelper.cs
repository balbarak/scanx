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

            string path = this.Context.Parameters["targetdir"];

            File.Create($"{path}\\FuckOff.txt");

        }
        protected override void OnAfterInstall(IDictionary savedState)
        {
            MessageBox.Show("Debug Me");

            base.OnAfterInstall(savedState);
        }
    }
}
