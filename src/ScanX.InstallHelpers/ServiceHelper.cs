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
        private static string _serviceName = "ScanX";

        public static void InstallService(string servicePath)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo startupInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C sc create {_serviceName} binPath=\"{servicePath}\" start=auto"
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
                    Arguments = $"/C sc start {_serviceName}"
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
                    Arguments = $"/C sc stop {_serviceName}"
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
                    Arguments = $"/c sc delete \"{_serviceName}\""
                };

                process.StartInfo = startupInfo;

                process.Start();

                process.WaitForExit();

            }
        }
    }
}
