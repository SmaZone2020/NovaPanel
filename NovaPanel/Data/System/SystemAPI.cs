using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

namespace NovaPanel.Data.SystemAPI
{
    public class UsageTimeItem
    {
        public UsageTimeItem(DateTime time, double value)
        {
            Time = time;
            Value = value;
        }

        public DateTime Time { get; set; }
        public double Value { get; set; }
    }
    public class SystemUsage
    {
        // 导入 DLL 中的 GetRamMem 函数
        [DllImport("STM32INFO.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetRamMem();

        // 导入 DLL 中的 GetCpuInfo 函数
        [DllImport("STM32INFO.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetCpuInfo();

        // 导入 DLL 中的 FreeMem 函数
        [DllImport("STM32INFO.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeMem(IntPtr buffer);

        // 获取 RAM 信息
        public static string GetRamInfo()
        {
            IntPtr ramPtr = GetRamMem();
            if (ramPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get RAM info from the DLL.");
            }

            // 将返回的指针转换为字符串
            string ramInfo = Marshal.PtrToStringAnsi(ramPtr) ?? string.Empty;

            // 释放 DLL 分配的内存
            FreeMem(ramPtr);

            return ramInfo;
        }

        // 获取 CPU 信息
        public static string GetCpuInfoString()
        {
            IntPtr cpuPtr = GetCpuInfo();
            if (cpuPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to get CPU info from the DLL.");
            }

            // 将返回的指针转换为字符串
            string cpuInfo = Marshal.PtrToStringAnsi(cpuPtr) ?? string.Empty;

            // 释放 DLL 分配的内存
            FreeMem(cpuPtr);

            return cpuInfo;
        }

        public static List<(string drive, float used, float total)> GetDiskUsage()
        {
            var diskUsage = new List<(string drive, float used, float total)>();

            try
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        float totalSize = drive.TotalSize / (1024f * 1024f * 1024f); // 转为 GB
                        float freeSpace = drive.TotalFreeSpace / (1024f * 1024f * 1024f); // 转为 GB
                        float usedSpace = totalSize - freeSpace;

                        diskUsage.Add((
                            drive.Name.TrimEnd('\\'), // 去掉尾部的反斜杠
                            (float)Math.Round(usedSpace, 2),
                            (float)Math.Round(totalSize, 2)
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving disk usage: {ex.Message}");
            }

            return diskUsage;
        }


    }
}
