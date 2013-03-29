using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class QueCount
    {
        public string SKU { get; set; }

        /// <summary>
        /// 总订单数
        /// </summary>
        public int Qty { get; set; }

        public string Standard { get; set; }

        public DateTime MinDate { get; set; }

        public int BuyQty { get; set; }

        public int NeedQty { get; set; }


    }
}