//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderWeightRecordType
    /// 订单重量扫描结果
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 XiDong 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>XiDong</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class OrderWeightRecordType
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// OId
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// OrderExNo
        /// </summary>
        public virtual String OrderExNo { get; set; }

        /// <summary>
        /// MaxWeight
        /// </summary>
        public virtual int MaxWeight { get; set; }

        /// <summary>
        /// MinWeight
        /// </summary>
        public virtual int MinWeight { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// CreateOn
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
