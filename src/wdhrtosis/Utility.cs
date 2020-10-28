using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace wdhrtosis
{
    public static class Utility
    {
        public static bool LogMemoryUsage { get; set; }

        public static double GetMemoryUsage()
        {
            double memory = 0;
            Process proc = Process.GetCurrentProcess();
            memory = Math.Round(proc.PrivateMemorySize64 / 1e+6, 2);
            proc.Dispose();
            return memory;
        }
    }
}
