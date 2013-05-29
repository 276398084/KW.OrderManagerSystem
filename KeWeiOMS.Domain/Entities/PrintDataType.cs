//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountEmailType
    /// 账户邮件
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
    public class PrintDataType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        public virtual string Content { get; set; }

        public virtual DateTime CreateOn { get; set; }

    }
}
