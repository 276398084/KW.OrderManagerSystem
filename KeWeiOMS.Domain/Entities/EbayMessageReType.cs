//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EbayMessageReType
    /// Ebay邮件回复
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
    public class EbayMessageReType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 回复消息Id
        /// </summary>
        public virtual int MessageId { get; set; }

        /// <summary>
        /// 回复主题
        /// </summary>
        public virtual String SubjectRe { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public virtual String BodyRe { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public virtual DateTime ReplayOn { get; set; }

        /// <summary>
        /// 回复人
        /// </summary>
        public virtual String ReplayBy { get; set; }

    }
}
