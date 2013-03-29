//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PurchaseTroubleType
    /// 采购问题表
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
    public class PurchaseTroubleType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 问题状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 采购计划Id
        /// </summary>
        public virtual int PurchaseId { get; set; }

        /// <summary>
        /// 采购计划编号
        /// </summary>
        public virtual String PurchaseCode { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public virtual double Freight { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual String Supplier { get; set; }

        /// <summary>
        /// 物流方式
        /// </summary>
        public virtual String LogisticsMode { get; set; }

        /// <summary>
        /// 快递单
        /// </summary>
        public virtual String LogisticsCode { get; set; }

        /// <summary>
        /// 购买时间
        /// </summary>
        public virtual DateTime BuyOn { get; set; }

        /// <summary>
        /// 到货时间
        /// </summary>
        public virtual DateTime ReceiveOn { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public virtual String TroubleType { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public virtual String TroubleDetail { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public virtual String DealBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public virtual DateTime DealOn { get; set; }

        /// <summary>
        /// 解决方式
        /// </summary>
        public virtual String SolutionType { get; set; }

        /// <summary>
        /// 解决描述
        /// </summary>
        public virtual String SolutionDetail { get; set; }

    }
}
