using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    /// <summary>
    /// 比例分析
    /// </summary>
    public class ProportionData
    {
        public string Key { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        public decimal Proportion { get; set; }

        public int Count { get; set; }

        public int TotalCount { get; set; }
    }
}