using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class AmountCount
    {
        /// <summary>
        ///账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Count { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public virtual decimal Qcount { get; set; }

    }
}