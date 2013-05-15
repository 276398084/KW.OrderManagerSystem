using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class DisputeCount
    {
        /// <summary>
        /// 包装人
        /// </summary>
        public virtual string DType { get; set; }

        /// <summary>
        /// 包装分数
        /// </summary>
        public virtual decimal Count { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public virtual decimal Qcount { get; set; }

    }
}