//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderPeiRecordType
    /// 配货记录表
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
    public class OrderPeiRecordType
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
        /// 扫描时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 配货人
        /// </summary>
        public virtual String PeiBy { get; set; }

        /// <summary>
        /// 检验人
        /// </summary>
        public virtual String ValiBy { get; set; }

        /// <summary>
        /// 扫描人
        /// </summary>
        public virtual String ScanBy { get; set; }

        /// <summary>
        /// 指定包装人
        /// </summary>
        public virtual String PackBy { get; set; }

    }
}
