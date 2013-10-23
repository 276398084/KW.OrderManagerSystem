using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web.Common
{
    public class KPIData
    {
        public string PeopleName { get; set; }

        public double PeiPoint { get; set; }

        public double ValiPoint { get; set; }

        public double PackPoint { get; set; }

        public double ScanPoint { get; set; }

        public double SotckInPoint { get; set; }

        public double ReturnPoint { get; set; }

        public double AddPoint { get; set; }

        public double TotalPoint
        {
            get
            {
                return Math.Round(PeiPoint + ValiPoint + PackPoint + ScanPoint + SotckInPoint + ReturnPoint + AddPoint, 0);

            }
        }

    }
}