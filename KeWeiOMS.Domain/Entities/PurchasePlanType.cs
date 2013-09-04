//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PurchasePlanType
    /// 采购计划表
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
    public class PurchasePlanType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 计划编号
        /// </summary>
        public virtual String PlanNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 到货数量
        /// </summary>
        public virtual int DaoQty { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public virtual double Freight { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String ProductName { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual String Suppliers { get; set; }

        /// <summary>
        /// 产品链接
        /// </summary>
        public virtual String ProductUrl { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public virtual String PicUrl { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public virtual int SId { get; set; }

        /// <summary>
        /// 发货方式
        /// </summary>
        public virtual String LogisticsMode { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public virtual String TrackCode { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public virtual String PostStatus { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 采购人
        /// </summary>
        public virtual String BuyBy { get; set; }

        /// <summary>
        /// 生成人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

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
        public virtual DateTime ReceiveOn { get; set; }


        /// <summary>
        /// 到货时间
        /// </summary>
        public virtual DateTime ExpectReceiveOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual String Memo { get; set; }

    }
}
