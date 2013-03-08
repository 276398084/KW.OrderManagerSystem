using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class OrderCount
    {
        public string Account { get; set; }

        public string Platform { get; set; }
        /// <summary>
        /// 总订单数
        /// </summary>
        public int OCount { get; set; }
    }
}