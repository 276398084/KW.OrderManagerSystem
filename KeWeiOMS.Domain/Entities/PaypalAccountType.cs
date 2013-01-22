//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , KeWei TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace KeWeiOMS.Domain
{

    /// <summary>
    /// PaypalAccountType
    /// paypal账户
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
    public class PaypalAccountType
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public virtual String AccountName { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public virtual String AppKey { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public virtual String AppPwd { get; set; }

        /// <summary>
        /// 会话
        /// </summary>
        public virtual String AppToken { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }

    }
}
