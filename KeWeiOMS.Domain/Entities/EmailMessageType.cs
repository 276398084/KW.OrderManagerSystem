//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailMessageType
    /// 邮件留言
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
    public class EmailMessageType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 邮件编号
        /// </summary>
        public virtual String MessageId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public virtual String OrderExNo { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public virtual DateTime RserverDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
