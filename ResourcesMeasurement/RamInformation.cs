using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcesMeasurement
{
    public class RamInformation
    {
        private double _free;
        private double _total;

        public double Free
        {
            get
            {
                return _free;
            }
            set
            {
                _free = Math.Round(value, 2);
            }
        }

        public double Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = Math.Round(value, 2);
            }
        }

        public double Used
        {
            get
            {
                return Math.Round((Total - Free) / Total * 100, 2);
            }
        }
    }
}
