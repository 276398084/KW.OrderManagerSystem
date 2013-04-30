using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    /// <summary>
    /// 比例分析
    /// </summary>
    public class LeveDays
    {
        public string Platform { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        public decimal OCount { get; set; }

        public int Account { get; set; }

        public int TotalCount { get; set; }
    }
}