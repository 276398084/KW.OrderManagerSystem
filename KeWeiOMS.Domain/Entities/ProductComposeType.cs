//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductComposeType
    /// 组合产品标记表
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
    public class ProductComposeType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 组合产品ID
        /// </summary>
        public virtual int PId { get; set; }

        /// <summary>
        /// 组合产品SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 被组合产品ID
        /// </summary>
        public virtual int SrcPId { get; set; }

        /// <summary>
        /// 被组合产品SKU
        /// </summary>
        public virtual String SrcSKU { get; set; }

        /// <summary>
        /// 被组合产品数量
        /// </summary>
        public virtual int SrcQty { get; set; }

    }
}
