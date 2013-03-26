//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// GMarketType
    /// GMarket
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
    public class GMarketType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual String ItemId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String ItemTitle { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public virtual String PicUrl { get; set; }

        /// <summary>
        /// 数量标题
        /// </summary>
        public virtual String NowNum { get; set; }

        /// <summary>
        /// 现在数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 产品链接
        /// </summary>
        public virtual String ProductUrl { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual String Account { get; set; }

    }
}
