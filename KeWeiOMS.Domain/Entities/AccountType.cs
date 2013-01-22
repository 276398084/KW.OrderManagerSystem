//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// AccountType
    /// 平台账户表
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
    public class AccountType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public virtual String AccountName { get; set; }

        /// <summary>
        /// 平台网址
        /// </summary>
        public virtual String AccountUrl { get; set; }

        /// <summary>
        /// APIKey
        /// </summary>
        public virtual String ApiKey { get; set; }

        /// <summary>
        /// API密钥
        /// </summary>
        public virtual String ApiSecret { get; set; }

        /// <summary>
        /// API会话
        /// </summary>
        public virtual String ApiToken { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual String Platform { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual String Description { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public virtual String Manager { get; set; }

        /// <summary>
        /// 负责人电话
        /// </summary>
        public virtual String Phone { get; set; }

        /// <summary>
        /// 负责人邮箱
        /// </summary>
        public virtual String Email { get; set; }

    }
}
