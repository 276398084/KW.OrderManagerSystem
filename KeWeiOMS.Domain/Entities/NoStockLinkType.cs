//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// NoStockLinkType
    /// 无库存链接
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
    public class NoStockLinkType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public virtual int PId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// OldSKU
        /// </summary>
        public virtual String OldSKU { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual String Supplier { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        public virtual String Url { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual double QPrice { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public virtual double Freight { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual String Adr { get; set; }

        /// <summary>
        /// 平均到货时间
        /// </summary>
        public virtual String Received { get; set; }

    }
}
