using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class OrderData
    {
        public string OrderNo { get; set; }

        public string Status { get; set; }

        public string OrderExNo { get; set; }


        public string OrderType { get; set; }

        public double OrderAmount { get; set; }

        public string CurrencyCode { get; set; }

        public double TotalCost { get; set; }

        public double RMB { get; set; }

        public double Weight { get; set; }

        public string TrackCode { get; set; }

        public string LogisticMode { get; set; }

        public double Freight { get; set; }

        public string Country { get; set; }

        public DateTime SendOn { get; set; }

        public string Platform { get; set; }

        public string Account { get; set; }

        public string Remark { get; set; }
    }
}