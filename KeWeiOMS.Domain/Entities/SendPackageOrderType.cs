//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountFeeType
    /// 平台账户费用表
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
    public class SendPackageOrderType
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 包裹ID
        /// </summary>
        public virtual int PackId { get; set; }

        /// <summary>
        /// 包裹名称
        /// </summary>
        public virtual string PackageName { get; set; }
    }
}
