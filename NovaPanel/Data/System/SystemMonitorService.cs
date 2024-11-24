using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace NovaPanel.Data.SystemAPI
{
    public class SystemMonitorService : IDisposable
    {
        #region Head
        public double CpuUsage { get; private set; }
        public (double used, double total) RamUsage { get; private set; } = (0, 0);
        public double RamUsagePercent => Math.Round((RamUsage.total > 0 ? (RamUsage.used / RamUsage.total) * 100 : 0), 3);

        public double DiskUsage { get; private set; }
        public List<DiskUsageInfo> DiskUsages { get; private set; } = new();

        private Timer updateTimer; // s
        private Timer hourlyTimer; // h

        public List<UsageTimeItem> CpuUsageHistory { get; private set; } = new();
        public List<UsageTimeItem> RamUsageHistory { get; private set; } = new();

        public event Action? OnDataUpdated;
        #endregion

        public SystemMonitorService()
        {
            // s
            updateTimer = new Timer(1000);
            updateTimer.Elapsed += UpdateUsage;
            updateTimer.Start();

            // h
            hourlyTimer = new Timer(3600 * 1000);
            hourlyTimer.Elapsed += RecordHourlyUsage;
            hourlyTimer.Start();
            Console.WriteLine("计时器开始");
        }

        private void UpdateUsage(object sender, ElapsedEventArgs e)
        {
            try
            {
                // CPU
                string cpuInfo = SystemUsage.GetCpuInfoString();
                if (!string.IsNullOrEmpty(cpuInfo) && cpuInfo.Contains('/'))
                {
                    var cpuParts = cpuInfo.Split('/');
                    if (double.TryParse(cpuParts[0], out double cpuUsed) &&
                        double.TryParse(cpuParts[1], out double cpuTotal) && cpuTotal > 0)
                    {
                        CpuUsage = Math.Round((cpuUsed / cpuTotal) * 100, 2);
                    }
                }
                
                // RAM
                string ramInfo = SystemUsage.GetRamInfo();
                if (!string.IsNullOrEmpty(ramInfo) && ramInfo.Contains('/'))
                {
                    var ramParts = ramInfo.Split('/');
                    if (double.TryParse(ramParts[0], out double ramUsed) &&
                        double.TryParse(ramParts[1], out double ramTotal) && ramTotal > 0)
                    {
                        RamUsage = (
                            Math.Round(ramUsed / (1024 * 1024 * 1024), 2),
                            Math.Round(ramTotal / (1024 * 1024 * 1024), 2)
                        );
                    }
                }

                Console.Title = $"CPU[{CpuUsage}%]  RAM[{RamUsage.used}GB / {RamUsage.total}GB | {Math.Round(RamUsagePercent, 3)}%]";

                // DISK
                var allDisks = SystemUsage.GetDiskUsage()
                    .Select(d => new DiskUsageInfo
                    {
                        drive = d.drive,
                        used = Math.Round(d.used, 2),
                        total = Math.Round(d.total, 2),
                        usagePercent = d.total > 0 ? Math.Round((d.used / d.total) * 100, 2) : 0
                    })
                    .ToList();

                double totalUsed = allDisks.Sum(d => d.used);
                double totalCapacity = allDisks.Sum(d => d.total);
                DiskUsage = totalCapacity > 0 ? Math.Round((totalUsed / totalCapacity) * 100, 2) : 0;

                DiskUsages = allDisks;

                // UI
                OnDataUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUsage: {ex.Message}");
            }
            
        }

        private void RecordHourlyUsage(object sender, ElapsedEventArgs e)
        {
            try
            {
                CpuUsageHistory.Add(new UsageTimeItem(DateTime.Now, CpuUsage));
                RamUsageHistory.Add(new UsageTimeItem(DateTime.Now, RamUsagePercent));

                // 144 value
                if (CpuUsageHistory.Count > 144) CpuUsageHistory.RemoveAt(0);
                if (RamUsageHistory.Count > 144) RamUsageHistory.RemoveAt(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RecordHourlyUsage: {ex.Message}");
            }
        }

        public void Dispose()
        {
            updateTimer?.Dispose();
            hourlyTimer?.Dispose();
        }

        public class DiskUsageInfo
        {
            public string drive { get; set; } = string.Empty;
            public double used { get; set; }
            public double total { get; set; }
            public double usagePercent { get; set; }
        }
    }

}
