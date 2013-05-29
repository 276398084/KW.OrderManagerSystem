//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderAmountType
    /// 订单金额表
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
    public class OrderOutRecordType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 拆分
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// 重发
        /// </summary>
        public virtual string OrderNo { get; set; }


        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual string OrderExNo { get; set; }

        /// <summary>
        /// 主ID
        /// </summary>
        public virtual string CreateBy { get; set; }


        /// <summary>
        /// 主ID
        /// </summary>
        public virtual DateTime CreateOn { get; set; }
        /// <summary>
        /// 是否恢复
        /// </summary>
        public virtual int IsRestoration { get; set; }


        /// <summary>
        /// Restoration时间
        /// </summary>
        public virtual DateTime RestorationOn { get; set; }
        /// <summary>
        /// Restoration时间
        /// </summary>
        public virtual String RestorationBy { get; set; }

        /// <summary>
        /// Restoration时间
        /// </summary>
        public virtual String Remark { get; set; }
    }
}
