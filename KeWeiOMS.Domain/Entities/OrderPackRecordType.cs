//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderPackRecordType
    /// 包装记录表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0  创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name></name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class OrderPackRecordType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 包装人
        /// </summary>
        public virtual String PackBy { get; set; }

        /// <summary>
        /// 扫描人
        /// </summary>
        public virtual String ScanBy { get; set; }

        /// <summary>
        /// 包装时间
        /// </summary>
        public virtual DateTime PackOn { get; set; }

        /// <summary>
        /// 包装系数
        /// </summary>
        public virtual double PackCoefficient { get; set; }

        /// <summary>
        /// 包装系数
        /// </summary>
        public virtual string SKU { get; set; }
    }
}
