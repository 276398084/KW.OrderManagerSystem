//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PlanDaoType
    /// 采购到货表
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
    public class PlanDaoType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 审核
        /// </summary>
        public virtual int IsAudit { get; set; }

        /// <summary>
        /// string
        /// </summary>
        public virtual int WId { get; set; }
        /// <summary>
        /// 藏仓库
        /// </summary>
        public virtual string WName { get; set; }

        /// <summary>
        /// 采购计划编号
        /// </summary>
        public virtual String PlanNo { get; set; }

        public virtual int PlanId { get; set; }

        /// <summary>
        /// 采购SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 采购时间
        /// </summary>
        public virtual DateTime BuyOn { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public virtual DateTime SendOn { get; set; }

        /// <summary>
        /// 到货时间
        /// </summary>
        public virtual DateTime DaoOn { get; set; }

        /// <summary>
        /// 开始SKU
        /// </summary>
        public virtual int SKUCode { get; set; }

        /// <summary>
        /// 物流
        /// </summary>
        public virtual String LogisticMode { get; set; }

        /// <summary>
        /// 追踪号
        /// </summary>
        public virtual String TrackCode { get; set; }

        /// <summary>
        /// 计划中采购数量
        /// </summary>
        public virtual int PlanQty { get; set; }

        /// <summary>
        /// 实际到货数量
        /// </summary>
        public virtual int RealQty { get; set; }

        /// <summary>
        /// 供货商
        /// </summary>
        public virtual String Suppliers { get; set; }

        /// <summary>
        /// 产品金额
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 到货状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 清点人
        /// </summary>
        public virtual String CheckBy { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public virtual double PassRate { get; set; }

        /// <summary>
        /// 产品检验人
        /// </summary>
        public virtual String ValiBy { get; set; }

        /// <summary>
        /// 采购备注
        /// </summary>
        public virtual String PlanMemo { get; set; }

        /// <summary>
        /// 到货备注
        /// </summary>
        public virtual String Memo { get; set; }

    }
}
