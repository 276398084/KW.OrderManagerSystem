//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EbayType
    /// Ebay
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
    public class EbayType
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
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String ItemTitle { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public virtual String Currency { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual String Price { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public virtual String PicUrl { get; set; }

        /// <summary>
        /// 初始数量
        /// </summary>
        public virtual int StartNum { get; set; }

        /// <summary>
        /// 现在数量
        /// </summary>
        public virtual int NowNum { get; set; }

        /// <summary>
        /// 未配货数量
        /// </summary>
        public virtual int UnPeiQty { get; set; }



        /// <summary>
        /// 产品链接
        /// </summary>
        public virtual String ProductUrl { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual String Status { get; set; }

    }
}
