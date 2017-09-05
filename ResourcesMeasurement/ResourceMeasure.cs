using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ResourcesMeasurement
{
    public class ResourceMeasure
    {
        private ManagementObjectSearcher _cpus;
        private ManagementObjectSearcher _ram;

        public ResourceMeasure()
        {
            _cpus = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor");
            _ram = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

        }

        public List<CpuInformation> MeasureCpu()
        {
            var result =  _cpus.Get()
                .Cast<ManagementObject>()
                .Select(mo => new CpuInformation
                {
                    Name = mo["Name"].ToString(),
                    Usage = mo["PercentProcessorTime"].ToString()
                })
               .ToList();
            return result;
        }

        public RamInformation MeasureRam()
        {
            var result = _ram.Get()
                .Cast<ManagementObject>()
                .Select(mo => new RamInformation
                {
                    Free = Double.Parse(mo["FreePhysicalMemory"].ToString()) / 1048576,
                    Total = Double.Parse(mo["TotalVisibleMemorySize"].ToString()) / 1048576
                })
               .FirstOrDefault();
            return result;
        }
    }
}
