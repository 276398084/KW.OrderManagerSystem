//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// WarehouseStockType
    /// 仓库库存表
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
    public class WarehouseStockType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public virtual int WId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public virtual String Warehouse { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public virtual int PId { get; set; }

        /// <summary>
        /// 商品SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateOn { get; set; }

    }
}
