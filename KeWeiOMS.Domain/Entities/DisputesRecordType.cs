//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// DisputesRecordType
    /// 纠纷处理记录
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
    public class DisputesRecordType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 纠纷Id
        /// </summary>
        public virtual int DId { get; set; }

        /// <summary>
        /// 处理类型
        /// </summary>
        public virtual String DealType { get; set; }

        /// <summary>
        /// 处理内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
