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
    public class AccountEmailType
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 账户ID
        /// </summary>
        public virtual int AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public virtual String AccountName { get; set; }

        /// <summary>
        /// 邮箱名称
        /// </summary>
        public virtual String Email { get; set; }

    }
}
