using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class ProductData
    {
        public string SKU { get; set; }

        public int Qty { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }

        public int OQty { get; set; }

        public string PicUrl { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string Standard { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get; set; }

        public string Remark { get; set; }
    }
}