//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// StockInType
    /// 入库记录表
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
    public class StockInType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 入库仓库Id
        /// </summary>
        public virtual int WId { get; set; }

        /// <summary>
        /// 入库仓库
        /// </summary>
        public virtual string WName { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        public virtual String InType { get; set; }

        /// <summary>
        /// 留言
        /// </summary>
        public virtual String Memo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 原有库存
        /// </summary>
        public virtual int SourceQty { get; set; }

        /// <summary>
        /// 入库人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
