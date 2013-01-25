//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailReturnType
    /// 邮件回复表
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
    public class EmailReturnType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 回复邮箱
        /// </summary>
        public virtual String REmail { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public virtual String Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 我们的邮箱
        /// </summary>
        public virtual String MyEmail { get; set; }

        /// <summary>
        /// 回复的邮件Id
        /// </summary>
        public virtual int EId { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 回复人
        /// </summary>
        public virtual DateTime CreateBy { get; set; }

    }
}
