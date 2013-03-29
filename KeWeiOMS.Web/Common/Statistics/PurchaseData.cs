using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    /// <summary>
    /// 采购数据（预采购和预警采购数据载体）
    /// </summary>
    public class PurchaseData
    {
        public string SKU { get; set; }

        public string ItemName { get; set; }

        public string SPic { get; set; }

        public int IsImportant { get; set; }

        public int NowQty { get; set; }

        public int WarningQty { get; set; }
        /// <summary>
        /// 已经购买了的数量
        /// </summary>
        public int BuyQty { get; set; }

        public int PreQty { get; set; }

        public int SevenDay { get; set; }

        public int FifteenDay { get; set; }

        public int ThirtyDay { get; set; }

        public int NeedQty { get; set; }

        public double AvgQty { get; set; }

        public int DayByStock { get; set; }
    }
}