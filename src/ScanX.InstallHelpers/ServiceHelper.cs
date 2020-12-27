using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanX.InstallHelpers
{
    public class ServiceHelper
    {
        private static string _serviceName = "ScanXFu";

        public static void InstallService(string servicePath)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo startupInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C sc create {_serviceName} binPath=\"{servicePath}\" start=auto",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                process.StartInfo = startupInfo;

                process.Start();

                process.WaitForExit();

            }
        }

        public static void StartService()
        {
            using (Process process = new Process())
            {

                ProcessStartInfo startupInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C sc start {_serviceName}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                process.StartInfo = startupInfo;

                process.Start();

                process.WaitForExit();
            }
        }

        public static void StopService()
        {
            using (Process process = new Process())
            {

                ProcessStartInfo startupInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C sc stop {_serviceName}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                process.StartInfo = startupInfo;

                process.Start();

                process.WaitForExit();

            }
        }

        public static void DeleteService()
        {
            using (Process process = new Process())
            {
                ProcessStartInfo startupInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c sc delete \"{_serviceName}\"",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                process.StartInfo = startupInfo;

                process.Start();

                process.WaitForExit();

            }
        }
    }
}
