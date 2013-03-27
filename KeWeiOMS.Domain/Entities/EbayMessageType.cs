//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EbayMessageType
    /// Ebay消息
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
    public class EbayMessageType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public virtual String MessageId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual String MessageType { get; set; }

        /// <summary>
        /// 问题类型
        /// </summary>
        public virtual String QuestionType { get; set; }

        /// <summary>
        /// 买家邮箱
        /// </summary>
        public virtual String SenderEmail { get; set; }

        /// <summary>
        /// 买家Id
        /// </summary>
        public virtual String SenderID { get; set; }

        /// <summary>
        /// 消息主题
        /// </summary>
        public virtual String Subject { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual String Body { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public virtual String MessageStatus { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public virtual String ItemId { get; set; }

        /// <summary>
        /// 问题创建时间
        /// </summary>
        public virtual DateTime CreationDate { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

    }
}
