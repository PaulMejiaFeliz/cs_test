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
        private const int BYTES_TO_GIGA = 1048576;

        public ResourceMeasure()
        {
            _cpus = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor");
            _ram = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");

        }

        //Gets the PC CPU cores stats and retrieves them in a List of CpuInformation
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


        //Gets the PC ram stats and retireves them in a RamInformation object
        public RamInformation MeasureRam()
        {
            var result = _ram.Get()
                .Cast<ManagementObject>()
                .Select(managementObject => new RamInformation
                {
                    Free = Double.Parse(managementObject["FreePhysicalMemory"].ToString()) / BYTES_TO_GIGA,
                    Total = Double.Parse(managementObject["TotalVisibleMemorySize"].ToString()) / BYTES_TO_GIGA
                })
               .FirstOrDefault();
            return result;
        }
    }
}
