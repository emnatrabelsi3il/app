using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace PR3_AgentService
{
    public static class AgentInfoCollector
    {
        public static AgentHeartbeatDto Collect()
        {
            var ram = GetRamInfo();
            var disk = GetDiskInfo();

            return new AgentHeartbeatDto
            {
                Numero = Environment.MachineName,
                NomMachine = Environment.MachineName,
                MacAdress = GetMacAddress(),
                AdresseIP = GetLocalIpAddress(),
                OsVersion = RuntimeInformation.OSDescription,
                RamDisponibleMb = ram.freeMb,
                RamTotaleMb = ram.totalMb,
                DisqueTotalGb = disk.totalGb,
                DisqueLibreGb = disk.freeGb
            };
        }

        private static string GetMacAddress()
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(n =>
                    n.OperationalStatus == OperationalStatus.Up &&
                    n.NetworkInterfaceType != NetworkInterfaceType.Loopback);

            return nic?.GetPhysicalAddress().ToString() ?? "UNKNOWN";
        }

        private static string GetLocalIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                var ip = host.AddressList
                    .FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);

                return ip?.ToString() ?? "UNKNOWN";
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        private static (double totalMb, double freeMb) GetRamInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

                foreach (ManagementObject obj in searcher.Get())
                {
                    double totalKb = Convert.ToDouble(obj["TotalVisibleMemorySize"]);
                    double freeKb = Convert.ToDouble(obj["FreePhysicalMemory"]);

                    return (
                        totalMb: Math.Round(totalKb / 1024, 2),
                        freeMb: Math.Round(freeKb / 1024, 2)
                    );
                }
            }
            catch
            {
                // Erreur WMI ignorée pour éviter de bloquer le service
            }

            return (0, 0);
        }

        private static (double totalGb, double freeGb) GetDiskInfo()
        {
            double total = 0;
            double free = 0;

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                {
                    total += drive.TotalSize;
                    free += drive.TotalFreeSpace;
                }
            }

            return (
                totalGb: Math.Round(total / 1024 / 1024 / 1024, 2),
                freeGb: Math.Round(free / 1024 / 1024 / 1024, 2)
            );
        }
    }
}