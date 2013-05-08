﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeWeiOMS.Web
{
    public class Sores
    {
        /// <summary>
        /// 包装人
        /// </summary>
        public virtual string PackBy { get; set; }

        /// <summary>
        /// 包装系数
        /// </summary>
        public virtual decimal PackSores { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public virtual decimal Qcount { get; set; }

        /// <summary>
        /// 平均系数
        /// </summary>
        public virtual decimal Avg { get; set; }
    }
}