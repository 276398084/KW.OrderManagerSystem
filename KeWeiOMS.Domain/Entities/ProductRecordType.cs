//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// ProductRecordType
    /// 商品日志
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
    public class ProductRecordType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// oid
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// OldSKU
        /// </summary>
        public virtual String OldSKU { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual String RecordType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
