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
    public class OrderAmountType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 拆分
        /// </summary>
        public virtual int IsSplit { get; set; }

        /// <summary>
        /// 重发
        /// </summary>
        public virtual int IsRepeat { get; set; }


        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// 主ID
        /// </summary>
        public virtual int MId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 订单平台ID
        /// </summary>
        public virtual String OrderExNo { get; set; }

        /// <summary>
        /// 重发次数
        /// </summary>
        public virtual int AgainCount { get; set; }

        /// <summary>
        /// 拆分次数
        /// </summary>
        public virtual int SplitCount { get; set; }

        /// <summary>
        /// 总运费
        /// </summary>
        public virtual double TotalFreight { get; set; }

        /// <summary>
        /// 总产品陈本
        /// </summary>
        public virtual double TotalCosts { get; set; }

        /// <summary>
        /// 订单手续费
        /// </summary>
        public virtual double Fee { get; set; }

        /// <summary>
        /// 订单交易费
        /// </summary>
        public virtual double TransactionFees { get; set; }

        /// <summary>
        /// 订单其他费
        /// </summary>
        public virtual double OtherFees { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public virtual double OrderAmount { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public virtual String CurrencyCode { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public virtual double ExchangeRate { get; set; }

        /// <summary>
        /// RMB
        /// </summary>
        public virtual double RMB { get; set; }

        /// <summary>
        /// RMB
        /// </summary>
        public virtual double Profit { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public virtual String Country { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual String Platform { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
