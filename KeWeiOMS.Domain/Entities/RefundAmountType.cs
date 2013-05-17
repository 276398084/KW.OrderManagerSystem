//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// RefundAmountType
    /// 退款金额表
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
    public class RefundAmountType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 纠纷Id
        /// </summary>
        public virtual int DId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 平台订单号
        /// </summary>
        public virtual String OrderExNo { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public virtual double Amount { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual String Platform { get; set; }

        /// <summary>
        /// 帐号
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// 退款方式
        /// </summary>
        public virtual String AmountType { get; set; }

        /// <summary>
        /// 买家账户
        /// </summary>
        public virtual String EmailAccount { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public virtual String TransactionNo { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public virtual String AuditBy { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public virtual DateTime AuditOn { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

    }
}
