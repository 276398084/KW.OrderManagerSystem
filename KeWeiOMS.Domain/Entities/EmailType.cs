//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// EmailType
    /// 邮件
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
    public class EmailType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual String Content { get; set; }

        /// <summary>
        /// 买家ID
        /// </summary>
        public virtual String BuyerCode { get; set; }

        /// <summary>
        /// 买家Email
        /// </summary>
        public virtual String BuyerEmail { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public virtual String MessageId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public virtual String MessageType { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 产品SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 产品标题
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 产品链接
        /// </summary>
        public virtual String ProductUrl { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public virtual float ProductPrice { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual String Platform { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 回复人员
        /// </summary>
        public virtual String ReplyBy { get; set; }

        /// <summary>
        /// 是否回复
        /// </summary>
        public virtual int IsReply { get; set; }

        /// <summary>
        /// 邮件生成时间
        /// </summary>
        public virtual DateTime GenerateOn { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public virtual DateTime ReplyOn { get; set; }

    }
}
