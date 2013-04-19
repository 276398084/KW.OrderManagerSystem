//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// NoStockType
    /// 无库存商品
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
    public class NoStockType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 父SKU
        /// </summary>
        public virtual String OldSKU { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public virtual String PicUrl { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual String Standard { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public virtual  int Enabled{ get; set; }

        /// <summary>
        /// rows
        /// </summary>
        public virtual string rows { get; set; }

        /// <summary>
        /// rows
        /// </summary>
        public virtual string rowse { get; set; }
    }
}
