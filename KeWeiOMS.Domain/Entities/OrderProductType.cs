//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// OrderProductType
    /// 订单商品表
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
    public class OrderProductType
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
        /// 订单号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 外部商品SKU
        /// </summary>
        public virtual String ExSKU { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 是否缺货
        /// </summary>
        public virtual int IsQue { get; set; }

        /// <summary>
        /// 对应内部库存SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public virtual String Remark { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        public virtual String Standard { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 产品网址
        /// </summary>
        public virtual String Url { get; set; }

    }
}
